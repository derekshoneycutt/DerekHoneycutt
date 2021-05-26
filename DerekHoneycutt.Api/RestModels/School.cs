using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Api.RestModels
{
    /// <summary>
    /// Defines a school to show in the education area of the application
    /// </summary>
    public class School
    {
        /// <summary>
        /// Gets or Sets an identifier of the school
        /// </summary>
        public string Self { get; set; }

        /// <summary>
        /// Gets or Sets the name of the school
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the city the school is in
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// Gets or Sets a start date of enrollment
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or Sets an end date of enrollment
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or Sets a program info to describe the education
        /// </summary>
        public string Program { get; set; }

        /// <summary>
        /// Gets or Sets the GPA achieved at the institution
        /// </summary>
        public decimal? Gpa { get; set; }

        /// <summary>
        /// Gets or Sets a string of other information about the school
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// Gets or Sets the order of the school
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or Sets links to navigate around the school
        /// </summary>
        public ICollection<Link> Links { get; set; }
    }
}
