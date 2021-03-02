using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines a page that displays a block of formatted text
    /// </summary>
    public class TextBlockPage : Page
    {
        /// <summary>
        /// Gets or Sets the original database model, if available
        /// </summary>
        public DbModels.TextBlockPage TextBlockPageOrigin { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifier for the textblock portion of the page
        /// </summary>
        public Guid TextBlockId { get; set; }

        /// <summary>
        /// Gets or Sets the block of text to display
        /// </summary>
        public string Text { get; set; }
    }
}
