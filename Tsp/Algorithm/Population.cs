namespace Tsp.Algorithm
{
	public class Population : List<Tour>
	{
		public Tour BestTour { get; set; }

		public void CreateRandomPopulation(
			int populationSize,
			Cities cityList, 
			Random rand, 
			int chanceToUseCloseCity)
		{
			int firstCity, lastCity, nextCity;

			for (var tourCount = 0; tourCount < populationSize; tourCount++)
			{
				var tour = new Tour(cityList.Count);

				firstCity = rand.Next(cityList.Count);
				lastCity = firstCity;

				for (var city = 0; city < cityList.Count - 1; city++)
				{
					do
					{
						if ((rand.Next(100) < chanceToUseCloseCity) &&
							(cityList[city].CloseCities.Count > 0))
						{
							nextCity = cityList[city].CloseCities[rand.Next(cityList[city].CloseCities.Count)];
						}
						else
						{
							nextCity = rand.Next(cityList.Count);
						}
					} 
					while ((tour[nextCity].Connection2 is not -1) || (nextCity == lastCity));

					tour[lastCity].Connection2 = nextCity;
					tour[nextCity].Connection1 = lastCity;
					lastCity = nextCity;
				}

				tour[lastCity].Connection2 = firstCity;
				tour[firstCity].Connection1 = lastCity;

				tour.DetermineFitness(cityList);

				Add(tour);

				if ((BestTour == null) ||
					(tour.Fitness < BestTour.Fitness))
				{
					BestTour = tour;
				}
			}
		}
	}
}