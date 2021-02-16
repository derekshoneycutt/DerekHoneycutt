using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines an Image Wall Page, basically just showing images off
    /// </summary>
    public class ImageWallPage : Page
    {
        /// <summary>
        /// Gets or Sets the ID for the database part capturing the image wall
        /// </summary>
        public Guid ImageWallId { get; set; }

        /// <summary>
        /// Gets or Sets a description that goes with the images
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the collection of images to show
        /// </summary>
        public string Images { get; set; }
    }
}
