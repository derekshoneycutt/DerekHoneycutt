using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess
{
    /// <summary>
    /// Class used to compare IcdCode objects' equality based on Title, Code, CodeType properties only
    /// </summary>
    public class IcdCodeCodeEqualityComparer : IEqualityComparer<IcdCode>
    {
        private static IcdCodeCodeEqualityComparer m_Static = new IcdCodeCodeEqualityComparer();
        /// <summary>
        /// Get a Static instance of the Comparer for use
        /// </summary>
        public static IcdCodeCodeEqualityComparer Static { get { return m_Static; } }

        /// <summary>
        /// Test the equality of 2 IcdCode objects based on Title, Code, and CodeType properties
        /// </summary>
        /// <param name="x">First IcdCode object to test</param>
        /// <param name="y">Second IcdCode object to test</param>
        /// <returns>Boolean value that is true if the objects are considered equal</returns>
        public bool Equals(IcdCode x, IcdCode y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }
            return String.Equals(x.Title, y.Title) && String.Equals(x.Code, y.Code) && String.Equals(x.CodeType, y.CodeType);
        }

        /// <summary>
        /// Get a hashcode from only the Title, Code, and CodeType Properties
        /// </summary>
        /// <param name="obj">IcdCode Object to get the hashcode for</param>
        /// <returns>Integer value representing the objects simple hashcode</returns>
        public int GetHashCode(IcdCode obj)
        {
            int hashcode = 0;

            hashcode = (hashcode * 397) ^ (obj.Title != null ? obj.Title.GetHashCode() : 0);
            hashcode = (hashcode * 397) ^ (obj.Code != null ? obj.Code.GetHashCode() : 0);
            hashcode = (hashcode * 397) ^ (obj.CodeType != null ? obj.CodeType.GetHashCode() : 0);

            return hashcode;
        }
    }
}
