using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    // Generic BST using key selector returning int key (Id)
    public class BinarySearchTree<T>
    {
        public class Node
        {
            public T Data;
            public Node? Left;
            public Node? Right;
            public Node(T data) { Data = data; }
        }

        private Node? Root;
        private readonly Func<T, int> _keySelector;

        public BinarySearchTree(Func<T, int> keySelector) { _keySelector = keySelector; }

        public void Insert(T item) { Root = Insert(Root, item); }

        private Node Insert(Node? node, T item)
        {
            if (node == null) return new Node(item);
            var key = _keySelector(item);
            var k = _keySelector(node.Data);
            if (key < k) node.Left = Insert(node.Left, item);
            else node.Right = Insert(node.Right, item);
            return node;
        }

        public T? SearchByKey(int key)
        {
            var cur = Root;
            while (cur != null)
            {
                var k = _keySelector(cur.Data);
                if (key == k) return cur.Data;
                cur = key < k ? cur.Left : cur.Right;
            }
            return default;
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
