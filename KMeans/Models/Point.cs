namespace KMeans.Models
{
    internal class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Cluster Cluster { get; set; }
        public double DistanceToCentroid { get; set; }
    }
}