using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines an extension to pages making them Image Wall Pages
    /// </summary>
    public class ImageWallPage
    {
        /// <summary>
        /// Gets or Sets a unique identifier for the extension
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifier of the page this extends
        /// </summary>
        public Guid PageId { get; set; }
        /// <summary>
        /// Gets or Sets the page this extends
        /// </summary>
        public Page Page { get; set; }


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
