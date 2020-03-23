namespace SpaceDealerCoreUi
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.fp2 = new System.Windows.Forms.FlowLayoutPanel();
			this.fp1 = new System.Windows.Forms.FlowLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.fp2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.fp1);
			this.splitContainer1.Size = new System.Drawing.Size(1284, 628);
			this.splitContainer1.SplitterDistance = 687;
			this.splitContainer1.TabIndex = 1;
			// 
			// fp2
			// 
			this.fp2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fp2.Location = new System.Drawing.Point(0, 0);
			this.fp2.Name = "fp2";
			this.fp2.Padding = new System.Windows.Forms.Padding(5);
			this.fp2.Size = new System.Drawing.Size(667, 628);
			this.fp2.AutoScroll = true;
			this.fp2.TabIndex = 0;
			this.fp2.WrapContents = false;
			this.fp2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			// 
			// fp1
			// 
			this.fp1.AutoScroll = true;
			this.fp1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fp1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.fp1.Location = new System.Drawing.Point(0, 0);
			this.fp1.Name = "fp1";
			this.fp1.Padding = new System.Windows.Forms.Padding(5);
			this.fp1.Size = new System.Drawing.Size(613, 628);
			this.fp1.TabIndex = 0;
			this.fp1.WrapContents = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1284, 652);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.FlowLayoutPanel fp1;
		private System.Windows.Forms.FlowLayoutPanel fp2;
	}
}

