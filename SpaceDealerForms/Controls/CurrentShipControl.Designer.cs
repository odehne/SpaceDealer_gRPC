namespace SpaceDealerForms.Controls
{
	partial class CurrentShipControl
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
			this.lblCurrentPosition = new System.Windows.Forms.Label();
			this.lblCargoSize = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.shipFp1 = new System.Windows.Forms.FlowLayoutPanel();
			this.shipFp2 = new System.Windows.Forms.FlowLayoutPanel();
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
			// 
			// ColorCodeBox
			// 
			this.ColorCodeBox.BackColor = System.Drawing.Color.LightSkyBlue;
			this.ColorCodeBox.Location = new System.Drawing.Point(0, 0);
			this.ColorCodeBox.Name = "ColorCodeBox";
			this.ColorCodeBox.Size = new System.Drawing.Size(12, 588);
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
			// lblCurrentPosition
			// 
			this.lblCurrentPosition.AutoSize = true;
			this.lblCurrentPosition.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCurrentPosition.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblCurrentPosition.Location = new System.Drawing.Point(164, 53);
			this.lblCurrentPosition.Name = "lblCurrentPosition";
			this.lblCurrentPosition.Size = new System.Drawing.Size(230, 22);
			this.lblCurrentPosition.TabIndex = 8;
			this.lblCurrentPosition.Text = "Position: Erde [0,0,0]";
			// 
			// lblCargoSize
			// 
			this.lblCargoSize.AutoSize = true;
			this.lblCargoSize.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCargoSize.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblCargoSize.Location = new System.Drawing.Point(164, 89);
			this.lblCargoSize.Name = "lblCargoSize";
			this.lblCargoSize.Size = new System.Drawing.Size(190, 22);
			this.lblCargoSize.TabIndex = 9;
			this.lblCargoSize.Text = "Ladekapazität: 30t";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.label1.Location = new System.Drawing.Point(26, 153);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(110, 22);
			this.label1.TabIndex = 10;
			this.label1.Text = "Ausrüstung";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.label2.Location = new System.Drawing.Point(344, 153);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 22);
			this.label2.TabIndex = 11;
			this.label2.Text = "Ladung";
			// 
			// shipFp1
			// 
			this.shipFp1.AutoScroll = true;
			this.shipFp1.WrapContents = false;
			this.shipFp1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.shipFp1.BackColor = System.Drawing.Color.Lavender;
			this.shipFp1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.shipFp1.Location = new System.Drawing.Point(30, 178);
			this.shipFp1.Margin = new System.Windows.Forms.Padding(0);
			this.shipFp1.Name = "shipFp1";
			this.shipFp1.Size = new System.Drawing.Size(295, 211);
			this.shipFp1.TabIndex = 12;
			// 
			// shipFp2
			// 
			this.shipFp2.AutoScroll = true;
			this.shipFp2.AutoSizeMode=System.Windows.Forms.AutoSizeMode.GrowOnly;
			this.shipFp2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.shipFp2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.shipFp2.Location = new System.Drawing.Point(348, 178);
			this.shipFp2.Margin = new System.Windows.Forms.Padding(0);
			this.shipFp2.Name = "shipFp2";
			this.shipFp2.Size = new System.Drawing.Size(295, 211);
			this.shipFp2.TabIndex = 13;
			this.shipFp2.WrapContents = false;

			// 
			// CurrentShipControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.shipFp2);
			this.Controls.Add(this.shipFp1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblCargoSize);
			this.Controls.Add(this.lblCurrentPosition);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.ColorCodeBox);
			this.Controls.Add(this.lblShipName);
			this.Name = "CurrentShipControl";
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
		private System.Windows.Forms.Label lblCurrentPosition;
		private System.Windows.Forms.Label lblCargoSize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.FlowLayoutPanel shipFp1;
		private System.Windows.Forms.FlowLayoutPanel shipFp2;
	}
}
