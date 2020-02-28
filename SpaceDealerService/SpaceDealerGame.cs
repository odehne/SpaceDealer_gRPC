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
		}

		public void Init()
		{
			var earth = new Planet("Erde", null);
			earth.Market = new Market("Berlin", null, earth);
			earth.Industries = new Industries(earth);
			earth.Sector = new Coordinates(0, 0, 0);

			var farming = earth.Industries.AddIndustry("Landwirtschaft", null);
				farming.GeneratedProducts.AddProduct("Kuh-Milch", null, 0.2, 1.0, 0.1);
				farming.GeneratedProducts.AddProduct("Mais", null, 0.2, 1.0, 0.1);
				farming.GeneratedProducts.AddProduct("Rindfleisch", null, 0.1, 0.5, 0.25);
				farming.GeneratedProducts.AddProduct("Reis", null, 0.1, 0.5, 0.3);
				farming.GeneratedProducts.AddProduct("Soya", null, 0.1, 0.5, 0.3);
				farming.GeneratedProducts.AddProduct("Orangensaft", null, 0.1, 0.5, 0.25);
				farming.GeneratedProducts.AddProduct("Bacon", null, 0.1, 0.5, 0.25);

			var moon = new Planet("Erden-Mond", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Beschreibung", "Kreist um den Planeten Erde. Besitzt keine eigene Athmossphäre.") });
				moon.Market = new Market("Mondbasis Aplha 1", null, moon);
				moon.Industries = new Industries(moon);
				moon.Sector = new Coordinates(1, 0, 0);

			var moonFactory = moon.Industries.AddIndustry("Raumschiff Fabrik", null);
				moonFactory.GeneratedProducts.AddProduct("Kleines Raumschiff Kapazität (30t)", null, .1, 0, 50.0);
				moonFactory.GeneratedProducts.AddProduct("Mittleres Raumschiff Kapazität (60t)", null, .05, 0, 100.0);
				moonFactory.GeneratedProducts.AddProduct("Kreuzer (100t) +Bewaffnung", null, .02, 0, 150.0);
				moonFactory.GeneratedProducts.AddProduct("Sensor-Einheit", null, .2, 0, 0.1);
				moonFactory.GeneratedProducts.AddProduct("Board-Kanone", null, .2, 0, 0.1);

			var tatooine = new Planet("Tatooine", null);
				tatooine.Market = new Market("Moseisley", null, earth);
				tatooine.Industries = new Industries(tatooine);
				tatooine.Sector = new Coordinates(10, 7, 1);

			var weltraumSchrott = tatooine.Industries.AddIndustry("Weltraumschrott Sammler", null);
				weltraumSchrott.GeneratedProducts.AddProduct("Tie-Fighter Flügel", null, .1, .1, 1.1);
				weltraumSchrott.GeneratedProducts.AddProduct("Wasser Evaporatoren", null, .3, 0, 0.5);
				weltraumSchrott.GeneratedProducts.AddProduct("Sternen-Zerstörer Triebwerke", null, .3, 0, 700);
				weltraumSchrott.GeneratedProducts.AddProduct("Cyberkristalle", null, .001, .1, .1);


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
