namespace SpaceDealerForms
{
	partial class Form1
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.spielToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.neuesSpielToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.spielstandLadenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.spielstandSpeichernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spielToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1071, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// spielToolStripMenuItem
			// 
			this.spielToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuesSpielToolStripMenuItem,
            this.toolStripMenuItem1,
            this.spielstandLadenToolStripMenuItem,
            this.spielstandSpeichernToolStripMenuItem,
            this.toolStripMenuItem2,
            this.beendenToolStripMenuItem});
			this.spielToolStripMenuItem.Name = "spielToolStripMenuItem";
			this.spielToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.spielToolStripMenuItem.Text = "Spiel";
			// 
			// neuesSpielToolStripMenuItem
			// 
			this.neuesSpielToolStripMenuItem.Name = "neuesSpielToolStripMenuItem";
			this.neuesSpielToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.neuesSpielToolStripMenuItem.Text = "&Neues Spiel";
			this.neuesSpielToolStripMenuItem.Click += new System.EventHandler(this.neuesSpielToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(179, 6);
			// 
			// spielstandLadenToolStripMenuItem
			// 
			this.spielstandLadenToolStripMenuItem.Name = "spielstandLadenToolStripMenuItem";
			this.spielstandLadenToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.spielstandLadenToolStripMenuItem.Text = "Spielstand laden";
			this.spielstandLadenToolStripMenuItem.Click += new System.EventHandler(this.spielstandLadenToolStripMenuItem_Click);
			// 
			// spielstandSpeichernToolStripMenuItem
			// 
			this.spielstandSpeichernToolStripMenuItem.Name = "spielstandSpeichernToolStripMenuItem";
			this.spielstandSpeichernToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.spielstandSpeichernToolStripMenuItem.Text = "Spielstand speichern";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(179, 6);
			// 
			// beendenToolStripMenuItem
			// 
			this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
			this.beendenToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.beendenToolStripMenuItem.Text = "B&eenden";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
			this.splitContainer1.Size = new System.Drawing.Size(1071, 628);
			this.splitContainer1.SplitterDistance = 382;
			this.splitContainer1.TabIndex = 1;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(685, 628);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1071, 652);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem spielToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem neuesSpielToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem spielstandLadenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem spielstandSpeichernToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
	}
}

