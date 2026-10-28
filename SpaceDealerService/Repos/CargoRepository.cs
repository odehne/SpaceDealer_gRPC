using SpaceDealerModels.Units;
using System;
using System.Collections.Concurrent;
using System.Data.SQLite;

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
							Quantity = reader.GetDouble(1)
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
                            Quantity = reader.GetDouble(2)
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

        public override DbCargo GetItemById(string id)
        {
            throw new NotImplementedException();
        }

        public override string GetItemId(string name)
        {
            throw new NotImplementedException();
        }

        public override DbCargo GetItemByName(string name)
        {
            throw new NotImplementedException();
        }

        public override void Save(DbCargo item)
        {
            Parent.Logger.Verbose($"Saving cargo.");
            var cargo = GetItemById(item.Id);
            if (cargo != null)
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
}
