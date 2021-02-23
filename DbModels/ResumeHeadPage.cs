using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines an extension to a page making it a Resume Head Page
    /// </summary>
    public class ResumeHeadPage
    {
        /// <summary>
        /// Gets or Sets the internal index (should let DB make this)
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Gets or Sets a unique identifier of the extension
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
        /// Gets or Sets a general description of the job
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the competencies to display, separated by a pipe |
        /// </summary>
        public string Competencies { get; set; }
    }
}
