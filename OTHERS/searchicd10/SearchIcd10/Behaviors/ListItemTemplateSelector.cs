using SearchIcd10.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SearchIcd10.Behaviors
{
    /// <summary>
    /// Class used to determine the appropriate Template to be used on the ListsWindow
    /// <para>Requires the following Template parameters:</para>
    /// <para>CodeItemTemplate, MoreButtonItemTemplate, DividerItemTemplate</para>
    /// </summary>
    public class ListItemTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var element = container as FrameworkElement;

            var fullItem = item as ViewModels.ListItemVM;
            if ((element != null) && (fullItem != null))
            {
                if (String.Equals(fullItem.Model.Code.CodeType, IcdCodeStrings.CodeType_Diagnosis)
                                || String.Equals(fullItem.Model.Code.CodeType, IcdCodeStrings.CodeType_Procedure))
                {
                    return element.FindResource("CodeItemTemplate") as DataTemplate;
                }
                else if (String.Equals(fullItem.Model.Code.CodeType, Lists.SearchList.MoreItemType))
                {
                    return element.FindResource("MoreButtonItemTemplate") as DataTemplate;
                }
                else
                {
                    return element.FindResource("DividerItemTemplate") as DataTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
