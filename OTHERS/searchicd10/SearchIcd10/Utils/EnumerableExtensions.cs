using System.Collections.Generic;
using System.Text;

namespace SearchIcd10.Utils
{
    /// <summary>
    /// Class containing helpers extensions for IEnumerable handling
    /// </summary>
    public static class EnumerableHelper
    {
        /// <summary>
        /// Append a collection of strings to a StringBuilder class
        /// </summary>
        /// <param name="strBuilder">StringBuilder object to append to</param>
        /// <param name="collection">Collection of strings to append</param>
        /// <returns>The StringBuilder class for chained calls</returns>
        public static StringBuilder AppendStrings(this StringBuilder strBuilder, IEnumerable<string> collection)
        {
            foreach (var str in collection)
            {
                strBuilder.Append(str);
            }
            return strBuilder;
        }

        /// <summary>
        /// Append a collection of strings to a StringBuilder class, each string followed by the NewLine sequence
        /// </summary>
        /// <param name="strBuilder">StringBuilder object to append to</param>
        /// <param name="collection">Collection of strings to append</param>
        /// <returns>The StringBuilder class for chained calls</returns>
        public static StringBuilder AppendLineStrings(this StringBuilder strBuilder, IEnumerable<string> collection)
        {
            foreach (var str in collection)
            {
                strBuilder.AppendLine(str);
            }
            return strBuilder;
        }
    }
}
