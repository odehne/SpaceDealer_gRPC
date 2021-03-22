using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using SpaceDealer;
using SpaceDealerModels.Units;

namespace SpaceDealerService.Repos
{

	public class ShipsRepository : Repository<DbShip>
	{
		public ShipsRepository(SqlPersistor parent) : base(parent)
		{
		}


		public override void Save(DbShip ship)
		{
			Parent.Logger.Log($"Saving ship with id {ship.Id}.", TraceEventType.Information);

			if (ship.Cruise == null)
			{
				var earth = Program.Persistor.PlanetsRepo.GetItem("Erde", "");
				ship.Cruise = new DbJourney() { CurrentSector = earth.Sector, Destination = earth, Departure = earth, Parent = ship };
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
						Parent.Logger.Log($"Ship {ship.Id} saved.", TraceEventType.Information);
					}
					catch (Exception e)
					{
						Parent.Logger.Log($"Failed to save ship {e.Message}", TraceEventType.Error);
					}
				
				}
				foreach (var ft in ship.Features)
				{
					if (ft != null)
					{
						Parent.FeaturesRepo.Save(ft);
						Parent.FeaturesRepo.SaveShipFeature(ship.Id, ft.Id);
					}
				}
				Parent.Logger.Log($"Ship {ship.Name} saved.", TraceEventType.Information);
				
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to save ship with Id [{ship.Id}] {e.Message}", TraceEventType.Error);

			}
		}

		public override List<DbShip> GetAll()
		{
			Parent.Logger.Log($"Loading all ships.", TraceEventType.Information);

			var ids = new List<string>();
			var lst = new Ships();

			var query = "SELECT Id FROM Ships;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						ids.Add(reader.GetString(0));
					}
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get all ships  {e.Message}", TraceEventType.Error);
			}

			foreach (var shipId in ids)
			{
				lst.Add(GetItem(null, shipId));
			}

			return lst;
		}

		public override DbShip GetItem(string name, string id)
		{
			var parameter = new SQLiteParameter();
			Parent.Logger.Log($"Loading ship with {name}, {id}.", TraceEventType.Information);

			var query = "SELECT Id, PlayerId, Name, PicturePath, CargoSize, Hull, Shield, ShipState," +
						" Journey_Source, Journey_Destination, Current_SectorX," +
						" Current_SectorY, Current_SectorZ FROM Ships WHERE ";
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
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var ship = new DbShip();
						ship.Id = reader.GetString(0);
						ship.PlayerId = reader.GetString(1);
						ship.Name = reader.GetString(2);
						ship.PicturePath = reader.GetString(3);
						ship.CargoSize = reader.GetDouble(4);
						ship.Hull = reader.GetInt32(5);
						ship.Shields = reader.GetInt32(6);
						ship.State = (SpaceDealer.Enums.ShipState)reader.GetInt32(7);
						ship.Features.AddRange(Program.Persistor.FeaturesRepo.GetAll(ship.Id));

						var source = Program.Persistor.PlanetsRepo.GetItem("", reader.GetString(8));
						var destination = Program.Persistor.PlanetsRepo.GetItem("", reader.GetString(9));
						var currentSector = new DbCoordinates(reader.GetInt32(10), reader.GetInt32(11), reader.GetInt32(12));
						
						ship.CurrentPlanet = source;

						ship.Cruise = new DbJourney() { Departure = source, Destination = destination, CurrentSector = currentSector, Parent = ship };

						reader.Close();
				
						return ship;
					}
				}

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get ship with Id [{id}] {e.Message}", TraceEventType.Error);
			}
			return null;
		}

		public override string GetItemId(string name)
		{
			throw new NotImplementedException();
		}

		public override List<DbShip> GetAll(string id)
		{
			Parent.Logger.Log($"Loading all ships of player {id}.", TraceEventType.Information);
			var ids = new List<string>();
			var lst = new Ships();

			var query = "SELECT Id FROM Ships WHERE PlayerId = @playerId;";
			try
			{
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@playerId", id);
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
				Parent.Logger.Log($"Failed to get ships for player Id [{id}] {e.Message}", TraceEventType.Error);
			}

			foreach (var shipId in ids)
			{
				lst.Add(GetItem(null, shipId));
			}

			return lst;
		}
	}
}
