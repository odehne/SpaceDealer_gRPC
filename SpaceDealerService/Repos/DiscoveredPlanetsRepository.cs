using SpaceDealer;
using SpaceDealerModels.Units;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{
	public class DiscoveredPlanetsRepository
	{
		public SqlPersistor Parent { get; set; }

		public DiscoveredPlanetsRepository(SqlPersistor parent)
		{
			Parent = parent;
		}

		public Planets GetDiscoveredPlanets(string playerId)
		{
			var lst = new Planets();

			var query = "SELECT PlanetId FROM DiscoveredPlanets WHERE PlayerId= @playerId;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@playerId", playerId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var planetId = reader.GetString(0);
						var planet = Parent.PlanetsRepo.GetPlanet("", planetId);
						lst.AddPlanet(planet);
					}
				}
				reader.Close();
				Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get planets for player Id [{playerId}] {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public void SaveDiscoveredPlanet(string playerId, string planetId)
		{
			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + Parent.DbPath))
				{
					Parent.OpenConnection(connection);
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO DiscoveredPlanets (PlayerID, PlanetId) VALUES (@playerId, @planetId);";
						command.Parameters.AddWithValue("@playerId", playerId);
						command.Parameters.AddWithValue("@planetId", planetId);
						try
						{
							command.ExecuteNonQuery();
							Parent.Logger.Log($"Discovered planet {planetId} saved.", TraceEventType.Information);
						}
						catch (System.Exception e)
						{
							Parent.Logger.Log($"Failed to save generated product {e.Message}", TraceEventType.Error);
						}
						finally
						{
							Parent.CloseConnection(connection);
						}
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to save discovered planet for planet Id [{planetId},{playerId}] {e.Message}", TraceEventType.Error);
			}
		}
	}
}
