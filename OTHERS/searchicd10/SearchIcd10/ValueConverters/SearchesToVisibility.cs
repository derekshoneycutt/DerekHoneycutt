using SearchIcd10.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SearchIcd10.ValueConverters
{
    /// <summary>
    /// Converts a collection of SearchVM object to a Visibility, based on String.NullOrWhitespace of CurrentText on all items
    /// <para>If no items or no item with text that is not NullOrWhitespace, returns Collapsed. Otherwise Visible</para>
    /// <para>ConvertBack always returns 2 Binding.DoNothing objects</para>
    /// <para>Requires the first value to be Collection of SearchVM objects, but ignores all others</para>
    /// </summary>
    public class SearchesToVisibility : IMultiValueConverter
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
                            return Visibility.Visible;
                        }
                    }
                }
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
