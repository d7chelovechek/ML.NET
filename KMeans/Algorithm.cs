using KMeans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace KMeans
{
    internal class Algorithm
    {
        public List<Point> Points { get; private set; }
        public System.Windows.Size FirstSize { get; private set; }
        public double SumOfDistance { get; private set; }
        public bool IsFinished { get; private set; }

        private readonly List<Cluster> _clusters;
        private readonly int _clustersCount;

        private bool _flag;

        public delegate void DrawHandler(
            Algorithm algorithm);
        public event DrawHandler Draw;

        internal Algorithm(List<Point> points, int clustersCount, System.Windows.Size size)
        {
            _clusters = new List<Cluster>();
            Points = points;
            _clustersCount = clustersCount;

            FirstSize = size;

            InitializeClusters(size);
        }

        private void InitializeClusters(System.Windows.Size size)
        {
            var random = new Random();

            for (var index = 0; index < _clustersCount; index++)
            {
                _clusters.Add(new Cluster()
                {
                    CentroidX = random.Next(20, (int)size.Width - 20),
                    CentroidY = random.Next(20, (int)size.Height - 20),
                    Brush = new SolidColorBrush(Color.FromRgb(
                        (byte)random.Next(0, 255),
                        (byte)random.Next(0, 255),
                        (byte)random.Next(0, 255)))
                });
            }
        }

        public void NextStep()
        {
            if (_flag)
            {
                MoveCentroids();
            }
            else
            {
                UpdateClusters();
            }

            Draw?.Invoke(this);
            _flag = !_flag;
        }

        private void UpdateClusters()
        {
            _clusters.ForEach(c => c.Points = new List<Point>());

            foreach (Point point in Points)
            {
                var min = double.MaxValue;
                Cluster tempCluster = null;

                foreach (Cluster cluster in _clusters)
                {
                    var distance = 
                        Math.Pow(cluster.CentroidX - point.X, 2) +
                        Math.Pow(cluster.CentroidY - point.Y, 2);

                    if (distance < min)
                    {
                        min = distance;
                        tempCluster = cluster;
                    }
                }

                tempCluster.Points.Add(point);
                point.Cluster = tempCluster;
                point.DistanceToCentroid = min;
            }
        }

        private void MoveCentroids()
        {
            var isFinished = true;

            foreach (Cluster cluster in _clusters)
            {
                if (cluster.Points.Any())
                {
                    var x = 0d;
                    var y = 0d;

                    foreach (Point point in cluster.Points)
                    {
                        x += point.X;
                        y += point.Y;
                    }

                    var oldPoint = new System.Windows.Point(
                        cluster.CentroidX,
                        cluster.CentroidY);

                    cluster.CentroidX = x / cluster.Points.Count;
                    cluster.CentroidY = y / cluster.Points.Count;

                    if (!oldPoint.Equals(new System.Windows.Point(
                        cluster.CentroidX, 
                        cluster.CentroidY)))
                    {
                        isFinished = false;
                    }
                }
            }

            if (isFinished)
            {
                Points.ForEach(p => SumOfDistance += p.DistanceToCentroid);
                IsFinished = true;
            }
        }
    }
}