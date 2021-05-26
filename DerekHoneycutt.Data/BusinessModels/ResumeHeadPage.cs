using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.BusinessModels
{
    /// <summary>
    /// Defines a Resume Header Page
    /// </summary>
    public class ResumeHeadPage : Page
    {
        /// <summary>
        /// Gets or Sets the original database model, if available
        /// </summary>
        public DbModels.ResumeHeadPage ResumeHeadPageOrigin { get; set; }

        /// <summary>
        /// Gets or Sets a unique identifier for the resume header part of the page
        /// </summary>
        public Guid ResumeHeadId { get; set; }


        /// <summary>
        /// Gets or Sets a general description of the job
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the competencies to display, separated by a pipe |
        /// </summary>
        public string Competencies { get; set; }

        /// <summary>
        /// GitHub repository to link to on the main resume page
        /// </summary>
        public string GitHub { get; set; }

        /// <summary>
        /// Description of GitHub repository to link to
        /// </summary>
        public string GitHubDescription { get; set; }
    }
}
