using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.DbModels
{
    /// <summary>
    /// Defines a Resume Experience extension to a page
    /// </summary>
    public class ResumeExpPage
    {
        /// <summary>
        /// Gets or Sets the internal index (should let DB make this)
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Gets or Sets a unique identifier for the extension
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifier of the page this is extending
        /// </summary>
        public Guid PageId { get; set; }
        /// <summary>
        /// Gets or Sets the page this is extending
        /// </summary>
        public virtual Page Page { get; set; }

        /// <summary>
        /// Gets or Sets a collection of the jobs associated to this page extension
        /// </summary>
        public virtual ICollection<ResumeExpJob> Jobs { get; set; }
    }
}
