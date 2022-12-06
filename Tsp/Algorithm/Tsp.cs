namespace Tsp.Algorithm
{
	public class Tsp
	{
		private Random _rand;
		private Cities _cityList;
		private Population _population;

		public bool Halt { get; set; }

		public Tour Begin(
			int populationSize, 
			int maxGenerations, 
			int groupSize, 
			int mutation, 
			int seed, 
			int chanceToUseCloseCity,
			Cities cityList)
		{
			_rand = new Random(seed);
			_cityList = cityList;

			_population = new Population();
			_population.CreateRandomPopulation(populationSize, cityList, _rand, chanceToUseCloseCity);

			int generation;
			for (generation = 0; generation < maxGenerations; generation++)
			{
				if (Halt)
				{
					break;
				}

				MakeChildren(groupSize, mutation);
			}

			return _population.BestTour;
		}

		private bool MakeChildren(int groupSize, int mutation)
		{
			int[] tourGroup = new int[groupSize];
			int tourCount, i, topTour, childPosition, tempTour;

			for (tourCount = 0; tourCount < groupSize; tourCount++)
			{
				tourGroup[tourCount] = _rand.Next(_population.Count);
			}

			for (tourCount = 0; tourCount < groupSize - 1; tourCount++)
			{
				topTour = tourCount;
				for (i = topTour + 1; i < groupSize; i++)
				{
					if (_population[tourGroup[i]].Fitness < 
						_population[tourGroup[topTour]].Fitness)
					{
						topTour = i;
					}
				}

				if (topTour != tourCount)
				{
					tempTour = tourGroup[tourCount];
					tourGroup[tourCount] = tourGroup[topTour];
					tourGroup[topTour] = tempTour;
				}
			}

			bool foundNewBestTour = false;

			childPosition = tourGroup[groupSize - 1];
			_population[childPosition] = Tour.Crossover(
				_population[tourGroup[0]], 
				_population[tourGroup[1]],
				_cityList,
				_rand);
			if (_rand.Next(100) < mutation)
			{
				_population[childPosition].Mutate(_rand);
			}
			_population[childPosition].DetermineFitness(_cityList);

			if (_population[childPosition].Fitness < _population.BestTour.Fitness)
			{
				_population.BestTour = _population[childPosition];
				foundNewBestTour = true;
			}

			childPosition = tourGroup[groupSize - 2];
			_population[childPosition] = Tour.Crossover(
				_population[tourGroup[1]],
				_population[tourGroup[0]], 
				_cityList, 
				_rand);
			if (_rand.Next(100) < mutation)
			{
				_population[childPosition].Mutate(_rand);
			}
			_population[childPosition].DetermineFitness(_cityList);

			if (_population[childPosition].Fitness < _population.BestTour.Fitness)
			{
				_population.BestTour = _population[childPosition];
				foundNewBestTour = true;
			}

			return foundNewBestTour;
		}
	}
}