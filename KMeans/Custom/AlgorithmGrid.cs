using KMeans.Models;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace KMeans.Custom
{
    internal class AlgorithmGrid : Grid
    {
        public Algorithm Algorithm { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Draw(drawingContext);
        }

        private void Draw(DrawingContext drawingContext)
        {
            if (Algorithm is not null)
            {
                DrawPoints(drawingContext);
                DrawLines(drawingContext);
                DrawCentroids(drawingContext);
            }
        }

        private void DrawPoints(DrawingContext drawingContext)
        {
            foreach (Point point in Algorithm.Points)
            {
                drawingContext.DrawEllipse(
                    point.Cluster.Brush,
                    new Pen(Brushes.Black, 1),
                    new System.Windows.Point(
                        point.X * ActualWidth / Algorithm.FirstSize.Width, 
                        point.Y * ActualHeight / Algorithm.FirstSize.Height),
                    10,
                    10);
            }
        }

        private void DrawLines(DrawingContext drawingContext)
        {
            foreach (Point point in Algorithm.Points)
            {
                drawingContext.DrawLine(
                    new Pen(point.Cluster.Brush, 2),
                    new System.Windows.Point(
                        point.X * ActualWidth / Algorithm.FirstSize.Width,
                        point.Y * ActualHeight / Algorithm.FirstSize.Height),
                    new System.Windows.Point(
                        point.Cluster.CentroidX * ActualWidth / Algorithm.FirstSize.Width,
                        point.Cluster.CentroidY * ActualHeight / Algorithm.FirstSize.Height));
            }
        }

        private void DrawCentroids(DrawingContext drawingContext)
        {
            foreach (Cluster cluster in Algorithm.Points.Select(p => p.Cluster).Distinct())
            {
                drawingContext.DrawEllipse(
                    cluster.Brush,
                    new Pen(Brushes.Black, 2),
                    new System.Windows.Point(
                        cluster.CentroidX * ActualWidth / Algorithm.FirstSize.Width,
                        cluster.CentroidY * ActualHeight / Algorithm.FirstSize.Height),
                    20,
                    20);
            }
        }
    }
}