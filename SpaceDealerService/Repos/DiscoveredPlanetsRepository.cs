using SpaceDealer;
using SpaceDealerModels.Units;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{
	public class DiscoveredPlanetsRepository
	{
		public string DbPath { get; set; }

		public ILogger Logger { get; set; }

		private PlanetsRepository PlanetRepo { get; set; }

		public DiscoveredPlanetsRepository(ILogger logger, string dbPath)
		{
			DbPath = dbPath;
			Logger = logger;
			PlanetRepo = new PlanetsRepository(logger, dbPath);
		}

		public Planets GetDiscoveredPlanets(string playerId)
		{
			var lst = new Planets();

			var query = "SELECT PlanetId FROM DiscoveredPlanets WHERE PlayerId= @playerId;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@playerId", playerId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var planetId = reader.GetString(0);
						var planet = PlanetRepo.GetPlanet(planetId);
						lst.AddPlanet(planet);
					}
				}

			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to get planets for player Id [{playerId}] {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public void SaveDiscoveredPlanet(string playerId, string planetId)
		{
			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + DbPath))
				{
					connection.Open();
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO DiscoveredPlanets (PlayerID, PlanetId) VALUES (@playerId, @planetId);";
						command.Parameters.AddWithValue("@playerId", playerId);
						command.Parameters.AddWithValue("@planetId", planetId);
						command.ExecuteNonQuery();
					}
				}
			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to save discovered planet for planet Id [{planetId},{playerId}] {e.Message}", TraceEventType.Error);
			}
		}
	}
}
