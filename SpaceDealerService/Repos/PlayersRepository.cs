using SpaceDealerModels;
using SpaceDealerModels.Units;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace SpaceDealerService.Repos
{

	public class PlayersRepository : Repository<DbPlayer>
	{
		public PlayersRepository(SqlPersistor parent) : base(parent)
		{
		}

		public List<string> PeoplePicPaths(string rootPath)
		{
			var paths = new List<string>();

			if (!Directory.Exists(rootPath))
				throw new ArgumentException($"Path to people pics not found [{rootPath}].");

			foreach (var fil in Directory.GetFiles(rootPath))
			{
				paths.Add(fil);
			}
			return paths;
		}

		public override ConcurrentDictionary<string, DbPlayer> GetAll()
		{
			var lst = new Players();
			//Parent.Logger.Log($"Loading all players.");

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
						lst.TryAdd(id, GetItem("", id));
					}
					reader.Close();
					//Parent.CloseConnection(connection);
				}
				else
				{
					Parent.Logger.Warning($"No players loaded.");
				}
				reader.Close();
				//Parent.CloseConnection(connection);
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to read players {e.Message}");
			}

			return lst;
		}

		public override ConcurrentDictionary<string, DbPlayer> GetAll(string id)
		{
			throw new NotImplementedException();
		}

		public override DbPlayer GetItem(string name, string id)
		{
			//Parent.Logger.Log($"Loading player {name}, {id}.");
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
						var pHomePlanet = Program.TheGame.GetPlanetById(reader.GetString(5));
				
						//TODO: Add discovered planets 
						var discoveredPlanets = Program.TheGame.GetDiscoveredPlanetsByPlayerId(pId);
						
						player = new DbPlayer(pName, pHomePlanet, discoveredPlanets, Program.TheGame.Game)
						{
							Id = pId,
							Credits = pCredits,
							PlayerType = pPlayerType,
							PicturePath = pPicturePath
						};

                        foreach (var dp in discoveredPlanets.Values)
                        {
                            player.DiscoveredPlanets.AddPlanet(dp);
                        }

                        var playersShips = Program.TheGame.GetAllShipsByPlayer(player);
						foreach (var item in playersShips.Values)
						{
							player.Fleet.AddShip(item);
						}
					}
					reader.Close();
					//Parent.CloseConnection(connection);
				}
				else
				{
					Parent.Logger.Warning($"Player with Id not found [{id}].");
				}
				reader.Close();
				//Parent.CloseConnection(connection);
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to read player {e.Message}");
			}
			return player;
		}

		public override string GetItemId(string name)
		{
			//Parent.Logger.Log($"Loading player with name {name}.");
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
				Parent.Logger.Error($"Failed to read player Id {e.Message}");
				return null;
			}
		}

		public override void Save(DbPlayer player)
		{
			Parent.Logger.Verbose($"Saving player {player.Name}.");

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
						Parent.Logger.Verbose($"Player {player.Name} saved.");
					}
					catch (Exception e)
					{
						Parent.Logger.Error($"Failed to save player {e.Message}");
					}
				}
			}
			catch (System.Exception e)
			{
				Parent.Logger.Error($"Failed to add player Id {e.Message}");
			}
		}

	
	}
}
