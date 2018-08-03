using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AvnConnect
{
    namespace Converter
    {
        public class BooleanToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value.GetType() == typeof(bool))
                {
                    bool b = (bool)value;
                    if (b) return Visibility.Visible;
                    else return Visibility.Collapsed;
                }
                return DependencyProperty.UnsetValue;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class EmptyStringToEnableConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value.ToString().Trim() != "";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class EmptyStringToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value.ToString().Trim() == "")
                {
                    return Visibility.Collapsed;
                } else
                {
                    return Visibility.Visible;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class SelectedToBackgroundBrush : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool b = (bool)value;
                if (b)
                {
                    return new SolidColorBrush(Colors.WhiteSmoke);
                } else
                {
                    return new SolidColorBrush(Colors.White);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class SelectedToForegroundBrush : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool b = (bool)value;
                if (b)
                {
                    return new SolidColorBrush(Color.FromRgb(33, 150, 243));
                }
                else
                {
                    return new SolidColorBrush(Colors.Gray);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class EmptyStringToRedBorderBrush : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value.ToString() == "")
                {
                    return new SolidColorBrush(Colors.Red);
                } else
                {
                    return new SolidColorBrush(Colors.Green);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class EmptySelectionToRedBorderBrush : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int index = (int)value;
                if (index >= 0)
                {
                    return new SolidColorBrush(Colors.Green);
                } else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class AccessLevelToVisibilityConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                bool visible = true;

                string toFilter = "";

                if (values[2]!= null) toFilter += values[2].ToString();
                if (values[3] != null) toFilter += values[3].ToString();
                if (values[4] != null) toFilter += values[4].ToString();

                if (values[0] != null && values[0].ToString() != "")
                {
                    //dang filter theo textbox
                    visible = visible && toFilter.ToLower().Contains(values[0].ToString().ToLower());
                }

                if (values[1] != null && values[1].ToString() != "")
                {
                    //dang filter theo alphabet
                    visible = visible && (toFilter[0].ToString().ToUpper() == values[1].ToString());
                }
 
                if (visible)
                {
                    return Visibility.Visible;
                } else
                {
                    return Visibility.Collapsed;
                }
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class PermissionToButtonEnableConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string UserKey = value.ToString();
                MainWindow M = (MainWindow)App.Current.MainWindow;
                if (UserKey == M.StaffKey) return true;
                return M.CanManageUser;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class CategoryNameToAbility : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value.ToString() == "All Projects") return Visibility.Collapsed;
                if (value.ToString() == "No Category") return Visibility.Collapsed;
                return Visibility.Visible;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class CategoryLevelToFontStyle : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int lvl = (int)value;

                switch (lvl)
                {
                    case 0:
                        return App.Current.MainWindow.FindResource("MaterialDesignSubheadingTextBlock");
                        break;
                    case 1:
                        return App.Current.MainWindow.FindResource("MaterialDesignBody2TextBlock");
                        break;
                    default:
                        return App.Current.MainWindow.FindResource("MaterialDesignBody1TextBlock");
                        break;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class GetFirstCharacter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value.ToString()[0].ToString();
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class GetAvatarConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (values[1] == null || values[1].ToString()== "")
                {
                    return values[0].ToString()[0].ToString();
                } else
                {
                    try
                    {
                        using (System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Convert.FromBase64String(values[1].ToString())))
                        {
                            PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                            BitmapFrame frame = decoder.Frames.First();
                            frame.Freeze();
                            System.Windows.Controls.Image m = new System.Windows.Controls.Image();
                            m.Source = frame;
                            return m;
                        }
                    }
                    catch (Exception)
                    {
                        MaterialDesignThemes.Wpf.PackIcon icon = new MaterialDesignThemes.Wpf.PackIcon();
                        icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Account;
                        return icon;
                    }
                }
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

        }

        public class ProjectDateToText : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (values[0] == null && values[1] == null) return "Not set";
                string s = "";
                if (values[0] != null)
                {
                    if (values[0].GetType() == typeof(DateTime))
                    {
                        DateTime from = (DateTime)values[0];
                        s = s + "From: " + from.ToShortDateString() + " ";
                    }  
                }
                if (values[1] != null)
                {
                    if (values[1].GetType() == typeof(DateTime))
                    {
                        if (s.Length > 0)
                        {
                            s += "\n";
                        }
                        DateTime To = (DateTime)values[1];
                        s = s + "To: " + To.ToShortDateString() + " ";
                    }
                }
                return s;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }


        public class SelectedTaskListToText : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value != null && value.GetType() == typeof(Data.ProjectTaskList))
                {
                    Data.ProjectTaskList pj = (Data.ProjectTaskList)value;
                    return pj.ListName;
                }
                return "Tasks";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
