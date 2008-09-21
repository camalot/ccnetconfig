/*
 * Copyright (c) 2006, Ryan Conrad. All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * 
 * - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the distribution.
 * 
 * - Neither the name of the Camalot Designs nor the names of its contributors may be used to endorse or promote products derived from this software 
 *    without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
 * DAMAGE.
 */
namespace CCNetConfig.UI {
  /// <summary>
  /// Options dialog
  /// </summary>
  partial class OptionsForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose ( bool disposing ) {
      if ( disposing && ( components != null ) ) {
        components.Dispose ();
      }
      base.Dispose ( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent () {
			this.components = new System.ComponentModel.Container ();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( OptionsForm ) );
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup ( "Core Components", System.Windows.Forms.HorizontalAlignment.Left );
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup ( "Plugin Components", System.Windows.Forms.HorizontalAlignment.Left );
			this.optionsTabControl = new System.Windows.Forms.TabControl ();
			this.generalTab = new System.Windows.Forms.TabPage ();
			this.sortProjectsAlphabetically = new System.Windows.Forms.CheckBox ();
			this.label11 = new System.Windows.Forms.Label ();
			this.externalViewerArguments = new System.Windows.Forms.TextBox ();
			this.label10 = new System.Windows.Forms.Label ();
			this.generalMonitorForChanges = new System.Windows.Forms.CheckBox ();
			this.externalFileViewerBrowseButton = new System.Windows.Forms.Button ();
			this.externalFileViewer = new System.Windows.Forms.TextBox ();
			this.label9 = new System.Windows.Forms.Label ();
			this.backupTab = new System.Windows.Forms.TabPage ();
			this.backupWhenSaving = new System.Windows.Forms.CheckBox ();
			this.backupSettingsGroupbox = new System.Windows.Forms.GroupBox ();
			this.label8 = new System.Windows.Forms.Label ();
			this.backupFilesList = new System.Windows.Forms.ListView ();
			this.chTimeStamp = new System.Windows.Forms.ColumnHeader ();
			this.imageList = new System.Windows.Forms.ImageList ( this.components );
			this.deleteBackupFilesButton = new System.Windows.Forms.Button ();
			this.backupFilesToKeep = new System.Windows.Forms.NumericUpDown ();
			this.label7 = new System.Windows.Forms.Label ();
			this.backupSavePath = new System.Windows.Forms.TextBox ();
			this.backupPathBrowseButton = new System.Windows.Forms.Button ();
			this.label6 = new System.Windows.Forms.Label ();
			this.updatesTab = new System.Windows.Forms.TabPage ();
			this.updatesCheckNowButton = new System.Windows.Forms.Button ();
			this.updateTypeCombo = new System.Windows.Forms.ComboBox ();
			this.label5 = new System.Windows.Forms.Label ();
			this.updatesCheckAtStartup = new System.Windows.Forms.CheckBox ();
			this.useProxy = new System.Windows.Forms.CheckBox ();
			this.proxyGroup = new System.Windows.Forms.GroupBox ();
			this.proxyPassword = new System.Windows.Forms.TextBox ();
			this.label4 = new System.Windows.Forms.Label ();
			this.proxyUser = new System.Windows.Forms.TextBox ();
			this.label3 = new System.Windows.Forms.Label ();
			this.proxyPort = new System.Windows.Forms.NumericUpDown ();
			this.proxyServerAddress = new System.Windows.Forms.TextBox ();
			this.label2 = new System.Windows.Forms.Label ();
			this.label1 = new System.Windows.Forms.Label ();
			this.componentsTab = new System.Windows.Forms.TabPage ();
			this.componentsList = new System.Windows.Forms.ListView ();
			this.chName = new System.Windows.Forms.ColumnHeader ();
			this.chType = new System.Windows.Forms.ColumnHeader ();
			this.chNamespace = new System.Windows.Forms.ColumnHeader ();
			this.chAssembly = new System.Windows.Forms.ColumnHeader ();
			this.chVersion = new System.Windows.Forms.ColumnHeader ();
			this.componentsInfoLabel = new System.Windows.Forms.Label ();
			this.pluginsTab = new System.Windows.Forms.TabPage ();
			this.pluginUpdateStatus = new System.Windows.Forms.Label ();
			this.updatePlugin = new System.Windows.Forms.Button ();
			this.pluginsList = new System.Windows.Forms.ListView ();
			this.pluginNameColumn = new System.Windows.Forms.ColumnHeader ();
			this.pluginAuthorColumn = new System.Windows.Forms.ColumnHeader ();
			this.footerPanel = new System.Windows.Forms.Panel ();
			this.applyButton = new System.Windows.Forms.Button ();
			this.okButton = new System.Windows.Forms.Button ();
			this.cancelButton = new System.Windows.Forms.Button ();
			this.backupFilesContextMenu = new System.Windows.Forms.ContextMenuStrip ( this.components );
			this.restoreBackupFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
			this.viewBackupFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator ();
			this.deleteBackupFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
			this.optionsTabControl.SuspendLayout ();
			this.generalTab.SuspendLayout ();
			this.backupTab.SuspendLayout ();
			this.backupSettingsGroupbox.SuspendLayout ();
			( (System.ComponentModel.ISupportInitialize)( this.backupFilesToKeep ) ).BeginInit ();
			this.updatesTab.SuspendLayout ();
			this.proxyGroup.SuspendLayout ();
			( (System.ComponentModel.ISupportInitialize)( this.proxyPort ) ).BeginInit ();
			this.componentsTab.SuspendLayout ();
			this.pluginsTab.SuspendLayout ();
			this.footerPanel.SuspendLayout ();
			this.backupFilesContextMenu.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// optionsTabControl
			// 
			this.optionsTabControl.Controls.Add ( this.generalTab );
			this.optionsTabControl.Controls.Add ( this.backupTab );
			this.optionsTabControl.Controls.Add ( this.updatesTab );
			this.optionsTabControl.Controls.Add ( this.componentsTab );
			this.optionsTabControl.Controls.Add ( this.pluginsTab );
			this.optionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.optionsTabControl.ImageList = this.imageList;
			this.optionsTabControl.Location = new System.Drawing.Point ( 0, 0 );
			this.optionsTabControl.Name = "optionsTabControl";
			this.optionsTabControl.SelectedIndex = 0;
			this.optionsTabControl.Size = new System.Drawing.Size ( 466, 228 );
			this.optionsTabControl.TabIndex = 0;
			// 
			// generalTab
			// 
			this.generalTab.Controls.Add ( this.sortProjectsAlphabetically );
			this.generalTab.Controls.Add ( this.label11 );
			this.generalTab.Controls.Add ( this.externalViewerArguments );
			this.generalTab.Controls.Add ( this.label10 );
			this.generalTab.Controls.Add ( this.generalMonitorForChanges );
			this.generalTab.Controls.Add ( this.externalFileViewerBrowseButton );
			this.generalTab.Controls.Add ( this.externalFileViewer );
			this.generalTab.Controls.Add ( this.label9 );
			this.generalTab.Location = new System.Drawing.Point ( 4, 23 );
			this.generalTab.Name = "generalTab";
			this.generalTab.Padding = new System.Windows.Forms.Padding ( 3 );
			this.generalTab.Size = new System.Drawing.Size ( 458, 201 );
			this.generalTab.TabIndex = 4;
			this.generalTab.Text = "General";
			this.generalTab.UseVisualStyleBackColor = true;
			// 
			// sortProjectsAlphabetically
			// 
			this.sortProjectsAlphabetically.AutoSize = true;
			this.sortProjectsAlphabetically.Location = new System.Drawing.Point ( 11, 97 );
			this.sortProjectsAlphabetically.Name = "sortProjectsAlphabetically";
			this.sortProjectsAlphabetically.Size = new System.Drawing.Size ( 409, 17 );
			this.sortProjectsAlphabetically.TabIndex = 12;
			this.sortProjectsAlphabetically.Text = "Sort Projects Alphabetically by name (applied the next time a config file is open" +
					"ed)";
			this.sortProjectsAlphabetically.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point ( 91, 61 );
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size ( 182, 13 );
			this.label11.TabIndex = 11;
			this.label11.Text = "Use \'{0}\' to represent the file to open.";
			// 
			// externalViewerArguments
			// 
			this.externalViewerArguments.Location = new System.Drawing.Point ( 94, 38 );
			this.externalViewerArguments.Name = "externalViewerArguments";
			this.externalViewerArguments.Size = new System.Drawing.Size ( 354, 20 );
			this.externalViewerArguments.TabIndex = 10;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point ( 8, 41 );
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size ( 60, 13 );
			this.label10.TabIndex = 9;
			this.label10.Text = "Arguments:";
			// 
			// generalMonitorForChanges
			// 
			this.generalMonitorForChanges.AutoSize = true;
			this.generalMonitorForChanges.Location = new System.Drawing.Point ( 11, 120 );
			this.generalMonitorForChanges.Name = "generalMonitorForChanges";
			this.generalMonitorForChanges.Size = new System.Drawing.Size ( 240, 17 );
			this.generalMonitorForChanges.TabIndex = 8;
			this.generalMonitorForChanges.Text = "Monitor configuration file for external changes";
			this.generalMonitorForChanges.UseVisualStyleBackColor = true;
			this.generalMonitorForChanges.Visible = false;
			// 
			// externalFileViewerBrowseButton
			// 
			this.externalFileViewerBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.externalFileViewerBrowseButton.Font = new System.Drawing.Font ( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
			this.externalFileViewerBrowseButton.Location = new System.Drawing.Point ( 422, 10 );
			this.externalFileViewerBrowseButton.Name = "externalFileViewerBrowseButton";
			this.externalFileViewerBrowseButton.Size = new System.Drawing.Size ( 30, 21 );
			this.externalFileViewerBrowseButton.TabIndex = 5;
			this.externalFileViewerBrowseButton.Text = "...";
			this.externalFileViewerBrowseButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.externalFileViewerBrowseButton.UseVisualStyleBackColor = true;
			// 
			// externalFileViewer
			// 
			this.externalFileViewer.Location = new System.Drawing.Point ( 94, 11 );
			this.externalFileViewer.Name = "externalFileViewer";
			this.externalFileViewer.Size = new System.Drawing.Size ( 322, 20 );
			this.externalFileViewer.TabIndex = 1;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point ( 8, 12 );
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size ( 83, 13 );
			this.label9.TabIndex = 0;
			this.label9.Text = "External Viewer:";
			// 
			// backupTab
			// 
			this.backupTab.Controls.Add ( this.backupWhenSaving );
			this.backupTab.Controls.Add ( this.backupSettingsGroupbox );
			this.backupTab.Location = new System.Drawing.Point ( 4, 23 );
			this.backupTab.Name = "backupTab";
			this.backupTab.Padding = new System.Windows.Forms.Padding ( 3 );
			this.backupTab.Size = new System.Drawing.Size ( 458, 201 );
			this.backupTab.TabIndex = 3;
			this.backupTab.Text = "Backup";
			this.backupTab.UseVisualStyleBackColor = true;
			// 
			// backupWhenSaving
			// 
			this.backupWhenSaving.AutoSize = true;
			this.backupWhenSaving.Location = new System.Drawing.Point ( 13, 9 );
			this.backupWhenSaving.Name = "backupWhenSaving";
			this.backupWhenSaving.Size = new System.Drawing.Size ( 188, 17 );
			this.backupWhenSaving.TabIndex = 0;
			this.backupWhenSaving.Text = "Backup ccnet.config when saving";
			this.backupWhenSaving.UseVisualStyleBackColor = true;
			this.backupWhenSaving.CheckedChanged += new System.EventHandler ( this.backupWhenSaving_CheckedChanged );
			// 
			// backupSettingsGroupbox
			// 
			this.backupSettingsGroupbox.Controls.Add ( this.label8 );
			this.backupSettingsGroupbox.Controls.Add ( this.backupFilesList );
			this.backupSettingsGroupbox.Controls.Add ( this.deleteBackupFilesButton );
			this.backupSettingsGroupbox.Controls.Add ( this.backupFilesToKeep );
			this.backupSettingsGroupbox.Controls.Add ( this.label7 );
			this.backupSettingsGroupbox.Controls.Add ( this.backupSavePath );
			this.backupSettingsGroupbox.Controls.Add ( this.backupPathBrowseButton );
			this.backupSettingsGroupbox.Controls.Add ( this.label6 );
			this.backupSettingsGroupbox.Enabled = false;
			this.backupSettingsGroupbox.Location = new System.Drawing.Point ( 6, 10 );
			this.backupSettingsGroupbox.Name = "backupSettingsGroupbox";
			this.backupSettingsGroupbox.Size = new System.Drawing.Size ( 442, 185 );
			this.backupSettingsGroupbox.TabIndex = 1;
			this.backupSettingsGroupbox.TabStop = false;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point ( 6, 68 );
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size ( 52, 13 );
			this.label8.TabIndex = 11;
			this.label8.Text = "Backups:";
			// 
			// backupFilesList
			// 
			this.backupFilesList.Columns.AddRange ( new System.Windows.Forms.ColumnHeader[] {
            this.chTimeStamp} );
			this.backupFilesList.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.backupFilesList.FullRowSelect = true;
			this.backupFilesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.backupFilesList.LargeImageList = this.imageList;
			this.backupFilesList.Location = new System.Drawing.Point ( 3, 84 );
			this.backupFilesList.Name = "backupFilesList";
			this.backupFilesList.Size = new System.Drawing.Size ( 436, 98 );
			this.backupFilesList.SmallImageList = this.imageList;
			this.backupFilesList.StateImageList = this.imageList;
			this.backupFilesList.TabIndex = 10;
			this.backupFilesList.UseCompatibleStateImageBehavior = false;
			this.backupFilesList.View = System.Windows.Forms.View.Details;
			this.backupFilesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler ( this.backupFilesList_MouseDoubleClick );
			this.backupFilesList.MouseClick += new System.Windows.Forms.MouseEventHandler ( this.backupFilesList_MouseClick );
			this.backupFilesList.KeyDown += new System.Windows.Forms.KeyEventHandler ( this.backupFilesList_KeyDown );
			// 
			// chTimeStamp
			// 
			this.chTimeStamp.Text = "Timestamp";
			this.chTimeStamp.Width = 200;
			// 
			// imageList
			// 
			this.imageList.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject ( "imageList.ImageStream" ) ) );
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName ( 0, "backup" );
			this.imageList.Images.SetKeyName ( 1, "3_1.ico" );
			// 
			// deleteBackupFilesButton
			// 
			this.deleteBackupFilesButton.Location = new System.Drawing.Point ( 309, 40 );
			this.deleteBackupFilesButton.Name = "deleteBackupFilesButton";
			this.deleteBackupFilesButton.Size = new System.Drawing.Size ( 126, 27 );
			this.deleteBackupFilesButton.TabIndex = 9;
			this.deleteBackupFilesButton.Text = "&Delete All Backups";
			this.deleteBackupFilesButton.UseVisualStyleBackColor = true;
			this.deleteBackupFilesButton.Click += new System.EventHandler ( this.deleteBackupFilesButton_Click );
			// 
			// backupFilesToKeep
			// 
			this.backupFilesToKeep.Location = new System.Drawing.Point ( 126, 42 );
			this.backupFilesToKeep.Name = "backupFilesToKeep";
			this.backupFilesToKeep.Size = new System.Drawing.Size ( 81, 20 );
			this.backupFilesToKeep.TabIndex = 7;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point ( 6, 47 );
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size ( 114, 13 );
			this.label7.TabIndex = 6;
			this.label7.Text = "# of Backups to Keep:";
			// 
			// backupSavePath
			// 
			this.backupSavePath.Location = new System.Drawing.Point ( 84, 16 );
			this.backupSavePath.Name = "backupSavePath";
			this.backupSavePath.ReadOnly = true;
			this.backupSavePath.Size = new System.Drawing.Size ( 320, 20 );
			this.backupSavePath.TabIndex = 5;
			// 
			// backupPathBrowseButton
			// 
			this.backupPathBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.backupPathBrowseButton.Font = new System.Drawing.Font ( "Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
			this.backupPathBrowseButton.Location = new System.Drawing.Point ( 410, 17 );
			this.backupPathBrowseButton.Name = "backupPathBrowseButton";
			this.backupPathBrowseButton.Size = new System.Drawing.Size ( 26, 18 );
			this.backupPathBrowseButton.TabIndex = 4;
			this.backupPathBrowseButton.Text = "...";
			this.backupPathBrowseButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.backupPathBrowseButton.UseVisualStyleBackColor = true;
			this.backupPathBrowseButton.Click += new System.EventHandler ( this.backupPathBrowseButton_Click );
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point ( 6, 19 );
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size ( 72, 13 );
			this.label6.TabIndex = 2;
			this.label6.Text = "Backup Path:";
			// 
			// updatesTab
			// 
			this.updatesTab.Controls.Add ( this.updatesCheckNowButton );
			this.updatesTab.Controls.Add ( this.updateTypeCombo );
			this.updatesTab.Controls.Add ( this.label5 );
			this.updatesTab.Controls.Add ( this.updatesCheckAtStartup );
			this.updatesTab.Controls.Add ( this.useProxy );
			this.updatesTab.Controls.Add ( this.proxyGroup );
			this.updatesTab.ImageKey = "(none)";
			this.updatesTab.Location = new System.Drawing.Point ( 4, 23 );
			this.updatesTab.Name = "updatesTab";
			this.updatesTab.Padding = new System.Windows.Forms.Padding ( 3 );
			this.updatesTab.Size = new System.Drawing.Size ( 458, 201 );
			this.updatesTab.TabIndex = 1;
			this.updatesTab.Text = "Updates";
			this.updatesTab.UseVisualStyleBackColor = true;
			// 
			// updatesCheckNowButton
			// 
			this.updatesCheckNowButton.Location = new System.Drawing.Point ( 9, 167 );
			this.updatesCheckNowButton.Name = "updatesCheckNowButton";
			this.updatesCheckNowButton.Size = new System.Drawing.Size ( 151, 28 );
			this.updatesCheckNowButton.TabIndex = 12;
			this.updatesCheckNowButton.Text = "Check for &Updates Now";
			this.updatesCheckNowButton.UseVisualStyleBackColor = true;
			this.updatesCheckNowButton.Click += new System.EventHandler ( this.updatesCheckNowButton_Click );
			// 
			// updateTypeCombo
			// 
			this.updateTypeCombo.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.updateTypeCombo.AllowDrop = true;
			this.updateTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.updateTypeCombo.FormattingEnabled = true;
			this.updateTypeCombo.Location = new System.Drawing.Point ( 260, 18 );
			this.updateTypeCombo.Name = "updateTypeCombo";
			this.updateTypeCombo.Size = new System.Drawing.Size ( 190, 21 );
			this.updateTypeCombo.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point ( 6, 18 );
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size ( 234, 13 );
			this.label5.TabIndex = 10;
			this.label5.Text = "When checking for updates, check updates for:";
			// 
			// updatesCheckAtStartup
			// 
			this.updatesCheckAtStartup.AutoSize = true;
			this.updatesCheckAtStartup.Location = new System.Drawing.Point ( 9, 43 );
			this.updatesCheckAtStartup.Name = "updatesCheckAtStartup";
			this.updatesCheckAtStartup.Size = new System.Drawing.Size ( 163, 17 );
			this.updatesCheckAtStartup.TabIndex = 9;
			this.updatesCheckAtStartup.Text = "Check for updates at start up";
			this.updatesCheckAtStartup.UseVisualStyleBackColor = true;
			// 
			// useProxy
			// 
			this.useProxy.BackColor = System.Drawing.Color.Transparent;
			this.useProxy.Location = new System.Drawing.Point ( 14, 72 );
			this.useProxy.Name = "useProxy";
			this.useProxy.Size = new System.Drawing.Size ( 120, 17 );
			this.useProxy.TabIndex = 0;
			this.useProxy.Text = "Use Proxy Server";
			this.useProxy.UseVisualStyleBackColor = false;
			this.useProxy.CheckedChanged += new System.EventHandler ( this.useProxy_CheckedChanged );
			// 
			// proxyGroup
			// 
			this.proxyGroup.Controls.Add ( this.proxyPassword );
			this.proxyGroup.Controls.Add ( this.label4 );
			this.proxyGroup.Controls.Add ( this.proxyUser );
			this.proxyGroup.Controls.Add ( this.label3 );
			this.proxyGroup.Controls.Add ( this.proxyPort );
			this.proxyGroup.Controls.Add ( this.proxyServerAddress );
			this.proxyGroup.Controls.Add ( this.label2 );
			this.proxyGroup.Controls.Add ( this.label1 );
			this.proxyGroup.Enabled = false;
			this.proxyGroup.Location = new System.Drawing.Point ( 8, 74 );
			this.proxyGroup.Name = "proxyGroup";
			this.proxyGroup.Size = new System.Drawing.Size ( 442, 87 );
			this.proxyGroup.TabIndex = 0;
			this.proxyGroup.TabStop = false;
			// 
			// proxyPassword
			// 
			this.proxyPassword.Font = new System.Drawing.Font ( "Wingdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 2 ) ) );
			this.proxyPassword.Location = new System.Drawing.Point ( 301, 53 );
			this.proxyPassword.Name = "proxyPassword";
			this.proxyPassword.PasswordChar = 'l';
			this.proxyPassword.Size = new System.Drawing.Size ( 135, 20 );
			this.proxyPassword.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point ( 239, 55 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size ( 56, 13 );
			this.label4.TabIndex = 7;
			this.label4.Text = "Password:";
			// 
			// proxyUser
			// 
			this.proxyUser.Location = new System.Drawing.Point ( 71, 52 );
			this.proxyUser.Name = "proxyUser";
			this.proxyUser.Size = new System.Drawing.Size ( 149, 20 );
			this.proxyUser.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point ( 7, 55 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size ( 58, 13 );
			this.label3.TabIndex = 5;
			this.label3.Text = "Username:";
			// 
			// proxyPort
			// 
			this.proxyPort.Location = new System.Drawing.Point ( 354, 22 );
			this.proxyPort.Maximum = new decimal ( new int[] {
            65535,
            0,
            0,
            0} );
			this.proxyPort.Minimum = new decimal ( new int[] {
            1,
            0,
            0,
            0} );
			this.proxyPort.Name = "proxyPort";
			this.proxyPort.Size = new System.Drawing.Size ( 82, 20 );
			this.proxyPort.TabIndex = 4;
			this.proxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.proxyPort.Value = new decimal ( new int[] {
            8080,
            0,
            0,
            0} );
			// 
			// proxyServerAddress
			// 
			this.proxyServerAddress.Location = new System.Drawing.Point ( 71, 22 );
			this.proxyServerAddress.Name = "proxyServerAddress";
			this.proxyServerAddress.Size = new System.Drawing.Size ( 212, 20 );
			this.proxyServerAddress.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point ( 319, 25 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size ( 29, 13 );
			this.label2.TabIndex = 1;
			this.label2.Text = "Port:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point ( 7, 24 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size ( 41, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Server:";
			// 
			// componentsTab
			// 
			this.componentsTab.Controls.Add ( this.componentsList );
			this.componentsTab.Controls.Add ( this.componentsInfoLabel );
			this.componentsTab.Location = new System.Drawing.Point ( 4, 23 );
			this.componentsTab.Name = "componentsTab";
			this.componentsTab.Padding = new System.Windows.Forms.Padding ( 3 );
			this.componentsTab.Size = new System.Drawing.Size ( 458, 201 );
			this.componentsTab.TabIndex = 2;
			this.componentsTab.Text = "Components";
			this.componentsTab.UseVisualStyleBackColor = true;
			// 
			// componentsList
			// 
			this.componentsList.CheckBoxes = true;
			this.componentsList.Columns.AddRange ( new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chType,
            this.chNamespace,
            this.chAssembly,
            this.chVersion} );
			this.componentsList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.componentsList.FullRowSelect = true;
			listViewGroup1.Header = "Core Components";
			listViewGroup1.Name = "lvgCore";
			listViewGroup2.Header = "Plugin Components";
			listViewGroup2.Name = "lvgPlugin";
			this.componentsList.Groups.AddRange ( new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2} );
			this.componentsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.componentsList.Location = new System.Drawing.Point ( 3, 28 );
			this.componentsList.MultiSelect = false;
			this.componentsList.Name = "componentsList";
			this.componentsList.ShowItemToolTips = true;
			this.componentsList.Size = new System.Drawing.Size ( 452, 170 );
			this.componentsList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.componentsList.TabIndex = 0;
			this.componentsList.UseCompatibleStateImageBehavior = false;
			this.componentsList.View = System.Windows.Forms.View.Details;
			this.componentsList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler ( this.componentsList_ColumnClick );
			// 
			// chName
			// 
			this.chName.Text = "Name";
			this.chName.Width = 80;
			// 
			// chType
			// 
			this.chType.Text = "Type";
			this.chType.Width = 62;
			// 
			// chNamespace
			// 
			this.chNamespace.DisplayIndex = 3;
			this.chNamespace.Text = "Namespace";
			this.chNamespace.Width = 78;
			// 
			// chAssembly
			// 
			this.chAssembly.DisplayIndex = 2;
			this.chAssembly.Text = "Assembly";
			this.chAssembly.Width = 70;
			// 
			// chVersion
			// 
			this.chVersion.Text = "CC.NET Version";
			this.chVersion.Width = 112;
			// 
			// componentsInfoLabel
			// 
			this.componentsInfoLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.componentsInfoLabel.Location = new System.Drawing.Point ( 3, 3 );
			this.componentsInfoLabel.Name = "componentsInfoLabel";
			this.componentsInfoLabel.Size = new System.Drawing.Size ( 452, 25 );
			this.componentsInfoLabel.TabIndex = 1;
			this.componentsInfoLabel.Text = "This lets you hide components that you do not wish to see when using CCNetConfig." +
					"";
			// 
			// pluginsTab
			// 
			this.pluginsTab.Controls.Add ( this.pluginUpdateStatus );
			this.pluginsTab.Controls.Add ( this.updatePlugin );
			this.pluginsTab.Controls.Add ( this.pluginsList );
			this.pluginsTab.Location = new System.Drawing.Point ( 4, 23 );
			this.pluginsTab.Name = "pluginsTab";
			this.pluginsTab.Padding = new System.Windows.Forms.Padding ( 3 );
			this.pluginsTab.Size = new System.Drawing.Size ( 458, 201 );
			this.pluginsTab.TabIndex = 5;
			this.pluginsTab.Text = "Plugins";
			this.pluginsTab.UseVisualStyleBackColor = true;
			// 
			// pluginUpdateStatus
			// 
			this.pluginUpdateStatus.AutoEllipsis = true;
			this.pluginUpdateStatus.Location = new System.Drawing.Point ( 8, 169 );
			this.pluginUpdateStatus.Name = "pluginUpdateStatus";
			this.pluginUpdateStatus.Size = new System.Drawing.Size ( 286, 17 );
			this.pluginUpdateStatus.TabIndex = 2;
			this.pluginUpdateStatus.Text = "<empty>";
			// 
			// updatePlugin
			// 
			this.updatePlugin.Enabled = false;
			this.updatePlugin.Location = new System.Drawing.Point ( 300, 161 );
			this.updatePlugin.Name = "updatePlugin";
			this.updatePlugin.Size = new System.Drawing.Size ( 145, 28 );
			this.updatePlugin.TabIndex = 1;
			this.updatePlugin.Text = "Check For Updates";
			this.updatePlugin.UseVisualStyleBackColor = true;
			this.updatePlugin.Click += new System.EventHandler ( this.updatePlugin_Click );
			// 
			// pluginsList
			// 
			this.pluginsList.Columns.AddRange ( new System.Windows.Forms.ColumnHeader[] {
            this.pluginNameColumn,
            this.pluginAuthorColumn} );
			this.pluginsList.FullRowSelect = true;
			this.pluginsList.Location = new System.Drawing.Point ( 10, 6 );
			this.pluginsList.Name = "pluginsList";
			this.pluginsList.Size = new System.Drawing.Size ( 436, 146 );
			this.pluginsList.TabIndex = 0;
			this.pluginsList.UseCompatibleStateImageBehavior = false;
			this.pluginsList.View = System.Windows.Forms.View.Details;
			this.pluginsList.SelectedIndexChanged += new System.EventHandler ( this.pluginsList_SelectedIndexChanged );
			// 
			// pluginNameColumn
			// 
			this.pluginNameColumn.Text = "Name";
			this.pluginNameColumn.Width = 25;
			// 
			// pluginAuthorColumn
			// 
			this.pluginAuthorColumn.Text = "Author";
			// 
			// footerPanel
			// 
			this.footerPanel.Controls.Add ( this.applyButton );
			this.footerPanel.Controls.Add ( this.okButton );
			this.footerPanel.Controls.Add ( this.cancelButton );
			this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.footerPanel.Location = new System.Drawing.Point ( 0, 228 );
			this.footerPanel.Name = "footerPanel";
			this.footerPanel.Size = new System.Drawing.Size ( 466, 40 );
			this.footerPanel.TabIndex = 1;
			// 
			// applyButton
			// 
			this.applyButton.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.applyButton.Location = new System.Drawing.Point ( 166, 6 );
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size ( 89, 29 );
			this.applyButton.TabIndex = 2;
			this.applyButton.Text = "&Apply";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler ( this.applyButton_Click );
			// 
			// okButton
			// 
			this.okButton.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.okButton.Location = new System.Drawing.Point ( 261, 6 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size ( 89, 29 );
			this.okButton.TabIndex = 1;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler ( this.okButton_Click );
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point ( 371, 6 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size ( 89, 29 );
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// backupFilesContextMenu
			// 
			this.backupFilesContextMenu.Items.AddRange ( new System.Windows.Forms.ToolStripItem[] {
            this.restoreBackupFileToolStripMenuItem,
            this.viewBackupFileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteBackupFileToolStripMenuItem} );
			this.backupFilesContextMenu.Name = "backupFilesContextMenu";
			this.backupFilesContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.backupFilesContextMenu.Size = new System.Drawing.Size ( 114, 76 );
			// 
			// restoreBackupFileToolStripMenuItem
			// 
			this.restoreBackupFileToolStripMenuItem.Name = "restoreBackupFileToolStripMenuItem";
			this.restoreBackupFileToolStripMenuItem.Size = new System.Drawing.Size ( 113, 22 );
			this.restoreBackupFileToolStripMenuItem.Text = "&Restore";
			this.restoreBackupFileToolStripMenuItem.Click += new System.EventHandler ( this.restoreBackupFileToolStripMenuItem_Click );
			// 
			// viewBackupFileToolStripMenuItem
			// 
			this.viewBackupFileToolStripMenuItem.Name = "viewBackupFileToolStripMenuItem";
			this.viewBackupFileToolStripMenuItem.Size = new System.Drawing.Size ( 113, 22 );
			this.viewBackupFileToolStripMenuItem.Text = "&View";
			this.viewBackupFileToolStripMenuItem.Click += new System.EventHandler ( this.viewBackupFileToolStripMenuItem_Click );
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size ( 110, 6 );
			// 
			// deleteBackupFileToolStripMenuItem
			// 
			this.deleteBackupFileToolStripMenuItem.Name = "deleteBackupFileToolStripMenuItem";
			this.deleteBackupFileToolStripMenuItem.Size = new System.Drawing.Size ( 113, 22 );
			this.deleteBackupFileToolStripMenuItem.Text = "&Delete";
			this.deleteBackupFileToolStripMenuItem.Click += new System.EventHandler ( this.deleteBackupFileToolStripMenuItem_Click );
			// 
			// OptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size ( 466, 268 );
			this.Controls.Add ( this.optionsTabControl );
			this.Controls.Add ( this.footerPanel );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.optionsTabControl.ResumeLayout ( false );
			this.generalTab.ResumeLayout ( false );
			this.generalTab.PerformLayout ();
			this.backupTab.ResumeLayout ( false );
			this.backupTab.PerformLayout ();
			this.backupSettingsGroupbox.ResumeLayout ( false );
			this.backupSettingsGroupbox.PerformLayout ();
			( (System.ComponentModel.ISupportInitialize)( this.backupFilesToKeep ) ).EndInit ();
			this.updatesTab.ResumeLayout ( false );
			this.updatesTab.PerformLayout ();
			this.proxyGroup.ResumeLayout ( false );
			this.proxyGroup.PerformLayout ();
			( (System.ComponentModel.ISupportInitialize)( this.proxyPort ) ).EndInit ();
			this.componentsTab.ResumeLayout ( false );
			this.pluginsTab.ResumeLayout ( false );
			this.footerPanel.ResumeLayout ( false );
			this.backupFilesContextMenu.ResumeLayout ( false );
			this.ResumeLayout ( false );

    }

    #endregion

    private System.Windows.Forms.TabControl optionsTabControl;
    private System.Windows.Forms.TabPage updatesTab;
    private System.Windows.Forms.GroupBox proxyGroup;
    private System.Windows.Forms.CheckBox useProxy;
    private System.Windows.Forms.Panel footerPanel;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.TextBox proxyServerAddress;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown proxyPort;
    private System.Windows.Forms.TextBox proxyPassword;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox proxyUser;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.CheckBox updatesCheckAtStartup;
    private System.Windows.Forms.ComboBox updateTypeCombo;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TabPage componentsTab;
    private System.Windows.Forms.ImageList imageList;
    private System.Windows.Forms.Label componentsInfoLabel;
    private System.Windows.Forms.ListView componentsList;
    private System.Windows.Forms.ColumnHeader chName;
    private System.Windows.Forms.Button updatesCheckNowButton;
    private System.Windows.Forms.ColumnHeader chType;
    private System.Windows.Forms.ColumnHeader chNamespace;
    private System.Windows.Forms.ColumnHeader chAssembly;
    private System.Windows.Forms.ColumnHeader chVersion;
    private System.Windows.Forms.TabPage backupTab;
    private System.Windows.Forms.CheckBox backupWhenSaving;
    private System.Windows.Forms.GroupBox backupSettingsGroupbox;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button backupPathBrowseButton;
    private System.Windows.Forms.TextBox backupSavePath;
    private System.Windows.Forms.NumericUpDown backupFilesToKeep;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Button deleteBackupFilesButton;
    private System.Windows.Forms.ListView backupFilesList;
    private System.Windows.Forms.ColumnHeader chTimeStamp;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.ContextMenuStrip backupFilesContextMenu;
    private System.Windows.Forms.ToolStripMenuItem restoreBackupFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem viewBackupFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem deleteBackupFileToolStripMenuItem;
    private System.Windows.Forms.TabPage generalTab;
    private System.Windows.Forms.Button externalFileViewerBrowseButton;
    private System.Windows.Forms.TextBox externalFileViewer;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.CheckBox generalMonitorForChanges;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox externalViewerArguments;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Button applyButton;
    private System.Windows.Forms.CheckBox sortProjectsAlphabetically;
		private System.Windows.Forms.TabPage pluginsTab;
		private System.Windows.Forms.Button updatePlugin;
		private System.Windows.Forms.ListView pluginsList;
		private System.Windows.Forms.ColumnHeader pluginNameColumn;
		private System.Windows.Forms.ColumnHeader pluginAuthorColumn;
		private System.Windows.Forms.Label pluginUpdateStatus;


  }
}