using Google.Protobuf.Collections;
using Grpc.Net.Client;
using SpaceDealerService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceDealerUI
{

	/// <summary>
	/// TODO: Neue Planeten entdecken, wenn man einen neuen erreicht
	/// TODO: Informationen kaufen, was könnte gebraucht werden auf Planeten in der Nähe
	/// TODO: Battle vollenden
	/// TODO: Marktplatz und Kauf/Verkauf implementieren
	/// TODO: Raumdock implementieren
	/// TODO: Spielstand speichern und laden
	/// </summary>

	class Program
	{
		public static Menu TheMenu { get; set; }
	
		static async Task Main(string[] args)
        {
			TheMenu = new Menu();
			TheMenu.AllPlanets = GameProxy.GetAllPlanets().Result;
			TheMenu.ShowStartupScreen();
			TheMenu.CurrentPlayer = GameProxy.AddPlayer(TheMenu.ShowNewPlayer()).Result;
			var shipName = TheMenu.ShowNewShip(TheMenu.CurrentPlayer);
			TheMenu.CurrentPlayer = GameProxy.GetPlayer(TheMenu.CurrentPlayer.Name).Result;
			TheMenu.CurrentShip = GameProxy.AddShip(TheMenu.CurrentPlayer.Name, shipName).Result;
			var started = TheMenu.ShowPlanetSelection();
		
			var updateThread = new Thread(GetGameUpdates) { IsBackground = true };
			updateThread.Start();

			TheMenu.ShowMainSelection();

			new Thread(() =>
			{
				while (true) TheMenu.Queue.Add(Console.ReadKey(true));
			})
			{ IsBackground = true }.Start();

			
			Thread.Sleep(Timeout.Infinite);
		}

		private static void GetGameUpdates()
		{
			IntPtr stdin;
			do
			{
				var updates = GameProxy.GetUpdates(TheMenu.CurrentPlayer.Name).Result;
				if (updates != null)
				{
					foreach (var u in updates)
					{
						switch (u.UpdateState)
						{
							case UpdateStates.ArrivedOnTarget:
								TheMenu.ShowPlanetInfo(u.Ship);
								break;
							case UpdateStates.NewPlanetDiscovered:
								TheMenu.ShowNewPlanet(u.Ship);
								break;
							case UpdateStates.UnderAttack:
								TheMenu.ShowAttackMenu(u.Ship);
								break;
						}
					}
				}
				Thread.Sleep(1000);
			} while (true);
			
		}

		private static void Menu_Answered(string answer)
		{
			
		}

		
	}
}
