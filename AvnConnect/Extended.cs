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
using System.Windows.Media;
using AvnConnect.Data;
using System.Windows.Shapes;

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

    public class TagChips: MaterialDesignThemes.Wpf.Chip
    {
        
        public TagChips()
        {
            this.IsDeletable = true;
        }

        public void SetTags(Data.Tags Tag)
        {
            this.Icon = new MaterialDesignThemes.Wpf.PackIcon()
            {
                Kind = MaterialDesignThemes.Wpf.PackIconKind.Tag
            };
           
            var color = ColorFunctions.FromHexString(Tag.Color);
            if (color != null)
            {
                if (color.Value.R + color.Value.G + color.Value.B < 382)
                {
                    this.IconForeground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    this.IconForeground = new SolidColorBrush(Colors.Black);
                }
                this.IconBackground = new SolidColorBrush(color.Value);
                this.BorderBrush = this.IconBackground;
            } else
            {
                this.IconBackground = new SolidColorBrush(Colors.LightGray);
            }
            this.Content = Tag.TagName;
        }
    }

    public class MyTagChips : UserControl, INotifyPropertyChanged
    {

        public event EventHandler Deleting;
        public event PropertyChangedEventHandler PropertyChanged;

        public MyTagChips()
        {
            this.PropertyChanged += MyTagChips_PropertyChanged;
        }

        private void MyTagChips_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDeletable")
            {
                if (this._IsDeletable)
                {
                    this.DeleteButton.Visibility = Visibility.Visible;
                } else
                {
                    this.DeleteButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        public MyTagChips(Data.Tags tag)
        {
            this.PropertyChanged += MyTagChips_PropertyChanged;
            this.Height = 24;
            this.MyTag = tag;
            this.CreateComponents();
        }

        public Tags MyTag { get; private set; }

        bool _IsDeletable = true;
        private Button DeleteButton;

        public bool IsDeletable
        {
            get { return _IsDeletable; }
            set
            {
                this._IsDeletable = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsDeletable"));
            }
        }


        private void CreateComponents()
        {
            //The border
            Border TextBorder = new Border();
            var BgColor = ColorFunctions.FromHexString(MyTag.Color);
            if (!BgColor.HasValue)
            {
                BgColor = Colors.DarkGray;
            } 
            TextBorder.Background = new SolidColorBrush(BgColor.Value);
            TextBorder.CornerRadius = new CornerRadius(3);
            TextBorder.BorderThickness = new Thickness(1);
            TextBorder.BorderBrush = new SolidColorBrush(ColorFunctions.ChangeColorBrightness(BgColor.Value, -0.3f));
            TextBorder.Padding = new Thickness(5, 2, 5, 2);

            //Stackpanel to host textblock and delete butotn
            StackPanel MainStack = new StackPanel();
            MainStack.Orientation = Orientation.Horizontal;
            TextBorder.Child = MainStack; 

            //Textblock that show tag name
            TextBlock Tex = new TextBlock()
                {
                    Text = MyTag.TagName ,
                    Margin = new Thickness (3, 0, 3, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    MaxWidth = 200,
                    TextTrimming = TextTrimming.CharacterEllipsis
                };

            Color background = ((SolidColorBrush)TextBorder.Background).Color;

            if (ColorFunctions.GetBrightness(background) > 0.5)
            {
                Tex.Foreground = new SolidColorBrush(Colors.Black);
            } else
            {
                Tex.Foreground = new SolidColorBrush(Colors.White);
            }

            //Delete button
            this. DeleteButton = new Button();
            DeleteButton.Style = (Style) this.FindResource("MaterialDesignFlatButton");
            DeleteButton.Content = new MaterialDesignThemes.Wpf.PackIcon() { Kind = MaterialDesignThemes.Wpf.PackIconKind.TagRemove };
            DeleteButton.Height = 20;
            DeleteButton.Foreground = TextBorder.BorderBrush;
            DeleteButton.Margin = new Thickness(2, 0, 0, 0);
            DeleteButton.Width = 20;
            DeleteButton.Padding = new Thickness(0);
            DeleteButton.Click += Delete_Click;
            DeleteButton.ToolTip = "Delete this tag";

            //Add controls to hosting panel
            MainStack.Children.Add(Tex);
            MainStack.Children.Add(DeleteButton);
            this.Content = TextBorder;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Deleting?.Invoke(this, EventArgs.Empty);
        }

        private SolidColorBrush GetBackground(string TagColor)
        {
            var color = ColorFunctions.FromHexString(TagColor);
            if (color != null)
            {
                return new SolidColorBrush(color.Value);
            }
            else
            {
                return new SolidColorBrush(Colors.Gray  );
            }
        }
    }

    public class TwoTagsChip: MaterialDesignThemes.Wpf.Chip
    {
        public object Tag2 { get; set; }
    }
}
