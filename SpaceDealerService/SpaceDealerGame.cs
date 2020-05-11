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
		public Players FleetCommanders { get; set; }
		public ILogger Logger { get; set; }
		public SpaceDealerGame(ILogger logger)
		{
			Logger = logger;
			Galaxy = new Planets();
			FleetCommanders = new Players();
			Repository.Init();
		}

		public void Init()
		{
			Repository.LoadFeatures(Program.Persistor.FeaturesRepo.GetFeatures());

			Program.Persistor.FeaturesRepo.SaveAll(Repository.Features);

			var earth = Program.Persistor.PlanetsRepo.GetPlanet("Erde", "");
			if(earth==null)
			{
				earth = new DbPlanet("Erde");
				earth.Market = new DbMarket("Berlin", earth);
				earth.Sector = new DbCoordinates(0, 0, 0);
				earth.Industry = Repository.GetIndustryByName("Landwirtschaft");
				earth.PicturePath = ".\\Planets\\earth.jpg";
			}

			var moon = Program.Persistor.PlanetsRepo.GetPlanet("Erden-Mond", "");
			if (moon == null)
			{
				moon = new DbPlanet("Erden-Mond");
				moon.Properties = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Beschreibung", "Kreist um den Planeten Erde. Besitzt keine eigene Athmossphäre.") };
				moon.Market = new DbMarket("Mondbasis Aplha 1", moon);
				moon.Sector = new DbCoordinates(1, 0, 0);
				moon.PicturePath = ".\\Planets\\moon.jpg";
				moon.Industry = Repository.GetIndustryByName("Raumschiff Fabrik");
				moon.Industry.AddGeneratedProduct(Repository.GetProductByName("Kleines Raumschiff Kapazität (30t)"));
				moon.Industry.AddGeneratedProduct(Repository.GetProductByName("Mittleres Raumschiff Kapazität (60t)"));
				moon.Industry.AddGeneratedProduct(Repository.GetProductByName("Kreuzer (100t) +Bewaffnung"));
				moon.Industry.AddGeneratedProduct(Repository.GetProductByName("Sensor-Einheit"));
				moon.Industry.AddGeneratedProduct(Repository.GetProductByName("Board-Kanone"));
				moon.Industry.AddNeededProduct(Repository.GetProductByName("Board-Kanone"));
			}

			var tatooine = Program.Persistor.PlanetsRepo.GetPlanet("Tatooine", "");
			if (tatooine == null)
			{
				tatooine = new DbPlanet("Tatooine");
				tatooine.PicturePath = ".\\Planets\\tatooine.jpg";
				tatooine.Market = new DbMarket("Moseisley", earth);
				tatooine.Sector = new DbCoordinates(10, 7, 1);
				tatooine.Industry = Repository.GetIndustryByName("Weltraumschrott Sammler");
			}

			Galaxy.AddPlanet(earth);
			Galaxy.AddPlanet(moon);
			Galaxy.AddPlanet(tatooine);

			
		}

		
		private void Ship_Arrived(string message, Coordinates newPosition, DbShip ship)
		{
			//Logger.Log($"Das Schiff ist angekommen: {ship.Cruise.Destination.ToString()}", TraceEventType.Information);
			ship.Cruise.Departure = ship.Cruise.Destination;
		//	ShowShipMenu(ship);
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
