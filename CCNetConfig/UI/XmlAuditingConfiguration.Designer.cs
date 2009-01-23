namespace CCNetConfig.UI
{
    partial class XmlAuditingConfiguration
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
            this.xmlSettings = new System.Windows.Forms.GroupBox();
            this.useXmlAuditing = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.xmlSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // xmlSettings
            // 
            this.xmlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlSettings.Controls.Add(this.checkBox4);
            this.xmlSettings.Controls.Add(this.checkBox3);
            this.xmlSettings.Controls.Add(this.checkBox2);
            this.xmlSettings.Enabled = false;
            this.xmlSettings.Location = new System.Drawing.Point(13, 13);
            this.xmlSettings.Name = "xmlSettings";
            this.xmlSettings.Size = new System.Drawing.Size(559, 300);
            this.xmlSettings.TabIndex = 0;
            this.xmlSettings.TabStop = false;
            // 
            // useXmlAuditing
            // 
            this.useXmlAuditing.AutoSize = true;
            this.useXmlAuditing.Location = new System.Drawing.Point(27, 13);
            this.useXmlAuditing.Name = "useXmlAuditing";
            this.useXmlAuditing.Size = new System.Drawing.Size(152, 17);
            this.useXmlAuditing.TabIndex = 1;
            this.useXmlAuditing.Text = "Use XML Auditing Logging";
            this.useXmlAuditing.UseVisualStyleBackColor = true;
            this.useXmlAuditing.CheckedChanged += new System.EventHandler(this.useXmlAuditing_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(14, 23);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(141, 17);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Audit Successful Events";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(14, 46);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(117, 17);
            this.checkBox3.TabIndex = 1;
            this.checkBox3.Text = "Audit Failed Events";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(14, 69);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(139, 17);
            this.checkBox4.TabIndex = 2;
            this.checkBox4.Text = "Set as the Audit Reader";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // XmlAuditingConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.useXmlAuditing);
            this.Controls.Add(this.xmlSettings);
            this.Name = "XmlAuditingConfiguration";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(585, 326);
            this.xmlSettings.ResumeLayout(false);
            this.xmlSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox xmlSettings;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox useXmlAuditing;
    }
}
