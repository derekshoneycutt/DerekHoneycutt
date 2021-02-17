using SearchIcd10.Data;
using SearchIcd10.Lists;
using SearchIcd10.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SearchIcd10.ViewModels
{
    public class SearchVM : NotifyingModel
    {
        public static readonly PropertyChangedEventArgs NonResultTextChangedArgs = new PropertyChangedEventArgs("NonResultText");
        public static readonly PropertyChangedEventArgs CurrentTextChangedArgs = new PropertyChangedEventArgs("CurrentText");

        /// <summary>
        /// The current list handler being used to generate the list
        /// </summary>
        private IListHandler CurrentHandler;
        /// <summary>
        /// Task used to perform the search on a background thread (YAY!)
        /// </summary>
        private Task searchTask;

        private List<ListItemVM> trueItems;
        /// <summary>
        /// Gets the collection of items currently being displayed
        /// </summary>
        public NotifyingItemVMList Items { get; private set; }

        private string m_NonResultText;
        /// <summary>
        /// Gets the text to be displayed when no results are to be displayed
        /// </summary>
        public string NonResultText
        {
            get { return m_NonResultText; }
            private set { SetValue(ref m_NonResultText, value, NonResultTextChangedArgs); }
        }

        private string m_CurrentText;
        /// <summary>
        /// Gets the current, full text of the user's choices and comments
        /// </summary>
        public string CurrentText
        {
            get { return m_CurrentText; }
            private set { SetValue(ref m_CurrentText, value, CurrentTextChangedArgs); }
        }

        /// <summary>
        /// Text to be displayed during the performance of a search
        /// </summary>
        private static readonly string SearchingText = "Searching...";

        /// <summary>
        /// Text to be displayed if no matches were found
        /// </summary>
        private static readonly string NoMatchesText = "No Matches Found";

        /// <summary>
        /// Text to fill the initial value of NonResultText with
        /// </summary>
        private static readonly string InitialNonResultText = " ";

        /// <summary>
        /// Command to be called to load more searches
        /// </summary>
        public ICommand LoadMoreSearches { get; private set; }

        public SearchVM()
        {
            NonResultText = InitialNonResultText;
            trueItems = new List<ListItemVM>();
            Items = new NotifyingItemVMList();

            LoadMoreSearches = new ActionCommand(LoadMoreSearchResults);
        }

        /// <summary>
        /// Load more search results onto the current view, if on a search list currently
        /// </summary>
        public void LoadMoreSearchResults()
        {
            if (CurrentHandler is SearchList)
            {
                var newItems = CurrentHandler.GetItemVMs(App.DataProvider, false, this)
                                                .ToList();

                var lastItem = Items.Items[Items.Items.Count - 1];
                if (String.Equals(lastItem.Model.Code.CodeType, SearchList.MoreItemType))
                {
                    Items.Items.RemoveAt(Items.Items.Count - 1);
                    trueItems.RemoveAt(trueItems.Count - 1);
                }

                trueItems.AddRange(newItems);
                foreach (var newItem in newItems)
                {
                    Items.Items.Add(newItem);
                }
                UpdateChecks();
            }
        }

        /// <summary>
        /// Start a new search for new terms
        /// </summary>
        public void StartNewSearch()
        {
            CurrentText = String.Empty;
            NonResultText = SearchingText;

            searchTask = Task.Factory.StartNew(() =>
            {
                var FirstList = CurrentHandler.GetItemVMs(App.DataProvider, false, this).ToList();

                Items.InterruptCollectionChanged();
                Items.Items.Clear();
                trueItems.Clear();
                if (FirstList.Count < 1)
                {
                    NonResultText = NoMatchesText;
                }
                else
                {
                    trueItems.AddRange(FirstList);
                    foreach (var item in FirstList)
                    {
                        Items.Items.Add(item);
                    }
                    NonResultText = String.Empty;
                }
                Items.AllowCollectionChanged();
                Items.ForceCollectionChanged();
                UpdateChecks();
            });
        }

        /// <summary>
        /// Perform a new search
        /// </summary>
        /// <param name="mainWin">The Main Window to make any child dialogs the owner on</param>
        /// <param name="newText">New text to search for : Default (empty string) will prompt user for new search terms</param>
        public void NewSearch(string newText)
        {
            if (!String.IsNullOrWhiteSpace(newText))
            {
                CurrentHandler = new SearchList(newText);
                StartNewSearch();
            }
        }

        /// <summary>
        /// Go To the Top List and perform the "search" for that list
        /// </summary>
        public void GoToTopList()
        {
            CurrentHandler = new DbStoredList(IcdCodeStrings.ListId_TopUsed);
            StartNewSearch();
        }

        /// <summary>
        /// Gets the result string from a specified Code Item
        /// </summary>
        /// <param name="item">Item to get the result string from</param>
        /// <returns>String containing the new Result String for the given option</returns>
        private string GetResultsStringFrom(IcdCodeItem item)
        {
            if (item.Enabled)
            {
                var retStr = new StringBuilder();

                retStr.AppendStrings(from child in item.Children
                                     where child.Code.ChildType.Equals(IcdCodeStrings.ChildType_CodeFirst)
                                     select GetResultsStringFrom(child));

                var directChildren = (from child in item.Children
                                      where child.Code.ChildType.Equals(IcdCodeStrings.ChildType_Direct)
                                      select child).ToList();

                if (directChildren.Any(i => i.Enabled))
                {
                    retStr.AppendStrings(directChildren.Select(i => GetResultsStringFrom(i)));
                }
                else
                {
                    retStr.AppendLine(item.Code.Title);
                }
                if (!String.IsNullOrWhiteSpace(item.Comment) && !String.Equals(item.Comment, ListItemVM.CommentStart))
                {
                    retStr.AppendLine(item.Comment);
                }
                retStr.AppendStrings(from child in item.Children
                                     where child.Code.ChildType.Equals(IcdCodeStrings.ChildType_CodeAlso)
                                     select GetResultsStringFrom(child))
                      .AppendStrings(from child in item.Children
                                     where child.Code.ChildType.Equals(IcdCodeStrings.ChildType_CodeAdditional)
                                     select GetResultsStringFrom(child));

                return retStr.ToString();
            }

            return String.Empty;
        }

        /// <summary>
        /// Update the ResultText to a Results String from all choices made
        /// </summary>
        private void UpdateResultsString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendStrings(Items.Where(livm => livm.IsChosen)
                                          .Select(livm => GetResultsStringFrom(livm.Model)));
            var setStr = strBuilder.ToString();
            if (String.IsNullOrWhiteSpace(setStr))
            {
                CurrentText = String.Empty;
            }
            else
            {
                CurrentText = setStr.Trim();
            }
        }

        /// <summary>
        /// Update the current data for any updated items that may have been (un)checked
        /// </summary>
        public void UpdateChecks()
        {
            UpdateResultsString();

            int countNum = 0;
            foreach (var item in Items)
            {
                if (!String.Equals(item.Model.Code.CodeType, IcdCodeStrings.CodeType_Divider))
                {
                    ++countNum;
                    countNum = item.Renumber(countNum);
                }
            }
        }

        /// <summary>
        /// If only one, selected item is chosen by the user, replace it with a new item
        /// </summary>
        /// <param name="newItem">The new item to replace the old with</param>
        /// <param name="selected">If false, will search for only 1 non-selected item; otherwise will search for 1 selected item only</param>
        public void ReplaceSelectedItem(ListItemVM newItem, bool selected = true)
        {
            if (newItem == null)
            {
                return;
            }

            if (Items.Items.Count == 1)
            {
                if (Items.Items[0].IsChosen == selected)
                {
                    Items.InterruptCollectionChanged();
                    Items.Items.Clear();
                    Items.Items.Add(newItem);
                    Items.AllowCollectionChanged();
                    Items.ForceCollectionChanged();
                    UpdateChecks();
                }
            }
        }

        /// <summary>
        /// Clear the list so only one item is displayed
        /// </summary>
        /// <param name="item">The single item to display</param>
        public void ClearToItem(ListItemVM item)
        {
            if (item == null)
            {
                return;
            }

            Items.InterruptCollectionChanged();
            Items.Items.Clear();
            Items.Items.Add(item);
            Items.AllowCollectionChanged();
            Items.ForceCollectionChanged();
            UpdateChecks();
        }

        /// <summary>
        /// Clear it out so all of the last searched items are shown
        /// </summary>
        public void ClearToFullList()
        {
            Items.InterruptCollectionChanged();
            Items.Items.Clear();
            foreach (var item in trueItems)
            {
                Items.Items.Add(item);
            }
            Items.AllowCollectionChanged();
            Items.ForceCollectionChanged();
            UpdateChecks();
        }

        /// <summary>
        /// Wait for any background, search task to complete before exiting
        /// </summary>
        public void WaitForExit()
        {
            var waitTask = searchTask;
            if (waitTask != null)
            {
                waitTask.Wait();
            }
        }
    }
}
