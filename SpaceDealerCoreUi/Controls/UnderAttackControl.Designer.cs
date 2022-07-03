namespace SpaceDealerCoreUi.Controls
{
	partial class UnderAttackControl
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
            this.btnHide = new System.Windows.Forms.Button();
            this.btnAttack = new System.Windows.Forms.Button();
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
            this.lblSubHeadline.Location = new System.Drawing.Point(174, 52);
            this.lblSubHeadline.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblSubHeadline.Multiline = true;
            this.lblSubHeadline.Name = "lblSubHeadline";
            this.lblSubHeadline.ReadOnly = true;
            this.lblSubHeadline.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lblSubHeadline.Size = new System.Drawing.Size(453, 94);
            this.lblSubHeadline.TabIndex = 2;
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
            // btnHide
            // 
            this.btnHide.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHide.Location = new System.Drawing.Point(462, 153);
            this.btnHide.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(166, 38);
            this.btnHide.TabIndex = 4;
            this.btnHide.Text = "Entkommen!";
            this.btnHide.UseVisualStyleBackColor = true;
            // 
            // btnAttack
            // 
            this.btnAttack.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAttack.Location = new System.Drawing.Point(174, 153);
            this.btnAttack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAttack.Name = "btnAttack";
            this.btnAttack.Size = new System.Drawing.Size(241, 38);
            this.btnAttack.TabIndex = 5;
            this.btnAttack.Text = "Attacke!!!";
            this.btnAttack.UseVisualStyleBackColor = true;
            this.btnAttack.Click += new System.EventHandler(this.btnAttack_Click);
            // 
            // UnderAttackControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnAttack);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.ColorCodeBox);
            this.Controls.Add(this.lblSubHeadline);
            this.Controls.Add(this.lblHeadline);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "UnderAttackControl";
            this.Size = new System.Drawing.Size(656, 208);
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
		private System.Windows.Forms.Button btnHide;
		private System.Windows.Forms.Button btnAttack;
	}
}
