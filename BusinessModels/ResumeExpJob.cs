using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines a job to be displayed on a resume experience page
    /// </summary>
    public class ResumeExpJob
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the job
        /// </summary>
        public Guid Id { get; set; }

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
    }
}
