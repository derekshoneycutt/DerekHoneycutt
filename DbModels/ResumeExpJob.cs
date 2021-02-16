using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines a Resume Experience Job for the portfolio application
    /// </summary>
    public class ResumeExpJob
    {
        /// <summary>
        /// Gets or Sets a unique identifier of the job
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifier of the page Resume Experience extension this job is shown on
        /// </summary>
        public Guid PageId { get; set; }
        /// <summary>
        /// Gets or Sets the page Resume Experience extension this job is shown on
        /// </summary>
        public ResumeExpPage Page { get; set; }

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
