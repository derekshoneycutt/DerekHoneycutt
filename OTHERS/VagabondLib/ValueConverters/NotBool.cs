using System;
using System.Windows.Data;

namespace VagabondLib.ValueConverters
{
    /// <summary>
    /// Performs a NOT operation on a Boolean Value; operates TwoWay with the same both ways
    /// </summary>
    public class NotBool : IValueConverter
    {
        private bool DoNot(object value)
        {
            var b = value as bool?;
            if (b.HasValue)
            {
                return !b.Value;
            }
            return false;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DoNot(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DoNot(value);
        }
    }
}
