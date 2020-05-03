using Google.Protobuf.Collections;
using SpaceDealerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceDealerCoreUi
{
	static class Program
	{
		public static RepeatedField<Planet> AllPlanets { get; set; }
		public static Player CurrentPlayer { get; set; }
		public static Ship CurrentShip { get; set; }
		public static Planet CurrentPlanet { get; set; }
		public static Form1 MainForm { get; set; }

		public static Assets PlanetAssets { get; set; }
		public static Assets PeopleAssets { get; set; }
		public static Assets SpaceShipAssets { get; set; }
		public static Assets AnimationAssets { get; set; }

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			PlanetAssets = new Assets(".\\Planets");
			PeopleAssets = new Assets(".\\People");
			SpaceShipAssets = new Assets(".\\Spaceships");
			AnimationAssets = new Assets(".\\Animations");
			MainForm = new Form1();
			Application.Run(MainForm);
		}
	}
}
