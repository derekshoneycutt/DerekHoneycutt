using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Api.RestModels
{
    /// <summary>
    /// Defines a GitHub page for a resume section of the application
    /// </summary>
    public class GitHubPage : Page
    {
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
