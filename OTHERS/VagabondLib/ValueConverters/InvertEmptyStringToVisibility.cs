using System;
using System.Windows;
using System.Windows.Data;

namespace VagabondLib.ValueConverters
{
    /// <summary>
    /// Converts a string to a Visibility, based on String.IsNullOrEmpty
    /// <para>If NullOrEmpty, returns Visible. Otherwise Collapsed</para>
    /// <para>ConvertBack always returns Binding.DoNothing</para>
    /// </summary>
    public class InvertEmptyStringToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var strValue = value as string;

            if (String.IsNullOrEmpty(strValue))
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
            return Binding.DoNothing;
        }
    }
}
