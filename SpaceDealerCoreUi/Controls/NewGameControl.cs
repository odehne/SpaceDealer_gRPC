using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceDealerCoreUi.Controls
{
	public partial class NewGameControl : UserControl
	{
		public NewGameControl()
		{
			InitializeComponent();
		}

		public void Init()
		{
			ClearFrames();
			pictureBox1.Load(".\\people\\Abyssin.png");
			pictureBox2.Load(".\\people\\Aqualish.png");
			pictureBox3.Load(".\\people\\Eowk.png");
			pictureBox4.Load(".\\people\\Khil.png");
			pictureBox5.Load(".\\people\\Lurmen.png");
			pictureBox6.Load(".\\people\\Chiss.png");
		}

		private void ClearFrames()
		{
			frame1.Visible = false;
			frame2.Visible = false;
			frame3.Visible = false;
			frame4.Visible = false;
			frame5.Visible = false;
			frame6.Visible = false;
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			ClearFrames();
			frame1.Visible = true;
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			ClearFrames(); 
			frame2.Visible = true;
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			ClearFrames();
			frame3.Visible = true;
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			ClearFrames();
			frame4.Visible = true;
		}

		private void pictureBox5_Click(object sender, EventArgs e)
		{
			ClearFrames();
			frame5.Visible = true;
		}

		private void pictureBox6_Click(object sender, EventArgs e)
		{
			ClearFrames();
			frame6.Visible = true;
		}

		private async void btnNewGame_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(txtPlayerName.Text))
			{
				picNameCheck.Load(".\\Other\\failed.png");
				return;
			}
			if (string.IsNullOrEmpty(txtShipName.Text))
			{
				picNameCheck.Load(".\\Other\\failed.png");
				return;
			}

			var playerNameExists = await GameProxy.PlayerNameTaken(txtPlayerName.Text);
			if (playerNameExists == true)
			{
				picNameCheck.Load(".\\Other\\failed.png");
				return;
			}
			else
			{
				picNameCheck.Load(".\\Other\\ok.png");
			}

			var shipNameExists = await GameProxy.ShipNameTaken(txtPlayerName.Text, txtShipName.Text);
			if (shipNameExists == true)
			{
				picShipName.Load(".\\Other\\failed.png");
				return;
			}
			else
			{
				picShipName.Load(".\\Other\\ok.png");
			}
			Program.CurrentPlayer = await GameProxy.AddPlayer(txtPlayerName.Text, GetSelectedPicturePath());
			var ship = await GameProxy.AddShip(Program.CurrentPlayer.Name, txtShipName.Text);
			var result = await GameProxy.SaveGame(Program.CurrentPlayer.Name);
			Program.CurrentPlayer.Ships.Add(ship);
			Program.MainForm.ShowPlayerStats();
		}

		private string GetSelectedPicturePath()
		{
			if (frame1.Visible == true)
			return ".\\people\\Abyssin.png";
			if (frame2.Visible == true)
				return ".\\people\\Aqualish.png";
			if (frame3.Visible == true)
				return ".\\people\\Eowk.png";
			if (frame4.Visible == true)
				return ".\\people\\Khil.png";
			if (frame5.Visible == true)
				return ".\\people\\Lurmen.png";
			if (frame6.Visible == true)
				return ".\\people\\Chiss.png";
			return ".\\people\\Chiss.png";
		}
	}
}
