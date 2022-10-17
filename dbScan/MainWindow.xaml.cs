using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace dbScan
{
    public partial class MainWindow : Window
    {
        private readonly List<Models.Point> _points;

        private const double _eps = 100;
        private const int _m = 4;

        private bool _isMouseDown;
        private readonly Stopwatch _stopwatch;

        public MainWindow()
        {
            InitializeComponent();

            _points = new List<Models.Point>();

            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        private void PointsGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;

            AddNewPoint(e.GetPosition(PointsGrid));
        }

        private void AddNewPoint(Point point)
        {
            _points.Add(new Models.Point()
            {
                X = point.X,
                Y = point.Y
            });

            var algorithm = new Algorithm(_points, _eps, _m);

            PointsGrid.Clusters = algorithm.Clusters;
            PointsGrid.InvalidateVisual();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var point in _points)
            {
                point.X = point.X * e.NewSize.Width / e.PreviousSize.Width;
                point.Y = point.Y * e.NewSize.Height / e.PreviousSize.Height;
            }

            var algorithm = new Algorithm(_points, _eps, _m);

            PointsGrid.Clusters = algorithm.Clusters;
            PointsGrid.InvalidateVisual();
        }

        private void PointsGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void PointsGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown &&
                _stopwatch.ElapsedMilliseconds > 50)
            {
                AddNewPoint(e.GetPosition(PointsGrid));

                _stopwatch.Restart();
            }
        }
    }
}