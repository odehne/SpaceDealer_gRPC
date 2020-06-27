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

		public override List<DbPlanet> GetAll()
		{
			Parent.Logger.Log($"Loading planets.", TraceEventType.Information);
			var lst = new List<DbPlanet>();
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
						var planet = new DbPlanet();
						planet.Id = reader.GetString(0);
						planet.Name = reader.GetString(1);
						planet.PicturePath = reader.GetString(2);
						planet.Sector = new DbCoordinates(reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
						planet.Industry = new DbIndustry($"{planet.Id}.industry");
						planet.Industry.ProductsNeeded = Parent.NeededProductsRepo.GetNeededProducts(planet.Id);
						planet.Industry.GeneratedProducts = Parent.GeneratedProductsRepo.GetGeneratedProducts(planet.Id);
						planet.Market = Parent.MarketRepo.GetMarket(planet.Id);
						lst.Add(planet);
					}
				}
				reader.Close();
				//Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to read planet {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public override List<DbPlanet> GetAll(string id)
		{
			throw new System.NotImplementedException();
		}

		public override DbPlanet GetItem(string name, string id)
		{
			var parameter = new SQLiteParameter();
			Parent.Logger.Log($"Loading planet {name}, {id}.", TraceEventType.Information);

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
				using var command = new SQLiteCommand(Parent.Connection);
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
						planet.Industry.GeneratedProducts = Parent.GeneratedProductsRepo.GetGeneratedProducts(planet.Id);
						planet.Industry.ProductsNeeded = Parent.NeededProductsRepo.GetNeededProducts(planet.Id);
						planet.Market = Parent.MarketRepo.GetMarket(planet.Id);
						return planet;
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to read planet {e.Message}", TraceEventType.Error);
			}

			return null;
		}

		public override string GetItemId(string name)
		{
			Parent.Logger.Log($"Loading planet with name {name}.", TraceEventType.Information);
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
				Parent.Logger.Log($"Failed to get planet Id {e.Message}", TraceEventType.Error);
				return null;
			}
		}

		public override void Save(DbPlanet item)
		{
			Parent.Logger.Log($"Saving planet with name {item.Name}.", TraceEventType.Information);
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
						Parent.Logger.Log($"Planet {item.Name} saved.", TraceEventType.Information);
					}
					catch (System.Exception e)
					{
						Parent.Logger.Log($"Failed to add planet {e.Message}", TraceEventType.Error);
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to add planet {e.Message}", TraceEventType.Error);

			}
		}
	}
}
