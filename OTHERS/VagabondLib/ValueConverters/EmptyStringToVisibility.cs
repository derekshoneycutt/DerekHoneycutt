using System;
using System.Windows;
using System.Windows.Data;

namespace VagabondLib.ValueConverters
{
    /// <summary>
    /// Converts a string to a Visibility, based on String.IsNullOrEmpty
    /// <para>If NullOrEmpty, returns Collapsed. Otherwise Visible</para>
    /// <para>ConvertBack always returns Binding.DoNothing</para>
    /// </summary>
    public class EmptyStringToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (String.IsNullOrEmpty((string)value))
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
            return Binding.DoNothing;
        }
    }
}
