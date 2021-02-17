using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SearchIcd10.ValueConverters
{
    /// <summary>
    /// Convert multiple Boolean values to a Visibility value
    /// <para>If any Boolean value is not True, returns Collapsed. Else returns Visible.</para>
    /// <para>Is only meant for OneWay binding</para>
    /// </summary>
    public class MultiBoolToVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var bools = values.OfType<bool>();
            if (bools.Any(b => !b))
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }
    }
}
