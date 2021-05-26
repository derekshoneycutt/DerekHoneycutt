using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Api.RestModels
{
    /// <summary>
    /// Defines a single page for the viewer to see
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or Sets the Identifier of the page
        /// </summary>
        public string Self { get; set; }

        /// <summary>
        /// Gets or Sets the type of the page
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets the title of the page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets a subtitle to display, if appropriate
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or Sets the background color for the page
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Gets or Sets an image that is associated to the page, if appropriate
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or Sets the preferred orientation of the page, if appropriate
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or Sets links to navigate around the page
        /// </summary>
        public ICollection<Link> Links { get; set; }
    }
}
