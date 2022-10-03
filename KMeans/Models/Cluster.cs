using System.Collections.Generic;
using System.Windows.Media;

namespace KMeans.Models
{
    internal class Cluster
    {
        public double CentroidX { get; set; }
        public double CentroidY { get; set; }
        public SolidColorBrush Brush { get; set; }
        public List<Point> Points { get; set; }

        internal Cluster()
        {
            Points = new List<Point>();
        }
    }
}