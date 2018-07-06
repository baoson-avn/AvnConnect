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
        public event EventHandler StaffAdded;
        private bool IsEditing = false ;


        public System.Collections.IList JobTitleDbset
        {
            get { return (System.Collections.IList)GetValue(JobTitleDbsetProperty); }
            set { SetValue(JobTitleDbsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for JobTitleDbset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JobTitleDbsetProperty =
            DependencyProperty.Register("JobTitleDbset", typeof(System.Collections.IList), typeof(AddStaff), new PropertyMetadata(null));


        public System.Collections.IList DepartmentDbset
        {
            get { return (System.Collections.IList)GetValue(DepartmentDbsetProperty); }
            set { SetValue(DepartmentDbsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DepartmentDbset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepartmentDbsetProperty =
            DependencyProperty.Register("DepartmentDbset", typeof(System.Collections.IList), typeof(AddStaff), new PropertyMetadata(null));



        private ConnectContainer Conn;
        private MainWindow MainWindow;
        private DbSet EducationDbset;
        private DbSet LanguageDbset;
        private DbSet JobTitleDataSet;
        private DbSet DepartmentDataSet;
        private DbSet PersonalDataset;
        private ComboBox JobTitleCombobox;
        private ComboBox DepartmentCombobox;
        private ComboBox MaritalStatusCombobox;
        private ComboBox GenderCombobox;
        private string StaffKey;
        private DbSet LicenseDbset;
        private DbSet ProfessionalDbset;
        private Staff MyStaff;
        private DbSet ExpDbset;
        private DbSet PermissionDbset;


        /// <summary>
        /// Sử dụng khi tạo hộp thoại mới để thêm nhân viên
        /// </summary>
        public AddStaff()
        {
            InitializeComponent();
            this.StaffKey = Encryption.GetUniqueKey(16);
            this.MainWindow = (MainWindow)App.Current.MainWindow;
            this.Loaded += AddStaff_Loaded;
        }

        private void AddStaff_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            this.ShowLoading();
        }




        /// <summary>
        /// Sử dụng khi mở hộp thoại để sửa nhân viên cũ
        /// </summary>
        /// <param name="Staff"></param>
        public AddStaff(Data.Staff Staff)
        {
            InitializeComponent();
            this.MyStaff = Staff;
            this.IsEditing = true;
            this.DataContext = this;
            this.StaffKey = MyStaff.Key;
            this.BindingDatabase();
        }


        /// <summary>
        /// Create a dataset from database and bind to datagrid view
        /// </summary>
        private void BindingDatabase()
        {
            //Lấy tham chiếu đến Database Entity Model
            if (this.Conn == null)
            {
                this.Conn = new Data.ConnectContainer ();
            }

            //Personal information
            if (this.PersonalDataset == null)
            {
                this.PersonalDataset = Conn.Set(typeof(Data.Staff));

                if (this.IsEditing)
                {
                    var Personal = Conn.Staffs.Where(stf => stf.Key == this.StaffKey);
                    foreach (var item in Personal)
                    {
                        PersonalDataset.Attach(item);
                    }
                }
                
                if (this.PersonalDataset.Local.Count == 0)
                {
                    var newPs = this.PersonalDataset.Create();
                    Staff C = (Staff)newPs;
                    C.Key = this.StaffKey;
                    this.PersonalDataset.Local.Add(newPs);
                }
                this.PersonalDatagrid.ItemsSource = PersonalDataset.Local;
            }

            //Chức vụ
            if (this.JobTitleDataSet ==null)
            {
                this.JobTitleDataSet = Conn.Set(typeof(Data.JobTitles));
                JobTitleDataSet.Load();
                this.JobTitleDbset = JobTitleDataSet.Local;
            }

            //Phòng ban
            if (this.DepartmentDataSet == null)
            {
                this.DepartmentDataSet = Conn.Set(typeof(Data.Department));
                DepartmentDataSet.Load();
                this.DepartmentDbset = DepartmentDataSet.Local;
            }

            //Học vấn
            if (this.EducationDbset == null)
            {
                var Edus = Conn.Educations.Where(edu => edu.StaffKey == this.StaffKey);
                this.EducationDbset = Conn.Set(typeof(Data.Education));
                foreach (var item in Edus)
                {
                    EducationDbset.Attach(item);
                }
                this.EducationGrid.ItemsSource = EducationDbset.Local;
            }

            //Ngoại ngữ
            if (this.LanguageDbset == null)
            {
                var Language = Conn.ForeignLanguages.Where(lg => lg.StaffKey == this.StaffKey);
                this.LanguageDbset = Conn.Set(typeof(Data.ForeignLanguage));
                foreach (var item in Language)
                {
                    LanguageDbset.Attach(item);
                }
                this.ForeignLanguageDatagrid.ItemsSource = this.LanguageDbset.Local;
            }

            //Lĩnh vực chuyên môn
            if (this.ProfessionalDbset == null)
            {
                this.ProfessionalDbset = Conn.Set(typeof(Data.ProfesstionalArea));
                ProfessionalDbset.Load();
            }

            //Giấy phép hành nghề
            if (this.LicenseDbset == null)
            {
                var Licenses = Conn.PracticingLicenses.Where(lc => lc.StaffKey == this.StaffKey);
                this.LicenseDbset = Conn.Set(typeof(Data.PracticingLicense));
                foreach (var item in Licenses)
                {
                    LicenseDbset.Attach(item);
                }
                this.PracticingDatagrid.ItemsSource = this.LicenseDbset.Local;
            }

            //Kinh nghiệm làm việc
            if (this.ExpDbset == null)
            {
                var Exps = Conn.WorkingExperiences.Where(exp => exp.StaffKey == this.StaffKey);
                this.ExpDbset = Conn.Set(typeof(Data.WorkingExperience));
                foreach (var item in Exps)
                {
                    ExpDbset.Attach(item);
                }
                this.ExperienceGrid.ItemsSource = this.ExpDbset.Local;
            }

            //Phan quyen
            if (this.PermissionDbset == null)
            {
                var Permission = Conn.Permissions.Where(pm => pm.StaffKey == this.StaffKey);
                this.PermissionDbset = Conn.Set(typeof(Data.Permission));
                foreach (var item in Permission)
                {
                    PermissionDbset.Attach(item);
                }
                this.PermissionGrid.ItemsSource = PermissionDbset.Local;
                if (PermissionDbset.Local.Count == 0)
                {
                    Data.Permission myPermission = (Data.Permission)PermissionDbset.Create();
                    myPermission.StaffKey = this.StaffKey;
                    myPermission.Key = Encryption.GetUniqueKey(16);
                    this.PermissionDbset.Local.Add(myPermission);
                }
            }
        }

        /// <summary>
        /// Show the loading progressbar
        /// </summary>
        private void ShowLoading()
        {
            ProgressBar Progress = new ProgressBar();
            Progress.Style = (Style)this.FindResource("MaterialDesignCircularProgressBar");
            Progress.IsIndeterminate = true;
            Progress.Value = 35;
            Progress.Margin = new Thickness(30);
            Progress.Maximum = 100;
            Progress.MinHeight = 100;
            Progress.MinWidth = 100;


            this.DialogHost.DialogContent = Progress;
            this.DialogHost.Visibility = Visibility.Visible;
            this.DialogHost.IsOpen = true;

            //Tải thông tin từ database
            this.BindingDatabase();

            this.CloseDialog();
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
        /// Delete education record 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EducationDeleteButton_Clicked(object sender, RoutedEventArgs e)
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
            C.StaffKey = this.StaffKey;
            this.EducationDbset.Local.Add(newEdu);
        }

        private void EducationGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Console.WriteLine("EducationGrid_RowEditEnding");
        }


        #endregion

        #region PERSONAL

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
        /// Khi thay đổi lựa chọn jobtitle thì bind vào Personal Instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobTitleComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Personal = (Data.Staff)this.PersonalDataset.Local[0];
            ComboBox b = (ComboBox)sender;
            Personal.JobTitle = ((Data.JobTitles)b.SelectedValue).Title;
        }

        /// <summary>
        /// Lấy ref đến jobtile combobox, sử dụng để load giá trị hiện tại của jobtitle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobTitleCombobox_Loaded(object sender, RoutedEventArgs e)
        {
            this.JobTitleCombobox = (ComboBox)sender;
        }


        private void DepartmentCombobox_Loaded(object sender, RoutedEventArgs e)
        {
            this.DepartmentCombobox = (ComboBox)sender;
        }

        private void DepartmentCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Personal = (Data.Staff)this.PersonalDataset.Local[0];
            Personal.Department = ((Data.Department)this.DepartmentCombobox.SelectedValue).DepartmentName;
        }


        private void MaritalCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Personal = (Data.Staff)this.PersonalDataset.Local[0];
            this.MaritalStatusCombobox = (ComboBox)sender;
            Personal.MaritalStatus = (this.MaritalStatusCombobox.SelectedValue).ToString();
        }

        private void GenderCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Personal = (Data.Staff)this.PersonalDataset.Local[0];
            this.GenderCombobox = (ComboBox)sender;
            Personal.Gender = (this.GenderCombobox.SelectedIndex == 0);
        }

        #endregion

        #region FOREIGN LANGUAGE
        private void LanguageDeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            Console.WriteLine("Deleting Language Key: {0}", Btn.Tag.ToString());
            DeleteLanguageRow(Btn.Tag.ToString());
        }

        private void DeleteLanguageRow(string v)
        {
            foreach (Data.ForeignLanguage item in LanguageDbset.Local)
            {
                if (item.Key == v)
                {
                    LanguageDbset.Local.Remove(item);
                    break;
                }
            }
        }

        private void LanguageAddNew_Clicked(object sender, RoutedEventArgs e)
        {
            CreateNewRow_Language();
        }

        private void CreateNewRow_Language()
        {
            var newLan = this.LanguageDbset.Create();
            ForeignLanguage C = (ForeignLanguage)newLan;
            C.Key = Encryption.GetUniqueKey(16);
            C.StaffKey = this.StaffKey;
            this.LanguageDbset.Local.Add(newLan);
        }
        #endregion

        #region PRACTICING LICENSES

        private void AddNewAreaButton_Clicked(object sender, RoutedEventArgs e)
        {
            AskForText newAreaDialog = new Staffs.AskForText();
            newAreaDialog.SetHeaderText("New Professional Area:");
            newAreaDialog.Canceled += newAreaDialog_Canceled;
            newAreaDialog.Confirmed += newAreaDialog_Confirmed;
            this.DialogHost.DialogContent = newAreaDialog;
            this.DialogHost.Visibility = Visibility.Visible;
            this.DialogHost.IsOpen = true;
        }

        private void newAreaDialog_Confirmed(object sender, EventArgs e)
        {
            try
            {
                AskForText newAreaDialog = (AskForText)sender;
                string newTitle = newAreaDialog.Value;
                this.CloseDialog();

                if (this.ProfessionalDbset != null)
                {
                    ProfesstionalArea newPa = new ProfesstionalArea()
                    {
                        Key = Encryption.GetUniqueKey(16),
                        Name = newTitle
                    };

                    this.ProfessionalDbset.Add(newPa);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("newAreaDialog_Confirmed: {0}", ex.Message);
            }
        }

        private void newAreaDialog_Canceled(object sender, EventArgs e)
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

        private void ProfestionalAreaCombobox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            cb.ItemsSource = this.ProfessionalDbset.Local;
            cb.SelectionChanged += ProfestionalAreaCombobox_SelectionChanged;
        }

        private void ProfestionalAreaCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.Tag == null) return; 
            string AreaKey = cb.Tag.ToString();
            foreach (PracticingLicense item in LicenseDbset.Local)
            {
                if (item.Key == AreaKey)
                {
                    ProfesstionalArea area = (ProfesstionalArea)cb.SelectedItem;
                    item.ProfessionalArea = area.Name;
                    break;
                } 
            }
        }

        private void LicenseDeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            Console.WriteLine("Deleting Language Key: {0}", Btn.Tag.ToString());
            DeleteLicenseRow(Btn.Tag.ToString());
        }

        private void DeleteLicenseRow(string v)
        {
            foreach (Data.PracticingLicense item in LicenseDbset.Local)
            {
                if (item.Key == v)
                {
                    LicenseDbset.Local.Remove(item);
                    break;
                }
            }
        }

        private void PracticingAddNew_Clicked(object sender, RoutedEventArgs e)
        {
            CreateNewRow_Practicing();
        }

        private void CreateNewRow_Practicing()
        {
            var newLic = this.LicenseDbset.Create();
            PracticingLicense C = (PracticingLicense)newLic;
            C.Key = Encryption.GetUniqueKey(16);
            C.StaffKey = this.StaffKey;
            this.LicenseDbset.Local.Add(newLic);
        }

        private void LicenseStatusComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!IsEditing)
            {
                cb.SelectedIndex = 0;
            } else
            {
                if (cb.Tag == null) return;
                string LicenseKey = cb.Tag.ToString();
                foreach (Data.PracticingLicense lic in LicenseDbset.Local)
                {
                    if (lic.Key == LicenseKey)
                    {
                        foreach (ComboBoxItem item in cb.Items)
                        {
                            if (item.Content.ToString() == lic.Status)
                            {
                                item.IsSelected = true;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            cb.SelectionChanged += Cb_SelectionChanged;
        }

        private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.Tag == null) return;
            string LicenseKey = cb.Tag.ToString();
            foreach (Data.PracticingLicense lic in LicenseDbset.Local)
            {
                if (lic.Key == LicenseKey)
                {
                    lic.Status = ((ComboBoxItem)cb.SelectedItem).Content.ToString();
                    break;
                }
            }
        }
        #endregion

        #region EXPERIENCES
        private void ExpDeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            Console.WriteLine("Deleting Experience Key: {0}", Btn.Tag.ToString());
            DeleteExpRow(Btn.Tag.ToString());
        }

        private void DeleteExpRow(string v)
        {
            foreach (Data.WorkingExperience item in ExpDbset.Local)
            {
                if (item.Key == v)
                {
                    ExpDbset.Local.Remove(item);
                    break;
                }
            }
        }

        private void ExpAddNew_Clicked(object sender, RoutedEventArgs e)
        {
            CreateNewRow_Exp();
        }

        private void CreateNewRow_Exp()
        {
            var newExp = this.ExpDbset.Create();
            WorkingExperience C = (WorkingExperience)newExp;
            C.Key = Encryption.GetUniqueKey(16);
            C.StaffKey = this.StaffKey;
            this.ExpDbset.Local.Add(newExp);
        }
        #endregion


        /// <summary>
        /// Thêm nhân viên mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUserButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var DateTimeNow = ExtendedFunctions.GetNetworkTime();
                foreach (var item in Conn.ChangeTracker.Entries<Staff>())
                {
                    if (!this.IsEditing)
                    {
                        item.Entity.AddedOn = DateTimeNow;
                        item.Entity.AddedBy = this.MainWindow.StaffKey != null ? this.MainWindow.StaffKey : item.Entity.Key;
                        item.Entity.Password = Encryption.GetUniqueKey(8);
                    };
                    item.Entity.ModifiedBy = this.MainWindow.StaffKey != null ? this.MainWindow.StaffKey : item.Entity.Key;
                    item.Entity.ModifiedOn = DateTimeNow;
                }

                bool _NoMoreError = true;

                _NoMoreError = this.Validate();

                if (_NoMoreError)
                {
                    int i = Conn.SaveChanges();
                    if (i > 0)
                    {
                        this.StaffAdded?.Invoke(this, EventArgs.Empty);
                    } else
                    {
                        MySnackbar.MessageQueue.Enqueue("Error while adding staff. Please try again later.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool Validate()
        {
            bool result = true;
            if (Conn.GetValidationErrors().Count() > 0)
            {
                this.MySnackbar.MessageQueue.Enqueue( "Please input all required information");
                result = false;
                foreach (var item in Conn.GetValidationErrors())
                {
                    foreach (var error in item.ValidationErrors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }

            }
            return result;
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
