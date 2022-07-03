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
	public partial class DistressCallControl : UserControl
	{
		private Coordinates _position { get; set; }
		public void SetMessage(string imagePath, string headline, string subHeadline, Coordinates position)
		{
			this.pictureBox1.Load(imagePath);
			lblHeadline.Text = headline;
			lblSubHeadline.Text = subHeadline;
			_position = position;
		}

		public DistressCallControl()
		{
			InitializeComponent();
		}

		private void lblHeadline_Click(object sender, EventArgs e)
		{

		}

        private async void btnHelp_Click(object sender, EventArgs e)
        {
			//BattleReply result = await GameProxy.BattleAttack(Program.CurrentPlayer.Name, Program.CurrentShip.ShipName);
			//lblSubHeadline.Text += result.Message + "\n";
			var result = await GameProxy.CruiseToLocation(Program.CurrentPlayer.Name, Program.CurrentShip.ShipName, _position);
			if (result == true)
			{
				
			}
		}

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
