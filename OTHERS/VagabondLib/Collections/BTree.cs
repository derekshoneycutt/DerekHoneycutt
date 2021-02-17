using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VagabondLib.Collections
{
    /// <summary>
    /// Class containing a BTree structure
    /// </summary>
    /// <typeparam name="TDataType">Type of data stored in Index data</typeparam>
    public class BTree<TDataType> : IBTree<TDataType> where TDataType : IComparable
    {
        /// <summary>
        /// Provides default comparisons of items in the BTree
        /// </summary>
        private class DefaultComparer : IComparer<TDataType>, IComparer
        {
            public int Compare(TDataType x, TDataType y)
            {
                return x.CompareTo(y);
            }

            public int Compare(object x, object y)
            {
                if (x is TDataType)
                {
                    return ((TDataType)x).CompareTo(y);
                }
                else if (y is TDataType)
                {
                    return ((TDataType)y).CompareTo(x) * -1;
                }
                throw new ArgumentException("One item being compared with BTree<TDataType>.DefaultComparer must be of type TDataType. Neither item passed is.");
            }
        }

        /// <summary>
        /// Contains index data for iterating over a BTree Node
        /// </summary>
        private class NodeIndex
        {
            /// <summary>
            /// Node to interate over
            /// </summary>
            public BTreeNode<TDataType> Node { get; set; }
            /// <summary>
            /// Current index to iterate over
            /// </summary>
            public int Index { get; set; }
            /// <summary>
            /// Current child in the node to iterate over (-1 = previous, 0 = index key, 1 = next)
            /// </summary>
            public int Child { get; set; }
            /// <summary>
            /// Gets and Sets an abstract Tag associated to the index
            /// </summary>
            public object Tag { get; set; }

            public NodeIndex(BTreeNode<TDataType> inNode)
            {
                Node = inNode;
                Index = 0;
                Tag = null;
                if (!Node.IsLeaf)
                {
                    Child = -1;
                }
                else
                {
                    Child = 0;
                }
            }

            public NodeIndex(BTreeNode<TDataType> inNode, int startChild)
            {
                Node = inNode;
                Index = 0;
                Tag = null;
                Child = startChild;
            }

            /// <summary>
            /// Move the index to the next item in the node
            /// </summary>
            /// <returns>Whether the node can be iterated further</returns>
            public bool MoveNext()
            {
                if (Index < Node.Keys.Count - 1)
                {
                    if (!Node.IsLeaf)
                    {
                        if (Child < 1)
                        {
                            ++Child;
                        }
                        else
                        {
                            Child = 0;
                            ++Index;
                        }
                    }
                    else
                    {
                        ++Index;
                    }

                    return true;
                }
                else
                {
                    if (!Node.IsLeaf)
                    {
                        if (Child < 1)
                        {
                            ++Child;
                            return true;
                        }
                    }
                    ++Index;
                    return false;
                }
            }

            /// <summary>
            /// Iterate to the next item in the node, not counting children
            /// </summary>
            /// <param name="nullTag">Whether to set the Tag to null while iterating</param>
            /// <returns>Whether the node can be iterated further</returns>
            public bool MoveNextInNode(bool nullTag = false)
            {
                if (Child >= 0)
                {
                    ++Index;
                    if (nullTag)
                    {
                        Tag = null;
                    }
                }
                Child = 0;

                if (Index < Node.Keys.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Member variable defining the maximum number of children nodes
        /// </summary>
        private int m_MaxDegree;
        /// <summary>
        /// Member variable defining the splitting point of overloaded nodes
        /// </summary>
        private int m_SplitDegree;

        /// <summary>
        /// Gets the maximum number of children nodes allowed during insertion
        /// </summary>
        public int MaxDegree { get { return m_MaxDegree; } }

        /// <summary>
        /// Root node of the BTree
        /// </summary>
        private BTreeNode<TDataType> rootNode;

        /// <summary>
        /// Comparer interface used to compare index datas
        /// </summary>
        private IComparer m_Comparer;

        /// <summary>
        /// Gets BTree Node data for a matching index key
        /// </summary>
        /// <param name="key">Key to search for</param>
        /// <returns>Tuple containing the node and key index</returns>
        public Tuple<BTreeNode<TDataType>, int> this[object key]
        {
            get
            {
                return Search(key);
            }
        }

        public int Count { get; private set; }

        public BTree(int maxDegree)
        {
            Count = 0;
            m_MaxDegree = maxDegree;
            m_SplitDegree = (m_MaxDegree - 1) / 2;

            rootNode = new BTreeNode<TDataType>(true, MaxDegree);

            m_Comparer = new DefaultComparer();
        }

        public BTree(int maxDegree, IComparer comparer)
        {
            Count = 0;
            m_MaxDegree = maxDegree;
            m_SplitDegree = (m_MaxDegree - 1) / 2;

            rootNode = new BTreeNode<TDataType>(true, MaxDegree);

            m_Comparer = comparer;
        }

        /// <summary>
        /// Search a given node for a matching key
        /// </summary>
        /// <param name="node">Node to start the search in</param>
        /// <param name="compare">Function comparing an index and returning whether the desired value is greater than, less than, or equal to</param>
        /// <returns>Tuple containing the node and key index</returns>
        private Tuple<BTreeNode<TDataType>, int> Search(BTreeNode<TDataType> node, Func<TDataType, int> compare)
        {
            int i = 0;
            int lastCmp = 0;
            while ((i < node.Keys.Count) && ((lastCmp = compare(node.Keys[i])) > 0))
            {
                ++i;
            }
            if (i < node.Keys.Count)
            {
                if (lastCmp == 0)
                {
                    return new Tuple<BTreeNode<TDataType>, int>(node, i);
                }
            }

            if (node.IsLeaf)
            {
                return null;
            }
            else
            {
                if (i >= node.Children.Count)
                {
                    return null;
                }
                return Search(node.Children[i], compare);
            }
        }

        /// <summary>
        /// Gets BTree Node data for a matching index key
        /// </summary>
        /// <param name="compare">Function comparing an index and returning whether the desired value is greater than, less than, or equal to</param>
        /// <returns>Tuple containing the node and key index</returns>
        public Tuple<BTreeNode<TDataType>, int> Search(Func<TDataType, int> compare)
        {
            return Search(rootNode, compare);
        }

        /// <summary>
        /// Gets BTree Node data for a matching index key
        /// </summary>
        /// <param name="key">Key to search for</param>
        /// <returns>Tuple containing the node and key index</returns>
        public Tuple<BTreeNode<TDataType>, int> Search(object key)
        {
            return Search((testKey) => m_Comparer.Compare(testKey, key) * -1);
        }

        /// <summary>
        /// Determine if the BTree contains a specified item
        /// </summary>
        /// <param name="item">Item to search for</param>
        /// <returns>Whether the BTree contains the specified item</returns>
        public bool Contains(TDataType item)
        {
            var search = Search(item);
            return (search != null);
        }

        /// <summary>
        /// Determine if the BTree contains a specified key
        /// </summary>
        /// <param name="item">Key to search for</param>
        /// <returns>Whether the BTree contains the specified item</returns>
        public bool Contains(object key)
        {
            var search = Search(key);
            return (search != null);
        }

        /// <summary>
        /// Gets a range of BTree Node datas according to a comparison function
        /// </summary>
        /// <param name="compare">Comparison function used to determine items</param>
        /// <returns>Range of matching BTree Nodes in Tupes with a node and key index</returns>
        public IEnumerable<Tuple<BTreeNode<TDataType>, int>> RangeSearch(Func<TDataType, int> compare)
        {
            Stack<NodeIndex> nodeStack = new Stack<NodeIndex>();
            if (rootNode.Keys.Count > 0)
            {
                nodeStack.Push(new NodeIndex(rootNode, 0));
            }

            while (nodeStack.Count > 0)
            {
                var current = nodeStack.Peek();

                switch (current.Child)
                {
                    case -1:
                        nodeStack.Push(new NodeIndex(current.Node.Children[current.Index], 0));
                        break;
                    case 0:
                        if (current.Index >= current.Node.Keys.Count)
                        {
                            nodeStack.Pop();
                            if (nodeStack.Count > 0)
                            {
                                current = nodeStack.Peek();
                                current.MoveNextInNode(true);
                            }
                        }
                        else
                        {
                            int cmp = (current.Tag == null) ? compare(current.Node.Keys[current.Index]) : (int)current.Tag;
                            if (cmp == 0)
                            {
                                if (current.Tag == null)
                                {
                                    current.Tag = cmp;
                                    if ((!current.Node.IsLeaf) && (current.Index < 1))
                                    {
                                        current.Child = -1;
                                    }
                                }
                                else
                                {
                                    yield return new Tuple<BTreeNode<TDataType>, int>(current.Node, current.Index);
                                    if (current.Node.IsLeaf)
                                    {
                                        current.MoveNextInNode(true);
                                    }
                                    else
                                    {
                                        current.Child = 1;
                                    }
                                }
                            }
                            else if (cmp > 0)
                            {
                                if ((!current.Node.IsLeaf) && (current.Index < 1) && (current.Tag == null))
                                {
                                    current.Tag = cmp;
                                    current.Child = -1;
                                }
                                else
                                {
                                    current.Index = current.Node.Keys.Count;
                                }
                            }
                            else if ((!current.Node.IsLeaf) && (current.Tag == null))
                            {
                                current.Tag = cmp;
                                current.Child = 1;
                            }
                            else
                            {
                                current.MoveNextInNode(true);
                            }
                        }
                        break;
                    case 1:
                        nodeStack.Push(new NodeIndex(current.Node.Children[current.Index + 1], 0));
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Test if a child node is overloaded and split it if it is
        /// </summary>
        /// <param name="parent">Parent node of the suspected overloaded node</param>
        /// <param name="childIndex">Child index expected to be overloaded</param>
        private void SplitOverloaded(BTreeNode<TDataType> parent, int childIndex)
        {
            var overloaded = parent.Children[childIndex];
            if (overloaded.Keys.Count >= m_MaxDegree)
            {
                var newNode = new BTreeNode<TDataType>(overloaded.IsLeaf, MaxDegree);

                for (int copyIndex = 0; copyIndex < m_SplitDegree; ++copyIndex)
                {
                    newNode.Keys.Add(overloaded.Keys[0]);
                    overloaded.Keys.RemoveAt(0);
                }
                if (!overloaded.IsLeaf)
                {
                    for (int copyIndex = 0; copyIndex <= m_SplitDegree; ++copyIndex)
                    {
                        newNode.Children.Add(overloaded.Children[0]);
                        overloaded.RemoveChild(0);
                    }
                }

                parent.Children.Insert(childIndex, newNode);

                parent.Keys.Insert(childIndex, overloaded.Keys[0]);
                overloaded.Keys.RemoveAt(0);
            }
        }

        /// <summary>
        /// Insert a new key into a child node
        /// </summary>
        /// <param name="parent">Parent node to insert a new key into the child of</param>
        /// <param name="childIndex">Index of the child to insert new key into</param>
        /// <param name="newKey">New key to insert into the child node</param>
        private void InsertInChild(BTreeNode<TDataType> parent, int childIndex, TDataType newKey)
        {
            int i = parent.Children[childIndex].Keys.Count - 1;
            int lastCmp = -1;

            while ((i >= 0) && ((lastCmp = m_Comparer.Compare(newKey, parent.Children[childIndex].Keys[i])) < 0))
            {
                --i;
            }

            if (lastCmp == 0)
            {
                parent.Children[childIndex].Keys[i] = newKey;
                return;
            }

            ++i;

            if (parent.Children[childIndex].IsLeaf)
            {
                if (lastCmp != 0)
                {
                    parent.Children[childIndex].Keys.Insert(i, newKey);
                    SplitOverloaded(parent, childIndex);
                }
            }
            else
            {
                if (lastCmp != 0)
                {
                    InsertInChild(parent.Children[childIndex], i, newKey);
                    SplitOverloaded(parent, childIndex);
                }
            }
        }

        /// <summary>
        /// Insert a new Key Data into the BTree
        /// </summary>
        /// <param name="newKey">New Key Data to insert into the BTree</param>
        private void InnerInsert(TDataType newKey)
        {
            var newRoot = new BTreeNode<TDataType>(false, MaxDegree);
            newRoot.Children.Add(rootNode);

            InsertInChild(newRoot, 0, newKey);
            if (newRoot.Keys.Count > 0)
            {
                rootNode = newRoot;
            }
        }

        /// <summary>
        /// Insert a new Key Data into the BTree
        /// </summary>
        /// <param name="newKey">New Key Data to insert into the BTree</param>
        public void Insert(TDataType newKey)
        {
            InnerInsert(newKey);
            Count = GetEnumerable().Count();
        }

        /// <summary>
        /// Insert a new Key Data into the BTree
        /// </summary>
        /// <param name="newKey">New Key Data to insert into the BTree</param>
        public void Add(TDataType newKey)
        {
            InnerInsert(newKey);
            Count = GetEnumerable().Count();
        }

        /// <summary>
        /// Insert a new range of Key Data into the BTree
        /// </summary>
        /// <param name="newKeys">New Key Datas to insert into the BTree</param>
        public void AddRange(IEnumerable<TDataType> newKeys)
        {
            foreach (var key in newKeys)
            {
                InnerInsert(key);
            }
            Count = GetEnumerable().Count();
        }

        /// <summary>
        /// Get the largest (right most) key data from a Node
        /// </summary>
        /// <param name="node">Node to get the largest data from</param>
        /// <returns>The largest key contained in the node</returns>
        private TDataType GetLargest(BTreeNode<TDataType> node)
        {
            if (node.IsLeaf)
            {
                return node.Keys[node.Keys.Count - 1];
            }
            else
            {
                return GetLargest(node.Children[node.Children.Count - 1]);
            }
        }

        /// <summary>
        /// Get the largest (left most) key data from a Node
        /// </summary>
        /// <param name="node">Node to get the smallest data from</param>
        /// <returns>The smallest key contained in the node</returns>
        private TDataType GetSmallest(BTreeNode<TDataType> node)
        {
            if (node.IsLeaf)
            {
                return node.Keys[0];
            }
            else
            {
                return GetSmallest(node.Children[0]);
            }
        }

        /// <summary>
        /// Rotate and Shift nodes to the left to correct imbalance
        /// </summary>
        /// <param name="upperNode">Upper Node to shift through</param>
        /// <param name="left">Left sided child node to shift to</param>
        /// <param name="right">Right sided child node to shift from</param>
        /// <returns>Tuple: Whether operation was completed; Whether any further imbalance needs to be corrected higher up the chain</returns>
        private Tuple<bool, bool> TryShiftLeft(BTreeNode<TDataType> upperNode, int left, int right)
        {
            if (upperNode.Children[right].Keys.Count > 1)
            {
                InsertInChild(upperNode, left, upperNode.Keys[left]);
                upperNode.Keys[left] = GetSmallest(upperNode.Children[right]);
                if (RemoveFromNode(upperNode.Children[right], upperNode.Keys[left]))
                {
                    return new Tuple<bool, bool>(true, RebalancePostDelete(upperNode, right));
                }
                else
                {
                    if (upperNode.Children[left].Keys.Count < 1)
                    {
                        return new Tuple<bool, bool>(true, RebalancePostDelete(upperNode, left));
                    }
                    return new Tuple<bool, bool>(true, false);
                }
            }
            return new Tuple<bool, bool>(false, false);
        }

        /// <summary>
        /// Rotate and Shift nodes to the right to correct imbalance
        /// </summary>
        /// <param name="upperNode">Upper Node to shift through</param>
        /// <param name="left">Left sided child node to shift from</param>
        /// <param name="right">Right sided child node to shift to</param>
        /// <returns>Tuple: Whether operation was completed; Whether any further imbalance needs to be corrected higher up the chain</returns>
        private Tuple<bool, bool> TryShiftRight(BTreeNode<TDataType> upperNode, int left, int right)
        {
            if (upperNode.Children[left].Keys.Count > 1)
            {
                InsertInChild(upperNode, right, upperNode.Keys[left]);
                upperNode.Keys[left] = GetLargest(upperNode.Children[left]);
                if (RemoveFromNode(upperNode.Children[left], upperNode.Keys[left]))
                {
                    return new Tuple<bool, bool>(true, RebalancePostDelete(upperNode, left));
                }
                else
                {
                    if (upperNode.Children[right].Keys.Count < 1)
                    {
                        return new Tuple<bool, bool>(true, RebalancePostDelete(upperNode, right));
                    }
                    return new Tuple<bool, bool>(true, false);
                }
            }
            return new Tuple<bool, bool>(false, false);
        }

        /// <summary>
        /// Merge deficient nodes to correct imbalance in a BTree
        /// </summary>
        /// <param name="upperNode">Parent node of nodes to be merged</param>
        /// <param name="deficient">Deficient node beginning the imbalance</param>
        /// <param name="highChild">Right-most child of the parent node</param>
        /// <param name="leftSib">Potential left sided sibling of the deficient node</param>
        /// <param name="rightSib">Potential right sided sibling of the deficient node</param>
        /// <returns>Whether further rebalancing needs to occur higher up the BTree chain</returns>
        private bool MergeDeficient(BTreeNode<TDataType> upperNode, int deficient, int highChild, int leftSib, int rightSib)
        {
            int mergeIntoNode = 0;
            int mergeFromNode = 0;
            TDataType mergeSep;
            if (deficient > 0)
            {
                mergeIntoNode = leftSib;
                mergeFromNode = deficient;
                mergeSep = upperNode.Keys[deficient - 1];
                upperNode.Keys.RemoveAt(deficient - 1);
            }
            else if (deficient < highChild)
            {
                mergeIntoNode = deficient;
                mergeFromNode = rightSib;
                mergeSep = upperNode.Keys[deficient];
                upperNode.Keys.RemoveAt(deficient);
            }
            else
            {
                return (upperNode.Keys.Count < 1);
            }

            upperNode.Children[mergeIntoNode].Keys.Add(mergeSep);
            upperNode.Children[mergeIntoNode].Keys.AddRange(upperNode.Children[mergeFromNode].Keys);
            if (!upperNode.Children[mergeFromNode].IsLeaf)
            {
                if (!upperNode.Children[mergeIntoNode].IsLeaf)
                {
                    upperNode.Children[mergeIntoNode].Children.AddRange(upperNode.Children[mergeFromNode].Children);
                }
            }
            upperNode.RemoveChild(mergeFromNode);
            if (upperNode.Keys.Count < 1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Rebalance a node after a deletion has completed
        /// </summary>
        /// <param name="upperNode">Upper node to rebalance on</param>
        /// <param name="deficient">Now deficient node to rebalance</param>
        /// <returns>Whether further rebalancing needs to occur up the chain of the BTree</returns>
        private bool RebalancePostDelete(BTreeNode<TDataType> upperNode, int deficient)
        {
            int highChild = upperNode.Children.Count - 1;
            int rightSib = deficient + 1;
            int leftSib = deficient - 1;

            if (deficient < highChild)
            {
                var ret = TryShiftLeft(upperNode, deficient, rightSib);
                if (ret.Item1)
                {
                    return ret.Item2;
                }
            }

            if (deficient > 0)
            {
                var ret = TryShiftRight(upperNode, leftSib, deficient);
                if (ret.Item1)
                {
                    return ret.Item2;
                }
            }

            return MergeDeficient(upperNode, deficient, highChild, leftSib, rightSib);
        }

        /// <summary>
        /// Remove a matching key from a BTree Node
        /// </summary>
        /// <param name="node">Node to remove any match from</param>
        /// <param name="key">Key to find and remove</param>
        /// <returns>Whether the node needs to be rebalanced now that the deletion has occurred</returns>
        private bool RemoveFromNode(BTreeNode<TDataType> node, object key)
        {
            int i = 0;
            int lastCmp = 0;
            while ((i < node.Keys.Count) && ((lastCmp = m_Comparer.Compare(node.Keys[i], key)) < 0))
            {
                ++i;
            }

            if (i < node.Keys.Count)
            {
                if (lastCmp == 0)
                {
                    if (node.IsLeaf)
                    {
                        node.Keys.RemoveAt(i);
                        if (node.Keys.Count < 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (node.Children[i].Keys.Count > 0)
                        {
                            node.Keys[i] = GetLargest(node.Children[i]);
                            if (RemoveFromNode(node.Children[i], node.Keys[i]))
                            {
                                return RebalancePostDelete(node, i);
                            }
                        }
                        else
                        {
                            node.RemoveChild(i);
                            node.Keys.RemoveAt(i);
                            if (node.Keys.Count < 1)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }

            if (!node.IsLeaf)
            {
                if (RemoveFromNode(node.Children[i], key))
                {
                    return RebalancePostDelete(node, i);
                }
            }
            return false;
        }

        /// <summary>
        /// Remove a matching key from the BTree
        /// </summary>
        /// <param name="matchKey">Key to find and remove</param>
        public void Remove(object matchKey)
        {
            if (RemoveFromNode(rootNode, matchKey))
            {
                if (!rootNode.IsLeaf && (rootNode.Keys.Count < 1))
                {
                    rootNode = rootNode.Children[0];
                }
            }
            Count = GetEnumerable().Count();
        }

        /// <summary>
        /// Remove a matching key from the BTree
        /// </summary>
        /// <param name="matchKey">Key to find and remove</param>
        /// <returns>Always returns true, contrary to anticipated; Assumed to always delete</returns>
        public bool Remove(TDataType item)
        {
            this.Remove(item);
            return false;
        }

        /// <summary>
        /// Clear out the BTree structure to be empty
        /// </summary>
        public void Clear()
        {
            rootNode = new BTreeNode<TDataType>(true, MaxDegree);
            Count = 0;
        }

        /// <summary>
        /// Get all of the Index Datas
        /// </summary>
        /// <returns>Enumerable of all items</returns>
        private IEnumerable<TDataType> GetEnumerable()
        {
            var nodeStack = new Stack<NodeIndex>();
            if (rootNode.Keys.Count > 0)
            {
                nodeStack.Push(new NodeIndex(rootNode));
            }
            while (nodeStack.Count > 0)
            {
                var current = nodeStack.Peek();
                switch (current.Child)
                {
                    case -1:
                        nodeStack.Push(new NodeIndex(current.Node.Children[current.Index]));
                        break;
                    case 0:
                        yield return current.Node.Keys[current.Index];

                        while (!current.MoveNext())
                        {
                            nodeStack.Pop();
                            if (nodeStack.Count < 1)
                            {
                                yield break;
                            }
                            current = nodeStack.Peek();
                        }
                        break;
                    case 1:
                        nodeStack.Push(new NodeIndex(current.Node.Children[current.Index + 1]));
                        break;
                    default:
                        yield break;
                }
            }
        }

        public IEnumerator<TDataType> GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Get the BTree as a Read Only collection
        /// </summary>
        /// <returns>A read only BTree pointer to the current structure</returns>
        public ReadOnlyBTree<TDataType> AsReadOnly()
        {
            return new ReadOnlyBTree<TDataType>(this);
        }

        /// <summary>
        /// Copy the BTree into an array
        /// </summary>
        /// <param name="array">Array to copy the BTree into</param>
        /// <param name="arrayIndex">Zero-based index of the array to start copying the data at</param>
        public void CopyTo(TDataType[] array, int arrayIndex)
        {
            var thisList = GetEnumerable().ToList();
            thisList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets whether this is a Read Only Collection
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}
