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
	public partial class UnderAttackControl : UserControl
	{
		public void SetMessage(string imagePath, string headline, string subHeadline)
		{
			this.pictureBox1.Load(imagePath);
			lblHeadline.Text = headline;
			lblSubHeadline.Text = subHeadline;
		}

		public UnderAttackControl()
		{
			InitializeComponent();
		}

		private void lblHeadline_Click(object sender, EventArgs e)
		{

		}

        private async void btnAttack_Click(object sender, EventArgs e)
        {
			BattleReply result = await GameProxy.BattleAttack(Program.CurrentPlayer.Name, Program.CurrentShip.ShipName);

			lblSubHeadline.Text += result.Message + "\n";

			//if (result.Message )
			//{
			//	var msg = new TravellingControl();
			//	msg.SetMessage(Program.AnimationAssets[0].Path, "Neuer Kurs", $"Neuer Kurs nach {destination.ToPlanetPosition()} liegt an, sir!");
			//	fp1.Controls.Add(msg);
			//}
		}
    }
}
