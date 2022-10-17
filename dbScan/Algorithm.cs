using dbScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace dbScan
{
    internal class Algorithm
    {
        private readonly List<Point> _points;
        private readonly double _eps;
        private readonly int _m;

        private readonly List<Point> _visitedPoints;
        private readonly List<Point> _clusteredPoints;

        public Dictionary<Cluster, List<Point>> Clusters { get; private set; }
        private readonly Cluster _currentCluster;
        private readonly Cluster _noiseCluster;

        internal Algorithm(List<Point> points, double eps, int m)
        {
            _points = points;
            _eps = eps;
            _m = m;

            _visitedPoints = new List<Point>();
            _clusteredPoints = new List<Point>();

            _noiseCluster = new Cluster()
            {
                Brush = Brushes.LightGray
            };

            Clusters = new Dictionary<Cluster, List<Point>>()
            {
                {
                    _noiseCluster,
                    new List<Point>() 
                }
            };

            foreach (Point point in _points)
            {
                if (!_visitedPoints.Contains(point))
                {
                    _visitedPoints.Add(point);

                    var neighbours = GetNeighboursByPoint(point);

                    if (neighbours.Count < m)
                    {
                        Clusters[_noiseCluster].Add(point);
                    }
                    else
                    {
                        _currentCluster = new Cluster()
                        {
                            Brush = GetClusterBrush(Clusters.Count)
                        };

                        UpdateCluster(point, neighbours);
                    }
                }
            }
        }

        private static SolidColorBrush GetClusterBrush(int currentClusterId)
        {
            switch (currentClusterId)
            {
                case 1:
                    {
                        return Brushes.LightGreen;
                    }
                case 2:
                    {
                        return Brushes.LightYellow;
                    }
                case 3:
                    {
                        return Brushes.IndianRed;
                    }
                default:
                    {
                        return Brushes.LightGray;
                    }
            }
        }

        private void UpdateCluster(Point point, List<Point> neighbours)
        {
            if (!Clusters.ContainsKey(_currentCluster))
            {
                Clusters.Add(_currentCluster, new List<Point>());
            }

            Clusters[_currentCluster].Add(point);
            _clusteredPoints.Add(point);

            while (neighbours.Any())
            {
                var neighbour = neighbours[0];
                neighbours.Remove(neighbour);

                if (!_visitedPoints.Contains(neighbour))
                {
                    _visitedPoints.Add(neighbour);

                    var tempNeighbours = GetNeighboursByPoint(neighbour);
                    if (tempNeighbours.Count >= _m)
                    {
                        neighbours = neighbours
                            .Concat(tempNeighbours)
                            .Distinct()
                            .ToList();
                    }
                }

                if (!_clusteredPoints.Contains(neighbour))
                {
                    Clusters[_currentCluster].Add(neighbour);
                    _clusteredPoints.Add(neighbour);
                    
                    if (Clusters[_noiseCluster].Contains(neighbour))
                    {
                        Clusters[_noiseCluster].Remove(neighbour);
                    }
                }
            }
        }

        private List<Point> GetNeighboursByPoint(Point point)
        {
            var neighbours = new List<Point>();

            foreach (Point neighbour in 
                _points.Except(new List<Point>() { point }))
            {
                var distance = Math.Sqrt(
                    Math.Pow(neighbour.X - point.X, 2) +
                    Math.Pow(neighbour.Y - point.Y, 2));

                if (distance < _eps)
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }
    }
}