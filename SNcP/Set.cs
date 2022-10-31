using SNcP.Models;
using System.Collections.Generic;

namespace SNcP
{
    internal class Set
    {
        public Algorithm Algorithm { get; set; }
        public List<Vertex> Vertices { get; set; }

        internal Set(Edge edge)
        {
            Algorithm = new Algorithm(edge);

            Vertices = new List<Vertex>
            {
                edge.VertexX,
                edge.VertexY
            };
        }

        public void Union(Set set, Edge connectingEdge)
        {
            Algorithm.Add(set.Algorithm);
            Algorithm.Add(connectingEdge);

            AddVertices(set.Vertices.ToArray());
        }

        public void AddEdge(Edge edge)
        {
            Algorithm.Add(edge);
            AddVertices(edge.VertexX, edge.VertexY);
        }

        private void AddVertices(params Vertex[] vertices)
        {
            foreach (Vertex vertex in vertices)
            {
                if (!Vertices.Contains(vertex))
                {
                    Vertices.Add(vertex);
                }
            }
        }

        public bool Contains(Vertex vertex)
        {
            return Vertices.Contains(vertex);
        }
    }
}