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
        private static Random _random = new Random();

        public NeededProductsRepository(SqlPersistor parent)
		{
			Parent = parent;
		}

		public DbProductsInStock GetNeededProducts(string planetId)
		{
			var lst = new DbProductsInStock();
			//Parent.Logger.Log($"Loading needed products for planet {planetId}.");

			var query = "SELECT ProductId, Interest, Amount FROM NeededProducts WHERE PlanetId = @planetId;";
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

		//public DbProductInStock GetProductInStock(string planetId, string productId)
		//{
		//	//Parent.Logger.Log($"Loading needed product for planet {planetId}.");

		//	var query = "SELECT Interest FROM NeededProducts WHERE PlanetId = @planetId AND ProductId = @productId;";
		//	try
		//	{
		//		using var command = new SQLiteCommand(Parent.Connection);
		//		command.CommandText = query;
		//		command.Parameters.AddWithValue("@planetId", planetId);
		//		command.Parameters.AddWithValue("@productId", productId);
		//		using var reader = command.ExecuteReader();
		//		if (reader.HasRows)
		//		{
		//			while (reader.Read())
		//			{
		//				var interest = reader.GetDouble(0);
		//				var product = Parent.ProductRepo.GetItemById(productId);
		//				product.ApplyInterest(interest);
		//				reader.Close();
		//				return product;
		//			}
		//		}
		//		reader.Close();
		//	}
		//	catch (System.Exception e)
		//	{
		//		Parent.Logger.Error($"Failed to get NeededProducts for planet Id [{planetId}] {e.Message}");
		//	}

		//	return null;
		//}

		public void SaveNeededProduct(string planetId, string productId, double amount, double interest)
		{
			Parent.Logger.Verbose($"Saving needed product for planet {planetId}.");

            try
            {
				using (var update = new SQLiteCommand(Parent.Connection))
				{
					update.CommandText = "UPDATE NeededProducts SET Amount = @amount, Interest = @interest WHERE PlanetId = @planetId AND ProductId = @productId;";
					update.Parameters.AddWithValue("@amount", amount);
					update.Parameters.AddWithValue("@planetId", planetId);
					update.Parameters.AddWithValue("@productId", productId);
                    update.Parameters.AddWithValue("@interest", interest);
                    if (update.ExecuteNonQuery() > 0)
						return;
				}
			}
			catch (Exception e)
			{
				Parent.Logger.Error($"Failed to update NeededProducts for planet Id [{planetId},{productId}] {e.Message}");
				return;
			}

			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
                    command.CommandText = $"INSERT OR REPLACE INTO NeededProducts (PlanetId, ProductId, Interest, Amount) VALUES (@planetId, @productId, @interest, @amount);";
					command.Parameters.AddWithValue("@amount", amount);
					command.Parameters.AddWithValue("@planetId", planetId);
					command.Parameters.AddWithValue("@productId", productId);
					command.Parameters.AddWithValue("@interest", interest);
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
