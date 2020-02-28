using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using SpaceDealerService;

namespace SpaceDealerUI
{
	public class GameProxy
	{
		public const string ServiceURL = "https://localhost:5001";

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

		public static async Task<Player> AddPlayer(string playerName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.AddPlayerAsync(new PlayerRequest { PlayerName = playerName });
			if (reply.Player != null)
			{
				return reply.Player;
			}
			return null;
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


		public static async Task<bool> ShipNameTaken(string playerName, string shipName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetShipsAsync(new ShipsRequest { PlayerName = playerName });
			if (reply.Ships != null)
			{
				foreach (var ship in reply.Ships)
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

		public static async Task<bool> GetAllShips(string playerName)
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply = await client.GetShipsAsync(new ShipsRequest { PlayerName = playerName });
			foreach (var ship in reply.Ships)
			{
				Console.WriteLine($"Ship: {ship.ShipName} [{ship.Cruise.Departure.PlanetName} -> {ship.Cruise.Destination.PlanetName}] Cargo Size: {ship.CargoSize}"); //Load: {ship.CargoLoad.CargoName}
			}
			return false;
		}

		public static async Task<List<Planet>> GetAllPlanets()
		{
			using var channel = GrpcChannel.ForAddress(ServiceURL);
			var client = new Game.GameClient(channel);
			var reply2 = await client.GetPlanetsAsync(new EmptyRequest());
			var lst = new List<Planet>();
			lst.AddRange(reply2.Planets);
			return lst;
		}
	}
}
