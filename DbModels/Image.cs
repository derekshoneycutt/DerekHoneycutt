using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines an image to display in the portfolio application
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or Sets a unique identifier of the image
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifier of the page Image Wall extension this will be shown on
        /// </summary>
        public Guid PageId { get; set; }
        /// <summary>
        /// Gets or Sets the page Images extension this will be shown on
        /// </summary>
        public ImageWallPage Page { get; set; }

        /// <summary>
        /// Source of the image to display
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Description of the image to display
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the order of the image
        /// </summary>
        public int? Order { get; set; }
    }
}
