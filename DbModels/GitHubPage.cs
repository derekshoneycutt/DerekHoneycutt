using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines an extension to a page making it a Resume GitHub Page
    /// </summary>
    public class GitHubPage
    {
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
        public Page Page { get; set; }

        /// <summary>
        /// GitHub repository to link to on the main resume page
        /// </summary>
        public string GitHub { get; set; }

        /// <summary>
        /// Gets or Sets a general description of the repository
        /// </summary>
        public string Description { get; set; }
    }
}
