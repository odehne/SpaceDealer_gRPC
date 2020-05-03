using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SpaceDealerCoreUi.Controls
{
	public partial class MenuControl : UserControl
	{
		public MenuControl()
		{
			InitializeComponent();
		}


		private void btnShowCurrentShip_Click(object sender, EventArgs e)
		{
			Program.MainForm.ShowShip();
		}

		private void btnShowAllPlanets_Click(object sender, EventArgs e)
		{
			Program.MainForm.ShowPlanetSelection();
		}

		private void btnShowAllShips_Click(object sender, EventArgs e)
		{
			Program.MainForm.ShowShipSelection();
		}

		private void btnShowPlayer_Click(object sender, EventArgs e)
		{
			Program.MainForm.ShowPlayerStats();
		}

		private void btnNewGame_Click(object sender, EventArgs e)
		{
			Program.MainForm.NewGame();
		}

		private void btnLoadGame_Click(object sender, EventArgs e)
		{
			Program.MainForm.LoadPlayer();
			Program.MainForm.ShowShip();
		}

	
	}
}
