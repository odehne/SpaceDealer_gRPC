using SpaceDealerCoreUi.Controls;
using SpaceDealerService;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SpaceDealerCoreUi
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			var updateThread = new Thread(GetGameUpdates) { IsBackground = true };
			updateThread.Start();
			Init();
		}

		private async void Init()
		{
			Program.AllPlanets = await GameProxy.GetAllPlanets();
			Program.CurrentPlayer = await GameProxy.AddPlayer("Olli");
			fp2.Controls.Clear();
			fp2.Controls.Add(new MenuControl());
		}

		private Ship GetCurrentShip()
		{
			if (Program.CurrentShip == null)
			{
				Program.CurrentShip = Program.CurrentPlayer.Ships[0];
			}
			return Program.CurrentShip;
		}

		public void ClearMenuPanel()
		{
			for (int i = fp2.Controls.Count -1; i > 0 ; i--)
			{
				fp2.Controls.Remove(fp2.Controls[i]);
			}
		}

		public void ShowShip()
		{
			ClearMenuPanel();
			var ctl = new CurrentShipControl();
			ctl.Init(GetCurrentShip());
			fp2.Controls.Add(ctl);
		}

		public async void ShowShipSelection()
		{
			ClearMenuPanel();
			var allShips = await GameProxy.GetAllShips(Program.CurrentPlayer.Name);
			foreach (var p in allShips)
			{
				var ctl = new ShipSelectionControl();
				ctl.Init(p);
				fp2.Controls.Add(ctl);
			}
		}

		public async void StartCruise(Planet destination)
		{
			var result = await GameProxy.StartCruise(Program.CurrentPlayer.Name, Program.CurrentShip.ShipName, destination.PlanetName);
			if (result == true)
			{
				var msg = new TravellingControl();
				msg.SetMessage(Program.SpaceShipAssets.GetRandomAsset().Path, "Neuer Kurs", $"Neuer Kurs nach {destination.ToPositiontring()} liegt an, sir!");
				fp1.Controls.Add(msg);
			}
		}

		public void ShowPlanetSelection()
		{
			ClearMenuPanel();
			foreach (var p in Program.AllPlanets)
			{
				var ctl = new PlanetSelectionControl();
				ctl.Init(p);
				fp2.Controls.Add(ctl);
			}
		}


		public void LoadPlayer()
		{
			if (Program.CurrentPlayer != null)
			{

			}
		}

		private void neuesSpielToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		
			//var mc = new InformantControl();
			//mc.SetMessage("People\\Chagrian.png", "Nützliche Informationen", "Der Chagrian hat nützliche Informationen über das Planetensystem in dem sich den Frachter King Creole befindet.");
			//fp1.Controls.Add(mc);

			//var mc2 = new DistressCallControl();
			//mc2.SetMessage("People\\Feeorin.png", "Wir brauchen Hilfe!", "Wir wachen auf dem Weg zum Planetensystem Betageuze und wurden überfallen!\n" +
			//	"Wer in der Nähe ist und uns unterstützen kann soll bitte schnell vorbeikommen!\n" +
			//	"Unsere Koordinaten sind [17,8,12].");
			//fp1.Controls.Add(mc2);

			//var mc3 = new FoundNewPlanetControl();
			//mc3.SetMessage("Planets\\image_part_017.jpg", "Neuer Planet entdeckt", "Dein Frachter King Creole hat einen neuen Planeten entdeckt.");
			//fp1.Controls.Add(mc3);

			//var ship = new CurrentShipControl();
			//ship
		}

		private void GetGameUpdates()
		{
			do
			{
				if (Program.CurrentPlayer != null)
				{
					var updates = GameProxy.GetUpdates(Program.CurrentPlayer.Name).Result;
					if (updates != null)
					{
						foreach (var u in updates)
						{
							switch (u.UpdateState)
							{
								case UpdateStates.ArrivedOnTarget:
									ArrivedAtTarget(u.Ship);
								//	TheMenu.ShowPlanetInfo(u.Ship);
									break;
								case UpdateStates.NewPlanetDiscovered:
									FoundNewPlanet(u.Ship);
									break;
								case UpdateStates.UnderAttack:
									//	TheMenu.ShowAttackMenu(u.Ship);
									UnderAttack(u.Ship);
									break;
							}
						}
					}
				}
				Thread.Sleep(1000);
			} while (true);

		}
		private void UnderAttack(Ship ship)
		{
			var planetName = ship.CurrentPlanet.PlanetName;
			var position = ship.CurrentPlanet.ToPositiontring();
			var mc1 = new DistressCallControl();
			mc1.SetMessage(Program.PeopleAssets.GetRandomAsset().Path, $"Roter Alarm!", $"Wir machen deinen Mini-Frachter fertig! Du hast keine Chance gegen uns. Gib lieber gleich auf oder wir zermalmen dich!\nPosition: {position}.");
			fp1.Controls.Add(mc1);
		}

		private void ArrivedAtTarget(Ship ship)
		{
			var planetName = ship.CurrentPlanet.PlanetName;
			var position = ship.CurrentPlanet.ToPositiontring();
			var mc1 = new FoundNewPlanetControl();
			mc1.SetMessage(Program.PlanetAssets.GetRandomAsset().Path, $"Willkommen in {planetName}", $"Dein Frachter {ship.ShipName} befindet sich im Orbit des Zielplanetens.\nPosition: {position}."); ;
			fp1.Controls.Add(mc1);
		}

		private void FoundNewPlanet(Ship ship)
		{
			var planetName = ship.CurrentPlanet.PlanetName;
			var position = ship.CurrentPlanet.ToPositiontring();
			var mc1 = new FoundNewPlanetControl();
			mc1.SetMessage(Program.PlanetAssets.GetRandomAsset().Path, $"Willkommen in {planetName}", $"Dein Frachter {ship.ShipName} hat einen neuen Planeten entdeckt.\nPosition: {position}.");
			fp1.Controls.Add(mc1);
		}
	}
}
