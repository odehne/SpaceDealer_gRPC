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
	public partial class PlayerDetailsControl : UserControl
	{
		public PlayerDetailsControl()
		{
			InitializeComponent();
		}

		public void Init(Player player)
		{
			lblPlayerName.Text = $"Name: {player.Name}";
			lblCredits.Text = $"Credits: ${player.Credits}";
			lblRank.Text = $"Rang: Fähnrich";

			
			pictureBox1.Load(player.PicturePath);

			foreach (var ship in player.Ships)
			{
				var ctl = new ShipListControl();
				ctl.Init(ship);
				shipFp1.Controls.Add(ctl);
			}
		}
	}
}
