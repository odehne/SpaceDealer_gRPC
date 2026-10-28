using SpaceDealerModels.Units;
using System;
using System.Collections.Concurrent;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

namespace SpaceDealerService.Repos
{

    public class FeaturesRepository : Repository<DbFeature>
	{
        public static DbFeatures DefaultFeatures { get; set; }

        public static void AddDefaultFeatures()
        {
            if (DefaultFeatures == null)
                DefaultFeatures = new DbFeatures();
            if (DefaultFeatures.Count < 5)
            {
                DefaultFeatures.Add(new DbFeature("8da6d97a-b7dc-4ec4-8655-e7548518f11c", $"{FeatureNames.ExtraShield}", "Verbesserte Schilde (+1)", FeatureBonusTypes.DefenceBonus, 1, 1750.0));
                DefaultFeatures.Add(new DbFeature("8b51dd3b-718b-468a-b6df-6a0dacd09cef", $"{FeatureNames.ExtraPhasers}", "Phaser-Bänke", FeatureBonusTypes.AttackBonus, 1, 2800.0));
                DefaultFeatures.Add(new DbFeature("943707fa-134f-460a-b7f9-e625c7b00487", $"{FeatureNames.ExtraTorpedos}", "Torpedos", FeatureBonusTypes.AttackBonus, 2, 7600.0));
                DefaultFeatures.Add(new DbFeature("8b2b2fc7-5f86-442e-b826-48fd449168a6", $"{FeatureNames.AdvancedWarp_1}", "Verbesserte Geschwindigkeit", FeatureBonusTypes.TravelSpeedBonus, 1, 12500.0));
                DefaultFeatures.Add(new DbFeature("d6147226-9e72-4d43-8c7a-b840e04e52f7", $"{FeatureNames.SignalRange_1}", "Verbesserter Signalempfang L", FeatureBonusTypes.RangeBonus, 1, 11750.0));
                DefaultFeatures.Add(new DbFeature("8fb7473e-c84a-46d3-9e40-a4bab3d8f00c", $"{FeatureNames.SignalRange_2}", "Verbesserter Signalempfang XL", FeatureBonusTypes.RangeBonus, 2, 17050.0));
                DefaultFeatures.Add(new DbFeature("c79d6c51-2f65-465c-bb20-6cc71e46a7d5", $"{FeatureNames.AdditionalCargoSpace_30}", "Erweiterter Laderaum L", FeatureBonusTypes.FreightBonus, 30, 8500.0));
                DefaultFeatures.Add(new DbFeature("c4c384b1-0fd3-4032-8883-6a9a418a06af", $"{FeatureNames.AdditionalCargoSpace_80}", "Erweiterter Laderaum XL", FeatureBonusTypes.FreightBonus, 80, 16500.0));
                DefaultFeatures.Add(new DbFeature("a3577842-d6e8-49d5-8084-c5f59c944d01", $"{FeatureNames.AdditionalProbe}", "Zusätzliche Sonde", FeatureBonusTypes.AddtionalProbe, 1, 26500.0));
            }

        }

        public FeaturesRepository(SqlPersistor parent) : base(parent)
		{
		}

        public override DbFeatures GetAll()
		{
			Parent.Logger.Verbose($"Loading all features.");

			var lst = new DbFeatures();

			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = "SELECT Id, Name, Description, BonusType, BonusValue, Costs FROM Features";
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var id = reader.GetString(0);

                        lst.TryAdd(id, new DbFeature()
						{
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
							Description = reader.GetString(2),
                            BonusType = (FeatureBonusTypes)reader.GetInt32(3),
                            BonusValue = reader.GetInt32(4),
                            Costs = reader.GetDouble(5)
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

      

        public override DbFeatures GetAll(string id)
		{
			Parent.Logger.Verbose($"Loading all features for ship {id}.");
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
                        lst.TryAdd(GetItemById(reader.GetString(0)).Id, GetItemById(reader.GetString(0)));
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to determine if ship has featured {e.Message}.");
			}
			return lst;
		}

		public override DbFeature GetItemById(string id)
		{
			Parent.Logger.Verbose($"Loading feature by id [{id}].");
			var parameter = new SQLiteParameter();
			var query = "SELECT Id, Name, Description, BonusType, BonusValue, Costs FROM Features WHERE Id = @id;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", id);
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var ret = new DbFeature
						{
                            Id = id,
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            BonusType = (FeatureBonusTypes)reader.GetInt32(3),
                            BonusValue = reader.GetInt32(4),
                            Costs = reader.GetDouble(5)
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
			var feature = GetItemByName(name);
            if (feature != null)
                return feature.Id;
            else
                return null;
        }

		public override void Save(DbFeature item)
		{
			Parent.Logger.Verbose($"Saving feature.");
			var ft = GetItemById(item.Id);
			if (ft != null)
				return;
			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					command.CommandText = $"INSERT OR REPLACE INTO Features " +
											$"(Id, Name, Description, BonusType, BonusValue, Costs) VALUES " +
											$"(@id, @name,@description,@bonusType,@bonusValue,@costs);";
					command.Parameters.AddWithValue("@id", item.Id);
					command.Parameters.AddWithValue("@name", item.Name);
					command.Parameters.AddWithValue("@description", item.Description);
                    command.Parameters.AddWithValue("@bonusType", item.BonusType);
                    command.Parameters.AddWithValue("@bonusValue", item.BonusValue);
                    command.Parameters.AddWithValue("@costs", item.Costs);

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




        public void SaveDefaultFeatures()
        {
			AddDefaultFeatures();

            foreach (var feature in DefaultFeatures.Values)
            {
                Save(feature);
            }
        }



        public void SaveAll(DbFeatures features)
		{
			foreach (var feature in features.Values)
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

        public override DbFeature GetItemByName(string name)
        {
            Parent.Logger.Verbose($"Loading feature by id [{name}].");
            var parameter = new SQLiteParameter();
            var query = "SELECT Id, Name, Description, BonusType, BonusValue, Costs FROM Features WHERE name = @name;";
            try
            {
                using var command = new SQLiteCommand(Parent.Connection);
                command.CommandText = query;
                command.Parameters.AddWithValue("@name", name);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var ret = new DbFeature
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            BonusType = (FeatureBonusTypes)reader.GetInt32(3),
                            BonusValue = reader.GetInt32(4),
                            Costs = reader.GetDouble(5)
                        };
                        reader.Close();
                        return ret;
                    }
                }
                else
                {
                    Parent.Logger.Error($"Failed to get feature with Id [{name}]");
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Parent.Logger.Error($"Failed to get feature with Id [{e.Message}]");
            }
            return null;
        }
    }
}
