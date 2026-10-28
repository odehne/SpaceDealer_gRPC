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

			Parent.Logger.Verbose($"Loading discovered planets.");
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
						var planet = Parent.PlanetsRepo.GetItemById(planetId);
						if(planet!=null)
							lst.AddPlanet(planet);
						else
                            Parent.Logger.Error($"Discovered planet {planetId} not found.");
                    }
				}
				reader.Close();
			}
			catch (Exception e)
			{
				Parent.Logger.Error($"Failed to get planets for player Id [{playerId}] {e.Message}"	);
			}

			return lst;
		}

		public void SaveDiscoveredPlanet(string playerId, string planetId)
		{
			Parent.Logger.Verbose($"Saving discovered planets.");
		
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
						Parent.Logger.Verbose($"Discovered planet {planetId} saved.");
					}
                    catch (Exception e)
					{
						Parent.Logger.Error($"Failed to save generated product {e.Message}"	);
					}
				}
                catch (Exception e)
				{
					Parent.Logger.Error($"Failed to save discovered planet for planet Id [{planetId},{playerId}] {e.Message}"	);
				}
			}
			
		}

		private bool PlayerHasDiscoveredPlanet(string playerId, string planetId)
		{
			Parent.Logger.Verbose($"Checking if player has discovered this planet already.");
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
			catch (Exception e)
			{
				Parent.Logger.Error($"Failed to determine if player has discovered planet {e.Message}."	);
			}
			return false;
		}
	}
}
