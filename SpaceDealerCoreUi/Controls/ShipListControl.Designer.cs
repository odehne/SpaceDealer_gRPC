namespace SpaceDealerCoreUi.Controls
{
	partial class ShipListControl
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblShipNameAndState = new System.Windows.Forms.Label();
			this.lblPositionAndFreight = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(17, 18);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(63, 60);
			this.pictureBox1.BackColor = System.Drawing.Color.Black;
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 9;
			this.pictureBox1.TabStop = false;
			// 
			// lblShipNameAndState
			// 
			this.lblShipNameAndState.AutoSize = true;
			this.lblShipNameAndState.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblShipNameAndState.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblShipNameAndState.Location = new System.Drawing.Point(95, 17);
			this.lblShipNameAndState.Name = "lblShipNameAndState";
			this.lblShipNameAndState.Size = new System.Drawing.Size(410, 22);
			this.lblShipNameAndState.TabIndex = 8;
			this.lblShipNameAndState.Text = "Name: USS Enterprise (Status: Unterwegs)";
			// 
			// lblPositionAndFreight
			// 
			this.lblPositionAndFreight.AutoSize = true;
			this.lblPositionAndFreight.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPositionAndFreight.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblPositionAndFreight.Location = new System.Drawing.Point(95, 55);
			this.lblPositionAndFreight.Name = "lblPositionAndFreight";
			this.lblPositionAndFreight.Size = new System.Drawing.Size(410, 22);
			this.lblPositionAndFreight.TabIndex = 10;
			this.lblPositionAndFreight.Text = "Position: Erde [0,0,1] Ladung: 10t / 30t";
			// 
			// ShipListControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Controls.Add(this.lblPositionAndFreight);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.lblShipNameAndState);
			this.Name = "ShipListControl";
			this.Size = new System.Drawing.Size(593, 92);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblShipNameAndState;
		private System.Windows.Forms.Label lblPositionAndFreight;
	}
}
