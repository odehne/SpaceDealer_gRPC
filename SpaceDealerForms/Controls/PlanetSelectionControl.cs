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
	public partial class PlanetSelectionControl : UserControl
	{
		public PlanetSelectionControl()
		{
			InitializeComponent();
		}

		public void Init(Planet planet)
		{
			lbPlanetName.Text = $"Name: {planet.PlanetName}";
			lblPosition.Text = $"Position: {planet.Sector}";
			pictureBox1.Load(".\\Spaceships\\mediumFrighter.jpg");
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{

		}
	}
}
