using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines an Landing Section for the portfolio application
    /// </summary>
    public class Landing
    {
        /// <summary>
        /// Gets or Sets a unique identifier for the application
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets a link that the sections should direct to
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Gets or Sets the title of the section
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the subtitle of the section
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or Sets the icon of the section
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or Sets the order of the landing
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or Sets a collection of all of the pages associated to the landing section
        /// </summary>
        public ICollection<Page> Pages { get; set; }
    }
}
