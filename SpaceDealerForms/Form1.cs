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
			var mc1 = new MessageControl();
			mc1.SetMessage("Planets\\image_part_023.jpg", "Neuer Planet entdeckt", "Dein Frachter King Creole hat einen neuen Planeten entdeckt.");
			flowLayoutPanel1.Controls.Add(mc1);

			var mc = new MessageControl();
			mc.SetMessage("People\\Chagrian.png", "Nützliche Informationen", "Der Chagrian hat nützliche Informationen über das Planetensystem in dem sich den Frachter King Creole befindet.");
			flowLayoutPanel1.Controls.Add(mc);

			var mc2 = new MessageControl();
			mc2.SetMessage("People\\Feeorin.png", "Nützliche Informationen", "Der Chagrian hat nützliche Informationen über das Planetensystem in dem sich den Frachter King Creole befindet.");
			flowLayoutPanel1.Controls.Add(mc2);

			var mc3 = new MessageControl();
			mc3.SetMessage("Planets\\image_part_017.jpg", "Neuer Planet entdeckt", "Dein Frachter King Creole hat einen neuen Planeten entdeckt.");
			flowLayoutPanel1.Controls.Add(mc3);

		}
	}
}
