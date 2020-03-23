using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf.Collections;
using SpaceDealerService;


namespace SpaceDealerForms
{
	static class Program
	{
		public static RepeatedField<Planet> AllPlanets { get; private set; }
		public static Player CurrentPlayer { get; set; }
		public static Ship CurrentShip { get; set; }
		public static Planet CurrentPlanet { get; set; }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
			
		}
	}
}
