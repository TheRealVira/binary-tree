using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinaryTree
{
    /// <summary>
    ///     Represents a binary tree <seealso href="https://en.wikipedia.org/wiki/Binary_tree" />.
    /// </summary>
    /// <typeparam name="T">The type which this tree should contain.</typeparam>
    [Serializable]
    [DataContract]
    public class BinaryTree<T> where T : IComparable<T>
    {
        [DataMember] private Node _parentNode;

        public BinaryTree()
        {
            Debug.WriteLine("Initializing binary-tree..");

            _parentNode = new Node();
        }

        /// <summary>
        ///     Adds the specific item inside the binary-tree using binary-tree logic.
        /// </summary>
        /// <param name="item">The item that will be added.</param>
        public void Add(T item)
        {
            Debug.WriteLine($"Adding '{item}' in this binary-tree..");

            var currentNode = _parentNode;
            while (true)
            {
                if (currentNode.Item.data == null || currentNode.Item.count.Equals(0))
                {
                    currentNode.Item.data = item;
                    currentNode.Item.count = 1;
                    return;
                }

                var comparisonValue = item.CompareTo(currentNode.Item.data);

                if (comparisonValue > 0)
                {
                    if (currentNode.Right == null)
                    {
                        currentNode.Right = new Node(item);
                        return;
                    }

                    Debug.WriteLine($"{item} > {currentNode.Item.data} → move to right lower node..");

                    currentNode = currentNode.Right;
                }
                else if (comparisonValue.Equals(0))
                {
                    currentNode.Item.count++;
                    return;
                }
                else
                {
                    if (currentNode.Left == null)
                    {
                        currentNode.Left = new Node(item);
                        return;
                    }

                    Debug.WriteLine($"{item} < {currentNode.Item.data} → move to left lower node..");

                    currentNode = currentNode.Left;
                }
            }
        }

        /// <summary>
        ///     Removes the specific item inside the binary-tree using binary-tree logic.
        /// </summary>
        /// <param name="item">The item that will be removed.</param>
        /// <param name="fullyRemoval">When set on true will remove said item without any respect towards their count.</param>
        public void Remove(T item, bool fullyRemoval = false)
        {
            Debug.WriteLine($"Removing '{item}' in this binary-tree..");

            var currentNode = _parentNode;
            Node prevNode = default;

            while (true)
            {
                if (currentNode.Item.data == null) return;

                var comparisonValue = item.CompareTo(currentNode.Item.data);

                if (comparisonValue > 0)
                {
                    if (currentNode.Right == null) return;

                    (prevNode, currentNode) = (currentNode, currentNode.Right);
                }
                else if (comparisonValue.Equals(0))
                {
                    if (currentNode.Item.count > 1 && !fullyRemoval)
                        currentNode.Item.count--;
                    else
                        SkipNode(prevNode, currentNode);

                    return;
                }
                else
                {
                    if (currentNode.Left == null) return;

                    (prevNode, currentNode) = (currentNode, currentNode.Left);
                }
            }
        }

        private void SkipNode(Node prevNode, Node nodeToSkip)
        {
            var countOfFurtherNodes = 0;

            if (nodeToSkip.Right != null) countOfFurtherNodes++;

            if (nodeToSkip.Left != null) countOfFurtherNodes++;
            if (prevNode == null)
                switch (countOfFurtherNodes)
                {
                    case 0:
                        nodeToSkip.Item = (default, 0);
                        return;
                    case 1:
                        var correctNode = nodeToSkip.Right ?? nodeToSkip.Left;
                        nodeToSkip.Item = correctNode.Item;
                        nodeToSkip.Right = correctNode.Right;
                        nodeToSkip.Left = correctNode.Left;
                        return;
                    case 2:
                        var temp = SearchForLeftMostRightNode(nodeToSkip);
                        Remove(temp.Item.data, true);
                        temp.Left = nodeToSkip.Left;
                        temp.Right = nodeToSkip.Right;

                        nodeToSkip.Item = temp.Item;
                        return;
                }

            var rightPath = nodeToSkip.Item.data.CompareTo(prevNode.Item.data);
            switch (countOfFurtherNodes)
            {
                case 0 when rightPath > 0:
                    prevNode.Right = null;
                    break;
                case 0:
                    prevNode.Left = null;
                    break;
                case 1 when rightPath > 0:
                    prevNode.Right = nodeToSkip.Right ?? nodeToSkip.Left;
                    break;
                case 1:
                    prevNode.Left = nodeToSkip.Right ?? nodeToSkip.Left;
                    break;
                case 2 when rightPath > 0:
                    var temp = SearchForLeftMostRightNode(nodeToSkip);
                    Remove(temp.Item.data, true);
                    temp.Left = nodeToSkip.Left;
                    temp.Right = nodeToSkip.Right;

                    prevNode.Right = temp;
                    break;
                case 2:
                    var temp2 = SearchForLeftMostRightNode(nodeToSkip);
                    Remove(temp2.Item.data, true);
                    temp2.Left = nodeToSkip.Left;
                    temp2.Right = nodeToSkip.Right;

                    prevNode.Left = temp2;
                    break;
            }
        }

        private Node SearchForLeftMostRightNode(Node node)
        {
            if (node.Right == null) return node;

            node = node.Right;

            while (node.Left != null) node = node.Left;

            return node;
        }

        /// <summary>
        ///     Searches for a specific item inside this binary-tree using binary-tree logic.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns the count of said item if existing - 0 if otherwise.</returns>
        public int Contains(T item)
        {
            Debug.WriteLine($"Searching '{item}' in this binary-tree..");

            var currentNode = _parentNode;
            while (true)
            {
                if (currentNode.Item.data == null) return 0;

                var comparisionValue = item.CompareTo(currentNode.Item.data);

                if (comparisionValue > 0)
                {
                    if (currentNode.Right == null) return 0;

                    Debug.WriteLine($"{item} > {currentNode.Item.data} → search in right lower node..");

                    currentNode = currentNode.Right;
                }
                else if (comparisionValue.Equals(0))
                {
                    return currentNode.Item.count;
                }
                else
                {
                    if (currentNode.Left == null) return 0;


                    Debug.WriteLine($"{item} < {currentNode.Item.data} → search in left lower node..");

                    currentNode = currentNode.Left;
                }
            }
        }

        /// <summary>
        ///     Clears everything out of this binary-tree.
        /// </summary>
        public void Clear()
        {
            Debug.WriteLine("Clearing this binary-tree..");

            _parentNode = new Node();
        }

        public override bool Equals(object obj)
        {
            return obj is BinaryTree<T> tree && tree._parentNode.Equals(_parentNode);
        }

        public void Serialize(string file)
        {
            using (var writer = new StreamWriter(file))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(writer.BaseStream, this);
            }
        }

        public static BinaryTree<T> Deserialize(string file)
        {
            using (var reader = new StreamReader(file))
            {
                var bf = new BinaryFormatter();
                return (BinaryTree<T>) bf.Deserialize(reader.BaseStream);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_parentNode);
        }

        /// <summary>
        ///     Represents a node of a binary-tree. This may be either a simple node, a parent, or a leaf.
        /// </summary>
        [Serializable]
        [DataContract]
        private class Node
        {
            /// <summary>
            ///     Saved data and count of data - inside this specific node.
            /// </summary>
            [DataMember] internal (T data, int count) Item;

            /// <summary>
            ///     Left "lower" arm of current node - this is where everything smaller than this node is getting redirect towards.
            /// </summary>
            [DataMember] internal Node Left;

            /// <summary>
            ///     Right "lower" arm of current node - this is where everything bigger than this node is getting redirect towards.
            /// </summary>
            [DataMember] internal Node Right;

            internal Node(T item)
            {
                Item = (item, 1);
            }

            internal Node()
            {
            }

            public override bool Equals(object obj)
            {
                return obj is Node node &&
                       (node.Right?.Equals(Right) ?? Right == null) &&
                       (node.Item.data?.CompareTo(Item.data).Equals(0) ?? Item.data == null) &&
                       node.Item.count.Equals(Item.count) &&
                       (node.Left?.Equals(Left) ?? Left == null);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Right, Item, Left);
            }
        }
    }
}