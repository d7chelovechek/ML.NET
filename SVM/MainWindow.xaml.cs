using java.awt;
using libsvm;
using SVM.Custom;
using SVM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Window = System.Windows.Window;

namespace SVM
{
    public partial class MainWindow : Window
    {
        private bool _lineDrew;

        private svm_model _model;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AlgorithmGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is AlgorithmGrid grid)
            {
                if (grid.FirstSize.Equals(Size.Empty))
                {
                    grid.FirstSize = new Size(grid.ActualWidth, grid.ActualHeight);
                }

                var position = e.GetPosition(grid);

                if (!_lineDrew)
                {
                    if (e.LeftButton is MouseButtonState.Pressed)
                    {
                        grid.Points.Add(new Models.Point()
                        {
                            Coordinate = new System.Windows.Point(position.X, position.Y),
                            Class = 0
                        });
                    }
                    else if (e.RightButton is MouseButtonState.Pressed)
                    {
                        grid.Points.Add(new Models.Point()
                        {
                            Coordinate = new System.Windows.Point(position.X, position.Y),
                            Class = 1
                        });
                    }
                }
                else
                {
                    var @class = (int)svm.svm_predict(_model, GetSvmNode(position));

                    grid.Points.Add(new Models.Point()
                    {
                        Coordinate = new System.Windows.Point(position.X, position.Y),
                        Class = @class
                    });
                }

                grid.InvalidateVisual();
            }
        }

        private svm_node[] GetSvmNode(System.Windows.Point point)
        {
            return new svm_node[2]
            {
                new svm_node()
                {
                    index = 1,
                    value = point.X
                },
                new svm_node()
                {
                    index = 2,
                    value = point.Y
                }
            };
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter &&
                !_lineDrew &&
                PointsGrid.Points.Any())
            {
                _lineDrew = true;

                var problem = new svm_problem
                {
                    x = new svm_node[PointsGrid.Points.Count][],
                    y = new double[PointsGrid.Points.Count],
                    l = PointsGrid.Points.Count
                };

                for (var index = 0; index < PointsGrid.Points.Count; index++)
                {
                    problem.x[index] = GetSvmNode(PointsGrid.Points[index].Coordinate);

                    problem.y[index] = PointsGrid.Points[index].Class;
                }

                _model = svm.svm_train(problem, new svm_parameter()
                {
                    kernel_type = svm_parameter.C_SVC,
                    C = 1,
                    cache_size = 100,
                    probability = 0,
                    nu = 0.0,
                    eps = 0.001,
                    p = 0.1,
                    shrinking = 1,
                    nr_weight = 0
                });

                PointsGrid.Line = GetLine();
                PointsGrid.InvalidateVisual();
            }
        }

        private (System.Windows.Point, System.Windows.Point) GetLine()
        {
            var result = (new System.Windows.Point(-1, -1), new System.Windows.Point(-1, -1));
            var currentClass = svm.svm_predict(_model, GetSvmNode(new System.Windows.Point(0, 0)));

            var height = 0d;
            for (var w = 0; w <= PointsGrid.ActualWidth; w++)
            {
                var point = new System.Windows.Point(w, height);

                var targetClass = svm.svm_predict(_model, GetSvmNode(point));

                if (!targetClass.Equals(currentClass))
                {
                    currentClass = targetClass;

                    if (result.Item1.Equals(new System.Windows.Point(-1, -1)))
                    {
                        result.Item1 = point;
                    }
                    else
                    {
                        result.Item2 = point;
                        return result;
                    }
                }
            }

            var width = PointsGrid.ActualWidth;
            for (var h = 0; h <= PointsGrid.ActualHeight; h++)
            {
                var point = new System.Windows.Point(width, h);

                var targetClass = svm.svm_predict(_model, GetSvmNode(point));

                if (!targetClass.Equals(currentClass))
                {
                    currentClass = targetClass;

                    if (result.Item1.Equals(new System.Windows.Point(-1, -1)))
                    {
                        result.Item1 = point;
                    }
                    else
                    {
                        result.Item2 = point;
                        return result;
                    }
                }
            }

            height = PointsGrid.ActualHeight;
            for (var w = PointsGrid.ActualWidth; w >= 0; w--)
            {
                var point = new System.Windows.Point(w, height);

                var targetClass = svm.svm_predict(_model, GetSvmNode(point));

                if (!targetClass.Equals(currentClass))
                {
                    currentClass = targetClass;

                    if (result.Item1.Equals(new System.Windows.Point(-1, -1)))
                    {
                        result.Item1 = point;
                    }
                    else
                    {
                        result.Item2 = point;
                        return result;
                    }
                }
            }

            width = 0d;
            for (var h = PointsGrid.ActualHeight; h >= 0; h--)
            {
                var point = new System.Windows.Point(width, h);

                var targetClass = svm.svm_predict(_model, GetSvmNode(point));

                if (!targetClass.Equals(currentClass))
                {
                    currentClass = targetClass;

                    if (result.Item1.Equals(new System.Windows.Point(-1, -1)))
                    {
                        result.Item1 = point;
                    }
                    else
                    {
                        result.Item2 = point;
                        return result;
                    }
                }
            }

            return result;
        }
    }
}