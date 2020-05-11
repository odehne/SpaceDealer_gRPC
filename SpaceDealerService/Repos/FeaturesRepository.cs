using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class FeaturesRepository
	{
		public SqlPersistor Parent { get; set; }

		public FeaturesRepository(SqlPersistor parent)
		{
			Parent = parent;
		}
		public DbFeatures GetFeatures()
		{
			var lst = new DbFeatures();

			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = "SELECT Id, Name, AttackBonus, RangeBonus, DefenceBonus, SpeedBonus, Description FROM Features";
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						lst.Add(new DbFeature()
						{
							Id = reader.GetString(0),
							Name = reader.GetString(1),
							AttackBonus = reader.GetInt32(2),
							RangeBonus = reader.GetInt32(3),
							DefenceBonus = reader.GetInt32(4),
							SpeedBonus = reader.GetInt32(5),
							Description = reader.GetString(6)
						});
						
					}
				}
				reader.Close();
				Parent.CloseConnection(connection);
			}
			catch (Exception e)
			{
				Parent.Logger.Log($"Failed to load features with Id [{e.Message}]", TraceEventType.Error);
			}
			return lst;
		}

		public void SaveAll(DbFeatures features)
		{
			foreach (var feature in features)
			{
				Save(feature);
			}
		}

		public DbFeature GetFeature(string name, string id)
		{
			var parameter = new SQLiteParameter();
			var query = "SELECT Id, Name, AttackBonus, RangeBonus, DefenceBonus, SpeedBonus, Description FROM Features WHERE ";
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
			
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.Add(parameter);
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var ret = new DbFeature
						{
							Id = reader.GetString(0),
							Name = reader.GetString(1),
							AttackBonus= reader.GetInt32(2),
							RangeBonus = reader.GetInt32(3),
							DefenceBonus = reader.GetInt32(4),
							SpeedBonus = reader.GetInt32(5),
							Description = reader.GetString(6)
						};
						reader.Close();
						Parent.CloseConnection(connection);
						return ret;
					}
				}
				else
				{
					Parent.Logger.Log($"Failed to get feature with Id [{id}]", TraceEventType.Error);
				}
				reader.Close();
				Parent.CloseConnection(connection);

			}
			catch (Exception e)
			{
				Parent.Logger.Log($"Failed to get feature with Id [{e.Message}]", TraceEventType.Error);
			}
			return null;
		}

		public void DeleteShipFeature(string id)
		{
			var query = "DELETE FROM ShipFeatures WHERE Id = @id;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", id);
				try
				{
					command.ExecuteNonQuery();
					Parent.Logger.Log($"Ship {id} deleted.", TraceEventType.Information);
				}
				catch (System.Exception e)
				{
					Parent.Logger.Log($"Failed to delete ship {e.Message}", TraceEventType.Error);
				}
				finally
				{
					Parent.CloseConnection(connection);
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Connection failed {e.Message}", TraceEventType.Error);
			}
		}

		public bool DeleteFeature(string featureId)
		{
			var query = "DELETE FROM Features WHERE Id = @id;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", featureId);
				try
				{
					command.ExecuteNonQuery();
					Parent.Logger.Log($"Feature {featureId} deleted.", TraceEventType.Information);
				}
				catch (System.Exception e)
				{
					Parent.Logger.Log($"Failed to delete feature {e.Message}", TraceEventType.Error);
				}
				finally
				{
					Parent.CloseConnection(connection);
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Connectio failed {e.Message}", TraceEventType.Error);
			}
			return false;
		}

		public bool ShipHasFeature(string shipId, string featureId)
		{
			var query = "SELECT Id FROM ShipFeatures WHERE ShipId = @shipId AND FeatureId = @featureId;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@shipId", shipId);
				command.Parameters.AddWithValue("@featureId", featureId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					reader.Close();
					Parent.CloseConnection(connection);
					return true;
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to determine if ship has featured {e.Message}.", TraceEventType.Error);
			}
			return false;
		}

		public void Save(DbFeature feature)
		{

			var ft = GetFeature(feature.Name, null);
			if (ft != null)
				return;
			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + Parent.DbPath))
				{
					Parent.OpenConnection(connection);
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO Features " +
							$"(Id,Name,AttackBonus,RangeBonus,DefenceBonus,SpeedBonus,Description) VALUES " +
							$"(@id, @name,@attackBonus,@rangeBonus,@defenceBonus,@speedBonus,@description);";
						command.Parameters.AddWithValue("@id", feature.Id);
						command.Parameters.AddWithValue("@name", feature.Name);
						command.Parameters.AddWithValue("@attackBonus", feature.AttackBonus);
						command.Parameters.AddWithValue("@rangeBonus", feature.RangeBonus);
						command.Parameters.AddWithValue("@defenceBonus", feature.DefenceBonus);
						command.Parameters.AddWithValue("@speedBonus", feature.SpeedBonus);
						command.Parameters.AddWithValue("@description", feature.Description);
						try
						{
							command.ExecuteNonQuery();
							Parent.Logger.Log($"Feature {feature.Id} saved.", TraceEventType.Information);
						}
						catch (Exception e)
						{
							Parent.Logger.Log($"Failed to save feature {e.Message}", TraceEventType.Error);
						}
					}
					Parent.CloseConnection(connection);
				}
			}
			catch (Exception e)
			{
				Parent.Logger.Log($"Failed to persist feature [{feature.Id}], {e.Message}", TraceEventType.Error);
			}
		}


		public void SaveShipFeature(string shipId, string featureId)
		{
			if(!ShipHasFeature(shipId, featureId))
			{
				try
				{
					using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
					Parent.OpenConnection(connection);
					using var command = new SQLiteCommand(connection);
					command.CommandText = $"INSERT INTO ShipFeatures (Id,ShipId,FeatureId) VALUES (@id,@shipId,@featureId);";
					command.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
					command.Parameters.AddWithValue("@featureId", featureId);
					command.Parameters.AddWithValue("@shipId", shipId);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Log($"Ship feature {featureId} saved.", TraceEventType.Information);
					}
					catch (System.Exception e)
					{
						Parent.Logger.Log($"Failed to save ship feature {e.Message}", TraceEventType.Error);
					}
					finally
					{
						Parent.CloseConnection(connection);
					}
				}
				catch (System.Exception e)
				{
					Parent.Logger.Log($"Failed to save ship feature {e.Message}", TraceEventType.Error);
				}
			}
			
		}
	}
}
