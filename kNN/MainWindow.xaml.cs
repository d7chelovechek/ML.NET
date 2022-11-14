using kNN.Models;
using kNN.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace kNN
{
    public partial class MainWindow : Window
    {
        private ScatterChart[][] _charts;

        private List<Iris> _irises;

        private int _flag;

        public MainWindow()
        {
            InitializeComponent();

            LoadIrises();
        }

        private void LoadIrises()
        {
            _irises = new List<Iris>();

            var irises = File.ReadAllLines("Files/Iris.csv").ToList().Skip(1);

            foreach (string iris in irises)
            {
                var line = iris
                    .Split(',')
                    .Select(i => i.Replace('.', ','))
                    .ToArray();
                
                _irises.Add(new Iris()
                {
                    Id = GetIrisId(line[^1]),
                    Params = new double[4] 
                    { 
                        Convert.ToDouble(line[1]), 
                        Convert.ToDouble(line[2]), 
                        Convert.ToDouble(line[3]), 
                        Convert.ToDouble(line[4]) 
                    }
                });
            }

            _irises = _irises.OrderBy(x => Guid.NewGuid().ToString()).ToList();
        }

        private static int GetIrisId(string name)
        {
            return name switch
            {
                "Iris-setosa" => 0,
                "Iris-versicolor" => 1,
                "Iris-virginica" => 2,
                _ => -1
            };
        }

        private void InitializeCharts()
        {
            _charts = new ScatterChart[4][]
            {
                new ScatterChart[4],
                new ScatterChart[4],
                new ScatterChart[4],
                new ScatterChart[4]
            };

            for (var index = 1; index <= 4; index++)
            {
                for (var temp = 0; temp < 4; temp++)
                {
                    var radCartesianChart = new ScatterChart();

                    Grid.SetColumn(radCartesianChart, index);
                    Grid.SetRow(radCartesianChart, temp);

                    MainGrid.Children.Add(radCartesianChart);

                    _charts[index - 1][temp] = radCartesianChart;
                }
            }

            ShowChartDatas();
        }

        private void ShowChartDatas()
        {
            for (var index = 0; index < 4; index++)
            {
                for (var temp = 0; temp < 4; temp++)
                {
                    _charts[index][temp].HorizontalAxis.Minimum = 
                        _irises.Min(i => i.Params[index]);
                    _charts[index][temp].HorizontalAxis.Maximum =
                        _irises.Max(i => i.Params[index]);

                    _charts[index][temp].VerticalAxis.Minimum =
                        _irises.Min(i => i.Params[temp]);
                    _charts[index][temp].VerticalAxis.Maximum =
                        _irises.Max(i => i.Params[temp]);

                    _charts[index][temp].Points.ItemsSource = _irises
                        .Select(i => new ChartData(i.Id, i.Params[index], i.Params[temp]))
                        .ToList();
                }
            }
        }

        private void NormalizeIrises()
        {
            for (var index = 0; index < 4; index++)
            {
                var minValue = _irises.Min(i => i.Params[index]);
                var maxValue = _irises.Max(i => i.Params[index]);

                _irises.ForEach(i => i.Params[index] = 
                    (i.Params[index] - minValue) / 
                    (maxValue - minValue));
            }

            ShowChartDatas();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_flag is 0)
            {
                _flag = 1;

                InitializeCharts();
            }
            else if (_flag is 1)
            {
                _flag = 2;

                NormalizeIrises();
            }
            else if (_flag is 2)
            {
                _flag = 3;

                var testCount = _irises.Count / 4;
                var scores = new List<(int, double)>();
                for (var index = 3; index < Math.Sqrt(_irises.Count); index++)
                {
                    var traneAlgorithm = new Algorithm(
                        _irises.Take(_irises.Count - testCount).ToList(),
                        index);

                    traneAlgorithm.Predict(_irises.TakeLast(testCount).ToList());

                    var score = 
                        traneAlgorithm.GetScore(_irises.Select(i => i.Id).TakeLast(testCount).ToList());
                    scores.Add((index, score));
                }

                var k = scores.OrderByDescending(s => s.Item2).First().Item1;

                var random = new Random();
                var algorithm = new Algorithm(_irises, k);
                var id = algorithm.GetIrisId(new Iris()
                {
                    Params = new double[4]
                    {
                        random.NextDouble(),
                        random.NextDouble(),
                        random.NextDouble(),
                        random.NextDouble()
                    }
                });
            }
        } 
    }
}