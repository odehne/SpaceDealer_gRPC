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
			Parent.Logger.Log($"Loading discovered sectors.", TraceEventType.Information);
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
				Parent.Logger.Log($"Failed to get all discovered sectors {e.Message}", TraceEventType.Error);
			}
			return sectors;
		}

		public void SaveSectors(Sectors sectors)
		{
			Parent.Logger.Log($"Saving discovered sectors.", TraceEventType.Information);
			if (sectors.Any())
			{
                foreach (var sector in sectors)
                {
					SaveSector(sector);
				}
			}
		}

		public bool SaveSector(Sector sector)
		{
			Parent.Logger.Log($"Saving discovered sector.", TraceEventType.Information);
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
				Parent.Logger.Log($"Sector {sector} saved.", TraceEventType.Information);
				return true;
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"ailed to save sector {sector} - {e}", TraceEventType.Error);
			}
			return false;
		}
	}
}
