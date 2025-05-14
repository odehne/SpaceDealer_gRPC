using Google.Protobuf.WellKnownTypes;
using SpaceDealerModels;
using SpaceDealerModels.Repositories;
using SpaceDealerModels.Units;
using SpaceDealerService;
using SpaceDealerService.Repos;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace SpaceDealer
{

	public class SpaceDealerGame : ISpaceDealerGame
	{
		public Planets Galaxy { get; set; }
		public Sectors ActiveSectors { get; set; }
		public Players FleetCommanders { get; set; }
		public ILogger Logger { get; set; }
        public SpaceDealerGame(ILogger logger)
        {
            Logger = logger;
            Galaxy = new Planets();
            FleetCommanders = new Players();
            Repository.Init();
        }

        public void AddPlanets(int amount = 1000)
		{
			for (int i = 0; i < amount; i++)
			{
				var randomPlanet = Repository.GenerateRandomPlanet(DbCoordinates.GenerateRandomCoordniates());
				randomPlanet.Name = $"{randomPlanet.Name}-{i}";
				randomPlanet.PicturePath = ".\\Planets\\image_part_" + Repository.GetRandomNumber(1, 36) + ".jpg";
				Galaxy.AddPlanet(randomPlanet);
			}
		}

        public void AddFleetCommanders(int amount = 100)
        {
           if(FleetCommanders == null)
                FleetCommanders = new Players();

            for(int i = 0; i < amount; i++)
            {
                var planet = Galaxy.GetRandomPlanet();
                var discoveredPlanets = Galaxy.GetRandomPlanets(5);
                var fcn = Repository.GetRandomFleetCommanderName();
                var player = new DbPlayer(fcn, planet, discoveredPlanets, Galaxy, Program.TheGame.ActiveSectors);
				var shipName = Repository.GetRandomShipName();
                var ship = new DbShip(shipName, player.HomePlanet, Repository.GetFeatureSet(new string[] { "SignalRange+1" }), player.Fleet)
                {
                    CargoSize = 30,
                    Parent = player.Fleet,
                    PlayerId = player.Id,
                    PicturePath = ".\\Spaceships\\MediumFrighter.jpg"
                };
                player.PlayerType = Enums.PlayerTypes.NPC;
                player.Fleet.AddShip(ship);
                FleetCommanders.Add(player);
            }

		}


        public void Init()
		{
			ActiveSectors = Program.Persistor.SectorRepo.GetAll();

            foreach (var p in Program.Persistor.PlanetsRepo.GetAll())
			{
                Galaxy.AddPlanet(p);
			}

			foreach (var p in Program.Persistor.PlayersRepo.GetAll())
			{
				p.Galaxy = Galaxy;
                FleetCommanders.AddPlayer(p);
            }

            Logger.Log($"Planets loaded: " + Galaxy.Count, TraceEventType.Information);
            Logger.Log($"Fleetcommanders loaded: " + FleetCommanders.Count, TraceEventType.Information);

            AddPlanets(10);
            AddFleetCommanders(5);

        }


        private void Ship_Arrived(string message, SpaceDealerModels.Units.Coordinates newPosition, DbShip ship)
		{
			ship.Cruise.Departure = ship.Cruise.Destination;
		}

		public Planets ScanPlanetsInNearbySectors(DbShip ship, double range = 1)
		{
			throw new NotImplementedException();
			// xxx
			// x+x
			// xxx
		}

		public Ships ScanShipsInNearbySectors(DbShip ship, double range = 1)
		{
			throw new NotImplementedException();
		}

	}
}
