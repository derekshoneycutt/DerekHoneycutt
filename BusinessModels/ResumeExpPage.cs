using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines a Resume Experience Page
    /// </summary>
    public class ResumeExpPage : Page
    {
        /// <summary>
        /// Gets or Sets a unique identifier for the resume experience part of the page
        /// </summary>
        public Guid ResumeExpId { get; set; }

        /// <summary>
        /// Gets or Sets a collection of the jobs to show on the page
        /// </summary>
        public ICollection<ResumeExpJob> Jobs { get; set; }
    }
}
