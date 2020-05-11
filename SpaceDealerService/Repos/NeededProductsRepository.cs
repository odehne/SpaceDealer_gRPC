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

		public ProductsInStock GetNeededProducts(string planetId)
		{
			var lst = new ProductsInStock();

			var query = "SELECT ProductId, Interest FROM NeededProducts WHERE PlanetId = @planetId;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
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
						var product = Parent.ProductRepo.GetProduct(null, productId);
						product.PricePerTon = Tools.AddPercent(product.PricePerTon, interest);
						lst.AddProduct(product);
					}
				}
				reader.Close();
				Parent.CloseConnection(connection);
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public DbProductInStock GetProductInStock(string planetId, string productId)
		{
		
			var query = "SELECT Interest FROM NeededProducts WHERE PlanetId = @planetId AND ProductId = @productId;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@planetId", planetId);
				command.Parameters.AddWithValue("@productId", productId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var interest = reader.GetDouble(0);
						var product = Parent.ProductRepo.GetProduct(null, productId);
						product.PricePerTon = Tools.AddPercent(product.PricePerTon, interest);
						reader.Close();
						Parent.CloseConnection(connection);
						return product;
					}
				}
				reader.Close();
				Parent.CloseConnection(connection);
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}

			return null;
		}

		public void SaveNeededProduct(string planetId, string productId)
		{
			var p = GetProductInStock(planetId, productId);
			if (p != null)
				return;

			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + Parent.DbPath))
				{
					Parent.OpenConnection(connection);
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO NeededProducts (PlanetId, ProductId, Interest) VALUES (@planetId, @productId, @interest);";
						command.Parameters.AddWithValue("@planetId", planetId);
						command.Parameters.AddWithValue("@productId", productId);
						command.Parameters.AddWithValue("@interest", 10.0);
						try
						{
							command.ExecuteNonQuery();
							Parent.Logger.Log($"Needed product {productId} saved.", TraceEventType.Information);
						}
						catch (Exception e)
						{
							Parent.Logger.Log($"Failed to save needed product {e.Message}", TraceEventType.Error);
						}
						finally
						{
							Parent.CloseConnection(connection);
						}
					}
				}
			}
			catch (Exception e)
			{
				Parent.Logger.Log($"Failed to save NeededProducts for planet Id [{planetId},{productId}] {e.Message}", TraceEventType.Error);
			}
		}
	}
}
