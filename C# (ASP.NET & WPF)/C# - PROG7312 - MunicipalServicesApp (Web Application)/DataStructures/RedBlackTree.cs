using System;

namespace MunicipalServicesApp.DataStructures
{
    public enum RBColor { Red, Black }

    public class RBNode<T> where T : IComparable<T>
    {
        public T? Data;
        public RBColor Color;
        public RBNode<T>? Left;
        public RBNode<T>? Right;
        public RBNode<T>? Parent;

        public RBNode(T? data)
        {
            Data = data;
            Color = RBColor.Red;
        }
    }

    // Generic Red-Black Tree for T : IComparable<T>
    public class RedBlackTree<T> where T : IComparable<T>
    {
        private readonly RBNode<T> _nullLeaf;
        private RBNode<T>? _root;

        public RedBlackTree()
        {
            _nullLeaf = new RBNode<T>(default(T)) { Color = RBColor.Black, Left = null, Right = null, Parent = null };
            _root = _nullLeaf;
        }

        public void Insert(T data)
        {
            var node = new RBNode<T>(data) { Left = _nullLeaf, Right = _nullLeaf, Parent = null };
            RBNode<T>? y = null;
            var x = _root;

            while (x != null && x != _nullLeaf)
            {
                y = x;
                if (data.CompareTo(x.Data!) < 0) x = x.Left;
                else x = x.Right;
            }

            node.Parent = y;
            if (y == null || y == _nullLeaf) _root = node;
            else if (data.CompareTo(y.Data!) < 0) y.Left = node;
            else y.Right = node;

            if (node.Parent == null) { node.Color = RBColor.Black; node.Parent = null; return; }
            if (node.Parent.Parent == null) return;
            FixInsert(node);
        }

        private void FixInsert(RBNode<T> k)
        {
            while (k.Parent != null && k.Parent.Color == RBColor.Red)
            {
                if (k.Parent == k.Parent.Parent?.Left)
                {
                    var u = k.Parent.Parent.Right;
                    if (u != null && u.Color == RBColor.Red)
                    {
                        k.Parent.Color = RBColor.Black;
                        u.Color = RBColor.Black;
                        k.Parent.Parent!.Color = RBColor.Red;
                        k = k.Parent.Parent!;
                    }
                    else
                    {
                        if (k == k.Parent.Right)
                        {
                            k = k.Parent;
                            RotateLeft(k);
                        }
                        k.Parent!.Color = RBColor.Black;
                        k.Parent.Parent!.Color = RBColor.Red;
                        RotateRight(k.Parent.Parent!);
                    }
                }
                else
                {
                    var u = k.Parent.Parent?.Left;
                    if (u != null && u.Color == RBColor.Red)
                    {
                        k.Parent.Color = RBColor.Black;
                        u.Color = RBColor.Black;
                        if (k.Parent.Parent != null) k.Parent.Parent.Color = RBColor.Red;
                        k = k.Parent.Parent!;
                    }
                    else
                    {
                        if (k == k.Parent.Left)
                        {
                            k = k.Parent;
                            RotateRight(k);
                        }
                        k.Parent!.Color = RBColor.Black;
                        k.Parent.Parent!.Color = RBColor.Red;
                        RotateLeft(k.Parent.Parent!);
                    }
                }
                if (k == _root) break;
            }
            if (_root != null) _root.Color = RBColor.Black;
        }

        private void RotateLeft(RBNode<T> x)
        {
            var y = x.Right!;
            x.Right = y.Left;
            if (y.Left != _nullLeaf) y.Left!.Parent = x;
            y.Parent = x.Parent;
            if (x.Parent == null) _root = y;
            else if (x == x.Parent.Left) x.Parent.Left = y;
            else x.Parent.Right = y;
            y.Left = x;
            x.Parent = y;
        }

        private void RotateRight(RBNode<T> x)
        {
            var y = x.Left!;
            x.Left = y.Right;
            if (y.Right != _nullLeaf) y.Right!.Parent = x;
            y.Parent = x.Parent;
            if (x.Parent == null) _root = y;
            else if (x == x.Parent.Right) x.Parent.Right = y;
            else x.Parent.Left = y;
            y.Right = x;
            x.Parent = y;
        }

        // Helpers for UI metrics
        public int CountNodes() => CountNodes(_root);
        private int CountNodes(RBNode<T>? node)
        {
            if (node == null || node == _nullLeaf || node.Data == null) return 0;
            return 1 + CountNodes(node.Left) + CountNodes(node.Right);
        }

        public int GetHeight() => GetHeight(_root);
        private int GetHeight(RBNode<T>? node)
        {
            if (node == null || node == _nullLeaf || node.Data == null) return 0;
            return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        public T? GetMin()
        {
            var cur = _root;
            if (cur == null || cur == _nullLeaf) return default;
            while (cur.Left != _nullLeaf) cur = cur.Left!;
            return cur.Data;
        }

        // Export tree to simple JSON-like object for front-end visualization
        public object? ExportAsJson()
        {
            return ExportNode(_root);
        }

        private object? ExportNode(RBNode<T>? node)
        {
            if (node == null || node == _nullLeaf || node.Data == null) return null;
            return new
            {
                value = node.Data.ToString(),
                color = node.Color == RBColor.Red ? "red" : "black",
                left = ExportNode(node.Left),
                right = ExportNode(node.Right)
            };
        }
    }
}
