using SNcP.Models;
using System.Collections.Generic;
using System.Linq;

namespace SNcP
{
    internal class DisjointSets
    {
        private readonly List<Set> _sets;

        internal DisjointSets()
        {
            _sets = new List<Set>();
        }

        public void AddEdgeInSet(Edge edge)
        {
            Set setA = GetSet(edge.VertexX);
            Set setB = GetSet(edge.VertexY);

            if (setA is not null && setB is null)
            {
                setA.AddEdge(edge);
            }
            else if (setA is null && setB is not null)
            {
                setB.AddEdge(edge);
            }
            else if (setA is null && setB is null)
            {
                var set = new Set(edge);

                _sets.Add(set);
            }
            else if (setA is not null && setB is not null)
            {
                if (!setA.Equals(setB))
                {
                    setA.Union(setB, edge);
                    _sets.Remove(setB);
                }
            }
        }

        public Algorithm GetAlgorithm()
        {
            return _sets.First().Algorithm;
        }

        public Set GetSet(Vertex vertex)
        {
            return _sets.FirstOrDefault(s => s.Vertices.Contains(vertex));
        }
    }
}