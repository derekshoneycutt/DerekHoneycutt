using System;
using System.Collections.Generic;

namespace VagabondLib.Collections
{
    /// <summary>
    /// Read only BTree Structure
    /// </summary>
    /// <typeparam name="TDataType">Type of Key Data</typeparam>
    public class ReadOnlyBTree<TDataType> : IBTree<TDataType> where TDataType : IComparable
    {
        /// <summary>
        /// True BTree structure
        /// </summary>
        private BTree<TDataType> trueTree;

        /// <summary>
        /// Get a matching key from the structure
        /// </summary>
        /// <param name="key">Key to find the matching key for</param>
        /// <returns>The matching key</returns>
        public Tuple<BTreeNode<TDataType>, int> this[object key]
        {
            get
            {
                return trueTree.Search(key);
            }
        }

        /// <summary>
        /// Gets the number of items in the collection
        /// </summary>
        public int Count
        {
            get { return trueTree.Count; }
        }

        /// <summary>
        /// Gets whether the collection is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        public ReadOnlyBTree(BTree<TDataType> readTree)
        {
            trueTree = readTree;
        }

        /// <summary>
        /// Gets BTree Node data for a matching index key
        /// </summary>
        /// <param name="key">Key to search for</param>
        /// <returns>Tuple containing the node and key index</returns>
        public Tuple<BTreeNode<TDataType>, int> Search(object key)
        {
            return trueTree.Search(key);
        }

        /// <summary>
        /// Gets BTree Node data for a matching index key
        /// </summary>
        /// <param name="compare">Function comparing an index and returning whether the desired value is greater than, less than, or equal to</param>
        /// <returns>Tuple containing the node and key index</returns>
        public Tuple<BTreeNode<TDataType>, int> Search(Func<TDataType, int> compare)
        {
            return trueTree.Search(compare);
        }

        /// <summary>
        /// Determines whether a given item is present within the BTree
        /// </summary>
        /// <param name="item">Item to look for in the BTree</param>
        /// <returns>Whether the item is present in the collection</returns>
        public bool Contains(TDataType item)
        {
            return trueTree.Contains(item);
        }

        /// <summary>
        /// Gets a range of BTree Node datas according to a comparison function
        /// </summary>
        /// <param name="compare">Comparison function used to determine items</param>
        /// <returns>Range of matching BTree Nodes in Tupes with a node and key index</returns>
        public IEnumerable<Tuple<BTreeNode<TDataType>, int>> RangeSearch(Func<TDataType, int> compare)
        {
            return trueTree.RangeSearch(compare);
        }

        public IEnumerator<TDataType> GetEnumerator()
        {
            return trueTree.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return trueTree.GetEnumerator();
        }

        /// <summary>
        /// Throws InvalidOperationException
        /// </summary>
        /// <param name="item">throws InvalidOperationException</param>
        /// <exception cref="InvalidOperationException">Always</exception>
        public void Add(TDataType item)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Throws InvalidOperationException
        /// </summary>
        /// <param name="item">throws InvalidOperationException</param>
        /// <exception cref="InvalidOperationException">Always</exception>
        public void Clear()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Copy the BTree into an array
        /// </summary>
        /// <param name="array">Array to copy the BTree into</param>
        /// <param name="arrayIndex">Zero-based index of the array to start copying the data at</param>
        public void CopyTo(TDataType[] array, int arrayIndex)
        {
            trueTree.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Throws InvalidOperationException
        /// </summary>
        /// <param name="item">throws InvalidOperationException</param>
        /// <exception cref="InvalidOperationException">Always</exception>
        public bool Remove(TDataType item)
        {
            throw new InvalidOperationException();
        }
    }
}
