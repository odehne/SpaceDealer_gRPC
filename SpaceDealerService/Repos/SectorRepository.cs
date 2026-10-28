using SpaceDealerModels.Units;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

namespace SpaceDealerService.Repos
{
    public class SectorRepository 
    {
		public SqlPersistor Parent { get; set; }

        public SectorRepository(SqlPersistor parent)
        {
            Parent = parent;
        }

		public Sectors GetAll()
        {
			var sectors = new Sectors();
			Parent.Logger.Verbose($"Loading discovered sectors.");
			var query = "SELECT X,Y,Z,ShipIds,PlanetIds,PlayerIds FROM Sectors;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var sector = new Sector(new DbCoordinates(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));

						var shipIds = reader.GetString(3);
						var planetIds = reader.GetString(4);
						var playerIds = reader.GetString(5);
						if(!string.IsNullOrEmpty(playerIds))
							sector.AddPlayers(playerIds);
						if (!string.IsNullOrEmpty(planetIds))
							sector.AddPlanets(planetIds);
						if (!string.IsNullOrEmpty(shipIds))
							sector.AddShips(shipIds);
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to get all discovered sectors {e.Message}");
			}
			return sectors;
		}

		public void SaveSectors(Sectors sectors)
		{
			Parent.Logger.Verbose($"Saving discovered sectors.");
			if (sectors.Any())
			{
                foreach (var sector in sectors.Values)
                {
					SaveSector(sector);
				}
			}
		}

		public bool SaveSector(Sector sector)
		{
			Parent.Logger.Verbose($"Saving discovered sector.");
			using var command = new SQLiteCommand(Parent.Connection);
			command.CommandText = $"INSERT OR REPLACE INTO Sectors (X,Y,Z,ShipIds,PlanetIds,PlayerIds) VALUES (@X, @Y, @Z,@ShipIds,@PlanetIds,@PlayerIds);";
			command.Parameters.AddWithValue("@X", sector.Position.X);
			command.Parameters.AddWithValue("@Y", sector.Position.Y);
			command.Parameters.AddWithValue("@Z", sector.Position.Z);
			command.Parameters.AddWithValue("@ShipIds", string.Join(",", sector.ShipIds));
			command.Parameters.AddWithValue("@PlanetIds", string.Join(",", sector.PlayerIds));
			command.Parameters.AddWithValue("@PlayerIds", string.Join(",", sector.PlanetIds));
			try
			{
				command.ExecuteNonQuery();
				Parent.Logger.Verbose($"Sector {sector} saved.");
				return true;
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to save sector {sector} - {e.Message}");
			}
			return false;
		}
	}
}
