using System;
using System.Collections;
using System.Collections.Generic;

namespace VagabondLib.Collections
{
    /// <summary>
    /// Class used to handle generic, automatically sorted lists, without sorted indices
    /// </summary>
    /// <typeparam name="T">Type to store and sort</typeparam>
    public class SortedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        private List<T> m_Stored;
        private IComparer<T> m_Comparer;

        /// <summary>
        /// Initialize the sorted list. Empty, with default comparer
        /// </summary>
        public SortedList()
        {
            m_Stored = new List<T>();
            m_Comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initialize the sorted list. Empty, with specified comparer
        /// </summary>
        /// <param name="comparer">Comparer object to utilize in sorting operations</param>
        public SortedList(IComparer<T> comparer)
        {
            m_Stored = new List<T>();
            m_Comparer = comparer;
        }

        /// <summary>
        /// Initialize the sorted list. Empty, specified capacity, with default comparer
        /// </summary>
        /// <param name="capacity">Specified starting capacity of the list</param>
        public SortedList(int capacity)
        {
            m_Stored = new List<T>(capacity);
            m_Comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initialize the sorted list. Empty, specified capacity, with specified comparer
        /// </summary>
        /// <param name="capacity">Specified starting capacity of the list</param>
        /// <param name="comparer">Comparer object to utilize in sorting operations</param>
        public SortedList(int capacity, IComparer<T> comparer)
        {
            m_Stored = new List<T>(capacity);
            m_Comparer = comparer;
        }

        /// <summary>
        /// Initialize the sorted list. Start with specified collection, default comparer
        /// </summary>
        /// <param name="collection">Collection to start with. Automatically sorted</param>
        public SortedList(IEnumerable<T> collection)
        {
            m_Stored = QuickSort.Sort(collection, Comparer<T>.Default);
            m_Comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initialize the sorted list. Start with specified collection, specified comparer
        /// </summary>
        /// <param name="collection">Collection to start with. Automatically sorted</param>
        /// <param name="comparer">Comparer object to utilize in sorting operations</param>
        public SortedList(IEnumerable<T> collection, IComparer<T> comparer)
        {
            m_Stored = QuickSort.Sort(collection, comparer);
            m_Comparer = comparer;
        }

        /// <summary>
        /// Get the index of a specified item, if it exists in the list
        /// </summary>
        /// <param name="item">Item to get the index of</param>
        /// <returns>0 based index of the specified item in the list</returns>
        public int IndexOf(T item)
        {
            return BinarySearch.Search(m_Stored, item);
        }

        /// <summary>
        /// Get the index of a specified item, if it exists in the list
        /// </summary>
        /// <param name="item">Item to get the index of</param>
        /// <returns>0 based index of the specified item in the list</returns>
        public int IndexOf(object value)
        {
            if (value is T)
            {
                return this.IndexOf((T)value);
            }
            return Int32.MinValue;
        }

        /// <summary>
        /// Throws InvalidOperationException
        /// </summary>
        /// <param name="index">Throws InvalidOperationException</param>
        /// <param name="item">Throws InvalidOperationException</param>
        /// <exception cref="InvalidOperationException">Always</exception>
        public void Insert(int index, T item)
        {
            throw new InvalidOperationException("Insert is not allowed on SortedList<T>");
        }

        /// <summary>
        /// Throws InvalidOperationException
        /// </summary>
        /// <param name="index">Throws InvalidOperationException</param>
        /// <param name="item">Throws InvalidOperationException</param>
        /// <exception cref="InvalidOperationException">Always</exception>
        public void Insert(int index, object value)
        {
            throw new InvalidOperationException("Insert is not allowed on SortedList<T>");
        }

        /// <summary>
        /// Remove the item at the specified index
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        public void RemoveAt(int index)
        {
            m_Stored.RemoveAt(index);
        }

        /// <summary>
        /// Gets the item at the specified index. Set operation throws InvalidOperationException
        /// </summary>
        /// <param name="index">Index to Get. (Set operation throws InvalidOperationException)</param>
        public T this[int index]
        {
            get
            {
                return m_Stored[index];
            }
            set
            {
                throw new InvalidOperationException("Setting value at specific index is not allowed in SortedList<T>");
            }
        }

        /// <summary>
        /// Gets the item at the specified index. Set operation throws InvalidOperationException
        /// </summary>
        /// <param name="index">Index to Get. (Set operation throws InvalidOperationException)</param>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new InvalidOperationException("Setting value at specific index is not allowed in SortedList<T>");
            }
        }

        /// <summary>
        /// Insert a new item into the list, maintaining the list to be sorted
        /// </summary>
        /// <param name="newItem">New item to insert into the list</param>
        /// <returns>New index of item</returns>
        public int InsertSorted(T newItem)
        {
            int index = BinarySearch.Search(m_Stored, newItem, m_Comparer);
            if (index < 0)
            {
                index = ~index;
            }
            m_Stored.Insert(index, newItem);
            return index;
        }

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="item">New item to add to the list</param>
        public void Add(T item)
        {
            InsertSorted(item);
        }

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="item">New item to add to the list</param>
        void ICollection<T>.Add(T item)
        {
            InsertSorted(item);
        }

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="item">New item to add to the list</param>
        /// <exception cref="ArgumentException">value is not of type T</exception>
        public int Add(object value)
        {
            if (value is T)
            {
                return InsertSorted((T)value);
            }
            throw new ArgumentException("Can only add items of generic type to SortedList<T>");
        }

        /// <summary>
        /// Clear all items from the list
        /// </summary>
        public void Clear()
        {
            m_Stored.Clear();
        }

        /// <summary>
        /// Clear all items from the list
        /// </summary>
        void ICollection<T>.Clear()
        {
            m_Stored.Clear();
        }

        /// <summary>
        /// Determine if the list contains a given item
        /// </summary>
        /// <param name="item">Item to look for in the list</param>
        /// <returns>Whether the item exists in the list</returns>
        public bool Contains(T item)
        {
            return (BinarySearch.Search(m_Stored, item, m_Comparer) >= 0);
        }

        /// <summary>
        /// Determine if the list contains a given item
        /// </summary>
        /// <param name="item">Item to look for in the list</param>
        /// <returns>Whether the item exists in the list</returns>
        bool ICollection<T>.Contains(T item)
        {
            return this.Contains(item);
        }

        /// <summary>
        /// Determine if the list contains a given item
        /// </summary>
        /// <param name="item">Item to look for in the list</param>
        /// <returns>Whether the item exists in the list</returns>
        public bool Contains(object value)
        {
            if (value is T)
            {
                return this.Contains((T)value);
            }
            return false;
        }

        /// <summary>
        /// Copy the list to an array, starting at a given index
        /// </summary>
        /// <param name="array">Array to copy items to</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Stored.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copy the list to an array, starting at a given index
        /// </summary>
        /// <param name="array">Array to copy items to</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            m_Stored.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copy the list to an array, starting at a given index. Only copies if Array is of type T[]
        /// </summary>
        /// <param name="array">Array to copy items to</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(Array array, int index)
        {
            if (array is T[])
            {
                m_Stored.CopyTo((T[])array, index);
            }
        }

        /// <summary>
        /// Copy the list to an array, starting at a given index. Only copies if Array is of type T[]
        /// </summary>
        /// <param name="array">Array to copy items to</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        void ICollection.CopyTo(Array array, int index)
        {
            if (array is T[])
            {
                m_Stored.CopyTo((T[])array, index);
            }
        }

        /// <summary>
        /// Gets the number of items in the list
        /// </summary>
        public int Count
        {
            get { return m_Stored.Count; }
        }

        /// <summary>
        /// Gets the number of items in the list
        /// </summary>
        int ICollection<T>.Count
        {
            get { return m_Stored.Count; }
        }

        /// <summary>
        /// Gets the number of items in the list
        /// </summary>
        int ICollection.Count
        {
            get { return m_Stored.Count; }
        }

        /// <summary>
        /// Gets whether the list is treated as read only (always False)
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether the list is treated as read only (always False)
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Remove an item from the list
        /// </summary>
        /// <param name="item">Item to remove from the list</param>
        /// <returns>Whether the item was found and removed</returns>
        public bool Remove(T item)
        {
            var findItem = BinarySearch.Search(m_Stored, item);
            if (findItem >= 0)
            {
                m_Stored.RemoveAt(findItem);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove an item from the list
        /// </summary>
        /// <param name="item">Item to remove from the list</param>
        /// <returns>Whether the item was found and removed</returns>
        bool ICollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }

        /// <summary>
        /// Remove an item from the list
        /// </summary>
        /// <param name="item">Item to remove from the list</param>
        /// <returns>Whether the item was found and removed</returns>
        public void Remove(object value)
        {
            if (value is T)
            {
                this.Remove((T)value);
            }
        }

        /// <summary>
        /// Gets the enumerator for this list
        /// </summary>
        /// <returns>An enumerator to iterate over the items of the list</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return m_Stored.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for this list
        /// </summary>
        /// <returns>An enumerator to iterate over the items of the list</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Stored.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for this list
        /// </summary>
        /// <returns>An enumerator to iterate over the items of the list</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return m_Stored.GetEnumerator();
        }

        /// <summary>
        /// Gets whether the list is of a fixed size (always False)
        /// </summary>
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether the list is synchronized and thread-safe (always False)
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether the list is synchronized and thread-safe (always False)
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a null object -- There is no Sync Root for this
        /// </summary>
        public object SyncRoot
        {
            get { return null; }
        }

        /// <summary>
        /// Gets a null object -- There is no Sync Root for this
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return null; }
        }
    }
}
