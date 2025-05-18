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
			var reply = await client.GetPlayerByNameAsync(new PlayerByNameRequest { PlayerName = playerName });
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
			var reply = await client.AddPlayerAsync(new AddPlayerRequest { PlayerName = playerName, PicturePath = picPath  });
			if (reply.Player != null)
			{
				return reply.Player;
			}
			return null;
		}

		public static async Task<RepeatedField<UpdateInfo>> GetUpdates(string playerId)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetUpdatesAsync(new PlayerRequest { PlayerId = playerId });
			return reply.UpdateInfos;
		}
		public static async Task<SaveGameReply> SaveGame(string playerId)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.SaveGameAsync(new PlayerRequest { PlayerId = playerId });
			return reply;
		}



		public static async Task<BattleReply> BattleAttack(string playerId, string shipId)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			return await client.BattleAttackAsync(new ShipRequest { PlayerId = playerId, ShipId = shipId });
		}
		public static async Task<BattleReply> BattleDefend(string playerId, string shipId)
        {
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			return await client.BattleDefendAsync(new ShipRequest { PlayerId = playerId, ShipId = shipId });
		}

		public static async Task<Ship> AddShip(string playerId, string shipName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.AddShipAsync(new AddShipRequest { PlayerId = playerId, ShipName = shipName });
			if (reply.Ship != null)
			{
				return reply.Ship;
			}
			return null;
		}

		public static async Task<RepeatedField<Ship>> GetAllShips(string playerId)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetShipsAsync(new ShipsRequest { PlayerId = playerId });
			return reply.Ships;
		}

		public static async Task<Ship> GetShip(string playerId, string shipId)
        {
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetShipAsync(new ShipRequest {PlayerId = playerId, ShipId = shipId });
			return reply.Ship;
		}

		public static async Task<bool> ShipNameTaken(string playerId, string shipName)
		{
			var ships = await GetAllShips(playerId);
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

		public static async Task<bool> StartCruise(string playerId, string shipId, string planetName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var cruiseStarted = await client.StartCruiseAsync(new CruiseRequest { DestinationPlanetName = planetName, PlayerId = playerId, ShipId = shipId });
			return cruiseStarted.OnItsWay;
		}

		public static async Task<bool> CruiseToLocation(string playerId, string shipId, Coordinates newPosition)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var cruiseStarted = await client.CruiseToLocationAsync(new CruiseToCoordinatesRequest { PlayerId = playerId , ShipId = shipId, Position = newPosition });
			return cruiseStarted.OnItsWay;
		}


		public static async Task<bool> ContinueTravel(string playerId, string shipId, string planetName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var cruiseStarted = await client.ContinueCruiseAsync(new CruiseRequest { DestinationPlanetName = planetName, PlayerId = playerId, ShipId = shipId });
			return cruiseStarted.OnItsWay;
		}

		public static async Task<Player> GetPlayer(string playerId)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetPlayerAsync(new PlayerRequest { PlayerId = playerId });
			return reply.Player;
		}

        public static async Task<Player> GetPlayerByName(string playerName)
        {
            using var channel = GrpcChannel.ForAddress(ServiceURL);
            var client = new Game.GameClient(channel);
            var reply = await client.GetPlayerByNameAsync(new PlayerByNameRequest { PlayerName = playerName});
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
