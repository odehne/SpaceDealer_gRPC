using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceDealerService;

namespace SpaceDealerForms.Controls
{
	public partial class PlayerDetailsControl : UserControl
	{
		public PlayerDetailsControl()
		{
			InitializeComponent();
		}

		public void Init(Ship ship)
		{
			lblShipName.Text = $"Name: {ship.ShipName}";
			lblCredits.Text = $"Position: {ship.CurrentPlanet.PlanetName}";
			lblCargoSize.Text = $"Ladekapazität: {ship.CargoSize}t";

			pictureBox1.Load(".\\Spaceships\\mediumFrighter.jpg");

			foreach (var product in ship.CargoLoad.LoadedProducts)
			{
				var cargo = new CargoControl();
				cargo.Init(product);
				//shipFp2.Controls.Add(cargo);
			}

			var shields = new ShipFeatureControl();
			shields.Init("Schilde (max. 3)", $"Aktueller Wert: {ship.Shields}");
			shipFp1.Controls.Add(shields);
			var hull = new ShipFeatureControl();
			hull.Init("Hülle (max. 3)", $"Aktueller Wert: {ship.Hull}");
			shipFp1.Controls.Add(hull);

		}

		private void lblShipName_Click(object sender, EventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}
}
