using SpaceDealer;
using SpaceDealerModels;
using SpaceDealerModels.Units;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class GeneratedProductsRepository
	{
		public string DbPath { get; set; }

		public ILogger Logger { get; set; }

		public ProductsRepository ProdRepo { get; set; }

		public GeneratedProductsRepository(ILogger logger, string dbPath)
		{
			DbPath = dbPath;
			Logger = logger;
			ProdRepo = new ProductsRepository(logger, dbPath);
		}

		public ProductsInStock GetGeneratedProducts(string planetId)
		{
			var lst = new ProductsInStock();

			var query = "SELECT ProductId, Interest FROM GeneratedProducts WHERE PlanetId = @planetId;";
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
						var productId = reader.GetString(0);
						var interest = reader.GetDouble(1);
						var product = ProdRepo.GetProduct(null, productId);
						product.PricePerTon = Tools.AddPercent(product.PricePerTon, interest);
						lst.AddProduct(product);
					}
				}

			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to get generated products for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}

			return lst;
		}


		public void SaveGeneratedProduct(string planetId, string productId)
		{
			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + DbPath))
				{
					connection.Open();
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO GeneratedProducts (PlanetId, ProductId, Interest) VALUES (@planetId, @productId, @interest);";
						command.Parameters.AddWithValue("@planetId", planetId);
						command.Parameters.AddWithValue("@productId", productId);
						command.Parameters.AddWithValue("@interest", -10.0);
						command.ExecuteNonQuery();
					}
				}
			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to save GeneratedProducts for planet Id [{planetId},{productId}] {e.Message}", TraceEventType.Error);
			}
		}
	}
}
