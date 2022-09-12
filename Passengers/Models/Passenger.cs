using Passengers.Enums;

namespace Passengers.Models
{
    internal class Passenger
    {
        public bool IsSurvived { get; set; }
        public PClass PClass { get; set; }
        public Sex Sex { get; set; }
        public double Age { get; set; }
    }
}