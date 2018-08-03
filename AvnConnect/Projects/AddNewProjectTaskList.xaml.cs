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

namespace AvnConnect.Projects
{
    /// <summary>
    /// Interaction logic for AddNewProjectTaskList.xaml
    /// </summary>
    public partial class AddNewProjectTaskList : UserControl
    {

        public event EventHandler Closed;
        public event EventHandler Adding;

        public Data.ProjectTaskList MyList
        {
            get { return (Data.ProjectTaskList)GetValue(MyListProperty); }
            set { SetValue(MyListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyListProperty =
            DependencyProperty.Register("MyList", typeof(Data.ProjectTaskList), typeof(AddNewProjectTaskList), new PropertyMetadata(null));


        public AddNewProjectTaskList()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        #region CLOSING

        private void CloseButton_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Close()
        {
            this.Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Adding?.Invoke(this, EventArgs.Empty);
        }
    }
}
