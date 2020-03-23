namespace SpaceDealerForms.Controls
{
	partial class PlanetSelectionControl
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
			this.lbPlanetName = new System.Windows.Forms.Label();
			this.ColorCodeBox = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblPosition = new System.Windows.Forms.Label();
			this.btnHelp = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// lbPlanetName
			// 
			this.lbPlanetName.AutoSize = true;
			this.lbPlanetName.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbPlanetName.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lbPlanetName.Location = new System.Drawing.Point(164, 17);
			this.lbPlanetName.Name = "lbPlanetName";
			this.lbPlanetName.Size = new System.Drawing.Size(210, 22);
			this.lbPlanetName.TabIndex = 2;
			this.lbPlanetName.Text = "Name: USS Enterprise";
			// 
			// ColorCodeBox
			// 
			this.ColorCodeBox.BackColor = System.Drawing.Color.LightSkyBlue;
			this.ColorCodeBox.Location = new System.Drawing.Point(0, 0);
			this.ColorCodeBox.Name = "ColorCodeBox";
			this.ColorCodeBox.Size = new System.Drawing.Size(12, 143);
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
			// lblPosition
			// 
			this.lblPosition.AutoSize = true;
			this.lblPosition.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPosition.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblPosition.Location = new System.Drawing.Point(164, 53);
			this.lblPosition.Name = "lblPosition";
			this.lblPosition.Size = new System.Drawing.Size(230, 22);
			this.lblPosition.TabIndex = 8;
			this.lblPosition.Text = "Position: Erde [0,0,0]";
			// 
			// btnHelp
			// 
			this.btnHelp.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnHelp.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.btnHelp.Location = new System.Drawing.Point(449, 96);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(192, 33);
			this.btnHelp.TabIndex = 11;
			this.btnHelp.Text = "Setzen Sie Kurs!";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// PlanetSelectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.lblPosition);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.ColorCodeBox);
			this.Controls.Add(this.lbPlanetName);
			this.Name = "PlanetSelectionControl";
			this.Size = new System.Drawing.Size(656, 143);
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbPlanetName;
		private System.Windows.Forms.PictureBox ColorCodeBox;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblPosition;
		private System.Windows.Forms.Button btnHelp;
	}
}
