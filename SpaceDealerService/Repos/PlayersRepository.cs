using SpaceDealer;
using SpaceDealerModels.Units;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class PlayersRepository
	{
		public ILogger Logger { get; set; }

		public string DbPath { get; set; }

		public PlayersRepository(ILogger logger, string dbPath)
		{
			DbPath = dbPath;
			Logger = logger;
		}

		public List<Player> GetPlayers()
		{
			var lst = new List<Player>();

			return lst;
		}

		public DbPlayer GetPlayerByName(string name)
		{
			var query = "SELECT id, Name, Credits, PlayerType, PicturePath FROM Players WHERE Name = @name;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@name", name);
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						return new DbPlayer
						{
							Id = reader.GetString(0),
							Name = reader.GetString(1),
							Credits = reader.GetDouble(2),
							PlayerType = (SpaceDealer.Enums.PlayerTypes)reader.GetInt32(3),
							PicturePath = reader.GetString(4)
						};
					}
				}
				else
				{
					Logger.Log($"Player with name not found [{name}].", TraceEventType.Warning);
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to read player {e.Message}", TraceEventType.Error);
			}
			return null;
		}


		public DbPlayer GetPlayer(string id)
		{
			var query = "SELECT id, Name, Credits, PlayerType, PicturePath FROM Players WHERE Id = @id;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@id", id);
				var reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						return new DbPlayer
						{
							Id = reader.GetString(0),
							Name = reader.GetString(1),
							Credits = reader.GetDouble(2),
							PlayerType = (SpaceDealer.Enums.PlayerTypes)reader.GetInt32(3),
							PicturePath = reader.GetString(4)
						};
					}
				}
				else
				{
					Logger.Log($"Player with Id not found [{id}].", TraceEventType.Warning);
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to read player {e.Message}", TraceEventType.Error);
			}
			return null;
		}

		public string GetPlayerId(string name)
		{
			var query = "SELECT Id FROM Players WHERE Name = @name;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + DbPath);
				connection.Open();
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@name", name);
				return (string)command.ExecuteScalar();

			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to read player Id {e.Message}", TraceEventType.Error);
				return null;
			}
		}

		public void SavePlayer(DbPlayer player)
		{

			var id = GetPlayerId(player.Name);
			if (id == null)
				id = player.Id;

			try
			{
				using (var connection = new SQLiteConnection("Data Source=" + DbPath))
				{
					connection.Open();
					using (var command = new SQLiteCommand(connection))
					{
						//Erstellen der Tabelle, sofern diese noch nicht existiert.
					    command.CommandText = $"INSERT OR REPLACE INTO Players (id, Name, Credits, PlayerType, PicturePath) VALUES (@id, @name, @credits, @playerType, @picturePath);";
						command.Parameters.AddWithValue("@id", player.Id);
						command.Parameters.AddWithValue("@name", player.Name);
						command.Parameters.AddWithValue("@credits", player.Credits);
						command.Parameters.AddWithValue("@playerType", player.PlayerType);
						command.Parameters.AddWithValue("@picturePath", player.PicturePath);
						command.ExecuteNonQuery();
						Logger.Log($"Player {player.Name} saved.", TraceEventType.Information);
					}
				}
			}
			catch (System.Exception e)
			{
				Logger.Log($"Failed to add player Id {e.Message}", TraceEventType.Error);
			}
		}
	}
}
