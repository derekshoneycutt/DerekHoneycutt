using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{
    /// <summary>
    /// Defines a Page that is displayed within a landing section of the portfolio application
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or Sets a unique identifier of the page
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets the unique identifier of the landing section this page belongs to
        /// </summary>
        public Guid LandingId { get; set; }
        /// <summary>
        /// Ges or Sets the landing section this page belongs to
        /// </summary>
        public Landing Landing { get; set; }

        /// <summary>
        /// Gets or Sets an Order index to order the page
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or Sets a string identifying the type of page
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets the title of the page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the subtitle of the page
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or Sets the background color of the page (anything CSS supports)
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Gets or Sets a single image associated with the page
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or Sets an orientation that the page should favor (depends on content & client)
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or Sets an Image Wall extension for the page
        /// </summary>
        public ImageWallPage ImageWallExt { get; set; }
        /// <summary>
        /// Gets or Sets a Resume Experience extension for the page
        /// </summary>
        public ResumeExpPage ResumeExpExt { get; set; }
        /// <summary>
        /// Gets or Sets a Resume Head extension for the page
        /// </summary>
        public ResumeHeadPage ResumeHeadExt { get; set; }
        /// <summary>
        /// Gets or Sets a Schools extension for the page
        /// </summary>
        public SchoolsPage SchoolsExt { get; set; }
        /// <summary>
        /// Gets or Sets a Text Block extension for the page
        /// </summary>
        public TextBlockPage TextBlockExt { get; set; }
    }
}
