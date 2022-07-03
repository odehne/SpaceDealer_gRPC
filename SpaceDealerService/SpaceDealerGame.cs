using SpaceDealerModels.Repositories;
using SpaceDealerModels.Units;
using SpaceDealerService;
using System;
using System.Collections.Generic;
using System.Text;

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

		public void AddPlanets(int amount = 100)
		{
			for (int i = 0; i < amount - 1; i++)
			{
				var randomPlanet = Repository.GetRandomPlanet(DbCoordinates.GerRandomCoordniates());
				randomPlanet.Name = $"{randomPlanet.Name}-{i}";
				randomPlanet.PicturePath = ".\\Planets\\image_part_" + Repository.GetRandomNumber(1, 36) + ".jpg";
				Galaxy.AddPlanet(randomPlanet);
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
				FleetCommanders.AddPlayer(p);
			}

	
			//AddPlanets(100);
			//AddFleetCommanders(100);
		}


		private void Ship_Arrived(string message, Coordinates newPosition, DbShip ship)
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
