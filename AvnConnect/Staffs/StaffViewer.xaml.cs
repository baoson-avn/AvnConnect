using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.ComponentModel;

namespace AvnConnect.Staffs
{
    /// <summary>
    /// Interaction logic for StaffViewer.xaml
    /// </summary>
    public partial class StaffViewer : UserControl
    {
        private MainWindow MainWindow;
        private ConnectContainer Conn;
        private DbSet StaffDbset;
        private HashSet<char> AlphabetHashSet;
        private bool SortAccending = true;

        public StaffViewer()
        {
            InitializeComponent();
            this.Loaded += StaffViewer_Loaded;
        }

        private void StaffViewer_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStaff();
            this.SortByCombobox.SelectionChanged += SortByCombobox_SelectionChanged;
        }   

        private async void LoadStaff()
        {
            //Lấy tham chiếu đến Database Entity Model
            if (this.Conn == null)
            {
                this.MainWindow = (MainWindow)App.Current.MainWindow;
                this.Conn = this.MainWindow.DataConn;
            }

            if (this.StaffDbset == null)
            {
                this.StaffDbset = this.Conn.Set(typeof(Data.Staff));
                await this.StaffDbset.LoadAsync();
                this.StaffGrid.ItemsSource = this.StaffDbset.Local;
                this.AlphabetListbox.SelectionChanged += AlphabetFilterChanged;
            }

            int i = this.StaffDbset.Local.Count;
            this.SetNoOfUserText(i);
            this.CreateAlphabetFilter();
        }


        private void SetNoOfUserText(int count)
        {
            switch (count)
            {
                case 0:
                    this.NumberOfUsetTextblock.Text = "No users";
                    break;
                case 1:
                    this.NumberOfUsetTextblock.Text = "1 user";
                    break;
                default:
                    this.NumberOfUsetTextblock.Text = count.ToString() + " users";
                    break;
            }
        }
       
        
        #region ALPHABET FILTER

        private void AlphabetFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AlphabetListbox.SelectedIndex == -1)
            {
                this.StaffGrid.Tag = "";
            }
            else
            {
                ListBoxItem lt = (ListBoxItem)this.AlphabetListbox.SelectedItem;
                this.StaffGrid.Tag = lt.Content.ToString();
            }
        }


        private void CreateAlphabetFilter()
        {
            this.AlphabetListbox.Items.Clear();
            this.AlphabetHashSet = new HashSet<Char>();
            foreach (Data.Staff staff in this.StaffDbset)
            {
                this.AlphabetHashSet.Add(staff.Firstname.ToUpper()[0]);
            }
            if (this.AlphabetHashSet.Count > 0)
            {
                var order = this.AlphabetHashSet.OrderBy(c => c);
                foreach (Char c in order)
                {
                    ListBoxItem lt = new ListBoxItem();
                    lt.Content = c;
                    this.AlphabetListbox.Items.Add(lt);
                }
            }
        } 
        #endregion

        #region ADD STAFF

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            Staffs.AddStaff Adf = new Staffs.AddStaff();
            this.DialogHost.DialogContent = Adf;
            Adf.Closing += Adf_Closing;
            Adf.StaffAdded += Adf_StaffAdded;
            Console.WriteLine("Opening Add Staff Dialog...");
            this.DialogHost.IsOpen = true;
        }

        private void Adf_StaffAdded(object sender, EventArgs e)
        {
            CloseAddStaff();

            var messageQueue = MySnackbar.MessageQueue;
            var message = "New Staff Added";

            //the message queue can be called from any thread
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            this.Refresh();
        }

        private void Refresh()
        {
            this.StaffGrid.ItemsSource = null;
            this.StaffGrid.Items.Clear();
            this.StaffDbset.Load();
            this.StaffGrid.ItemsSource = this.StaffDbset.Local;
            this.CreateAlphabetFilter();
        }

        private void Adf_Closing(object sender, EventArgs e)
        {
            CloseAddStaff();
        }

        private void CloseAddStaff()
        {
            Console.WriteLine("Add Staff Dialog Closed without saving!");
            this.DialogHost.IsOpen = false;
            this.DialogHost.DialogContent = null;
        }



        #endregion


        #region SORT
        private void SortByCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SortNow();
        }

        private void SortNow()
        {
            ListSortDirection CurrentSort = this.SortAccending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            switch (SortByCombobox.SelectedIndex)
            {
                case 0:
                    StaffGrid.Items.SortDescriptions.Clear();
                    StaffGrid.Items.SortDescriptions.Add(new SortDescription("Firstname", CurrentSort));
                    break;

                case 1:
                    StaffGrid.Items.SortDescriptions.Clear();
                    StaffGrid.Items.SortDescriptions.Add(new SortDescription("Surname", CurrentSort));
                    break;

                case 2:
                    StaffGrid.Items.SortDescriptions.Clear();
                    StaffGrid.Items.SortDescriptions.Add(new SortDescription("Jobtitle", CurrentSort));
                    break;

                case 3:
                    StaffGrid.Items.SortDescriptions.Clear();
                    StaffGrid.Items.SortDescriptions.Add(new SortDescription("AddedOn", CurrentSort));
                    break;

                default:
                    break;
            }
        }

        private void ChangeSort(object sender, RoutedEventArgs e)
        {
            this.SortAccending = !this.SortAccending;
            this.SortIcon.Kind = this.SortAccending ? MaterialDesignThemes.Wpf.PackIconKind.SortAscending : MaterialDesignThemes.Wpf.PackIconKind.SortDescending;
            this.SortNow();
        } 
        #endregion
    }
}
