using System;
using System.Collections.Generic;

namespace VagabondLib.Collections
{
    /// <summary>
    /// Interface defining a BTree Data Object
    /// </summary>
    /// <typeparam name="TDataType">Type of key data stored within the BTree structure</typeparam>
    public interface IBTree<TDataType> : IEnumerable<TDataType>, ICollection<TDataType> where TDataType : IComparable
    {
        /// <summary>
        /// Get a matching key from the structure
        /// </summary>
        /// <param name="key">Key to find the matching key for</param>
        /// <returns>The matching key</returns>
        Tuple<BTreeNode<TDataType>, int> this[object key]
        {
            get;
        }

        /// <summary>
        /// Gets BTree Node data for a matching index key
        /// </summary>
        /// <param name="key">Key to search for</param>
        /// <returns>Tuple containing the node and key index</returns>
        Tuple<BTreeNode<TDataType>, int> Search(object key);

        /// <summary>
        /// Gets BTree Node data for a matching index key
        /// </summary>
        /// <param name="compare">Function comparing an index and returning whether the desired value is greater than, less than, or equal to</param>
        /// <returns>Tuple containing the node and key index</returns>
        Tuple<BTreeNode<TDataType>, int> Search(Func<TDataType, int> compare);

        /// <summary>
        /// Gets a range of BTree Node datas according to a comparison function
        /// </summary>
        /// <param name="compare">Comparison function used to determine items</param>
        /// <returns>Range of matching BTree Nodes in Tupes with a node and key index</returns>
        IEnumerable<Tuple<BTreeNode<TDataType>, int>> RangeSearch(Func<TDataType, int> compare);
    }
}
