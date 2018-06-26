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
    /// Interaction logic for AddNewJobTitle.xaml
    /// </summary>
    public partial class AskForText : UserControl
    {
        public event EventHandler Confirmed;
        public event EventHandler Canceled;

        public string Value
        {
            get { return (string)GetValue(NewTitleProperty); }
            set { SetValue(NewTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewTitleProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(AskForText), new PropertyMetadata(""));



        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(AskForText), new PropertyMetadata("Input:"));


        public AskForText()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void SetHeaderText(string HeaderTxt)
        {
            this.Header = HeaderTxt;
        }

        /// <summary>
        /// Trả về tên của chức vụ mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Confirmed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gọi event hủy bỏ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Canceled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Bật/tắt nút ok khi textbox thay đổi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox T = (TextBox)sender;
            this.OkButton.IsEnabled = !(T.Text == "");
        }
    }
}
