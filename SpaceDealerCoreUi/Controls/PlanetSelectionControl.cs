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
	public partial class PlanetSelectionControl : UserControl
	{
		private Planet _SelectedPlanet { get; set; }

		public PlanetSelectionControl()
		{
			InitializeComponent();
		}
		public void Init(Planet planet)
		{
			_SelectedPlanet = planet;
			lbPlanetName.Text = $"Name: {planet.PlanetName}";
			lblPosition.Text = $"Position: {planet.ToPositiontring()}";
			pictureBox1.Load(Program.PlanetAssets.GetRandomAsset().Path);
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			Program.MainForm.StartCruise(_SelectedPlanet);
		}
	}
}
