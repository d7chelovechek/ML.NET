using SNcP.Models;
using System;
using System.Globalization;
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
                DrawWeights(drawingContext);
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

        private void DrawWeights(DrawingContext drawingContext)
        {
            foreach (Edge edge in Algorithm.Edges)
            {
                var formattedText = new FormattedText(
                    edge.Weight.ToString(),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(
                        new FontFamily("Segoe UI"),
                        FontStyles.Normal,
                        FontWeights.SemiBold,
                        FontStretches.Normal),
                    10,
                    Brushes.Black,
                    null,
                    TextFormattingMode.Ideal);

                drawingContext.DrawText(
                    formattedText,
                    new Point(
                        (Math.Max(edge.VertexX.Point.X, edge.VertexY.Point.X) - 
                            Math.Abs(edge.VertexX.Point.X - edge.VertexY.Point.X) / 2 -
                            formattedText.Width / 2) * ActualWidth / Algorithm.FirstSize.Width,
                        (Math.Max(edge.VertexX.Point.Y, edge.VertexY.Point.Y) - 
                            Math.Abs(edge.VertexX.Point.Y - edge.VertexY.Point.Y) / 2 - 
                            formattedText.Height / 2) * ActualHeight / Algorithm.FirstSize.Height));
            }
        }
    }
}