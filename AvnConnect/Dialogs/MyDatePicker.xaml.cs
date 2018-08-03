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

namespace AvnConnect.Dialogs
{
    /// <summary>
    /// Interaction logic for MyDatePicker.xaml
    /// </summary>
    public partial class MyDatePicker : UserControl
    {


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(MyDatePicker), new PropertyMetadata(""));



        public Nullable<DateTime> SelectedDate
        {
            get { return (Nullable<DateTime>)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(Nullable<DateTime>), typeof(MyDatePicker), new PropertyMetadata(null));



        public MyDatePicker()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void ToDayMyTextBlock_Click(object sender, EventArgs e)
        {
            var date = ExtendedFunctions.GetNetworkTime();
            this.SelectedDate = date.Date;
        }

        private void Tomorrow_Click(object sender, EventArgs e)
        {
            var date = ExtendedFunctions.GetNetworkTime();
            this.SelectedDate = date.AddDays(1).Date;
        }

        private void NextWeek_Click(object sender, EventArgs e)
        {
            var date = ExtendedFunctions.GetNetworkTime();
            this.SelectedDate = date.AddDays(7).Date;
        }

        private void NextMonday_Click(object sender, EventArgs e)
        {
            var date = ExtendedFunctions.GetNetworkTime();
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
            }
            this.SelectedDate = date.Date;
        }

        private void ClearDateClick(object sender, EventArgs e)
        {
            this.SelectedDate = null;
        }
    }
}
