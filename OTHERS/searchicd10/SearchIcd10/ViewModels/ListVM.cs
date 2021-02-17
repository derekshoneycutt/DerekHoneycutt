using SearchIcd10.Data;
using SearchIcd10.Lists;
using SearchIcd10.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SearchIcd10.ViewModels
{
    /// <summary>
    /// View Model Class for handling the displaying of ICD Lists
    /// </summary>
    public class ListVM : NotifyingModel
    {
        public static readonly PropertyChangedEventArgs RememberUserChangedArgs = new PropertyChangedEventArgs("RememberUser");
        public static readonly PropertyChangedEventArgs SearchTermsChangedArgs = new PropertyChangedEventArgs("SearchTerms");
        public static readonly PropertyChangedEventArgs CurrentSearchChangedArgs = new PropertyChangedEventArgs("CurrentSearch");

        /// <summary>
        /// Gets or Sets whether the Session will be closed and the user logged out
        /// </summary>
        public bool WillLogout { get; set; }

        private bool m_RememberUser;
        /// <summary>
        /// Gets or Sets whether the user wants to be remembered next time they launch the application
        /// </summary>
        public bool RememberUser
        {
            get { return m_RememberUser; }
            set { SetValue(ref m_RememberUser, value, RememberUserChangedArgs); }
        }

        private string m_SearchTerms;
        /// <summary>
        /// Gets the Search Terms desired for the search
        /// </summary>
        public string SearchTerms
        {
            get { return m_SearchTerms; }
            set { SetValue(ref m_SearchTerms, value, SearchTermsChangedArgs); }
        }

        private SearchVM m_CurrentSearch;
        /// <summary>
        /// Gets or Sets the View Model representing the currently viewed search
        /// </summary>
        public SearchVM CurrentSearch
        {
            get { return m_CurrentSearch; }
            set { SetValue(ref m_CurrentSearch, value, CurrentSearchChangedArgs); }
        }

        /// <summary>
        /// Gets the current collection of all searches
        /// </summary>
        public ObservableCollection<SearchVM> AllSearches
        {
            get;
            private set;
        }

        /// <summary>
        /// Command to be called when search terms are entered and OK is clicked
        /// </summary>
        public ICommand SearchOkCmnd { get; private set; }
        /// <summary>
        /// Command to be called when the user wishes to go to their Lists
        /// </summary>
        public ICommand SearchListsCmnd { get; private set; }

        /// <summary>
        /// Command to be called in order to copy current results to the clipboard
        /// </summary>
        public ICommand CopyToClipboardCmd { get; private set; }

        /// <summary>
        /// Command to be called in order to switch to another, previous search
        /// </summary>
        public ICommand SwitchToSearchCmd { get; private set; }

        /// <summary>
        /// Command to be called in order to clear all of the searches
        /// </summary>
        public ICommand ClearSearchesCmd { get; private set; }

        /// <summary>
        /// Initiate a new ListVM
        /// </summary>
        public ListVM()
        {
            WillLogout = String.IsNullOrEmpty(App.StartupArgs.SessionToken);
            RememberUser = App.RememberMe;
            CurrentSearch = new SearchVM();
            AllSearches = new ObservableCollection<SearchVM>();
            AllSearches.Add(CurrentSearch);

            SearchOkCmnd = new ActionCommand(() => NewSearch(SearchTerms));
            SearchListsCmnd = new ActionCommand(GoToTopList);

            CopyToClipboardCmd = new ActionCommand(CopyToClipboard);

            SwitchToSearchCmd = new ActionCommand<SearchVM>(SwitchToSearch);

            ClearSearchesCmd = new ActionCommand(ClearSearches);

            GoToTopList();
        }

        /// <summary>
        /// Copy the current chosen result text to the clipboard
        /// </summary>
        public void CopyToClipboard()
        {
            var buildingStr = new StringBuilder();
            buildingStr.AppendLine("Based on evidence within the medical record, during this visit the following was evaluated:");
            foreach (var search in AllSearches)
            {
                buildingStr.AppendLine(search.CurrentText);
            }
            Clipboard.Clear();
            Clipboard.SetText(buildingStr.ToString(), TextDataFormat.Text);
        }

        /// <summary>
        /// Prepare a new search, using either the current search if not currently already selected, or creating a new search VM
        /// </summary>
        /// <param name="perform">Action to perform on the appropriate search VM</param>
        private void PrepareNewSearch(Action<SearchVM> perform)
        {
            if (String.IsNullOrWhiteSpace(CurrentSearch.CurrentText))
            {
                if (perform != null)
                {
                    perform(CurrentSearch);
                }
            }
            else
            {
                var newSearch = new SearchVM();
                AllSearches.Add(newSearch);
                CurrentSearch = newSearch;
                if (perform != null)
                {
                    perform(newSearch);
                }
            }
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
                PrepareNewSearch(s => s.NewSearch(newText));
            }
        }

        /// <summary>
        /// Go To the Top List and perform the "search" for that list
        /// </summary>
        public void GoToTopList()
        {
            PrepareNewSearch(s => s.GoToTopList());
            SearchTerms = String.Empty;
        }

        /// <summary>
        /// Clear all of the searches and go to the Top List without any chosen results
        /// </summary>
        public void ClearSearches()
        {
            AllSearches.Clear();
            var newSearch = new SearchVM();
            AllSearches.Add(newSearch);
            CurrentSearch = newSearch;
            CurrentSearch.GoToTopList();
        }

        /// <summary>
        /// Switch to another search. Search must be present in AllSearches already
        /// </summary>
        /// <param name="switchTo">Search to switch to</param>
        public void SwitchToSearch(SearchVM switchTo)
        {
            if (AllSearches.Contains(switchTo))
            {
                var currSearch = CurrentSearch;
                CurrentSearch = switchTo;

                if (String.IsNullOrWhiteSpace(currSearch.CurrentText))
                {
                    AllSearches.Remove(currSearch);
                }
            }
        }

        /// <summary>
        /// Wait for any background, search task to complete before exiting
        /// </summary>
        public void WaitForExit()
        {
            if (!RememberUser && WillLogout)
            {
                App.DataProvider.CloseSession();
            }

            foreach (var search in AllSearches)
            {
                search.WaitForExit();
            }
        }
    }
}
