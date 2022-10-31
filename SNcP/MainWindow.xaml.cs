using SNcP.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace SNcP
{
    public partial class MainWindow : Window
    {
        private const int _vertexCount = 50;

        public MainWindow()
        {
            InitializeComponent();

            var random = new Random();
            var algorithm = new Algorithm(5)
            {
                FirstSize = new Size(Width, Height)
            };

            for (var index = 0; index < _vertexCount; index++)
            {
                for (var temp = 0; temp < _vertexCount; temp++)
                {
                    if (!index.Equals(temp) && 
                        random.Next(0, 2) is 0 &&
                        algorithm.GetEdgeById(index, temp) is null)
                    {
                        var edge = new Edge()
                        {
                            VertexX = algorithm.GetVertexById(index) ?? new Vertex()
                            {
                                Id = index,
                                Point = GetRandomPoint(random)
                            },
                            VertexY = algorithm.GetVertexById(temp) ?? new Vertex()
                            {
                                Id = temp,
                                Point = GetRandomPoint(random)
                            },
                            Weight = random.Next(1, 21)
                        };

                        edge.Weight = (int)Math.Sqrt(
                            Math.Pow(edge.VertexX.Point.X - edge.VertexY.Point.X, 2) +
                            Math.Pow(edge.VertexX.Point.Y - edge.VertexY.Point.Y, 2));

                        algorithm.Add(edge);
                    }
                }
            }

            GraphGrid.Algorithm = algorithm;
            GraphGrid.InvalidateVisual();
        }

        private Point GetRandomPoint(Random random)
        {
            return new Point(
                random.Next(20, (int)Width - 19),
                random.Next(20, (int)Height - 19));
        }

        private void PointsGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GraphGrid.Algorithm.NextStep();
            GraphGrid.InvalidateVisual();
        }
    }
}