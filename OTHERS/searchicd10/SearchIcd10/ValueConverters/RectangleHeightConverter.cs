using System;
using System.Windows.Data;

namespace SearchIcd10.ValueConverters
{
    /// <summary>
    /// Converter class to make the Rectangle for the ScrollViewer on the ListWindow slightly smaller than the scrollviewer size itself
    /// </summary>
    public class RectangleHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var dblValue = value as double?;
            if (dblValue != null)
            {
                return dblValue.Value - 35;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
