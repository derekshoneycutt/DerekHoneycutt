using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.BusinessModels
{
    /// <summary>
    /// Defines a landing section for the application
    /// </summary>
    public class Landing
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the landing section
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the outside link that the section should point to
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
        /// Gets or Sets a collection of pages that make up the landing section
        /// </summary>
        public ICollection<Page> Pages { get; set; }
    }
}
