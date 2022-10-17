using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace KMeans
{
    public partial class MainWindow : Window
    {
        private Algorithm _algorithm;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PointsGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _algorithm?.NextStep();
        }

        private void PointsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var points = new List<Models.Point>();
            var random = new Random();

            for (var index = 0; index < 200; index++)
            {
                points.Add(new Models.Point()
                {
                    X = random.Next(
                        10,
                        (int)PointsGrid.ActualWidth - 10),
                    Y = random.Next(
                        10,
                        (int)PointsGrid.ActualHeight - 10)
                });
            }

            var size = new Size(PointsGrid.ActualWidth, PointsGrid.ActualHeight);
            var clustersCount = GetOptimalClustersCount(points, size);

            _algorithm = new Algorithm(
                points,
                clustersCount,
                size);
            _algorithm.Draw += (Algorithm algorithm) => 
            {
                PointsGrid.Algorithm = algorithm;
                PointsGrid.InvalidateVisual();
            };
        }

        private static int GetOptimalClustersCount(List<Models.Point> points, Size size)
        {
            var sumOfDistance = new List<double>() { 0d };
            for (var index = 1; index < 2 * (int)Math.Sqrt(points.Count); index++)
            {
                var algorithm = new Algorithm(points, index, size);

                while (!algorithm.IsFinished)
                {
                    algorithm.NextStep();
                }

                sumOfDistance.Add(algorithm.SumOfDistance);
            }

            var clustersCount = 0;
            var min = double.MaxValue;
            for (var index = 2; index < sumOfDistance.Count - 1; index++)
            {
                var temp = Math.Abs(sumOfDistance[index] - sumOfDistance[index + 1]) / 
                    Math.Abs(sumOfDistance[index] - sumOfDistance[index - 1]);

                if (temp < min)
                {
                    min = temp;
                    clustersCount = index;
                }
            }

            return clustersCount;
        }
    }
}