using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    /// <summary>
    /// Defines a link used to navigate the RESTful interface
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Gets or Sets the relation defined by the link
        /// Used as name of function on frontend
        /// </summary>
        public string Rel { get; set; }
        /// <summary>
        /// Gets or Sets the HTTP Method utilized to access the link
        /// May be several, separated by a pipe |
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Gets or Sets the address of the link
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// Gets or Sets exemplar data to be used in POST body if appropriate
        /// </summary>
        public object PostData { get; set; }
    }
}
