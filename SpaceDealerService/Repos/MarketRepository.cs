using SpaceDealer;
using SpaceDealerModels.Units;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{
	public class MarketRepository
	{
		public SqlPersistor Parent { get; set; }

		public MarketRepository(SqlPersistor parent)
		{
			Parent = parent;
		}

		public DbMarket GetMarket(string planetId)
		{
			//Parent.Logger.Log($"Loading market for planet {planetId}.", TraceEventType.Information);
			var market = new DbMarket();
			var query = "SELECT Id, Name FROM Markets WHERE PlanetId = @planetId;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
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
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get market for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}
			return market;
		}

		public void SaveMarket(string planetId, DbMarket market)
		{
			Parent.Logger.Log($"Saving market of planet {planetId}.", TraceEventType.Information);
			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					command.CommandText = $"INSERT OR REPLACE INTO Markets (Id, PlanetId, Name) VALUES (@id, @planetId, @name);";
					command.Parameters.AddWithValue("@planetId", planetId);
					command.Parameters.AddWithValue("@id", market.Id);
					command.Parameters.AddWithValue("@name", market.Name);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Log($"Generated market {market.Id} saved.", TraceEventType.Information);
					}
					catch (System.Exception e)
					{
						Parent.Logger.Log($"Failed to save market {e.Message}", TraceEventType.Error);
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to save market for planet Id [{planetId},{market.Id}] {e.Message}", TraceEventType.Error);
			}
		}
	}
}
