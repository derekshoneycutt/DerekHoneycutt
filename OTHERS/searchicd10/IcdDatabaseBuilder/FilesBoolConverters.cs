using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IcdDatabaseBuilder
{
    /// <summary>
    /// Converts a Boolean value to a System.Windows.Visibility value (true => Visible ; false => Collapsed)
    /// </summary>
    public class BoolToFileVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((Visibility)value)
            {
                case Visibility.Visible:
                    return true;
                case Visibility.Collapsed:
                case Visibility.Hidden:
                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// Converts a Boolean value to a System.Windows.Visibility value (true => Collapsed ; false => Visible)
    /// </summary>
    public class BoolToNotFileVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((Visibility)value)
            {
                case Visibility.Visible:
                    return false;
                case Visibility.Collapsed:
                case Visibility.Hidden:
                default:
                    return true;
            }
        }
    }
}
