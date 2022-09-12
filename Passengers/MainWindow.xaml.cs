using Passengers.Enums;
using Passengers.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.Legend;

namespace Passengers
{
    public partial class MainWindow : Window
    {
        private readonly List<Passenger> _passengers;

        public MainWindow()
        {
            InitializeComponent();

            _passengers = new List<Passenger>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("Dataset/Passengers.csv"))
            {
                App.Current.Shutdown();
            }

            LoadDataset();

            LoadFirstTask();
            LoadSecondTask();
            LoadThirdTask();
        }

        private void LoadDataset()
        {
            var lines = File.ReadLines("Dataset/Passengers.csv").ToList();

            for (var index = 1; index < lines.Count; index++)
            {
                var data = lines[index].Split(',');
                data[6] = data[6].Replace('.', ',');

                _passengers.Add(new Passenger()
                {
                    IsSurvived = data[1] is "1",
                    PClass = (PClass)int.Parse(data[2]),
                    Sex = data[5] is "male" ? Sex.Male : Sex.Female,
                    Age = data[6] is "" ? double.NaN : double.Parse(data[6])
                });
            }
        }

        #region FirstTask
        private void LoadFirstTask()
        {
            LoadBarPointsBySex(Sex.Male);
            LoadBarPointsBySex(Sex.Female);
        }

        private void LoadBarPointsBySex(Sex sex)
        {
            LoadBarPointBySex(sex, PClass.First);
            LoadBarPointBySex(sex, PClass.Second);
            LoadBarPointBySex(sex, PClass.Third);
        }

        private void LoadBarPointBySex(Sex sex, PClass pClass)
        {
            BarSeries bar = sex is Sex.Male ?
                MalesBar : FemalesBar;

            bar.DataPoints.Add(new CategoricalDataPoint()
            {
                Category = $"{pClass} class",
                Value = _passengers
                    .Count(p => p.Sex.Equals(sex) &&
                        p.PClass.Equals(pClass) &&
                        p.IsSurvived)
            });
        }
        #endregion

        #region SecondTask
        private void LoadSecondTask()
        {
            SecondChartLegend.Items = 
                new LegendItemCollection();

            LoadPiePointByClass(PClass.First);
            LoadPiePointByClass(PClass.Second);
            LoadPiePointByClass(PClass.Third);
        }

        private void LoadPiePointByClass(PClass pClass)
        {
            int count =
                _passengers.Count(p => p.PClass.Equals(pClass));

            decimal percent =
                decimal.Round(
                    count * 100 / (decimal)_passengers.Count,
                    2);

            var brushes = new List<Brush>()
            {
                Brushes.LightGray,
                Brushes.Pink,
                Brushes.LightGreen
            };

            SecondChartLegend.Items.Add(new LegendItem()
            {
                MarkerFill = brushes[(int)pClass - 1],
                Title = $"{pClass} class"
            });

            ClassesPie.DataPoints.Add(new PieDataPoint()
            {
                Label = $"{count} passengers\n{percent}%",
                Value = count
            });
        }
        #endregion

        #region ThirdTask
        private void LoadThirdTask()
        {
            var temp =
                _passengers.Where(p => p.Age is double.NaN);

            LoadBarPointByAge("double.NaN", temp.Count());

            var groups =
                _passengers
                    .Except(temp)
                    .GroupBy(p => (int)p.Age / 10)
                    .OrderBy(g => g.Key);

            foreach (var group in groups)
            {
                var left = group.Key * 10;

                LoadBarPointByAge(
                    $"{left}-{left + 10}",
                    group.Count());
            }
        }

        private void LoadBarPointByAge(string category, double value)
        {
            AgesBar.DataPoints.Add(new CategoricalDataPoint()
            {
                Category = category,
                Value = value
            });
        }
        #endregion
    }
}