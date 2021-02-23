using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines a school to display on the portfolio application
    /// </summary>
    public class School
    {
        /// <summary>
        /// Gets or Sets a unique identifier of the school
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifier of the page Schools extension this will be shown on
        /// </summary>
        public Guid PageId { get; set; }
        /// <summary>
        /// Gets or Sets the page Schools extension this will be shown on
        /// </summary>
        public SchoolsPage Page { get; set; }

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
        public decimal? GPA { get; set; }

        /// <summary>
        /// Gets or Sets a string of other information about the school
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// Gets or Sets the order of the school
        /// </summary>
        public int? Order { get; set; }
    }
}
