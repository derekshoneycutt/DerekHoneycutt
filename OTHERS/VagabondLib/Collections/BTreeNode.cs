using System;
using System.Collections.Generic;

namespace VagabondLib.Collections
{
    /// <summary>
    /// Contains all data for a BTree Node
    /// </summary>
    /// <typeparam name="TDataType">Type of data stored in Index data</typeparam>
    public class BTreeNode<TDataType> where TDataType : IComparable
    {
        private List<TDataType> m_Keys;
        private List<BTreeNode<TDataType>> m_Children;

        /// <summary>
        /// Gets the List of Keys stored in the node
        /// </summary>
        public List<TDataType> Keys { get { return m_Keys; } }
        /// <summary>
        /// Gets the list of all children nodes
        /// </summary>
        public List<BTreeNode<TDataType>> Children { get { return m_Children; } }

        private bool m_IsLeaf;
        /// <summary>
        /// Gets whether the item is a leaf node, containing only Index data
        /// </summary>
        public bool IsLeaf { get { return m_IsLeaf; } }

        public BTreeNode(bool isLeaf)
        {
            m_IsLeaf = isLeaf;

            m_Keys = new List<TDataType>();
            if (!m_IsLeaf)
            {
                m_Children = new List<BTreeNode<TDataType>>();
            }
        }

        public BTreeNode(bool isLeaf, int MarkDegrees)
        {
            m_IsLeaf = isLeaf;

            m_Keys = new List<TDataType>(MarkDegrees);
            if (!m_IsLeaf)
            {
                m_Children = new List<BTreeNode<TDataType>>(MarkDegrees + 1);
            }
        }

        /// <summary>
        /// Remove a child node and Make a leaf if no children remain
        /// </summary>
        /// <param name="index">Index of child node to remove</param>
        public void RemoveChild(int index)
        {
            m_Children.RemoveAt(index);
            if (m_Children.Count < 1)
            {
                m_Children = null;
                m_IsLeaf = true;
            }
        }
    }
}
