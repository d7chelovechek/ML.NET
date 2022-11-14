using System.Windows.Media;

namespace kNN.Models
{
    internal class ChartData
    {
        public int IrisId { get; set; }

        public double XValue { get; set; }
        public double YValue { get; set; }

        public Brush Brush
        {
            get => IrisId switch
            {
                0 => Brushes.Red,
                1 => Brushes.Green,
                2 => Brushes.Blue,
                _ => Brushes.Transparent
            };
        }

        public ChartData(int id, double x, double y)
        {
            IrisId = id;

            XValue = x;
            YValue = y;
        }
    }
}