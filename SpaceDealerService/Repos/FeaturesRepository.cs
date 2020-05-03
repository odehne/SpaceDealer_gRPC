using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SpaceDealerService.Repos
{
	public class FeaturesRepository
	{
		public string DbPath { get; set; }

		public FeaturesRepository(string dbPath)
		{
			DbPath = dbPath;
		}
		public List<DbFeature> GetFeatures()
		{
			var lst = new List<DbFeature>();
			return lst;
		}

		public string GetFeatureId(string featureName)
		{
			var p = new DbFeature();
			var query = "SELECT Id FROM Features WHERE Name = @name;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@name", featureName);
				return (string)command.ExecuteScalar();
				
			}
			catch (System.Exception e)
			{
				return null;
			}
		}

		public DbFeature GetFeature(string featureId)
		{
			var p = new DbFeature();
			var query = "SELECT Id, Name, AttackBonus, RangeBonus, SpeedBonus, Description FROM Features WHERE Id = @id;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", featureId);
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						return new DbFeature
						{
							Id = reader.GetString(0),
							Name = reader.GetString(1),
							AttackBonus= reader.GetInt32(2),
							RangeBonus = reader.GetInt32(3),
							DefenceBonus = reader.GetInt32(4),
							SpeedBonus = reader.GetInt32(5),
							Description = reader.GetString(6)
						};
					}
				}
				else
				{
					Console.WriteLine("No rows found.");
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				throw e;
			}
			return p;
		}

		public bool DeleteShipFeature(string id)
		{
			var query = "DELETE FROM ShipFeatures WHERE Id = @id;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", id);
				var rows = command.ExecuteNonQuery();
				if (rows > 0)
					return true;
			}
			catch (System.Exception e)
			{
				throw e;
			}
			return false;
		}

		public bool DeleteFeature(string featureId)
		{
			var query = "DELETE FROM Features WHERE Id = @id;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", featureId);
				var rows = command.ExecuteNonQuery();
				if (rows > 0)
					return true;
			}
			catch (System.Exception e)
			{
				throw e;
			}
			return false;
		}


		public bool ShipHasFeature(string shipId, string featureId)
		{
			var query = "SELECT Id FROM ShipFeatures WHERE ShipId = @shipId AND FeatureId = @featureId;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@shipId", shipId);
				command.Parameters.AddWithValue("@featureId", featureId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
					return true;
			}
			catch (System.Exception e)
			{
				throw e;
			}
			return false;
		}

		public void Save(DbFeature feature)
		{

			var id = GetFeatureId(feature.Name);
			if (id != null)
				feature.Id = id;
			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + DbPath))
				{
					connection.Open();
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO ShipFeatures " +
							$"(Id,Name,AttackBonus,RangeBonus,DefenceBonus,SpeedBonus,Description) VALUES " +
							$"(@id, @name,@attackBonus,@rangeBonus,@defenceBonus,@speedBonus,@description);";
						command.Parameters.AddWithValue("@id", feature.Id);
						command.Parameters.AddWithValue("@name", feature.Name);
						command.Parameters.AddWithValue("@attackBonus", feature.AttackBonus);
						command.Parameters.AddWithValue("@rangeBonus", feature.RangeBonus);
						command.Parameters.AddWithValue("@defenceBonus", feature.Description);
						command.Parameters.AddWithValue("@speedBonus", feature.SpeedBonus);
						command.Parameters.AddWithValue("@description", feature.Description);
						command.ExecuteNonQuery();
					}
				}
			}
			catch (System.Exception e)
			{
				throw e;
			}
		}


		public void SaveShipFeature(string shipId, string featureId)
		{
			if(!ShipHasFeature(shipId, featureId))
			{
				try
				{
					using var connection = new SQLiteConnection("Data Source=" + DbPath);
					connection.Open();
					using var command = new SQLiteCommand(connection);
					command.CommandText = $"INSERT INTO ShipFeatures (Id,ShipId,FeatureId) VALUES (@id,@shipId,@featureId);";
					command.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
					command.Parameters.AddWithValue("@featureId", featureId);
					command.Parameters.AddWithValue("@shipId", shipId);
					command.ExecuteNonQuery();
				}
				catch (System.Exception e)
				{
					throw e;
				}
			}
			
		}
	}
}
