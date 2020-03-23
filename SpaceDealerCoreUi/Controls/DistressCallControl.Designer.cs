namespace SpaceDealerCoreUi.Controls
{
	partial class DistressCallControl
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
			this.ColorCodeBox = new System.Windows.Forms.PictureBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(23, 22);
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
			this.lblHeadline.Click += new System.EventHandler(this.lblHeadline_Click);
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
			// ColorCodeBox
			// 
			this.ColorCodeBox.BackColor = System.Drawing.Color.OrangeRed;
			this.ColorCodeBox.Location = new System.Drawing.Point(0, 0);
			this.ColorCodeBox.Name = "ColorCodeBox";
			this.ColorCodeBox.Size = new System.Drawing.Size(12, 180);
			this.ColorCodeBox.TabIndex = 3;
			this.ColorCodeBox.TabStop = false;
			// 
			// btnHelp
			// 
			this.btnHelp.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.btnHelp.Location = new System.Drawing.Point(396, 133);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(142, 33);
			this.btnHelp.TabIndex = 4;
			this.btnHelp.Text = "Wir helfen!";
			this.btnHelp.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(149, 133);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(207, 33);
			this.button1.TabIndex = 5;
			this.button1.Text = "Leider zu weit weg";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// DistressCallControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.ColorCodeBox);
			this.Controls.Add(this.lblSubHeadline);
			this.Controls.Add(this.lblHeadline);
			this.Controls.Add(this.pictureBox1);
			this.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.Name = "DistressCallControl";
			this.Size = new System.Drawing.Size(562, 180);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblHeadline;
		private System.Windows.Forms.TextBox lblSubHeadline;
		private System.Windows.Forms.PictureBox ColorCodeBox;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Button button1;
	}
}
