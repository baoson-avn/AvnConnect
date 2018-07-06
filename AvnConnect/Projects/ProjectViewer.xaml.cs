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

        public CategoryItem ProjectCategories { get; private set; }

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

            using (Data.ConnectContainer Conn = new ConnectContainer ())
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
            using (Data.ConnectContainer Conn = new ConnectContainer ())
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
            this.MainWindow.ShowDialog(mng);
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



        /// <summary>
        /// Close the manage dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mng_Closing(object sender, EventArgs e)
        {
            this.MainWindow.DialogHost.DialogContent = null;
            this.MainWindow.DialogHost.Visibility = Visibility.Collapsed;
            this.MainWindow.DialogHost.IsOpen = false;
        }
    }
}
