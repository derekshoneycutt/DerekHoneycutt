using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    /// <summary>
    /// Definition of the Home object returned at the initial access point
    /// </summary>
    public class Home
    {
        /// <summary>
        /// Gets or Sets a collection of all landings for the application
        /// </summary>
        public ICollection<Landing> Landings { get; set; }

        /// <summary>
        /// Gets or Sets a collection of links for the user to navigate through the application
        /// </summary>
        public ICollection<Link> Links { get; set; }
    }
}
