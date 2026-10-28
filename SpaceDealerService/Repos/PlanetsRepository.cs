using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using SpaceDealer;
using SpaceDealerModels.Units;

namespace SpaceDealerService.Repos
{
	public class PlanetsRepository : Repository<DbPlanet>
	{
		public PlanetsRepository(SqlPersistor parent) : base(parent)
		{
		}

		public override ConcurrentDictionary<string, DbPlanet> GetAll()
		{
			Parent.Logger.Verbose($"Loading planets.");
			var lst = new ConcurrentDictionary<string, DbPlanet>();
			var query = "SELECT Id, Name, PicturePath, X, Y, Z, IndustryName FROM Planets;";
			try
			{
				
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
                        var planet = new DbPlanet
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            PicturePath = reader.GetString(2),
                            Sector = new DbCoordinates(reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5))
                        };
                        planet.Industry = new DbIndustry($"{planet.Id}.industry")
                        {
                            ProductsNeeded = Parent.NeededProductsRepo.GetNeededProducts(planet.Id),
                            GeneratedProducts = Parent.GeneratedProductsRepo.GetGeneratedProducts(planet.Id)
                        };
                        //planet.Market = Parent.MarketRepo.GetMarket(planet.Id);
						lst.TryAdd(planet.Id, planet);
					}
				}
				reader.Close();
				//Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to read planet {e.Message}");
			}

			return lst;
		}

		public override ConcurrentDictionary<string, DbPlanet> GetAll(string id)
		{
			throw new System.NotImplementedException();
		}

		public override DbPlanet GetItemById(string id)
		{
			var query = "SELECT Id, Name, PicturePath, X, Y, Z, IndustryName FROM Planets WHERE Id = @id;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", id);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
                        var planet = new DbPlanet
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            PicturePath = reader.GetString(2),
                            Sector = new DbCoordinates(reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5))
                        };
                        planet.Industry = new DbIndustry($"{planet.Id}.industry")
                        {
                            GeneratedProducts = Parent.GeneratedProductsRepo.GetGeneratedProducts(planet.Id),
                            ProductsNeeded = Parent.NeededProductsRepo.GetNeededProducts(planet.Id)
                        };
                  	return planet;
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to read planet {e.Message}");
			}

			return null;
		}

        public override DbPlanet GetItemByName(string name)
        {
            var query = "SELECT Id, Name, PicturePath, X, Y, Z, IndustryName FROM Planets WHERE Name = @name;";
            try
            {
                using var command = new SQLiteCommand(Parent.Connection);
                command.CommandText = query;
                command.Parameters.AddWithValue("@name", name);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var planet = new DbPlanet
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            PicturePath = reader.GetString(2),
                            Sector = new DbCoordinates(reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5))
                        };
                        planet.Industry = new DbIndustry($"{planet.Id}.industry")
                        {
                            GeneratedProducts = Parent.GeneratedProductsRepo.GetGeneratedProducts(planet.Id),
                            ProductsNeeded = Parent.NeededProductsRepo.GetNeededProducts(planet.Id)
                        };
                        //planet.Market = Parent.MarketRepo.GetMarket(planet.Id);
                        return planet;
                    }
                }
                reader.Close();
            }
            catch (System.Exception e)
            {
                Parent.Logger.Error($"Failed to read planet {e.Message}");
            }

            return null;
        }

        public override string GetItemId(string name)
		{
			Parent.Logger.Verbose($"Loading planet with name {name}.");
			var query = "SELECT Id FROM Planets WHERE Name = @name;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@name", name);
				var ret = (string)command.ExecuteScalar();
				//Parent.CloseConnection(connection);
				return ret;
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to get planet Id {e.Message}");
				return null;
			}
		}

		public override void Save(DbPlanet item)
		{
			Parent.Logger.Verbose($"Saving planet with name {item.Name}.");
			var id = GetItemId(item.Name);
			if (id != null)
				return;
			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					command.CommandText = $"INSERT OR REPLACE INTO Planets (Id, Name, X, Y, Z, PicturePath, IndustryName) VALUES (@id, @name, @x, @y, @z, @picturePath, @industryName);";
					command.Parameters.AddWithValue("@id", item.Id);
					command.Parameters.AddWithValue("@name", item.Name);
					command.Parameters.AddWithValue("@picturePath", item.PicturePath);
					command.Parameters.AddWithValue("@industryName", item.Industry.Name);
					command.Parameters.AddWithValue("@x", item.Sector.X);
					command.Parameters.AddWithValue("@y", item.Sector.Y);
					command.Parameters.AddWithValue("@z", item.Sector.Z);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Information($"Planet {item.Name} saved.");
					}
					catch (System.Exception e)
					{
						Parent.Logger.Error($"Failed to add planet {e.Message}");
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to add planet {e.Message}");
			}
		}
	}
}
