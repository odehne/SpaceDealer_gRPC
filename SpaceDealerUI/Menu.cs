using Google.Protobuf.Collections;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerUI
{
	public class Menu
	{
		public event AnswerRecieved Answered;
		public delegate void AnswerRecieved(string answer);

		public SpaceDealerModels.Units.Player UiPlayer { get; set; }

		public void ShowStartupScreen()
		{
			Console.Clear();
			Console.WriteLine(@"                        .__   __                                .__                       .___     .__   ");
			Console.WriteLine(@"         __  _  __ ____ |  |_/  |_____________   __ __  _____   |  |__ _____    ____    __| _/____ |  |  ");
			Console.WriteLine(@"         \ \/ \/ // __ \|  |\   __\_  __ \__  \ |  |  \/     \  |  |  \\__  \  /    \  / __ |/ __ \|  |  ");
			Console.WriteLine(@"          \     /\  ___/|  |_|  |  |  | \// __ \|  |  /  Y Y  \ |   Y  \/ __ \|   |  \/ /_/ \  ___/|  |__");
			Console.WriteLine(@"           \/\_/  \___  >____/__|  |__|  (____  /____/|__|_|  / |___|  (____  /___|  /\____ |\___  >____/");
			Console.WriteLine(@"                      \/                      \/            \/       \/     \/     \/      \/    \/      ");
			Console.WriteLine();
			Console.WriteLine();
		}

		public void ShowPlayerStats(SpaceDealerModels.Units.Player player)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine($"Spieler: {player.Name}\tHeimat Planet:{player.HomePlanet}\tCredits:${player.Credits}");
			foreach (var ship in player.Fleet)
			{
				Console.WriteLine($"Schiff: {ship.Name}\t{ship.Cruise.Depature} --> {ship.Cruise.Destination}\tDistanz: {ship.Cruise.CurrentDistanceToDestination.ToDecimalString()} parsec\tSektor: {ship.Cruise.CurrentSector.ToString()}");
			}
		}

		private void ClearConsoleBody()
		{
			for (int i = 5; i < 28; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.Write("                                                                                                                      ");
			}
			Console.SetCursorPosition(0, 28);
			Console.WriteLine(CenterLine("H: Hauptmenü | F: Flotten-Übersicht | M: Märkte | R: Raumdock | CTRL+C: Beenden"));
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

		public int ShowMainSelection()
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine("---- Hauptmenü ----"));
			Console.WriteLine(CenterLine("1. Neues Ziel"));
			Console.WriteLine(CenterLine("2. Marktplatz"));
			Console.WriteLine(CenterLine("3. Raumdock"));
			Console.WriteLine(CenterLine("4. Anderes Schiff wählen"));
			return GetAnswerInt(1, 4);
		}

	
		public int ShowShipSelection(RepeatedField<SpaceDealerService.Ship> ships)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine("---- Shiff auswählen ----"));
			var i = 1;
			foreach (var ship in ships)
			{
				Console.WriteLine(CenterLine($"{i}. {ship.ShipName}"));
				i++;
			}

			return GetAnswerInt(1, 2);
		}

		public string ShowPlanetSelection(RepeatedField<SpaceDealerService.Planet> planets)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
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
				if (IsMainMenu(row))
					return 0;
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

		private bool IsMainMenu(string row)
		{
			if (row.Equals("h", StringComparison.InvariantCultureIgnoreCase))
			{
				ShowMainSelection();
				return true;
			}
			if (row.Equals("f", StringComparison.InvariantCultureIgnoreCase))
			{
				ShowShipSelection(Program.ThePlayer.Ships);
				return true;
			}
			if (row.Equals("m", StringComparison.InvariantCultureIgnoreCase))
			{
				ShowPlanetSelection(Program.AllPlanets);
				return true;
			}
			return false;
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

		public SpaceDealerModels.Units.ProductInStock SelectProductToSell(SpaceDealerModels.Units.Ship ship, SpaceDealerModels.Units.Planet planet)
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

		public SpaceDealerModels.Units.ProductInStock Buy(SpaceDealerModels.Units.Ship ship, SpaceDealerModels.Units.Planet planet)
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

		public double BuyProduct(SpaceDealerModels.Units.Ship ship, SpaceDealerModels.Units.Planet planet, SpaceDealerModels.Units.ProductInStock selectedProduct)
		{
			throw new NotImplementedException();
		}

		public double SellProduct(SpaceDealerModels.Units.Ship ship, SpaceDealerModels.Units.Planet planet, SpaceDealerModels.Units.ProductInStock selectedProduct)
		{
			throw new NotImplementedException();
		}

		internal SpaceDealerModels.Units.Ship SelectShip(Ships fleet)
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
