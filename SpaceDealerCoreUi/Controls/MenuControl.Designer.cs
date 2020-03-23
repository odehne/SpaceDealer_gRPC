using System;

namespace SpaceDealerCoreUi.Controls
{
	partial class MenuControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ColorCodeBox = new System.Windows.Forms.PictureBox();
			this.btnNewGame = new System.Windows.Forms.Button();
			this.btnLoadGame = new System.Windows.Forms.Button();
			this.btnShowAllPlanets = new System.Windows.Forms.Button();
			this.btnShowAllShips = new System.Windows.Forms.Button();
			this.btnShowPlayer = new System.Windows.Forms.Button();
			this.btnShowCurrentShip = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).BeginInit();
			this.SuspendLayout();
			// 
			// ColorCodeBox
			// 
			this.ColorCodeBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.ColorCodeBox.Location = new System.Drawing.Point(0, 0);
			this.ColorCodeBox.Name = "ColorCodeBox";
			this.ColorCodeBox.Size = new System.Drawing.Size(12, 106);
			this.ColorCodeBox.TabIndex = 4;
			this.ColorCodeBox.TabStop = false;
			// 
			// btnNewGame
			// 
			this.btnNewGame.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnNewGame.Location = new System.Drawing.Point(18, 13);
			this.btnNewGame.Name = "btnNewGame";
			this.btnNewGame.Size = new System.Drawing.Size(142, 33);
			this.btnNewGame.TabIndex = 5;
			this.btnNewGame.Text = "Neues Spiel";
			this.btnNewGame.UseVisualStyleBackColor = true;
			this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
			// 
			// btnLoadGame
			// 
			this.btnLoadGame.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnLoadGame.Location = new System.Drawing.Point(18, 52);
			this.btnLoadGame.Name = "btnLoadGame";
			this.btnLoadGame.Size = new System.Drawing.Size(142, 33);
			this.btnLoadGame.TabIndex = 6;
			this.btnLoadGame.Text = "Spiel laden";
			this.btnLoadGame.UseVisualStyleBackColor = true;
			this.btnLoadGame.Click += new System.EventHandler(btnLoadGame_Click);
			// 
			// btnShowAllPlanets
			// 
			this.btnShowAllPlanets.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnShowAllPlanets.Location = new System.Drawing.Point(183, 13);
			this.btnShowAllPlanets.Name = "btnShowAllPlanets";
			this.btnShowAllPlanets.Size = new System.Drawing.Size(142, 33);
			this.btnShowAllPlanets.TabIndex = 7;
			this.btnShowAllPlanets.Text = "Alle Planeten";
			this.btnShowAllPlanets.UseVisualStyleBackColor = true;
			this.btnShowAllPlanets.Click += new System.EventHandler(this.btnShowAllPlanets_Click);
			// 
			// btnShowAllShips
			// 
			this.btnShowAllShips.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnShowAllShips.Location = new System.Drawing.Point(183, 52);
			this.btnShowAllShips.Name = "btnShowAllShips";
			this.btnShowAllShips.Size = new System.Drawing.Size(142, 33);
			this.btnShowAllShips.TabIndex = 8;
			this.btnShowAllShips.Text = "Alle Shiffe";
			this.btnShowAllShips.UseVisualStyleBackColor = true;
			this.btnShowAllShips.Click += new System.EventHandler(btnShowAllShips_Click);
			// 
			// btnShowPlayer
			// 
			this.btnShowPlayer.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnShowPlayer.Location = new System.Drawing.Point(346, 13);
			this.btnShowPlayer.Name = "btnShowPlayer";
			this.btnShowPlayer.Size = new System.Drawing.Size(176, 33);
			this.btnShowPlayer.TabIndex = 9;
			this.btnShowPlayer.Text = "Spieler Übersicht";
			this.btnShowPlayer.UseVisualStyleBackColor = true;
			this.btnShowPlayer.Click += new System.EventHandler(this.btnShowPlayer_Click);
			// 
			// btnShowCurrentShip
			// 
			this.btnShowCurrentShip.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnShowCurrentShip.Location = new System.Drawing.Point(346, 52);
			this.btnShowCurrentShip.Name = "btnShowCurrentShip";
			this.btnShowCurrentShip.Size = new System.Drawing.Size(176, 33);
			this.btnShowCurrentShip.TabIndex = 10;
			this.btnShowCurrentShip.Text = "Schiff Übersicht";
			this.btnShowCurrentShip.UseVisualStyleBackColor = true;
			this.btnShowCurrentShip.Click += new System.EventHandler(this.btnShowCurrentShip_Click);
			// 
			// MenuControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.btnShowCurrentShip);
			this.Controls.Add(this.btnShowPlayer);
			this.Controls.Add(this.btnShowAllShips);
			this.Controls.Add(this.btnShowAllPlanets);
			this.Controls.Add(this.btnLoadGame);
			this.Controls.Add(this.btnNewGame);
			this.Controls.Add(this.ColorCodeBox);
			this.Name = "MenuControl";
			this.Size = new System.Drawing.Size(656, 106);
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).EndInit();
			this.ResumeLayout(false);

		}


		#endregion

		private System.Windows.Forms.PictureBox ColorCodeBox;
		private System.Windows.Forms.Button btnNewGame;
		private System.Windows.Forms.Button btnLoadGame;
		private System.Windows.Forms.Button btnShowAllPlanets;
		private System.Windows.Forms.Button btnShowAllShips;
		private System.Windows.Forms.Button btnShowPlayer;
		private System.Windows.Forms.Button btnShowCurrentShip;
	}
}
