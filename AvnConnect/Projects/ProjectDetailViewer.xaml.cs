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
    public partial class ProjectDetailViewer : UserControl
    {

        #region DEFINE PROPERTIES

        public Data.Project MyProject
        {
            get { return (Data.Project)GetValue(MyProjectProperty); }
            set {
                SetValue(MyProjectProperty, value);
                this.LoadInfomations(); }
        }
        // Using a DependencyProperty as the backing store for MyProject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyProjectProperty =
            DependencyProperty.Register("MyProject", typeof(Data.Project), typeof(ProjectViewer), new PropertyMetadata(null));

        private ConnectContainer TaskListConn;
        private DbSet<ProjectTaskList> TaskListDbset;
        private MainWindow MainWindow;

        #endregion





        public ProjectDetailViewer()
        {
            InitializeComponent();
            this.DataContext = this;
            this.MainWindow = (MainWindow)App.Current.MainWindow;
        }


        /// <summary>
        /// Tải các thông tin chi tiết của dự án
        /// </summary>
        private void LoadInfomations()
        {
            this.LoadTaskList();
        }

        /// <summary>
        /// Tải danh sách nhóm các công việc
        /// </summary>
        private void LoadTaskList()
        {
            if (this.TaskListConn == null) this.TaskListConn = new Data.ConnectContainer();
            if (this.TaskListDbset == null) this.TaskListDbset = this.TaskListConn.Set<Data.ProjectTaskList>();

            //Danh sách các nhóm công việc thuộc dự án này
            var list = this.TaskListConn.ProjectTaskLists.Where(tl => tl.ProjectKey == this.MyProject.Key).ToList();

            //Tải vào dbset
            foreach (var item in list)
            {
                this.TaskListDbset.Attach(item);
            }

            //Thêm dbset làm nguồn dữ liệu
            this.ProjectList_ListBox.ItemsSource = this.TaskListDbset.Local;
        }

        private void NewTaskList_Click(object sender, RoutedEventArgs e)
        {
            this.AddNewTaskList();
        }

        /// <summary>
        /// Thêm một danh sách công việc mới
        /// </summary>
        private void AddNewTaskList()
        {
            var _newTaskList = this.TaskListDbset.Create();
            AddNewProjectTaskList adddialog = new Projects.AddNewProjectTaskList();
            adddialog.MyList = _newTaskList;
            adddialog.Adding += Adddialog_Adding;
            adddialog.Closed += Adddialog_Closed;
            this.MainWindow.ShowCustomDialog(adddialog);
        }

        private void Adddialog_Closed(object sender, EventArgs e)
        {
            this.MainWindow.CloseCustomDialog();
        }

        private void Adddialog_Adding(object sender, EventArgs e)
        {
            AddNewProjectTaskList adddialog = (AddNewProjectTaskList)sender;
            adddialog.MyList.CreatedBy = this.MainWindow.StaffKey;
            adddialog.MyList.CreatedOn = ExtendedFunctions.GetNetworkTime();
            adddialog.MyList.ProjectKey = this.MyProject.Key;
            adddialog.MyList.Key = Encryption.GetUniqueKey(16);
            this.TaskListDbset.Add(adddialog.MyList);

            if (this.TaskListConn.GetValidationErrors().Count() > 0)
            {
                foreach (var item in TaskListConn.GetValidationErrors())
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
                    TaskListConn.SaveChanges();
                }
                catch (Exception ex)
                {
                    //Nếu có lỗi thì ghi vào console và thông báo
                    Console.WriteLine(ex.Message);
                }
            }
        }


        #region NAVIGATION
        public event EventHandler GoBack;
        private void MyTextBlock_Click(object sender, EventArgs e)
        {
            this.GoBack?.Invoke(this, EventArgs.Empty);
        }
        #endregion


       
    }
}
