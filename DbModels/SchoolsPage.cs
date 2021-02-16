using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines an extension to a page making it a Schools page
    /// </summary>
    public class SchoolsPage
    {
        /// <summary>
        /// Gets or Sets a unique identifier of the schools page extension
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifer of the page this extends
        /// </summary>
        public Guid PageId { get; set; }
        /// <summary>
        /// Gets or Sets the page this extends
        /// </summary>
        public Page Page { get; set; }

        /// <summary>
        /// Gets or Sets a collection of the schools associated to this extension
        /// </summary>
        public ICollection<School> Schools { get; set; }
    }
}
