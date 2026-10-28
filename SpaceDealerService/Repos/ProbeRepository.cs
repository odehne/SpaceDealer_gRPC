using SpaceDealerModels.Units;
using System;
using System.Collections.Concurrent;
using System.Data.SQLite;

namespace SpaceDealerService.Repos
{
    public class ProbeRepository : Repository<Probe>
    {
        public ProbeRepository(SqlPersistor parent) : base(parent)
        {
        }

        public override ConcurrentDictionary<string, Probe> GetAll()
        {
            Parent.Logger.Verbose($"Loading all probes.");

            var lst = new ConcurrentDictionary<string, Probe>();

            try
            {
                using var command = new SQLiteCommand(Parent.Connection);
                command.CommandText = "SELECT Id, Name, ShipId, State, X, Y, Z FROM Probes";
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lst.TryAdd(reader.GetString(0), new Probe()
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            ShipId = reader.GetString(2),
                            State = (ProbeStates)reader.GetInt32(3),
                            CurrentSector = new DbCoordinates(reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6))
                        });
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Parent.Logger.Error($"Failed to load all probes: [{e.Message}]");
            }
            return lst;
        }

        public override ConcurrentDictionary<string, Probe> GetAll(string shipId)
        {
            Parent.Logger.Verbose($"Loading probes for ship {shipId}.");

            var lst = new ConcurrentDictionary<string, Probe>();

            try
            {
                using var command = new SQLiteCommand(Parent.Connection);
                command.CommandText = "SELECT Id, Name, ShipId, State, X, Y, Z  FROM Probes WHERE ShipId = @shipId";
                command.Parameters.AddWithValue("@shipId", shipId);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lst.TryAdd(reader.GetString(0), new Probe()
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            ShipId = reader.GetString(2),
                            State = (ProbeStates)reader.GetInt32(3),
                            CurrentSector = new DbCoordinates(reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6))
                        });
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Parent.Logger.Error($"Failed to load probe for ship {shipId}: [{e.Message}]");
            }
            return lst;
        }

        public override Probe GetItemById(string id)
        {
            var query = "SELECT Id, Name, ShipId, State, X, Y, Z  FROM Probes WHERE Id = @id;";
            try
            {
                using var command = new SQLiteCommand(Parent.Connection);
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var probe = new Probe
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            ShipId = reader.GetString(2),
                            State = (ProbeStates)reader.GetInt32(3),
                            CurrentSector = new DbCoordinates(reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6))
                        };
                        return probe;
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Parent.Logger.Error($"Failed to read probe {e.Message}");
            }

            return null;
        }

        public override Probe GetItemByName(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetItemId(string name)
        {
            throw new NotImplementedException();
        }

        public override void Save(Probe item)
        {
            Parent.Logger.Verbose($"Saving probe with id {item.Id}.");
            
            try
            {
                using (var command = new SQLiteCommand(Parent.Connection))
                {
                    command.CommandText = $"INSERT OR REPLACE INTO Probes (Id, Name, ShipId, State, X, Y, Z) VALUES (@id, @name, @shipId, @state, @x, @y, @z);";
                    command.Parameters.AddWithValue("@id", item.Id);
                    command.Parameters.AddWithValue("@name", item.Name);
                    command.Parameters.AddWithValue("@shipId", item.ShipId);
                    command.Parameters.AddWithValue("@state", (int)item.State);
                    command.Parameters.AddWithValue("@x", item.CurrentSector.X);
                    command.Parameters.AddWithValue("@y", item.CurrentSector.Y);
                    command.Parameters.AddWithValue("@z", item.CurrentSector.Z);
                    try
                    {
                        command.ExecuteNonQuery();
                        Parent.Logger.Information($"Probe {item.Name} saved.");
                    }
                    catch (System.Exception e)
                    {
                        Parent.Logger.Error($"Failed to add probe {e.Message}");
                    }
                }
            }
            catch (System.Exception e)
            {
                Parent.Logger.Error($"Failed to add probe {e.Message}");
            }
        }
    }
}
