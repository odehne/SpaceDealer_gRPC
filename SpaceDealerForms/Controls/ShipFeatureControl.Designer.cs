namespace SpaceDealerForms.Controls
{
	partial class ShipFeatureControl
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
			this.lblHeadline = new System.Windows.Forms.Label();
			this.lblSubHeadline = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblHeadline
			// 
			this.lblHeadline.AutoSize = true;
			this.lblHeadline.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeadline.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblHeadline.Location = new System.Drawing.Point(14, 3);
			this.lblHeadline.Name = "lblHeadline";
			this.lblHeadline.Size = new System.Drawing.Size(189, 19);
			this.lblHeadline.TabIndex = 9;
			this.lblHeadline.Text = "Name: USS Enterprise";
			// 
			// lblSubHeadline
			// 
			this.lblSubHeadline.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
			this.lblSubHeadline.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.lblSubHeadline.Location = new System.Drawing.Point(15, 32);
			this.lblSubHeadline.Name = "lblSubHeadline";
			this.lblSubHeadline.Size = new System.Drawing.Size(254, 47);
			this.lblSubHeadline.TabIndex = 10;
			this.lblSubHeadline.Text = "Name: USS ";
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Location = new System.Drawing.Point(0, 87);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(297, 2);
			this.label1.TabIndex = 11;
			// 
			// ShipFeatureControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lavender;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblSubHeadline);
			this.Controls.Add(this.lblHeadline);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ShipFeatureControl";
			this.Size = new System.Drawing.Size(297, 89);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lblHeadline;
		private System.Windows.Forms.Label lblSubHeadline;
		private System.Windows.Forms.Label label1;
	}
}
