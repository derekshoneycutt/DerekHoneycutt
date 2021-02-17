using SearchIcd10.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SearchIcd10.ValueConverters
{
    /// <summary>
    /// Class to convert a collection of SearchVM objects to an integer for Rowspan value on the ScrollViewer on the ListsWindow.
    /// <para>If any items with a result string exist, returns 1. Otherwise 3.</para>
    /// <para>ConvertBack always returns 2 Binding.DoNothing objects</para>
    /// <para>Requires the first value to be Collection of SearchVM objects, but ignores all others</para>
    /// </summary>
    public class SearchesToRowspan : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length >= 1)
            {
                var items = values[0] as Collection<SearchVM>;
                if (items != null)
                {
                    foreach (var search in items)
                    {
                        if (!String.IsNullOrWhiteSpace(search.CurrentText))
                        {
                            return 1;
                        }
                    }
                }
            }

            return 3;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
