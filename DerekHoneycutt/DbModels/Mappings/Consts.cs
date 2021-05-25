using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// "Constants" to use for DB Model mappings
    /// </summary>
    public static class Consts
    {
        private static int m_MaxTitleLength = 150;
        /// <summary>
        /// Maximum length for title columns
        /// </summary>
        public static int MaxTitleLength => m_MaxTitleLength;

        private static int m_MaxSubtitleLength = 350;
        /// <summary>
        /// Maximum length for subtitle columns
        /// </summary>
        public static int MaxSubtitleLength => m_MaxSubtitleLength;

        private static int m_MaxLinkLength = 350;
        /// <summary>
        /// Maximum length for link columns
        /// </summary>
        public static int MaxLinkLength => m_MaxLinkLength;

        private static int m_MaxTextLength = 4096;
        /// <summary>
        /// Maximum length for basic text columns
        /// </summary>
        public static int MaxTextLength => m_MaxTextLength;
    }
}
