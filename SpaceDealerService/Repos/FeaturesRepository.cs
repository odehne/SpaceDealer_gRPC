using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{
    public class  CargoRepository : Repository<DbCargo>
    {
        public CargoRepository(SqlPersistor parent) : base(parent)
        {
        }

        public override ConcurrentDictionary<string, DbCargo> GetAll(string shipId)
        {
            Parent.Logger.Verbose($"Loading cargo for ship {shipId}.");

            var lst = new ConcurrentDictionary<string, DbCargo>();

			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = "SELECT ProductId, Quantity FROM Cargo WHERE ShipId = @shipId";
				command.Parameters.AddWithValue("@shipId", shipId);
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						lst.TryAdd(reader.GetString(0), new DbCargo()
						{
							ShipId = shipId,
							Id = reader.GetString(0),
							Quantity = reader.GetInt32(1)
						});
					}
				}
				reader.Close();
			}
			catch (Exception e)
			{
				Parent.Logger.Error($"Failed to load cargo for ship {shipId}: [{e.Message}]");
			}
            return lst;
        }

        public override ConcurrentDictionary<string, DbCargo> GetAll()
        {
            Parent.Logger.Verbose($"Loading all cargo.");

            var lst = new ConcurrentDictionary<string, DbCargo>();

            try
            {
                using var command = new SQLiteCommand(Parent.Connection);
                command.CommandText = "SELECT ShipId, ProductId, Quantity FROM Cargo";
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lst.TryAdd(reader.GetString(1), new DbCargo()
                        {
                            ShipId = reader.GetString(0),
                            Id = reader.GetString(1),
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Parent.Logger.Error($"Failed to load cargo: [{e.Message}]");
            }
            return lst;
        }

        public override DbCargo GetItem(string name, string id)
        {
            throw new NotImplementedException();
        }

        public override string GetItemId(string name)
        {
            throw new NotImplementedException();
        }

        public override void Save(DbCargo item)
        {
            Parent.Logger.Verbose($"Saving feature.");
            var ft = GetItem(item.Name, null);
            if (ft != null)
                return;
            try
            {
                using (var command = new SQLiteCommand(Parent.Connection))
                {
                    command.CommandText = $"INSERT OR REPLACE INTO Cargo " +
                        $"(ShipId, ProductId, Quantity) VALUES " +
                        $"(@shipId, @productId, @quantity);";
                    command.Parameters.AddWithValue("@shipId", item.ShipId);
                    command.Parameters.AddWithValue("@productId", item.Id);
                    command.Parameters.AddWithValue("@quantity", item.Quantity);
                    try
                    {
                        command.ExecuteNonQuery();
                        Parent.Logger.Verbose($"Cargo {item.Id} saved.");
                    }
                    catch (Exception e)
                    {
                        Parent.Logger.Error($"Failed to save cargo {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Parent.Logger.Error($"Failed to persist cargo [{item.Id}], {e.Message}");
            }
        }
    }


    public class FeaturesRepository : Repository<DbFeature>
	{
		public FeaturesRepository(SqlPersistor parent) : base(parent)
		{
		}

		public override ConcurrentDictionary<string, DbFeature> GetAll()
		{
			Parent.Logger.Verbose($"Loading all features.");

			var lst = new ConcurrentDictionary<string, DbFeature>();

			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = "SELECT Id, Name, AttackBonus, RangeBonus, DefenceBonus, SpeedBonus, Description FROM Features";
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var id = reader.GetString(0);

                        lst.TryAdd(id, new DbFeature()
						{
							Id = id,
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
				Parent.Logger.Error($"Failed to load features with Id [{e.Message}]", TraceEventType.Error);
			}
			return lst;
		}

		public override ConcurrentDictionary<string, DbFeature> GetAll(string id)
		{
			Parent.Logger.Verbose($"Loading all features for ship {id}.");
			var lst = new ConcurrentDictionary<string, DbFeature>();

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
						lst.TryAdd(GetItem("", reader.GetString(0)).Id, GetItem("", reader.GetString(0)));
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to determine if ship has featured {e.Message}.");
			}
			return lst;
		}

		public override DbFeature GetItem(string name, string id)
		{
			Parent.Logger.Verbose($"Loading feature {name}, {id}.");
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
					Parent.Logger.Error($"Failed to get feature with Id [{id}]");
				}
				reader.Close();
			}
			catch (Exception e)
			{
				Parent.Logger.Error($"Failed to get feature with Id [{e.Message}]");
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
			Parent.Logger.Verbose($"Saving feature.");
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
						Parent.Logger.Verbose($"Feature {item.Id} saved.");
					}
					catch (Exception e)
					{
						Parent.Logger.Error($"Failed to save feature {e.Message}");
					}
				}
			}
			catch (Exception e)
			{
				Parent.Logger.Error($"Failed to persist feature [{item.Id}], {e.Message}");
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
			Parent.Logger.Verbose($"Deleting ship feature reference {id}.");

			var query = "DELETE FROM ShipFeatures WHERE Id = @id;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", id);
				try
				{
					command.ExecuteNonQuery();
					Parent.Logger.Verbose($"Ship {id} deleted.");
				}
				catch (System.Exception e)
				{
					Parent.Logger.Error($"Failed to delete ship {e.Message}");
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Connection failed {e.Message}");
			}
		}

		public bool DeleteFeature(string featureId)
		{
			Parent.Logger.Verbose($"Deleting feature {featureId}.");
			var query = "DELETE FROM Features WHERE Id = @id;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", featureId);
				try
				{
					command.ExecuteNonQuery();
					Parent.Logger.Verbose($"Feature {featureId} deleted.");
				}
				catch (System.Exception e)
				{
					Parent.Logger.Error($"Failed to delete feature {e.Message}");
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Connection failed {e.Message}");
			}
			return false;
		}

		public bool ShipHasFeature(string shipId, string featureId)
		{
			Parent.Logger.Verbose($"Checking if ship has feature.");
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
				Parent.Logger.Error($"Failed to determine if ship has feature {e.Message}.");
			}
			return false;
		}



		public void SaveShipFeature(string shipId, string featureId)
		{
			Parent.Logger.Verbose($"Saving ship feature.");
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
						Parent.Logger.Verbose($"Ship feature {featureId} saved.");
					}
					catch (System.Exception e)
					{
						Parent.Logger.Error($"Failed to save ship feature {e.Message}");
					}
					
				}
				catch (System.Exception e)
				{
					Parent.Logger.Error($"Failed to save ship feature {e.Message}");
				}
			}
			
		}

	
	}
}
