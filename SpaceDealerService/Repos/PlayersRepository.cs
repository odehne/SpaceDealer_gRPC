using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{

	public class PlayersRepository
	{


		public SqlPersistor Parent { get; set; }

		public PlayersRepository(SqlPersistor parent)
		{
			Parent = parent;
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
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
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
					Parent.Logger.Log($"Player with name not found [{name}].", TraceEventType.Warning);
				}
				reader.Close();
				Parent.CloseConnection(connection);
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to read player {e.Message}", TraceEventType.Error);
			}
			return null;
		}


		public DbPlayer GetPlayer(string name, string id)
		{
			var parameter = new SQLiteParameter();

			var query = "SELECT id, Name, Credits, PlayerType, PicturePath FROM Players WHERE ";
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
					reader.Close();
					Parent.CloseConnection(connection);
				}
				else
				{
					Parent.Logger.Log($"Player with Id not found [{id}].", TraceEventType.Warning);
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to read player {e.Message}", TraceEventType.Error);
			}
			return null;
		}

		public string GetPlayerId(string name)
		{
			var query = "SELECT Id FROM Players WHERE Name = @name;";
			try
			{
				using var connection = new SQLiteConnection("Data Source=" + Parent.DbPath);
				Parent.OpenConnection(connection);
				using var command = new SQLiteCommand(connection);
				command.CommandText = query;
				command.Parameters.AddWithValue("@name", name);
				var ret = (string)command.ExecuteScalar();
				Parent.CloseConnection(connection);
				return ret;
			}
			catch (System.Exception e)
			{
				Parent.Logger.Log($"Failed to read player Id {e.Message}", TraceEventType.Error);
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
				using (var connection = new SQLiteConnection("Data Source=" + Parent.DbPath))
				{
					Parent.OpenConnection(connection);
					using (var command = new SQLiteCommand(connection))
					{
						//Erstellen der Tabelle, sofern diese noch nicht existiert.
					    command.CommandText = $"INSERT OR REPLACE INTO Players (id, Name, Credits, PlayerType, PicturePath) VALUES (@id, @name, @credits, @playerType, @picturePath);";
						command.Parameters.AddWithValue("@id", player.Id);
						command.Parameters.AddWithValue("@name", player.Name);
						command.Parameters.AddWithValue("@credits", player.Credits);
						command.Parameters.AddWithValue("@playerType", player.PlayerType);
						command.Parameters.AddWithValue("@picturePath", player.PicturePath);
						try
						{
							command.ExecuteNonQuery();
							Parent.Logger.Log($"Player {player.Name} saved.", TraceEventType.Information);
						}
						catch (Exception e)
						{
							Parent.Logger.Log($"Failed to save feature {e.Message}", TraceEventType.Error);
						}
						finally
						{
							Parent.CloseConnection(connection);
						}
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
