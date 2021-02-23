using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines an extension to a page making it a Text Block Page
    /// </summary>
    public class TextBlockPage
    {
        /// <summary>
        /// Gets or Sets the internal index (should let DB make this)
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Gets or Sets a unique identifer for the extension
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifier of the page this extends
        /// </summary>
        public Guid PageId { get; set; }
        /// <summary>
        /// Gets or Sets the page this extends
        /// </summary>
        public virtual Page Page { get; set; }

        /// <summary>
        /// Gets or Sets the block of text to display
        /// </summary>
        public string Text { get; set; }
    }
}
