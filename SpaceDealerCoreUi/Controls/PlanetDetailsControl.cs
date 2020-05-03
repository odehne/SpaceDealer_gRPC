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
	public partial class PlanetDetailsControl : UserControl
	{
		public PlanetDetailsControl()
		{
			InitializeComponent();
		}

		public void Init(Planet planet)
		{
			lblShipName.Text = $"Name: {planet.PlanetName}";
			lblCurrentPosition.Text = $"Position: {planet.ToPlanetPosition()}";
			//lblCargoSize.Text = $"Ladekapazität: {planet.}t";

			pictureBox1.Load(Program.PlanetAssets.GetRandomAsset().Path);

			foreach (var product in planet.Industries[0].GeneratedProducts)
			{
				var cargo = new CargoControl();
				cargo.Init(product);
				shipFp1.Controls.Add(cargo);
			}

			foreach (var product in planet.Industries[0].ProductsNeeded)
			{
				var cargo = new CargoControl();
				cargo.Init(product);
				shipFp2.Controls.Add(cargo);
			}

		}
	}
}
