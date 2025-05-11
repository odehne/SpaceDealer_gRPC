namespace SpaceDealerCoreUi.Controls
{
    partial class MarketControl
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
            marketBuy = new System.Windows.Forms.FlowLayoutPanel();
            marketSell = new System.Windows.Forms.FlowLayoutPanel();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            lblCargoSize = new System.Windows.Forms.Label();
            lblCurrentPosition = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            lblShipName = new System.Windows.Forms.Label();
            ColorCodeBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorCodeBox).BeginInit();
            SuspendLayout();
            // 
            // marketBuy
            // 
            marketBuy.AutoScroll = true;
            marketBuy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            marketBuy.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            marketBuy.Location = new System.Drawing.Point(473, 203);
            marketBuy.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            marketBuy.Name = "marketBuy";
            marketBuy.Size = new System.Drawing.Size(411, 243);
            marketBuy.TabIndex = 21;
            marketBuy.WrapContents = false;
            // 
            // marketSell
            // 
            marketSell.AutoScroll = true;
            marketSell.BackColor = System.Drawing.Color.Lavender;
            marketSell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            marketSell.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            marketSell.Location = new System.Drawing.Point(36, 203);
            marketSell.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            marketSell.Name = "marketSell";
            marketSell.Size = new System.Drawing.Size(403, 243);
            marketSell.TabIndex = 20;
            marketSell.WrapContents = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            label2.Location = new System.Drawing.Point(473, 175);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(70, 22);
            label2.TabIndex = 19;
            label2.Text = "Kaufen";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            label1.Location = new System.Drawing.Point(31, 175);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 22);
            label1.TabIndex = 18;
            label1.Text = "Verkaufen";
            // 
            // lblCargoSize
            // 
            lblCargoSize.AutoSize = true;
            lblCargoSize.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblCargoSize.ForeColor = System.Drawing.SystemColors.WindowFrame;
            lblCargoSize.Location = new System.Drawing.Point(192, 101);
            lblCargoSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCargoSize.Name = "lblCargoSize";
            lblCargoSize.Size = new System.Drawing.Size(190, 22);
            lblCargoSize.TabIndex = 17;
            lblCargoSize.Text = "Ladekapazität: 30t";
            // 
            // lblCurrentPosition
            // 
            lblCurrentPosition.AutoSize = true;
            lblCurrentPosition.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblCurrentPosition.ForeColor = System.Drawing.SystemColors.WindowFrame;
            lblCurrentPosition.Location = new System.Drawing.Point(192, 59);
            lblCurrentPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCurrentPosition.Name = "lblCurrentPosition";
            lblCurrentPosition.Size = new System.Drawing.Size(230, 22);
            lblCurrentPosition.TabIndex = 16;
            lblCurrentPosition.Text = "Position: Erde [0,0,0]";
            lblCurrentPosition.Click += lblCurrentPosition_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.Black;
            pictureBox1.Location = new System.Drawing.Point(36, 18);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(131, 129);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            // 
            // lblShipName
            // 
            lblShipName.AutoSize = true;
            lblShipName.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblShipName.ForeColor = System.Drawing.SystemColors.WindowFrame;
            lblShipName.Location = new System.Drawing.Point(192, 18);
            lblShipName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblShipName.Name = "lblShipName";
            lblShipName.Size = new System.Drawing.Size(210, 22);
            lblShipName.TabIndex = 14;
            lblShipName.Text = "Name: USS Enterprise";
            // 
            // ColorCodeBox
            // 
            ColorCodeBox.BackColor = System.Drawing.Color.LightSkyBlue;
            ColorCodeBox.Location = new System.Drawing.Point(0, 0);
            ColorCodeBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ColorCodeBox.Name = "ColorCodeBox";
            ColorCodeBox.Size = new System.Drawing.Size(14, 678);
            ColorCodeBox.TabIndex = 22;
            ColorCodeBox.TabStop = false;
            // 
            // MarketControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(ColorCodeBox);
            Controls.Add(marketBuy);
            Controls.Add(marketSell);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblCargoSize);
            Controls.Add(lblCurrentPosition);
            Controls.Add(pictureBox1);
            Controls.Add(lblShipName);
            Name = "MarketControl";
            Size = new System.Drawing.Size(921, 474);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorCodeBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel marketBuy;
        private System.Windows.Forms.FlowLayoutPanel marketSell;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCargoSize;
        private System.Windows.Forms.Label lblCurrentPosition;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblShipName;
        private System.Windows.Forms.PictureBox ColorCodeBox;
    }
}
