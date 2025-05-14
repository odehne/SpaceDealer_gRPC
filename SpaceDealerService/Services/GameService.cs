using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SpaceDealerModels;
using SpaceDealerModels.Repositories;

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

			ship.StartCruise(ship.CurrentPlanet, planet);
			return Task.FromResult(new CruiseReply { OnItsWay = true });
		}

		public override Task<CruiseReply> CruiseToLocation(CruiseToCoordinatesRequest request, ServerCallContext context)
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
		 
			ship.StartCruiseToLocation(ship.Cruise.CurrentSector, new SpaceDealerModels.Units.DbCoordinates(request.Position.X, request.Position.Y, request.Position.Z));
			return Task.FromResult(new CruiseReply { OnItsWay = true });
		}

		public override Task<CruiseReply> ContinueCruise(CruiseRequest request, ServerCallContext context)
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

			var result = ship.Cruise.ContinueTravel();
			return Task.FromResult(new CruiseReply { OnItsWay = result });
		}

		public override Task<SaveGameReply> SaveGame(PlayerRequest request, ServerCallContext context)
		{
			var player = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);
			var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
			var ret = Program.Persistor.SavePlayers(Program.TheGame.FleetCommanders);
			return Task.FromResult(new SaveGameReply { GameSaved = ret });
		}

		public override Task<UpdateReply> GetUpdates(PlayerRequest request, ServerCallContext context)
		{
			var reply = new UpdateReply();
			var player = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);

			if (player.UpdateQueue != null)
			{
				while (player.UpdateQueue.Count > 0)
				{
					var info = (SpaceDealerModels.Units.UpdateInfo)player.UpdateQueue.Dequeue();
					reply.UpdateInfos.Add(ProtoBufConverter.ConvertToUpdateInfo(info));
				}
			}

			return Task.FromResult(new UpdateReply(reply));
		}

		public override Task<BattleReply> BattleAttack(ShipRequest request, ServerCallContext context)
		{
			var player = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);
			var ship = player.Fleet.GetShipByName(request.ShipName);
			var result = ship.Attack();
			if (result.Defeaded == true)
				player.Credits += result.Treasure;
			var reply = ProtoBufConverter.ConvertToBattleReply(result);
			return Task.FromResult(reply);
		}

		public override Task<BattleReply> BattleDefend(ShipRequest request, ServerCallContext context)
		{
			var player = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);
			var ship = player.Fleet.GetShipByName(request.ShipName);
			var result = ship.Defend();
			var reply = ProtoBufConverter.ConvertToBattleReply(result);
			return Task.FromResult(reply);
		}

		public override Task<ShipsReply> GetShips(ShipsRequest request, ServerCallContext context)
		{
			var ships = new ShipsReply();
			var player = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);
			if (player == null)
				return Task.FromResult(new ShipsReply(ships));

			foreach (var ship in player.Fleet)
			{
				ships.Ships.Add(ProtoBufConverter.ConvertToShip(ship));
			}
			return Task.FromResult(new ShipsReply(ships));
		}

		public override Task<PlanetsReply> GetPlanets(EmptyRequest request, ServerCallContext context)
		{
			var ps = new PlanetsReply();
			foreach (var planet in Program.TheGame.Galaxy)
			{
				ps.Planets.Add(ProtoBufConverter.ConvertToPlanet(planet));
			}
			return Task.FromResult(new PlanetsReply(ps));
		}

		public override Task<PlayerReply> AddPlayer(AddPlayerRequest request, ServerCallContext context)
		{
			var player = Program.Persistor.PlayersRepo.GetItem(request.PlayerName, null);

			if (player == null)
			{
<<<<<<< HEAD
				var homePlanet = Program.TheGame.Galaxy.GetRandomPlanet();
                player = new SpaceDealerModels.Units.DbPlayer(request.PlayerName, homePlanet, Program.TheGame.Galaxy.GetFirstFivePlanets(), Program.TheGame.Galaxy, Program.TheGame.ActiveSectors);
=======
				player = new SpaceDealerModels.Units.DbPlayer(request.PlayerName, Program.TheGame.Galaxy.GetPlanetByName("erde"), Program.TheGame.Galaxy.GetFirstFivePlanets(), Program.TheGame.Galaxy, Program.TheGame.ActiveSectors);
>>>>>>> dbd57fff9c962da63d94361fc58baa3c51357c6a
				player.PicturePath = request.PicturePath;
            }
            else
            {
				player.Galaxy = Program.TheGame.Galaxy;
				player.ActiveSectors = Program.TheGame.ActiveSectors;
			}

            Program.TheGame.FleetCommanders.AddPlayer(player);

			Program.Persistor.SavePlayers(Program.TheGame.FleetCommanders);
			return Task.FromResult(new PlayerReply { Player = ProtoBufConverter.ConvertToPlayer(player) });

		}

		public override Task<ShipReply> AddShip(ShipRequest request, ServerCallContext context)
		{
			var p = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);
<<<<<<< HEAD
			var s = new SpaceDealerModels.Units.DbShip(request.ShipName, 
														p.HomePlanet, 
														Repository.GetFeatureSet(new string[] { "SignalRange+1" }), p.Fleet) { CargoSize = 30 };
=======
			var s = new SpaceDealerModels.Units.DbShip(request.ShipName, p.HomePlanet, Repository.GetFeatureSet(new string[] { "SignalRange+1" })) { CargoSize = 30, Parent = p.Fleet };
>>>>>>> dbd57fff9c962da63d94361fc58baa3c51357c6a
			s.PlayerId = p.Id;
			s.PicturePath = ".\\Spaceships\\MediumFrighter.jpg";
			p.Fleet.AddShip(s);
			return Task.FromResult(new ShipReply { Ship = ProtoBufConverter.ConvertToShip(s) });
		}

		public override Task<PlayerReply> GetPlayer(PlayerRequest request, ServerCallContext context)
		{
			var p = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);
			if(p==null)
				return Task.FromResult(new PlayerReply { Player = null });
			return Task.FromResult(new PlayerReply { Player = ProtoBufConverter.ConvertToPlayer(p) });
		}

		public override Task<ShipReply> GetShip(ShipRequest request, ServerCallContext context)
		{
			var p = Program.TheGame.FleetCommanders.GetPlayerByName(request.PlayerName);
			if (p == null)
				return Task.FromResult(new ShipReply { Ship = null });

			var ship = p.Fleet.GetShipByName(request.ShipName);

			return Task.FromResult(new ShipReply { Ship = ProtoBufConverter.ConvertToShip(ship) }); 
		}

		public override Task<PlayersReply> GetPlayers(EmptyRequest request, ServerCallContext context)
		{
			return base.GetPlayers(request, context);
		}
	}

}
