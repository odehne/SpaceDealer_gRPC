using SpaceDealer;
using SpaceDealerModels.Units;
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

		public override List<DbProductInStock> GetAll()
		{
			var lst = new List<DbProductInStock>();

			Parent.Logger.Log($"Loading all products.", TraceEventType.Information);

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
				//Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get product {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public override List<DbProductInStock> GetAll(string id)
		{
			throw new System.NotImplementedException();
		}

		public override DbProductInStock GetItem(string name, string id)
		{
			var parameter = new SQLiteParameter();
			Parent.Logger.Log($"Loading product with {name} or {id}.", TraceEventType.Information);

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
						p.PicturePath = reader.GetString(5);
					}
				}
				reader.Close();
				//Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get product for player Id [{id}] {e.Message}", TraceEventType.Error);
			}

			return p;
		}

		public override string GetItemId(string name)
		{
			var p = new DbFeature();
			Parent.Logger.Log($"Loading product with name {name}.", TraceEventType.Information);
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
				Parent.Logger.Log($"Failed to get product {e.Message}", TraceEventType.Error);
			}
			return null;
		}

		public override void Save(DbProductInStock item)
		{
			var id = GetItemId(item.Name);
			if (id != null)
				return;
			Parent.Logger.Log($"Saving product {item.Name}.", TraceEventType.Information);

			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					command.CommandText = $"INSERT OR REPLACE INTO Products (Id, Name, Weight, PricePerTon, AmountGeneratedPerRound, PicturePath) VALUES (@id, @name, @weight, @pricePerTon, @amountGeneratedPerRound, @picturePath);";
					command.Parameters.AddWithValue("@id", item.Id);
					command.Parameters.AddWithValue("@name", item.Name);
					command.Parameters.AddWithValue("@weight", item.Weight);
					command.Parameters.AddWithValue("@PricePerTon", item.PricePerTon);
					command.Parameters.AddWithValue("@amountGeneratedPerRound", item.AmountGeneratedPerRound);
					command.Parameters.AddWithValue("@picturePath", item.PicturePath);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Log($"Prouct {item.Name} saved.", TraceEventType.Information);
					}
					catch (System.Exception e)
					{
						Parent.Logger.Log($"Failed to add product {e.Message}", TraceEventType.Error);
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
