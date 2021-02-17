using System;
using System.Windows;
using System.Windows.Data;

namespace VagabondLib.ValueConverters
{
    /// <summary>
    /// Value Converter class to convert a Boolean value to a Visibility value
    /// <para>True converts to Visible; False converts to Collapsed</para>
    /// </summary>
    public class BoolToVisibility : IValueConverter
    {
        /// <summary>
        /// Convert from expected Boolean value to a Visibility Value
        /// <para>True converts to Visible; False converts to Collapsed</para>
        /// </summary>
        /// <param name="value">Boolean value to convert</param>
        /// <param name="targetType">Ignored</param>
        /// <param name="parameter">Ignored</param>
        /// <param name="culture">Ignored</param>
        /// <returns>Visibility value; defaults to Collapsed if unsuccessful</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var boolValue = value as bool?;
            if (boolValue.HasValue)
            {
                if (boolValue.Value)
                {
                    return Visibility.Visible;
                }
            }
            
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts from Visibility value to Boolean value
        /// <para>Visible converts to True; All other values convert to False</para>
        /// </summary>
        /// <param name="value">Visibility value to convert to boolean</param>
        /// <param name="targetType">Ignored</param>
        /// <param name="parameter">Ignored</param>
        /// <param name="culture">Ignored</param>
        /// <returns>Boolean value; Defaults to False</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var visibility = value as Visibility?;
            if (visibility.HasValue)
            {
                if (visibility.Value == Visibility.Visible)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
