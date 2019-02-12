using System;
using BinaryTree;

namespace Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "result.bin";

            var tree = new BinaryTree<string>();
            tree.Add("This is a test");
            tree.Add("This is a test");
            tree.Add("This is a test");
            tree.Add("bli");
            tree.Add("bla");

            tree.Serialize(fileName);

            Console.WriteLine(tree.Equals(BinaryTree<string>.Deserialize(fileName)) ? "Workz!" : "Failed..");
        }
    }
}
