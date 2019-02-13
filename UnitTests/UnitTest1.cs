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
    }
}
