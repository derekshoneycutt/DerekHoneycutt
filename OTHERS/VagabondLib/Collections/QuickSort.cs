using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VagabondLib.Collections
{
    /// <summary>
    /// Utility class for performing 
    /// </summary>
    public static class QuickSort
    {
        /// <summary>
        /// Choose an index to pivot on in the QuickSort algorithm below
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to choose the pivot within</param>
        /// <param name="low">Low index of the current partition</param>
        /// <param name="high">High index of the current partition</param>
        /// <param name="comparer">Comparer to use in determining pivot point</param>
        /// <returns>Best index to pivot partitioning on</returns>
        private static int ChoosePartPivot<T>(IList<T> collection, int low, int high, IComparer<T> comparer)
        {
            int middle = low + (high - low >> 1);

            if (comparer.Compare(collection[low], collection[middle]) < 0)
            {
                if (comparer.Compare(collection[low], collection[high]) >= 0)
                {
                    return low;
                }
                else if (comparer.Compare(collection[middle], collection[high]) < 0)
                {
                    return middle;
                }
            }
            else
            {
                if (comparer.Compare(collection[low], collection[high]) < 0)
                {
                    return low;
                }
                else if (comparer.Compare(collection[middle], collection[high]) >= 0)
                {
                    return middle;
                }
            }
            return high;
        }

        /// <summary>
        /// Swap Items in the list
        /// </summary>
        /// <typeparam name="T">Type used within the collections</typeparam>
        /// <param name="collection">Collection to choose the pivot within</param>
        /// <param name="item1">First item to swap</param>
        /// <param name="item2">Second item to swap</param>
        private static void SwapItems<T>(IList<T> collection, int item1, int item2)
        {
            var tempItem = collection[item1];
            collection[item1] = collection[item2];
            collection[item2] = tempItem;
        }

        /// <summary>
        /// Partition the list for QuickSort
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to partition</param>
        /// <param name="low">Low index of the current partition</param>
        /// <param name="high">High index of the current partition</param>
        /// <param name="comparer">Comparer to use in partitioning the list</param>
        /// <returns>Pivot point of remaining 2 partitions</returns>
        private static int PartList<T>(IList<T> collection, int low, int high, IComparer<T> comparer)
        {
            int pivotIndex = ChoosePartPivot(collection, low, high, comparer);
            var pivotVal = collection[pivotIndex];
            SwapItems(collection, pivotIndex, high);
            int storeIndex = low;
            for (int i = low; i < high; ++i)
            {
                if (comparer.Compare(collection[i], pivotVal) < 0)
                {
                    SwapItems(collection, i, storeIndex++);
                }
            }
            SwapItems(collection, storeIndex, high);
            return storeIndex;
        }

        /// <summary>
        /// QuickSort algorithm for sorting a list
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <param name="low">Low index to begin sorting</param>
        /// <param name="high">High index to end sorting</param>
        /// <param name="comparer">Comparer to use in sorting the list</param>
        private static void QSortList<T>(IList<T> collection, int low, int high, IComparer<T> comparer)
        {
            if (low < high)
            {
                int partLocat = PartList(collection, low, high, comparer);
                QSortList(collection, low, partLocat - 1, comparer);
                QSortList(collection, partLocat + 1, high, comparer);
            }
        }

        /// <summary>
        /// Sort a collection in a given range. Returns a new, sorted List
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <param name="low">Low index of range to sort</param>
        /// <param name="high">High index of range to sort</param>
        /// <param name="comparer">Comparer to use in sorting the collection</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static List<T> Sort<T>(IEnumerable<T> collection, int low, int high, IComparer<T> comparer)
        {
            var retList = collection.ToList();
            QSortList(retList, (low >= 0) ? low : 0, (high < retList.Count) ? high : retList.Count - 1, comparer);
            return retList;
        }

        /// <summary>
        /// Sort a collection in a given range. Sorts the collection passed.
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <param name="low">Low index of range to sort</param>
        /// <param name="high">High index of range to sort</param>
        /// <param name="comparer">Comparer to use in sorting the collection</param>
        public static void InlineSort<T>(IList<T> collection, int low, int high, IComparer<T> comparer)
        {
            QSortList(collection, (low >= 0) ? low : 0, (high < collection.Count) ? high : collection.Count - 1, comparer);
        }

        /// <summary>
        /// Sort the list in a given range. Uses Default Comparer
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <param name="low">Low index of range to sort</param>
        /// <param name="high">High index of range to sort</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static List<T> Sort<T>(IEnumerable<T> collection, int low, int high)
        {
            return Sort(collection, low, high, Comparer<T>.Default);
        }

        /// <summary>
        /// Sort the list in a given range. Uses Default Comparer. Sorts the collection passed.
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <param name="low">Low index of range to sort</param>
        /// <param name="high">High index of range to sort</param>
        public static void InlineSort<T>(IList<T> collection, int low, int high)
        {
            InlineSort(collection, low, high, Comparer<T>.Default);
        }

        /// <summary>
        /// Sort the entire list
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <param name="comparer">Comparer to use in sorting the collection</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static List<T> Sort<T>(IEnumerable<T> collection, IComparer<T> comparer)
        {
            return Sort(collection, 0, Int32.MaxValue, comparer);
        }

        /// <summary>
        /// Sort the entire list. Sorts the collection passed.
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <param name="comparer">Comparer to use in sorting the collection</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static void InlineSort<T>(IList<T> collection, IComparer<T> comparer)
        {
            InlineSort(collection, 0, Int32.MaxValue, comparer);
        }

        /// <summary>
        /// Sort the entire list. Uses Default Comparer
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static List<T> Sort<T>(IEnumerable<T> collection)
        {
            return Sort(collection, Comparer<T>.Default);
        }

        /// <summary>
        /// Sort the entire list. Uses Default Comparer. Sorts the collection passed.
        /// </summary>
        /// <typeparam name="T">Type used within the collections and comparer</typeparam>
        /// <param name="collection">Collection to sort</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static void InlineSort<T>(IList<T> collection)
        {
            InlineSort(collection, Comparer<T>.Default);
        }

        /// <summary>
        /// Sort the list in a given range
        /// </summary>
        /// <param name="collection">Collection to sort</param>
        /// <param name="low">Low index of range to sort</param>
        /// <param name="high">High index of range to sort</param>
        /// <param name="comparer">Comparer to use in sorting the collection</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static List<object> Sort(IEnumerable collection, int low, int high, IComparer comparer)
        {
            return Sort(collection.Cast<object>(), low, high, new RoutedComparer(comparer));
        }

        /// <summary>
        /// Sort the list in a given range. Uses Default Comparer
        /// </summary>
        /// <param name="collection">Collection to sort</param>
        /// <param name="low">Low index of range to sort</param>
        /// <param name="high">High index of range to sort</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static List<object> Sort(IEnumerable collection, int low, int high)
        {
            return Sort(collection, low, high, Comparer.Default);
        }

        /// <summary>
        /// Sort the entire list
        /// </summary>
        /// <param name="collection">Collection to sort</param>
        /// <param name="comparer">Comparer to use in sorting the collection</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static List<object> Sort(IEnumerable collection, IComparer comparer)
        {
            return Sort(collection, 0, Int32.MaxValue, comparer);
        }

        /// <summary>
        /// Sort the entire list. Uses Default Comparer
        /// </summary>
        /// <param name="collection">Collection to sort</param>
        /// <returns>A new, sorted List with the original values</returns>
        public static List<object> Sort(IEnumerable collection)
        {
            return Sort(collection, Comparer.Default);
        }
    }
}
