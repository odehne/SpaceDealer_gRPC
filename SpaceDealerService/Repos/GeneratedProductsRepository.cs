using SpaceDealerModels;
using SpaceDealerModels.Units;
using System;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class GeneratedProductsRepository
	{

		public SqlPersistor Parent { get; set; }
        private static Random _random = new Random();


        public GeneratedProductsRepository(SqlPersistor parent)
		{
			Parent = parent;
		}

		public DbProductsInStock GetGeneratedProducts(string planetId)
		{
			var lst = new DbProductsInStock();
			Parent.Logger.Verbose($"Loading generated products for planet {planetId}.");

			var query = "SELECT ProductId, Interest, Amount FROM GeneratedProducts WHERE PlanetId = @planetId;";
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
						var productId = reader.GetString(0);
						var interest = reader.GetDouble(1);
						var product = Parent.ProductRepo.GetItemById(productId);
						if (product != null)
						{
							product.Amount = reader.GetDouble(2);
							product.ApplyInterest(interest);
							lst.AddProduct(product);
						}
						else
						{
                            Parent.Logger.Warning($"Failed to get generated product with id [{productId}] for planet Id [{planetId}]");
                        }
					}
				}
				reader.Close();
			}
			catch (Exception e)
			{
				Parent.Logger.Error($"Failed to get generated products for planet Id [{planetId}] {e.Message}");
			}

			return lst;
		}

		public DbProductInStock GetProductInStock(string planetId, string productId)
		{
			//Parent.Logger.Log($"Loading products in stock for planet {planetId}.");

			var query = "SELECT Interest FROM GeneratedProducts WHERE PlanetId = @planetId AND ProductId = @productId;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@planetId", planetId);
				command.Parameters.AddWithValue("@productId", productId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var interest = reader.GetDouble(0);
						var product = Parent.ProductRepo.GetItemById(productId);
						product.ApplyInterest(interest);
						reader.Close();
						//Parent.CloseConnection(connection);
						return product;
					}
				}
				reader.Close();
				//Parent.CloseConnection(connection);
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}

			return null;
		}

		public void SaveGeneratedProduct(string planetId, string productId, double amount, double interest)
		{
			Parent.Logger.Verbose($"Saving generated product for planet {planetId}.");

			try
			{
				using (var update = new SQLiteCommand(Parent.Connection))
				{
					update.CommandText = "UPDATE GeneratedProducts SET Amount = @amount, Interest = @interest WHERE PlanetId = @planetId AND ProductId = @productId;";
					update.Parameters.AddWithValue("@amount", amount);
					update.Parameters.AddWithValue("@interest", interest);
					update.Parameters.AddWithValue("@planetId", planetId);
					update.Parameters.AddWithValue("@productId", productId);
					if (update.ExecuteNonQuery() > 0)
						return;
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to update GeneratedProducts for planet Id [{planetId},{productId}] {e.Message}");
				return;
			}

			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
                    command.CommandText = $"INSERT OR REPLACE INTO GeneratedProducts (PlanetId, ProductId, Interest, Amount) VALUES (@planetId, @productId, @interest, @amount);";
					command.Parameters.AddWithValue("@amount", amount);
					command.Parameters.AddWithValue("@planetId", planetId);
					command.Parameters.AddWithValue("@productId", productId);
					command.Parameters.AddWithValue("@interest", interest);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Information($"Generated product {productId} with interest {interest}% saved.");
					}

                    catch (Exception e)
					{
						Parent.Logger.Error($"Failed to save generated product {e.Message}");
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to save GeneratedProducts for planet Id [{planetId},{productId}] {e.Message}");
			}
		}

    }
}
