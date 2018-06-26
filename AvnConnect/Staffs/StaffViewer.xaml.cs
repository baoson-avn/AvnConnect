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

namespace AvnConnect.Staffs
{
    /// <summary>
    /// Interaction logic for StaffViewer.xaml
    /// </summary>
    public partial class StaffViewer : UserControl
    {
        public StaffViewer()
        {
            InitializeComponent();
        }


        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            Staffs.AddStaff Adf = new Staffs.AddStaff();
            this.DialogHost.DialogContent = Adf;
            Adf.Closing += Adf_Closing;
            Console.WriteLine("Opening Add Staff Dialog...");
            this.DialogHost.IsOpen = true;
            
        }

        private void Adf_Closing(object sender, EventArgs e)
        {
            Console.WriteLine("Add Staff Dialog Closed without saving!");
            this.DialogHost.IsOpen = false;
            this.DialogHost.DialogContent = null;
        }
    }
}
