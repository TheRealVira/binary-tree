using System.Collections.Generic;
using System.Linq;
using BinaryTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private BinaryTree<int> _testTree;

        [TestInitialize]
        public void InitializeTest()
        {
            _testTree = new BinaryTree<int>();
        }

        [TestMethod]
        public void AddTest()
        {
            _testTree.Add(123);
            Assert.AreEqual(_testTree.Contains(123), 1);

            _testTree.Add(12);
            _testTree.Add(12);
            Assert.AreEqual(_testTree.Contains(12), 2);

            _testTree.Add(324);
            Assert.AreEqual(_testTree.Contains(324), 1);

            _testTree.Add(3);
            Assert.AreEqual(_testTree.Contains(3), 1);

            _testTree.Add(333);
            Assert.AreEqual(_testTree.Contains(333), 1);
        }

        [TestMethod]
        public void ClearTest()
        {
            _testTree.Clear();
            Assert.AreEqual(_testTree.Contains(123), 0);
        }

        [TestMethod]
        public void RemoveTest()
        {
            _testTree.Clear();
            _testTree.Add(123);
            _testTree.Add(12);
            _testTree.Add(12);
            _testTree.Add(324);
            _testTree.Add(3);
            _testTree.Add(333);

            _testTree.Remove(123);
            Assert.AreEqual(_testTree.Contains(123), 0);

            _testTree.Remove(12);
            Assert.AreEqual(_testTree.Contains(12), 1);

            _testTree.Clear();
            _testTree.Add(5);
            _testTree.Remove(5);
            Assert.AreEqual(_testTree.Contains(5), 0);
        }

        [TestMethod]
        public void SerializeTest()
        {
            const string fileName = "testfile.bin";

            _testTree.Clear();
            _testTree.Add(123);
            _testTree.Add(12);
            _testTree.Add(12);
            _testTree.Add(324);
            _testTree.Add(3);
            _testTree.Add(333);

            _testTree.Serialize(fileName);

            Assert.AreEqual(BinaryTree<int>.Deserialize(fileName), _testTree);
        }

        [TestMethod]
        public void TestCombinations()
        {
            // Test all sequences of 5 operations drawn from adding and removing four distinct elements.
            for (var i = 0; i < 1 << 15; i++)
            {
                var list = new List<int>();
                var tree = new BinaryTree<int>();

                var program = i;
                for (var j = 0; j < 5; j++)
                {
                    var instruction = program & 1;
                    program >>= 1;
                    var value = program & 3;
                    program >>= 2;

                    if (instruction == 0)
                    {
                        list.Add(value);
                        tree.Add(value);
                    }
                    else
                    {
                        list.Remove(value);
                        tree.Remove(value);
                    }

                    for (value = 0; value < 4; value++)
                        Assert.AreEqual(list.Count(elt => elt == value), tree.Contains(value));
                }
            }
        }
    }
}