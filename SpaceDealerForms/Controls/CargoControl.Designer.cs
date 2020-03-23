namespace SpaceDealerForms.Controls
{
	partial class CargoControl
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
			this.lblProductName = new System.Windows.Forms.Label();
			this.lblWeight = new System.Windows.Forms.Label();
			this.lblPrice = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(9, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(70, 70);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 10;
			this.pictureBox1.TabStop = false;
			// 
			// lblProductName
			// 
			this.lblProductName.AutoSize = true;
			this.lblProductName.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProductName.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblProductName.Location = new System.Drawing.Point(89, 3);
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.Size = new System.Drawing.Size(90, 19);
			this.lblProductName.TabIndex = 9;
			this.lblProductName.Text = "Kuh-Milch";
			// 
			// lblWeight
			// 
			this.lblWeight.AutoSize = true;
			this.lblWeight.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWeight.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblWeight.Location = new System.Drawing.Point(90, 28);
			this.lblWeight.Name = "lblWeight";
			this.lblWeight.Size = new System.Drawing.Size(99, 19);
			this.lblWeight.TabIndex = 11;
			this.lblWeight.Text = "Menge: 10t";
			// 
			// lblPrice
			// 
			this.lblPrice.AutoSize = true;
			this.lblPrice.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPrice.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblPrice.Location = new System.Drawing.Point(90, 55);
			this.lblPrice.Name = "lblPrice";
			this.lblPrice.Size = new System.Drawing.Size(117, 19);
			this.lblPrice.TabIndex = 12;
			this.lblPrice.Text = "Preis: $1024";
			// 
			// CargoControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lblPrice);
			this.Controls.Add(this.lblWeight);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.lblProductName);
			this.Name = "CargoControl";
			this.Size = new System.Drawing.Size(295, 87);
			this.Load += new System.EventHandler(this.CargoControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblProductName;
		private System.Windows.Forms.Label lblWeight;
		private System.Windows.Forms.Label lblPrice;
	}
}
