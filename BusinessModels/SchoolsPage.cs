using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines a page that shows schools attended
    /// </summary>
    public class SchoolsPage : Page
    {
        /// <summary>
        /// Gets or Sets the original database model, if available
        /// </summary>
        public DbModels.SchoolsPage SchoolsPageOrigin { get; set; }

        /// <summary>
        /// Gets or Sets a unique identifier for the schools part of the page
        /// </summary>
        public Guid SchoolsId { get; set; }

        /// <summary>
        /// Gets or Sets a collection of the schools to display
        /// </summary>
        public ICollection<School> Schools { get; set; }
    }
}
