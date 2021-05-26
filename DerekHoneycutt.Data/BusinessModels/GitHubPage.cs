using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.BusinessModels
{
    /// <summary>
    /// Defines a Resume GitHub Page
    /// </summary>
    public class GitHubPage : Page
    {
        /// <summary>
        /// Gets or Sets the original database model, if available
        /// </summary>
        public DbModels.GitHubPage GitHubPageOrigin { get; set; }

        /// <summary>
        /// Gets or Sets a unique identifier for the resume header part of the page
        /// </summary>
        public Guid GitHubId { get; set; }



        /// <summary>
        /// GitHub repository to link to on the main resume page
        /// </summary>
        public string GitHub { get; set; }
        /// <summary>
        /// Gets or Sets a general description of the job
        /// </summary>
        public string Description { get; set; }
    }
}
