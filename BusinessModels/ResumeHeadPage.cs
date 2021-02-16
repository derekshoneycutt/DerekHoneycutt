using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines a Resume Header Page
    /// </summary>
    public class ResumeHeadPage : Page
    {
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
    }
}
