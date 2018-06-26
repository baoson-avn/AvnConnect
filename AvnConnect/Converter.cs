using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

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
    }
}
