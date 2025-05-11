using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
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

			//Parent.Logger.Log($"Loading discovered planets.", TraceEventType.Information);
			var query = "SELECT PlanetId FROM DiscoveredPlanets WHERE PlayerId= @playerId;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@playerId", playerId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var planetId = reader.GetString(0);
						var planet = Program.Persistor.PlanetsRepo.GetItem("", planetId);
						lst.AddPlanet(planet);
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get planets for player Id [{playerId}] {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public void SaveDiscoveredPlanet(string playerId, string planetId)
		{
			//Parent.Logger.Log($"Saving discovered planets.", TraceEventType.Information);
		
			if(!PlayerHasDiscoveredPlanet(playerId, planetId))
			{
				try
				{
					using var command = new SQLiteCommand(Parent.Connection);
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
				}
				catch (System.Exception e)
				{
					Parent.Logger.Log($"Failed to save discovered planet for planet Id [{planetId},{playerId}] {e.Message}", TraceEventType.Error);
				}
			}
			
		}

		private bool PlayerHasDiscoveredPlanet(string playerId, string planetId)
		{
			Parent.Logger.Log($"Checking if player has discovered this planet already.", TraceEventType.Information);
			var query = "SELECT PlanetId FROM DiscoveredPlanets WHERE PlayerID = @playerId AND planetId = @planetId;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@playerId", playerId);
				command.Parameters.AddWithValue("@planetId", planetId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					reader.Close();
					return true;
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to determine if player has discovered planet {e.Message}.", TraceEventType.Error);
			}
			return false;
		}
	}
}
