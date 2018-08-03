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
using System.Windows.Controls.Primitives;
using MaterialDesignThemes.Wpf;
using System.Xml.Serialization;
using System.IO;

namespace AvnConnect.Projects
{
    /// <summary>
    /// Interaction logic for AddNewProject.xaml
    /// </summary>
    public partial class AddNewProject : UserControl
    {
        #region DEPENDENCY PROPERTIES

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(AddNewProject), new PropertyMetadata("Add project"));

        #endregion

        #region FIELDS
        private MainWindow MainWindow;
        private ConnectContainer Conn;
        private DbSet<Project> ProjectDbset;
        private DbSet<Staff> StaffDbset;
        private DbSet<Tags> TagsDbset;
        private DbSet<ProjectStaffs> ProjectStaffDbset;
        private DbSet<UserProjectPermission> ProjectPermissionDbset;
        private DbSet<Tags> RecentTagsDbset; 
        #endregion

        public List<Staff> SelectedStaff { get; set; }

        public Project NewProject { get; set; }

        public List<Tags> ListOfTags { get; set; }

        public string NoCategoryKey { get; set; }


        #region EVENTS
        public event EventHandler Closed;
        public event EventHandler ProjectAdded;
        public event EventHandler AddingProjectFailed;
        #endregion


        public AddNewProject()
        {
            InitializeComponent();
            this.DataContext = this;
            this.MainWindow = (MainWindow)App.Current.MainWindow;
            this.Conn = new ConnectContainer();
            this.LoadStaff();
            this.CreateEmptyProjectInstance();
        }

        private void CreateEmptyProjectInstance()
        {
            if (this.ProjectDbset == null)
            {
                this.ProjectDbset = this.Conn.Set<Data.Project>();
                this.NewProject = this.ProjectDbset.Create();
                this.NewProject.Key = Encryption.GetUniqueKey(16);
            }
        }

        private void LoadStaff()
        {
            if (this.StaffDbset == null)
            {
                this.StaffDbset = this.Conn.Set<Data.Staff>();
                this.StaffDbset.Load();
                this.UserListbox.ItemsSource = this.StaffDbset.Local;
            }
        }

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

        #region ADDING PROJECT

        /// <summary>
        /// Gọi hàm để thêm dự án khi người dùng chọn thêm 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddProjectToDb();
        }

        /// <summary>
        /// Thêm các thông tin cuối cùng và gọi framework để chèn dự án vào csdl
        /// </summary>
        private void AddProjectToDb()
        {
            //Thông tin về phân nhóm dự án
            SetCategory();

            //Thông tin về khách hàng (chưa làm)
            SetCustomer();

            //Thông tin về nhân sự của dự án
            SetPeople();

            //Thông tin về ngày của dự án
            SetDates();

            //Thông tin về các thẻ đính kèm;
            SetTags();

            //Thông tin về người tạo, giờ tạo
            this.NewProject.AddedBy = this.MainWindow.StaffKey;
            this.NewProject.AddedOn = ExtendedFunctions.GetNetworkTime();

            //Thông tin về người sửa, giờ sửa
            this.NewProject.ModifiedBy = this.NewProject.AddedBy;
            this.NewProject.ModifiedOn = this.NewProject.AddedOn;

            //Danh sách công việc mặc định
            this.CreateDefaultTaskList();

            //Kiểm tra lỗi 
            if (this.Conn.GetValidationErrors().Count() > 0)
            {
                foreach (var item in Conn.GetValidationErrors())
                {
                    Console.WriteLine(item.Entry.GetType().ToString());
                    foreach (var error in item.ValidationErrors)
                    {
                        Console.WriteLine("     " + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }
            }
            else
            {
                try
                {
                    //Thêm vào cơ sở dữ liệu
                    Conn.SaveChanges();

                    //Nếu thành công thì gọi event
                    this.ProjectAdded?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    //Nếu có lỗi thì ghi vào console và thông báo
                    Console.WriteLine(ex.Message);

                    //Raise event
                    this.AddingProjectFailed?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        private void CreateDefaultTaskList()
        {
            //Tất cả các nhóm
            ProjectTaskList AllList = this.Conn.ProjectTaskLists.Create();
            AllList.CreatedBy = NewProject.AddedBy;
            AllList.CreatedOn = NewProject.AddedOn;
            AllList.ListName = "All Tasks";
            AllList.ProjectKey = this.NewProject.Key;
            AllList.Key = Encryption.GetUniqueKey(16);
            this.Conn.ProjectTaskLists.Add(AllList);

            //Nhóm chung chung
            ProjectTaskList GeneralList = this.Conn.ProjectTaskLists.Create();
            GeneralList.CreatedBy = NewProject.AddedBy;
            GeneralList.CreatedOn = NewProject.AddedOn;
            GeneralList.ListName = "General Tasks";
            GeneralList.ProjectKey = this.NewProject.Key;
            GeneralList.Key = Encryption.GetUniqueKey(16);
            this.Conn.ProjectTaskLists.Add(GeneralList);
        }

        /// <summary>
        /// Thêm các thông tin về thẻ 
        /// </summary>
        private void SetTags()
        {
            if (this.ListOfTags != null && this.ListOfTags.Count > 0)
            {
                StringWriter writer = new StringWriter();
                new XmlSerializer(typeof(List<Data.Tags>)).Serialize(writer, this.ListOfTags);
                this.NewProject.Tag = writer.ToString();
            }

            this.ProjectDbset.Local.Add(NewProject);
        }

        /// <summary>
        /// Thêm các thông tin về ngày tháng
        /// </summary>
        private void SetDates()
        {
            this.NewProject.StartDate = this.StartDatePicker.SelectedDate;
            this.NewProject.EndDate = this.EndDatePicker.SelectedDate;
        }


        /// <summary>
        /// Thêm các thông tin về nhân sự dự án
        /// </summary>
        private void SetPeople()
        {

            //Create the staff dbset
            if (this.ProjectStaffDbset == null)
            {
                this.ProjectStaffDbset = this.Conn.Set<ProjectStaffs>();
            } else
            {
                this.ProjectStaffDbset.Local.Clear();
            }

            //Create the permission db set
            if (this.ProjectPermissionDbset == null)
            {
                this.ProjectPermissionDbset = this.Conn.Set<Data.UserProjectPermission>();
            }

            if (this.SelectedStaff == null)
            {
                this.SelectedStaff = new List<Data.Staff>();
                this.SelectedStaff.Add(MainWindow.LoggedInStaff);
            }

            //Create the assignment and permission
            foreach (Staff s in SelectedStaff)
            {            
                var newAssign = this.ProjectStaffDbset.Create();

                if (IsProjectOwner(s.Key))
                {
                    this.NewProject.ProjectOwner = s.Key;
                }

                newAssign.StaffKey = s.Key;
                newAssign.ProjectKey = this.NewProject.Key;
                newAssign.Key = Encryption.GetUniqueKey(16);
                newAssign.AddedBy = this.MainWindow.StaffKey;
                newAssign.AddedOn = ExtendedFunctions.GetNetworkTime().ToString();

                //Create a permission record
                UserProjectPermission permission = this.ProjectPermissionDbset.Create();
                permission.ProjectStaffKey = newAssign.Key;
                permission.IsAdmin = IsProjectOwner(newAssign.StaffKey);

                newAssign.PermissionKey = Encryption.GetPermisionKey(permission);

                this.ProjectPermissionDbset.Add(permission);
                this.ProjectStaffDbset.Add(newAssign);
            }
        }

        /// <summary>
        /// Thêm các thông tin về khách hàng của dự án
        /// </summary>
        private void SetCustomer()
        {
        }

        /// <summary>
        /// Thêm các thông tin về phân nhóm của dự án
        /// </summary>
        private void SetCategory()
        {
            if (this.ProjectCategoryTree.SelectedItem == null)
            {
                this.NewProject.Category = this.NoCategoryKey;
            }
            else
            {
                CategoryItem item = (CategoryItem)this.ProjectCategoryTree.SelectedItem;

                //IF the selected category is root then assign no category
                if (item.Category.ParentKey == "")
                {
                    this.NewProject.Category = this.NoCategoryKey;
                }
                else
                {
                    this.NewProject.Category = item.Category.Key;
                }
            }
            Console.WriteLine("Category selected, key={0}", this.NewProject.Category);
        }

        #endregion

        #region CHOOSE CATEGORY
        internal void SetTreeViewSource(CategoryItem projectCategories)
        {
            this.ProjectCategoryTree.Items.Add(projectCategories);
        }

        #endregion

        #region TAGS
        private void AddTagPopupButton_Opened(object sender, RoutedEventArgs e)
        {
            this.AddTagPopupButton.IsPopupOpen = true;
        }

        private void AddTagButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.TagsDbset == null)
            {
                this.TagsDbset = Conn.Set<Tags>();
            }
            if (this.ListOfTags == null)
            {
                this.ListOfTags = new List<Tags>();
            }

            //Detect if this tag is currently added to the project
            bool TagExist = this.ListOfTags.Where(lt => lt.TagName.ToLower() == this.NewTagTextBox.Text.ToLower().Trim()).FirstOrDefault() != null;

            if (TagExist)
            {
                this.AddTagPopupButton.IsPopupOpen = false;
                this.MySnackbar.MessageQueue.Enqueue("This tag is already added to project".ToUpper());
                return;
            }

            //Create the new tag
            var newTag = this.TagsDbset.Create();
            newTag.TagName = this.NewTagTextBox.Text;
            newTag.TagKey = Encryption.GetUniqueKey(16);
            newTag.Color = this.TagColorPicker.CurrentSelectedColor.Color.ToString();
            newTag.AddedBy = ((MainWindow)App.Current.MainWindow).StaffKey;
            newTag.AddOn = ExtendedFunctions.GetNetworkTime();

            MyTagChips tc = new MyTagChips(newTag);
            tc.Margin = new Thickness(3);
            tc.Deleting += Tc_Deleting;

            //Add to collections
            this.TagWrapPanel.Children.Add(tc);
            this.ListOfTags.Add(newTag);
            this.TagsDbset.Add(newTag);

            //Close the popup
            this.AddTagPopupButton.IsPopupOpen = false;
        }

        private void Tc_Deleting(object sender, EventArgs e)
        {
            MyTagChips mt = (MyTagChips)sender;
            this.TagWrapPanel.Children.Remove(mt);
            this.ListOfTags.Remove(mt.MyTag);
            this.TagsDbset.Remove(mt.MyTag);
        }


        private void NewTagTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            this.LoadRecentTags();

            if (this.NewTagTextBox.Text.Trim() != "")
            {
                this.AddTagButton.IsEnabled = true;
            }
            else
            {
                this.AddTagButton.IsEnabled = false;
            }
        }

        private void LoadRecentTags()
        {
            if (this.RecentTagsDbset == null)
            {
                this.RecentTagsDbset = this.Conn.Set<Data.Tags>();
                var recentTag = this.Conn.Tags.OrderByDescending(tag => tag.AddOn).Take(20).ToList();
                foreach (Tags t in recentTag)
                {
                    this.RecentTagsDbset.Attach(t);
                    this.RecentTagListBox.ItemsSource = this.RecentTagsDbset.Local;
                }
            }
        }
        #endregion

        #region PEOPLE

        //Select staff
        private void StaffSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                //User can only select staff by check a toggle button
                ToggleButton btn = (ToggleButton)sender;

                //Get the staff instance
                string staffkey = btn.Tag.ToString();
                var User = this.StaffDbset.Local.Where(st => st.Key == staffkey).FirstOrDefault();

                //Allow to be select as project owner
                bool _CItemExist = false;
                foreach (ComboBoxItem _item in this.ProjectOwner_Combobox.Items)
                {
                    Staff _staff = (Staff)_item.Tag;
                    if (_staff.Key == User.Key)
                    {
                        _CItemExist = true;
                        break;
                    }
                }
                if (!_CItemExist)
                {
                    ComboBoxItem _CItem = new ComboBoxItem();
                    _CItem.Content = User.Surname + " " + User.Firstname;
                    _CItem.Tag = User;
                    this.ProjectOwner_Combobox.Items.Add(_CItem);
                }


                //if found
                if (User != null)
                {
                    //Create the list of selected staffs and attach this list to ProjectOwner_Combobox
                    if (this.SelectedStaff == null)
                    {
                        this.SelectedStaff = new List<Staff>();
                    }

                    //Add user to selected list
                    this.SelectedStaff.Add(User);

                    //Create the chip and add to wrap panel
                    TwoTagsChip chip = new TwoTagsChip();
                    chip.Margin = new Thickness(3);
                    chip.Icon = btn.Content;
                    chip.Tag = User;
                    chip.Tag2 = btn;
                    chip.Content = User.Firstname;
                    chip.ToolTip = User.Surname + " " + User.Firstname;
                    chip.IsDeletable = true;
                    chip.DeleteClick += StaffChip_DeleteClick;
                    this.SelectedStaffsWrapPanel.Children.Add(chip);
                }
                else
                {
                    this.MySnackbar.MessageQueue.Enqueue("UNABLE TO SELECT USER");
                }
            }
            catch (Exception ex)
            {
                this.MySnackbar.MessageQueue.Enqueue("UNABLE TO SELECT USER");
                Console.WriteLine(ex.Message);
            }
        }

        //Remove staff from project when user un_select the listbox toggle button
        private void Staff_Unselected(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = (ToggleButton)sender;
            string staffkey = btn.Tag.ToString();

            bool canRemove = IsDeletable(staffkey);
            if (!canRemove)
            {
                btn.Checked -= StaffSelected;
                btn.IsChecked = true;
                btn.Checked += StaffSelected;
                return;
            }

            foreach (TwoTagsChip chip in SelectedStaffsWrapPanel.Children)
            {
                Data.Staff st = (Staff)chip.Tag;
                if (st.Key == staffkey)
                {
                    this.RemoveStaffChip(chip);
                    return;
                }
            }
        }

        //Remove staff from project when user click delete button of the chip
        private void StaffChip_DeleteClick(object sender, RoutedEventArgs e)
        {
            TwoTagsChip chip = (TwoTagsChip)sender;
            Staff t = (Staff)chip.Tag;
            bool canRemove = IsDeletable(t.Key);
            if (!canRemove) return;

            ToggleButton btn = (ToggleButton)chip.Tag2;
            btn.IsChecked = false;
            RemoveStaffChip(chip);
        }


        private bool IsDeletable(string StaffKey)
        {
            //IF only 1 user in project then cannot remove
            if (this.SelectedStaff .Count <= 1)
            {
                this.MySnackbar.MessageQueue.Enqueue("CANNOT REMOVE THE LAST USER");
                return false;
            }

            //if is project owner then cannot remove
            bool isProjectOwner = IsProjectOwner(StaffKey);
            
            if (isProjectOwner)
            {
                this.MySnackbar.MessageQueue.Enqueue("CANNOT REMOVE PROJECT OWNER");
                return false;
            } else
            {
                return true;
            }

        }

        private bool IsProjectOwner(string Staffkey)
        {
            if (this.ProjectOwner_Combobox.SelectedIndex < 0) return true;
            ComboBoxItem Item = (ComboBoxItem)this.ProjectOwner_Combobox.SelectedItem;
            string key = ((Staff)Item.Tag).Key;
            return (key == Staffkey);
        }



        //Remove the chips from the wrappanel
        private void RemoveStaffChip(TwoTagsChip chip)
        {
            var User = (Staff)chip.Tag;
            foreach (Staff item in this.SelectedStaff)
            {
                if (item.Key == User.Key)
                {
                    this.SelectedStaff.Remove(item);
                    break;
                }
            }

            foreach (ComboBoxItem item in this.ProjectOwner_Combobox.Items)
            {
                if (((Staff)item.Tag).Key == User.Key)
                {
                    this.ProjectOwner_Combobox.Items.Remove(item);
                    break;
                }
            }
            this.SelectedStaffsWrapPanel.Children.Remove(chip);
        }


        //Add current user to project
        private void StaffToggleButton_Initialized(object sender, EventArgs e)
        {
            ToggleButton Btn = (ToggleButton)sender;
            if (Btn.Tag != null && Btn.Tag.ToString() == this.MainWindow.StaffKey)
            {
                Btn.IsChecked = true;

                //Find the coresponding comboboxitem
                foreach (ComboBoxItem item in this.ProjectOwner_Combobox.Items)
                {
                    Staff t = (Staff)item.Tag;
                    if (t.Key == Btn.Tag.ToString())
                    {
                        item.IsSelected = true;
                    }
                }
            }
        }

        #endregion

    }
}
