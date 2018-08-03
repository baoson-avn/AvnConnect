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
using AvnConnect.Projects;
using MaterialDesignThemes.Wpf;

namespace AvnConnect.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : UserControl
    {


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ConfirmDialog), new PropertyMetadata(""));


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ConfirmDialog), new PropertyMetadata("Confirm"));






        public Nullable<DialogResults> DialogResult { get; private set; }
        public event EventHandler Closing;



        public enum DialogTypes
        {
            YesNo,
            OK
        }

        public enum DialogResults
        {
            Yes,
            No,
            Cancel,
            OK
        }


        public ConfirmDialog(string Message, DialogTypes TypeOfDialog)
        {
            InitializeComponent();
            this.DialogResult = null;
            this.DataContext = this;
            this.Message = Message;
            this.CreateContent(TypeOfDialog);
        }

        private void CreateContent(DialogTypes typeOfDialog)
        {
            this.ButtonDockPanel.LastChildFill = false;

            switch (typeOfDialog)
            {
                case DialogTypes.YesNo:
                    this.CreateYesNoContent();
                    break;
                case DialogTypes.OK:
                    this.CreateOKContent();
                    break;
                default:
                    break;
            }
        }

        private void CreateOKContent()
        {
            var PrimaryButtonStyle = this.TryFindResource("MaterialDesignRaisedButton");
            Button OK = new Button()
            {
                Content = "OK",
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            DockPanel.SetDock(OK, Dock.Top);
            this.ButtonDockPanel.Children.Add(OK);
            if (PrimaryButtonStyle != null)
            {
                OK.Style = (Style)PrimaryButtonStyle;
            }
            OK.Click += OK_Click;

        }



        private void CreateYesNoContent()
        {
            var PrimaryButtonStyle = this.TryFindResource("MaterialDesignRaisedButton");
            var AccentButtonStyle = this.TryFindResource("MaterialDesignRaisedAccentButton");

            //Yes Button
            Button Yes = new Button()
            {
                Content = "Yes",
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            DockPanel.SetDock(Yes, Dock.Right);
            this.ButtonDockPanel.Children.Add(Yes);
            if (PrimaryButtonStyle != null)
            {
                Yes.Style = (Style)PrimaryButtonStyle;
            }
            Yes.Click += Yes_Click;

            //No Button
            Button No = new Button()
            {
                Content = "No",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            DockPanel.SetDock(No, Dock.Right);
            this.ButtonDockPanel.Children.Add(No);
            if (AccentButtonStyle != null)
            {
                No.Style = (Style)AccentButtonStyle;
            }
            No.Click += No_Click;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = DialogResults.No;
            this.Close();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = DialogResults.Yes;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = DialogResults.OK;
            this.Close();
        }

        private void Close()
        {
            this.Closing?.Invoke(this, EventArgs.Empty);
        }
    }
}
