using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using Tsp.Algorithm;
using Tsp.Algorithm.Models;
using Tsp.Models;

namespace Tsp.Controllers
{
	public class TspController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			ViewData.Model = new TspModel()
			{
				CenterX = "49.13128646500384",
				CenterY = "55.78945176087188",
				Zoom = "13",
				ShowRoad = "false",
				Points = "[]"
			};

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(TspModel model)
		{
			var coords = model.Points.Split(',');
			var tempPoints = new List<(string, string)>();

			for (var index = 1; index < coords.Length; index += 2)
			{
				tempPoints.Add((coords[index - 1], coords[index]));
			}

			var cities = new Cities();
			foreach ((string, string) city in tempPoints)
			{
				cities.Add(new City(city.Item1, city.Item2));
			}

			int populationSize = 10000;
			int maxGenerations = 500000;
			int mutation = 3;
			int groupSize = 5;
			int seed = DateTime.UtcNow.Millisecond;
			int numberOfCloseCities = 5;
			int chanceUseCloseCity = 90;

			await cities.CalculateCityDistances(numberOfCloseCities);

			var tsp = new Algorithm.Tsp();
			var tour = tsp.Begin(
				populationSize,
				maxGenerations,
				groupSize, 
				mutation,
				seed,
				chanceUseCloseCity, 
				cities);

			var lastId = 0;
			var nextId = tour[0].Connection1;

			var result = new StringBuilder();
			foreach (City city in cities) 
			{
				result.Append($"[{cities[lastId].Location.Item1}!{cities[lastId].Location.Item2}]!");

				if (lastId != tour[nextId].Connection1)
				{
					lastId = nextId;
					nextId = tour[nextId].Connection1;
				}
				else
				{
					lastId = nextId;
					nextId = tour[nextId].Connection2;
				}
			}

			result.Append($"[{cities[nextId].Location.Item1}!{cities[nextId].Location.Item2}]!");
			model.Points = $"[{result.ToString().Replace(',','.').TrimEnd('!').Replace('!',',')}]";

			ViewData.Model = model;

			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}