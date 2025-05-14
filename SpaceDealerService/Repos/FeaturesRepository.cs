using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class FeaturesRepository : Repository<DbFeature>
	{
		public FeaturesRepository(SqlPersistor parent) : base(parent)
		{
		}

		public override List<DbFeature> GetAll()
		{
			//Parent.Logger.Log($"Loading all features.", TraceEventType.Information);

			var lst = new DbFeatures();

			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
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
			}
			catch (Exception e)
			{
				Parent.Logger.Log($"Failed to load features with Id [{e.Message}]", TraceEventType.Error);
			}
			return lst;
		}

		public override List<DbFeature> GetAll(string id)
		{
			//Parent.Logger.Log($"Loading all features for ship {id}.", TraceEventType.Information);
			var lst = new DbFeatures();

			var query = "SELECT FeatureId FROM ShipFeatures WHERE ShipId = @shipId";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@shipId", id);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						lst.Add(GetItem("", reader.GetString(0)));
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to determine if ship has featured {e.Message}.", TraceEventType.Error);
			}
			return lst;
		}

		public override DbFeature GetItem(string name, string id)
		{
			//Parent.Logger.Log($"Loading feature {name}, {id}.", TraceEventType.Information);
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
				using var command = new SQLiteCommand(Parent.Connection);
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
							AttackBonus = reader.GetInt32(2),
							RangeBonus = reader.GetInt32(3),
							DefenceBonus = reader.GetInt32(4),
							SpeedBonus = reader.GetInt32(5),
							Description = reader.GetString(6)
						};
						reader.Close();
						return ret;
					}
				}
				else
				{
					Parent.Logger.Log($"Failed to get feature with Id [{id}]", TraceEventType.Error);
				}
				reader.Close();
			}
			catch (Exception e)
			{
				Parent.Logger.Log($"Failed to get feature with Id [{e.Message}]", TraceEventType.Error);
			}
			return null;
		}

		public override string GetItemId(string name)
		{
			var feature = GetItem(name, null);
            if (feature != null)
                return feature.Id;
            else
                return null;
        }

		public override void Save(DbFeature item)
		{
			Parent.Logger.Log($"Saving feature.", TraceEventType.Information);
			var ft = GetItem(item.Name, null);
			if (ft != null)
				return;
			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					command.CommandText = $"INSERT OR REPLACE INTO Features " +
						$"(Id,Name,AttackBonus,RangeBonus,DefenceBonus,SpeedBonus,Description) VALUES " +
						$"(@id, @name,@attackBonus,@rangeBonus,@defenceBonus,@speedBonus,@description);";
					command.Parameters.AddWithValue("@id", item.Id);
					command.Parameters.AddWithValue("@name", item.Name);
					command.Parameters.AddWithValue("@attackBonus", item.AttackBonus);
					command.Parameters.AddWithValue("@rangeBonus", item.RangeBonus);
					command.Parameters.AddWithValue("@defenceBonus", item.DefenceBonus);
					command.Parameters.AddWithValue("@speedBonus", item.SpeedBonus);
					command.Parameters.AddWithValue("@description", item.Description);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Log($"Feature {item.Id} saved.", TraceEventType.Information);
					}
					catch (Exception e)
					{
						Parent.Logger.Log($"Failed to save feature {e.Message}", TraceEventType.Error);
					}
				}
			}
			catch (Exception e)
			{
				Parent.Logger.Log($"Failed to persist feature [{item.Id}], {e.Message}", TraceEventType.Error);
			}
		}

	

		public void SaveAll(DbFeatures features)
		{
			foreach (var feature in features)
			{
				Save(feature);
			}
		}


		public void DeleteShipFeature(string id)
		{
			Parent.Logger.Log($"Deleting ship feature reference {id}.", TraceEventType.Information);

			var query = "DELETE FROM ShipFeatures WHERE Id = @id;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
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
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Connection failed {e.Message}", TraceEventType.Error);
			}
		}

		public bool DeleteFeature(string featureId)
		{
			Parent.Logger.Log($"Deleting feature {featureId}.", TraceEventType.Information);
			var query = "DELETE FROM Features WHERE Id = @id;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
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
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Connectio failed {e.Message}", TraceEventType.Error);
			}
			return false;
		}

		public bool ShipHasFeature(string shipId, string featureId)
		{
			Parent.Logger.Log($"Checking if ship has feature.", TraceEventType.Information);
			var query = "SELECT Id FROM ShipFeatures WHERE ShipId = @shipId AND FeatureId = @featureId;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@shipId", shipId);
				command.Parameters.AddWithValue("@featureId", featureId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					reader.Close();
					return true;
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to determine if ship has featured {e.Message}.", TraceEventType.Error);
			}
			return false;
		}



		public void SaveShipFeature(string shipId, string featureId)
		{
			Parent.Logger.Log($"Saving ship feature.", TraceEventType.Information);
			if (!ShipHasFeature(shipId, featureId))
			{
				try
				{
					using var command = new SQLiteCommand(Parent.Connection);
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
					
				}
				catch (System.Exception e)
				{
					Parent.Logger.Log($"Failed to save ship feature {e.Message}", TraceEventType.Error);
				}
			}
			
		}

	
	}
}
