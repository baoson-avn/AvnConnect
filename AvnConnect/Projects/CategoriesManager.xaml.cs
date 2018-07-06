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
    /// Interaction logic for CategoriesManager.xaml
    /// </summary>
    public partial class CategoriesManager : UserControl
    {
        private CategoryItem RootCategory;

        public event EventHandler Closing;
        public event EventHandler NeedToReloadTree;
        public bool NeedToRefreshTree { get; private set; }


        public CategoriesManager(CategoryItem ProjectCategories)
        {
            InitializeComponent();
            this.NeedToRefreshTree = false;
            this.RootCategory = ProjectCategories;
            this.ProjectCategoryTree.Items.Add(this.RootCategory);
       
        }

        public void Notify(string message)
        {
            this.MySnackbar.MessageQueue.Enqueue(message);
        }

        internal void RefreshTree(CategoryItem projectCategories)
        {
            this.ProjectCategoryTree.Items.Clear();
            this.ProjectCategoryTree.Items.Add(projectCategories);
            this.RootCategory = projectCategories;
        }

        #region ADD NEW CATEGORY
        private void AddNewCategory_Clicked(object sender, EventArgs e)
        {
            AddNewCategory newCategoryDialog = new AddNewCategory(this.RootCategory);
            newCategoryDialog.Closing += NewCategoryDialog_Closing;
            newCategoryDialog.Confirmed += NewCategoryDialog_Confirmed;
            this.ShowDialog(newCategoryDialog);
        }

        private void NewCategoryDialog_Confirmed(object sender, EventArgs e)
        {
            AddNewCategory anc = (AddNewCategory)sender;
            this.Notify("CATEGORY \"" + anc.NameTextbox.Text + "\" ADDED");
            CloseDialog();
            this.NeedToReloadTree?.Invoke(this, EventArgs.Empty);
        }

        private void NewCategoryDialog_Closing(object sender, EventArgs e)
        {
            CloseDialog();
        }

        #endregion

        #region DIALOGS SHOW/HIDE
        internal void ShowDialog(object Content)
        {
            this.MydialogHost.DialogContent = Content;
            this.MydialogHost.Visibility = Visibility.Visible;
            this.MydialogHost.IsOpen = true;
        }

        private void CloseDialog()
        {
            this.MydialogHost.Visibility = Visibility.Collapsed;
            this.MydialogHost.IsOpen = false;
            this.MydialogHost.DialogContent = null;
        }
        #endregion

        #region DELETE CATEGORY

        private void DeleteCategory_Click(object sender, EventArgs e)
        {
            MyTextBlock mt = (MyTextBlock)sender;
            if (mt.Tag != null)
            {
                Data.Category category = (Data.Category)mt.Tag;
                Dialogs.ConfirmDialog Cfm = new Dialogs.ConfirmDialog("Are you sure want to delete category: \n" + category.Name, Dialogs.ConfirmDialog.DialogTypes.YesNo);
                Cfm.Header = "Confirm Delete Category";
                Cfm.Tag = category.Key;
                Cfm.Closing += DeleteCfmClosing;
                this.ShowDialog(Cfm);
            }
        }

        private void DeleteCfmClosing(object sender, EventArgs e)
        {
            Dialogs.ConfirmDialog Cfm = sender as Dialogs.ConfirmDialog;
            if (Cfm.DialogResult == Dialogs.ConfirmDialog.DialogResults.Yes)
            {
                this.tryDeleteCategory(Cfm.Tag.ToString());
            }
            this.CloseDialog();
        }

        private void tryDeleteCategory(string Key)
        {
            using (Data.ConnectContainer Conn = new Data.ConnectContainer())
            {
                try
                {
                    var categoryToDelete = Conn.Categories.Where(c => c.Key == Key).FirstOrDefault();
                    string name = categoryToDelete.Name;
                    if (categoryToDelete != null)
                    {
                        Conn.Categories.Remove(categoryToDelete);
                        var i = Conn.SaveChanges();
                        if (i >= 1)
                        {
                            this.Notify("CATEGORY:\"" + name.ToUpper() + "\" DELETED");
                            this.NeedToReloadTree?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            this.Notify("ERROR:\"" + name.ToUpper() + "\" NOT DELETED");
                        }
                    }
                    else
                    {
                        this.Notify("0 CATEGORY DELETED");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.Notify("ERROR:\"" + ex.Message + "\"");
                }
                finally
                {
                    Conn.Dispose();
                }
            }
        }

        #endregion

        #region CLOSE THIS DIALOG

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            this.Closing?.Invoke(this, EventArgs.Empty);
        }

        private void CloseButton_Click(object sender, MouseButtonEventArgs e)
        {
            this.Closing?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region EDIT CATEGORY
        private void EditCategory_Click(object sender, EventArgs e)
        {
            MyTextBlock mt = (MyTextBlock)sender;
            if (mt.Tag != null)
            {
                Data.Category category = (Data.Category)mt.Tag;
                AddNewCategory newCategoryDialog = new Projects.AddNewCategory(category, this.RootCategory);
                newCategoryDialog.Closing += NewCategoryDialog_Closing;
                newCategoryDialog.Confirmed += NewCategoryDialog_UpdateConfirmed;
                this.ShowDialog(newCategoryDialog);
            }
        }


        private void NewCategoryDialog_UpdateConfirmed(object sender, EventArgs e)
        {
            AddNewCategory anc = (AddNewCategory)sender;
            this.Notify("CATEGORY \"" + anc.NameTextbox.Text + "\" UPDATED");
            CloseDialog();
            this.NeedToReloadTree?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
