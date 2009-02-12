/*
 * Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using CCNetConfig.Components;
using CCNetConfig.Components.Nodes;
using CCNetConfig.Controls;
using CCNetConfig.Core;
using CCNetConfig.Core.Exceptions;
using CCNetConfig.Exceptions;
using CCNetConfig.UI.Wizards;

namespace CCNetConfig.UI {
  /// <summary>
  /// The Main CCNetConfig Form
  /// </summary>
  public partial class MainForm : Form {
    private CruiseControlTreeNode rootNode;
    private ProjectQueuesTreeNode queuesNode = null;
    private ServerSecurityNode securityNode = null;
    private ContextMenuStrip genericContextMenu;
    private bool configModified = false;
    private TreeNode _dragNode = null;
    private TreeNode _hoverNode = null;
    private FileInfo loadedConfigFile = null;
    private CloneableList<Version> _ccnetVersions = null;
    private BackupControler _backupControler = null;
    private QueueImageKeys queueImageKeys;
    private SecurityImageKeys securityImageKeys;
    private ValidationForm validation;

    private EventHandler _pasteMenuItemEventHandler = null;
    private EventHandler _copyMenuItemEventHandler = null;
    private EventHandler _cutMenuItemEventHandler = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
        genericContextMenu = new ContextMenuStrip();
        _ccnetVersions = Util.GetCCNetVersions();
        _backupControler = new BackupControler();
        InitializeComponent();
        this.AllowDrop = true;
        Initialize();

        queueImageKeys = new QueueImageKeys(
            treeImages.Images.IndexOfKey("queue"),
            treeImages.Images.IndexOfKey("queueConfig"),
            treeImages.Images.IndexOfKey("project"));
        securityImageKeys = new SecurityImageKeys(
            treeImages.Images.IndexOfKey("security_16x16"));
        validation = new ValidationForm(this);
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    private void Initialize ( ) {

      this.Text = string.IsNullOrEmpty ( Application.ProductName ) ? "CCNetConfig" : Application.ProductName;
      // attach to events for the webbrowser.
      this.wbDocs.Navigating += new WebBrowserNavigatingEventHandler ( wbDocs_Navigating );
      this.wbDocs.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler ( wbDocs_DocumentCompleted );
      this.wbDocs.CanGoBackChanged += new EventHandler ( wbDocs_CanGoBackChanged );
      this.wbDocs.CanGoForwardChanged += new EventHandler ( wbDocs_CanGoBackChanged );
      // listen for mouse click on the nodes
      tvProjects.NodeMouseClick += new TreeNodeMouseClickEventHandler ( tvProjects_NodeMouseClick );

      // listen for events on the property grid
      this.pgProperties.HelpRequested += new HelpEventHandler ( pgProperties_HelpRequested );
      this.pgProperties.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler ( pgProperties_SelectedGridItemChanged );
      this.pgProperties.SelectedObjectsChanged += new EventHandler ( pgProperties_SelectedObjectsChanged );
      this.pgProperties.AutoSizeProperties = true;
      this.pgProperties.HelpCommentTitle.Font = new Font ( this.Font.FontFamily, 13f, FontStyle.Bold );
      this.pgProperties.DrawFlatToolbar = false;
      this.helpToolStripButton.Click += new EventHandler ( helpToolStripButton_Click );
      // remove propertypages
      this.pgProperties.ToolStrip.Items.RemoveAt ( 4 );
      this.pgProperties.ToolStrip.GripStyle = ToolStripGripStyle.Visible;


      this.submitBugToolStripButton.Enabled = Program.BugTracker.Enabled;

      this.tvProjects.DragDrop += new DragEventHandler ( tvProjects_DragDrop );
      this.tvProjects.ItemDrag += new ItemDragEventHandler ( tvProjects_ItemDrag );
      this.tvProjects.DragOver += new DragEventHandler ( tvProjects_DragOver );
      this.tvProjects.AllowDrop = true;
      string filePath = string.Empty;
      if ( Program.CommandLineArguments.ContainsParam ( "file" ) || Program.CommandLineArguments.ContainsParam ( "f" ) )
        filePath = string.IsNullOrEmpty ( Program.CommandLineArguments[ "file" ] ) ? Program.CommandLineArguments[ "f" ] : Program.CommandLineArguments[ "file" ];
      if ( !string.IsNullOrEmpty ( filePath ) ) {
        FileInfo loadedFile = new FileInfo ( filePath );
        if ( loadedFile.Exists )
          LoadConfigurationFile ( loadedFile );
      }
    }


    #region project treeview event handlers
    /// <summary>
    /// Handles the DragOver event of the tvProjects control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
    void tvProjects_DragOver ( object sender, DragEventArgs e ) {
      Point mousePos = new Point ( e.X, e.Y );
      TreeNode positionNode = tvProjects.GetNodeAt ( tvProjects.PointToClient ( mousePos ) );
      if ( positionNode == null )
        return;
      if ( _hoverNode != null && _hoverNode != positionNode ) {
        _hoverNode.ForeColor = SystemColors.WindowText;
        _hoverNode.BackColor = tvProjects.BackColor;

        _hoverNode = positionNode;

        _hoverNode.ForeColor = SystemColors.HighlightText;
        _hoverNode.BackColor = SystemColors.Highlight;
      } else {
        _hoverNode = positionNode;
        _hoverNode.ForeColor = SystemColors.HighlightText;
        _hoverNode.BackColor = SystemColors.Highlight;
      }

      if ( _dragNode != null ) {
        if ( _hoverNode.GetType ( ) == typeof ( TriggerItemTreeNode ) && _dragNode.GetType ( ) == typeof ( TriggerItemTreeNode ) )
          e.Effect = DragDropEffects.Move;
        else if ( _hoverNode.GetType ( ) == typeof ( PublisherTaskItemTreeNode ) && _dragNode.GetType ( ) == typeof ( PublisherTaskItemTreeNode ) ) {
          if ( _hoverNode.Parent == _dragNode.Parent )
            e.Effect = DragDropEffects.Move;
          else
            e.Effect = DragDropEffects.None;
        } else
          e.Effect = DragDropEffects.None;
      }
    }

    /// <summary>
    /// Handles the ItemDrag event of the tvProjects control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.ItemDragEventArgs"/> instance containing the event data.</param>
    void tvProjects_ItemDrag ( object sender, ItemDragEventArgs e ) {
      if ( e.Button == MouseButtons.Left && ( e.Item.GetType ( ) == typeof ( PublisherTaskItemTreeNode ) ||
        e.Item.GetType ( ) == typeof ( TriggerItemTreeNode ) ) ) {
        _dragNode = ( TreeNode ) e.Item;
        tvProjects.SelectedNode = _dragNode;
        this.DoDragDrop ( _dragNode, DragDropEffects.Move );
      } else
        return;
    }

    /// <summary>
    /// Handles the DragDrop event of the tvProjects control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
    void tvProjects_DragDrop ( object sender, DragEventArgs e ) {
      Point mousePos = new Point ( e.X, e.Y );
      TreeNode positionNode = tvProjects.GetNodeAt ( tvProjects.PointToClient ( mousePos ) );
      if ( positionNode == null )
        return;

      if ( _hoverNode != null ) {
        _hoverNode.ForeColor = SystemColors.WindowText;
        _hoverNode.BackColor = tvProjects.BackColor;
      }

      if ( _dragNode != null ) {
        try {
          if ( _hoverNode.GetType ( ) == typeof ( TriggerItemTreeNode ) && _dragNode.GetType ( ) == typeof ( TriggerItemTreeNode ) ) {
            TriggerItemTreeNode nclone = ( TriggerItemTreeNode ) _dragNode.Clone ( );
            nclone.Trigger = ( ( TriggerItemTreeNode ) _dragNode ).Trigger;
            if ( nclone != null ) {
              int index = _dragNode.Parent.Nodes.IndexOf ( _hoverNode );
              if ( index > -1 ) {
                _dragNode.Parent.Nodes.Insert ( index, nclone );
                ProjectTreeNode ptn = ( ProjectTreeNode ) nclone.Parent.Parent;
                if ( ptn != null ) {
                  ptn.Project.Triggers.Remove ( ( ( TriggerItemTreeNode ) _dragNode ).Trigger );
                  ptn.Project.Triggers.Insert ( index, nclone.Trigger );
                }
                _dragNode.Remove ( );
              }
              tvProjects.SelectedNode = nclone;
            }
          } else if ( _hoverNode.GetType ( ) == typeof ( PublisherTaskItemTreeNode ) && _dragNode.GetType ( ) == typeof ( PublisherTaskItemTreeNode ) ) {
            PublisherTaskItemTreeNode nclone = ( PublisherTaskItemTreeNode ) _dragNode.Clone ( );
            nclone.PublisherTask = ( ( PublisherTaskItemTreeNode ) _dragNode ).PublisherTask;
            if ( nclone != null ) {
              int index = _dragNode.Parent.Nodes.IndexOf ( _hoverNode );
              if ( index > -1 ) {
                _dragNode.Parent.Nodes.Insert ( index, nclone );
                ProjectTreeNode ptn = ( ProjectTreeNode ) nclone.Parent.Parent;
                if ( ptn != null ) {
                  bool isPub = _dragNode.Parent.GetType ( ) == typeof ( PublishersTreeNode );
                  if ( isPub ) {
                    ptn.Project.Publishers.Remove ( ( ( PublisherTaskItemTreeNode ) _dragNode ).PublisherTask );
                    ptn.Project.Publishers.Insert ( index, nclone.PublisherTask );
                  } else {
                    ptn.Project.Tasks.Remove ( ( ( PublisherTaskItemTreeNode ) _dragNode ).PublisherTask );
                    ptn.Project.Tasks.Insert ( index, nclone.PublisherTask );
                  }
                }
                _dragNode.Remove ( );
                _dragNode.Remove ( );
              }
              tvProjects.SelectedNode = nclone;
            }
          }
          OnConfigurationModified ( new CancelEventArgs ( false ) );
        } catch ( Exception ex ) {
          Console.WriteLine ( ex.ToString ( ) );
        }

      }
    }

    /// <summary>
    /// Handles the NodeMouseClick event of the tvProjects control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void tvProjects_NodeMouseClick ( object sender, TreeNodeMouseClickEventArgs e ) {
      this.copyProjectToolStripButton.Enabled = false;

      if (e.Node is IInteractiveTreeNode)
      {
          // Changed to use an interface so the node functionality can be encapsulated
          (e.Node as IInteractiveTreeNode).HandleMouseClick(this, e);
      }
      else
      {
          // Old-style of checking for each type
          Type t = e.Node.GetType();
          if (t == typeof(ProjectTreeNode))
              OnProjectTreeNodeMouseClick(e);
          else if (t == typeof(CruiseControlTreeNode))
              OnCruiseControlTreeNodeMouseClick(e);
          else if (t == typeof(TasksTreeNode))
              OnTasksTreeNodeMouseClick(e);
          else if (t == typeof(PublishersTreeNode))
              OnPublishersTreeNodeMouseClick(e);
          else if (t == typeof(TriggersTreeNode))
              OnTriggersTreeNodeMouseClick(e);
          else if (t == typeof(TriggerItemTreeNode))
              OnTriggerItemTreeNodeMouseClick(e);
          else if (t == typeof(PrebuildTreeNode))
              OnPrebuildTreeNodeMouseClick(e);
          else if (t == typeof(PublisherTaskItemTreeNode))
          {
              if (e.Node.Parent.GetType() == typeof(PublishersTreeNode))
                  OnPublisherItemTreeNodeMouseClick(e);
              else if (e.Node.Parent.GetType() == typeof(TasksTreeNode))
                  OnTaskItemTreeNodeMouseClick(e);
              else if (e.Node.Parent.GetType() == typeof(PrebuildTreeNode))
                  OnPublisherItemTreeNodeMouseClick(e);
          }
          else if (t == typeof(ExtensionItemTreeNode))
              OnExtensionItemTreeNodeMouseClick(e);
          else if (t == typeof(ExtensionTreeNode))
              OnExtensionTreeNodeMouseClick(e);
      }

    }
    #endregion

    #region PropertyGrid Event Handlers
    /// <summary>
    /// raised when the selected object of the property grid changes
    /// </summary>
    /// <param name="sender">the property grid</param>
    /// <param name="e"></param>
    void pgProperties_SelectedObjectsChanged ( object sender, EventArgs e ) {
      // check if the propertygrid has a value.
      // if it does, see if the object type implements ICCNetDocumentation and set the helpbotton enabled based on that
      if ( pgProperties.SelectedObject != null ) {
        this.helpToolStripButton.Enabled = pgProperties.SelectedObject.GetType ( ).GetInterface ( typeof ( ICCNetDocumentation ).FullName ) != null;
        if ( this.helpToolStripButton.Enabled )
          this.helpToolStripButton.ToolTipText = string.Format ( Properties.Resources.HelpToolTip, pgProperties.SelectedObject.ToString ( ) );
        else
          this.helpToolStripButton.ToolTipText = string.Empty;
      } else {
        this.helpToolStripButton.Enabled = false;
        this.helpToolStripButton.ToolTipText = string.Empty;
      }
    }

    /// <summary>
    /// Handles the SelectedGridItemChanged event of the pgProperties control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.SelectedGridItemChangedEventArgs"/> instance containing the event data.</param>
    void pgProperties_SelectedGridItemChanged ( object sender, SelectedGridItemChangedEventArgs e ) {
      //see if the object type implements ICCNetDocumentation and set the helpbotton enabled based on that
      try {
        if ( e.NewSelection.Value == null ) {
          this.helpToolStripButton.Enabled = e.NewSelection.PropertyDescriptor.PropertyType.GetInterface ( typeof ( ICCNetDocumentation ).FullName ) != null;
          this.helpToolStripButton.ToolTipText = string.Format ( Properties.Resources.HelpToolTip, e.NewSelection.PropertyDescriptor.PropertyType.Name );
        } else {
          this.helpToolStripButton.Enabled = e.NewSelection.Value.GetType ( ).GetInterface ( typeof ( ICCNetDocumentation ).FullName ) != null;
          if ( this.helpToolStripButton.Enabled )
            this.helpToolStripButton.ToolTipText = string.Format ( Properties.Resources.HelpToolTip, e.NewSelection.Value.GetType ( ).Name );
          else
            this.helpToolStripButton.ToolTipText = string.Empty;
        }
      } catch { }
    }

    /// <summary>
    /// Handles the HelpRequested event of the pgProperties control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="hlpevent">The <see cref="System.Windows.Forms.HelpEventArgs"/> instance containing the event data.</param>
    void pgProperties_HelpRequested ( object sender, HelpEventArgs hlpevent ) {
      // if help is enabled, check if the griditem implements the ICCNetDocumentation, if it does, "performclick" on the help button
      if ( this.helpToolStripButton.Enabled && !hlpevent.Handled ) {
        if ( this.pgProperties.SelectedGridItem.PropertyDescriptor.PropertyType.GetInterface ( typeof ( ICCNetDocumentation ).FullName ) != null )
          this.helpToolStripButton.PerformClick ( );
      }
    }

    /// <summary>
    /// Handles the PropertyValueChanged event of the pgProperties control.
    /// </summary>
    /// <param name="s">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.PropertyValueChangedEventArgs"/> instance containing the event data.</param>
    private void pgProperties_PropertyValueChanged ( object s, PropertyValueChangedEventArgs e ) {
      if ( this.pgProperties.SelectedObject.GetType ( ) == typeof ( Project ) ) {
        Project proj = ( Project ) this.pgProperties.SelectedObject;
        if ( string.Compare ( e.ChangedItem.Label, "(Name)", true ) == 0 ) {
          if ( rootNode.CruiseControl.Projects.GetCountByName ( e.ChangedItem.Value.ToString ( ) ) > 1 ) {
            MessageBox.Show ( this, Properties.Resources.ProjectExists, Properties.Resources.ProjectExistsTitle, MessageBoxButtons.OK, MessageBoxIcon.Error );
            proj.Name = e.OldValue.ToString ( );
            return;
          } else {
            foreach ( ProjectTreeNode ptn in rootNode.Nodes ) {
              if ( string.Compare ( e.OldValue.ToString ( ), ptn.Text, false ) == 0 ) {
                //rootNode.CruiseControl.Projects.Remove ( e.OldValue.ToString ( ) );
                //rootNode.CruiseControl.Projects.Add ( ptn.Project );
                ptn.Text = e.ChangedItem.Value.ToString ( );
              }
            }
          }
        }
      }

      OnConfigurationModified ( new CancelEventArgs ( false ) );

      // now check if we have documentation available for the value we changed
      if ( e.ChangedItem.Value != null ) {
        this.helpToolStripButton.Enabled = e.ChangedItem.Value.GetType ( ).GetInterface ( typeof ( ICCNetDocumentation ).FullName ) != null;
        if ( this.helpToolStripButton.Enabled )
          this.helpToolStripButton.ToolTipText = string.Format ( Properties.Resources.HelpToolTip, e.ChangedItem.Value.ToString ( ) );
        else
          this.helpToolStripButton.ToolTipText = string.Empty;
      } else {
        this.helpToolStripButton.Enabled = e.ChangedItem.PropertyDescriptor.PropertyType.GetType ( ).GetInterface ( typeof ( ICCNetDocumentation ).FullName ) != null;
        if ( this.helpToolStripButton.Enabled )
          this.helpToolStripButton.ToolTipText = string.Format ( Properties.Resources.HelpToolTip, e.ChangedItem.PropertyDescriptor.PropertyType.Name );
        else
          this.helpToolStripButton.ToolTipText = string.Empty;
      }
    }

    /// <summary>
    /// Properties the grid check removed item.
    /// </summary>
    /// <param name="removedObject">The removed object.</param>
    private void PropertyGridCheckRemovedItem ( object removedObject ) {
      if ( this.pgProperties.SelectedObject == removedObject )
        pgProperties.SelectedObject = null;
    }
    #endregion

    #region doc browser event handlers

    /// <summary>
    /// Handles the CanGoBackChanged event of the wbDocs control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void wbDocs_CanGoBackChanged ( object sender, EventArgs e ) {
    }

    /// <summary>
    /// Handles the Navigating event of the wbDocs control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.WebBrowserNavigatingEventArgs"/> instance containing the event data.</param>
    void wbDocs_Navigating ( object sender, WebBrowserNavigatingEventArgs e ) {

    }

    /// <summary>
    /// Handles the DocumentCompleted event of the wbDocs control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.WebBrowserDocumentCompletedEventArgs"/> instance containing the event data.</param>
    void wbDocs_DocumentCompleted ( object sender, WebBrowserDocumentCompletedEventArgs e ) {
    }
    #endregion

    #region Toolbar Menu / Context Menu
    /// <summary>
    /// Creates the remove publisher menu item.
    /// </summary>
    /// <param name="parentProjectNode">The parent project node.</param>
    /// <param name="ptitn">The ptitn.</param>
    private void CreateRemovePublisherMenuItem ( ProjectTreeNode parentProjectNode, PublisherTaskItemTreeNode ptitn ) {
      genericContextMenu.Items.Clear ( );
      PublisherToolStripMenuItem ptsmi = new PublisherToolStripMenuItem ( "Remove {0}", ptitn, parentProjectNode );
      ptsmi.Image = CCNetConfig.Properties.Resources.delete_16x16;
      ptsmi.Click += new EventHandler ( removePublisherToolStripMenuItem_Click );
      genericContextMenu.Items.Add ( ptsmi );
      genericContextMenu.Items.Add ( new ToolStripSeparator ( ) );
      ptsmi = new PublisherToolStripMenuItem ( "Copy {0}", ptitn, parentProjectNode );
      ptsmi.Image = CCNetConfig.Properties.Resources.copy_16x16;
      ptsmi.Click += delegate ( object sender, EventArgs e ) {
        ClipboardManager.CopyToClipboard<PublisherTask> ( ptitn.PublisherTask );
      };
      genericContextMenu.Items.Add ( ptsmi );
      ptsmi = new PublisherToolStripMenuItem ( "Cut {0}", ptitn, parentProjectNode );
      ptsmi.Image = CCNetConfig.Properties.Resources.cutfile_16x16;
      ptsmi.Click += delegate ( object sender, EventArgs e ) {
        ClipboardManager.CopyToClipboard<PublisherTask> ( ptitn.PublisherTask );
        ptitn.Remove ( );
        OnConfigurationModified ( new CancelEventArgs ( false ) );
      };
      genericContextMenu.Items.Add ( ptsmi );
    }

    /// <summary>
    /// Creates the remove task menu item.
    /// </summary>
    /// <param name="parentProjectNode">The parent project node.</param>
    /// <param name="ptitn">The ptitn.</param>
    private void CreateRemoveTaskMenuItem ( ProjectTreeNode parentProjectNode, PublisherTaskItemTreeNode ptitn ) {
      genericContextMenu.Items.Clear ( );
      TaskToolStripMenuItem ptsmi = new TaskToolStripMenuItem ( "Remove {0}", ptitn, parentProjectNode );
      ptsmi.Image = CCNetConfig.Properties.Resources.delete_16x16;
      ptsmi.Click += new EventHandler ( removeTaskToolStripMenuItem_Click );
      genericContextMenu.Items.Add ( ptsmi );
      genericContextMenu.Items.Add ( new ToolStripSeparator ( ) );
      ptsmi = new TaskToolStripMenuItem ( "Copy {0}", ptitn, parentProjectNode );
      ptsmi.Image = CCNetConfig.Properties.Resources.copy_16x16;
      ptsmi.Click += delegate ( object sender, EventArgs e ) {
        ClipboardManager.CopyToClipboard<PublisherTask> ( ptitn.PublisherTask );
      };
      genericContextMenu.Items.Add ( ptsmi );
      ptsmi = new TaskToolStripMenuItem ( "Cut {0}", ptitn, parentProjectNode );
      ptsmi.Image = CCNetConfig.Properties.Resources.cutfile_16x16;
      ptsmi.Click += delegate ( object sender, EventArgs e ) {
        ClipboardManager.CopyToClipboard<PublisherTask> ( ptitn.PublisherTask );
        ptitn.Remove ( );
        OnConfigurationModified ( new CancelEventArgs ( false ) );
      };
      genericContextMenu.Items.Add ( ptsmi );
    }

    /// <summary>
    /// Creates the remove trigger menu item.
    /// </summary>
    /// <param name="parentProjectNode">The parent project node.</param>
    /// <param name="ttitn">The ttitn.</param>
    private void CreateRemoveTriggerMenuItem ( ProjectTreeNode parentProjectNode, TriggerItemTreeNode ttitn ) {
      genericContextMenu.Items.Clear ( );
      TriggerToolStripMenuItem ttsmi = new TriggerToolStripMenuItem ( "Remove {0}", ttitn, parentProjectNode );
      ttsmi.Image = treeImages.Images[ "remove" ];
      ttsmi.Click += new EventHandler ( removeTriggerToolStripMenuItem_Click );
      genericContextMenu.Items.Add ( ttsmi );
      genericContextMenu.Items.Add ( new ToolStripSeparator ( ) );
      ttsmi = new TriggerToolStripMenuItem ( "Copy {0}", ttitn, parentProjectNode );
      ttsmi.Image = treeImages.Images[ "copy" ];
      ttsmi.Click += delegate ( object sender, EventArgs e ) {
        ClipboardManager.CopyToClipboard<Trigger> ( ttitn.Trigger );
      };
      genericContextMenu.Items.Add ( ttsmi );
      ttsmi = new TriggerToolStripMenuItem ( "Cut {0}", ttitn, parentProjectNode );
      ttsmi.Image = CCNetConfig.Properties.Resources.cutfile_16x16;
      ttsmi.Click += delegate ( object sender, EventArgs e ) {
        ClipboardManager.CopyToClipboard<Trigger> ( ttitn.Trigger );
        ttitn.Remove ( );
        OnConfigurationModified ( new CancelEventArgs ( false ) );
      };
      genericContextMenu.Items.Add ( ttsmi );
    }

    /// <summary>
    /// adds the triggers to the toolbar menu and the contextMenu
    /// </summary>
    private void PopulateTriggers ( ) {
      addTriggerToolStripMenuItem.DropDownItems.Clear ( );
      foreach ( Type t in Core.Util.Triggers ) {
        if ( ( CCNetConfig.Core.Util.UserSettings.Components.Contains ( t.FullName ) && !CCNetConfig.Core.Util.UserSettings.Components[ t.FullName ].Display ) )
          continue;

        Version minVer = Core.Util.GetMinimumVersion ( t );
        Version maxVer = Core.Util.GetMaximumVersion ( t );
        Version exactVer = Core.Util.GetExactVersion ( t );
        if ( Core.Util.IsInVersionRange ( minVer, maxVer, Core.Util.CurrentConfigurationVersion ) || Core.Util.IsExactVersion ( exactVer, Core.Util.CurrentConfigurationVersion ) ) {

          ToolStripMenuItem item = new ToolStripMenuItem ( t.Name );
          item.Image = treeImages.Images[ "application" ];
          // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
          item.Tag = t;
          item.Click += new EventHandler ( triggerItem_Click );
          triggersToolStripMenuItem.DropDownItems.Add ( item );
          ToolStripMenuItem item2 = new ToolStripMenuItem ( t.Name );
          item2.Image = treeImages.Images[ "application" ];
          // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
          item2.Tag = t;
          item2.Click += new EventHandler ( triggerItem_Click );
          addTriggerToolStripMenuItem.DropDownItems.Add ( item2 );
        }
      }
    }

    /// <summary>
    /// Handles the click of a trigger menu item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void triggerItem_Click ( object sender, EventArgs e ) {
      // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
      Type t = ( Type ) ( ( ToolStripMenuItem ) sender ).Tag;
      Trigger trigger = ( Trigger ) Util.CreateInstanceOfType ( t );
      if ( projectContextMenu != null && projectContextMenu.Tag != null && projectContextMenu.Tag.GetType ( ) == typeof ( ProjectTreeNode ) ) {
        ProjectTreeNode ptn = ( ProjectTreeNode ) projectContextMenu.Tag;
        TriggersTreeNode tTN = ( TriggersTreeNode ) Util.FindNodeByType ( ptn, typeof ( TriggersTreeNode ) );
        if ( tTN != null ) {
          TriggerItemTreeNode titn = new TriggerItemTreeNode ( trigger, treeImages.Images.IndexOfKey ( "triggers" ) );
          ptn.Project.Triggers.Add ( trigger );
          tTN.Nodes.Add ( titn );
          OnConfigurationModified ( new CancelEventArgs ( false ) );

          if ( !tTN.IsExpanded )
            tTN.Expand ( );
        }
      }

    }

    /// <summary>
    /// adds tasks to the toolbar menu and the contextmenu
    /// </summary>
    private void PopulateTasks ( ) {
      addTaskToolStripMenuItem.DropDownItems.Clear ( );
      foreach ( Type t in Core.Util.PublisherTasks ) {
        if ( ( CCNetConfig.Core.Util.UserSettings.Components.Contains ( t.FullName ) && !CCNetConfig.Core.Util.UserSettings.Components[ t.FullName ].Display ) )
          continue;
        Version minVer = Core.Util.GetMinimumVersion ( t );
        Version maxVer = Core.Util.GetMaximumVersion ( t );
        Version exactVer = Core.Util.GetExactVersion ( t );
        if ( Core.Util.IsInVersionRange ( minVer, maxVer, Core.Util.CurrentConfigurationVersion ) || Core.Util.IsExactVersion ( exactVer, Core.Util.CurrentConfigurationVersion ) ) {
          ToolStripMenuItem item = new ToolStripMenuItem ( t.Name );
          item.Image = treeImages.Images[ "application" ];
          // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
          item.Tag = t;
          item.Click += new EventHandler ( taskItem_Click );
          tasksToolStripMenuItem.DropDownItems.Add ( item );
          ToolStripMenuItem item2 = new ToolStripMenuItem ( t.Name );
          item2.Image = treeImages.Images[ "application" ];
          // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
          item2.Tag = t;
          item2.Click += new EventHandler ( taskItem_Click );
          addTaskToolStripMenuItem.DropDownItems.Add ( item2 );
        }
      }
    }

    /// <summary>
    /// handles the click of a task menuitem
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void taskItem_Click ( object sender, EventArgs e ) {
      // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
      Type t = ( Type ) ( ( ToolStripMenuItem ) sender ).Tag;
      PublisherTask pt = ( PublisherTask ) Util.CreateInstanceOfType ( t );
      if ( projectContextMenu != null && projectContextMenu.Tag != null && projectContextMenu.Tag.GetType ( ) == typeof ( ProjectTreeNode ) ) {
        ProjectTreeNode ptn = ( ProjectTreeNode ) projectContextMenu.Tag;
        TasksTreeNode tTN = ( TasksTreeNode ) Util.FindNodeByType ( ptn, typeof ( TasksTreeNode ) );
        if ( tTN != null ) {
          PublisherTaskItemTreeNode ptitn = new PublisherTaskItemTreeNode ( pt, treeImages.Images.IndexOfKey ( "tasks" ) );
          ptn.Project.Tasks.Add ( pt );
          tTN.Nodes.Add ( ptitn );
          OnConfigurationModified ( new CancelEventArgs ( false ) );

          if ( !tTN.IsExpanded )
            tTN.Expand ( );
        }
      }
    }

    /// <summary>
    /// adds publishers to the toolbar menu and the context menu
    /// </summary>
    private void PopulatePublishers ( ) {
      addPublisherToolStripMenuItem.DropDownItems.Clear ( );
      foreach ( Type t in Core.Util.PublisherTasks ) {
        if ( ( CCNetConfig.Core.Util.UserSettings.Components.Contains ( t.FullName ) && !CCNetConfig.Core.Util.UserSettings.Components[ t.FullName ].Display ) )
          continue;
        Version minVer = Core.Util.GetMinimumVersion ( t );
        Version maxVer = Core.Util.GetMaximumVersion ( t );
        Version exactVer = Core.Util.GetExactVersion ( t );
        if ( Core.Util.IsInVersionRange ( minVer, maxVer, Core.Util.CurrentConfigurationVersion ) || Core.Util.IsExactVersion ( exactVer, Core.Util.CurrentConfigurationVersion ) ) {
          ToolStripMenuItem item = new ToolStripMenuItem ( t.Name );
          item.Image = treeImages.Images[ "application" ];
          // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
          item.Tag = t;
          item.Click += new EventHandler ( publisherItem_Click );
          publishersToolStripMenuItem.DropDownItems.Add ( item );
          ToolStripMenuItem item2 = new ToolStripMenuItem ( t.Name );
          item2.Image = treeImages.Images[ "application" ];
          // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
          item2.Tag = t;
          item2.Click += new EventHandler ( publisherItem_Click );
          addPublisherToolStripMenuItem.DropDownItems.Add ( item2 );
        }
      }
    }

    /// <summary>
    /// adds prebuild to the toolbar menu and the context menu
    /// </summary>
    private void PopulatePrebuild ( ) {
      addPrebuildToolStripMenuItem.DropDownItems.Clear ( );
      foreach ( Type t in Core.Util.PublisherTasks ) {
        if ( ( CCNetConfig.Core.Util.UserSettings.Components.Contains ( t.FullName ) && !CCNetConfig.Core.Util.UserSettings.Components[ t.FullName ].Display ) )
          continue;
        Version minVer = Core.Util.GetMinimumVersion ( t );
        Version maxVer = Core.Util.GetMaximumVersion ( t );
        Version exactVer = Core.Util.GetExactVersion ( t );
        if ( Core.Util.IsInVersionRange ( minVer, maxVer, Core.Util.CurrentConfigurationVersion ) || Core.Util.IsExactVersion ( exactVer, Core.Util.CurrentConfigurationVersion ) ) {
          ToolStripMenuItem item = new ToolStripMenuItem ( t.Name );
          item.Image = treeImages.Images[ "application" ];
          // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
          item.Tag = t;
          item.Click += new EventHandler ( preBuildItem_Click );
          prebuildToolStripMenuItem.DropDownItems.Add ( item );
          ToolStripMenuItem item2 = new ToolStripMenuItem ( t.Name );
          item2.Image = treeImages.Images[ "application" ];
          // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
          item2.Tag = t;
          item2.Click += new EventHandler ( preBuildItem_Click );
          addPrebuildToolStripMenuItem.DropDownItems.Add ( item2 );
        }
      }
    }

    /// <summary>
    /// Handles the Click event of the preBuildItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void preBuildItem_Click ( object sender, EventArgs e ) {
      // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
      Type t = ( Type ) ( ( ToolStripMenuItem ) sender ).Tag;
      PublisherTask pt = Util.CreateInstanceOfType ( t ) as PublisherTask;
      if ( projectContextMenu != null && projectContextMenu.Tag != null && projectContextMenu.Tag.GetType ( ) == typeof ( ProjectTreeNode ) ) {
        ProjectTreeNode ptn = projectContextMenu.Tag as ProjectTreeNode;
        PrebuildTreeNode pbTN = Util.FindNodeByType ( ptn, typeof ( PrebuildTreeNode ) ) as PrebuildTreeNode;
        if ( pbTN != null ) {
          PublisherTaskItemTreeNode ptitn = new PublisherTaskItemTreeNode ( pt, treeImages.Images.IndexOfKey ( "prebuild" ) );
          ptn.Project.PreBuild.Add ( pt );
          pbTN.Nodes.Add ( ptitn );
          OnConfigurationModified ( new CancelEventArgs ( false ) );

          if ( !pbTN.IsExpanded )
            pbTN.Expand ( );
        }
      }
    }


    /// <summary>
    /// handles the click of a publisher menu item.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void publisherItem_Click ( object sender, EventArgs e ) {
      // TODO: create menuitem specific for this so the Tag property doesnt have to be used.
      Type t = ( Type ) ( ( ToolStripMenuItem ) sender ).Tag;
      PublisherTask pt = ( PublisherTask ) Util.CreateInstanceOfType ( t );
      if ( projectContextMenu != null && projectContextMenu.Tag != null && projectContextMenu.Tag.GetType ( ) == typeof ( ProjectTreeNode ) ) {
        ProjectTreeNode ptn = ( ProjectTreeNode ) projectContextMenu.Tag;
        PublishersTreeNode pubTN = ( PublishersTreeNode ) Util.FindNodeByType ( ptn, typeof ( PublishersTreeNode ) );
        if ( pubTN != null ) {
          PublisherTaskItemTreeNode ptitn = new PublisherTaskItemTreeNode ( pt, treeImages.Images.IndexOfKey ( "publishers" ) );
          ptn.Project.Publishers.Add ( pt );
          pubTN.Nodes.Add ( ptitn );
          OnConfigurationModified ( new CancelEventArgs ( false ) );

          if ( !pubTN.IsExpanded )
            pubTN.Expand ( );
        }
      }
    }

    /// <summary>
    /// Handles the Click event of the removeTaskToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void removeTaskToolStripMenuItem_Click ( object sender, EventArgs e ) {
      if ( sender.GetType ( ) == typeof ( TaskToolStripMenuItem ) ) {
        TaskToolStripMenuItem ptsmi = ( TaskToolStripMenuItem ) sender;
        if ( Util.ConfirmDelete ( this, ptsmi.PublisherTaskItemTreeNode.PublisherTask.GetType ( ) ) == DialogResult.Yes ) {
          ptsmi.ProjectTreeNode.Project.Tasks.Remove ( ptsmi.PublisherTaskItemTreeNode.PublisherTask );
          PropertyGridCheckRemovedItem ( ptsmi.PublisherTaskItemTreeNode.PublisherTask );
          ptsmi.PublisherTaskItemTreeNode.Remove ( );
          OnConfigurationModified ( new CancelEventArgs ( false ) );
        }
      } else
        throw new ArgumentException ( "Invalid menu type: Use PublisherToolStripMenuItem." );
    }

    /// <summary>
    /// Handles the Click event of the removeTriggerToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void removeTriggerToolStripMenuItem_Click ( object sender, EventArgs e ) {
      if ( sender.GetType ( ) == typeof ( TriggerToolStripMenuItem ) ) {
        TriggerToolStripMenuItem ttsmi = ( TriggerToolStripMenuItem ) sender;
        if ( Util.ConfirmDelete ( this, ttsmi.TriggerTreeNode.Trigger.GetType ( ) ) == DialogResult.Yes ) {
          ttsmi.ProjectTreeNode.Project.Triggers.Remove ( ttsmi.TriggerTreeNode.Trigger );
          PropertyGridCheckRemovedItem ( ttsmi.TriggerTreeNode.Trigger );
          ttsmi.TriggerTreeNode.Remove ( );
          OnConfigurationModified ( new CancelEventArgs ( false ) );
        }
      } else
        throw new ArgumentException ( "Invalid menu type: Use TriggerToolStripMenuItem." );
    }

    /// <summary>
    /// Handles the Click event of the removePublisherToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void removePublisherToolStripMenuItem_Click ( object sender, EventArgs e ) {
      if ( sender.GetType ( ) == typeof ( PublisherToolStripMenuItem ) ) {
        PublisherToolStripMenuItem ptsmi = ( PublisherToolStripMenuItem ) sender;
        if ( Util.ConfirmDelete ( this, ptsmi.PublisherTaskItemTreeNode.PublisherTask.GetType ( ) ) == DialogResult.Yes ) {
          if ( ptsmi.PublisherTaskItemTreeNode.Parent.GetType () == typeof ( PrebuildTreeNode ) )
            ptsmi.ProjectTreeNode.Project.PreBuild.Remove ( ptsmi.PublisherTaskItemTreeNode.PublisherTask );
          else
            ptsmi.ProjectTreeNode.Project.Publishers.Remove ( ptsmi.PublisherTaskItemTreeNode.PublisherTask );

          PropertyGridCheckRemovedItem ( ptsmi.PublisherTaskItemTreeNode.PublisherTask );
          ptsmi.PublisherTaskItemTreeNode.Remove ( );
          OnConfigurationModified ( new CancelEventArgs ( false ) );

        }
      } else
        throw new ArgumentException ( "Invalid menu type: Use PublisherToolStripMenuItem." );
    }

    /// <summary>
    /// Handles the Click event of the documentationBrowserToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void documentationBrowserToolStripMenuItem_Click ( object sender, EventArgs e ) {
      this.webSplitter.ToggleState ( );
      this.documentationBrowserToolStripMenuItem.Checked = !this.webSplitter.IsCollapsed;
    }

    /// <summary>
    /// Handles the Click event of the configurationTreeToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void configurationTreeToolStripMenuItem_Click ( object sender, EventArgs e ) {
      treeSplitter.ToggleState ( );
      this.configurationTreeToolStripMenuItem.Checked = !this.treeSplitter.IsCollapsed;
    }

    /// <summary>
    /// Handles the LocationChanged event of the treeSplitter control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void treeSplitter_LocationChanged ( object sender, EventArgs e ) {
      this.configurationTreeToolStripMenuItem.Checked = !treeSplitter.IsCollapsed;
    }

    /// <summary>
    /// Handles the LocationChanged event of the webSplitter control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void webSplitter_LocationChanged ( object sender, EventArgs e ) {
      this.documentationBrowserToolStripMenuItem.Checked = !webSplitter.IsCollapsed;
    }

    /// <summary>
    /// Handles the Click event of the reportABugToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void reportABugToolStripMenuItem_Click ( object sender, EventArgs e ) {
      Program.BugTracker.SubmitBugDialog ( this );
    }
    #endregion


    /// <summary>
    /// Raises the <see cref="E:ExtensionTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    private void OnExtensionTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      if ( e.Button == MouseButtons.Left ) {
        this.removeProjectToolButton.Enabled = false;
        this.addProjectItems.Enabled = true;
        SetToolbarMenuItemsVisible ( null );
        ProjectTreeNode ptn = e.Node.Parent as ProjectTreeNode;
        pgProperties.SelectedObject = null;
      } else if ( e.Button == MouseButtons.Right ) {
        // hide context menu items...
        SetContextMenuItemsVisible ( null );
        tvProjects.ContextMenuStrip = null;
      }
      projectContextMenu.Tag = e.Node.Parent;
    }

    /// <summary>
    /// Raises the <see cref="E:ProjectTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnProjectTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      ProjectTreeNode ptn = e.Node as ProjectTreeNode;
      if ( ptn != null ) {
        if ( e.Button == MouseButtons.Left ) {
          pgProperties.SelectedObject = rootNode.CruiseControl.Projects[ ptn.Project.Name ];
          DocumentationNavigate ( ptn.Project.DocumentationUri );
          this.removeProjectToolButton.Enabled = true;
          this.addProjectItems.Enabled = true;
          this.copyProjectToolStripButton.Enabled = true;
          SetToolbarMenuItemsVisible ( null );
        } else if ( e.Button == MouseButtons.Right ) {
          SetContextMenuItemsVisible ( null );
          copyProjectMenuItem.Text = string.Format ( Properties.Strings.CopyProjectMenuItem, ptn.Project.Name );
          copyProjectAsMenuItem.Text = string.Format ( Properties.Strings.CopyProjectAsMenuItem, ptn.Project.Name );
          tvProjects.ContextMenuStrip = projectContextMenu;

          copyItemsToolStripMenuItem.Visible = true;
          if ( ptn != null ) {
            copyItemsToolStripMenuItem.Text = "Copy Project to clipboard";
            if ( _copyMenuItemEventHandler != null )
              copyItemsToolStripMenuItem.Click -= _copyMenuItemEventHandler;
            _copyMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
              ClipboardManager.CopyToClipboard<Project> ( ptn.Project );
            };
            copyItemsToolStripMenuItem.Click += _copyMenuItemEventHandler;
          }
        }
      }
      projectContextMenu.Tag = e.Node;
    }

    /// <summary>
    /// Raises the <see cref="E:PrebuildTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnPrebuildTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      ProjectTreeNode ptn = e.Node.Parent as ProjectTreeNode;
      if ( e.Button == MouseButtons.Left ) {
        this.removeProjectToolButton.Enabled = false;
        this.addProjectItems.Enabled = true;
        SetToolbarMenuItemsVisible ( prebuildToolStripMenuItem );
        pgProperties.SelectedObject = ptn.Project;
      } else if ( e.Button == MouseButtons.Right ) {
        // hide context menu items...
        SetContextMenuItemsVisible ( addPrebuildToolStripMenuItem );
        tvProjects.ContextMenuStrip = projectContextMenu;
        copyItemsToolStripMenuItem.Visible = true;
        if ( ptn != null ) {
          copyItemsToolStripMenuItem.Text = "Copy Prebuilds";
          if ( _copyMenuItemEventHandler != null )
            copyItemsToolStripMenuItem.Click -= _copyMenuItemEventHandler;
          _copyMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
            ClipboardManager.CopyToClipboard<PrebuildsList> ( ptn.Project.PreBuild );
          };
          copyItemsToolStripMenuItem.Click += _copyMenuItemEventHandler;

          cutItemToolStripMenuItem.Text = "Cut Prebuilds";
          if ( _cutMenuItemEventHandler != null )
            cutItemToolStripMenuItem.Click -= _cutMenuItemEventHandler;
          _cutMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
            ClipboardManager.CopyToClipboard<PrebuildsList> ( ptn.Project.PreBuild );
            foreach ( PublisherTaskItemTreeNode ptin in e.Node.Nodes )
              ptin.Remove ( );
            OnConfigurationModified ( new CancelEventArgs ( false ) );
          };
          cutItemToolStripMenuItem.Click += _cutMenuItemEventHandler;
        }
        CreatePublisherTaskPasteMenu ( e.Node );
      }
      projectContextMenu.Tag = e.Node.Parent;
    }

    /// <summary>
    /// Raises the <see cref="E:CruiseControlTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnCruiseControlTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      if ( e.Button == MouseButtons.Left ) {
        this.pgProperties.SelectedObject = null;
        this.addProjectItems.Enabled = false;
        this.removeProjectToolButton.Enabled = false;
        SetToolbarMenuItemsVisible ( null );
        this.configPropertiesToolStripButton.Enabled = true;
      } else if ( e.Button == MouseButtons.Right ) {
        tvProjects.ContextMenuStrip = projectContextMenu;
        SetContextMenuItemsVisible ( null );
        copyItemsToolStripMenuItem.Visible = true;
        copyItemsToolStripMenuItem.Text = "Copy Configuration";
        //if ( _copyMenuItemEventHandler != null )
        //_copyMenuItemEventHandler = _copyMenuItemEventHandler;
        _copyMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
          ClipboardManager.CopyToClipboard<CCNetConfig.Core.CruiseControl> ( this.CruiseControl );
        };
        copyItemsToolStripMenuItem.Click += _copyMenuItemEventHandler;
      }
      projectContextMenu.Tag = null;
    }

    /// <summary>
    /// Raises the <see cref="E:TasksTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnTasksTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      ProjectTreeNode ptn = e.Node.Parent as ProjectTreeNode;
      if ( e.Button == MouseButtons.Left ) {
        this.removeProjectToolButton.Enabled = false;
        this.addProjectItems.Enabled = true;
        SetToolbarMenuItemsVisible ( tasksToolStripMenuItem );
        pgProperties.SelectedObject = ptn.Project;
      } else if ( e.Button == MouseButtons.Right ) {
        // hide context menu items...
        SetContextMenuItemsVisible ( addTaskToolStripMenuItem );
        tvProjects.ContextMenuStrip = projectContextMenu;
        copyItemsToolStripMenuItem.Visible = true;
        if ( ptn != null ) {
          copyItemsToolStripMenuItem.Text = "Copy Tasks";
          if ( _copyMenuItemEventHandler != null )
            copyItemsToolStripMenuItem.Click -= _copyMenuItemEventHandler;
          _copyMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
            ClipboardManager.CopyToClipboard<TasksList> ( ptn.Project.Tasks );
          };
          copyItemsToolStripMenuItem.Click += _copyMenuItemEventHandler;
          cutItemToolStripMenuItem.Text = "Cut Tasks";
          if ( _cutMenuItemEventHandler != null )
            cutItemToolStripMenuItem.Click -= _cutMenuItemEventHandler;
          _cutMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
            ClipboardManager.CopyToClipboard<TasksList> ( ptn.Project.Tasks );
            foreach ( PublisherTaskItemTreeNode ptin in e.Node.Nodes )
              ptin.Remove ( );
            OnConfigurationModified ( new CancelEventArgs ( false ) );
          };
          cutItemToolStripMenuItem.Click += _cutMenuItemEventHandler;
        }
        CreatePublisherTaskPasteMenu ( e.Node );

      }
      projectContextMenu.Tag = e.Node.Parent;
    }

    /// <summary>
    /// Raises the <see cref="E:PublishersTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnPublishersTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      ProjectTreeNode ptn = e.Node.Parent as ProjectTreeNode;
      if ( e.Button == MouseButtons.Left ) {
        this.removeProjectToolButton.Enabled = false;
        this.addProjectItems.Enabled = true;
        SetToolbarMenuItemsVisible ( publishersToolStripMenuItem );
        pgProperties.SelectedObject = ptn.Project;
      } else if ( e.Button == MouseButtons.Right ) {
        // hide context menu items...
        SetContextMenuItemsVisible ( addPublisherToolStripMenuItem );
        tvProjects.ContextMenuStrip = projectContextMenu;
        copyItemsToolStripMenuItem.Visible = true;
        if ( ptn != null ) {
          copyItemsToolStripMenuItem.Text = "Copy Publishers";
          if ( _copyMenuItemEventHandler != null )
            copyItemsToolStripMenuItem.Click -= _copyMenuItemEventHandler;
          _copyMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
            ClipboardManager.CopyToClipboard<PublishersList> ( ptn.Project.Publishers );
          };
          copyItemsToolStripMenuItem.Click += _copyMenuItemEventHandler;

          cutItemToolStripMenuItem.Text = "Cut Publishers";
          if ( _cutMenuItemEventHandler != null )
            cutItemToolStripMenuItem.Click -= _cutMenuItemEventHandler;
          _cutMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
            ClipboardManager.CopyToClipboard<PublishersList> ( ptn.Project.Publishers );
            foreach ( PublisherTaskItemTreeNode ptin in e.Node.Nodes )
              ptin.Remove ( );
            OnConfigurationModified ( new CancelEventArgs ( false ) );
          };
          cutItemToolStripMenuItem.Click += _cutMenuItemEventHandler;
          CreatePublisherTaskPasteMenu ( e.Node );
        }
      }
      projectContextMenu.Tag = e.Node.Parent;
    }

    /// <summary>
    /// Creates the publisher task paste menu.
    /// </summary>
    /// <param name="owner">The owner.</param>
    private void CreatePublisherTaskPasteMenu ( TreeNode owner ) {
      // enable the paste menu item
      pasteNewItemToolStripMenuItem.Enabled = true;
      if ( _pasteMenuItemEventHandler != null )
        pasteNewItemToolStripMenuItem.Click -= _pasteMenuItemEventHandler;
      _pasteMenuItemEventHandler = null;
      XmlElement objElement = null;
      // check if the clipboard has an xml element
      if ( ClipboardManager.ContainsXmlElement ( out objElement ) ) {
        if ( objElement != null ) {
          // if a publishers node
          if ( string.Compare ( objElement.Name, owner.Text.ToLower ( ), false ) == 0 ) {
            // set text
            pasteNewItemToolStripMenuItem.Text = string.Format ( "Paste New {0}", owner.Text );
            _pasteMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
              // create list
              PublishersTasksList ptl = new PublishersTasksList ( owner.Text.ToLower ( ) );
              // deserialize object
              ptl.Deserialize ( objElement );
              // add new items
              foreach ( PublisherTask pto in ptl )
                PastePublisherTaskObject ( owner, pto );
            };
            pasteNewItemToolStripMenuItem.Click += _pasteMenuItemEventHandler;
          } else {
            // attempt to convert to a publisher
            PublisherTask pto = Core.Util.GetPublisherTaskFromElement ( objElement );
            // if converted
            if ( pto != null ) {
              pasteNewItemToolStripMenuItem.Text = string.Format ( "Paste New {0}", pto.GetType ( ).Name );
              _pasteMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
                PastePublisherTaskObject ( owner, pto );
              };
              pasteNewItemToolStripMenuItem.Click += _pasteMenuItemEventHandler;
            } else
              pasteNewItemToolStripMenuItem.Enabled = false;
          }
        }
      } else {
        pasteNewItemToolStripMenuItem.Enabled = false;
      }
    }

    /// <summary>
    /// Creates the trigger paste menu.
    /// </summary>
    /// <param name="owner">The owner.</param>
    public void CreateTriggerPasteMenu ( TreeNode owner ) {
      // enable the paste menu item
      if ( _pasteMenuItemEventHandler != null )
        pasteNewItemToolStripMenuItem.Click -= _pasteMenuItemEventHandler;
      _pasteMenuItemEventHandler = null;
      pasteNewItemToolStripMenuItem.Enabled = true;
      XmlElement objElement = null;
      // check if the clipboard has an xml element
      if ( ClipboardManager.ContainsXmlElement ( out objElement ) ) {
        if ( objElement != null ) {
          // if a triggers node
          if ( string.Compare ( objElement.Name, owner.Text.ToLower ( ), false ) == 0 ) {
            // set text
            pasteNewItemToolStripMenuItem.Text = string.Format ( "Paste New {0}", owner.Text );
            _pasteMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
              // create list
              TriggersList tl = new TriggersList ( );
              // deserialize object
              tl.Deserialize ( objElement );
              // add new items
              foreach ( Trigger to in tl )
                PasteTriggerObject ( owner, to );
            };
            pasteNewItemToolStripMenuItem.Click += _pasteMenuItemEventHandler;
          } else {
            // attempt to convert to a publisher
            Trigger to = Core.Util.GetTriggerFromElement ( objElement );
            // if converted
            if ( to != null ) {
              pasteNewItemToolStripMenuItem.Text = string.Format ( "Paste New {0}", to.GetType ( ).Name );
              _pasteMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
                PasteTriggerObject ( owner, to );
              };
              pasteNewItemToolStripMenuItem.Click += _pasteMenuItemEventHandler;
            } else
              pasteNewItemToolStripMenuItem.Enabled = false;
          }
        }
      } else {
        pasteNewItemToolStripMenuItem.Enabled = false;
      }
    }

    /// <summary>
    /// Pastes the publisher task object.
    /// </summary>
    /// <param name="owner">The owner node.</param>
    /// <param name="pt">The pt.</param>
    private void PastePublisherTaskObject ( TreeNode owner, PublisherTask pt ) {
      ProjectTreeNode ptn = owner.Parent as ProjectTreeNode;
      if ( ptn != null ) {
        if ( owner.GetType ( ) == typeof ( PublishersTreeNode ) )
          ptn.Project.Publishers.Add ( pt );
        else if ( owner.GetType ( ) == typeof ( PrebuildTreeNode ) )
          ptn.Project.PreBuild.Add ( pt );
        else if ( owner.GetType ( ) == typeof ( TasksTreeNode ) )
          ptn.Project.Tasks.Add ( pt );
        else
          return;
        PublisherTaskItemTreeNode ptitn = new PublisherTaskItemTreeNode ( pt, owner.ImageIndex );
        owner.Nodes.Add ( ptitn );
        OnConfigurationModified ( new CancelEventArgs ( false ) );
      }
    }

    /// <summary>
    /// Pastes the trigger object.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="trigger">The trigger.</param>
    private void PasteTriggerObject ( TreeNode owner, Trigger trigger ) {
      ProjectTreeNode ptn = owner.Parent as ProjectTreeNode;
      if ( ptn != null ) {
        ptn.Project.Triggers.Add ( trigger );
        TriggerItemTreeNode titn = new TriggerItemTreeNode ( trigger, owner.ImageIndex );
        owner.Nodes.Add ( titn );
        OnConfigurationModified ( new CancelEventArgs ( false ) );
      }
    }

    /// <summary>
    /// Raises the <see cref="E:TriggersTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnTriggersTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      ProjectTreeNode ptn = e.Node.Parent as ProjectTreeNode;
      if ( e.Button == MouseButtons.Left ) {
        this.removeProjectToolButton.Enabled = false;
        this.addProjectItems.Enabled = true;
        SetToolbarMenuItemsVisible ( triggersToolStripMenuItem );
        pgProperties.SelectedObject = ptn.Project;
      } else if ( e.Button == MouseButtons.Right ) {
        // hide context menu items...
        SetContextMenuItemsVisible ( addTriggerToolStripMenuItem );
        tvProjects.ContextMenuStrip = projectContextMenu;
        copyItemsToolStripMenuItem.Visible = true;
        if ( ptn != null ) {
          copyItemsToolStripMenuItem.Text = "Copy Triggers";
          if ( _copyMenuItemEventHandler != null )
            copyItemsToolStripMenuItem.Click -= _copyMenuItemEventHandler;
          _copyMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
            ClipboardManager.CopyToClipboard<TriggersList> ( ptn.Project.Triggers );
          };
          copyItemsToolStripMenuItem.Click += _copyMenuItemEventHandler;

          cutItemToolStripMenuItem.Text = "Cut Triggers";
          if ( _cutMenuItemEventHandler != null )
            cutItemToolStripMenuItem.Click -= _cutMenuItemEventHandler;
          _cutMenuItemEventHandler = delegate ( object sender, EventArgs evt ) {
            ClipboardManager.CopyToClipboard<TriggersList> ( ptn.Project.Triggers );
            foreach ( TriggerItemTreeNode titn in e.Node.Nodes )
              titn.Remove ( );
            OnConfigurationModified ( new CancelEventArgs ( false ) );
          };
          cutItemToolStripMenuItem.Click += _cutMenuItemEventHandler;
          CreateTriggerPasteMenu ( e.Node );
        }

      }
      projectContextMenu.Tag = e.Node.Parent;
    }

    /// <summary>
    /// Raises the <see cref="E:TriggerItemTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnTriggerItemTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      TriggerItemTreeNode titn = e.Node as TriggerItemTreeNode;
      ProjectTreeNode ptn = e.Node.Parent.Parent as ProjectTreeNode;

      if ( e.Button == MouseButtons.Left ) {
        pgProperties.SelectedObject = titn.Trigger;
        if ( titn.Trigger.GetType ( ).GetInterface ( typeof ( ICCNetDocumentation ).FullName ) != null )
          this.DocumentationNavigate ( ( ( ICCNetDocumentation ) titn.Trigger ).DocumentationUri );
      } else if ( e.Button == MouseButtons.Right ) {
        // hide context menu items...
        CreateRemoveTriggerMenuItem ( ptn, titn );
        tvProjects.ContextMenuStrip = genericContextMenu;
      }
    }

    /// <summary>
    /// Raises the <see cref="E:PublisherItemTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnPublisherItemTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      PublisherTaskItemTreeNode ptitn = e.Node as PublisherTaskItemTreeNode;
      ProjectTreeNode ptn = e.Node.Parent.Parent as ProjectTreeNode;
      if ( e.Button == MouseButtons.Left ) {
        pgProperties.SelectedObject = ptitn.PublisherTask;
        if ( ptitn.PublisherTask.GetType ( ).GetInterface ( typeof ( ICCNetDocumentation ).FullName ) != null )
          this.DocumentationNavigate ( ( ( ICCNetDocumentation ) ptitn.PublisherTask ).DocumentationUri );
      } else if ( e.Button == MouseButtons.Right ) {
        // hide context menu items...
        CreateRemovePublisherMenuItem ( ptn, ptitn );
        tvProjects.ContextMenuStrip = genericContextMenu;
      }
    }

    /// <summary>
    /// Raises the <see cref="E:TaskItemTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    void OnTaskItemTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      PublisherTaskItemTreeNode ptitn = e.Node as PublisherTaskItemTreeNode;
      ProjectTreeNode ptn = e.Node.Parent.Parent as ProjectTreeNode;
      if ( e.Button == MouseButtons.Left ) {
        pgProperties.SelectedObject = ptitn.PublisherTask;
        DisplayDocumentation(ptitn.PublisherTask);
      } else if ( e.Button == MouseButtons.Right ) {
        CreateRemoveTaskMenuItem ( ptn, ptitn );
        tvProjects.ContextMenuStrip = genericContextMenu;
      }
    }

    /// <summary>
    /// Raises the <see cref="E:ExtensionItemTreeNodeMouseClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
    private void OnExtensionItemTreeNodeMouseClick ( TreeNodeMouseClickEventArgs e ) {
      ExtensionItemTreeNode eitn = e.Node as ExtensionItemTreeNode;
      ProjectTreeNode ptn = e.Node.Parent.Parent as ProjectTreeNode;
      if ( e.Button == MouseButtons.Left ) {
          DisplayItem(eitn.ProjectExtension);
      } else if ( e.Button == MouseButtons.Right ) {
          ChangeContextMenu(null);
      }
    }

    #region ChangeContextMenu()
      /// <summary>
      /// Changes the context menu for the treeview.
      /// </summary>
      /// <param name="menu">The new menu.</param>
    public virtual void ChangeContextMenu(ContextMenuStrip menu)
    {
        tvProjects.ContextMenuStrip = menu;
    }
    #endregion

    #region DisplayItem()
    /// <summary>
    /// Displays an item.
    /// </summary>
    /// <param name="value">The item to display.</param>
    /// <remarks>
    /// This will update both the property grid and the documentation link.
    /// </remarks>
    public virtual void DisplayItem(object value)
    {
        DisplayProperties(value);
        DisplayDocumentation(value);
    }
    #endregion

    #region DisplayProperties()
    /// <summary>
      /// Displays the properties in the property grid.
      /// </summary>
      /// <param name="source">The source of the properties.</param>
    public virtual void DisplayProperties(object source)
    {
        pgProperties.SelectedObject = source;
    }
    #endregion

    #region DisplayDocumentation()
    /// <summary>
    /// Displays any associated documentation.
    /// </summary>
    /// <param name="source">The object to retrieve the documentation for.</param>
    public virtual void DisplayDocumentation(object source)
    {
        if (source is ICCNetDocumentation)
        {
            DocumentationNavigate(((ICCNetDocumentation)source).DocumentationUri);
        }
    }
    #endregion

    #region form event overrides
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Load"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnLoad ( EventArgs e ) {
     base.OnLoad ( e );
     if ( CCNetConfig.Core.Util.UserSettings.UpdateSettings.CheckOnStartup )
        this.checkForUpdatesToolStripMenuItem.PerformClick ( );
     Util.HideSplashScreen ( );
    }


    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"></see> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"></see> that contains the event data.</param>
    protected override void OnClosing ( CancelEventArgs e ) {
      if ( !e.Cancel && this.configModified ) {
        DialogResult dr = MessageBox.Show ( this, Properties.Resources.SaveConfigurationOnExit, Properties.Resources.SaveConfigurationTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
        if ( dr == DialogResult.Yes ) {
          this.saveConfigurationToolButton.PerformClick ( );
          e.Cancel = configModified;
        } else if ( dr == DialogResult.Cancel )
          e.Cancel = true;
      }
      base.OnClosing ( e );
    }
    #endregion

    /// <summary>
    /// Sets the context menu items visible.
    /// </summary>
    /// <param name="visibleItem">The visible item.</param>
    void SetContextMenuItemsVisible ( ToolStripMenuItem visibleItem ) {
      ProjectTreeNode ptn = projectContextMenu.Tag as ProjectTreeNode;
      // workitem : 15093 
      //    removed the if block checking if ptn was null
      //    This is what would disable menu items that should not be visible for the root node
      bool isCCNode = visibleItem == null && ptn == null && this.CruiseControl != null;
      // workitem : 14549
      bool isProject = !isCCNode && ptn.Parent != null && ptn.Parent.GetType ( ) == typeof ( CruiseControlTreeNode ) && visibleItem == null;

      addPublisherToolStripMenuItem.Visible = ( addPublisherToolStripMenuItem == visibleItem || visibleItem == null ) && ptn != null;
      addTriggerToolStripMenuItem.Visible = ( addTriggerToolStripMenuItem == visibleItem || visibleItem == null ) && ptn != null;
      addTaskToolStripMenuItem.Visible = ( addTaskToolStripMenuItem == visibleItem || visibleItem == null ) && ptn != null;
      addPrebuildToolStripMenuItem.Visible = ( addPrebuildToolStripMenuItem == visibleItem || visibleItem == null ) && ptn != null;

      copyProjectMenuItem.Visible = copyProjectAsMenuItem.Visible = isProject;
      copySepMenuItem.Visible = isProject;

      addProjectToolStripMenuItem.Visible = isCCNode;
      cutItemToolStripMenuItem.Visible = !isProject && !isCCNode;
      pasteNewItemToolStripMenuItem.Visible = !isProject && !isCCNode;
    }

    /// <summary>
    /// Sets the toolbar menu items visible.
    /// </summary>
    /// <param name="visibleItem">The visible item.</param>
    void SetToolbarMenuItemsVisible ( ToolStripMenuItem visibleItem ) {
      this.tasksToolStripMenuItem.Visible = tasksToolStripMenuItem == visibleItem || visibleItem == null;
      this.publishersToolStripMenuItem.Visible = publishersToolStripMenuItem == visibleItem || visibleItem == null;
      this.triggersToolStripMenuItem.Visible = triggersToolStripMenuItem == visibleItem || visibleItem == null;
      this.prebuildToolStripMenuItem.Visible = this.prebuildToolStripMenuItem == visibleItem || visibleItem == null;

      this.configPropertiesToolStripButton.Enabled = false;
    }

    /// <summary>
    /// Sets the edit menu items enabled.
    /// </summary>
    /// <param name="copy">if set to <see langword="true"/> [copy].</param>
    /// <param name="cut">if set to <see langword="true"/> [cut].</param>
    /// <param name="paste">if set to <see langword="true"/> [paste].</param>
    void SetEditMenuItemsEnabled ( bool copy, bool cut, bool paste ) {
      copyToolStripButton.Enabled = copyToolStripMenuItem.Enabled = copy;
      cutToolStripButton.Enabled = cutToolStripMenuItem.Enabled = cut;
      pasteToolStripButton.Enabled = pasteToolStripMenuItem.Enabled = paste;
    }

    /// <summary>
    /// Handles the Click event of the addProjectToolButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void addProjectToolButton_Click ( object sender, EventArgs e ) {
      NewProjectForm npf = new NewProjectForm ( );
      if ( npf.ShowDialog ( this ) == DialogResult.OK ) {
        if ( rootNode.CruiseControl.Projects.GetCountByName ( npf.ProjectName ) == 0 ) {
          Project p = new Project ( );
          p.Name = npf.ProjectName;
          AddProjectTreeNode ( p );
          OnConfigurationModified ( new CancelEventArgs ( false ) );
        } else {
          int result = TaskDialog.ShowCommandBox ( Properties.Strings.ProjectNameExistsTitle, Properties.Strings.ProjectNameExistsMessage,
            string.Empty, string.Empty, string.Empty, string.Empty, Properties.Strings.ProjectNameExistsCommandButtons, false, SysIcons.Question, SysIcons.Question );
          switch ( ( ProjectNameExistsTaskDialogCommandButton ) result ) {
            case ProjectNameExistsTaskDialogCommandButton.NewName:
              addProjectToolButton.PerformClick ( );
              break;
            case ProjectNameExistsTaskDialogCommandButton.Cancel:
              break;
          }
        }
      }
    }

    /// <summary>
    /// copies a project configuration to a new node.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void copyProjectToolStripButton_Click ( object sender, EventArgs e ) {
      copyProjectMenuItem.PerformClick ( );
    }


    /// <summary>
    /// Handles the Click event of the copyProjectMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void copyProjectMenuItem_Click ( object sender, EventArgs e ) {
      if ( this.tvProjects.SelectedNode.GetType ( ) == typeof ( ProjectTreeNode ) ) {
        ProjectTreeNode ptn = this.tvProjects.SelectedNode as ProjectTreeNode;
        Project pjct = ptn.Project.Clone ( );
        pjct.Name = Util.GetUniqueName ( this.CruiseControl, pjct );
        this.AddProjectTreeNode ( pjct );
      }
    }

    /// <summary>
    /// Handles the Click event of the copyProjectAsMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void copyProjectAsMenuItem_Click ( object sender, EventArgs e ) {
      if ( this.tvProjects.SelectedNode.GetType ( ) == typeof ( ProjectTreeNode ) ) {
        ProjectTreeNode ptn = this.tvProjects.SelectedNode as ProjectTreeNode;
        Project pjct = ptn.Project.Clone ( );
        NewProjectForm npf = new NewProjectForm ( );
        npf.Text = "Name of Project";
        if ( npf.ShowDialog ( this ) == DialogResult.OK ) {
          if ( !CruiseControl.Projects.Contains ( npf.ProjectName ) ) {
            pjct.Name = npf.ProjectName;
            this.AddProjectTreeNode ( pjct );
          }
        }
      }
    }

    /// <summary>
    /// Navigates tot he documentation Url
    /// </summary>
    /// <param name="url">The URL.</param>
    private void DocumentationNavigate ( Uri url ) {
      if ( wbDocs.Url != url )
        wbDocs.Navigate ( url );
    }

    /// <summary>
    /// Handles the Click event of the saveConfigurationToolButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void saveConfigurationToolButton_Click ( object sender, EventArgs e ) {
      Exception exception = null;
      if ( rootNode == null || rootNode.CruiseControl == null )
        return;

      if ( rootNode.CruiseControl.IsValidConfiguration ( out exception ) ) {
        if ( loadedConfigFile == null )
          loadedConfigFile = SaveAs ( );
        else // file has been saved before so lets just save it.
          Save ( loadedConfigFile );
      } else {
        if ( exception != null )
          MessageBox.Show ( this, exception.Message/* + "\n\n" + exception.StackTrace*/, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
        else
          MessageBox.Show ( this, Properties.Strings.UnableToSaveConfiguration, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
    }

    /// <summary>
    /// Saves the config file asking where to save it.
    /// </summary>
    /// <returns>The file info.</returns>
    private FileInfo SaveAs ( ) {
      VersionFileDialog vfd = new VersionFileDialog ( );
      vfd.Filter = Properties.Strings.OpenSaveFilter;
      vfd.FilterIndex = 0;
      vfd.DialogType = VersionFileDialog.VersionFileDialogType.SaveFileDialog;
      vfd.VersionList = _ccnetVersions;
      vfd.VersionIndex = rootNode != null ? _ccnetVersions.IndexOf ( rootNode.CruiseControl.Version ) : _ccnetVersions.Count - 1;
      vfd.FileName = loadedConfigFile.FullName;

      if ( loadedConfigFile != null )
        vfd.InitialDirectory = loadedConfigFile.Directory.FullName;

      try {
        if ( vfd.ShowDialog ( this ) == DialogResult.OK && vfd.SelectedVersion != null ) {
          Core.Util.RegisterTypeDescriptionProviders ( vfd.SelectedVersion );
          configVersionStatusLabel.Text = string.Format ( Properties.Strings.ConfigVersionStatusLabelFormat, vfd.SelectedVersion );
          // adjust the version
          rootNode.CruiseControl.Version = vfd.SelectedVersion;
          this.addProjectToolButton.Enabled = true;
          FileInfo file = new FileInfo ( vfd.FileName );
          Save ( file );
          return file;
        } else
          return loadedConfigFile;
      } catch ( VersionNotSelectedException ) {
        MessageBox.Show ( this, Properties.Strings.VersionNotSelectedMessage, Properties.Strings.VersionNotSelected, MessageBoxButtons.OK, MessageBoxIcon.Stop );
        return loadedConfigFile;
      }
    }

    /// <summary>
    /// Saves the config file
    /// </summary>
    /// <param name="file">The file info</param>
    private void Save ( FileInfo file ) {
      // put the back up routine in the try so if it fails for some reason, the bug can be reported.
      try {
        this._backupControler.BackupFile ( file );
        loadedConfigFile = file;
        if ( file.IsReadOnly && file.Exists ) {
          TaskDialog.ShowTaskDialogBox ( 
						Properties.Strings.ReadonlyFileTitle, 
						Properties.Strings.ReadonlyFileTitle,
            Properties.Strings.ReadonlyFileMessage, string.Empty, string.Empty, string.Empty, string.Empty,
            Properties.Strings.ReadonlyFileCommandButtons, TaskDialogButtons.None, SysIcons.Warning, SysIcons.Information );
          switch ( ( ReadOnlyTaskDialogCommandButton ) TaskDialog.CommandButtonResult ) {
            case ReadOnlyTaskDialogCommandButton.SaveAs:
              loadedConfigFile = SaveAs ( );
              return;
            case ReadOnlyTaskDialogCommandButton.RemoveReadOnly:
              file.Attributes -= FileAttributes.ReadOnly;
              break;
            case ReadOnlyTaskDialogCommandButton.Cancel:
              return;
          }

        }
        // changing FileMode to Create will solve any issues with "stray" characters from config if it was longer then new one.
        FileStream fs = new FileStream ( file.FullName, FileMode.Create, FileAccess.Write );
        rootNode.CruiseControl.SaveConfig ( fs );
        OnConfigurationModified ( new CancelEventArgs ( true ) );
			} catch ( System.IO.IOException ioex ) {
				TaskDialog.ShowTaskDialogBox ( Properties.Strings.ErrorSavingTitle,
					Properties.Strings.ErrorSavingTitle,
					string.Format(Properties.Strings.ErrorSavingMessage,ioex.Message), 
					string.Empty, 
					string.Empty, 
					string.Empty, 
					string.Empty,
					Properties.Strings.ErrorSavingCommandButtons, 
					TaskDialogButtons.None, 
					SysIcons.Error, 
					SysIcons.Information );
				switch ( (CommonTaskDialogCommandButton)TaskDialog.CommandButtonResult ) {
					case CommonTaskDialogCommandButton.Abort:
						return;
					case CommonTaskDialogCommandButton.Retry:
						this.Save ( file );
						break;
					case CommonTaskDialogCommandButton.ReportAsBug:
						Program.BugTracker.SubmitExceptionDialog ( this, ioex, file );
						return;
				}
      } catch ( Exception ex ) {
        Program.BugTracker.SubmitExceptionDialog ( this, ex, file );
      }
    }



    /// <summary>
    /// Handles the Click event of the removeProjectToolButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void removeProjectToolButton_Click ( object sender, EventArgs e ) {
      if ( this.tvProjects.SelectedNode.GetType ( ) == typeof ( ProjectTreeNode ) ) {
        ProjectTreeNode ptn = ( ProjectTreeNode ) tvProjects.SelectedNode;
        if ( MessageBox.Show ( this, string.Format ( Properties.Resources.ConfirmDelete, ptn.Project.Name ), Properties.Resources.ConfirmDeleteTitle,
          MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question ) == DialogResult.Yes ) {
          rootNode.CruiseControl.Projects.Remove ( ptn.Project.Name );
          rootNode.Nodes.Remove ( ptn );
          OnConfigurationModified ( new CancelEventArgs ( false ) );

        }
      }
    }

    /// <summary>
    /// Handles the Click event of the configPropertiesToolStripButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void configPropertiesToolStripButton_Click ( object sender, EventArgs e ) {
      if ( this.rootNode != null && this.loadedConfigFile != null ) {
        ConfigurationPropertiesForm cpf = new ConfigurationPropertiesForm ( this.rootNode.CruiseControl, this.loadedConfigFile, this._ccnetVersions );
        if ( cpf.ShowDialog ( this ) == DialogResult.OK ) {
          // if the config is modified we should save first...
          if ( this.configModified ) {
            DialogResult dr = MessageBox.Show ( this, Properties.Resources.SaveConfiguration, Properties.Resources.SaveConfigurationTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
            if ( dr == DialogResult.Yes )
              this.saveConfigurationToolButton.PerformClick ( );
          }

          this.rootNode.CruiseControl.Version = cpf.SelectedVersion;
          //configVersionStatusLabel.Text = string.Format ( Properties.Strings.ConfigVersionStatusLabelFormat, cpf.SelectedVersion );
          this.saveConfigurationToolButton.PerformClick ( );
          // need to set this because this will be wiped when ClearConfiguration() is called.
          FileInfo tFile = new FileInfo ( this.loadedConfigFile.FullName );
          ClearConfiguration ( );
          CreateConfigurationNode ( cpf.SelectedVersion );
          LoadConfigurationFile ( tFile );
        }
      }
    }

    /// <summary>
    /// Handles the Click event of the helpToolStripButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void helpToolStripButton_Click ( object sender, EventArgs e ) {
      ICCNetDocumentation idoc = null;
      if ( this.pgProperties.SelectedGridItem.Value != null )
        idoc = ( ICCNetDocumentation ) this.pgProperties.SelectedGridItem.Value;
      else
        idoc = ( ICCNetDocumentation ) Util.CreateInstanceOfType ( this.pgProperties.SelectedGridItem.PropertyDescriptor.PropertyType );
      DocumentationNavigate ( idoc.DocumentationUri );
    }

    /// <summary>
    /// Handles the Click event of the submitBugToolStripButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void submitBugToolStripButton_Click ( object sender, EventArgs e ) {
      this.reportABugToolStripMenuItem.PerformClick ( );
    }

    /// <summary>
    /// Handles the Click event of the newConfigurationToolButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void newConfigurationToolButton_Click ( object sender, EventArgs e ) {
      if ( configModified ) {
        DialogResult dr = MessageBox.Show ( this, Properties.Resources.SaveConfiguration, Properties.Resources.SaveConfigurationTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
        if ( dr == DialogResult.Yes )
          this.saveConfigurationToolButton.PerformClick ( );
        else if ( dr == DialogResult.Cancel )
          return;
      }

      NewConfigurationFile ( );
    }

    /// <summary>
    /// Handles the Click event of the openConfigurationToolButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void openConfigurationToolButton_Click ( object sender, EventArgs e ) {
      if ( configModified ) {
        DialogResult dr = MessageBox.Show ( this, Properties.Resources.SaveConfiguration, Properties.Resources.SaveConfigurationTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
        if ( dr == DialogResult.Yes )
          this.saveConfigurationToolButton.PerformClick ( );
        else if ( dr == DialogResult.Cancel )
          return;
      }
      OpenConfigurationFile ( );
    }

    /// <summary>
    /// Clears the configuration.
    /// </summary>
    private void ClearConfiguration ( ) {
      loadedConfigFile = null;
      OnConfigurationModified ( new CancelEventArgs ( true ) );
      pgProperties.SelectedObject = null;
      tvProjects.Nodes.Clear ( );
      this.addProjectToolButton.Enabled = false;
    }

    /// <summary>
    /// Creates the new configuration.
    /// </summary>
    private void OpenConfigurationFile ( ) {
      VersionFileDialog vfd = new VersionFileDialog ( );
      vfd.Filter = Properties.Strings.OpenSaveFilter;
      vfd.FilterIndex = 0;
      vfd.DialogType = VersionFileDialog.VersionFileDialogType.OpenFileDialog;
      vfd.VersionList = _ccnetVersions;
      vfd.VersionIndex = _ccnetVersions.Count - 1;
      vfd.SelectionChanged += delegate ( object sender, string path, out int newVersionIndex ) {
        try {
          Version tVersion = Core.Util.GetConfigFileVersion ( new FileInfo ( path ) );
          newVersionIndex = _ccnetVersions.IndexOf ( tVersion );
        } catch { newVersionIndex = -1; }
      };

      if ( loadedConfigFile != null )
        vfd.InitialDirectory = loadedConfigFile.Directory.FullName;
      try {
        if ( vfd.ShowDialog ( this ) == DialogResult.OK && vfd.SelectedVersion != null ) {
          ClearConfiguration ( );
          Core.Util.RegisterTypeDescriptionProviders ( vfd.SelectedVersion );
          configVersionStatusLabel.Text = string.Format ( Properties.Strings.ConfigVersionStatusLabelFormat, vfd.SelectedVersion );
          CreateConfigurationNode ( vfd.SelectedVersion );
          this.addProjectToolButton.Enabled = true;
          FileInfo file = new FileInfo ( vfd.FileName );
          if ( file.Exists ) {
            LoadConfigurationFile ( file );
          } else
            MessageBox.Show ( this, string.Format ( Properties.Strings.ConfigFileNotFoundMessage, file.Directory.FullName ), Properties.Strings.FileNotFound,
              MessageBoxButtons.OK, MessageBoxIcon.Error );
        }
      } catch ( VersionNotSelectedException ) {
        MessageBox.Show ( this, Properties.Strings.VersionNotSelectedMessage, Properties.Strings.VersionNotSelected, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      }
    }

    /// <summary>
    /// creates a  new configuration file.
    /// </summary>
    private void NewConfigurationFile ( ) {
      VersionFileDialog vfd = new VersionFileDialog ( );
      vfd.Filter = Properties.Strings.OpenSaveFilter;
      vfd.FilterIndex = 0;
      vfd.DialogType = VersionFileDialog.VersionFileDialogType.SaveFileDialog;
      vfd.VersionList = _ccnetVersions;

      if ( loadedConfigFile != null )
        vfd.InitialDirectory = loadedConfigFile.Directory.FullName;
      try {
        if ( vfd.ShowDialog ( this ) == DialogResult.OK && vfd.SelectedVersion != null ) {
          ClearConfiguration ( );
          Core.Util.RegisterTypeDescriptionProviders ( vfd.SelectedVersion );
          // create the root node.
          CreateConfigurationNode ( vfd.SelectedVersion );
          this.addProjectToolButton.Enabled = true;
          FileInfo file = new FileInfo ( vfd.FileName );
          loadedConfigFile = file;
        }
      } catch ( VersionNotSelectedException ) {
        MessageBox.Show ( this, Properties.Strings.VersionNotSelectedMessage, Properties.Strings.VersionNotSelected, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      }
    }

    private void CreateConfigurationNode(Version version)
    {
        Core.Util.CurrentConfigurationVersion = version;

        // create the menus
        PopulatePublishers();
        PopulateTasks();
        PopulateTriggers();
        PopulatePrebuild();

        rootNode = new CruiseControlTreeNode(version);
        // add to the tree
        tvProjects.Nodes.Add(rootNode);
        configVersionStatusLabel.Text = string.Format(Properties.Strings.ConfigVersionStatusLabelFormat, version);

        if (version.CompareTo(new Version("1.3")) >= 0)
        {
            queuesNode = new ProjectQueuesTreeNode(rootNode.CruiseControl, queueImageKeys);
            tvProjects.Nodes.Add(queuesNode);
        }
        else
        {
            queuesNode = null;
        }

        if (version.CompareTo(new Version("1.5")) >= 0)
        {
            securityNode = new ServerSecurityNode(rootNode.CruiseControl, securityImageKeys);
            tvProjects.Nodes.Add(securityNode);
        }
        else
        {
            securityNode = null;
        }
    }

    /// <summary>
    /// Raises the <see cref="E:ConfigurationModified"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
    private void OnConfigurationModified ( CancelEventArgs e ) {
      this.configModified = !e.Cancel;
      this.Text = string.IsNullOrEmpty ( Application.ProductName ) ? string.Format ( "CCNetConfig {0}", configModified ? "*" : string.Empty ) :
        string.Format ( "{0} {1}", Application.ProductName, ( configModified ? "*" : string.Empty ) );
    }

    /// <summary>
    /// Loads the configuration file.
    /// </summary>
    /// <param name="file">The file.</param>
    private void LoadConfigurationFile ( FileInfo file ) {
      if ( file.Exists ) {
        try {
          loadedConfigFile = file;
          this.rootNode.CruiseControl.Deserialize ( file );
          if (queuesNode != null) queuesNode.Configuration = rootNode.CruiseControl;
          // now we need to setup the tree
          rootNode.Nodes.Clear();
          AddAllProjectsToTree();
          AddAllQueuesToTree();
          if (securityNode != null)
          {
              securityNode.ChangeSecurity(rootNode.CruiseControl.Security);
          }
				} catch ( XmlException xe ) {
					// look specifically for xml errors. 
					// give the user the option to manually edit the config file
					// or report as an error.
					TaskDialog.ShowTaskDialogBox ( 
						Properties.Strings.ConfigXmlErrorsTitle,
						string.Format(Properties.Strings.ConfigXmlErrorsMessage,xe.Message),
						string.Empty,
						string.Empty,
						string.Empty,
						string.Empty,
						string.Empty,
						Properties.Strings.ConfigXmlErrorsCommandButtons,
						TaskDialogButtons.Cancel,
						SysIcons.Error,
						SysIcons.Error );
					switch ( (ConfigXmlErrorsTaskDialogCommandButton)TaskDialog.CommandButtonResult ) {
						case ConfigXmlErrorsTaskDialogCommandButton.ManuallyEdit:
							// manually edit. Once edit complete (process ends) attempt to reload the file
							Process proc = new Process ();
							ProcessStartInfo psi = new ProcessStartInfo ( @"notepad.exe", file.FullName );
							proc.StartInfo = psi;
							proc.Start ();
							proc.WaitForExit ();
							LoadConfigurationFile ( file );
							break;
						case ConfigXmlErrorsTaskDialogCommandButton.ReportAsBug:
							// user states that there is no xml error, its a bug in ccnetconfig
							Program.BugTracker.SubmitExceptionDialog ( this, xe, loadedConfigFile );
							loadedConfigFile = null;
							break;
					}
        } catch ( RequiredAttributeException ) {
          // there was an error loading the config. 
          // a required attribute was missing. lets see how the user wants to handle this...
          TaskDialog.ShowTaskDialogBox ( Properties.Strings.ErrorLoadingConfigFileTitle, Properties.Strings.ErrorLoadingConfigFileMessage,
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Properties.Strings.ErrorLoadingConfigFileCommandButtons,
            TaskDialogButtons.Cancel, SysIcons.Error, SysIcons.Error );
          switch ( ( ErrorLoadingConfigTaskDialogCommandButton ) TaskDialog.CommandButtonResult ) {
            case ErrorLoadingConfigTaskDialogCommandButton.ManuallyEdit:
							// manually edit. Once edit complete (process ends) attempt to reload the file
							Process proc = new Process ();
							ProcessStartInfo psi = new ProcessStartInfo ( @"notepad.exe", file.FullName );
							proc.StartInfo = psi;
							proc.Start ();
							proc.WaitForExit ();
							LoadConfigurationFile ( file );
							break;
					  // TODO: view/ignore errors
            case ErrorLoadingConfigTaskDialogCommandButton.ViewErrors:
            case ErrorLoadingConfigTaskDialogCommandButton.IgnoreErrors:
              MessageBox.Show ( "Not Yet Implemented." );
              break;
            default:
              break;
          }
        } catch ( Exception ex ) {
          Program.BugTracker.SubmitExceptionDialog ( this, ex, loadedConfigFile );
          loadedConfigFile = null;
        }
      }
    }

    /// <summary>
    /// Adds all projects to tree.
    /// </summary>
    private void AddAllProjectsToTree ( ) {
      foreach ( Project proj in rootNode.CruiseControl.Projects )
        AddProjectTreeNode ( proj );
    }

      #region AddAllQueuesToTree()
      /// <summary>
      /// Adds all projects to tree.
      /// </summary>
      private void AddAllQueuesToTree()
      {
          foreach (Queue queue in rootNode.CruiseControl.Queues)
          {
              AddQueueTreeNode(queue);
          }
      }
      #endregion

      #region AddQueueTreeNode()
      /// <summary>
      /// Adds a queue node to the tree.
      /// </summary>
      /// <param name="value"></param>
      private void AddQueueTreeNode(Queue value)
      {
          if (queuesNode != null)
          {
              IntegrationQueueTreeNode newNode = new IntegrationQueueTreeNode(value, queueImageKeys);
              queuesNode.Nodes.Add(newNode);
          }
      }
      #endregion

    /// <summary>
    /// Adds the project tree node.
    /// </summary>
    /// <param name="project">The project.</param>
    private void AddProjectTreeNode ( Project project ) {
      ProjectTreeNode tn = new ProjectTreeNode ( project, treeImages.Images.IndexOfKey ( "project" ) );
      tn.Tag = project.GetHashCode ( );
      TriggersTreeNode tnTriggers = new TriggersTreeNode ( treeImages.Images.IndexOfKey ( "triggers" ) );
      foreach ( Trigger trig in project.Triggers ) {
        TriggerItemTreeNode titn = new TriggerItemTreeNode ( trig, treeImages.Images.IndexOfKey ( "triggers" ) );
        tnTriggers.Nodes.Add ( titn );
        /*if ( !tnTriggers.IsExpanded )
          tnTriggers.Expand ();*/
      }
      tn.Nodes.Add ( tnTriggers );
      TasksTreeNode tnTasks = new TasksTreeNode ( treeImages.Images.IndexOfKey ( "tasks" ) );
      foreach ( PublisherTask task in project.Tasks ) {
        PublisherTaskItemTreeNode ptitn = new PublisherTaskItemTreeNode ( task, treeImages.Images.IndexOfKey ( "tasks" ) );
        tnTasks.Nodes.Add ( ptitn );
        /*if ( !tnTasks.IsExpanded )
          tnTasks.Expand ();*/
      }
      tn.Nodes.Add ( tnTasks );

      PublishersTreeNode tnPublishers = new PublishersTreeNode ( treeImages.Images.IndexOfKey ( "publishers" ) );
      foreach ( PublisherTask pub in project.Publishers ) {
        PublisherTaskItemTreeNode ptitn = new PublisherTaskItemTreeNode ( pub, treeImages.Images.IndexOfKey ( "publishers" ) );
        tnPublishers.Nodes.Add ( ptitn );
        /*if ( !tnPublishers.IsExpanded )
          tnPublishers.Expand ();*/
      }
      tn.Nodes.Add ( tnPublishers );

      PropertyDescriptor pd = Core.Util.GetPropertyDescriptor ( typeof ( Project ), "Prebuild", true );
      if ( pd != null ) {
        Version minVer = Core.Util.GetMinimumVersion ( pd );
        if ( rootNode.CruiseControl.Version.CompareTo ( minVer ) >= 0 ) {
          PrebuildTreeNode tnPrebuild = new PrebuildTreeNode ( treeImages.Images.IndexOfKey ( "prebuild" ) );
          foreach ( PublisherTask pub in project.PreBuild ) {
            PublisherTaskItemTreeNode ptitn = new PublisherTaskItemTreeNode ( pub, treeImages.Images.IndexOfKey ( "prebuild" ) );
            tnPrebuild.Nodes.Add ( ptitn );
          }
          tn.Nodes.Add ( tnPrebuild );
        }
      }

      ExtensionTreeNode etn = new ExtensionTreeNode ( treeImages.Images.IndexOfKey ( "extension" ) );
      foreach ( ProjectExtension ipe in project.ProjectExtensions ) {
        if ( !tvProjects.ImageList.Images.ContainsKey ( ipe.TypeName ) )
          tvProjects.ImageList.Images.Add ( ipe.TypeName, ipe.ExtensionImage == null ? tvProjects.ImageList.Images[ "extension" ] : ipe.ExtensionImage );

        ExtensionItemTreeNode eitn = new ExtensionItemTreeNode ( ipe, treeImages.Images.IndexOfKey ( ipe.TypeName ) );
        etn.Nodes.Add ( eitn );
        /*if ( !etn.IsExpanded )
          etn.Expand ();*/
      }
      tn.Nodes.Add ( etn );

      /*if ( !tn.IsExpanded )
        tn.Expand ();*/
      if ( !rootNode.CruiseControl.Projects.Contains ( project.Name ) )
        rootNode.CruiseControl.Projects.Add ( project );
      if ( !rootNode.Nodes.ContainsKey ( project.Name ) )
        rootNode.Nodes.Add ( tn );
      if ( !rootNode.IsExpanded )
        rootNode.Expand ( );

      // This needs to be done after the project node has been added to the tree, otherwise the
      // images will not be detected
      ReflectionHelper.GenerateChildNodes(tn, project);
      project.PropertyChanged += (o, e) =>
      {
          // This is a bit of a hack, since security uses the new reflection attributes to generate the tree structure, but
          // the rest of the project node is "old-style"
          if (e.PropertyName == "Security")
          {
              // Remove the current dynamically generated nodes
              ReflectionHelper.RemoveDynamicNodes(tn);

              // Now we can regenerate the dynamic nodes
              ReflectionHelper.GenerateChildNodes(tn, project);
          }
      };

    }

    /// <summary>
    /// Handles the Click event of the cCNetConfigToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void cCNetConfigToolStripMenuItem_Click ( object sender, EventArgs e ) {
      System.Diagnostics.Process.Start ( "http://ccnetconfig.org/" );
    }

    /// <summary>
    /// Handles the Click event of the checkForUpdatesToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void checkForUpdatesToolStripMenuItem_Click ( object sender, EventArgs e ) {
      new UpdateChecker ( ).Check ( this );
    }



    /// <summary>
    /// Handles the Click event of the cCNetDocumentationToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void cCNetDocumentationToolStripMenuItem_Click ( object sender, EventArgs e ) {
      FileInfo helpDoc = new FileInfo ( Path.Combine ( Application.StartupPath, "CCNetConfig.chm" ) );
      if ( helpDoc.Exists )
        Process.Start ( helpDoc.FullName );
    }

    /// <summary>
    /// Handles the Click event of the aboutCCNetConfigToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void aboutCCNetConfigToolStripMenuItem_Click ( object sender, EventArgs e ) {
      AboutForm abt = new AboutForm ( );
      abt.StartPosition = FormStartPosition.CenterParent;
      abt.ShowDialog ( this );
    }

    #region File Menu Event Handlers

    /// <summary>
    /// Handles the Click event of the newToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void newToolStripMenuItem_Click ( object sender, EventArgs e ) {
      this.newConfigurationToolButton.PerformClick ( );
    }

    /// <summary>
    /// Handles the Click event of the saveToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void saveToolStripMenuItem_Click ( object sender, EventArgs e ) {
      this.saveConfigurationToolButton.PerformClick ( );
    }


    private void saveAsToolStripMenuItem_Click ( object sender, EventArgs e ) {
      loadedConfigFile = SaveAs ( );
    }

    /// <summary>
    /// Handles the Click event of the openToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void openToolStripMenuItem_Click ( object sender, EventArgs e ) {
      this.openConfigurationToolButton.PerformClick ( );
    }

    /// <summary>
    /// Handles the Click event of the exitToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void exitToolStripMenuItem_Click ( object sender, EventArgs e ) {

      this.Close ( );
    }
    #endregion

    #region Tools Menu Event Handlers
    /// <summary>
    /// Handles the Click event of the optionsToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void optionsToolStripMenuItem_Click ( object sender, EventArgs e ) {
      OptionsForm of = new OptionsForm ( loadedConfigFile );
      of.BeforeRestoreConfigurationFromBackup += delegate ( object senderX, BackupFileEventArgs be ) {
        if ( this.loadedConfigFile != null )
          this._backupControler.BackupFile ( loadedConfigFile );
      };
      of.AfterRestoreConfigurationFromBackup += delegate ( object senderX, BackupFileEventArgs be ) {
        ClearConfiguration ( );
        Version ver = CCNetConfig.Core.Util.GetConfigFileVersion ( be.RestoredFile );
        CreateConfigurationNode ( ver );
        LoadConfigurationFile ( be.RestoredFile );
      };
      // ClearConfiguration ();
      // CreateConfigurationNode ( cpf.SelectedVersion );
      // LoadConfigurationFile ( tFile );
      of.ShowDialog ( this );
      of.Dispose ( );
    }

    /// <summary>
    /// Handles the Click event of the showOptionsDialogToolStripButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void showOptionsDialogToolStripButton_Click ( object sender, EventArgs e ) {
      optionsToolStripMenuItem.PerformClick ( );
    }
    #endregion

    #region File Drop Handlers
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.DragEnter"></see> event.
    /// </summary>
    /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
    protected override void OnDragEnter ( DragEventArgs drgevent ) {
      Console.WriteLine ( drgevent.Effect );
      Console.WriteLine ( string.Join ( "\n", drgevent.Data.GetFormats ( ) ) );
      if ( drgevent.Data.GetDataPresent ( DataFormats.FileDrop, false ) )
        drgevent.Effect = DragDropEffects.All;
      else
        base.OnDragEnter ( drgevent );
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.DragDrop"></see> event.
    /// </summary>
    /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
    protected override void OnDragDrop ( DragEventArgs drgevent ) {
      string[ ] files = ( string[ ] ) drgevent.Data.GetData ( DataFormats.FileDrop, false );
      if ( files.Length >= 0 ) {
        FileInfo fi = new FileInfo ( files[ 0 ] );
        if ( fi.Exists ) {
          ClearConfiguration ( );
          Version tVersion = Core.Util.GetConfigFileVersion ( fi );
          int newVersionIndex = _ccnetVersions.IndexOf ( tVersion );
          if ( newVersionIndex == -1 ) {
            List<Version> tVersions = this._ccnetVersions.Clone ( );
            tVersions.Sort ( );
            tVersions.Reverse ( );
            tVersion = tVersions[ 0 ];
          }
          Core.Util.RegisterTypeDescriptionProviders ( tVersion );
          configVersionStatusLabel.Text = string.Format ( Properties.Strings.ConfigVersionStatusLabelFormat, tVersion );
          CreateConfigurationNode ( tVersion );
          this.addProjectToolButton.Enabled = true;
          LoadConfigurationFile ( fi );

        }
      }
    }
    #endregion

    /// <summary>
    /// Gets the cruise control.
    /// </summary>
    /// <value>The cruise control.</value>
    public CCNetConfig.Core.CruiseControl CruiseControl { get { return this.rootNode != null ? this.rootNode.CruiseControl : null; } }

      /// <summary>
      /// Handle the user navigating through the tree via the keyboard.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
    private void tvProjects_AfterSelect(object sender, TreeViewEventArgs e)
    {
        tvProjects_NodeMouseClick(sender,
            new TreeNodeMouseClickEventArgs(e.Node, MouseButtons.Left, 1, 0, 0));
    }

    private void validateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (CheckForConfiguration())
        {
            DisplayValidationResults();
            validation.Validate(tvProjects);
        }
    }

    private void validateToolStripButton_Click(object sender, EventArgs e)
    {
        validateToolStripMenuItem.PerformClick();
    }

      /// <summary>
      /// Queues a node for validation.
      /// </summary>
      /// <param name="node"></param>
    public void QueueValidation(TreeNode node)
    {
        validation.QueueNode(node);
    }

      /// <summary>
      /// Validates a single node.
      /// </summary>
    public void StartValidation()
    {
        DisplayValidationResults();
        validation.StartValidation();
    }

      /// <summary>
      /// Displays the validation results window.
      /// </summary>
    private void DisplayValidationResults()
    {
        if (!validation.Visible)
        {
            validation.Top = this.Top;
            validation.Left = this.Right;
            validation.Height = this.Height;
            validation.Show();
        }
        else
        {
            validation.BringToFront();
        }
        validation.Select();
    }

    private void securityWizardsToolstripMenuItem_Click(object sender, EventArgs e)
    {
        if (CheckForConfiguration())
        {
            var options = new string[] {
                    "Configure security",
                    "Import users",
                    "Setup permissions"
                };
            TaskDialog.ShowTaskDialogBox(
                "Security Wizards",
                "What would you like to do?",
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Join("|", options),
                TaskDialogButtons.Cancel,
                SysIcons.Information,
                SysIcons.Information);
            switch (TaskDialog.CommandButtonResult)
            {
                case 0:
                    new ConfigureSecurityWizard(rootNode.CruiseControl).Run();
                    break;
                case 1:
                    new ImportUsersWizard(rootNode.CruiseControl).Run();
                    break;
                case 2:
                    new SetPermissionsWizard(rootNode.CruiseControl).Run();
                    break;
            }
        }
    }

    private bool CheckForConfiguration()
    {
        if (rootNode == null)
        {
            MessageBox.Show("You must open or start a configuration before you can do this",
                "Unable to continue",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }
        else
        {
            return true;
        }
    }

    private void previewMenuItem_Click(object sender, EventArgs e)
    {
        if (CheckForConfiguration())
        {
            var window = new XmlPreviewWindow();
            window.GeneratePreview(rootNode.CruiseControl);
            window.ShowDialog(this);
            window.Dispose();
            window = null;
        }
    }
  }
}
