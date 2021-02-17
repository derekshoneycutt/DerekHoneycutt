using System;
using System.Collections.Generic;
using System.Linq;

namespace VagabondLib.Files
{
    /// <summary>
    /// Representation of a Filter for a file
    /// </summary>
    public class Filter
    {
        private static readonly string m_AllFilesTitle = "All Files (*.*)";
        private static readonly string m_AllFilesWildcard = "*.*";
        /// <summary>
        /// Default title for an All Files filter
        /// </summary>
        public static string AllFilesTitle { get { return m_AllFilesTitle; } }
        /// <summary>
        /// Default Wildcard for an All Files filter
        /// </summary>
        public static string AllFilesWildcard { get { return m_AllFilesWildcard; } }

        private static readonly Filter m_AllFilesFilter = new Filter();
        /// <summary>
        /// Filter with default All Files strings filled
        /// </summary>
        public static Filter AllFilesFilter { get { return m_AllFilesFilter; } }

        /// <summary>
        /// Gets or Sets the title of the filter
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the wildcard of the filter
        /// </summary>
        public string Wildcard { get; set; }

        /// <summary>
        /// Initializes a new filter with the default All Files title and wildcard
        /// </summary>
        public Filter()
        {
            Title = AllFilesTitle;
            Wildcard = AllFilesWildcard;
        }

        /// <summary>
        /// Initializes a new filter with the specified title and wildcard
        /// </summary>
        /// <param name="title">Title for the new filter object</param>
        /// <param name="wildcard">Wildcard for the new filter object</param>
        public Filter(string title, string wildcard)
        {
            Title = title;
            Wildcard = wildcard;
        }

        /// <summary>
        /// Initialize a new range of Filter objects with the specified string of titles and wildcards
        /// </summary>
        /// <param name="titlesAndWildcards">Range of strings alternating title and wildcard; always: Title, Wildcard, Title, Wildcard, ...</param>
        /// <returns>New delayed execution range of filter objects</returns>
        public static IEnumerable<Filter> Range(IEnumerable<string> titlesAndWildcards)
        {
            var arr = titlesAndWildcards.ToList();
            if (arr.Count > 1)
            {
                for (int strOn = 0; strOn < arr.Count - 1; strOn += 2)
                {
                    yield return new Filter(arr[strOn], arr[strOn + 1]);
                }
            }
        }

        /// <summary>
        /// Initialize a new range of Filter objects with the specified string of titles and wildcards
        /// </summary>
        /// <param name="titlesAndWildcards">Range of strings alternating title and wildcard; always: Title, Wildcard, Title, Wildcard, ...</param>
        /// <returns>New delayed execution range of filter objects</returns>
        public static IEnumerable<Filter> Range(params string[] titlesAndWildcards)
        {
            return Range(titlesAndWildcards);
        }

        /// <summary>
        /// Convert a range of Filters into a single string
        /// </summary>
        /// <param name="range">Range of Filters to convert into a string</param>
        /// <returns>String created from the range of filters</returns>
        public static string RangeToString(IEnumerable<Filter> range)
        {
            return String.Join("|", from f in range select f.ToString());
        }

        public override string ToString()
        {
            return String.Format("{0}|{1}", Title, Wildcard);
        }
    }
}
