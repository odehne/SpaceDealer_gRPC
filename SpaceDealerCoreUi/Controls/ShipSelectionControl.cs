using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SpaceDealerService;

namespace SpaceDealerCoreUi.Controls
{
	public partial class ShipSelectionControl : UserControl
	{
		public ShipSelectionControl()
		{
			InitializeComponent();
		}
		public void Init(Ship ship)
		{
			lbPlanetName.Text = $"Name: {ship.ShipName}";
			lblPosition.Text = $"Position: {ship.CurrentPlanet.ToPlanetPosition()}";
			pictureBox1.Load(".\\Spaceships\\Collector.jpg");
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{

		}
	}
}
