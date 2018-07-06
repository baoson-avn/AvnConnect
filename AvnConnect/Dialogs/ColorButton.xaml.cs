using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ColorButton.xaml
    /// </summary>
    public partial class ColorButton : UserControl
    {
        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush)GetValue(ColorBrushProperty); }
            set { SetValue(ColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(ColorButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));




        public SolidColorBrush DefaultBorderBrush
        {
            get { return (SolidColorBrush)GetValue(DefaultBorderBrushProperty); }
            set { SetValue(DefaultBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultBorderBrushProperty =
            DependencyProperty.Register("DefaultBorderBrush", typeof(SolidColorBrush), typeof(ColorButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));




        public event EventHandler Selected;

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    Select();
                } else
                {
                    DeSelect();
                }
            }
        }

        private void DeSelect()
        {
            this.MainBorder.BorderBrush = DefaultBorderBrush;

        }

        public ColorButton()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        internal void Select()
        {
            this.MainBorder.BorderBrush = this.ContentEllipse.Fill;
            this.Selected?.Invoke(this, EventArgs.Empty);
        }



        private void MainBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount <=2)
            {
                if (IsSelected)
                {
                    this.IsSelected = false;
                    return;
                }

                this.IsSelected = true;
            }         
        }
    }


    
}
