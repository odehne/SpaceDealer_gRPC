using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
			var ret = GamePersistor.SaveFile(JsonConvert.SerializeObject(player, Formatting.Indented, jset), player.Name + ".json");
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

		public override Task<PlayerReply> AddPlayer(PlayerRequest request, ServerCallContext context)
		{
			SpaceDealerModels.Units.Player player;
			var g = Program.TheGame;
			var json = GamePersistor.LoadFile(request.PlayerName + ".json");
			if(!string.IsNullOrEmpty(json))
			{
				player = JsonConvert.DeserializeObject<SpaceDealerModels.Units.Player>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
				player = AddSavedPlayer(player);
			}
			else
			{
				player = new SpaceDealerModels.Units.Player(request.PlayerName, g.Galaxy.GetPlanetByName("erde"), g.Galaxy);
			}

			g.FleetCommanders.AddPlayer(player);
		
			return Task.FromResult(new PlayerReply { Player = ProtoBufConverter.ConvertToPlayer(player) });
		}

		private SpaceDealerModels.Units.Player AddSavedPlayer(SpaceDealerModels.Units.Player player)
		{
			var galaxy = Program.TheGame.Galaxy;
			foreach (var p in player.Galaxy)
			{
				galaxy.AddPlanet(p);
			}
			
			var newPlayer = new SpaceDealerModels.Units.Player(player.Name, galaxy.GetPlanetByName(player.HomePlanet.Name), galaxy);
			newPlayer.Credits = player.Credits;
			newPlayer.CurrentPlanet = player.CurrentPlanet;
			
			foreach (var s in player.Fleet)
			{
				newPlayer.Fleet.AddShip(s);
			}
			return newPlayer;
		}

		public override Task<ShipReply> AddShip(ShipRequest request, ServerCallContext context)
		{
			var g = Program.TheGame;
			var p = g.FleetCommanders.GetPlayerByName(request.PlayerName);
			var s = new SpaceDealerModels.Units.Ship(request.ShipName, p.HomePlanet, Repository.GetFeatureSet(new string[] { "Signal Reichweite" })) { CargoSize = 30, Parent = p.Fleet };
			p.Fleet.AddShip(s);
			return Task.FromResult(new ShipReply { Ship = ProtoBufConverter.ConvertToShip(s) });
		}

		public override Task<PlayerReply> GetPlayer(PlayerRequest request, ServerCallContext context)
		{
			var g = Program.TheGame;
			var p = g.FleetCommanders.GetPlayerByName(request.PlayerName);
			if(p==null)
				return Task.FromResult(new PlayerReply { Player = null });
			return Task.FromResult(new PlayerReply { Player = ProtoBufConverter.ConvertToPlayer(p) });
		}

		public override Task<ShipReply> GetShip(ShipRequest request, ServerCallContext context)
		{
			var g = Program.TheGame;
			var p = g.FleetCommanders.GetPlayerByName(request.PlayerName);
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
