using System;
using System.Windows.Data;

namespace VagabondLib.ValueConverters
{
    /// <summary>
    /// Converts a String and a Boolean value through Boolean.TryParse ; returns false if TryParse failed.
    /// <para>ConvertBack returns the Boolean.ToString of the passed value</para>
    /// </summary>
    public class StringToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = (string)value;
            bool ret = false;
            Boolean.TryParse(strValue, out ret);
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bValue = (bool)value;
            return bValue.ToString();
        }
    }
}
