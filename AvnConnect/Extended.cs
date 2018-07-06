using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AvnConnect
{
    public static class ExtendedFunctions
    {
        public static DateTime GetNetworkTime()
        {
            try
            {
                //default Windows time server
                const string ntpServer = "time.windows.com";

                // NTP message size - 16 bytes of the digest (RFC 2030)
                var ntpData = new byte[48];

                //Setting the Leap Indicator, Version Number and Mode values
                ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;

                //The UDP port number assigned to NTP is 123
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                //NTP uses UDP

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.Connect(ipEndPoint);
                    //Stops code hang if NTP is blocked
                    socket.ReceiveTimeout = 3000;
                    socket.Send(ntpData);
                    socket.Receive(ntpData);
                    socket.Close();
                }

                //Offset to get to the "Transmit Timestamp" field (time at which the reply 
                //departed the server for the client, in 64-bit timestamp format."
                const byte serverReplyTime = 40;

                //Get the seconds part
                ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

                //Get the seconds fraction
                ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

                //Convert From big-endian to little-endian
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

                //**UTC** time
                var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

                return networkDateTime.ToUniversalTime();
            }
            catch (Exception)
            {
                return DateTime.UtcNow;
            }
        }

        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }
    }

    public class MyTextBlock: TextBlock
    {
        public event EventHandler Click;



        public string TextTag
        {
            get { return (string)GetValue(TextTagProperty); }
            set { SetValue(TextTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextTagProperty =
            DependencyProperty.Register("TextTag", typeof(string), typeof(MyTextBlock), new PropertyMetadata(""));


        public MyTextBlock()
        {
            this.Cursor = System.Windows.Input.Cursors.Hand;
            this.MouseEnter += MyTextBlock_MouseEnter;
            this.MouseLeave += MyTextBlock_MouseLeave;
            this.MouseLeftButtonUp += MyTextBlock_MouseLeftButtonUp;
        }

        private void MyTextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount ==1)
            {
                this.Click?.Invoke(this, e);
            }
        }

        private void MyTextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.TextDecorations.Remove(System.Windows.TextDecorations.Underline[0]);
        }

        private void MyTextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.TextDecorations.Add(System.Windows.TextDecorations.Underline[0]);
        }
    }
}
