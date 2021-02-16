using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    /// <summary>
    /// Defines a Job description in a resume experience page
    /// </summary>
    public class ResumeExpJob
    {
        /// <summary>
        /// Gets or Sets the identifier of the job object
        /// </summary>
        public string Self { get; set; }

        /// <summary>
        /// Gets or Sets the title of the job
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the name of the employer
        /// </summary>
        public string Employer { get; set; }

        /// <summary>
        /// Gets or Sets the city of the employer
        /// </summary>
        public string EmployerCity { get; set; }

        /// <summary>
        /// Gets or Sets the date employment started
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or Sets the date employment ended
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets a description of the job
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets links to navigate around the job
        /// </summary>
        public ICollection<Link> Links { get; set; }
    }
}
