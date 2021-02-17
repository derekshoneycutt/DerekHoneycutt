using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SearchIcd10.ValueConverters
{
    /// <summary>
    /// Converts an IEnumerable of ViewModels.ListItemVM to Visibility value, with a Nullable Boolean flag included as well
    /// <para>First value should always be the IEnumerable and Second always a Nullable Boolean</para>
    /// <para>If More than 1 item in IEnumerable and Boolean is True, converts to Visible. Otherwise, Collapsed</para>
    /// <para>Does not ConvertBack at all</para>
    /// </summary>
    public class ChildrenBoolToVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var itemEnum = values[0] as IEnumerable<ViewModels.ListItemVM>;
            var boolVal = values[1] as bool?;
            if ((itemEnum != null) && (boolVal != null))
            {
                if ((itemEnum.Count() > 0) && boolVal.Value)
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
