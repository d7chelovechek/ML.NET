using SNcP.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SNcP.Custom
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
                DrawEdges(drawingContext);
                DrawVertices(drawingContext);
            }
        }

        private void DrawEdges(DrawingContext drawingContext)
        {
            var pen = new Pen(
                Brushes.LightGray,
                2);

            foreach (Edge edge in Algorithm.Edges)
            {
                drawingContext.DrawLine(
                    pen,
                    new Point(
                        edge.VertexX.Point.X * ActualWidth / Algorithm.FirstSize.Width,
                        edge.VertexX.Point.Y * ActualHeight / Algorithm.FirstSize.Height),
                    new Point(
                        edge.VertexY.Point.X * ActualWidth / Algorithm.FirstSize.Width,
                        edge.VertexY.Point.Y * ActualHeight / Algorithm.FirstSize.Height));
            }
        }

        private void DrawVertices(DrawingContext drawingContext)
        {
            var pen = new Pen(
                Brushes.Black,
                1);

            foreach (Vertex vertex in Algorithm.Vertices)
            {
                drawingContext.DrawEllipse(
                    Brushes.LightGray,
                    pen,
                    new Point(
                        vertex.Point.X * ActualWidth / Algorithm.FirstSize.Width,
                        vertex.Point.Y * ActualHeight / Algorithm.FirstSize.Height),
                    10,
                    10);
            }
        }
    }
}