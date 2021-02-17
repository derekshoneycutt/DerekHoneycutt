using ICD.DataAccess.ObjectModels;
using System.Collections.Generic;
using System.Linq;

namespace SearchIcd10.Data
{
    /// <summary>
    /// Enumerable wrapper to handle defferred once children of IcdCode objects to IcdCodeItem objects
    /// </summary>
    internal class DeferredOnceChildren : IEnumerable<IcdCodeItem>
    {
        /// <summary>
        /// The Child Items list : Should be null until first iteration; use directly instead of re-iteration if not null
        /// </summary>
        private List<IcdCodeItem> items;
        /// <summary>
        /// IcdCode item to act as parent
        /// </summary>
        private IcdCode parent;
        /// <summary>
        /// ICD Data Provider Object to query for children
        /// </summary>
        private DataProviders.IAppDataProvider dataProvider;

        /// <summary>
        /// Initiate a new DeferredOnceChildren object with necessary data
        /// </summary>
        /// <param name="fromParent">Parent IcdCode object to get children from</param>
        /// <param name="useProvider">Data Provider to get the data from</param>
        public DeferredOnceChildren(IcdCode fromParent, DataProviders.IAppDataProvider useProvider)
        {
            items = null;
            parent = fromParent;
            dataProvider = useProvider;
        }

        /// <summary>
        /// Get the enumerator for the children
        /// On first run, it retrieves the children; all initial runs use the previously retrieved children
        /// </summary>
        /// <returns>Enumerator for IcdCodeItem collection</returns>
        public IEnumerator<IcdCodeItem> GetEnumerator()
        {
            if (items == null)
            {
                items = (from child in dataProvider.GetChildren(parent.Code, parent.CodeType)
                         select child.ToIcdCodeItem(dataProvider))
                         .ToList();
                parent = null;
                dataProvider = null;
            }
            return items.GetEnumerator();
        }

        /// <summary>
        /// Get the enumerator for the children. Explicit.
        /// On first run, it retrieves the children; all initial runs use the previously retrieved children
        /// </summary>
        /// <returns>Enumerator for IcdCodeItem collection</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
