using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.DataStructures
{
    // Undirected weighted graph modeling relationships (nodes are TrackingId strings)
    public class Graph
    {
        private readonly Dictionary<string, List<(string to, double weight)>> _adj = new();

        public void AddVertex(string id) { if (!_adj.ContainsKey(id)) _adj[id] = new List<(string, double)>(); }

        public void AddEdge(string a, string b, double weight = 1.0)
        {
            AddVertex(a); AddVertex(b);
            _adj[a].Add((b, weight));
            _adj[b].Add((a, weight));
        }

        public List<string> BFS(string start)
        {
            var res = new List<string>();
            if (!_adj.ContainsKey(start)) return res;
            var q = new Queue<string>();
            var vis = new HashSet<string> { start };
            q.Enqueue(start);
            while (q.Any())
            {
                var u = q.Dequeue();
                res.Add(u);
                foreach (var e in _adj[u])
                    if (!vis.Contains(e.to)) { vis.Add(e.to); q.Enqueue(e.to); }
            }
            return res;
        }

        public List<string> DFS(string start)
        {
            var res = new List<string>();
            if (!_adj.ContainsKey(start)) return res;
            var vis = new HashSet<string>();
            var st = new Stack<string>();
            st.Push(start);
            while (st.Any())
            {
                var u = st.Pop();
                if (vis.Contains(u)) continue;
                vis.Add(u);
                res.Add(u);
                foreach (var e in _adj[u]) if (!vis.Contains(e.to)) st.Push(e.to);
            }
            return res;
        }

        // Prim's MST for connected component containing 'start'
        public List<(string from, string to, double weight)> PrimMST(string start)
        {
            var mst = new List<(string, string, double)>();
            if (!_adj.ContainsKey(start)) return mst;
            var visited = new HashSet<string> { start };
            var pq = new EdgeMinHeap();
            foreach (var e in _adj[start]) pq.Insert((e.weight, start, e.to));
            while (pq.Count > 0)
            {
                var (w, f, t) = pq.ExtractMin();
                if (visited.Contains(t)) continue;
                visited.Add(t);
                mst.Add((f, t, w));
                foreach (var e in _adj[t])
                    if (!visited.Contains(e.to)) pq.Insert((e.weight, t, e.to));
            }
            return mst;
        }

        public Dictionary<string, List<(string to, double weight)>> Snapshot() => _adj.ToDictionary(k => k.Key, v => v.Value.ToList());
    }

    // Edge min heap to support Prim's algorithm (custom)
    public class EdgeMinHeap
    {
        private List<(double weight, string from, string to)> heap = new();
        public int Count => heap.Count;
        private int Parent(int i) => (i - 1) / 2;
        private int Left(int i) => 2 * i + 1;
        private int Right(int i) => 2 * i + 2;
        private void Swap(int a, int b) { var t = heap[a]; heap[a] = heap[b]; heap[b] = t; }

        public void Insert((double weight, string from, string to) e)
        {
            heap.Add(e);
            int i = heap.Count - 1;
            while (i > 0 && heap[i].weight < heap[Parent(i)].weight) { Swap(i, Parent(i)); i = Parent(i); }
        }

        public (double weight, string from, string to) ExtractMin()
        {
            var root = heap[0];
            heap[0] = heap[^1];
            heap.RemoveAt(heap.Count - 1);
            MinHeapify(0);
            return root;
        }

        private void MinHeapify(int i)
        {
            int l = Left(i), r = Right(i), smallest = i;
            if (l < heap.Count && heap[l].weight < heap[smallest].weight) smallest = l;
            if (r < heap.Count && heap[r].weight < heap[smallest].weight) smallest = r;
            if (smallest != i) { Swap(i, smallest); MinHeapify(smallest); }
        }
    }
}
