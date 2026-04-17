using System;
using System.Collections.Generic;

namespace B_Tree_Insertion
{
    public class BTreeNode
    {
        public List<int> Keys { get; set; }
        public List<BTreeNode> Children { get; set; }
        public bool IsLeaf { get; set; }
        public BTreeNode(int degree, bool isLeaf)
        {
            Keys = new List<int>();
            Children = new List<BTreeNode>();
            IsLeaf = isLeaf;
        }
    }

    public class BTree
    {
        private BTreeNode Root;
        private int Degree;
        public BTree(int degree)
        {
            Degree = degree;
            Root = new BTreeNode(degree, true);
        }

        public void Insert(int key)
        {
            if (Root.Keys.Count == (2 * Degree) - 1)
            {
                var newRoot = new BTreeNode(Degree, false);
                newRoot.Children.Add(Root);
                SplitChild(newRoot, 0);
                Root = newRoot;
            }
            InsertInLeafOrNonFullNode(Root, key);
        }

        private void InsertInLeafOrNonFullNode(BTreeNode node, int key)
        {
            int i = node.Keys.Count - 1;
            if (node.IsLeaf)
            {
                while (i >= 0 && key < node.Keys[i])
                {
                    i--;
                }
                node.Keys.Insert(i + 1, key);
            }
            else
            {
                while (i >= 0 && key < node.Keys[i])
                {
                    i--;
                }
                i++;

                if (node.Children[i].Keys.Count == (2 * Degree) - 1)
                {
                    SplitChild(node, i);
                    if (key > node.Keys[i])
                    {
                        i++;
                    }
                }
                InsertInLeafOrNonFullNode(node.Children[i], key);
            }
        }

        private void SplitChild(BTreeNode parent, int i)
        {
            var fullChild = parent.Children[i];
            var newChild = new BTreeNode(Degree, fullChild.IsLeaf);

            parent.Keys.Insert(i, fullChild.Keys[Degree - 1]);
            parent.Children.Insert(i + 1, newChild);

            newChild.Keys.AddRange(fullChild.Keys.GetRange(Degree, Degree - 1));
            fullChild.Keys.RemoveRange(Degree - 1, Degree);

            if (!fullChild.IsLeaf)
            {
                newChild.Children.AddRange(fullChild.Children.GetRange(Degree, Degree));
                fullChild.Children.RemoveRange(Degree, fullChild.Children.Count - Degree);
            }
        }

        public void Display(BTreeNode node, int level = 0)
        {
            if (node != null)
            {
                Console.WriteLine(new string(' ', level * 2) + "Keys: " + string.Join(", ", node.Keys));
                if (!node.IsLeaf)
                {
                    foreach (var child in node.Children)
                    {
                        Display(child, level + 1);
                    }
                }
            }
        }
        public BTreeNode GetRoot() => Root;
    }

    public class Program
    {
        public static void Main()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            var bTree = new BTree(3);

            bTree.Insert(10);
            bTree.Insert(20);
            bTree.Insert(5);
            bTree.Insert(6);
            bTree.Insert(12);
            bTree.Insert(30);
            bTree.Insert(7);
            bTree.Insert(17);
            bTree.Insert(25);
            bTree.Insert(40);
            bTree.Insert(50);
            bTree.Insert(60);

            Console.WriteLine("B-tree structure:");
            bTree.Display(bTree.GetRoot());
            Console.ReadLine();
        }
    }
}