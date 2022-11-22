using SVM.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace SVM.Custom
{
    internal class AlgorithmGrid : Grid
    {
        public (System.Windows.Point, System.Windows.Point) Line { get; set; }
        public List<Point> Points { get; private set; }
        public System.Windows.Size FirstSize { get; set; }

        public AlgorithmGrid()
        {
            Line = (new System.Windows.Point(0, 0), new System.Windows.Point(0, 0));
            FirstSize = System.Windows.Size.Empty;
            Points = new List<Point>();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Draw(drawingContext);
        }

        private void Draw(DrawingContext drawingContext)
        {
            DrawPoints(drawingContext);

            if (!Line.Item1.Equals(new System.Windows.Point(0, 0)) &&
                !Line.Item2.Equals(new System.Windows.Point(0, 0)))
            {
                drawingContext.DrawLine(
                    new Pen(Brushes.Black, 1),
                    new System.Windows.Point(
                        Line.Item1.X * ActualWidth / FirstSize.Width,
                        Line.Item1.Y * ActualHeight / FirstSize.Height),
                    new System.Windows.Point(
                        Line.Item2.X * ActualWidth / FirstSize.Width,
                        Line.Item2.Y * ActualHeight / FirstSize.Height));
            }
        }

        private void DrawPoints(DrawingContext drawingContext)
        {
            var pen = new Pen(Brushes.Black, 1);

            foreach (Point point in Points)
            {
                drawingContext.DrawEllipse(
                    point.Class is 0 ? Brushes.IndianRed : Brushes.LightBlue,
                    pen,
                    new System.Windows.Point(
                        point.Coordinate.X * ActualWidth / FirstSize.Width,
                        point.Coordinate.Y * ActualHeight / FirstSize.Height),
                    10,
                    10);
            }
        }
    }
}