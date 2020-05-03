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

namespace SpaceDealerCoreUi.Controls
{
	public partial class CurrentShipControl : UserControl
	{
		public CurrentShipControl()
		{
			InitializeComponent();
		}

		public void Init(Ship ship)
		{
			lblShipName.Text = $"Name: {ship.ShipName}";
			lblCurrentPosition.Text = $"Position: {ship.CurrentPlanet.ToPlanetPosition()}";
			lblCargoSize.Text = $"Ladekapazität: {ship.CargoSize}t";

			pictureBox1.Load(ship.PicturePath);

			foreach (var product in ship.CargoLoad.LoadedProducts)
			{
				var cargo = new CargoControl();
				cargo.Init(product);
				shipFp2.Controls.Add(cargo);
			}

			var shields = new ShipFeatureControl();
			shields.Init("Schilde (max. 3)", $"Aktueller Wert: {ship.Shields}");
			shipFp1.Controls.Add(shields);
			var hull = new ShipFeatureControl();
			hull.Init("Hülle (max. 3)", $"Aktueller Wert: {ship.Hull}");
			shipFp1.Controls.Add(hull);

		}
	}
}
