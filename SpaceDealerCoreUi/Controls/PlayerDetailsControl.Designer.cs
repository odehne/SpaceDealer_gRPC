namespace SpaceDealerCoreUi.Controls
{
	partial class PlayerDetailsControl
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
			this.lblPlayerName = new System.Windows.Forms.Label();
			this.ColorCodeBox = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblCredits = new System.Windows.Forms.Label();
			this.lblRank = new System.Windows.Forms.Label();
			this.shipFp1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblShipName
			// 
			this.lblPlayerName.AutoSize = true;
			this.lblPlayerName.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPlayerName.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblPlayerName.Location = new System.Drawing.Point(164, 17);
			this.lblPlayerName.Name = "lblShipName";
			this.lblPlayerName.Size = new System.Drawing.Size(210, 22);
			this.lblPlayerName.TabIndex = 2;
			this.lblPlayerName.Text = "Name: USS Enterprise";
			// 
			// ColorCodeBox
			// 
			this.ColorCodeBox.BackColor = System.Drawing.Color.Firebrick;
			this.ColorCodeBox.Location = new System.Drawing.Point(0, 0);
			this.ColorCodeBox.Name = "ColorCodeBox";
			this.ColorCodeBox.Size = new System.Drawing.Size(12, 403);
			this.ColorCodeBox.TabIndex = 4;
			this.ColorCodeBox.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(30, 17);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(112, 112);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 7;
			this.pictureBox1.TabStop = false;
			// 
			// lblCredits
			// 
			this.lblCredits.AutoSize = true;
			this.lblCredits.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCredits.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblCredits.Location = new System.Drawing.Point(164, 53);
			this.lblCredits.Name = "lblCredits";
			this.lblCredits.Size = new System.Drawing.Size(160, 22);
			this.lblCredits.TabIndex = 8;
			this.lblCredits.Text = "Credits: $12000";
			// 
			// lblCargoSize
			// 
			this.lblRank.AutoSize = true;
			this.lblRank.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRank.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblRank.Location = new System.Drawing.Point(164, 89);
			this.lblRank.Name = "lblCargoSize";
			this.lblRank.Size = new System.Drawing.Size(150, 22);
			this.lblRank.TabIndex = 9;
			this.lblRank.Text = "Rang: Fähnrich";
			// 
			// shipFp1
			// 
			this.shipFp1.AutoScroll = true;
			this.shipFp1.BackColor = System.Drawing.Color.Lavender;
			this.shipFp1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.shipFp1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.shipFp1.Location = new System.Drawing.Point(30, 169);
			this.shipFp1.Margin = new System.Windows.Forms.Padding(0);
			this.shipFp1.Name = "shipFp1";
			this.shipFp1.Size = new System.Drawing.Size(602, 211);
			this.shipFp1.TabIndex = 14;
			this.shipFp1.WrapContents = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.label2.Location = new System.Drawing.Point(26, 144);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 22);
			this.label2.TabIndex = 13;
			this.label2.Text = "Schiffe";
			// 
			// PlayerDetailsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.shipFp1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblRank);
			this.Controls.Add(this.lblCredits);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.ColorCodeBox);
			this.Controls.Add(this.lblPlayerName);
			this.Name = "PlayerDetailsControl";
			this.Size = new System.Drawing.Size(656, 403);
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPlayerName;
		private System.Windows.Forms.PictureBox ColorCodeBox;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblCredits;
		private System.Windows.Forms.Label lblRank;
		private System.Windows.Forms.FlowLayoutPanel shipFp1;
		private System.Windows.Forms.Label label2;
	}
}
