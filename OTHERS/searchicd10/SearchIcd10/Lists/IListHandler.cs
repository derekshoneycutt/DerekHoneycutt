using System.Collections.Generic;

namespace SearchIcd10.Lists
{
    /// <summary>
    /// Interface describing objects used to get a collection of IcdCodeItem objects that form a list
    /// </summary>
    public interface IListHandler
    {
        /// <summary>
        /// Get the items for the desired list
        /// </summary>
        /// <param name="provider">Data Provider used to retrieve the items</param>
        /// <param name="showCode">Whether to show the code in the title</param>
        /// <returns>A new collection of ListItemVM objects making up the desired list</returns>
        IEnumerable<ViewModels.ListItemVM> GetItemVMs(DataProviders.IAppDataProvider provider, bool showCode, ViewModels.SearchVM parentVM);
    }
}
