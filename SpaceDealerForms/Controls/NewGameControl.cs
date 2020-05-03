using System;
using System.Windows.Forms;

namespace SpaceDealerForms.Controls
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

		private void btnNewGame_Click(object sender, EventArgs e)
		{
			
		}
	}
}
