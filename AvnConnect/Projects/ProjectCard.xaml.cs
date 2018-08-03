using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace AvnConnect.Projects
{

   

    /// <summary>
    /// Interaction logic for ProjectCard.xaml
    /// </summary>
    public partial class ProjectCard : UserControl, INotifyPropertyChanged
    {
        public Data.Project MyProject
        {
            get { return (Data.Project)GetValue(MyProjectProperty); }
            set
            {
                SetValue(MyProjectProperty, value);
                NotifyPropertyChanged();
            }
        }
        public static readonly DependencyProperty MyProjectProperty =
            DependencyProperty.Register("MyProject", typeof(Data.Project), typeof(ProjectCard), new PropertyMetadata(null));

        private List<Tags> ListOfTag;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestOpenDetail;


        public ProjectCard()
        {
            InitializeComponent();
            this.PropertyChanged += ProjectCard_PropertyChanged;
            this.DataContext = this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectCard_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MyProject")
            {
                this.LoadTags();
                if (this.MyProject.Description != null && this.MyProject.Description != "")
                {
                    this.DescriptionIcon.Foreground = (SolidColorBrush)this.FindResource("PrimaryHueMidBrush");
                } else
                {
                    this.MyProject.Description = "No description";
                }
            }
        }

        /// <summary>
        /// Load tags from xml
        /// </summary>
        private void LoadTags()
        {
            if (this.MyProject.Tag != null && this.MyProject.Tag != "")
            {
                this.ListOfTag = new List<Data.Tags>();

                using (System.IO.TextReader reader = new StringReader(this.MyProject.Tag))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Data.Tags>));
                    this.ListOfTag = (List<Data.Tags>)serializer.Deserialize(reader);
                    if (this.ListOfTag.Count > 0)
                    {
                        foreach (var tags in this.ListOfTag)
                        {
                            MyTagChips tc = new AvnConnect.MyTagChips(tags);
                            tc.IsDeletable = false;
                            tc.Margin = new Thickness(3);
                            this.ListOfTagsPanel.Children.Add(tc);
                        }
                    }

                }
            }
        }

        private void ProjectTitleClicked(object sender, EventArgs e)
        {
            this.OpenDetail();
        }

        private void OpenDetail()
        {
            this.RequestOpenDetail?.Invoke(this.MyProject, EventArgs.Empty);
        }
    }
}
