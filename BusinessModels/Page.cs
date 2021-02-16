using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines a page that is shown in the application
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or Sets a unique identifier for this page
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets a string identifying the type of page
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets the title of the page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the subtitle of the page
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or Sets the background color of the page (anything CSS supports)
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Gets or Sets a single image associated with the page
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or Sets an orientation that the page should favor (depends on content & client)
        /// </summary>
        public string Orientation { get; set; }
    }
}
