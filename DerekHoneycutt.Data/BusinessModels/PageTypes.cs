using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.BusinessModels
{
    /// <summary>
    /// Definition of standard page types that are supported
    /// </summary>
    public static class PageTypes
    {
        /// <summary>
        /// String describing the Image Wall Page type
        /// </summary>
        public static string ImageWall { get; } = "imagewall";

        /// <summary>
        /// String describing the Resume Experience Page type
        /// </summary>
        public static string ResumeExp { get; } = "resumeexp";

        /// <summary>
        /// String describing the Resume Head Page type
        /// </summary>
        public static string ResumeHead { get; } = "resumehead";

        /// <summary>
        /// String describing the Resume GitHub Page type
        /// </summary>
        public static string GitHub { get; } = "github";

        /// <summary>
        /// String describing the Schools Page type
        /// </summary>
        public static string Schools { get; } = "schools";

        /// <summary>
        /// String describing the TextBlock Page Type
        /// </summary>
        public static string TextBlock { get; } = "textblock";
    }
}
