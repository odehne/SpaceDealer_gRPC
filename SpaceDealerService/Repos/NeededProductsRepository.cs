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
			//Parent.Logger.Log($"Loading needed products for planet {planetId}.", TraceEventType.Information);

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
						product.PricePerTon = Tools.AddPercent(product.PricePerTon, interest);
						lst.AddProduct(product);
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public DbProductInStock GetProductInStock(string planetId, string productId)
		{
			//Parent.Logger.Log($"Loading needed product for planet {planetId}.", TraceEventType.Information);

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
				Parent.Logger.Log($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}", TraceEventType.Error);
			}

			return null;
		}

		public void SaveNeededProduct(string planetId, string productId)
		{
			Parent.Logger.Log($"Saving needed product for planet {planetId}.", TraceEventType.Information);
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
						Parent.Logger.Log($"Needed product {productId} saved.", TraceEventType.Information);
					}
					catch (Exception e)
					{
						Parent.Logger.Log($"Failed to save needed product {e.Message}", TraceEventType.Error);
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
