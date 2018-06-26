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

namespace AvnConnect.Staffs
{
    /// <summary>
    /// Interaction logic for AddStaff.xaml
    /// </summary>
    public partial class AddStaff : UserControl
    {
        public event EventHandler Closing;

        public Data.MyStaff MyStaff
        {
            get { return (Data.MyStaff)GetValue(MyStaffProperty); }
            set { SetValue(MyStaffProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyStaff.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyStaffProperty =
            DependencyProperty.Register("MyStaff", typeof(Data.MyStaff), typeof(AddStaff), new PropertyMetadata(null));


        private ConnectContainer Conn;
        private MainWindow MainWindow;
        private DbSet EducationDbset;
        private DbSet LanguageDbset;
        private DbSet JobTitleDataSet;
        private DbSet DepartmentDataSet;


        /// <summary>
        /// Sử dụng khi tạo hộp thoại mới để thêm nhân viên
        /// </summary>
        public AddStaff()
        {
            InitializeComponent();
            this.DataContext = this;

            //Tạo instance của class MyStaff
            this.MyStaff = new Data.MyStaff();

            //Tạo key cho nhân viên
            this.MyStaff.Key = Encryption.GetUniqueKey(16);

            //Tải thông tin từ database
            this.BindingDatabase();
        }


        /// <summary>
        /// Sử dụng khi mở hộp thoại để sửa nhân viên cũ
        /// </summary>
        /// <param name="Staff"></param>
        public AddStaff(Data.Staff Staff)
        {
            InitializeComponent();
            this.MyStaff.FromEframeStaff(Staff);
            this.DataContext = this;
        }


        /// <summary>
        /// Create a dataset from database and bind to datagrid view
        /// </summary>
        private async void BindingDatabase()
        {
            //Lấy tham chiếu đến Database Entity Model
            if (this.Conn == null)
            {
                this.MainWindow = (MainWindow)App.Current.MainWindow;
                this.Conn = this.MainWindow.DataConn;
            }

            //Chức vụ
            if (this.JobTitleDataSet ==null)
            {
                this.JobTitleDataSet = Conn.Set(typeof(Data.JobTitles));
                await JobTitleDataSet.LoadAsync();
                Console.WriteLine("Jobtitle Loaded and binded to combobox");
                this.JobTitleCombobox.DisplayMemberPath = "Title";
                this.JobTitleCombobox.ItemsSource = JobTitleDataSet.Local;
            }

            //Phòng ban
            if (this.DepartmentDataSet == null)
            {
                this.DepartmentDataSet = Conn.Set(typeof(Data.Department));
                await DepartmentDataSet.LoadAsync();
                Console.WriteLine("Jobtitle Loaded and binded to combobox");
                this.DepartmentCombobox.DisplayMemberPath = "DepartmentName";
                this.DepartmentCombobox.ItemsSource = DepartmentDataSet.Local;
            }

            //Học vấn
            if (this.EducationDbset == null)
            {
                var Edus = Conn.Educations.Where(edu => edu.StaffKey == this.MainWindow.StaffKey);

                this.EducationDbset = Conn.Set(typeof(Data.Education));
                foreach (var item in Edus)
                {
                    EducationDbset.Attach(item);
                }
                this.EducationGrid.ItemsSource = EducationDbset.Local;
                if (this.EducationGrid.Items.Count == 0)
                {
                    this.CreateNewRow_Education();
                }
            }

            ////Ngoại ngữ
            //if (this.LanguageDbset == null)
            //{
            //    this.LanguageDbset = Conn.Set(typeof(Data.ForeignLanguage));
            //    await LanguageDbset.LoadAsync();
            //    this.ForeignLanguageDatagrid.ItemsSource = this.LanguageDbset.Local;
            //}



        }


        /// <summary>
        /// Close the popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Closing?.Invoke(this, EventArgs.Empty);
        }

        #region EDUCATION


        /// <summary>
        /// Thêm 1 dòng mới để nhập thông tin học vấn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EducationAddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Conn.ChangeTracker.DetectChanges();
            try
            {
                var changes = this.Conn.ChangeTracker.Entries<Data.Education>();
                foreach (var item in changes)
                {
                    item.Entity.StaffKey = this.MainWindow.StaffKey;
                    if (item.Entity.EducationKey == null || item.Entity.EducationKey == "")
                    {
                        item.Entity.EducationKey = Encryption.GetUniqueKey(16);
                    }
                }
                Console.WriteLine("Database updated. {0} row(s) changed!", this.Conn.SaveChanges());
            }
            catch (Exception ex)
            {
                this.Closing?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Delete education record 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            Console.WriteLine("Deleting Education Key: {0}", Btn.Tag.ToString());
            DeleteEducationRow(Btn.Tag.ToString());

        }

        /// <summary>
        /// Delete the Education Record with the key
        /// </summary>
        /// <param name="v">Education Key</param>
        private void DeleteEducationRow(string v)
        {
            foreach (Data.Education item in EducationDbset.Local)
            {
                if (item.EducationKey == v)
                {
                    EducationDbset.Local.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// Add new Education Row to input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EducationAddNewRow(object sender, RoutedEventArgs e)
        {
            CreateNewRow_Education();
        }


        /// <summary>
        /// Add new EducationRow to Input
        /// </summary>
        private void CreateNewRow_Education()
        {
            var newEdu = this.EducationDbset.Create();
            Education C = (Education)newEdu;
            C.EducationKey = Encryption.GetUniqueKey(16);
            C.StaffKey = this.MainWindow.StaffKey;
            this.EducationDbset.Local.Add(newEdu);
        }

        #endregion


        private void EducationGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Console.WriteLine("EducationGrid_RowEditEnding");
        }

        private void ForeignDatagrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            Console.WriteLine(e.NewItem.GetType().ToString());
        }


        /// <summary>
        /// Select the listboxitem that control display of UI elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderButtonClicked(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            if (Btn.Tag != null)
            {
                string itemName = Btn.Tag.ToString();
                foreach (ListBoxItem item in this.ItemListBox.Items)
                {
                    if (item.Name == itemName)
                    {
                        item.IsSelected = true;
                    }
                }
            }
        }


        /// <summary>
        /// Thêm nhân viên mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUserButtonClicked(object sender, RoutedEventArgs e)
        {
            bool b = this.ValidatePersonalInformation();
            try
            {
                Conn.ChangeTracker.DetectChanges();
                Conn.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Kiểm tra các thông tin cá nhân đã nhập
        /// </summary>
        /// <returns></returns>
        private bool ValidatePersonalInformation()
        {
            //Chưa nhập họ --> trả về false
            if (MyStaff.Surname == null || MyStaff.Surname == "") return false;

            //Chưa nhập tên --> trả về false
            if (MyStaff.Firstname == null || MyStaff.Firstname == "") return false;

            //Chưa nhập email --> trả về false
            if (MyStaff.EmailAddress == null || MyStaff.EmailAddress == "") return false;

            //Nếu email không phù hợp --> trả về false
            AvnConnect.RegexUtilities Regex = new AvnConnect.RegexUtilities();
            if (!Regex.IsValidEmail(MyStaff.EmailAddress)) return false;

            //Nếu chưa chọn chức vụ --> trả về false
            if (MyStaff.JobTitle == null || MyStaff.JobTitle == "") return false;

            //Nếu chưa chọn phòng ban --> trả về false
            if (MyStaff.Department == null || MyStaff.Department == "") return false;

            //Thông tin về tình trạng hôn nhân
            this.MyStaff.MaritalStatus = this.MaritalCombobox.SelectedValue.ToString();

            //Thông tin về quốc tịch
            this.MyStaff.Nationality = "Vietnamese";

            //Nếu đã nhập hết thì ok
            return true;
        }

        #region ADD JOB TITLE

        //Tạo popup để thêm chức vụ mới
        private void AddNewJobTitleButton_Clicked(object sender, RoutedEventArgs e)
        {
            AskForText newJobTitleDialog = new Staffs.AskForText();
            newJobTitleDialog.SetHeaderText("New Job Title:");
            newJobTitleDialog.Canceled += NewJobTitleDialog_Canceled;
            newJobTitleDialog.Confirmed += NewJobTitleDialog_AddingNewTitle;
            this.DialogHost.DialogContent = newJobTitleDialog;
            this.DialogHost.Visibility = Visibility.Visible;
            this.DialogHost.IsOpen = true;
        }

        /// <summary>
        /// Add new title
        /// </summary>
        /// <param name="sender">The dialog</param>
        /// <param name="e"></param>
        private void NewJobTitleDialog_AddingNewTitle(object sender, EventArgs e)
        {
            try
            {
                AskForText newJobTitleDialog = (AskForText)sender;
                string newTitle = newJobTitleDialog.Value;
                this.CloseDialog();

                if (this.JobTitleDataSet != null)
                {
                    JobTitles newJobtitle = new JobTitles()
                    {
                        Key = Encryption.GetUniqueKey(16),
                        Title = newTitle
                    };

                    this.JobTitleDataSet.Add(newJobtitle);

                    //Chọn chức vụ mới được thêm vào
                    if (JobTitleCombobox.Items.Count > 0) this.JobTitleCombobox.SelectedIndex = JobTitleCombobox.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("NewJobTitleDialog_AddingNewTitle: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Đóng popup thêm chức vụ mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewJobTitleDialog_Canceled(object sender, EventArgs e)
        {
            try
            {
                CloseDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine("NewJobTitleDialog_Canceled: {0}", ex.Message);
            }
        }


        #endregion

        #region ADD DEPARTMENT

        /// <summary>
        /// Tạo popup để thêm phòng ban mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDepartmentButton_Clicked(object sender, RoutedEventArgs e)
        {
            AskForText newDepartmentDialog = new Staffs.AskForText();
            newDepartmentDialog.SetHeaderText("New Department Name:");
            newDepartmentDialog.Canceled += newDepartmentDialog_Canceled;
            newDepartmentDialog.Confirmed += newDepartmentDialog_AddingNewTitle;
            this.DialogHost.DialogContent = newDepartmentDialog;
            this.DialogHost.Visibility = Visibility.Visible;
            this.DialogHost.IsOpen = true;
        }

        private void newDepartmentDialog_AddingNewTitle(object sender, EventArgs e)
        {
            try
            {
                AskForText newDepartmentDialog = (AskForText)sender;
                string newTitle = newDepartmentDialog.Value;
                this.CloseDialog();

                if (this.DepartmentDataSet != null)
                {
                    Department newDepartment = new Department()
                    {
                        Key = Encryption.GetUniqueKey(16),
                        DepartmentName = newTitle
                    };

                    this.DepartmentDataSet.Add(newDepartment);

                    if (this.DepartmentCombobox.Items.Count > 0) this.DepartmentCombobox.SelectedIndex = this.DepartmentCombobox.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("newDepartmentDialog_AddingNewTitle: {0}", ex.Message);
            }
        }


        /// <summary>
        /// Đóng popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newDepartmentDialog_Canceled(object sender, EventArgs e)
        {
            try
            {
                CloseDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine("NewJobTitleDialog_Canceled: {0}", ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Close any dialog hosted by this.DialogHost
        /// </summary>
        private void CloseDialog()
        {
            this.DialogHost.DialogContent = null;
            this.DialogHost.IsOpen = false;
            this.DialogHost.Visibility = Visibility.Collapsed;
        }

      
    }
}
