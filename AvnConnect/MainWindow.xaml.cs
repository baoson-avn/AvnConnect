using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvnConnect.Data;
using MahApps.Metro.Controls;

namespace AvnConnect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public Data.ConnectContainer DataConn;
        public string StaffKey;
        public bool CanManageUser { get; private set; }
        public Nullable<DateTime> LoggedInDateTime { get; set; }

        public TimeSpan DateTimeOffset { get; private set; }
        public Staff LoggedInStaff { get; private set; }
        public Permission Permission { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataConn = new Data.ConnectContainer();

            this.DataConn.Staffs.FirstOrDefault().Password = "12345678";
            this.DataConn.SaveChanges();

            //this.WindowsCommandCollection.IsHitTestVisible = true;
            //this.BlockingBackground.Visibility = Visibility.Collapsed;

            this.Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// Kết nối đến NTS để lấy dữ liệu ngày tháng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Get the time
            System.ComponentModel.BackgroundWorker GetTimeWorker = new System.ComponentModel.BackgroundWorker();
            GetTimeWorker.DoWork += GetTimeWorker_DoWork;
            GetTimeWorker.RunWorkerCompleted += GetTimeWorker_RunWorkerCompleted;
            GetTimeWorker.RunWorkerAsync();

            //Show the login dialog
            Login.Login Lg = new Login.Login();
            Lg.LoginCompleted += Lg_LoginCompleted;
            this.ShowDialog(Lg);


        }

        public void ShowDialog(object Content)
        {
            this.DialogHost.DialogContent = Content;
            this.DialogHost.Visibility = Visibility.Visible;
            this.DialogHost.IsOpen = true;
        }


        private void Lg_LoginCompleted(object sender, EventArgs e)
        {
            //Login dialog instance
            Login.Login lg = (Login.Login)sender;

            //Disable the blocking border
            this.BlockingBackground.Visibility = Visibility.Collapsed;
            Grid G = (Grid)this.BlockingBackground.Parent;
            G.Children.Remove(BlockingBackground);

            //Close the dialog host
            this.DialogHost.Visibility = Visibility.Collapsed;
            this.DialogHost.IsOpen = false;

            //Create model
            this.DataConn = new Data.ConnectContainer();

            //Logged in staff
            this.LoggedInStaff = lg.LoggedInStaff;
            this.StaffKey = this.LoggedInStaff.Key;

            //Get permission of current user
            this.Permission = this.DataConn.Permissions.Where(p => p.StaffKey == this.StaffKey).FirstOrDefault();
            if (this.Permission != null)
            {
                this.CanManageUser = this.Permission.CanManageStaff;
            }

            //Enable the windows command button
            this.WindowsCommandCollection.IsHitTestVisible = true;

            //Create the view
            this.ProjectViewer.LoadTreeView();
        }

        private void GetTimeWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                this.LoggedInDateTime = null;
            } else
            {
                this.LoggedInDateTime = (DateTime)e.Result;
                this.DateTimeOffset = this.LoggedInDateTime.Value.Subtract(DateTime.Now);
            }
        }

        private void GetTimeWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Console.WriteLine("Trying to get time from timeserver...");
                DateTime Now = ExtendedFunctions.GetNetworkTime();
                Console.WriteLine("It's {0}", Now.ToLocalTime().ToString());
                e.Result = Now;
            }
            catch (Exception)
            {
                e.Result = null;
            }
        }

        private void WindowCommandButton_Click(object sender, EventArgs e)
        {
            MyTextBlock mt = (MyTextBlock)sender;
            string itemname = mt.Tag.ToString();
            ListBoxItem lt = (ListBoxItem) this.FindName(itemname);
            lt.IsSelected = true;
            mt.Opacity = 1;

            foreach (MyTextBlock item in WindowsCommandCollection.Items)
            {
                if (item != mt)
                {
                    item.Opacity = 0.8;
                }
            }
        }
    }
}
