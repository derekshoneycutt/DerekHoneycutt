using SearchIcd10.Data;
using SearchIcd10.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchIcd10.Lists
{
    /// <summary>
    /// Class used to get a list from a Search on a Data Provider
    /// </summary>
    public sealed class SearchList : IListHandler
    {
        private static readonly int DefaultTake = 50;
        private static readonly string m_MoreItemType = "MoreItems";
        /// <summary>
        /// Item type used for a "More..." Button on the list
        /// </summary>
        public static string MoreItemType { get { return m_MoreItemType; } }

        /// <summary>
        /// Gets or Sets the terms to search for (processed by the Data Provider)
        /// </summary>
        public string SearchFor { get; set; }

        /// <summary>
        /// How many items to take at a given time
        /// </summary>
        public int TakeAtATime { get; set; }

        /// <summary>
        /// How many items should be skipped from the initial list
        /// <para>This is updated with each call for more items</para>
        /// </summary>
        public int Skip { get; set; }

        private string lastRetrievedFrom;

        /// <summary>
        /// Types that the search will return, along with the Divider Header expected to be used
        /// </summary>
        private static readonly KeyValuePair<string, string>[] SearchTypes
            = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>(
                    IcdCodeStrings.RetrievedFrom_WholeWord,
                    "Whole Word Exact Matches"),
                new KeyValuePair<string, string>(
                    IcdCodeStrings.RetrievedFrom_WholeWordSimilar,
                    "Whole Word Similar Matches"),
                new KeyValuePair<string, string>(
                    IcdCodeStrings.RetrievedFrom_PartialWord,
                    "Partial Word Exact Matches"),
                new KeyValuePair<string, string>(
                    IcdCodeStrings.RetrievedFrom_PartialWordSimilar,
                    "Partial Word Similar Matches")
            };

        /// <summary>
        /// Initialize a new SearchList object without any search terms
        /// </summary>
        public SearchList()
        {
            SearchFor = String.Empty;
            Skip = 0;
            TakeAtATime = DefaultTake;
            lastRetrievedFrom = String.Empty;
        }

        /// <summary>
        /// Initialize a new SearchList object with search terms
        /// </summary>
        /// <param name="searchFor"></param>
        public SearchList(string searchFor)
        {
            SearchFor = searchFor;
            Skip = 0;
            TakeAtATime = DefaultTake;
            lastRetrievedFrom = String.Empty;
        }

        public void ResetSearchData()
        {
            Skip = 0;
            lastRetrievedFrom = String.Empty;
        }

        /// <summary>
        /// Get the items for the desired list
        /// </summary>
        /// <param name="provider">Data Provider used to retrieve the items</param>
        /// <param name="showCode">Whether to show codes on the items</param>
        /// <returns>A new collection of ListItemVM objects making up the desired list</returns>
        public IEnumerable<ViewModels.ListItemVM> GetItemVMs(DataProviders.IAppDataProvider provider, bool showCode, ViewModels.SearchVM parentVM)
        {
            if (String.IsNullOrWhiteSpace(SearchFor))
            {
                yield break;
            }

            var allResults = provider.GetSearch(SearchFor, Skip, TakeAtATime + 1);
            bool hasMore = (allResults.Count > TakeAtATime);
            Skip += (hasMore) ? TakeAtATime : allResults.Count;

            var index = 0;
            var results = from result in allResults.Take(TakeAtATime)
                          group result by result.RetrievedFrom;
            foreach (var type in SearchTypes)
            {
                var currMatches = (from result in results
                                   where result.Key == type.Key
                                   from ret in result
                                   select ret).ToArray();
                if (currMatches.Length > 0)
                {
                    if (!String.Equals(lastRetrievedFrom, type.Key))
                    {
                        yield return new ListItemVM(0,
                            IcdCodeItemMethods.NewDividerItem(type.Value),
                            Enumerable.Empty<string>(), showCode)
                            {
                                ParentVM = parentVM
                            };
                        lastRetrievedFrom = type.Key;
                    }

                    foreach (var match in currMatches)
                    {
                        ++index;
                        yield return new ListItemVM(index,
                            match.ToIcdCodeItem(provider),
                            Enumerable.Empty<string>(), showCode)
                            {
                                ParentVM = parentVM
                            };
                    }
                }
            }

            if (hasMore)
            {
                yield return new ListItemVM(0,
                    IcdCodeItemMethods.NewMoreItem(),
                    Enumerable.Empty<string>(), showCode)
                    {
                        ParentVM = parentVM
                    };
            }
        }
    }
}
