using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VagabondLib.Collections
{
    /// <summary>
    /// Utility functions for performing Binary Searches on Sorted Collections
    /// </summary>
    public static class BinarySearch
    {
        /// <summary>
        /// Searches the sorted list for a matching item
        /// <para />
        /// <para>If list is not sorted, this does not guarantee appropriate return values</para>
        /// <para />
        /// <para>If the return is negative, the bitwise complement contains the next highest index in the list : The location the new item should be inserted to maintain sorted status</para>
        /// </summary>
        /// <typeparam name="T">Type of each item within the collection</typeparam>
        /// <param name="collection">Collection to search within; Expected to be pre-sorted</param>
        /// <param name="target">Target item to search for</param>
        /// <param name="comparer">Comparer to utilize in the search ; Should be sorted according to same comparer</param>
        /// <returns>The zero-based index of the matching item, or bitwise complement of the next highest (index new item should be inserted in)</returns>
        public static int Search<T>(IEnumerable<T> collection, T target, IComparer<T> comparer)
        {
            var list = collection.ToList();
            int low = 0;
            int high = list.Count - 1;
            while (low <= high)
            {
                int median = low + (high - low >> 1);
                int value = comparer.Compare(list[median], target);
                if (value == 0)
                {
                    return median;
                }
                else if (value < 0)
                {
                    low = median + 1;
                }
                else if (value > 0)
                {
                    high = median - 1;
                }
            }
            return ~low;
        }

        /// <summary>
        /// Searches the sorted list for a matching item
        /// <para />
        /// <para>If list is not sorted, this does not guarantee appropriate return values</para>
        /// <para />
        /// <para>If the return is negative, the bitwise complement contains the next highest index in the list : The location the new item should be inserted to maintain sorted status</para>
        /// <para />
        /// <para>Uses the Default Comparer for the generic type</para>
        /// </summary>
        /// <typeparam name="T">Type of each item within the collection</typeparam>
        /// <param name="collection">Collection to search within; Expected to be pre-sorted</param>
        /// <param name="target">Target item to search for</param>
        /// <returns>The zero-based index of the matching item, or bitwise complement of the next highest (index new item should be inserted in)</returns>
        public static int Search<T>(IEnumerable<T> collection, T target)
        {
            return Search<T>(collection, target, Comparer<T>.Default);
        }

        /// <summary>
        /// Searches the sorted list for a matching item
        /// <para />
        /// <para>If list is not sorted, this does not guarantee appropriate return values</para>
        /// <para />
        /// <para>If the return is negative, the bitwise complement contains the next highest index in the list : The location the new item should be inserted to maintain sorted status</para>
        /// </summary>
        /// <param name="collection">Collection to search within; Expected to be pre-sorted</param>
        /// <param name="target">Target item to search for</param>
        /// <param name="comparer">Comparer to utilize in the search ; Should be sorted according to same comparer</param>
        /// <returns>The zero-based index of the matching item, or bitwise complement of the next highest (index new item should be inserted in)</returns>
        public static int Search(IEnumerable collection, object target, IComparer comparer)
        {
            return Search(collection.Cast<object>(), target, new RoutedComparer(comparer));
        }

        /// <summary>
        /// Searches the sorted list for a matching item
        /// <para />
        /// <para>If list is not sorted, this does not guarantee appropriate return values</para>
        /// <para />
        /// <para>If the return is negative, the bitwise complement contains the next highest index in the list : The location the new item should be inserted to maintain sorted status</para>
        /// <para />
        /// <para>Uses the Default Comparer</para>
        /// </summary>
        /// <param name="collection">Collection to search within; Expected to be pre-sorted</param>
        /// <param name="target">Target item to search for</param>
        /// <returns>The zero-based index of the matching item, or bitwise complement of the next highest (index new item should be inserted in)</returns>
        public static int Search(IEnumerable collection, object target)
        {
            return Search(collection, target, Comparer.Default);
        }
    }
}
