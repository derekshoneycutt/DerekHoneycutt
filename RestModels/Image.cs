using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    /// <summary>
    /// Defines an image that is to be displayed
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or Sets the identifier of the image object
        /// </summary>
        public string Self { get; set; }

        /// <summary>
        /// Gets or Sets the source of the images
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or Sets a description that goes with the image
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets links to navigate around the image
        /// </summary>
        public ICollection<Link> Links { get; set; }
    }
}
