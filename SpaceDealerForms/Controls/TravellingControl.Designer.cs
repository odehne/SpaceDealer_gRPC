namespace SpaceDealerForms.Controls
{
	partial class TravellingControl
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
			this.lblHeadline = new System.Windows.Forms.Label();
			this.lblSubHeadline = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(16, 15);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(112, 112);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// lblHeadline
			// 
			this.lblHeadline.AutoSize = true;
			this.lblHeadline.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeadline.Location = new System.Drawing.Point(145, 20);
			this.lblHeadline.Name = "lblHeadline";
			this.lblHeadline.Size = new System.Drawing.Size(240, 22);
			this.lblHeadline.TabIndex = 1;
			this.lblHeadline.Text = "Wir werden angegriffen!";
			// 
			// lblSubHeadline
			// 
			this.lblSubHeadline.Location = new System.Drawing.Point(149, 45);
			this.lblSubHeadline.Multiline = true;
			this.lblSubHeadline.Name = "lblSubHeadline";
			this.lblSubHeadline.ReadOnly = true;
			this.lblSubHeadline.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.lblSubHeadline.Size = new System.Drawing.Size(389, 82);
			this.lblSubHeadline.TabIndex = 2;
			// 
			// MessageControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lblSubHeadline);
			this.Controls.Add(this.lblHeadline);
			this.Controls.Add(this.pictureBox1);
			this.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.Name = "MessageControl";
			this.Size = new System.Drawing.Size(562, 140);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblHeadline;
		private System.Windows.Forms.TextBox lblSubHeadline;
	}
}
