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
using System.Collections.ObjectModel;

namespace AvnConnect.Projects
{
    /// <summary>
    /// Interaction logic for ProjectViewer.xaml
    /// </summary>
    public partial class ProjectViewer : UserControl
    {
        private MainWindow MainWindow;
        private ConnectContainer projectConn;
        private List<Project> ProjectsLoaded;

        public CategoryItem ProjectCategories { get; private set; }
        public string NoCategoryKey { get; private set; }

        public ProjectViewer()
        {
            InitializeComponent();
            this.Loaded += ProjectViewer_Loaded;
        }

        private void ProjectViewer_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainWindow = (MainWindow)App.Current.MainWindow;
        }

        /// <summary>
        /// Load categories to treeview
        /// </summary>
        internal void LoadTreeView()
        {

            //Clear all items
            this.ProjectCategoryTree.Items.Clear();

            //Create the basic categories
            CreateBasicCategory();

            using (Data.ConnectContainer Conn = new ConnectContainer())
            {
                //Create new dbset (short live)
                var CategoryDbset = Conn.Set<Data.Category>();
                CategoryDbset.Load();

                //The root item
                this.ProjectCategories = new CategoryItem();
                this.ProjectCategories.Category = CategoryDbset.Local.Where(cate => cate.ParentKey == "").FirstOrDefault();
                this.FindChild(this.ProjectCategories, CategoryDbset.Local);

                //Attach to the tree
                this.ProjectCategoryTree.Items.Add(this.ProjectCategories);
                this.ProjectCategoryTree.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;


            }
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (this.ProjectCategoryTree.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated
                && this.ProjectCategoryTree.Items.Count > 0)
            {
                TreeViewItem ti = (TreeViewItem)this.ProjectCategoryTree.ItemContainerGenerator.ContainerFromIndex(0);
                ti.IsExpanded = true;
            }
        }



        /// <summary>
        /// Create the All Project category (the root) and the No Category
        /// </summary>
        private void CreateBasicCategory()
        {
            using (Data.ConnectContainer Conn = new ConnectContainer())
            {
                if (Conn.Categories.Count() == 0)
                {
                    try
                    {
                        string AllProjectKey = Encryption.GetUniqueKey(16);
                        Conn.Categories.Add(new Category()
                        {
                            Name = "All Projects",
                            Key = AllProjectKey,
                            AddedBy = this.MainWindow.StaffKey,
                            AddedOn = ExtendedFunctions.GetNetworkTime(),
                            Color = "DimGray",
                            IsDeleted = false,
                            ModifiedBy = this.MainWindow.StaffKey,
                            ModifiedOn = ExtendedFunctions.GetNetworkTime(),
                            ParentKey = "",
                            Level = 0
                        });
                        Conn.Categories.Add(new Category()
                        {
                            Name = "No Category",
                            Key = Encryption.GetUniqueKey(16),
                            AddedBy = this.MainWindow.StaffKey,
                            AddedOn = ExtendedFunctions.GetNetworkTime(),
                            Color = "DimGray",
                            IsDeleted = false,
                            ModifiedBy = this.MainWindow.StaffKey,
                            ModifiedOn = ExtendedFunctions.GetNetworkTime(),
                            ParentKey = AllProjectKey,
                            Level = 1

                        });

                        if (Conn.GetValidationErrors().Count() > 0)
                        {
                            foreach (var item in Conn.GetValidationErrors())
                            {
                                foreach (var error in item.ValidationErrors)
                                {
                                    Console.WriteLine(error.ErrorMessage);
                                }
                            }
                        }
                        else
                        {
                            Conn.SaveChanges();
                        }
                    }
                    finally
                    {
                        Conn.Dispose();
                    }
                }
            }
        }




        /// <summary>
        /// Create the tree like instance CategoryItem => become the root of the category tree
        /// </summary>
        /// <param name="projectCategories"></param>
        private void FindChild(CategoryItem projectCategories, ObservableCollection<Category> LocalCategoryDbSet)
        {
            Category g = projectCategories.Category;
            if (g.Name == "No Category")
            {
                this.NoCategoryKey = g.Key;
            }

            var children = LocalCategoryDbSet.Where(cate => cate.ParentKey == g.Key);
            foreach (var item in children)
            {
                CategoryItem childItem = new Projects.CategoryItem();
                childItem.Category = item;
                projectCategories.SubCategories.Add(childItem);
                FindChild(childItem, LocalCategoryDbSet);
            }
        }



        /// <summary>
        /// Show the manage dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryManageButton_Click(object sender, RoutedEventArgs e)
        {
            CategoriesManager mng = new CategoriesManager(this.ProjectCategories);
            mng.Closing += Mng_Closing;
            mng.NeedToReloadTree += Mng_NeedToReloadTree;
            this.MainWindow.ShowCustomDialog(mng);
        }



        /// <summary>
        /// Reload the category treeview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mng_NeedToReloadTree(object sender, EventArgs e)
        {
            this.LoadTreeView();
            CategoriesManager mng = sender as CategoriesManager;
            mng.RefreshTree(this.ProjectCategories);
        }


        #region DIALOGS MANAGEMENT

        //Gọi hàm đóng hộp thoại đang mở khi sự kiện closing xảy ra
        private void Mng_Closing(object sender, EventArgs e)
        {
            this.CloseShowingDialog();
        }

        /// Đóng hộp thoại đang mở
        private void CloseShowingDialog()
        {
            this.MainWindow.DialogHost.DialogContent = null;
            this.MainWindow.DialogHost.Visibility = Visibility.Collapsed;
            this.MainWindow.DialogHost.IsOpen = false;
        }

        #endregion

        #region ADDPROJECT

        //Gọi hộp thoại thêm dự án
        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowAddProjectDialog();
        }

        /// <summary>
        /// Gọi hộp thoại thêm dự án
        /// </summary>
        private void ShowAddProjectDialog()
        {
            AddNewProject AddDialog = new Projects.AddNewProject();
            AddDialog.SetTreeViewSource(this.ProjectCategories);
            AddDialog.NoCategoryKey = this.NoCategoryKey;
            AddDialog.Closed += Mng_Closing;
            AddDialog.AddingProjectFailed += AddDialog_AddingProjectFailed;
            AddDialog.ProjectAdded += AddDialog_ProjectAdded;
            this.MainWindow.ShowCustomDialog(AddDialog);
        }

        private void AddDialog_ProjectAdded(object sender, EventArgs e)
        {
            this.MySnackbar.MessageQueue.Enqueue("NEW PROJECT ADDED");
            this.CloseShowingDialog();
            this.LoadProjects();
        }

        private void AddDialog_AddingProjectFailed(object sender, EventArgs e)
        {
            this.MySnackbar.MessageQueue.Enqueue("UNEXPECTED ERROR WHILE ADDING NEW PROJECT");
            this.CloseShowingDialog();
        }


        #endregion

        #region LOAD PROJECT

        /// <summary>
        /// Tải danh sách các dự án
        /// </summary>
        internal void LoadProjects()
        {
            //Tạo kết nối riêng cho các dự án
            if (this.projectConn == null) this.projectConn = new ConnectContainer();

            //Lưu danh sách các dự án
            if (this.ProjectsLoaded == null) this.ProjectsLoaded = new List<Project>();

            //Tất cả các dự án liên quan đến người dùng hiện tại
            var projectsToLoad = projectConn.ProjectStaffs.Where(assign => assign.StaffKey == this.MainWindow.StaffKey).ToList();

            //Duyệt qua tất cả các dự án, nếu chưa thêm vào trang xem thì thêm vào
            foreach (ProjectStaffs item in projectsToLoad)
            {
                bool existed = (this.ProjectsLoaded.Where(pr => pr.Key == item.ProjectKey).FirstOrDefault() != null);
                if (!existed)
                {
                    var p = projectConn.Projects.Where(x => x.Key == item.ProjectKey).FirstOrDefault();
                    ProjectCard card = new ProjectCard();
                    card.MyProject = p;
                    this.ProjectsLoaded.Add(p);
                    ProjectList_ListBox.Items.Add(card);
                    card.RequestOpenDetail += Card_RequestOpenDetail; 
                }
            }
        }

        /// Mở chi tiết của dự án khi người dùng click vào tiêu đề
        private void Card_RequestOpenDetail(object sender, EventArgs e)
        {
            this.OpenProjectDetail((Project)sender);
        }

        /// Mở chi tiết của dự án
        private void OpenProjectDetail(Project pj)
        {
            ProjectDetailViewer detailViewer = new Projects.ProjectDetailViewer();
            detailViewer.MyProject = pj;
            this.ProjectOverviewGrid.Visibility = Visibility.Collapsed;
            this.ProjectDetailGrid.Visibility = Visibility.Visible;
            this.ProjectDetailGrid.Children.Add(detailViewer);
            detailViewer.GoBack += DetailViewer_GoBack;
        }

        /// Đóng trang xem chi tiết dự án, trở về danh sách
        private void DetailViewer_GoBack(object sender, EventArgs e)
        {
            this.ProjectOverviewGrid.Visibility = Visibility.Visible;
            this.ProjectDetailGrid.Visibility = Visibility.Collapsed;
            this.ProjectDetailGrid.Children.Remove((UIElement)sender);
        }

        #endregion
    }
}
