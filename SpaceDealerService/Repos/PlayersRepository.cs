﻿using SpaceDealer;
using SpaceDealerModels.Repositories;
using SpaceDealerModels.Units;
using System;
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

		//public void AddFleetCommanders(Planets theGalaxy, int amount = 100)
		//{
		//	var lst = new Players();
		//	var fleetCommanderNames = new string[]
		//		{
		//			"Stery Gonzal",
		//			"Raymy Reson",
		//			"Jeffry Watson",
		//			"Johny Whelley",
		//			"Kenne Barner",
		//			"Danio Parking",
		//			"Raymy Ander",
		//			"Jery Clery",
		//			"Jamy Ganes",
		//			"Phardy Hillee",
		//			"Justeph Hughy",
		//			"Tine Coopet",
		//			"Rege Belley",
		//			"Wardy Rodra",
		//			"Johnne Pera",
		//			"Aadan Jenkell",
		//			"Randy Hernes",
		//			"Justev Finels",
		//			"Peteph Sonett",
		//			"Grence Bennels"
		//		};

		//	foreach (var fcn in fleetCommanderNames)
		//	{
		//		var planet = theGalaxy.GetRandomPlanet();
		//		var player = new DbPlayer(fcn, planet, theGalaxy, theGalaxy);
		//		var ship = new DbShip($"{player}s Raumschiff", player.HomePlanet, Repository.GetFeatureSet(new string[] { "SignalRange+1" }))
		//		{
		//			CargoSize = 30,
		//			Parent = player.Fleet,
		//			PlayerId = player.Id,
		//			PicturePath = ".\\Spaceships\\MediumFrighter.jpg"
		//		};
		//		player.PlayerType = SpaceDealer.Enums.PlayerTypes.NPC;
		//		player.Fleet.AddShip(ship);
		//		lst.Add(player);
		//	}
		//}


		public override List<DbPlayer> GetAll()
		{
			var lst = new Players();
			//Parent.Logger.Log($"Loading all players.", TraceEventType.Information);

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
			//Parent.Logger.Log($"Loading player {name}, {id}.", TraceEventType.Information);
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
				
						//TODO: Add discovered planets 
						var discoveredPlanets = Program.Persistor.DiscoveredPlanetsRepo.GetDiscoveredPlanets(pId);
						foreach (var dp in discoveredPlanets)
						{
							player.DiscoveredPlanets.AddPlanet(dp);
						}

						player = new DbPlayer(pName, pHomePlanet, discoveredPlanets, Program.TheGame.Galaxy, Program.TheGame.ActiveSectors)
						{
							Id = pId,
							Credits = pCredits,
							PlayerType = pPlayerType,
							PicturePath = pPicturePath
						};
						
						var playersShips = Program.Persistor.ShipsRepo.GetAll(player.Id);
						foreach (var item in playersShips)
						{
							player.Fleet.AddShip(item);
						}

					

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
			//Parent.Logger.Log($"Loading player with name {name}.", TraceEventType.Information);
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
