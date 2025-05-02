using SpaceDealer;
using SpaceDealerModels;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
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
			//Parent.Logger.Log($"Loading generated products for planet {planetId}.", TraceEventType.Information);

			var query = "SELECT ProductId, Interest FROM GeneratedProducts WHERE PlanetId = @planetId;";
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
						var product = Parent.ProductRepo.GetItem(null, productId);
						product.PricePerTon = Tools.AddPercent(product.PricePerTon, interest);
						lst.AddProduct(product);
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get generated products for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public DbProductInStock GetProductInStock(string planetId, string productId)
		{
			//Parent.Logger.Log($"Loading products in stock for planet {planetId}.", TraceEventType.Information);

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
						var product = Parent.ProductRepo.GetItem(null, productId);
						product.PricePerTon = Tools.AddPercent(product.PricePerTon, interest);
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
				Parent.Logger.Log($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}

			return null;
		}

		public void SaveGeneratedProduct(string planetId, string productId)
		{
			Parent.Logger.Log($"Saving generated product for planet {planetId}.", TraceEventType.Information);
			var p = GetProductInStock(planetId, productId);
			if (p != null)
				return;

			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					var interest = GetRandomInterest();
                    command.CommandText = $"INSERT OR REPLACE INTO GeneratedProducts (PlanetId, ProductId, Interest) VALUES (@planetId, @productId, @interest);";
					command.Parameters.AddWithValue("@planetId", planetId);
					command.Parameters.AddWithValue("@productId", productId);
					command.Parameters.AddWithValue("@interest", interest);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Log($"Generated product {productId} with interest {interest}% saved.", TraceEventType.Information);
					}

                    catch (Exception e)
					{
						Parent.Logger.Log($"Failed to save generated product {e.Message}", TraceEventType.Error);
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to save GeneratedProducts for planet Id [{planetId},{productId}] {e.Message}", TraceEventType.Error);
			}
		}

		public static double GetRandomInterest()
        {
			var interest = _random.Next(-10, 10);
            return interest;
        }
    }
}
