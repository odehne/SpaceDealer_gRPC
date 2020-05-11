using SpaceDealer;
using SpaceDealerModels.Units;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class ProductsRepository
	{

		public SqlPersistor Parent { get; set; }

		public ProductsRepository(SqlPersistor parent)
		{
			Parent = parent;
		}

		public List<DbProductInStock> GetProducts()
		{
			var lst = new List<DbProductInStock>();

			var query = "SELECT Id, Name, Weight, PricePerTon, AmountGeneratedPerRound, PicturePath FROM Products;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var p = new DbProductInStock();
						p.Id = reader.GetString(0);
						p.Name = reader.GetString(1);
						p.Weight = reader.GetDouble(2);
						p.PricePerTon = reader.GetDouble(3);
						p.AmountGeneratedPerRound = reader.GetDouble(4);
						p.PicturePath = reader.GetString(5);
						lst.Add(p);
					}
				}
				reader.Close();
				Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get product {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public DbProductInStock GetProduct(string name, string id)
		{
			var parameter = new SQLiteParameter();

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
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
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
						p.PicturePath = reader.GetString(5);
					}
				}
				reader.Close();
				Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get product for player Id [{id}] {e.Message}", TraceEventType.Error);
			}

			return p;
		}

		public string GetProductId(string name)
		{
			var p = new DbFeature();
			var query = "SELECT Id FROM Products WHERE Name = @name;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@name", name);
				var ret = (string)command.ExecuteScalar();
				Parent.CloseConnection(connection);
				return ret;

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get product {e.Message}", TraceEventType.Error);
			}
			return null;
		}

		public void SaveProduct(DbProductInStock product)
		{

			var id = GetProductId(product.Name);
			if (id != null)
				return;

			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + Parent.DbPath))
				{
					Parent.OpenConnection(connection);
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO Products (Id, Name, Weight, PricePerTon, AmountGeneratedPerRound, PicturePath) VALUES (@id, @name, @weight, @pricePerTon, @amountGeneratedPerRound, @picturePath);";
						command.Parameters.AddWithValue("@id", product.Id);
						command.Parameters.AddWithValue("@name", product.Name);
						command.Parameters.AddWithValue("@weight", product.Weight);
						command.Parameters.AddWithValue("@PricePerTon", product.PricePerTon);
						command.Parameters.AddWithValue("@amountGeneratedPerRound", product.AmountGeneratedPerRound);
						command.Parameters.AddWithValue("@picturePath", product.PicturePath);
						try
						{
							command.ExecuteNonQuery();
							Parent.Logger.Log($"Prouct {product.Name} saved.", TraceEventType.Information);
						}
						catch (System.Exception e)
						{
							Parent.Logger.Log($"Failed to add product {e.Message}", TraceEventType.Error);
						}
						finally
						{
							Parent.CloseConnection(connection);
						}
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to add product Id {e.Message}", TraceEventType.Error);

			}
		}
	}
}
