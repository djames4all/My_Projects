using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    // Generic AVL Tree using int key selector
    public class AVLTree<T>
    {
        public class Node
        {
            public T Data;
            public Node? Left;
            public Node? Right;
            public int Height;
            public Node(T d) { Data = d; Height = 1; }
        }

        private Node? Root;
        private readonly Func<T, int> _keySelector;

        public AVLTree(Func<T, int> keySelector) { _keySelector = keySelector; }

        private int Height(Node? n) => n?.Height ?? 0;
        private int Balance(Node? n) => n == null ? 0 : Height(n.Left) - Height(n.Right);

        private Node RightRotate(Node y)
        {
            var x = y.Left!;
            var T2 = x.Right;
            x.Right = y;
            y.Left = T2;
            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
            return x;
        }

        private Node LeftRotate(Node x)
        {
            var y = x.Right!;
            var T2 = y.Left;
            y.Left = x;
            x.Right = T2;
            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
            return y;
        }

        public void Insert(T item) { Root = Insert(Root, item); }

        private Node Insert(Node? node, T item)
        {
            if (node == null) return new Node(item);

            var key = _keySelector(item);
            var k = _keySelector(node.Data);

            if (key < k) node.Left = Insert(node.Left, item);
            else if (key > k) node.Right = Insert(node.Right, item);
            else return node; // ignore duplicate keys

            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
            int balance = Balance(node);

            // LL
            if (balance > 1 && key < _keySelector(node.Left!.Data)) return RightRotate(node);
            // RR
            if (balance < -1 && key > _keySelector(node.Right!.Data)) return LeftRotate(node);
            // LR
            if (balance > 1 && key > _keySelector(node.Left!.Data))
            {
                node.Left = LeftRotate(node.Left!);
                return RightRotate(node);
            }
            // RL
            if (balance < -1 && key < _keySelector(node.Right!.Data))
            {
                node.Right = RightRotate(node.Right!);
                return LeftRotate(node);
            }

            return node;
        }

        public List<T> InOrder()
        {
            var list = new List<T>();
            InOrder(Root, list);
            return list;
        }

        private void InOrder(Node? node, List<T> list)
        {
            if (node == null) return;
            InOrder(node.Left, list);
            list.Add(node.Data);
            InOrder(node.Right, list);
        }
    }
}
