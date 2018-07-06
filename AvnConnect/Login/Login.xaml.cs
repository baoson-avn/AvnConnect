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

namespace AvnConnect.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Staff LoggedInStaff { get; private set; }
        public event EventHandler LoginCompleted;


        public Login()
        {
            InitializeComponent();
            LoginButton.IsEnabled = false;  
            this.EmailTextbox.Text = "trinhhuubaoson@gmail.com";
            this.PasswordTextbox.Password = "12345678";
        }



        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.PasswordTextbox.IsEnabled = false;
            this.EmailTextbox.IsEnabled = false;
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            string[] arg = { this.EmailTextbox.Text, this.PasswordTextbox.Password };
            worker.RunWorkerAsync(arg);
            
        }

        private void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                this.LoggedInStaff = (Data.Staff)e.Result;
                this.LoginCompleted?.Invoke(this, EventArgs.Empty);
            } else
            {
                this.EmailTextbox.IsEnabled = true;
                this.PasswordTextbox.IsEnabled = true;
                this.MySnackbar.MessageQueue.Enqueue("LOGIN FAILED");
            }
        }

        private void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string[] arg = (string[])e.Argument;
            using (AvnConnect.Data.ConnectContainer conn = new Data.ConnectContainer())
            {
                string email = arg[0];
                string password = arg[1];
                var Staff = conn.Staffs.Where(st => st.EmailAddress == email && st.Password == password).FirstOrDefault();
                if (Staff != null)
                {
                    e.Result = Staff;
                }
            }
        }

        private void EmailTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginButton.IsEnabled = this.EmailTextbox.Text.Trim() != "" && this.PasswordTextbox.Password.Trim() != "";
        }

        private void PasswordTextbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = this.EmailTextbox.Text.Trim() != "" && this.PasswordTextbox.Password.Trim() != "";
        }
    }
}
