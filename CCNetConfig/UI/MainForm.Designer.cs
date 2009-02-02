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
  partial class MainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing ) {
      if ( disposing && ( components != null ) ) {
        components.Dispose ();
      }
      base.Dispose (disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        this.treeImages = new System.Windows.Forms.ImageList(this.components);
        this.tvProjects = new System.Windows.Forms.TreeView();
        this.treePanel = new System.Windows.Forms.Panel();
        this.configurationToolStrip = new System.Windows.Forms.ToolStrip();
        this.addProjectToolButton = new System.Windows.Forms.ToolStripButton();
        this.removeProjectToolButton = new System.Windows.Forms.ToolStripButton();
        this.copyProjectToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.addProjectItems = new System.Windows.Forms.ToolStripDropDownButton();
        this.triggersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.tasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.publishersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.prebuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.configPropertiesToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.menuToolStrip = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
        this.previewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
        this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.configurationTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.documentationBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.validateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.securityWizardsToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
        this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.cCNetDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.cCNetConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
        this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.reportABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
        this.aboutCCNetConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.mainToolStrip = new System.Windows.Forms.ToolStrip();
        this.newConfigurationToolButton = new System.Windows.Forms.ToolStripButton();
        this.openConfigurationToolButton = new System.Windows.Forms.ToolStripButton();
        this.saveConfigurationToolButton = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.validateToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
        this.submitBugToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.showOptionsDialogToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
        this.checkForUpdatesToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.docPanel = new System.Windows.Forms.Panel();
        this.wbDocs = new System.Windows.Forms.WebBrowser();
        this.projectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.addProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.addTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.addTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.addPublisherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.addPrebuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.copySepMenuItem = new System.Windows.Forms.ToolStripSeparator();
        this.copyProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.copyProjectAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
        this.copyItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.cutItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.pasteNewItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.configPanel = new System.Windows.Forms.Panel();
        this.pgProperties = new CCNetConfig.Controls.PropertyGridEx();
        this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        this.configVersionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
        this.webSplitter = new CCNetConfig.Controls.CollapsibleSplitter();
        this.treeSplitter = new CCNetConfig.Controls.CollapsibleSplitter();
        this.editToolStrip = new System.Windows.Forms.ToolStrip();
        this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
        this.treePanel.SuspendLayout();
        this.configurationToolStrip.SuspendLayout();
        this.menuToolStrip.SuspendLayout();
        this.mainToolStrip.SuspendLayout();
        this.docPanel.SuspendLayout();
        this.projectContextMenu.SuspendLayout();
        this.configPanel.SuspendLayout();
        this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
        this.toolStripContainer1.ContentPanel.SuspendLayout();
        this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
        this.toolStripContainer1.SuspendLayout();
        this.statusStrip1.SuspendLayout();
        this.editToolStrip.SuspendLayout();
        this.SuspendLayout();
        // 
        // treeImages
        // 
        this.treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImages.ImageStream")));
        this.treeImages.TransparentColor = System.Drawing.Color.Transparent;
        this.treeImages.Images.SetKeyName(0, "root");
        this.treeImages.Images.SetKeyName(1, "application");
        this.treeImages.Images.SetKeyName(2, "help");
        this.treeImages.Images.SetKeyName(3, "project");
        this.treeImages.Images.SetKeyName(4, "publishers");
        this.treeImages.Images.SetKeyName(5, "tasks");
        this.treeImages.Images.SetKeyName(6, "extension");
        this.treeImages.Images.SetKeyName(7, "sourceControl");
        this.treeImages.Images.SetKeyName(8, "triggers");
        this.treeImages.Images.SetKeyName(9, "prebuild");
        this.treeImages.Images.SetKeyName(10, "remove");
        this.treeImages.Images.SetKeyName(11, "copy");
        this.treeImages.Images.SetKeyName(12, "queue");
        this.treeImages.Images.SetKeyName(13, "queueConfig");
        this.treeImages.Images.SetKeyName(14, "auditlogger_16x16");
        this.treeImages.Images.SetKeyName(15, "auditreader_16x16");
        this.treeImages.Images.SetKeyName(16, "security_16x16");
        this.treeImages.Images.SetKeyName(17, "assertion_16x16");
        this.treeImages.Images.SetKeyName(18, "securitysetting_16x16");
        this.treeImages.Images.SetKeyName(19, "securitycache_16x16");
        this.treeImages.Images.SetKeyName(20, "users_16x16");
        // 
        // tvProjects
        // 
        this.tvProjects.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tvProjects.FullRowSelect = true;
        this.tvProjects.HideSelection = false;
        this.tvProjects.ImageIndex = 0;
        this.tvProjects.ImageList = this.treeImages;
        this.tvProjects.Location = new System.Drawing.Point(0, 25);
        this.tvProjects.Name = "tvProjects";
        this.tvProjects.SelectedImageIndex = 0;
        this.tvProjects.ShowLines = false;
        this.tvProjects.Size = new System.Drawing.Size(202, 407);
        this.tvProjects.TabIndex = 12;
        this.tvProjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvProjects_AfterSelect);
        // 
        // treePanel
        // 
        this.treePanel.Controls.Add(this.tvProjects);
        this.treePanel.Controls.Add(this.configurationToolStrip);
        this.treePanel.Dock = System.Windows.Forms.DockStyle.Left;
        this.treePanel.Location = new System.Drawing.Point(0, 0);
        this.treePanel.Name = "treePanel";
        this.treePanel.Size = new System.Drawing.Size(202, 432);
        this.treePanel.TabIndex = 14;
        // 
        // configurationToolStrip
        // 
        this.configurationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProjectToolButton,
            this.removeProjectToolButton,
            this.copyProjectToolStripButton,
            this.toolStripSeparator2,
            this.addProjectItems,
            this.toolStripSeparator3,
            this.configPropertiesToolStripButton});
        this.configurationToolStrip.Location = new System.Drawing.Point(0, 0);
        this.configurationToolStrip.Name = "configurationToolStrip";
        this.configurationToolStrip.Size = new System.Drawing.Size(202, 25);
        this.configurationToolStrip.TabIndex = 14;
        this.configurationToolStrip.Text = "toolStrip2";
        // 
        // addProjectToolButton
        // 
        this.addProjectToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.addProjectToolButton.Enabled = false;
        this.addProjectToolButton.Image = global::CCNetConfig.Properties.Resources.addbox_16x16;
        this.addProjectToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.addProjectToolButton.Name = "addProjectToolButton";
        this.addProjectToolButton.Size = new System.Drawing.Size(23, 22);
        this.addProjectToolButton.Text = "Add Project";
        this.addProjectToolButton.ToolTipText = "Add New Project";
        this.addProjectToolButton.Click += new System.EventHandler(this.addProjectToolButton_Click);
        // 
        // removeProjectToolButton
        // 
        this.removeProjectToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.removeProjectToolButton.Enabled = false;
        this.removeProjectToolButton.Image = global::CCNetConfig.Properties.Resources.deletebox_16x16;
        this.removeProjectToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.removeProjectToolButton.Name = "removeProjectToolButton";
        this.removeProjectToolButton.Size = new System.Drawing.Size(23, 22);
        this.removeProjectToolButton.Text = "Remove Project";
        this.removeProjectToolButton.Click += new System.EventHandler(this.removeProjectToolButton_Click);
        // 
        // copyProjectToolStripButton
        // 
        this.copyProjectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.copyProjectToolStripButton.Enabled = false;
        this.copyProjectToolStripButton.Image = global::CCNetConfig.Properties.Resources.copy_16x16;
        this.copyProjectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.copyProjectToolStripButton.Name = "copyProjectToolStripButton";
        this.copyProjectToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.copyProjectToolStripButton.Text = "Copy Project";
        this.copyProjectToolStripButton.Click += new System.EventHandler(this.copyProjectToolStripButton_Click);
        // 
        // toolStripSeparator2
        // 
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
        // 
        // addProjectItems
        // 
        this.addProjectItems.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.addProjectItems.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.triggersToolStripMenuItem,
            this.tasksToolStripMenuItem,
            this.publishersToolStripMenuItem,
            this.prebuildToolStripMenuItem});
        this.addProjectItems.Enabled = false;
        this.addProjectItems.Image = global::CCNetConfig.Properties.Resources.options_16x16;
        this.addProjectItems.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.addProjectItems.Name = "addProjectItems";
        this.addProjectItems.Size = new System.Drawing.Size(29, 22);
        this.addProjectItems.Text = "Project Items";
        // 
        // triggersToolStripMenuItem
        // 
        this.triggersToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.appfolder_16x16;
        this.triggersToolStripMenuItem.Name = "triggersToolStripMenuItem";
        this.triggersToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
        this.triggersToolStripMenuItem.Text = "Triggers";
        // 
        // tasksToolStripMenuItem
        // 
        this.tasksToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.calendar_16x16;
        this.tasksToolStripMenuItem.Name = "tasksToolStripMenuItem";
        this.tasksToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
        this.tasksToolStripMenuItem.Text = "Tasks";
        // 
        // publishersToolStripMenuItem
        // 
        this.publishersToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.move_16x16;
        this.publishersToolStripMenuItem.Name = "publishersToolStripMenuItem";
        this.publishersToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
        this.publishersToolStripMenuItem.Text = "Publishers";
        // 
        // prebuildToolStripMenuItem
        // 
        this.prebuildToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.window_protected_16x16;
        this.prebuildToolStripMenuItem.Name = "prebuildToolStripMenuItem";
        this.prebuildToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
        this.prebuildToolStripMenuItem.Text = "Prebuild";
        // 
        // toolStripSeparator3
        // 
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
        // 
        // configPropertiesToolStripButton
        // 
        this.configPropertiesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.configPropertiesToolStripButton.Enabled = false;
        this.configPropertiesToolStripButton.Image = global::CCNetConfig.Properties.Resources.configProperties_16x16;
        this.configPropertiesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.configPropertiesToolStripButton.Name = "configPropertiesToolStripButton";
        this.configPropertiesToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.configPropertiesToolStripButton.Text = "Configuration Properties";
        this.configPropertiesToolStripButton.Click += new System.EventHandler(this.configPropertiesToolStripButton_Click);
        // 
        // menuToolStrip
        // 
        this.menuToolStrip.Dock = System.Windows.Forms.DockStyle.None;
        this.menuToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
        this.menuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
        this.menuToolStrip.Location = new System.Drawing.Point(0, 0);
        this.menuToolStrip.Name = "menuToolStrip";
        this.menuToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
        this.menuToolStrip.Size = new System.Drawing.Size(785, 24);
        this.menuToolStrip.TabIndex = 15;
        // 
        // fileToolStripMenuItem
        // 
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator5,
            this.previewMenuItem,
            this.toolStripSeparator9,
            this.exitToolStripMenuItem});
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        this.fileToolStripMenuItem.Text = "&File";
        // 
        // newToolStripMenuItem
        // 
        this.newToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.new_16x16;
        this.newToolStripMenuItem.Name = "newToolStripMenuItem";
        this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
        this.newToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
        this.newToolStripMenuItem.Text = "&New";
        this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
        // 
        // saveToolStripMenuItem
        // 
        this.saveToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.save2_16x16;
        this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
        this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
        this.saveToolStripMenuItem.Text = "&Save";
        this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
        // 
        // saveAsToolStripMenuItem
        // 
        this.saveAsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
        this.saveAsToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.saveas2_16x16;
        this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
        this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                    | System.Windows.Forms.Keys.S)));
        this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
        this.saveAsToolStripMenuItem.Text = "Save &As...";
        this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
        // 
        // openToolStripMenuItem
        // 
        this.openToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.open_16x16;
        this.openToolStripMenuItem.Name = "openToolStripMenuItem";
        this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
        this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
        this.openToolStripMenuItem.Text = "&Open";
        this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
        // 
        // toolStripSeparator5
        // 
        this.toolStripSeparator5.Name = "toolStripSeparator5";
        this.toolStripSeparator5.Size = new System.Drawing.Size(192, 6);
        // 
        // previewMenuItem
        // 
        this.previewMenuItem.Name = "previewMenuItem";
        this.previewMenuItem.Size = new System.Drawing.Size(195, 22);
        this.previewMenuItem.Text = "P&review";
        this.previewMenuItem.Click += new System.EventHandler(this.previewMenuItem_Click);
        // 
        // toolStripSeparator9
        // 
        this.toolStripSeparator9.Name = "toolStripSeparator9";
        this.toolStripSeparator9.Size = new System.Drawing.Size(192, 6);
        // 
        // exitToolStripMenuItem
        // 
        this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
        this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
        this.exitToolStripMenuItem.Text = "E&xit";
        this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
        // 
        // editToolStripMenuItem
        // 
        this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
        this.editToolStripMenuItem.Name = "editToolStripMenuItem";
        this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
        this.editToolStripMenuItem.Text = "&Edit";
        this.editToolStripMenuItem.Visible = false;
        // 
        // cutToolStripMenuItem
        // 
        this.cutToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.cutfile_16x16;
        this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
        this.cutToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
        this.cutToolStripMenuItem.Text = "Cut";
        // 
        // copyToolStripMenuItem
        // 
        this.copyToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.copy_16x16;
        this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
        this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
        this.copyToolStripMenuItem.Text = "Copy";
        // 
        // pasteToolStripMenuItem
        // 
        this.pasteToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.paste_16x16;
        this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
        this.pasteToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
        this.pasteToolStripMenuItem.Text = "Paste";
        // 
        // viewToolStripMenuItem
        // 
        this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationTreeToolStripMenuItem,
            this.documentationBrowserToolStripMenuItem});
        this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
        this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        this.viewToolStripMenuItem.Text = "&View";
        // 
        // configurationTreeToolStripMenuItem
        // 
        this.configurationTreeToolStripMenuItem.Checked = true;
        this.configurationTreeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        this.configurationTreeToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.window_16x16;
        this.configurationTreeToolStripMenuItem.Name = "configurationTreeToolStripMenuItem";
        this.configurationTreeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
        this.configurationTreeToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
        this.configurationTreeToolStripMenuItem.Text = "&Configuration Tree";
        this.configurationTreeToolStripMenuItem.Click += new System.EventHandler(this.configurationTreeToolStripMenuItem_Click);
        // 
        // documentationBrowserToolStripMenuItem
        // 
        this.documentationBrowserToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.info_16x16;
        this.documentationBrowserToolStripMenuItem.Name = "documentationBrowserToolStripMenuItem";
        this.documentationBrowserToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
        this.documentationBrowserToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
        this.documentationBrowserToolStripMenuItem.Text = "&Documentation Browser";
        this.documentationBrowserToolStripMenuItem.Click += new System.EventHandler(this.documentationBrowserToolStripMenuItem_Click);
        // 
        // toolsToolStripMenuItem
        // 
        this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.validateToolStripMenuItem,
            this.securityWizardsToolstripMenuItem,
            this.toolStripSeparator7,
            this.optionsToolStripMenuItem});
        this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
        this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
        this.toolsToolStripMenuItem.Text = "&Tools";
        // 
        // validateToolStripMenuItem
        // 
        this.validateToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.checkmark_16x16;
        this.validateToolStripMenuItem.Name = "validateToolStripMenuItem";
        this.validateToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.validateToolStripMenuItem.Text = "&Validate...";
        this.validateToolStripMenuItem.Click += new System.EventHandler(this.validateToolStripMenuItem_Click);
        // 
        // securityWizardsToolstripMenuItem
        // 
        this.securityWizardsToolstripMenuItem.Image = global::CCNetConfig.Properties.Resources.wizard_16x16;
        this.securityWizardsToolstripMenuItem.Name = "securityWizardsToolstripMenuItem";
        this.securityWizardsToolstripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.securityWizardsToolstripMenuItem.Text = "&Security Wizards...";
        this.securityWizardsToolstripMenuItem.Click += new System.EventHandler(this.securityWizardsToolstripMenuItem_Click);
        // 
        // toolStripSeparator7
        // 
        this.toolStripSeparator7.Name = "toolStripSeparator7";
        this.toolStripSeparator7.Size = new System.Drawing.Size(188, 6);
        // 
        // optionsToolStripMenuItem
        // 
        this.optionsToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.configure_16x16;
        this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
        this.optionsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                    | System.Windows.Forms.Keys.O)));
        this.optionsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
        this.optionsToolStripMenuItem.Text = "&Options";
        this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
        // 
        // helpToolStripMenuItem
        // 
        this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cCNetDocumentationToolStripMenuItem,
            this.cCNetConfigToolStripMenuItem,
            this.toolStripMenuItem2,
            this.checkForUpdatesToolStripMenuItem,
            this.reportABugToolStripMenuItem,
            this.toolStripMenuItem1,
            this.aboutCCNetConfigToolStripMenuItem});
        this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        this.helpToolStripMenuItem.Text = "&Help";
        // 
        // cCNetDocumentationToolStripMenuItem
        // 
        this.cCNetDocumentationToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.help_16x16;
        this.cCNetDocumentationToolStripMenuItem.Name = "cCNetDocumentationToolStripMenuItem";
        this.cCNetDocumentationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
        this.cCNetDocumentationToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
        this.cCNetDocumentationToolStripMenuItem.Text = "&CCNetConfig Documentation";
        this.cCNetDocumentationToolStripMenuItem.Visible = false;
        this.cCNetDocumentationToolStripMenuItem.Click += new System.EventHandler(this.cCNetDocumentationToolStripMenuItem_Click);
        // 
        // cCNetConfigToolStripMenuItem
        // 
        this.cCNetConfigToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.logo16;
        this.cCNetConfigToolStripMenuItem.Name = "cCNetConfigToolStripMenuItem";
        this.cCNetConfigToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
        this.cCNetConfigToolStripMenuItem.Text = "CC&NetConfig";
        this.cCNetConfigToolStripMenuItem.Click += new System.EventHandler(this.cCNetConfigToolStripMenuItem_Click);
        // 
        // toolStripMenuItem2
        // 
        this.toolStripMenuItem2.Name = "toolStripMenuItem2";
        this.toolStripMenuItem2.Size = new System.Drawing.Size(247, 6);
        // 
        // checkForUpdatesToolStripMenuItem
        // 
        this.checkForUpdatesToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.updates;
        this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
        this.checkForUpdatesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
        this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
        this.checkForUpdatesToolStripMenuItem.Text = "Check For &Updates";
        this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
        // 
        // reportABugToolStripMenuItem
        // 
        this.reportABugToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.WindowEdit;
        this.reportABugToolStripMenuItem.Name = "reportABugToolStripMenuItem";
        this.reportABugToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                    | System.Windows.Forms.Keys.B)));
        this.reportABugToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
        this.reportABugToolStripMenuItem.Text = "&Report A Bug";
        this.reportABugToolStripMenuItem.Click += new System.EventHandler(this.reportABugToolStripMenuItem_Click);
        // 
        // toolStripMenuItem1
        // 
        this.toolStripMenuItem1.Name = "toolStripMenuItem1";
        this.toolStripMenuItem1.Size = new System.Drawing.Size(247, 6);
        // 
        // aboutCCNetConfigToolStripMenuItem
        // 
        this.aboutCCNetConfigToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.help_16x16;
        this.aboutCCNetConfigToolStripMenuItem.Name = "aboutCCNetConfigToolStripMenuItem";
        this.aboutCCNetConfigToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
        this.aboutCCNetConfigToolStripMenuItem.Text = "&About CCNetConfig";
        this.aboutCCNetConfigToolStripMenuItem.Click += new System.EventHandler(this.aboutCCNetConfigToolStripMenuItem_Click);
        // 
        // mainToolStrip
        // 
        this.mainToolStrip.Dock = System.Windows.Forms.DockStyle.None;
        this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newConfigurationToolButton,
            this.openConfigurationToolButton,
            this.saveConfigurationToolButton,
            this.toolStripSeparator1,
            this.validateToolStripButton,
            this.toolStripSeparator8,
            this.submitBugToolStripButton,
            this.showOptionsDialogToolStripButton,
            this.toolStripSeparator4,
            this.checkForUpdatesToolStripButton});
        this.mainToolStrip.Location = new System.Drawing.Point(3, 24);
        this.mainToolStrip.Name = "mainToolStrip";
        this.mainToolStrip.Size = new System.Drawing.Size(191, 25);
        this.mainToolStrip.TabIndex = 16;
        this.mainToolStrip.Text = "toolStrip1";
        // 
        // newConfigurationToolButton
        // 
        this.newConfigurationToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.newConfigurationToolButton.Image = global::CCNetConfig.Properties.Resources.new_16x16;
        this.newConfigurationToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.newConfigurationToolButton.Name = "newConfigurationToolButton";
        this.newConfigurationToolButton.Size = new System.Drawing.Size(23, 22);
        this.newConfigurationToolButton.Text = "New Configuration";
        this.newConfigurationToolButton.Click += new System.EventHandler(this.newConfigurationToolButton_Click);
        // 
        // openConfigurationToolButton
        // 
        this.openConfigurationToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.openConfigurationToolButton.Image = global::CCNetConfig.Properties.Resources.open_16x16;
        this.openConfigurationToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.openConfigurationToolButton.Name = "openConfigurationToolButton";
        this.openConfigurationToolButton.Size = new System.Drawing.Size(23, 22);
        this.openConfigurationToolButton.Text = "Open Configuration";
        this.openConfigurationToolButton.Click += new System.EventHandler(this.openConfigurationToolButton_Click);
        // 
        // saveConfigurationToolButton
        // 
        this.saveConfigurationToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.saveConfigurationToolButton.Image = global::CCNetConfig.Properties.Resources.save2_16x16;
        this.saveConfigurationToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.saveConfigurationToolButton.Name = "saveConfigurationToolButton";
        this.saveConfigurationToolButton.Size = new System.Drawing.Size(23, 22);
        this.saveConfigurationToolButton.Text = "Save";
        this.saveConfigurationToolButton.Click += new System.EventHandler(this.saveConfigurationToolButton_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
        // 
        // validateToolStripButton
        // 
        this.validateToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.validateToolStripButton.Image = global::CCNetConfig.Properties.Resources.checkmark_16x16;
        this.validateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.validateToolStripButton.Name = "validateToolStripButton";
        this.validateToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.validateToolStripButton.Text = "Validate...";
        this.validateToolStripButton.Click += new System.EventHandler(this.validateToolStripButton_Click);
        // 
        // toolStripSeparator8
        // 
        this.toolStripSeparator8.Name = "toolStripSeparator8";
        this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
        // 
        // submitBugToolStripButton
        // 
        this.submitBugToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.submitBugToolStripButton.Image = global::CCNetConfig.Properties.Resources.WindowEdit;
        this.submitBugToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.submitBugToolStripButton.Name = "submitBugToolStripButton";
        this.submitBugToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.submitBugToolStripButton.Text = "Report A Bug";
        this.submitBugToolStripButton.Click += new System.EventHandler(this.submitBugToolStripButton_Click);
        // 
        // showOptionsDialogToolStripButton
        // 
        this.showOptionsDialogToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.showOptionsDialogToolStripButton.Image = global::CCNetConfig.Properties.Resources.configure_16x16;
        this.showOptionsDialogToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.showOptionsDialogToolStripButton.Name = "showOptionsDialogToolStripButton";
        this.showOptionsDialogToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.showOptionsDialogToolStripButton.Text = "Options";
        this.showOptionsDialogToolStripButton.Click += new System.EventHandler(this.showOptionsDialogToolStripButton_Click);
        // 
        // toolStripSeparator4
        // 
        this.toolStripSeparator4.Name = "toolStripSeparator4";
        this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
        // 
        // checkForUpdatesToolStripButton
        // 
        this.checkForUpdatesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.checkForUpdatesToolStripButton.Image = global::CCNetConfig.Properties.Resources.updates;
        this.checkForUpdatesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.checkForUpdatesToolStripButton.Name = "checkForUpdatesToolStripButton";
        this.checkForUpdatesToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.checkForUpdatesToolStripButton.Text = "Check for Updates";
        this.checkForUpdatesToolStripButton.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
        // 
        // docPanel
        // 
        this.docPanel.Controls.Add(this.wbDocs);
        this.docPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.docPanel.Location = new System.Drawing.Point(210, 285);
        this.docPanel.Name = "docPanel";
        this.docPanel.Size = new System.Drawing.Size(575, 147);
        this.docPanel.TabIndex = 19;
        this.docPanel.Visible = false;
        // 
        // wbDocs
        // 
        this.wbDocs.Dock = System.Windows.Forms.DockStyle.Fill;
        this.wbDocs.Location = new System.Drawing.Point(0, 0);
        this.wbDocs.MinimumSize = new System.Drawing.Size(20, 20);
        this.wbDocs.Name = "wbDocs";
        this.wbDocs.Size = new System.Drawing.Size(575, 147);
        this.wbDocs.TabIndex = 1;
        // 
        // projectContextMenu
        // 
        this.projectContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProjectToolStripMenuItem,
            this.addTaskToolStripMenuItem,
            this.addTriggerToolStripMenuItem,
            this.addPublisherToolStripMenuItem,
            this.addPrebuildToolStripMenuItem,
            this.copySepMenuItem,
            this.copyProjectMenuItem,
            this.copyProjectAsMenuItem,
            this.toolStripSeparator6,
            this.copyItemsToolStripMenuItem,
            this.cutItemToolStripMenuItem,
            this.pasteNewItemToolStripMenuItem});
        this.projectContextMenu.Name = "projectContextMenu";
        this.projectContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
        this.projectContextMenu.Size = new System.Drawing.Size(168, 236);
        // 
        // addProjectToolStripMenuItem
        // 
        this.addProjectToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.addbox_16x16;
        this.addProjectToolStripMenuItem.Name = "addProjectToolStripMenuItem";
        this.addProjectToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.addProjectToolStripMenuItem.Text = "Add Project";
        this.addProjectToolStripMenuItem.Visible = false;
        this.addProjectToolStripMenuItem.Click += new System.EventHandler(this.addProjectToolButton_Click);
        // 
        // addTaskToolStripMenuItem
        // 
        this.addTaskToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.calendar_16x16;
        this.addTaskToolStripMenuItem.Name = "addTaskToolStripMenuItem";
        this.addTaskToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.addTaskToolStripMenuItem.Text = "Add Tas&k";
        // 
        // addTriggerToolStripMenuItem
        // 
        this.addTriggerToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.appfolder_16x16;
        this.addTriggerToolStripMenuItem.Name = "addTriggerToolStripMenuItem";
        this.addTriggerToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.addTriggerToolStripMenuItem.Text = "Add &Trigger";
        // 
        // addPublisherToolStripMenuItem
        // 
        this.addPublisherToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.move_16x16;
        this.addPublisherToolStripMenuItem.Name = "addPublisherToolStripMenuItem";
        this.addPublisherToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.addPublisherToolStripMenuItem.Text = "Add &Publisher";
        // 
        // addPrebuildToolStripMenuItem
        // 
        this.addPrebuildToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.window_protected_16x16;
        this.addPrebuildToolStripMenuItem.Name = "addPrebuildToolStripMenuItem";
        this.addPrebuildToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.addPrebuildToolStripMenuItem.Text = "Add Pre&build";
        // 
        // copySepMenuItem
        // 
        this.copySepMenuItem.Name = "copySepMenuItem";
        this.copySepMenuItem.Size = new System.Drawing.Size(164, 6);
        // 
        // copyProjectMenuItem
        // 
        this.copyProjectMenuItem.Image = global::CCNetConfig.Properties.Resources.addbox_16x16;
        this.copyProjectMenuItem.Name = "copyProjectMenuItem";
        this.copyProjectMenuItem.Size = new System.Drawing.Size(167, 22);
        this.copyProjectMenuItem.Text = "Copy XXX";
        this.copyProjectMenuItem.Click += new System.EventHandler(this.copyProjectMenuItem_Click);
        // 
        // copyProjectAsMenuItem
        // 
        this.copyProjectAsMenuItem.Image = global::CCNetConfig.Properties.Resources.addbox_16x16;
        this.copyProjectAsMenuItem.Name = "copyProjectAsMenuItem";
        this.copyProjectAsMenuItem.Size = new System.Drawing.Size(167, 22);
        this.copyProjectAsMenuItem.Text = "Copy Project As...";
        this.copyProjectAsMenuItem.Click += new System.EventHandler(this.copyProjectAsMenuItem_Click);
        // 
        // toolStripSeparator6
        // 
        this.toolStripSeparator6.Name = "toolStripSeparator6";
        this.toolStripSeparator6.Size = new System.Drawing.Size(164, 6);
        // 
        // copyItemsToolStripMenuItem
        // 
        this.copyItemsToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.copy_16x16;
        this.copyItemsToolStripMenuItem.Name = "copyItemsToolStripMenuItem";
        this.copyItemsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.copyItemsToolStripMenuItem.Text = "Copy Items";
        // 
        // cutItemToolStripMenuItem
        // 
        this.cutItemToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.cutfile_16x16;
        this.cutItemToolStripMenuItem.Name = "cutItemToolStripMenuItem";
        this.cutItemToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.cutItemToolStripMenuItem.Text = "Cut Item";
        // 
        // pasteNewItemToolStripMenuItem
        // 
        this.pasteNewItemToolStripMenuItem.Image = global::CCNetConfig.Properties.Resources.paste_16x16;
        this.pasteNewItemToolStripMenuItem.Name = "pasteNewItemToolStripMenuItem";
        this.pasteNewItemToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
        this.pasteNewItemToolStripMenuItem.Text = "Paste New Item";
        // 
        // configPanel
        // 
        this.configPanel.Controls.Add(this.pgProperties);
        this.configPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.configPanel.Location = new System.Drawing.Point(210, 0);
        this.configPanel.Name = "configPanel";
        this.configPanel.Size = new System.Drawing.Size(575, 277);
        this.configPanel.TabIndex = 20;
        // 
        // pgProperties
        // 
        this.pgProperties.AutoSizeProperties = true;
        this.pgProperties.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pgProperties.DrawFlatToolbar = true;
        // 
        // 
        // 
        this.pgProperties.HelpCommentDescription.AutoEllipsis = true;
        this.pgProperties.HelpCommentDescription.BackColor = System.Drawing.Color.Transparent;
        this.pgProperties.HelpCommentDescription.Cursor = System.Windows.Forms.Cursors.Default;
        this.pgProperties.HelpCommentDescription.Location = new System.Drawing.Point(3, 18);
        this.pgProperties.HelpCommentDescription.Name = "";
        this.pgProperties.HelpCommentDescription.Size = new System.Drawing.Size(569, 37);
        this.pgProperties.HelpCommentDescription.TabIndex = 1;
        this.pgProperties.HelpCommentImage = null;
        // 
        // 
        // 
        this.pgProperties.HelpCommentTitle.BackColor = System.Drawing.Color.Transparent;
        this.pgProperties.HelpCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
        this.pgProperties.HelpCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
        this.pgProperties.HelpCommentTitle.Location = new System.Drawing.Point(3, 3);
        this.pgProperties.HelpCommentTitle.Name = "";
        this.pgProperties.HelpCommentTitle.Size = new System.Drawing.Size(569, 15);
        this.pgProperties.HelpCommentTitle.TabIndex = 0;
        this.pgProperties.HelpCommentTitle.UseMnemonic = false;
        this.pgProperties.Location = new System.Drawing.Point(0, 0);
        this.pgProperties.Name = "pgProperties";
        this.pgProperties.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
        this.pgProperties.Size = new System.Drawing.Size(575, 277);
        this.pgProperties.TabIndex = 0;
        // 
        // 
        // 
        this.pgProperties.ToolStrip.AccessibleName = "ToolBar";
        this.pgProperties.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
        this.pgProperties.ToolStrip.AllowMerge = false;
        this.pgProperties.ToolStrip.AutoSize = false;
        this.pgProperties.ToolStrip.CanOverflow = false;
        this.pgProperties.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
        this.pgProperties.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
        this.pgProperties.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripButton});
        this.pgProperties.ToolStrip.Location = new System.Drawing.Point(0, 1);
        this.pgProperties.ToolStrip.Name = "";
        this.pgProperties.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
        this.pgProperties.ToolStrip.Size = new System.Drawing.Size(575, 25);
        this.pgProperties.ToolStrip.TabIndex = 1;
        this.pgProperties.ToolStrip.TabStop = true;
        this.pgProperties.ToolStrip.Text = "PropertyGridToolBar";
        this.pgProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgProperties_PropertyValueChanged);
        // 
        // helpToolStripButton
        // 
        this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.helpToolStripButton.Enabled = false;
        this.helpToolStripButton.Image = global::CCNetConfig.Properties.Resources.help_16x16;
        this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.helpToolStripButton.Name = "helpToolStripButton";
        this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
        // 
        // toolStripContainer1
        // 
        // 
        // toolStripContainer1.BottomToolStripPanel
        // 
        this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
        // 
        // toolStripContainer1.ContentPanel
        // 
        this.toolStripContainer1.ContentPanel.Controls.Add(this.configPanel);
        this.toolStripContainer1.ContentPanel.Controls.Add(this.webSplitter);
        this.toolStripContainer1.ContentPanel.Controls.Add(this.docPanel);
        this.toolStripContainer1.ContentPanel.Controls.Add(this.treeSplitter);
        this.toolStripContainer1.ContentPanel.Controls.Add(this.treePanel);
        this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(785, 432);
        this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
        this.toolStripContainer1.Name = "toolStripContainer1";
        this.toolStripContainer1.Size = new System.Drawing.Size(785, 503);
        this.toolStripContainer1.TabIndex = 23;
        this.toolStripContainer1.Text = "toolStripContainer1";
        // 
        // toolStripContainer1.TopToolStripPanel
        // 
        this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuToolStrip);
        this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.mainToolStrip);
        this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.editToolStrip);
        // 
        // statusStrip1
        // 
        this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
        this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
        this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configVersionStatusLabel});
        this.statusStrip1.Location = new System.Drawing.Point(0, 0);
        this.statusStrip1.Name = "statusStrip1";
        this.statusStrip1.Size = new System.Drawing.Size(785, 22);
        this.statusStrip1.TabIndex = 18;
        this.statusStrip1.Text = "statusStrip1";
        // 
        // configVersionStatusLabel
        // 
        this.configVersionStatusLabel.Name = "configVersionStatusLabel";
        this.configVersionStatusLabel.Size = new System.Drawing.Size(0, 17);
        this.configVersionStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // webSplitter
        // 
        this.webSplitter.AnimationDelay = 20;
        this.webSplitter.AnimationStep = 20;
        this.webSplitter.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
        this.webSplitter.ControlToHide = this.docPanel;
        this.webSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.webSplitter.ExpandParentForm = false;
        this.webSplitter.Location = new System.Drawing.Point(210, 277);
        this.webSplitter.MinExtra = 0;
        this.webSplitter.MinSize = 0;
        this.webSplitter.Name = "collapsibleSplitter1";
        this.webSplitter.TabIndex = 24;
        this.webSplitter.TabStop = false;
        this.webSplitter.UseAnimations = false;
        this.webSplitter.VisualStyle = CCNetConfig.Controls.VisualStyles.DoubleDots;
        // 
        // treeSplitter
        // 
        this.treeSplitter.AnimationDelay = 20;
        this.treeSplitter.AnimationStep = 20;
        this.treeSplitter.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
        this.treeSplitter.ControlToHide = this.treePanel;
        this.treeSplitter.ExpandParentForm = false;
        this.treeSplitter.Location = new System.Drawing.Point(202, 0);
        this.treeSplitter.MinExtra = 0;
        this.treeSplitter.MinSize = 0;
        this.treeSplitter.Name = "treeSplitter";
        this.treeSplitter.TabIndex = 23;
        this.treeSplitter.TabStop = false;
        this.treeSplitter.UseAnimations = false;
        this.treeSplitter.VisualStyle = CCNetConfig.Controls.VisualStyles.DoubleDots;
        // 
        // editToolStrip
        // 
        this.editToolStrip.Dock = System.Windows.Forms.DockStyle.None;
        this.editToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton});
        this.editToolStrip.Location = new System.Drawing.Point(168, 24);
        this.editToolStrip.Name = "editToolStrip";
        this.editToolStrip.Size = new System.Drawing.Size(81, 25);
        this.editToolStrip.TabIndex = 17;
        this.editToolStrip.Visible = false;
        // 
        // cutToolStripButton
        // 
        this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.cutToolStripButton.Enabled = false;
        this.cutToolStripButton.Image = global::CCNetConfig.Properties.Resources.cutfile_16x16;
        this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.cutToolStripButton.Name = "cutToolStripButton";
        this.cutToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.cutToolStripButton.Text = "toolStripButton1";
        // 
        // copyToolStripButton
        // 
        this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.copyToolStripButton.Enabled = false;
        this.copyToolStripButton.Image = global::CCNetConfig.Properties.Resources.copy_16x16;
        this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.copyToolStripButton.Name = "copyToolStripButton";
        this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.copyToolStripButton.Text = "toolStripButton2";
        // 
        // pasteToolStripButton
        // 
        this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.pasteToolStripButton.Enabled = false;
        this.pasteToolStripButton.Image = global::CCNetConfig.Properties.Resources.paste_16x16;
        this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.pasteToolStripButton.Name = "pasteToolStripButton";
        this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
        this.pasteToolStripButton.Text = "toolStripButton3";
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(785, 503);
        this.Controls.Add(this.toolStripContainer1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MainMenuStrip = this.menuToolStrip;
        this.Name = "MainForm";
        this.Text = "CCNet Config";
        this.treePanel.ResumeLayout(false);
        this.treePanel.PerformLayout();
        this.configurationToolStrip.ResumeLayout(false);
        this.configurationToolStrip.PerformLayout();
        this.menuToolStrip.ResumeLayout(false);
        this.menuToolStrip.PerformLayout();
        this.mainToolStrip.ResumeLayout(false);
        this.mainToolStrip.PerformLayout();
        this.docPanel.ResumeLayout(false);
        this.projectContextMenu.ResumeLayout(false);
        this.configPanel.ResumeLayout(false);
        this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
        this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
        this.toolStripContainer1.ContentPanel.ResumeLayout(false);
        this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
        this.toolStripContainer1.TopToolStripPanel.PerformLayout();
        this.toolStripContainer1.ResumeLayout(false);
        this.toolStripContainer1.PerformLayout();
        this.statusStrip1.ResumeLayout(false);
        this.statusStrip1.PerformLayout();
        this.editToolStrip.ResumeLayout(false);
        this.editToolStrip.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList treeImages;
    private System.Windows.Forms.TreeView tvProjects;
    private System.Windows.Forms.Panel treePanel;
    private System.Windows.Forms.ToolStrip configurationToolStrip;
    private System.Windows.Forms.MenuStrip menuToolStrip;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStrip mainToolStrip;
    private System.Windows.Forms.ToolStripButton addProjectToolButton;
    private System.Windows.Forms.ToolStripButton removeProjectToolButton;
    private System.Windows.Forms.ToolStripButton saveConfigurationToolButton;
    private System.Windows.Forms.ToolStripButton newConfigurationToolButton;
    private System.Windows.Forms.ToolStripButton openConfigurationToolButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripDropDownButton addProjectItems;
    private System.Windows.Forms.ToolStripMenuItem tasksToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem publishersToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem triggersToolStripMenuItem;
    private System.Windows.Forms.Panel docPanel;
    private System.Windows.Forms.WebBrowser wbDocs;
    private System.Windows.Forms.ContextMenuStrip projectContextMenu;
    private System.Windows.Forms.ToolStripMenuItem addTaskToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addTriggerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addPublisherToolStripMenuItem;
    private System.Windows.Forms.Panel configPanel;
    private System.Windows.Forms.ToolStripButton submitBugToolStripButton;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cCNetDocumentationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cCNetConfigToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem reportABugToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem aboutCCNetConfigToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripButton checkForUpdatesToolStripButton;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private CCNetConfig.Controls.PropertyGridEx pgProperties;
    private System.Windows.Forms.ToolStripButton helpToolStripButton;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem configurationTreeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem documentationBrowserToolStripMenuItem;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private CCNetConfig.Controls.CollapsibleSplitter webSplitter;
    private CCNetConfig.Controls.CollapsibleSplitter treeSplitter;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton copyProjectToolStripButton;
    private System.Windows.Forms.ToolStripSeparator copySepMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyProjectMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyProjectAsMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripStatusLabel configVersionStatusLabel;
    private System.Windows.Forms.ToolStripMenuItem prebuildToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addPrebuildToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripButton configPropertiesToolStripButton;
    private System.Windows.Forms.ToolStripButton showOptionsDialogToolStripButton;
    private System.Windows.Forms.ToolStripMenuItem copyItemsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStrip editToolStrip;
    private System.Windows.Forms.ToolStripButton cutToolStripButton;
    private System.Windows.Forms.ToolStripButton copyToolStripButton;
    private System.Windows.Forms.ToolStripButton pasteToolStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem cutItemToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteNewItemToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem validateToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    private System.Windows.Forms.ToolStripButton validateToolStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    private System.Windows.Forms.ToolStripMenuItem securityWizardsToolstripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem previewMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
  }
}