using System;
using System.Windows.Data;

namespace VagabondLib.ValueConverters
{
    /// <summary>
    /// Converts between a String with a valid DateTime format and a DateTime value
    /// </summary>
    public class StringToDateTime : IValueConverter
    {
        /// <summary>
        /// Convert a string value into Date Time
        /// <para>Returns null if string is empty or whitespace; All other failed parsing returns Now</para>
        /// </summary>
        /// <param name="value">String value to convert</param>
        /// <param name="targetType">Ignored</param>
        /// <param name="parameter">Ignored</param>
        /// <param name="culture">Ignored</param>
        /// <returns>DateTime parsed from string. Null of string is empty or whitespace. Now if failed parsing otherwise</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = System.Convert.ToString(value);
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            else if (String.IsNullOrWhiteSpace(strValue))
            {
                return null;
            }
            return DateTime.Now;
        }

        /// <summary>
        /// Convert a DateTime object to a string
        /// <para>If failed, returns Empty string</para>
        /// </summary>
        /// <param name="value">DateTime value to convert to string</param>
        /// <param name="targetType">Ignored</param>
        /// <param name="parameter">Ignored</param>
        /// <param name="culture">Ignored</param>
        /// <returns>String representation of DateTime, or Empty String</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var resultDateTime = value as DateTime?;
            if (resultDateTime != null)
            {
                return resultDateTime.Value.ToString();
            }
            return String.Empty;
        }
    }
}
