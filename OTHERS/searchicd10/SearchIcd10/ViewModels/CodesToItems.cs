using SearchIcd10.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchIcd10.ViewModels
{
    /// <summary>
    /// Class used to take Codes retrieved and get ViewModel items from them
    /// </summary>
    public static class CodesToItems
    {
        private static readonly string m_SpecificityString = "Specificity";
        /// <summary>
        /// String used for the Specificity Divider
        /// </summary>
        public static string SpecificityString { get { return m_SpecificityString; } }

        /// <summary>
        /// Child Types that will be returned, along with the Divider Header expected to be used
        /// </summary>
        private static readonly KeyValuePair<string, string>[] ChildTypes
            = new KeyValuePair<string, string>[]
            {
                /*new KeyValuePair<string, string>(
                    IcdCodeStrings.ChildType_Direct,
                    SpecificityString),*/
                new KeyValuePair<string, string>(
                    IcdCodeStrings.ChildType_CodeAlso,
                    "Document Also"),
                new KeyValuePair<string, string>(
                    IcdCodeStrings.ChildType_CodeAdditional,
                    "Document Additionally"),
                new KeyValuePair<string, string>(
                    IcdCodeStrings.ChildType_CodeFirst,
                    "Document First")
            };

        /// <summary>
        /// Convert Child items into new ViewModel Items
        /// </summary>
        /// <param name="inItems">Children items to convert</param>
        /// <param name="parentVM">ListVM object that is the highest parent</param>
        /// <param name="itemParentVM">The parent Item's ViewModel</param>
        /// <returns>New range of ListItemVM objects for the children</returns>
        public static IEnumerable<ListItemVM> GetChildItems(IEnumerable<IcdCodeItem> inItems, SearchVM parentVM, ListItemVM itemParentVM)
        {
            var useParentTitle = new List<string>();
            if (itemParentVM != null)
            {
                useParentTitle.AddRange(itemParentVM.ParentTitle);
                useParentTitle.Add(itemParentVM.Title.Title);
            }

            var resultGroups = from child in inItems
                               group child by child.Code.ChildType;


            var directChildren = (from result in resultGroups
                                  where result.Key == IcdCodeStrings.ChildType_Direct
                                  from ret in result
                                  select ret).ToList();
            if (directChildren.Count > 0)
            {
                yield return new ListItemVM(0, IcdCodeItemMethods.NewDividerItem(SpecificityString), Enumerable.Empty<string>())
                {
                    ParentVM = parentVM,
                    ParentItemVM = itemParentVM
                };
                foreach (var child in directChildren)
                {
                    yield return new ListItemVM(0, child, useParentTitle)
                    {
                        ParentVM = parentVM,
                        ParentItemVM = itemParentVM
                    };
                }
                yield break;
            }

            foreach (var type in ChildTypes)
            {
                var currChildren = (from result in resultGroups
                                    where result.Key == type.Key
                                    from ret in result
                                    select ret).ToList();
                if (currChildren.Count > 0)
                {
                    yield return new ListItemVM(0, IcdCodeItemMethods.NewDividerItem(type.Value), Enumerable.Empty<string>())
                        {
                            ParentVM = parentVM,
                            ParentItemVM = itemParentVM
                        };
                    foreach (var child in currChildren)
                    {
                        yield return new ListItemVM(0, child, Enumerable.Empty<string>())
                            {
                                ParentVM = parentVM,
                                ParentItemVM = itemParentVM
                            };
                    }
                }
            }
        }
    }
}
