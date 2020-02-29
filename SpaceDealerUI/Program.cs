using Google.Protobuf.Collections;
using Grpc.Net.Client;
using SpaceDealerService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceDealerUI
{
	class Program
	{
		public static Player ThePlayer { get; set; }
		public static RepeatedField<Planet> AllPlanets { get; set; }
		public static Menu TheMenu { get; set; }

        static async Task Main(string[] args)
        {
			AllPlanets = GameProxy.GetAllPlanets().Result;
			TheMenu = new Menu();
			TheMenu.Answered += Menu_Answered;
			TheMenu.ShowStartupScreen();
			ThePlayer = GameProxy.AddPlayer(TheMenu.ShowNewPlayer()).Result;
			var shipName = TheMenu.ShowNewShip(ThePlayer);
			var newShip = GameProxy.AddShip(ThePlayer.Name, shipName).Result;
			var selectedPlanet = TheMenu.ShowPlanetSelection(AllPlanets);

			ThePlayer = GameProxy.GetPlayer(ThePlayer.Name).Result;

			var result = GameProxy.StartCruise(ThePlayer.Name, newShip.ShipName, selectedPlanet).Result;

			if(result==true)
				Console.WriteLine($"{newShip.ShipName} auf dem Weg nach {selectedPlanet}.");
			Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

		private static void Menu_Answered(string answer)
		{
			
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
