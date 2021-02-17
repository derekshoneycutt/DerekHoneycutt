using System;
using System.Windows;
using System.Windows.Data;

namespace VagabondLib.ValueConverters
{
    /// <summary>
    /// Converts between an Enum value and a Boolean value
    /// </summary>
    public class EnumToBoolean : IValueConverter
    {
        /// <summary>
        /// Convert from an enum value to a boolean value
        /// <para>True if value and parameter match; false or DependencyProperty.UnsetValue otherwise</para>
        /// </summary>
        /// <param name="value">Enum value to convert to the boolean value</param>
        /// <param name="targetType">Ignored</param>
        /// <param name="parameter">Expected string representation of enum value to match for True</param>
        /// <param name="culture">Ignored</param>
        /// <returns>True if value and parameter match; false or DependencyProperty.UnsetValue otherwise</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var parameterString = parameter as string;
            if (parameterString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return DependencyProperty.UnsetValue;
            }

            var parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        /// <summary>
        /// Convert from a boolean to an Enum value, if appropriate
        /// </summary>
        /// <param name="value">Boolean value to convert</param>
        /// <param name="targetType">Enum type to convert to</param>
        /// <param name="parameter">String representation of value to set, if value is true</param>
        /// <param name="culture">Ignored</param>
        /// <returns>Enum value requested in parameter, if value is true; otherwise returns DependencyProperty.UnsetValue</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var parameterString = parameter as string;
            if ((parameterString == null) || (value.Equals(false)))
            {
                return DependencyProperty.UnsetValue;
            }

            return Enum.Parse(targetType, parameterString);
        }
    }
}
