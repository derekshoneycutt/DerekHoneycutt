using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Api.RestModels
{
    /// <summary>
    /// Defines a major landing of the application for the user to view
    /// </summary>
    public class Landing
    {
        /// <summary>
        /// Gets or Sets the Identifier of the current landing
        /// </summary>
        public string Self { get; set; }

        /// <summary>
        /// Gets or Sets a URL that the section should link out to
        /// Either this should be null or everything following should
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Gets or Sets the title of the landing
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or Sets the subtitle of the landing
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or Sets the icon of the section
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or Sets the order of the landing
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or Sets a collection of the pages that makeup the current landing
        /// </summary>
        public ICollection<Page> Pages { get; set; }

        /// <summary>
        /// Gets or Sets links to navigate around the landing
        /// </summary>
        public ICollection<Link> Links { get; set; }
    }
}
