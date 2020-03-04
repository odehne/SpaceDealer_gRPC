using SpaceDealerModels.Repositories;
using SpaceDealerModels.Units;
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
			var earth = new Planet("Erde");
			earth.Market = new Market("Berlin", earth);
			earth.Industries = new Industries(earth);
			earth.Sector = new Coordinates(0, 0, 0);
			earth.Industries.Add(Repository.IndustryLibrary.GetIndustryByName("Landwirtschaft"));

			var moon = new Planet("Erden-Mond");
				moon.Properties = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Beschreibung", "Kreist um den Planeten Erde. Besitzt keine eigene Athmossphäre.") };
				moon.Market = new Market("Mondbasis Aplha 1", moon);
				moon.Industries = new Industries(moon);
				moon.Sector = new Coordinates(1, 0, 0);

			var moonFactory = moon.Industries.AddIndustry("Raumschiff Fabrik");
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Kleines Raumschiff Kapazität (30t)"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Mittleres Raumschiff Kapazität (60t)"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Kreuzer (100t) +Bewaffnung"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Sensor-Einheit"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Board-Kanone"));
				moonFactory.AddNeededProduct(Repository.GetProductByName("Board-Kanone"));

			var tatooine = new Planet("Tatooine");
				tatooine.Market = new Market("Moseisley", earth);
				tatooine.Sector = new Coordinates(10, 7, 1);
				tatooine.Industries = new Industries(tatooine);
				tatooine.Industries.Add(Repository.IndustryLibrary.GetIndustryByName("Weltraumschrott Sammler"));
				

			Galaxy.AddPlanet(earth);
			Galaxy.AddPlanet(moon);
			Galaxy.AddPlanet(tatooine);
	
		//	ShowMainMenu(UiPlayer);
		}

		
		private void Ship_Arrived(string message, Coordinates newPosition, Ship ship)
		{
			//Logger.Log($"Das Schiff ist angekommen: {ship.Cruise.Destination.ToString()}", TraceEventType.Information);
			ship.Cruise.Departure = ship.Cruise.Destination;
		//	ShowShipMenu(ship);
		}

		


		public Planets ScanPlanetsInNearbySectors(Ship ship, double range = 1)
		{
			throw new NotImplementedException();
			// xxx
			// x+x
			// xxx
		}

		public Ships ScanShipsInNearbySectors(Ship ship, double range = 1)
		{
			throw new NotImplementedException();
		}

	}
}
