using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using SpaceDealer;
using SpaceDealerModels.Units;

namespace SpaceDealerService.Repos
{

	public class ShipsRepository
	{
		public SqlPersistor Parent { get; set; }

		public ShipsRepository(SqlPersistor parent)
		{
			Parent = parent;
		}

		
		public List<DbShip> GetShips(string playerId)
		{
			var ids = new List<string>();
			var lst = new List<DbShip>();

			var query = "SELECT Id FROM Ships WHERE PlayerId = @playerId;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@playerId", playerId);
				using var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						ids.Add(reader.GetString(0));
					}
				}
				reader.Close();
				Parent.CloseConnection(connection);

			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to get ships for player Id [{playerId}] {e.Message}", TraceEventType.Error);
			}

			foreach (var shipId in ids)
			{
				lst.Add(GetShip(null, shipId));
			}

			return lst;
		}

		public DbShip GetShip(string name, string id)
		{
			var parameter = new SQLiteParameter();

			var query = "SELECT Id, PlayerId, Name, PicturePath, CargoSize, Hull, Shield, ShipState, Name FROM Ships WHERE ";
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
						reader.Close();
						Parent.CloseConnection(connection);

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

		public void SaveShip(DbShip ship)
		{
			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + Parent.DbPath))
				{
					Parent.OpenConnection(connection);
					using (var command = new SQLiteCommand(connection))
					{
						command.CommandText = $"INSERT OR REPLACE INTO Ships (Id, PlayerId, Name, PicturePath, CargoSize, Hull, Shield, ShipState) " +
							$"VALUES (@id, @playerId, @name, @picturePath, @cargoSize, @hull, @shield, @shipState);";
						command.Parameters.AddWithValue("@id", ship.Id);
						command.Parameters.AddWithValue("@name", ship.Name);
						command.Parameters.AddWithValue("@playerId", ship.PlayerId);
						command.Parameters.AddWithValue("@picturePath", ship.PicturePath);
						command.Parameters.AddWithValue("@cargoSize", ship.CargoSize);
						command.Parameters.AddWithValue("@hull", ship.Hull);
						command.Parameters.AddWithValue("@shield", ship.Shields);
						command.Parameters.AddWithValue("@shipState", ship.State);
						try
						{
							command.ExecuteNonQuery();
							Parent.Logger.Log($"Ship {ship.Id} saved.", TraceEventType.Information);
						}
						catch (Exception e)
						{
							Parent.Logger.Log($"Failed to save ship {e.Message}", TraceEventType.Error);
						}
						finally
						{
							Parent.CloseConnection(connection);
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
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to save ship with Id [{ship.Id}] {e.Message}", TraceEventType.Error);

			}
		}
	}
}
