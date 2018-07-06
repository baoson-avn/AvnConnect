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
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
            foreach (Dialogs.ColorButton cb in this.UniGrid.Children)
            {
                cb.Selected += Cb_Selected;
            }
            this.CurrentSelectedColor = new SolidColorBrush(Colors.DimGray);
        }

        public SolidColorBrush CurrentSelectedColor { get; private set; }
        private Dialogs.ColorButton SelectedItem = null;


        private void Cb_Selected(object sender, EventArgs e)
        {
            Dialogs.ColorButton cb = sender as Dialogs.ColorButton;
            this.CurrentSelectedColor = cb.ColorBrush;
            if (this.SelectedItem != null)
            {
                this.SelectedItem.IsSelected = false;
            }
            this.SelectedItem = cb;
        }

        internal void PickColor(string color)
        {
            foreach (Dialogs.ColorButton cb in this.UniGrid.Children)
            {
                if (cb.ColorBrush.Color.ToString() == color)
                {
                    cb.IsSelected = true;
                    cb.Select();
                    break;
                }
            }
        }
    }
}
