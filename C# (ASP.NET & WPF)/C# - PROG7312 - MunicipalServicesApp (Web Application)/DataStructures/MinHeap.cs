using System;
using System.Collections.Generic;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.DataStructures
{
    // Specialized min-heap for ServiceRequest using Priority (lower is higher priority)
    public class MinHeap
    {
        private List<ServiceRequest> _items = new();

        public int Count => _items.Count;

        private void Swap(int a, int b) { var t = _items[a]; _items[a] = _items[b]; _items[b] = t; }

        public void Insert(ServiceRequest r)
        {
            _items.Add(r);
            int i = _items.Count - 1;
            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (_items[i].Priority < _items[p].Priority) { Swap(i, p); i = p; } else break;
            }
        }

        public ServiceRequest? Peek() => _items.Count == 0 ? null : _items[0];

        public ServiceRequest? ExtractMin()
        {
            if (_items.Count == 0) return null;
            var root = _items[0];
            _items[0] = _items[^1];
            _items.RemoveAt(_items.Count - 1);
            Heapify(0);
            return root;
        }

        private void Heapify(int i)
        {
            int left = 2 * i + 1, right = left + 1, smallest = i;
            if (left < _items.Count && _items[left].Priority < _items[smallest].Priority) smallest = left;
            if (right < _items.Count && _items[right].Priority < _items[smallest].Priority) smallest = right;
            if (smallest != i) { Swap(i, smallest); Heapify(smallest); }
        }

        // Non destructive sorted list of items by priority (lowest first)
        public List<ServiceRequest> ToSortedList()
        {
            var copy = new MinHeap();
            foreach (var i in _items) copy.Insert(i);
            var list = new List<ServiceRequest>();
            while (copy.Count > 0) list.Add(copy.ExtractMin()!);
            return list;
        }
    }
}
