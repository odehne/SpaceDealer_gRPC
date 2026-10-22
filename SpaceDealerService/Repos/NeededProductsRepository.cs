using SpaceDealer;
using SpaceDealerModels;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class NeededProductsRepository
	{
		public SqlPersistor Parent { get; set; }

		public NeededProductsRepository(SqlPersistor parent)
		{
			Parent = parent;
		}

		public DbProductsInStock GetNeededProducts(string planetId)
		{
			var lst = new DbProductsInStock();
			//Parent.Logger.Log($"Loading needed products for planet {planetId}.");

			var query = "SELECT ProductId, Interest FROM NeededProducts WHERE PlanetId = @planetId;";
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

						if (product != null)
						{
							product.PricePerTon = Tools.AddPercent(product.PricePerTon, interest);
							lst.AddProduct(product);
						}
						else
						{
                            Parent.Logger.Warning($"Product [{productId}] not found.");
                        }
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}");
			}

			return lst;
		}

		public DbProductInStock GetProductInStock(string planetId, string productId)
		{
			//Parent.Logger.Log($"Loading needed product for planet {planetId}.");

			var query = "SELECT Interest FROM NeededProducts WHERE PlanetId = @planetId AND ProductId = @productId;";
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
						return product;
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}");
			}

			return null;
		}

		public void SaveNeededProduct(string planetId, string productId)
		{
			Parent.Logger.Verbose($"Saving needed product for planet {planetId}.");
			var p = GetProductInStock(planetId, productId);
			if (p != null)
				return;

			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					command.CommandText = $"INSERT OR REPLACE INTO NeededProducts (PlanetId, ProductId, Interest) VALUES (@planetId, @productId, @interest);";
					command.Parameters.AddWithValue("@planetId", planetId);
					command.Parameters.AddWithValue("@productId", productId);
					command.Parameters.AddWithValue("@interest", 10.0);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Information($"Needed product {productId} saved.");
					}
					catch (Exception e)
					{
						Parent.Logger.Error($"Failed to save needed product {e.Message}");
					}
				}
			}
			catch (Exception e)
			{
				Parent.Logger.Error($"Failed to save NeededProducts for planet Id [{planetId},{productId}] {e.Message}");
			}
		}
	}
}
