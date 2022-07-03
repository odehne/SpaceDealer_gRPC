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
            this.lblSubHeadline = new System.Windows.Forms.Label();
            this.ColorCodeBox = new System.Windows.Forms.PictureBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(27, 25);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(131, 129);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblHeadline
            // 
            this.lblHeadline.AutoSize = true;
            this.lblHeadline.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeadline.Location = new System.Drawing.Point(169, 23);
            this.lblHeadline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeadline.Name = "lblHeadline";
            this.lblHeadline.Size = new System.Drawing.Size(240, 22);
            this.lblHeadline.TabIndex = 1;
            this.lblHeadline.Text = "Wir werden angegriffen!";
            this.lblHeadline.Click += new System.EventHandler(this.lblHeadline_Click);
            // 
            // lblSubHeadline
            // 
            this.lblSubHeadline.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSubHeadline.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lblSubHeadline.Location = new System.Drawing.Point(172, 52);
            this.lblSubHeadline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubHeadline.Name = "lblSubHeadline";
            this.lblSubHeadline.Size = new System.Drawing.Size(454, 87);
            this.lblSubHeadline.TabIndex = 11;
            this.lblSubHeadline.Text = "Name: USS ";
            // 
            // ColorCodeBox
            // 
            this.ColorCodeBox.BackColor = System.Drawing.Color.OrangeRed;
            this.ColorCodeBox.Location = new System.Drawing.Point(0, 0);
            this.ColorCodeBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ColorCodeBox.Name = "ColorCodeBox";
            this.ColorCodeBox.Size = new System.Drawing.Size(14, 208);
            this.ColorCodeBox.TabIndex = 3;
            this.ColorCodeBox.TabStop = false;
            // 
            // btnHelp
            // 
            this.btnHelp.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHelp.Location = new System.Drawing.Point(462, 153);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(166, 38);
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "Wir helfen!";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(174, 153);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(241, 38);
            this.button1.TabIndex = 5;
            this.button1.Text = "Leider zu weit weg";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DistressCallControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
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
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DistressCallControl";
            this.Size = new System.Drawing.Size(656, 208);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorCodeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblHeadline;
		private System.Windows.Forms.Label lblSubHeadline;
		private System.Windows.Forms.PictureBox ColorCodeBox;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Button button1;
	}
}
