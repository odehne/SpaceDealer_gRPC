using SpaceDealer;
using SpaceDealerModels.Units;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class ProductsRepository : Repository<DbProductInStock>
	{
		public ProductsRepository(SqlPersistor parent) : base(parent)
		{
		}

		public override ConcurrentDictionary<string, DbProductInStock> GetAll()
		{
			var lst = new ConcurrentDictionary<string, DbProductInStock>();

			Parent.Logger.Verbose($"Loading all products.");

			var query = "SELECT Id, Name, Weight, PricePerTon, AmountGeneratedPerRound, PicturePath FROM Products;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
                        var p = new DbProductInStock
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            Weight = reader.GetDouble(2),
                            PricePerTon = reader.GetDouble(3),
                            AmountGeneratedPerRound = reader.GetDouble(4),
                            PicturePath = !(reader.IsDBNull(5)) ? reader.GetString(5) : null
                        };
                        lst.TryAdd(p.Id, p);
					}
				}
				reader.Close();
				//Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to get product {e.Message}");
			}

			return lst;
		}

		public override ConcurrentDictionary<string, DbProductInStock> GetAll(string id)
		{
			throw new System.NotImplementedException();
		}

		public override DbProductInStock GetItem(string name, string id)
		{
			var parameter = new SQLiteParameter();
			//Parent.Logger.Log($"Loading product with {name} or {id}.");

			var query = "SELECT Id, Name, Weight, PricePerTon, AmountGeneratedPerRound, PicturePath FROM Products WHERE ";
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

			var p = new DbProductInStock();

			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.Add(parameter);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						p.Id = reader.GetString(0);
						p.Name = reader.GetString(1);
						p.Weight = reader.GetDouble(2);
						p.PricePerTon = reader.GetDouble(3);
						p.AmountGeneratedPerRound = reader.GetDouble(4);
						if(!reader.IsDBNull(5)) p.PicturePath = reader.GetString(5);
					}
				}
				else
				{
                    Parent.Logger.Warning($"Product with name {name} or id {id} not found.");
                    p = null;
                }
                reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to get product for player Id [{id}] {e.Message}");
			}

            return p;
		}

		public override string GetItemId(string name)
		{
			var p = new DbFeature();
			//Parent.Logger.Log($"Loading product with name {name}.");
			var query = "SELECT Id FROM Products WHERE Name = @name;";
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
				Parent.Logger.Error($"Failed to get product {e.Message}");
			}
			return null;
		}

		public override void Save(DbProductInStock item)
		{
			var id = GetItemId(item.Name);
			if (id != null)
				return;
			Parent.Logger.Verbose($"Saving product {item.Name}.");

			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					command.CommandText = $"INSERT OR REPLACE INTO Products (Id, Name, Weight, PricePerTon, AmountGeneratedPerRound) VALUES (@id, @name, @weight, @pricePerTon, @amountGeneratedPerRound);";
					command.Parameters.AddWithValue("@id", item.Id);
					command.Parameters.AddWithValue("@name", item.Name);
					command.Parameters.AddWithValue("@weight", item.Weight);
					command.Parameters.AddWithValue("@pricePerTon", item.PricePerTon);
					command.Parameters.AddWithValue("@amountGeneratedPerRound", item.AmountGeneratedPerRound);
					//command.Parameters.AddWithValue("@picturePath", item.PicturePath);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Information($"Product {item.Name} saved.");
					}
					catch (System.Exception e)
					{
						Parent.Logger.Error($"Failed to add product {e.Message}");
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to add product Id {e.Message}");

			}
		}

	}
}
