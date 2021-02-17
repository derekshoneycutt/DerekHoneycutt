using System;
using System.Windows;
using System.Windows.Data;

namespace VagabondLib.ValueConverters
{
    /// <summary>
    /// Attempts to convert between a string and a bool value to a Visibility value
    /// <para>Always expects a string value followed by a bool value; no more, no less</para>
    /// <para>If string is empty or bool is false, return Collapsed; otherwise (String is not empty and bool is true) returns Visible</para>
    /// </summary>
    public class EmptyStringAndBoolToVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = (string)values[0];
            bool doShow = (bool)values[1];

            if (String.IsNullOrEmpty((string)str))
            {
                return Visibility.Collapsed;
            }
            else
            {
                if (doShow)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
