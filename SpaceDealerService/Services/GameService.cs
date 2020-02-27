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
		public override Task<ShipsReply> GetShips(ShipsRequest request, ServerCallContext context)
		{
			var ships = new ShipsReply();
			var player = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);

			foreach (var ship in player.Fleet)
			{
				var protoShip = new Ship()
				{
					ShipName = ship.Name,
					Cruise = new Journey()
					{
						CurrentDistance = ship.Cruise.CurrentDistanceToDestination,
						Departure = new Coordinates()
						{
							X = ship.Cruise.Depature.Sector.X,
							Y = ship.Cruise.Depature.Sector.Y,
							Z = ship.Cruise.Depature.Sector.Z
						},
						Destination = new Coordinates
						{
							X = ship.Cruise.Destination.Sector.X,
							Y = ship.Cruise.Destination.Sector.Y,
							Z = ship.Cruise.Destination.Sector.Z
						},
					},
					CargoLoad = new Load
					{
						CargoName = "Rinder",
						Size = 30.0
					},
					CargoSize = 35.0
				};
				ships.Ships.Add(protoShip);
			}
			return Task.FromResult(new ShipsReply(ships));
		}

		public override Task<PlanetsReply> GetPlanets(PlanetsRequest request, ServerCallContext context)
		{
			var ps = new PlanetsReply();
			var earth = new Planet { PlanetName = "earth", Sector = new Coordinates { X = 1.0, Y = 1.0, Z = 1.0 } };
			var tatooine = new Planet { PlanetName = "tatooine", Sector = new Coordinates { X = 14, Y = 3, Z = 7} };

			ps.Planets.Add(earth);
			ps.Planets.Add(tatooine);

			return Task.FromResult(new PlanetsReply(ps));
		}
	}
}
