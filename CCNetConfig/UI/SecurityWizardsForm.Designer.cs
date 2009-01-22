namespace CCNetConfig.UI
{
    partial class SecurityWizardsForm
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
            System.Windows.Forms.Button cancelButton;
            System.Windows.Forms.Panel panel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityWizardsForm));
            this.permissionsButton = new System.Windows.Forms.Button();
            this.importUsersButton = new System.Windows.Forms.Button();
            this.configureButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(138, 166);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "&Close";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            panel1.BackColor = System.Drawing.Color.White;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel1.Controls.Add(this.permissionsButton);
            panel1.Controls.Add(this.importUsersButton);
            panel1.Controls.Add(this.configureButton);
            panel1.Location = new System.Drawing.Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(201, 148);
            panel1.TabIndex = 0;
            // 
            // permissionsButton
            // 
            this.permissionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.permissionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.permissionsButton.Image = global::CCNetConfig.Properties.Resources.assignpermissions_16x16;
            this.permissionsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.permissionsButton.Location = new System.Drawing.Point(9, 99);
            this.permissionsButton.Margin = new System.Windows.Forms.Padding(9);
            this.permissionsButton.Name = "permissionsButton";
            this.permissionsButton.Size = new System.Drawing.Size(179, 37);
            this.permissionsButton.TabIndex = 2;
            this.permissionsButton.Text = "&Assign Permissions";
            this.permissionsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.permissionsButton.UseVisualStyleBackColor = true;
            this.permissionsButton.Click += new System.EventHandler(this.permissionsButton_Click);
            // 
            // importUsersButton
            // 
            this.importUsersButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.importUsersButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importUsersButton.Image = global::CCNetConfig.Properties.Resources.usersimport_16x16;
            this.importUsersButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.importUsersButton.Location = new System.Drawing.Point(9, 55);
            this.importUsersButton.Margin = new System.Windows.Forms.Padding(9);
            this.importUsersButton.Name = "importUsersButton";
            this.importUsersButton.Size = new System.Drawing.Size(179, 37);
            this.importUsersButton.TabIndex = 1;
            this.importUsersButton.Text = "&Import Users";
            this.importUsersButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.importUsersButton.UseVisualStyleBackColor = true;
            this.importUsersButton.Click += new System.EventHandler(this.importUsersButton_Click);
            // 
            // configureButton
            // 
            this.configureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.configureButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.configureButton.Image = global::CCNetConfig.Properties.Resources.securitysetting_16x16;
            this.configureButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.configureButton.Location = new System.Drawing.Point(9, 9);
            this.configureButton.Margin = new System.Windows.Forms.Padding(9);
            this.configureButton.Name = "configureButton";
            this.configureButton.Size = new System.Drawing.Size(179, 37);
            this.configureButton.TabIndex = 0;
            this.configureButton.Text = "Configure &Security";
            this.configureButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.configureButton.UseVisualStyleBackColor = true;
            this.configureButton.Click += new System.EventHandler(this.configureButton_Click);
            // 
            // SecurityWizardsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 200);
            this.ControlBox = false;
            this.Controls.Add(panel1);
            this.Controls.Add(cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SecurityWizardsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Security Wizards";
            panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button permissionsButton;
        private System.Windows.Forms.Button importUsersButton;
        private System.Windows.Forms.Button configureButton;
    }
}