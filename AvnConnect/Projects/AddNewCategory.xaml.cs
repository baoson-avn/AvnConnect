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

namespace AvnConnect.Projects
{
    /// <summary>
    /// Interaction logic for AddNewCategory.xaml
    /// </summary>
    public partial class AddNewCategory : UserControl
    {
        private ConnectContainer conn;
        private bool IsEditing = false ;
        private Category EditingCategory;
        private CategoryItem rootCategory;

        public event EventHandler Closing;
        public event EventHandler Confirmed;
        public event EventHandler Failed;



        /// <summary>
        /// Edit category
        /// </summary>
        /// <param name="EditingCategory"></param>
        /// <param name="rootCategory"></param>
        public AddNewCategory(Category EditingCategory, CategoryItem rootCategory)
        {
            InitializeComponent();
            this.IsEditing = true;
            this.rootCategory = rootCategory;
            this.EditingCategory = EditingCategory;
            this.NameTextbox.Text = EditingCategory.Name;
            this.ConfirmButton.Content = "Update Category";
            this.Loaded += AddNewCategory_Loaded;
        }


        /// <summary>
        /// Add category
        /// </summary>
        /// <param name="rootCategory"></param>
        public AddNewCategory(CategoryItem rootCategory)
        {
            InitializeComponent();
            this.rootCategory = rootCategory;
            this.Loaded += AddNewCategory_Loaded;
        }


        private void AddNewCategory_Loaded(object sender, RoutedEventArgs e)
        {
            this.NestCombobox.Items.Clear();
            LoadCategory(rootCategory);
            if (this.NestCombobox.Items.Count > 0 && this.NestCombobox.SelectedIndex == -1)
            {
                this.NestCombobox.SelectedIndex = 0;
            }
            if (IsEditing)
            {
                this.colorPicker.PickColor(this.EditingCategory.Color);
            }
        }


        private void LoadCategory(CategoryItem ParentCategory)
        {
            
            string Display = "";
            for (int i = 0; i < ParentCategory.Category.Level; i++)
            {
                Display = "-" + Display;
            }

            if (ParentCategory.Category.Name != "No Category")
            {
                ComboBoxItem newCi = new ComboBoxItem()
                {
                    Content = Display + " " + ParentCategory.Category.Name,
                    Tag = ParentCategory.Category
                };

                if (IsEditing)
                {
                    if (ParentCategory.Category.Key == this.EditingCategory.ParentKey)
                    {
                        newCi.IsSelected = true;
                    }
                    if (ParentCategory.Category.Key == this.EditingCategory.Key)
                    {
                        return;
                    }
                    
                }
                this.NestCombobox.Items.Add(newCi);
            }

            foreach (var item in ParentCategory.SubCategories)
            {
                LoadCategory(item);
            }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Closing?.Invoke(this, EventArgs.Empty);
        }
        

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEditing)
            {
                this.AddNewCategoryToDb();
            } else
            {
                this.UpdateCategoryToDb();
            }
        }

        private void UpdateCategoryToDb()
        {
            try
            {
                MainWindow M = (MainWindow)App.Current.MainWindow;
                using (Data.ConnectContainer conn = new Data.ConnectContainer())
                {
                    var cate = conn.Categories.Where(c => c.Key == EditingCategory.Key).FirstOrDefault();
                    var Parent = ((ComboBoxItem)this.NestCombobox.SelectedItem).Tag as Category;

                    bool changed = false;

                    if (cate != null)
                    {
                        changed = changed || (cate.Name != NameTextbox.Text);
                        changed = changed || (cate.ParentKey != Parent.Key);
                        changed = changed || (cate.Color != this.colorPicker.CurrentSelectedColor.Color.ToString());
                    }

                    if (changed)
                    {
                        cate.Name = NameTextbox.Text;
                        cate.Color = this.colorPicker.CurrentSelectedColor.Color.ToString();
                        cate.ParentKey = Parent.Key;
                        cate.ModifiedBy = M.StaffKey;
                        cate.ModifiedOn = ExtendedFunctions.GetNetworkTime();
                        cate.Level = Parent.Level+1;
                    }
                    int i = conn.SaveChanges();
                    if (i > 0)
                    {
                        this.Confirmed?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        this.Failed?.Invoke(this, EventArgs.Empty);
                    }
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void AddNewCategoryToDb()
        {
            try
            {
                MainWindow M = (MainWindow)App.Current.MainWindow;
                using (Data.ConnectContainer conn = new Data.ConnectContainer())
                {
                    var cate = new Data.Category()
                    {
                        Name = NameTextbox.Text,
                        ModifiedBy = M.StaffKey,
                        ModifiedOn = ExtendedFunctions.GetNetworkTime(),
                        AddedBy = M.StaffKey,
                        AddedOn = ExtendedFunctions.GetNetworkTime(),
                        Color = this.colorPicker.CurrentSelectedColor.Color.ToString(),
                        IsDeleted = false,
                        Key = Encryption.GetUniqueKey(16)
                    };
                    var Parent = ((ComboBoxItem)this.NestCombobox.SelectedItem).Tag as Category;

                    cate.ParentKey = Parent.Key;
                    cate.Level = Parent.Level + 1;

                    conn.Categories.Add(cate);
                    int i = conn.SaveChanges();
                    if (i > 0)
                    {
                        this.Confirmed?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        this.Failed?.Invoke(this, EventArgs.Empty);
                    }
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
