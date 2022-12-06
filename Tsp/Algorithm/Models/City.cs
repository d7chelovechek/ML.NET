namespace Tsp.Algorithm.Models
{
	public class City
	{
		public City(string x, string y)
		{
			Location = 
				(Convert.ToDouble(x.Replace('.', ',')), 
				Convert.ToDouble(y.Replace('.', ',')));

			Distances = new List<double>();
			CloseCities = new List<int>();
		}

		public (double, double) Location { get; set; }

		public List<double> Distances { get; set; }
		public List<int> CloseCities { get; set; }

		public void FindClosestCities(int numberOfCloseCities)
		{
			double shortestDistance;
			int shortestCity = 0;
			double[] dist = new double[Distances.Count];
			Distances.CopyTo(dist);

			if (numberOfCloseCities > Distances.Count - 1)
			{
				numberOfCloseCities = Distances.Count - 1;
			}

			CloseCities.Clear();

			for (var index = 0; index < numberOfCloseCities; index++)
			{
				shortestDistance = double.MaxValue;
				for (var cityNum = 0; cityNum < Distances.Count; cityNum++)
				{
					if (dist[cityNum] < shortestDistance)
					{
						shortestDistance = dist[cityNum];
						shortestCity = cityNum;
					}
				}
				CloseCities.Add(shortestCity);
				dist[shortestCity] = double.MaxValue;
			}
		}
	}
}