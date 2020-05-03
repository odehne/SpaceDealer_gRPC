namespace SpaceDealerForms.Controls
{
	partial class InformantControl
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
			this.btnHelp = new System.Windows.Forms.Button();
			this.ColorCodeBox = new System.Windows.Forms.PictureBox();
			this.lblHeadline = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblSubHeadline = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnHelp
			// 
			this.btnHelp.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnHelp.Location = new System.Drawing.Point(346, 133);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(192, 33);
			this.btnHelp.TabIndex = 15;
			this.btnHelp.Text = "Auf den Schirm!";
			this.btnHelp.UseVisualStyleBackColor = true;
			// 
			// ColorCodeBox
			// 
			this.ColorCodeBox.BackColor = System.Drawing.Color.Goldenrod;
			this.ColorCodeBox.Location = new System.Drawing.Point(0, 0);
			this.ColorCodeBox.Name = "ColorCodeBox";
			this.ColorCodeBox.Size = new System.Drawing.Size(12, 180);
			this.ColorCodeBox.TabIndex = 14;
			this.ColorCodeBox.TabStop = false;
			// 
			// lblHeadline
			// 
			this.lblHeadline.AutoSize = true;
			this.lblHeadline.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeadline.Location = new System.Drawing.Point(145, 20);
			this.lblHeadline.Name = "lblHeadline";
			this.lblHeadline.Size = new System.Drawing.Size(410, 22);
			this.lblHeadline.TabIndex = 12;
			this.lblHeadline.Text = "Wir haben einen neuen Planeten entdeckt!";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(23, 22);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(112, 112);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 11;
			this.pictureBox1.TabStop = false;
			// 
			// lblSubHeadline
			// 
			this.lblSubHeadline.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
			this.lblSubHeadline.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblSubHeadline.Location = new System.Drawing.Point(147, 45);
			this.lblSubHeadline.Name = "lblSubHeadline";
			this.lblSubHeadline.Size = new System.Drawing.Size(389, 75);
			this.lblSubHeadline.TabIndex = 11;
			this.lblSubHeadline.Text = "Name: USS ";
			// 
			// InformantControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lblSubHeadline);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.ColorCodeBox);
			this.Controls.Add(this.lblHeadline);
			this.Controls.Add(this.pictureBox1);
			this.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.Name = "InformantControl";
			this.Size = new System.Drawing.Size(562, 180);
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.PictureBox ColorCodeBox;
		private System.Windows.Forms.Label lblHeadline;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblSubHeadline;
	}
}
