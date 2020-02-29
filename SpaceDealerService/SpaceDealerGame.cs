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
			var earth = new Planet("Erde", null);
			earth.Market = new Market("Berlin", null, earth);
			earth.Industries = new Industries(earth);
			earth.Sector = new Coordinates(0, 0, 0);

			var farming = earth.Industries.AddIndustry("Landwirtschaft", null);
				farming.GeneratedProducts.AddProduct(Repository.GetProductByName("Kuh-Milch"));
				farming.GeneratedProducts.AddProduct(Repository.GetProductByName("Mais"));
				farming.GeneratedProducts.AddProduct(Repository.GetProductByName("Rindfleisch"));
				farming.GeneratedProducts.AddProduct(Repository.GetProductByName("Reis"));
				farming.GeneratedProducts.AddProduct(Repository.GetProductByName("Soya"));
				farming.GeneratedProducts.AddProduct(Repository.GetProductByName("Orangensaft"));
				farming.GeneratedProducts.AddProduct(Repository.GetProductByName("Bacon"));

			var moon = new Planet("Erden-Mond", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Beschreibung", "Kreist um den Planeten Erde. Besitzt keine eigene Athmossphäre.") });
				moon.Market = new Market("Mondbasis Aplha 1", null, moon);
				moon.Industries = new Industries(moon);
				moon.Sector = new Coordinates(1, 0, 0);

			var moonFactory = moon.Industries.AddIndustry("Raumschiff Fabrik", null);
				moonFactory.GeneratedProducts.AddProduct(Repository.GetProductByName("Kleines Raumschiff Kapazität (30t)"));
			moonFactory.GeneratedProducts.AddProduct(Repository.GetProductByName("Mittleres Raumschiff Kapazität (60t)"));
			moonFactory.GeneratedProducts.AddProduct(Repository.GetProductByName("Kreuzer (100t) +Bewaffnung"));
			moonFactory.GeneratedProducts.AddProduct(Repository.GetProductByName("Sensor-Einheit"));
			moonFactory.GeneratedProducts.AddProduct(Repository.GetProductByName("Board-Kanone"));

			var tatooine = new Planet("Tatooine", null);
				tatooine.Market = new Market("Moseisley", null, earth);
				tatooine.Industries = new Industries(tatooine);
				tatooine.Sector = new Coordinates(10, 7, 1);

			var weltraumSchrott = tatooine.Industries.AddIndustry("Weltraumschrott Sammler", null);
				weltraumSchrott.GeneratedProducts.AddProduct(Repository.GetProductByName("Tie-Fighter Flügel"));
				weltraumSchrott.GeneratedProducts.AddProduct(Repository.GetProductByName("Wasser Evaporatoren"));
				weltraumSchrott.GeneratedProducts.AddProduct(Repository.GetProductByName("Sternen-Zerstörer Triebwerke"));
				weltraumSchrott.GeneratedProducts.AddProduct(Repository.GetProductByName("Cyberkristalle"));


			Galaxy.AddPlanet(earth);
			Galaxy.AddPlanet(moon);
			Galaxy.AddPlanet(tatooine);
	
		//	ShowMainMenu(UiPlayer);
		}

		
		private void Ship_Arrived(string message, Coordinates newPosition, Ship ship)
		{
			//Logger.Log($"Das Schiff ist angekommen: {ship.Cruise.Destination.ToString()}", TraceEventType.Information);
			ship.Cruise.Depature = ship.Cruise.Destination;
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
