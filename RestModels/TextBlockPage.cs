using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    /// <summary>
    /// Defines a page to display a block of text
    /// </summary>
    public class TextBlockPage : Page
    {
        /// <summary>
        /// Gets or Sets the block of text to display
        /// </summary>
        public string Text { get; set; }
    }
}
