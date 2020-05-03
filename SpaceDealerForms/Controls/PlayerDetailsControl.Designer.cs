namespace SpaceDealerForms.Controls
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
			this.lblShipName = new System.Windows.Forms.Label();
			this.ColorCodeBox = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblCredits = new System.Windows.Forms.Label();
			this.lblCargoSize = new System.Windows.Forms.Label();
			this.shipFp1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblShipName
			// 
			this.lblShipName.AutoSize = true;
			this.lblShipName.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblShipName.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblShipName.Location = new System.Drawing.Point(164, 17);
			this.lblShipName.Name = "lblShipName";
			this.lblShipName.Size = new System.Drawing.Size(210, 22);
			this.lblShipName.TabIndex = 2;
			this.lblShipName.Text = "Name: USS Enterprise";
			this.lblShipName.Click += new System.EventHandler(this.lblShipName_Click);
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
			this.lblCargoSize.AutoSize = true;
			this.lblCargoSize.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCargoSize.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblCargoSize.Location = new System.Drawing.Point(164, 89);
			this.lblCargoSize.Name = "lblCargoSize";
			this.lblCargoSize.Size = new System.Drawing.Size(150, 22);
			this.lblCargoSize.TabIndex = 9;
			this.lblCargoSize.Text = "Rang: Fähnrich";
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
			this.Controls.Add(this.lblCargoSize);
			this.Controls.Add(this.lblCredits);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.ColorCodeBox);
			this.Controls.Add(this.lblShipName);
			this.Name = "PlayerDetailsControl";
			this.Size = new System.Drawing.Size(656, 403);
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblShipName;
		private System.Windows.Forms.PictureBox ColorCodeBox;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblCredits;
		private System.Windows.Forms.Label lblCargoSize;
		private System.Windows.Forms.FlowLayoutPanel shipFp1;
		private System.Windows.Forms.Label label2;
	}
}
