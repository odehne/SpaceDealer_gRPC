using Google.Protobuf.Collections;
using SpaceDealerService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SpaceDealerUI
{
	public class Menu
	{
		private enum MenuType
		{
			MainSelection = 0,
			PlayerInfo = 10,
			PlanetSelection = 20,
			PlanetInfo = 21,
			Market = 30,
			ShipSelection = 40,
			SpaceDock = 50,
			AddNewFeatures = 60,
			BattleAttack = 70,
			BattleDefend = 80,
			SaveGame = 100,
			Buy = 101,
			Sell = 102
		}


		private MenuType CurrentMenu { get; set; }
		public Ship CurrentShip { get; set; }
		public Player CurrentPlayer { get; set; }
		public RepeatedField<Planet> AllPlanets { get; set; }
		public BlockingCollection<ConsoleKeyInfo> Queue { get; set; }

		public Menu()
		{
			Queue = new BlockingCollection<ConsoleKeyInfo>();
			new Thread(() =>
			{
				while (true)
				{
					if (Queue.Any())
					{
						var k = Queue.Take();
						if (k != null)
						{
							if (k.Key == ConsoleKey.UpArrow)
							{
								//Menu up
							}
							if (k.Key == ConsoleKey.DownArrow)
							{
								//Menu down
							}
							if (k.Key == ConsoleKey.Enter)
							{
								//Select
							}
							switch (k.KeyChar)
							{
								case 'h':
									CurrentMenu = MenuType.MainSelection;
									ShowMainSelection();
									break;
								case 'a':
									CurrentMenu = MenuType.PlayerInfo;
									ShowPlayerInfo();
									break;
								case 'z':
									CurrentMenu = MenuType.PlanetSelection;
									ShowPlanetSelection();
									break;
								case 'm':
									CurrentMenu = MenuType.Market;
									ShowMarket(CurrentShip.CurrentPlanet);
									break;
								case 's':
									CurrentMenu = MenuType.ShipSelection;
									ShowShipSelection(CurrentPlayer.Ships);
									break;
								case 'r':
									CurrentMenu = MenuType.SpaceDock;
									ShowSpaceDock(CurrentPlayer, CurrentShip);
									break;
								case 'v':
									CurrentMenu = MenuType.SaveGame;
									ShowSaveGameResult();
									break;
								case 'p':
									CurrentMenu = MenuType.PlanetInfo;
									ShowPlanetInfo(CurrentShip);
									break;
								case 'e':
									CurrentMenu = MenuType.AddNewFeatures;
									ShowAddNewFeatures(CurrentShip);
									break;
								case 'g':
									CurrentMenu = MenuType.BattleAttack;
									SchowBattleAttack(CurrentShip);
									break;
								case 'w':
									CurrentMenu = MenuType.BattleDefend;
									ShowBattleDefend(CurrentShip);
									break;
								case 'k':
									CurrentMenu = MenuType.Buy;
									ShowBuy(CurrentPlayer, CurrentShip);
									break;
								case 'f':
									CurrentMenu = MenuType.Sell;
									ShowSell(CurrentPlayer, CurrentShip);
									break;
							}
						}
					}
					
				}
			})
			{ IsBackground = true }.Start();
		}

		private void ShowAddNewFeatures(Ship currentShip)
		{
			throw new NotImplementedException();
		}

		private void ShowSpaceDock(Player currentPlayer, Ship currentShip)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine($"\t\t\t  ------------------ Willkommen im Raumdock -----------------------");
			Console.WriteLine();
			Console.WriteLine($"\t\t\t\t    Name            : {currentShip.ShipName} - Position: {PlanetToString(currentShip.CurrentPlanet)}");
			Console.WriteLine($"\t\t\t\t    Schilde/Hülle   : {currentShip.Shields} / {currentShip.Hull}");
			Console.WriteLine($"\t\t\t\t    Frachtraumgröße : {currentShip.CargoSize}t");
			Console.WriteLine($"\t\t\t\t    Ladung          : {GetCargoLoad(currentShip)}");
			Console.WriteLine($"");

			Console.WriteLine(CenterLine("Schild (r)eparieren (+1) - $1500"));
			Console.WriteLine(CenterLine("(B)oardkanone hinzufügen - $8000"));
			Console.WriteLine(CenterLine("Sens(o)ren verbessern (1 Feld) - $6000"));
			Console.WriteLine(CenterLine("(L)aderaum erweitern (10t) - $3500"));
			Console.WriteLine(CenterLine("Anderes (S)chiff wählen"));
			Console.WriteLine($"");
			Console.WriteLine($"\t\t\t  -----------------------------------------------------------------");
		}

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

		private void ClearConsoleBody()
		{
			while (Console.In.Peek() != -1)
					Console.In.Read();
			
			for (int i = 5; i < 28; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.WriteLine("                                                                                                                                                 ");
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

		internal void ShowPlanetInfo(Ship ship)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine($"---- Willkommen auf {ship.CurrentPlanet.PlanetName} ----"));
			Console.WriteLine();
			foreach (var indi in ship.CurrentPlanet.Industries)
			{
				Console.WriteLine(CenterLine($"Der Planet besitzt diese Industrie: {indi.IndustryName}."));
				Console.WriteLine(CenterLine($"Diese Produkte werden produziert:"));
				Console.WriteLine(CenterLine(GetSingleLine(indi.GeneratedProducts.Select(x => x.ProductName))));
				Console.WriteLine(CenterLine("Diese Produkte werden benötigt:"));
				Console.WriteLine(CenterLine(GetSingleLine(indi.ProductsNeeded.Select(x => x.ProductName))));
			}
			Console.WriteLine();
			Console.WriteLine(CenterLine("(M)arktplatz"));
			Console.WriteLine(CenterLine("(I)nformationen"));
			Console.WriteLine(CenterLine("Neues (Z)iel"));
			Console.WriteLine(CenterLine("Anderes (S)chiff wählen"));
		}

		private void ShowMarket(Planet currentPlanet)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine($"---- Willkommen auf dem Marktplatz von {currentPlanet.PlanetName} ----"));
			Console.WriteLine();
			Console.WriteLine($"\t\t\t\t    Name            : {PlanetToString(currentPlanet)}");
			Console.WriteLine($"\t\t\t\t    Schiffe         : {CurrentShip.ShipName} ({CurrentShip.CargoSize}t)");
			Console.WriteLine($"\t\t\t\t    Credits         : ${CurrentPlayer.Credits}");
			Console.WriteLine();
			Console.WriteLine(CenterLine("---- Aktionen ----"));
			Console.WriteLine(CenterLine("(K)aufen"));
			Console.WriteLine(CenterLine("Verkau(f)en"));
		}


		private void ShowSell(Player currentPlayer, Ship currentShip)
		{
			var currentPlanet = CurrentShip.CurrentPlanet;
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine($"---- Willkommen auf dem Marktplatz von {currentPlanet.PlanetName} ----"));
			Console.WriteLine();
			Console.WriteLine($"\t\t\t\t    Name            : {PlanetToString(currentPlanet)}");
			Console.WriteLine($"\t\t\t\t    Schiffe         : {CurrentShip.ShipName} ({CurrentShip.CargoSize}t)");
			Console.WriteLine($"\t\t\t\t    Credits         : ${CurrentPlayer.Credits}");
			Console.WriteLine();
			Console.WriteLine($"\t\t\t\t    Produkte, die benötigt werden:");
			foreach (var neededProduct in currentShip.CurrentPlanet.Industries[0].ProductsNeeded)
			{
				Console.WriteLine($"\t\t\t\t    {neededProduct.ProductName.Tabyfy()}${neededProduct.PricePerTon.ToDecimalString()}/t - Auf Lager: [{neededProduct.TotalWeight.ToDecimalString()}t]");
			}

			Console.WriteLine($"\t\t\t\t    Aktuelle Ladung:");
			if (currentShip.CargoLoad != null)
			{
				foreach (var cargo in currentShip.CargoLoad.LoadedProducts)
				{
					Console.WriteLine($"\t\t\t\t    {cargo.ProductName.Tabyfy()}${cargo.PricePerTon.ToDecimalString()}/t - Auf Lager: [{cargo.TotalWeight.ToDecimalString()}t]");
				}
			}

		}

		private void ShowBuy(Player currentPlayer, Ship currentShip)
		{
			var currentPlanet = CurrentShip.CurrentPlanet;
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine($"---- Willkommen auf dem Marktplatz von {currentPlanet.PlanetName} ----"));
			Console.WriteLine();
			Console.WriteLine($"\t\t\t\t    Name            : {PlanetToString(currentPlanet)}");
			Console.WriteLine($"\t\t\t\t    Schiffe         : {CurrentShip.ShipName} ({CurrentShip.CargoSize}t)");
			Console.WriteLine($"\t\t\t\t    Credits         : ${CurrentPlayer.Credits}");
			Console.WriteLine();
			Console.WriteLine($"\t\t\t\t    Produkte, die angeboten werden:");
			foreach (var generatedProduct in currentShip.CurrentPlanet.Industries[0].GeneratedProducts)
			{
				Console.WriteLine($"\t\t\t\t    {generatedProduct.ProductName.Tabyfy()}${generatedProduct.PricePerTon.ToDecimalString()}/t - Auf Lager: [{generatedProduct.TotalWeight.ToDecimalString()}t]");
			}
		}


		private void ShowSaveGameResult()
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			var result = GameProxy.SaveGame(CurrentPlayer.Name).Result;
			if (result.GameSaved == true)
			{
				Console.WriteLine(CenterLine($"Der Spielstand von {CurrentPlayer.Name} wurde erfogreich gespeichert."));
			}
			else
			{
				Console.WriteLine(CenterLine($"Beim Speichern ging etwas schief.Der Spielstand von {CurrentPlayer.Name} konnte nicht gespeichert werden."));
			}
		}

		private void ShowPlayerInfo()
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			CurrentPlayer = GameProxy.GetPlayer(CurrentPlayer.Name).Result;
			Console.WriteLine($"\t\t\t  ------------------ Spieler Informationen -----------------------");
			Console.WriteLine();
			Console.WriteLine($"\t\t\t\t  Spieler      : {CurrentPlayer.Name} - Heimat: {CurrentPlayer.HomePlanet}");
			Console.WriteLine($"\t\t\t\t  Kontostand   : ${CurrentPlayer.Credits}");
			Console.WriteLine();
			Console.WriteLine($"\t\t\t\t  Schiffe: ");
			foreach (var ship in CurrentPlayer.Ships)
			{
				Console.WriteLine($"\t\t\t\t    Name            : {ship.ShipName} - Position: {PlanetToString(ship.CurrentPlanet)}");
				Console.WriteLine($"\t\t\t\t    Schilde/Hülle   : {ship.Shields} / {ship.Hull}");
				Console.WriteLine($"\t\t\t\t    Frachtraumgröße : {ship.CargoSize}t");
				Console.WriteLine($"\t\t\t\t    Ladung          : {GetCargoLoad(ship)}");
				Console.WriteLine($"");
			}
			Console.WriteLine($"\t\t\t\t  Entdeckte Planeten: ");
			foreach (var p in AllPlanets)
			{
				Console.WriteLine($"\t\t\t\t    Name            : {PlanetToString(p)}");
				Console.WriteLine($"\t\t\t\t    Produkte, die benötigt werden:");
				foreach (var neededProduct in p.Industries[0].ProductsNeeded)
				{
					Console.WriteLine($"\t\t\t\t    {neededProduct.ProductName.Tabyfy()}${neededProduct.PricePerTon.ToDecimalString()}/t - Auf Lager: [{neededProduct.TotalWeight.ToDecimalString()}t]");
				}
				Console.WriteLine($"\t\t\t\t    Produkte, die verkauft werden:");
				foreach (var generatedProduct in p.Industries[0].GeneratedProducts)
				{
					Console.WriteLine($"\t\t\t\t    {generatedProduct.ProductName.Tabyfy()}${generatedProduct.PricePerTon.ToDecimalString()}/t - Auf Lager: [{generatedProduct.TotalWeight.ToDecimalString()}t]");
				}
				Console.WriteLine($"");
			}
			Console.WriteLine($"\t\t\t  ------------------ Spieler Informationen -----------------------");
		}

		private string GetCargoLoad(Ship ship)
		{
			var lst = "";
			if (ship.CargoLoad == null || ship.CargoLoad.LoadedProducts.Count==0)
				return "Keine Fracht";
			foreach (var p in ship.CargoLoad.LoadedProducts)
			{
				lst += $"{p.ProductName} ({p.TotalWeight}t";
			}
			return lst;
		}

		internal void ShowNewPlanet(Ship ship)
		{
			CurrentShip = ship;
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine($"{ship.ShipName} hat einen neuen Planeten gefunden! Planet: {PlanetToString(ship.Cruise.NewPlanetDiscovered)}"));
			Console.WriteLine(CenterLine(""));
			Console.WriteLine(CenterLine("---- Aktionen ----"));
			Console.WriteLine(CenterLine("Informationen zum (P)laneten"));
			Console.WriteLine(CenterLine("Neues (Z)iel festlegen"));
		}

		internal void ShowAttackMenu(Ship ship)
		{
			CurrentShip = ship;
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine($"{ship.ShipName} wird angegriffen! Gegnerisches Schiff: {ship.Cruise.EnemyBattleShip.ShipName} Schile: {ship.Cruise.EnemyBattleShip.Shields} Hülle: {ship.Cruise.EnemyBattleShip.Hull}"));
			Console.WriteLine(CenterLine(""));
			Console.WriteLine(CenterLine("---- Aktionen ----"));
			Console.WriteLine(CenterLine("An(g)reifen"));
			Console.WriteLine(CenterLine("Aus(w)eichen"));
		}

		private void ShowBattleDefend(Ship ship)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine("---- AUSWEICHEN ----"));
			var defenceResult = GameProxy.BattleDefend(CurrentPlayer.Name, CurrentShip.ShipName).Result;
			var attackResult = GameProxy.BattleAttack(CurrentPlayer.Name, CurrentShip.ShipName).Result;
			Console.WriteLine(CenterLine(""));
			Console.WriteLine(CenterLine($"Angriff: {attackResult.Message}"));
			Console.WriteLine(CenterLine($"Verteidigung: {defenceResult.Message}"));
			Console.WriteLine(CenterLine(""));
			Console.WriteLine(CenterLine("---- Aktionen ----"));
			Console.WriteLine(CenterLine("An(g)reifen"));
			Console.WriteLine(CenterLine("Aus(w)eichen"));
		}

		private void SchowBattleAttack(Ship ship)
		{
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine("---- ANGRIFF ----"));
			var attackResult = GameProxy.BattleAttack(CurrentPlayer.Name, CurrentShip.ShipName).Result;
			var defenceResult = GameProxy.BattleDefend(CurrentPlayer.Name, CurrentShip.ShipName).Result;
			Console.WriteLine(CenterLine(""));
			Console.WriteLine(CenterLine($"Angriff: {attackResult.Message}"));
			Console.WriteLine(CenterLine($"Verteidigung: {defenceResult.Message}"));
			Console.WriteLine(CenterLine(""));
			Console.WriteLine(CenterLine("---- Aktionen ----"));
			Console.WriteLine(CenterLine("An(g)reifen"));
			Console.WriteLine(CenterLine("Aus(w)eichen"));
		}

		public string ShowNewShip(Player player)
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

		public void ShowMainSelection()
		{
			AllPlanets = GameProxy.GetAllPlanets().Result;
			CurrentPlayer = GameProxy.GetPlayer(CurrentPlayer.Name).Result;
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine("---- Hauptmenü ----"));
			Console.WriteLine(CenterLine("(A)ktuelle Informationen"));
			Console.WriteLine(CenterLine("Neues (Z)iel"));
			Console.WriteLine(CenterLine("(R)aumdock"));
			Console.WriteLine(CenterLine("(v) Spiel speichern"));
			Console.WriteLine(CenterLine("Anderes (S)chiff wählen"));
		}

		public void ShowShipSelection(RepeatedField<Ship> ships)
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
			var index = GetAnswerInt(1, i);
			CurrentShip = ships[index - 1];
			ShowMainSelection();
		}

		public bool ShowPlanetSelection()
		{
			AllPlanets = GameProxy.GetAllPlanets().Result;
			ClearConsoleBody();
			Console.SetCursorPosition(0, 7);
			Console.WriteLine(CenterLine("---- Ziel planet wählen ----"));
			var i = 0;
			foreach (var p in AllPlanets)
			{
				i += 1;
				Console.WriteLine(CenterLine($"{i}. {p.PlanetName.Tabyfy()}"));
			}

			var selected = GetAnswerInt(1, i);
			var planetName = AllPlanets[selected - 1].PlanetName;
			return GameProxy.StartCruise(CurrentPlayer.Name, CurrentShip.ShipName, planetName).Result;
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
				ShowShipSelection(CurrentPlayer.Ships);
				return true;
			}
			if (row.Equals("m", StringComparison.InvariantCultureIgnoreCase))
			{
				ShowPlanetSelection();
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

		//public SpaceDealerModels.Units.ProductInStock SelectProductToSell(SpaceDealerModels.Units.Ship ship, SpaceDealerModels.Units.Planet planet)
		//{
		//	ClearConsoleBody();
		//	Console.SetCursorPosition(0, 5);
		//	Console.WriteLine(CenterLine("---- Verkaufen ----"));
		//	Console.WriteLine(CenterLine($"Planet: {planet.Name}"));
		//	Console.WriteLine(CenterLine($"Markplatz: {planet.Market.Name}"));
		//	Console.WriteLine(CenterLine($"Raumschiff: {ship.Name}"));
		//	Console.WriteLine(CenterLine($"Kapazität: {ship.CargoSize}t"));
		//	Console.WriteLine(CenterLine("---- Interessiert an ----"));
		//	var i = 0;
		//	foreach (var p in planet.Industries[0].ProductsNeeded)
		//	{
		//		i += 1;
		//		Console.WriteLine($"{i}. {p.Name.Tabyfy()}");
		//	}
		//	var selected = GetAnswerInt(1, i);
		//	return planet.Industries[0].ProductsNeeded[selected - 1];
		//}

		private string GetSingleLine(IEnumerable<string> names)
		{
			return string.Join(',', names);
		}

		//public double BuyProduct(SpaceDealerModels.Units.Ship ship, SpaceDealerModels.Units.Planet planet, SpaceDealerModels.Units.ProductInStock selectedProduct)
		//{
		//	throw new NotImplementedException();
		//}

		//public double SellProduct(SpaceDealerModels.Units.Ship ship, SpaceDealerModels.Units.Planet planet, SpaceDealerModels.Units.ProductInStock selectedProduct)
		//{
		//	throw new NotImplementedException();
		//}

		public string CoordinatesToString(Coordinates coordinates)
		{
			return $"[{coordinates.X},{coordinates.Y},{coordinates.Z}]";
		}

		public string PlanetToString(Planet planet)
		{
			return $"{planet.PlanetName} {CoordinatesToString(planet.Sector)}";
		}

	}
}
