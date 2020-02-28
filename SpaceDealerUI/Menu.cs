using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerUI
{
	public class Menu
	{ 
		public void ShowStartupScreen()
		{
			Console.Clear();
			Console.WriteLine(@"               .__   __                                .__                       .___     .__   ");
			Console.WriteLine(@"__  _  __ ____ |  |_/  |_____________   __ __  _____   |  |__ _____    ____    __| _/____ |  |  ");
			Console.WriteLine(@"\ \/ \/ // __ \|  |\   __\_  __ \__  \ |  |  \/     \  |  |  \\__  \  /    \  / __ |/ __ \|  |  ");
			Console.WriteLine(@" \     /\  ___/|  |_|  |  |  | \// __ \|  |  /  Y Y  \ |   Y  \/ __ \|   |  \/ /_/ \  ___/|  |__");
			Console.WriteLine(@"  \/\_/  \___  >____/__|  |__|  (____  /____/|__|_|  / |___|  (____  /___|  /\____ |\___  >____/");
			Console.WriteLine(@"             \/                      \/            \/       \/     \/     \/      \/    \/      ");
			
		}

		public void ShowHeader(Player player)
		{
			Console.SetCursorPosition(0, 0);
			Console.WriteLine($"Spieler: {player.Name}\tHeimat Planet:{player.HomePlanet}\tCredits:${player.Credits}");
			foreach (var ship in player.Fleet)
			{
				Console.WriteLine($"Schiff: {ship.Name}\t{ship.Cruise.Depature} --> {ship.Cruise.Destination}\tDistanz: {ship.Cruise.CurrentDistanceToDestination.ToDecimalString()} parsec\tSektor: {ship.Cruise.CurrentSector.ToString()}");
			}
			Console.SetCursorPosition(0, 28);
			Console.WriteLine(CenterLine("F2: Hauptmenü | F3: Flotten-Übersicht | F4: Märkte | F5: Raumdock | CTRL+C: Beenden"));
		}

		private void ClearConsoleBody()
		{
			for (int i = 5; i < 28; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.Write("                                                                                                                      ");
			}
		}

		private string CenterLine(string v)
		{
			var l = 120 - v.Length;
			var tabs = "";
			if (l > 0)
			{
				for (int i = 0; i < l / 2; i++)
				{
					tabs += " ";
				}
			}
			return tabs + v;
		}

		public string ShowNewShip(SpaceDealerService.Player player)
		{
			do
			{
				var shipName = GetAnswerString("Raumschiff name");

				if (GameProxy.ShipNameTaken(player.Name, shipName).Result == false)
				{
					return shipName;
				}
				else
				{
					Console.WriteLine(CenterLine($"{player} besitzt bereits ein Schiff mit diesem Namen."));
				}

			} while (true);

		}

		public string ShowNewPlayer()
		{
			do
			{
				var playerName = GetAnswerString("Spieler name");

				if (GameProxy.PlayerNameTaken(playerName).Result == false)
				{
					return playerName;
				}
				else
				{
					Console.WriteLine(CenterLine("Spieler name leider bereits vergeben."));
				}

			} while (true);
			
		}

		public int ShowShipSelection()
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 5);
			Console.WriteLine(CenterLine("---- Hauptmenü ----"));
			Console.WriteLine(CenterLine("1. Neues Ziel"));
			Console.WriteLine(CenterLine("2. Marktplatz"));
			Console.WriteLine(CenterLine("3. Raumdock"));
			Console.WriteLine(CenterLine("4. Anderes Schiff wählen"));
			return GetAnswerInt(1, 4);
		}

		public int ShowMainSelection()
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 5);
			Console.WriteLine(CenterLine("---- Hauptmenü ----"));
			Console.WriteLine(CenterLine("1. Shiff auswählen"));
			Console.WriteLine(CenterLine("2. Beenden"));
			return GetAnswerInt(1, 2);
		}

		public string SelectPlanet(List<SpaceDealerService.Planet> planets)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 5);
			Console.WriteLine(CenterLine("---- Ziel planet wählen ----"));
			var i = 0;
			foreach (var p in planets)
			{
				i += 1;
				Console.WriteLine(CenterLine($"{i}. {p.PlanetName.Tabyfy()}"));
			}

			var selected = GetAnswerInt(1, i);
			return planets[selected - 1].PlanetName;
		}

		private string GetAnswerString(string question)
		{
			do
			{
				Console.Write(CenterLine($"{question}: "));
				var answer = Console.ReadLine();
				if (!string.IsNullOrEmpty(question))
				{
					return answer;
				}
			} while (true);
		}

		private int GetAnswerInt(int min, int max)
		{

			do
			{
				Console.Write(CenterLine($"Bitte wählen [{min}-{max}]: "));
				var row = Console.ReadLine();
				if (int.TryParse(row, out int mnuItem))
				{
					if (mnuItem >= 0 && mnuItem <= max)
					{
						return mnuItem;
					}
				}
				Console.WriteLine(CenterLine("Unerwartete Eingabe."));
			} while (true);
		}

		public int SelectSellOrBuy()
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 5);
			Console.WriteLine(CenterLine("---- Marktplatz ----"));
			Console.WriteLine(CenterLine("1. Kaufen"));
			Console.WriteLine(CenterLine("2. Verkaufen"));
			return GetAnswerInt(1, 2);
		}

		public ProductInStock SelectProductToSell(Ship ship, Planet planet)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 5);
			Console.WriteLine(CenterLine("---- Verkaufen ----"));
			Console.WriteLine(CenterLine($"Planet: {planet.Name}"));
			Console.WriteLine(CenterLine($"Markplatz: {planet.Market.Name}"));
			Console.WriteLine(CenterLine($"Raumschiff: {ship.Name}"));
			Console.WriteLine(CenterLine($"Kapazität: {ship.CargoSize}t"));
			Console.WriteLine(CenterLine("---- Interessiert an ----"));
			var i = 0;
			foreach (var p in planet.Market.ProductsNeeded)
			{
				i += 1;
				Console.WriteLine($"{i}. {p.Name.Tabyfy()}");
			}
			var selected = GetAnswerInt(1, i);
			return planet.Market.ProductsNeeded[selected - 1];
		}

		public ProductInStock Buy(Ship ship, Planet planet)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 5);
			Console.WriteLine(CenterLine("---- Kaufen ----"));
			Console.WriteLine(CenterLine($"Planet: {planet.Name}"));
			Console.WriteLine(CenterLine($"Markplatz: {planet.Market.Name}"));
			Console.WriteLine(CenterLine($"Raumschiff: {ship.Name}"));
			Console.WriteLine(CenterLine($"Kapazität: {ship.CargoSize}t"));
			Console.WriteLine(CenterLine("---- Verkauft gern ----"));
			var i = 0;
			foreach (var p in planet.Market.GetProductsInStock())
			{
				i += 1;
				Console.WriteLine($"{i}. {p.Name.Tabyfy()}{p.GetTotalWeight().ToDecimalString()}t\t{p.PricePerTon.ToDecimalString()} credits/t.");
			}
			var selected = GetAnswerInt(1, i);
			return planet.Market.ProductsNeeded[selected - 1];
		}

		public double BuyProduct(Ship ship, Planet planet, ProductInStock selectedProduct)
		{
			throw new NotImplementedException();
		}

		public double SellProduct(Ship ship, Planet planet, ProductInStock selectedProduct)
		{
			throw new NotImplementedException();
		}

		internal Ship SelectShip(Ships fleet)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 5);
			Console.WriteLine(CenterLine("---- Shiff wählen ----"));
			var i = 0;
			foreach (var s in fleet)
			{
				i += 1;
				Console.WriteLine(CenterLine($"{i}. {s.Name.Tabyfy()}"));
			}

			var selected = GetAnswerInt(1, i);
			return fleet[selected - 1];
		}
	}
}
