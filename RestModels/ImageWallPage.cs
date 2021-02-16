using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    /// <summary>
    /// Defines a page representing a wall of images to select and view
    /// </summary>
    public class ImageWallPage : Page
    {
        /// <summary>
        /// Gets or Sets a description that goes with the images
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the collection of images to show
        /// </summary>
        public ICollection<string> Images { get; set; }
    }
}
