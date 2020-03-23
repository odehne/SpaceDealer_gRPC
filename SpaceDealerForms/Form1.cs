using SpaceDealerForms.Controls;
using SpaceDealerService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceDealerForms
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}


		private void spielstandLadenToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void neuesSpielToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var s = new Ship { ShipName = "TestShip" };
			s.Shields = 3;
			s.Hull = 3;
			s.CargoLoad = new Load();
			s.CurrentPlanet = new Planet { PlanetName = "Erde", Sector = new Coordinates { X = 0, Y = 0, Z = 1 } };
			s.State = ShipState.Idle;
			var shC = new CurrentShipControl();
			shC.Init(s);
			fp2.Controls.Add(shC);

			var mc1 = new FoundNewPlanetControl();
			mc1.SetMessage("Planets\\image_part_023.jpg", "Wir haben einen neuen Planeten entdeckt!", "Dein Frachter King Creole hat einen neuen Planeten entdeckt.");
			fp1.Controls.Add(mc1);

			var mc = new InformantControl();
			mc.SetMessage("People\\Chagrian.png", "Nützliche Informationen", "Der Chagrian hat nützliche Informationen über das Planetensystem in dem sich den Frachter King Creole befindet.");
			fp1.Controls.Add(mc);

			var mc2 = new DistressCallControl();
			mc2.SetMessage("People\\Feeorin.png", "Wir brauchen Hilfe!", "Wir wachen auf dem Weg zum Planetensystem Betageuze und wurden überfallen!\n" +
				"Wer in der Nähe ist und uns unterstützen kann soll bitte schnell vorbeikommen!\n" +
				"Unsere Koordinaten sind [17,8,12].");
			fp1.Controls.Add(mc2);

			var mc3 = new FoundNewPlanetControl();
			mc3.SetMessage("Planets\\image_part_017.jpg", "Neuer Planet entdeckt", "Dein Frachter King Creole hat einen neuen Planeten entdeckt.");
			fp1.Controls.Add(mc3);

			var c = new ShipFeatureControl();
			shC.Controls.Add(c);


			var c2 = new ShipFeatureControl();
			shC.Controls.Add(c2);


		}

	}
}
