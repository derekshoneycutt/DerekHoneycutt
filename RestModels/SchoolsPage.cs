using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    /// <summary>
    /// Defines a page that shows schools attended
    /// </summary>
    public class SchoolsPage : Page
    {
        /// <summary>
        /// Gets or Sets a collection of the schools attended
        /// </summary>
        public ICollection<School> Schools { get; set; }
    }
}
