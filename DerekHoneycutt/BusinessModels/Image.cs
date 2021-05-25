using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines an image to display
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or Sets the original database model, if available
        /// </summary>
        public DbModels.Image ImageOrigin { get; set; }

        /// <summary>
        /// Gets or Sets a unique identifier of the image
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the source of the image
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or Sets the description of the image
        /// </summary>
        public string Description { get; set; }
    }
}
