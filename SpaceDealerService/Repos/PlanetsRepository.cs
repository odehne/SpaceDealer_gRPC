using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using SpaceDealer;
using SpaceDealerModels.Units;

namespace SpaceDealerService.Repos
{
	public class PlanetsRepository
	{
		public string DbPath { get; set; }
		public ILogger Logger { get; set; }

		private NeededProductsRepository NeededProdRepo { get; set; }
		private GeneratedProductsRepository GeneratedProdRepo { get; set; }
		private MarketRepository MarketRepo{ get; set; }

		public PlanetsRepository(ILogger logger, string dbPath)
		{
			DbPath = dbPath;
			Logger = logger;
			GeneratedProdRepo = new GeneratedProductsRepository(Logger, DbPath);
			NeededProdRepo = new NeededProductsRepository(Logger, DbPath);
			MarketRepo = new MarketRepository(Logger, DbPath);
		}

		public List<DbPlanet> GetPlanets()
		{
			var lst = new List<DbPlanet>();
			var query = "SELECT Id, Name, PicturePath, X, Y, Z, IndustryName FROM Planets;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var planet = new DbPlanet();
						planet.Id = reader.GetString(0);
						planet.Name = reader.GetString(1);
						planet.PicturePath = reader.GetString(2);
						planet.Sector = new DbCoordinates(reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
						planet.Industry = new DbIndustry($"{planet.Id}.industry");
						planet.Industry.ProductsNeeded = NeededProdRepo.GetNeededProducts(planet.Id);
						planet.Industry.GeneratedProducts = GeneratedProdRepo.GetGeneratedProducts(planet.Id);
						planet.Market = MarketRepo.GetMarket(planet.Id);
						lst.Add(planet);
					}
				}

			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to read planet {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public string GetPlanetId(string name)
		{
			var query = "SELECT Id FROM Planets WHERE Name = @name;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@name", name);
				return (string)command.ExecuteScalar();
			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to get planet Id {e.Message}", TraceEventType.Error);
				return null;
			}
		}

		
		public DbPlanet GetPlanet(string name, string id)
		{
			var parameter = new SQLiteParameter();

			var query = "SELECT Id, Name, PicturePath, X, Y, Z, IndustryName FROM Planets WHERE ";
			if (!string.IsNullOrEmpty(name))
			{
				query += "Name = @name;";
				parameter.ParameterName = "@name";
				parameter.Value = name;
			}
			else
			{
				query += "Id = @id;";
				parameter.ParameterName = "@id";
				parameter.Value = id;
			}

			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.Add(parameter);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var planet = new DbPlanet();
						planet.Id = reader.GetString(0);
						planet.Name = reader.GetString(1);
						planet.PicturePath = reader.GetString(2);
						planet.Sector = new DbCoordinates(reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
						planet.Industry = new DbIndustry($"{planet.Id}.industry");
						planet.Industry.GeneratedProducts = Program.Persistor.GeneratedProductsRepo.GetGeneratedProducts(planet.Id);
						planet.Industry.ProductsNeeded = Program.Persistor.NeededProductsRepo.GetNeededProducts(planet.Id);
						planet.Market = Program.Persistor.MarketRepo.GetMarket(planet.Id);

						return planet;
					}
				}

			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to read planet {e.Message}", TraceEventType.Error);
			}

			return null;
		}

		public void SavePlanet(DbPlanet planet)
		{
			var id = GetPlanetId(planet.Name);
			if (id != null)
				planet.Id = id;
			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + DbPath))
				{
					connection.Open();
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO Planets (Id, Name, X, Y, Z, PicturePath, IndustryName) VALUES (@id, @name, @x, @y, @z, @picturePath, @industryName);";
						command.Parameters.AddWithValue("@id", planet.Id);
						command.Parameters.AddWithValue("@name", planet.Name);
						command.Parameters.AddWithValue("@picturePath", planet.PicturePath);
						command.Parameters.AddWithValue("@industryName", planet.Industry.Name);
						command.Parameters.AddWithValue("@x", planet.Sector.X);
						command.Parameters.AddWithValue("@y", planet.Sector.Y);
						command.Parameters.AddWithValue("@z", planet.Sector.Z);
						command.ExecuteNonQuery();
						Logger.Log($"Planet {planet.Name} saved.", TraceEventType.Information);

					}
				}
			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to add planet {e.Message}", TraceEventType.Error);

			}
		}
	}
}
