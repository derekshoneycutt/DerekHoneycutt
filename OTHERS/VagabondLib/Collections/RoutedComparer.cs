using System.Collections;
using System.Collections.Generic;

namespace VagabondLib.Collections
{
    /// <summary>
    /// Class used to route from a non-generic IComparer object to IComparer&lt;object&gt;
    /// </summary>
    public class RoutedComparer : IComparer<object>
    {
        private IComparer m_True;

        /// <summary>
        /// Initialize a comparer object with an existing non-generic IComparer object
        /// </summary>
        /// <param name="useTrue">True, non-generic comparer to utilize in comparison</param>
        public RoutedComparer(IComparer useTrue)
        {
            m_True = useTrue;
        }

        /// <summary>
        /// Compare 2 objects
        /// </summary>
        /// <param name="x">First object to compare</param>
        /// <param name="y">Second object to compare</param>
        /// <returns>The value returned by the inner, non-generic comparer, used to compare the two values</returns>
        public int Compare(object x, object y)
        {
            return m_True.Compare(x, y);
        }
    }
}
