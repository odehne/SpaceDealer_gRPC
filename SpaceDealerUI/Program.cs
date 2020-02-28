using Grpc.Net.Client;
using SpaceDealerService;
using System;
using System.Threading.Tasks;

namespace SpaceDealerUI
{
	class Program
	{
        static async Task Main(string[] args)
        {
			var planets = GameProxy.GetAllPlanets().Result;
			var menu = new Menu();
			menu.ShowStartupScreen();
			var newPlayer = GameProxy.AddPlayer(menu.ShowNewPlayer()).Result;
			var shipName = menu.ShowNewShip(newPlayer);
			var newShip = GameProxy.AddShip(newPlayer.Name, shipName).Result;
			var selectedPlanet = menu.SelectPlanet(planets);
			var result = GameProxy.StartCruise(newPlayer.Name, newShip.ShipName, selectedPlanet).Result;

			if(result==true)
				Console.WriteLine($"{newShip.ShipName} auf dem Weg nach {selectedPlanet}.");
			Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

		//public void ShowMainMenu(Player player)
		//{
		//	Menu.ShowHeader(player);
		//	var mainSelected = Menu.ShowMainSelection();
		//	switch (mainSelected)
		//	{
		//		case 1:
		//			var selectedShip = Menu.SelectShip(player.Fleet);
		//			ShowShipMenu(selectedShip);
		//			break;
		//		case 2:
		//			Console.WriteLine("Wirklich beenden (j/n)?");
		//			var ret = Console.ReadLine();
		//			if (ret == "j")
		//				Environment.Exit(0);
		//			break;
		//	}
		//}

		//public void ShowShipMenu(Ship ship)
		//{
		//	//do
		//	//{
		//		var departurePlanet = ship.Cruise.Depature;
		//		Menu.ShowHeader(ship.Parent.Parent);
		//		var mainSelected = Menu.ShowShipSelection();
		//		switch (mainSelected)
		//		{
		//			case 1:
		//				var selectedPlanet = Menu.SelectPlanet(Galaxy.GetAllPlanets(departurePlanet));
		//				ship.StartCruise(departurePlanet, selectedPlanet);
		//				//ShowShipMenu(ship);
		//				break;
		//			case 2:
		//				var selectedMarket = Menu.SelectSellOrBuy();
		//				if (selectedMarket == 1)
		//				{
		//					var selectedProduct = Menu.SelectProductToSell(ship, departurePlanet);
		//					var sold = Menu.SellProduct(ship, departurePlanet, selectedProduct);
		//				}
		//				if (selectedMarket == 2)
		//				{
		//					var selectedProduct = Menu.Buy(ship, departurePlanet);
		//					var bought = Menu.BuyProduct(ship, departurePlanet, selectedProduct);
		//				}
		//				break;
		//			case 3:
		//				Console.WriteLine("Raumdock gibt es noch nicht.");
		//				break;
		//			case 4:
		//				ShowMainMenu(ship.Parent.Parent);
		//				break;
		//		}
		//	//} while (true);

		//}

	}
}
