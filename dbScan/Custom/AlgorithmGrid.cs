using dbScan.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace dbScan.Custom
{
    internal class AlgorithmGrid : Grid
    {
        public Dictionary<Cluster, List<Point>> Clusters { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Draw(drawingContext);
        }

        private void Draw(DrawingContext drawingContext)
        {
            if (Clusters is not null)
            {
                DrawPoints(drawingContext);
            }
        }

        private void DrawPoints(DrawingContext drawingContext)
        {
            foreach (KeyValuePair<Cluster, List<Point>> cluster in Clusters)
            {
                foreach (Point point in cluster.Value)
                {
                    drawingContext.DrawEllipse(
                        cluster.Key.Brush,
                        new Pen(Brushes.Black, 1),
                        new System.Windows.Point(
                            point.X,
                            point.Y),
                        10,
                        10);
                }
            }
        }
    }
}