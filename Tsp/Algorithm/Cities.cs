using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Tsp.Algorithm.Models;

namespace Tsp.Algorithm
{
	public class Cities : List<City>
	{
		public async Task CalculateCityDistances(int numberOfCloseCities)
		{
			using (var client = new HttpClient()) 
			{
				foreach (City target in this)
				{
					foreach (City destination in this)
					{
						if (target.Equals(destination))
						{
							target.Distances.Add(0);
							continue;
						}

						var data = new Data()
						{
							Points = new List<Point>(),
							Type = "shortest"
						};
						data.Points.Add(new Point()
						{
							Type = "walking",
							X = target.Location.Item1,
							Y = target.Location.Item2
						});
						data.Points.Add(new Point()
						{
							Type = "walking",
							X = destination.Location.Item1,
							Y = destination.Location.Item2
						});

						var json = JsonConvert.SerializeObject(data);

						var response = await client.PostAsync(
							"https://routing.api.2gis.com/carrouting/6.0.0/global?key=rurbbn3446",
							new StringContent(
								json, 
								Encoding.UTF8, 
								"application/json"));

						var distance = JObject.Parse(await response.Content.ReadAsStringAsync())
							.SelectToken("result[0].total_distance");

						target.Distances.Add(Convert.ToDouble(distance));
					}
				}
			}

			foreach (City city in this)
			{
				city.FindClosestCities(numberOfCloseCities);
			}
		}
	}
}