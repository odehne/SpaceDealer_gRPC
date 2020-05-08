using SpaceDealer;
using SpaceDealerModels.Units;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{
	public class MarketRepository
	{
		public string DbPath { get; set; }
		public ILogger Logger { get; set; }

		public MarketRepository(ILogger logger, string dbPath)
		{
			DbPath = dbPath;
			Logger = logger;
		}

		public DbMarket GetMarket(string planetId)
		{
			var market = new DbMarket();
			var query = "SELECT Id, Name FROM Markets WHERE PlanetId = @planetId;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@planetId", planetId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var id = reader.GetString(0);
						var name = reader.GetString(0);
						market =  new DbMarket() { Id = id, Name = name };
					}
				}

			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to get market for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}
			return market;
		}

		public void SaveMarket(string planetId, DbMarket market)
		{
			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + DbPath))
				{
					connection.Open();
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO Markets (Id, PlanetId, Name) VALUES (@id, @planetId, @name);";
						command.Parameters.AddWithValue("@planetId", planetId);
						command.Parameters.AddWithValue("@id", market.Id);
						command.Parameters.AddWithValue("@name", market.Name);
						command.ExecuteNonQuery();
					}
				}
			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to save marked for planet Id [{planetId},{market.Id}] {e.Message}", TraceEventType.Error);
			}
		}
	}
}
