using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    /// <summary>
    /// Defines a head page for a resume section of the application
    /// </summary>
    public class ResumeHeadPage : Page
    {

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
