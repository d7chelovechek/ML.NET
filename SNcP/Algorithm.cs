using SNcP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SNcP
{
    internal class Algorithm
    {
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }

        public Size FirstSize { get; set; }

        private bool? _flag;
        private readonly int _clustersCount;

        internal Algorithm(int clustersCount)
        {
            Edges = new List<Edge>();
            Vertices = new List<Vertex>();

            _clustersCount = clustersCount - 1;
        }

        internal Algorithm(Edge edge)
        {
            Edges = new List<Edge>()
            {
                edge
            };

            Vertices = new List<Vertex>()
            {
                edge.VertexX,
                edge.VertexY
            };
        }

        public void Add(Algorithm algorithm)
        {
            foreach (Edge edge in algorithm.Edges)
            {
                AddElements(edge);
            }
        }

        private void AddElements(Edge edge)
        {
            if (!Edges.Contains(edge))
            {
                Edges.Add(edge);
            }

            if (!Vertices.Contains(edge.VertexX))
            {
                Vertices.Add(edge.VertexX);
            }
            if (!Vertices.Contains(edge.VertexY))
            {
                Vertices.Add(edge.VertexY);
            }
        }

        public void Add(Edge edge)
        {
            AddElements(edge);
        }

        private Algorithm GetMinimumSpanningTree()
        {
            var disjointSets = new DisjointSets();

            foreach (Edge edge in Edges.OrderBy(e => e.Weight))
            {
                disjointSets.AddEdgeInSet(edge);
            }

            return disjointSets.GetAlgorithm();
        }

        public Vertex GetVertexById(int id)
        {
            return Vertices.FirstOrDefault(v => v.Id.Equals(id));
        }

        public void NextStep()
        {
            if (_flag is null)
            {
                _flag = false;

                Edges = GetMinimumSpanningTree().Edges;
            }
            else if (_flag is false)
            {
                _flag = true;

                Edges = Edges.OrderBy(e => e.Weight).ToList();
                Edges.RemoveRange(Edges.Count - _clustersCount, _clustersCount);
            }
        }

        public Edge GetEdgeById(int index, int temp)
        {
            return Edges.FirstOrDefault(e =>
                (e.VertexX.Id.Equals(index) && e.VertexY.Id.Equals(temp)) ||
                (e.VertexX.Id.Equals(temp) && e.VertexY.Id.Equals(index)));
        }
    }
}