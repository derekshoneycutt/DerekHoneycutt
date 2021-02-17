using SearchIcd10.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchIcd10.Lists
{
    /// <summary>
    /// List Handler class to support the special lists stored in the Database
    /// </summary>
    public sealed class DbStoredList : IListHandler
    {
        /// <summary>
        /// Gets or Sets the Identifier of the list to retrieve from the database
        /// </summary>
        public string ListId { get; set; }

        /// <summary>
        /// Initializes a new DbStoredList object without a valid ListId property
        /// </summary>
        public DbStoredList()
        {
            ListId = String.Empty;
        }

        /// <summary>
        /// Initializes a new DbStoredList object with an expected List Identifier
        /// </summary>
        /// <param name="listId"></param>
        public DbStoredList(string listId)
        {
            ListId = listId;
        }

        /// <summary>
        /// Get the items for the expected list from the given provider
        /// </summary>
        /// <param name="provider">Provider to get the list from</param>
        /// <param name="showCode">Whether to show codes on the items</param>
        /// <returns>A new collection of ListItemVM objects containing the list; Empty if none available or invalid ListId property</returns>
        public IEnumerable<ViewModels.ListItemVM> GetItemVMs(DataProviders.IAppDataProvider provider, bool showCode, ViewModels.SearchVM parentVM)
        {
            if (String.IsNullOrWhiteSpace(ListId))
            {
                return Enumerable.Empty<ViewModels.ListItemVM>();
            }

            return provider.GetList(ListId).Select((c, i) =>
                new ViewModels.ListItemVM(i + 1, c.ToIcdCodeItem(provider), Enumerable.Empty<string>(), showCode)
                {
                    ParentVM = parentVM
                });
        }
    }
}
