using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Api.RestModels
{
    /// <summary>
    /// Defines a page that shows work experience
    /// </summary>
    public class ResumeExpPage : Page
    {
        /// <summary>
        /// Gets or Sets the jobs that should be displayed on the page
        /// </summary>
        public ICollection<ResumeExpJob> Jobs { get; set; }
    }
}
