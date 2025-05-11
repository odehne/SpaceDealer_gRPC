using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using SpaceDealerService;

namespace SpaceDealerTerminal
{
	public class GameProxy
	{
		public const string ServiceURL = "http://localhost:58079";

		public static async Task<bool> PlayerNameTaken(string playerName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetPlayerAsync(new PlayerRequest { PlayerName = playerName });
			if (reply.Player != null)
			{
				return true;
			}
			return false;
		}

		public static async Task<Player> AddPlayer(string playerName, string picPath)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.AddPlayerAsync(new AddPlayerRequest { PlayerName = playerName, PicturePath =picPath  });
			if (reply.Player != null)
			{
				return reply.Player;
			}
			return null;
		}

		public static async Task<RepeatedField<UpdateInfo>> GetUpdates(string playerName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetUpdatesAsync(new PlayerRequest { PlayerName = playerName });
			return reply.UpdateInfos;
		}
		public static async Task<SaveGameReply> SaveGame(string playerName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.SaveGameAsync(new PlayerRequest { PlayerName = playerName });
			return reply;
		}



		public static async Task<BattleReply> BattleAttack(string playerName, string shipName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			return await client.BattleAttackAsync(new ShipRequest { PlayerName = playerName, ShipName = shipName });
		}
		public static async Task<BattleReply> BattleDefend(string playerName, string shipName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			return await client.BattleDefendAsync(new ShipRequest { PlayerName = playerName, ShipName = shipName });
		}

		public static async Task<Ship> AddShip(string playerName, string shipName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.AddShipAsync(new ShipRequest { PlayerName = playerName, ShipName = shipName });
			if (reply.Ship != null)
			{
				return reply.Ship;
			}
			return null;
		}

		public static async Task<RepeatedField<Ship>> GetAllShips(string playerName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetShipsAsync(new ShipsRequest { PlayerName = playerName });
			return reply.Ships;
		}

		public static async Task<Ship> GetShip(string playerName, string shipName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetShipAsync(new ShipRequest { PlayerName = playerName, ShipName = shipName });
			return reply.Ship;
		}

		public static async Task<bool> ShipNameTaken(string playerName, string shipName)
		{
			var ships = await GetAllShips(playerName);
			if (ships != null)
			{
				foreach (var ship in ships)
				{
					if (ship.ShipName.Equals(shipName, StringComparison.InvariantCultureIgnoreCase))
						return true;
				}
			}
			return false;
		}

		public static async Task<bool> StartCruise(string playerName, string shipName, string planetName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var cruiseStarted = await client.StartCruiseAsync(new CruiseRequest { DestinationPlanetName = planetName, PlayerName = playerName, ShipName = shipName });
			return cruiseStarted.OnItsWay;
		}

		public static async Task<bool> CruiseToLocation(string playerName, string shipName, Coordinates newPosition)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var cruiseStarted = await client.CruiseToLocationAsync(new CruiseToCoordinatesRequest { PlayerName = playerName, ShipName = shipName, Position = newPosition });
			return cruiseStarted.OnItsWay;
		}


		public static async Task<bool> ContinueTravel(string playerName, string shipName, string planetName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var cruiseStarted = await client.ContinueCruiseAsync(new CruiseRequest { DestinationPlanetName = planetName, PlayerName = playerName, ShipName = shipName });
			return cruiseStarted.OnItsWay;
		}

		public static async Task<Player> GetPlayer(string playerName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetPlayerAsync(new PlayerRequest { PlayerName = playerName });
			return reply.Player;
		}

		public static async Task<RepeatedField<Planet>> GetAllPlanets()
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply2 = await client.GetPlanetsAsync(new EmptyRequest());
			return reply2.Planets;
		}
	}
}
