using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace SpaceDealerService
{
	public class GameSvc : Game.GameBase
	{
		public override Task<CruiseReply> StartCruise(CruiseRequest request, ServerCallContext context)
		{
			var player = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);
			if (player == null)
			{
				return Task.FromResult(new CruiseReply { OnItsWay = false });
			}
			var ship = player.Fleet.GetShipByName(request.ShipName);
			if (ship == null)
			{
				return Task.FromResult(new CruiseReply { OnItsWay = false });
			}
			var planet = Program.TheGame.Galaxy.GetPlanetByName(request.DestinationPlanetName);
			if (planet == null)
			{
				return Task.FromResult(new CruiseReply { OnItsWay = false });
			}
			ship.StartCruise(ship.Cruise.Depature, planet);
			return Task.FromResult(new CruiseReply { OnItsWay = true });
		}

	

		public override Task<ShipsReply> GetShips(ShipsRequest request, ServerCallContext context)
		{
			var ships = new ShipsReply();
			var player = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);

			foreach (var ship in player.Fleet)
			{
				ships.Ships.Add(ProtoBuConverter.ConvertToShip(ship));
			}
			return Task.FromResult(new ShipsReply(ships));
		}

		public override Task<PlanetsReply> GetPlanets(EmptyRequest request, ServerCallContext context)
		{
			var ps = new PlanetsReply();
			foreach (var planet in Program.TheGame.Galaxy)
			{
				ps.Planets.Add(ProtoBuConverter.ConvertToPlanet(planet));
			}
			return Task.FromResult(new PlanetsReply(ps));
		}

		public override Task<PlayerReply> AddPlayer(PlayerRequest request, ServerCallContext context)
		{
			var g = Program.TheGame;
			var player = new SpaceDealerModels.Units.Player(request.PlayerName, null, g.Galaxy.GetPlanetByName("erde"));
			g.FleetCommanders.Add(player);
			return Task.FromResult(new PlayerReply { Player = ProtoBuConverter.ConvertToPlayer(player) });
		}

		public override Task<ShipReply> AddShip(ShipRequest request, ServerCallContext context)
		{
			var g = Program.TheGame;
			var p = g.FleetCommanders.GetPlayerByName(request.PlayerName);
			var s = new SpaceDealerModels.Units.Ship(request.ShipName, null, p.HomePlanet) { CargoSize = 30, Parent = p.Fleet };
			p.Fleet.Add(s);
			return Task.FromResult(new ShipReply { Ship = ProtoBuConverter.ConvertToShip(s) });
		}

		public override Task<PlayerReply> GetPlayer(PlayerRequest request, ServerCallContext context)
		{
			var g = Program.TheGame;
			var p = g.FleetCommanders.GetPlayerByName(request.PlayerName);
			if(p==null)
				return Task.FromResult(new PlayerReply { Player = null });
			//var replyPlayer = new Player { Name = p.Name, Ships = p.Fleet }
			return Task.FromResult(new PlayerReply { Player = null });
		}

		public override Task<ShipReply> GetShip(ShipRequest request, ServerCallContext context)
		{
			var g = Program.TheGame;
			var p = g.FleetCommanders.GetPlayerByName(request.PlayerName);
			if (p == null)
				return Task.FromResult(new ShipReply { Ship = null });

			var ship = p.Fleet.GetShipByName(request.ShipName);

			return Task.FromResult(new ShipReply { Ship = ProtoBuConverter.ConvertToShip(ship) }); 
		}

		public override Task<PlayersReply> GetPlayers(EmptyRequest request, ServerCallContext context)
		{
			return base.GetPlayers(request, context);
		}
	}

	//var player1 = new Player("Oliver", null, earth);
	//player1.Fleet = new Ships(player1);
	//var ship = new Ship("Dark Star", null, earth) { MaxWeight = 3.0 };
	//var ship2 = new Ship("Dark Star 2", null, earth) { MaxWeight = 5.0 };
	//ship.Arrived += Ship_Arrived;
	//		ship2.Arrived += Ship_Arrived;
	//		player1.Fleet.AddShip(ship);
	//		player1.Fleet.AddShip(ship2);
	//		player1.CurrentPlanet = earth;
	//		player1.Fleet[0].StartCruise(earth, earth);
	//player1.Fleet[1].StartCruise(earth, earth);
	//FleetCommanders.Add(player1);
}
