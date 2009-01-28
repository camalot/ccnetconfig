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
            this.auditFailed = new System.Windows.Forms.CheckBox();
            this.auditSuccessful = new System.Windows.Forms.CheckBox();
            this.useXmlAuditing = new System.Windows.Forms.CheckBox();
            this.xmlSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // xmlSettings
            // 
            this.xmlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlSettings.Controls.Add(this.auditFailed);
            this.xmlSettings.Controls.Add(this.auditSuccessful);
            this.xmlSettings.Enabled = false;
            this.xmlSettings.Location = new System.Drawing.Point(13, 13);
            this.xmlSettings.Name = "xmlSettings";
            this.xmlSettings.Size = new System.Drawing.Size(559, 300);
            this.xmlSettings.TabIndex = 0;
            this.xmlSettings.TabStop = false;
            // 
            // auditFailed
            // 
            this.auditFailed.AutoSize = true;
            this.auditFailed.Checked = true;
            this.auditFailed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.auditFailed.Location = new System.Drawing.Point(14, 46);
            this.auditFailed.Name = "auditFailed";
            this.auditFailed.Size = new System.Drawing.Size(117, 17);
            this.auditFailed.TabIndex = 1;
            this.auditFailed.Text = "Audit Failed Events";
            this.auditFailed.UseVisualStyleBackColor = true;
            // 
            // auditSuccessful
            // 
            this.auditSuccessful.AutoSize = true;
            this.auditSuccessful.Location = new System.Drawing.Point(14, 23);
            this.auditSuccessful.Name = "auditSuccessful";
            this.auditSuccessful.Size = new System.Drawing.Size(141, 17);
            this.auditSuccessful.TabIndex = 0;
            this.auditSuccessful.Text = "Audit Successful Events";
            this.auditSuccessful.UseVisualStyleBackColor = true;
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
        private System.Windows.Forms.CheckBox auditFailed;
        private System.Windows.Forms.CheckBox auditSuccessful;
        private System.Windows.Forms.CheckBox useXmlAuditing;
    }
}
