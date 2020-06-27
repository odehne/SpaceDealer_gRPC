using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class PlayersRepository : Repository<DbPlayer>
	{
		public PlayersRepository(SqlPersistor parent) : base(parent)
		{
		}

		public override List<DbPlayer> GetAll()
		{
			var lst = new Players();
			Parent.Logger.Log($"Loading all players.", TraceEventType.Information);

			var query = "SELECT id FROM Players;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var id = reader.GetString(0);
						lst.Add(GetItem("", id));
					}
					reader.Close();
					//Parent.CloseConnection(connection);
				}
				else
				{
					Parent.Logger.Log($"No players loaded.", TraceEventType.Warning);
				}
				reader.Close();
				//Parent.CloseConnection(connection);
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to read players {e.Message}", TraceEventType.Error);
			}

			return lst;
		}

		public override List<DbPlayer> GetAll(string id)
		{
			throw new NotImplementedException();
		}

		public override DbPlayer GetItem(string name, string id)
		{
			Parent.Logger.Log($"Loading player {name}, {id}.", TraceEventType.Information);
			var parameter = new SQLiteParameter();
			DbPlayer player = null;

			var query = "SELECT id, Name, Credits, PlayerType, PicturePath, HomePlanetId FROM Players WHERE ";
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
				
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.Add(parameter);
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var pId = reader.GetString(0);
						var pName = reader.GetString(1);
						var pCredits = reader.GetDouble(2);
						var pPlayerType = (SpaceDealer.Enums.PlayerTypes)reader.GetInt32(3);
						var pPicturePath = reader.GetString(4);
						var pHomePlanet = Program.Persistor.PlanetsRepo.GetItem("", reader.GetString(5));
						var galaxy = new Planets();
						galaxy.AddRange(Parent.PlanetsRepo.GetAll());

						player = new DbPlayer(pName, pHomePlanet, galaxy)
						{
							Id = pId,
							Credits = pCredits,
							PlayerType = pPlayerType,
							PicturePath = pPicturePath
						};
						
						player.Fleet.AddRange(Program.Persistor.ShipsRepo.GetAll(player.Id));
					}
					reader.Close();
					//Parent.CloseConnection(connection);
				}
				else
				{
					Parent.Logger.Log($"Player with Id not found [{id}].", TraceEventType.Warning);
				}
				reader.Close();
				//Parent.CloseConnection(connection);
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to read player {e.Message}", TraceEventType.Error);
			}
			return player;
		}

		public override string GetItemId(string name)
		{
			Parent.Logger.Log($"Loading player with name {name}.", TraceEventType.Information);
			var query = "SELECT Id FROM Players WHERE Name = @name;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				
				using var command = new SQLiteCommand(Parent.Connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@name", name);
				var ret = (string)command.ExecuteScalar();
				//Parent.CloseConnection(connection);
				return ret;
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to read player Id {e.Message}", TraceEventType.Error);
				return null;
			}
		}

		public override void Save(DbPlayer player)
		{
			Parent.Logger.Log($"Saving player {player.Name}.", TraceEventType.Information);

			var id = GetItemId(player.Name);
			if (id == null)
				id = player.Id;

			try
			{
				using (var command = new SQLiteCommand(Parent.Connection))
				{
					//Erstellen der Tabelle, sofern diese noch nicht existiert.
				    command.CommandText = $"INSERT OR REPLACE INTO Players (id, Name, Credits, PlayerType, PicturePath, HomePlanetId) VALUES (@id, @name, @credits, @playerType, @picturePath, @homePlanetId);";
					command.Parameters.AddWithValue("@id", player.Id);
					command.Parameters.AddWithValue("@name", player.Name);
					command.Parameters.AddWithValue("@credits", player.Credits);
					command.Parameters.AddWithValue("@playerType", player.PlayerType);
					command.Parameters.AddWithValue("@picturePath", player.PicturePath);
					command.Parameters.AddWithValue("@homePlanetId", player.HomePlanet.Id);
					try
					{
						command.ExecuteNonQuery();
						Parent.Logger.Log($"Player {player.Name} saved.", TraceEventType.Information);
					}
					catch (Exception e)
					{
						Parent.Logger.Log($"Failed to save feature {e.Message}", TraceEventType.Error);
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to add player Id {e.Message}", TraceEventType.Error);
			}
		}

	
	}
}
