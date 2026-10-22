using SpaceDealer;
using SpaceDealerModels;
using SpaceDealerModels.Units;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Xml.Linq;

namespace SpaceDealerService.Repos
{

	public class ShipsRepository : Repository<DbShip>
	{
		public ShipsRepository(SqlPersistor parent) : base(parent)
		{
		}


		public override void Save(DbShip ship)
		{
			Parent.Logger.Verbose($"Saving ship with id {ship.Id}.");

			if (ship.Cruise == null)
			{
				var shipOwner = Program.TheGame.FleetCommanders.GetPlayerById(ship.PlayerId);
                var sourcePlanet = shipOwner.HomePlanet;
                var destinationPlanet = shipOwner.DiscoveredPlanets.GetRandomPlanet();
                while (sourcePlanet.Id == destinationPlanet.Id)
                {
                    destinationPlanet = Program.TheGame.Galaxy.GetRandomPlanet();
                }
                ship.Cruise = new DbJourney() { CurrentSector = sourcePlanet.Sector, Destination = destinationPlanet, Departure = sourcePlanet, Parent = ship };
			}

			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					command.CommandText = $"INSERT OR REPLACE INTO Ships (Id, PlayerId, Name, PicturePath, CargoSize, Hull, Shield, ShipState, Journey_Source, Journey_Destination, Current_SectorX, Current_SectorY,Current_SectorZ ) " +
						$"VALUES (@id, @playerId, @name, @picturePath, @cargoSize, @hull, @shield, @shipState, @source, @destination, @currentX, @currentY, @currentZ);";
					command.Parameters.AddWithValue("@id", ship.Id);
					command.Parameters.AddWithValue("@name", ship.Name);
					command.Parameters.AddWithValue("@playerId", ship.PlayerId);
					command.Parameters.AddWithValue("@picturePath", ship.PicturePath);
					command.Parameters.AddWithValue("@cargoSize", ship.CargoSize);
					command.Parameters.AddWithValue("@hull", ship.Hull);
					command.Parameters.AddWithValue("@shield", ship.Shields);
					command.Parameters.AddWithValue("@shipState", ship.State);

					command.Parameters.AddWithValue("@source", ship.Cruise.Departure.Id);
					command.Parameters.AddWithValue("@destination", ship.Cruise.Destination.Id);
					command.Parameters.AddWithValue("@currentX", ship.Cruise.CurrentSector.X);
					command.Parameters.AddWithValue("@currentY", ship.Cruise.CurrentSector.Y);
					command.Parameters.AddWithValue("@currentZ", ship.Cruise.CurrentSector.Z);

					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Verbose($"Ship {ship.Id} saved.");
					}
					catch (Exception e)
					{
						Parent.Logger.Error($"Failed to save ship {e.Message}");
					}
				
				}
				foreach (var ft in ship.Features)
				{
					if (ft != null)
					{
						var featureId = Parent.FeaturesRepo.GetItemId(ft.Name);
                        if(string.IsNullOrEmpty(featureId))
						{
                            Parent.FeaturesRepo.Save(ft);
                            featureId = ft.Id;
                        }

                        
                        Parent.FeaturesRepo.SaveShipFeature(ship.Id, featureId);
                        Parent.Logger.Verbose($"Ship features {ft.Name}.");
                    }
				}
				Parent.Logger.Verbose($"Ship {ship.Name} saved.");
				
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to save ship with Id [{ship.Id}] {e.Message}");

			}
		}

		public override ConcurrentDictionary<string, DbShip> GetAll()
		{
			throw new NotImplementedException();
        }

		public override DbShip GetItem(string name, string id)
		{
			throw new NotImplementedException();
            //var parameter = new SQLiteParameter();
            ////Parent.Logger.Log($"Loading ship with {name}, {id}.");

            //var query = "SELECT Id, PlayerId, Name, PicturePath, CargoSize, Hull, Shield, ShipState," +
            //			" Journey_Source, Journey_Destination, Current_SectorX," +
            //			" Current_SectorY, Current_SectorZ FROM Ships WHERE ";
            //if (!string.IsNullOrEmpty(name))
            //{
            //	query += "Name = @name;";
            //	parameter.ParameterName = "@name";
            //	parameter.Value = name;
            //}
            //else
            //{
            //	query += "Id = @id;";
            //	parameter.ParameterName = "@id";
            //	parameter.Value = id;
            //}
            //try
            //{
            //	using var command = new SQLiteCommand(Parent.Connection);
            //	command.CommandText = query;
            //	command.Parameters.Add(parameter);
            //	using var reader = command.ExecuteReader();
            //	if (reader.HasRows)
            //	{
            //		while (reader.Read())
            //		{
            //                     var shipId = reader.GetString(0);
            //			var playerId = reader.GetString(1);
            //			var shipName = reader.GetString(2);
            //			var picturePath = reader.GetString(3);
            //			var hull = reader.GetInt32(5);
            //			var shields = reader.GetInt32(6);
            //			var state = (SpaceDealer.Enums.ShipState)reader.GetInt32(7);
            //			var features = Program.Persistor.FeaturesRepo.GetAll(shipId);
            //			var player = Program.TheGame.FleetCommanders.GetPlayerById(playerId);

            //			var source = Program.Persistor.PlanetsRepo.GetItem("", reader.GetString(8));
            //			var destination = Program.Persistor.PlanetsRepo.GetItem("", reader.GetString(9));
            //			var currentSector = new DbCoordinates(reader.GetInt32(10), reader.GetInt32(11), reader.GetInt32(12));

            //                    var feats = new DbFeatures();

            //                     foreach (var ft in features.Values)
            //                         feats.Add(ft);

            //                     var ship = new DbShip(shipName, player, feats, player.Fleet);

            //                     ship.CurrentPlanet = source;
            //			ship.Cruise = new DbJourney(source, destination, currentSector, ship);

            //                     reader.Close();

            //			return ship;
            //		}
            //	}

            //}
            //catch (System.Exception e)
            //{
            //	Parent.Logger.Error($"Failed to get ship with Id [{id}] {e.Message}");
            //}
            //return null;
        }

		public DbShip GetShipByPlayer(DbPlayer player, string shipId)
        {
            var parameter = new SQLiteParameter();
            //Parent.Logger.Log($"Loading ship with {name}, {id}.");

            var query = "SELECT Id, PlayerId, Name, PicturePath, CargoSize, Hull, Shield, ShipState," +
                        " Journey_Source, Journey_Destination, Current_SectorX," +
                        " Current_SectorY, Current_SectorZ FROM Ships WHERE Id = @id;";
          
            try
            {
                using var command = new SQLiteCommand(Parent.Connection);
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", shipId);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        shipId = reader.GetString(0);
                        var playerId = reader.GetString(1);
                        var shipName = reader.GetString(2);
                        var picturePath = reader.GetString(3);
                        var hull = reader.GetInt32(5);
                        var shields = reader.GetInt32(6);
                        var state = (SpaceDealer.Enums.ShipState)reader.GetInt32(7);
                        var features = Program.TheGame.GetAllFeaturesByShipId(shipId);

                        var source = Program.TheGame.GetPlanetById(reader.GetString(8));
                        var destination = Program.TheGame.GetPlanetById(reader.GetString(9));
                        var currentSector = new DbCoordinates(reader.GetInt32(10), reader.GetInt32(11), reader.GetInt32(12));


                        if(destination == null)
                        {
                            destination = Program.TheGame.Galaxy.GetRandomPlanet();
                        }

                        var feats = new DbFeatures();

                        foreach (var ft in features.Values)
                            feats.Add(ft);

                        var ship = new DbShip(shipName, player, feats, player.Fleet)
                        {
                            Parent = player.Fleet,
                            CurrentPlanet = source
                        };
                        ship.Cruise = new DbJourney(source, destination, currentSector, ship);

                        reader.Close();

                        return ship;
                    }
                }

            }
            catch (System.Exception e)
            {
                Parent.Logger.Error($"Failed to get ship with Id [{shipId}] {e.Message}");
            }
            return null;
        }


        public override string GetItemId(string name)
		{
			throw new NotImplementedException();
		}

        public Ships GetAllShipsByPlayer(DbPlayer player)
        {
            //Parent.Logger.Log($"Loading all ships of player {playerId}.");
            var ids = new List<string>();
            var lst = new Ships();

            var query = "SELECT Id FROM Ships WHERE PlayerId = @playerId;";
            try
            {
                using var command = new SQLiteCommand(Parent.Connection);
                command.CommandText = query;
                command.Parameters.AddWithValue("@playerId", player.Id);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetString(0));
                    }
                }
                reader.Close();
                //Parent.CloseConnection(connection);

            }
            catch (System.Exception e)
            {
                Parent.Logger.Error($"Failed to get ships for player Id [{player.Id}] {e.Message}");
            }

            foreach (var shipId in ids)
            {
                lst.TryAdd(shipId, GetShipByPlayer(player, shipId));
            }

            return lst;
        }

        public override ConcurrentDictionary<string, DbShip> GetAll(string playerId)
		{
			throw new NotImplementedException();

        }
	}
}
