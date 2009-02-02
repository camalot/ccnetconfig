namespace CCNetConfig.UI.Wizards
{
    partial class CacheConfiguration
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
            this.label1 = new System.Windows.Forms.Label();
            this.cacheMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.expiryMode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.sessionLength = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sessionLength)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cache Mode:";
            // 
            // cacheMode
            // 
            this.cacheMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cacheMode.FormattingEnabled = true;
            this.cacheMode.Items.AddRange(new object[] {
            "(Default)",
            "In memory",
            "File based"});
            this.cacheMode.Location = new System.Drawing.Point(122, 13);
            this.cacheMode.Name = "cacheMode";
            this.cacheMode.Size = new System.Drawing.Size(213, 21);
            this.cacheMode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Expiry Mode:";
            // 
            // expiryMode
            // 
            this.expiryMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.expiryMode.FormattingEnabled = true;
            this.expiryMode.Items.AddRange(new object[] {
            "(Default)",
            "Fixed",
            "Sliding"});
            this.expiryMode.Location = new System.Drawing.Point(122, 40);
            this.expiryMode.Name = "expiryMode";
            this.expiryMode.Size = new System.Drawing.Size(213, 21);
            this.expiryMode.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Session Length:";
            // 
            // sessionLength
            // 
            this.sessionLength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.sessionLength.Location = new System.Drawing.Point(122, 67);
            this.sessionLength.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.sessionLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sessionLength.Name = "sessionLength";
            this.sessionLength.Size = new System.Drawing.Size(120, 20);
            this.sessionLength.TabIndex = 5;
            this.sessionLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sessionLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "minutes";
            // 
            // CacheConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.sessionLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.expiryMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cacheMode);
            this.Controls.Add(this.label1);
            this.Name = "CacheConfiguration";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(461, 269);
            ((System.ComponentModel.ISupportInitialize)(this.sessionLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cacheMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox expiryMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown sessionLength;
        private System.Windows.Forms.Label label4;
    }
}
