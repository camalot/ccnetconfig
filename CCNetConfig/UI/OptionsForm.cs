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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core.Enums;
using System.Threading;
using CCNetConfig.Core.Components;
using CCNetConfig.Core.Configuration;
using CCNetConfig.Core.Configuration.Handlers;
using CCNetConfig.Updater.Core;
using System.Diagnostics;
using System.IO;
using CCNetConfig.Components;
using System.Reflection;

namespace CCNetConfig.UI {
	/// <summary>
	/// The options editor
	/// </summary>
	public partial class OptionsForm : Form {
		private delegate void AddComponentToListViewHandler ( Type type );
		private delegate void AutoSizeColumnHeaderHandler ( ColumnHeader header );
		private delegate void AddBackupFileToListViewHandler ( FileInfo file );
		private delegate void AddAssemblyToListViewHandler ( Assembly assembly );
		/// <summary>
		/// Event handler for restoring a file from a backup.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void RestoreConfigureationFromBackupEventHandler ( object sender, BackupFileEventArgs e );
		private delegate void ListViewGenericHandler ( ListView listView );

		ListViewColumnStringSorter _colStringSorter = null;
		ListViewColumnBackupFileSorter _colDateTimeSorter = null;

		/// <summary>
		/// Raised before a file is restored
		/// </summary>
		public event RestoreConfigureationFromBackupEventHandler BeforeRestoreConfigurationFromBackup;
		/// <summary>
		/// Raised after the file is restored
		/// </summary>
		public event RestoreConfigureationFromBackupEventHandler AfterRestoreConfigurationFromBackup;
		private FileInfo _loadedConfigFile = null;
		/// <summary>
		/// Initializes a new instance of the <see cref="OptionsForm"/> class.
		/// </summary>
		public OptionsForm () {
			InitializeComponent ();

			this.pluginUpdateStatus.Text = string.Empty;

			_colStringSorter = new ListViewColumnStringSorter ();
			_colDateTimeSorter = new ListViewColumnBackupFileSorter ();
			this.componentsList.ListViewItemSorter = _colStringSorter;
			this.backupFilesList.ListViewItemSorter = _colDateTimeSorter;
			LoadUserSettings ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionsForm"/> class.
		/// </summary>
		/// <param name="configFile">The config file.</param>
		public OptionsForm ( FileInfo configFile )
			: this () {
			_loadedConfigFile = configFile;
		}

		/// <summary>
		/// Loads the components to list view.
		/// </summary>
		private void LoadComponentsToListView () {
			if ( this.InvokeRequired )
				this.Invoke ( new ListViewGenericHandler ( this.ClearListViewItems ), new object[] { componentsList } );
			else
				ClearListViewItems ( componentsList );

			List<Type> allTypes = new List<Type> ();
			allTypes.AddRange ( CCNetConfig.Core.Util.PublisherTasks );
			allTypes.AddRange ( CCNetConfig.Core.Util.Labellers );
			allTypes.AddRange ( CCNetConfig.Core.Util.Triggers );
			allTypes.AddRange ( CCNetConfig.Core.Util.SourceControls );
			allTypes.AddRange ( CCNetConfig.Core.Util.States );

			foreach ( Type t in allTypes ) {
				if ( this.InvokeRequired )
					this.Invoke ( new AddComponentToListViewHandler ( this.AddComponentToListView ), new object[] { t } );
				else
					this.AddComponentToListView ( t );
			}

			if ( this.InvokeRequired ) {
				this.Invoke ( new AutoSizeColumnHeaderHandler ( this.AutoSizeColumn ), new object[] { this.chName } );
				this.Invoke ( new AutoSizeColumnHeaderHandler ( this.AutoSizeColumn ), new object[] { this.chNamespace } );
				this.Invoke ( new AutoSizeColumnHeaderHandler ( this.AutoSizeColumn ), new object[] { this.chType } );
				this.Invoke ( new AutoSizeColumnHeaderHandler ( this.AutoSizeColumn ), new object[] { this.chAssembly } );
			} else {
				this.AutoSizeColumn ( this.chName );
				this.AutoSizeColumn ( this.chType );
				this.AutoSizeColumn ( this.chNamespace );
				this.AutoSizeColumn ( this.chAssembly );
			}
		}

		private void LoadPluginAssembliesInToListView () {
			if ( this.InvokeRequired )
				this.Invoke ( new ListViewGenericHandler ( this.ClearListViewItems ), new object[] { this.pluginsList } );
			else
				ClearListViewItems ( pluginsList );
			string path = Path.GetDirectoryName ( this.GetType ().Assembly.Location );

			foreach ( Assembly asm in AppDomain.CurrentDomain.GetAssemblies () ) {
				try {
							if ( string.Compare ( path, Path.GetDirectoryName ( asm.Location ) ) == 0 ) {
								if ( this.InvokeRequired ) {
									this.Invoke ( new AddAssemblyToListViewHandler ( this.AddAssemblyToListView ), new object[] { asm } );
								} else {
									AddAssemblyToListView ( asm );
								}
							}
				} catch { }
			}

			if ( this.InvokeRequired ) {
				this.Invoke ( new AutoSizeColumnHeaderHandler ( this.AutoSizeColumn ), new object[] { this.pluginNameColumn } );
				this.Invoke ( new AutoSizeColumnHeaderHandler ( this.AutoSizeColumn ), new object[] { this.pluginAuthorColumn } );
			} else {
				this.AutoSizeColumn ( this.pluginNameColumn );
				this.AutoSizeColumn ( this.pluginAuthorColumn );
			}

		}

		private void LoadBackupsToListView () {
			CCNetConfigSettings settings = CCNetConfig.Core.Util.UserSettings;
			if ( this.InvokeRequired )
				this.Invoke ( new ListViewGenericHandler ( this.ClearListViewItems ), new object[] { backupFilesList } );
			else
				ClearListViewItems ( backupFilesList );

			List<FileInfo> files = new List<FileInfo> ();
			if ( settings.BackupSettings.SavePath.Exists ) {
				files.AddRange ( settings.BackupSettings.SavePath.GetFiles () );
				foreach ( FileInfo file in files ) {
					if ( this.InvokeRequired )
						this.Invoke ( new AddBackupFileToListViewHandler ( this.AddBackupFileToListView ), new object[] { file } );
					else
						this.AddBackupFileToListView ( file );
				}
			}
		}

		/// <summary>
		/// Autoes the size column.
		/// </summary>
		/// <param name="header">The header.</param>
		private void AutoSizeColumn ( ColumnHeader header ) {
			header.AutoResize ( ColumnHeaderAutoResizeStyle.ColumnContent );
		}

		private void ClearListViewItems ( ListView lstView ) {
			lstView.Items.Clear ();
		}

		/// <summary>
		/// Adds the backup file to list view.
		/// </summary>
		/// <param name="file">The file.</param>
		private void AddBackupFileToListView ( FileInfo file ) {
			this.backupFilesList.Items.Add ( new BackupFileListViewItem ( file ) );
		}

		private void AddAssemblyToListView ( Assembly asm ) {
			this.pluginsList.Items.Add ( new AssemblyListViewItem ( asm ) );
		}

		/// <summary>
		/// Adds the component to list view.
		/// </summary>
		/// <param name="type">The type.</param>
		private void AddComponentToListView ( Type type ) {

			ListViewGroup lvg = componentsList.Groups[ "lvgCore" ];
			object[] attributes = type.GetCustomAttributes ( typeof ( PluginAttribute ), true );
			if ( attributes != null && attributes.Length >= 1 )
				lvg = componentsList.Groups[ "lvgPlugin" ];
			ListViewItem lvi = new ListViewItem ( type.Name, lvg );
			string componentName = type.FullName;
			if ( CCNetConfig.Core.Util.UserSettings.Components.Contains ( componentName ) )
				lvi.Checked = CCNetConfig.Core.Util.UserSettings.Components[ componentName ].Display;
			else
				lvi.Checked = true;

			// workitem: 14553
			lvi.SubItems.Add ( type.BaseType.Name );
			lvi.SubItems.Add ( type.Assembly.GetName ().Name );
			lvi.SubItems.Add ( type.Namespace );

			Version exVer = CCNetConfig.Core.Util.GetExactVersion ( type );
			if ( exVer != null )
				lvi.SubItems.Add ( exVer.ToString () );
			else {
				Version minVer = CCNetConfig.Core.Util.GetMinimumVersion ( type );
				Version maxVer = CCNetConfig.Core.Util.GetMaximumVersion ( type );
				StringBuilder sbData = new StringBuilder ();
				if ( minVer != null )
					sbData.AppendFormat ( "Minimum: {0}-", minVer.ToString () );
				if ( maxVer != null )
					sbData.AppendFormat ( "Maximum: {0} ", maxVer.ToString () );
				if ( sbData.Length > 0 )
					sbData = sbData.Remove ( sbData.Length - 1, 1 );
				else
					sbData.Append ( "ALL" );
				lvi.SubItems.Add ( sbData.ToString () );
			}

			this.componentsList.Items.Add ( lvi );
		}

		/// <summary>
		/// Handles the CheckedChanged event of the useProxy control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void useProxy_CheckedChanged ( object sender, EventArgs e ) {
			this.proxyGroup.Enabled = this.useProxy.Checked;
		}

		/// <summary>
		/// Handles the Click event of the okButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void okButton_Click ( object sender, EventArgs e ) {
			this.applyButton.PerformClick ();
			this.DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Loads the user settings.
		/// </summary>
		private void LoadUserSettings () {
			CCNetConfigSettings settings = CCNetConfig.Core.Util.UserSettings;

			updateTypeCombo.DataSource = Enum.GetValues ( typeof ( UpdateCheckType ) );

			//Thread threadLoadComponents = new Thread ( new ThreadStart ( LoadComponentsToListView ) );
			//threadLoadComponents.Start ();

			LoadComponentsToListView ();

			//Thread threadLoadBackupFiles = new Thread ( new ThreadStart ( LoadBackupsToListView ) );
			//threadLoadBackupFiles.Start ();

			LoadBackupsToListView ();

			LoadPluginAssembliesInToListView ();

			this.useProxy.Checked = settings.UpdateSettings.ProxySettings.UseProxy;
			this.proxyServerAddress.Text = settings.UpdateSettings.ProxySettings.ProxyServer;
			this.proxyPort.Value = settings.UpdateSettings.ProxySettings.ProxyPort;
			this.proxyPassword.Text = settings.UpdateSettings.ProxySettings.Password;
			this.proxyUser.Text = settings.UpdateSettings.ProxySettings.Username;

			updateTypeCombo.SelectedIndex = updateTypeCombo.Items.IndexOf ( settings.UpdateSettings.UpdateCheckType );
			this.updatesCheckAtStartup.Checked = settings.UpdateSettings.CheckOnStartup;

			this.backupSavePath.Text = settings.BackupSettings.SavePath.FullName;
			this.backupWhenSaving.Checked = settings.BackupSettings.Enabled;
			this.backupFilesToKeep.Value = settings.BackupSettings.NumberOfBackupsToKeep;

			this.generalMonitorForChanges.Checked = settings.WatchForFileChanges;
			this.externalViewerArguments.Text = settings.ExternalViewerArguments;
			this.externalFileViewer.Text = settings.ExternalViewer;

			this.sortProjectsAlphabetically.Checked = settings.SortProject;
		}

		/// <summary>
		/// Saves the user settings.
		/// </summary>
		private void SaveUserSettings () {
			CCNetConfigSettings settings = new CCNetConfigSettings ();
			settings.Components = new ComponentSettingsList ();
			foreach ( ListViewItem lvi in this.componentsList.Items )
				settings.Components.Add ( new ComponentSettings ( string.Format ( "{0}.{1}", lvi.SubItems[ chNamespace.Index ].Text, lvi.SubItems[ chName.Index ].Text ), lvi.Checked ) );

			settings.UpdateSettings.CheckOnStartup = this.updatesCheckAtStartup.Checked;
			if ( this.updateTypeCombo.SelectedItem != null )
				settings.UpdateSettings.UpdateCheckType = (UpdateCheckType)this.updateTypeCombo.SelectedItem;

			settings.UpdateSettings.ProxySettings.UseProxy = this.useProxy.Checked;
			settings.UpdateSettings.ProxySettings.Username = this.proxyUser.Text.Trim ();
			settings.UpdateSettings.ProxySettings.Password = this.proxyPassword.Text;
			settings.UpdateSettings.ProxySettings.ProxyPort = (int)this.proxyPort.Value;
			settings.UpdateSettings.ProxySettings.ProxyServer = this.proxyServerAddress.Text.Trim ();

			settings.BackupSettings.Enabled = this.backupWhenSaving.Checked;
			settings.BackupSettings.SavePath = new DirectoryInfo ( this.backupSavePath.Text );
			settings.BackupSettings.NumberOfBackupsToKeep = (int)this.backupFilesToKeep.Value;

			settings.ExternalViewer = this.externalFileViewer.Text.Trim ();
			settings.ExternalViewerArguments = this.externalViewerArguments.Text.Trim ();
			settings.WatchForFileChanges = this.generalMonitorForChanges.Checked;

			settings.SortProject = this.sortProjectsAlphabetically.Checked;

			CCNetConfigSettingsConfigurationSectionHandler.SaveSettings ( settings );
			CCNetConfig.Core.Util.RefreshUserSettings ();
		}

		/// <summary>
		/// Handles the Click event of the updatesCheckNowButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void updatesCheckNowButton_Click ( object sender, EventArgs e ) {
			new UpdateChecker ().Check ( this );
		}

		/// <summary>
		/// Handles the ColumnClick event of the componentsList control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.ColumnClickEventArgs"/> instance containing the event data.</param>
		private void componentsList_ColumnClick ( object sender, ColumnClickEventArgs e ) {
			/*if ( e.Column == this._colSorter.SortColumn ) {
				_colSorter.Order = _colSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
			} else {
				_colSorter.SortColumn = e.Column;
				_colSorter.Order = SortOrder.Ascending;
			}
			this.componentsList.Sort ();*/
		}

		/// <summary>
		/// Handles the Click event of the backupPathBrowseButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void backupPathBrowseButton_Click ( object sender, EventArgs e ) {
			FolderBrowserDialog fbd = new FolderBrowserDialog ();
			fbd.ShowNewFolderButton = true;
			if ( CCNetConfig.Core.Util.UserSettings.BackupSettings.SavePath.Exists )
				fbd.SelectedPath = CCNetConfig.Core.Util.UserSettings.BackupSettings.SavePath.FullName;
			if ( fbd.ShowDialog ( this ) == DialogResult.OK )
				this.backupSavePath.Text = fbd.SelectedPath;
			else
				this.backupSavePath.Text = CCNetConfig.Core.Util.UserSettings.BackupSettings.SavePath.FullName;
		}

		/// <summary>
		/// Handles the Click event of the deleteBackupFilesButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void deleteBackupFilesButton_Click ( object sender, EventArgs e ) {
			if ( MessageBox.Show ( this, Properties.Strings.ConfirmDeleteBackupsMessage, Properties.Strings.ConfirmDeleteBackupsTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes ) {
				CCNetConfigSettings settings = CCNetConfig.Core.Util.UserSettings;
				this.backupFilesList.Items.Clear ();
				if ( settings.BackupSettings.SavePath.Exists ) {
					FileInfo[] files = settings.BackupSettings.SavePath.GetFiles ();
					if ( files.Length > 0 ) {
						foreach ( FileInfo file in files )
							file.Delete ();
					}
				}
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the backupWhenSaving control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void backupWhenSaving_CheckedChanged ( object sender, EventArgs e ) {
			this.backupSettingsGroupbox.Enabled = this.backupWhenSaving.Checked;
		}

		/// <summary>
		/// Raises the <see cref="E:BeforeRestoreConfigurationFromBackup"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnBeforeRestoreConfigurationFromBackup ( BackupFileEventArgs e ) {
			if ( this.BeforeRestoreConfigurationFromBackup != null )
				this.BeforeRestoreConfigurationFromBackup ( this, e );
		}

		/// <summary>
		/// Raises the <see cref="E:AfterRestoreConfigurationFromBackup"/> event.
		/// </summary>
		/// <param name="e">The <see cref="CCNetConfig.Components.BackupFileEventArgs"/> instance containing the event data.</param>
		private void OnAfterRestoreConfigurationFromBackup ( BackupFileEventArgs e ) {
			if ( this.AfterRestoreConfigurationFromBackup != null )
				this.AfterRestoreConfigurationFromBackup ( this, e );
		}

		#region Backup File ListView Context Menu Event Handlers
		/// <summary>
		/// Handles the Click event of the restoreBackupFileToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void restoreBackupFileToolStripMenuItem_Click ( object sender, EventArgs e ) {
			if ( this.backupFilesList.SelectedItems.Count > 0 ) {
				if ( _loadedConfigFile == null ) {
					SaveFileDialog sfd = new SaveFileDialog ();
					sfd.Title = "Restore backup to file...";
					sfd.FileName = "ccnet.config";
					sfd.Filter = Properties.Strings.OpenSaveFilter;
					if ( sfd.ShowDialog ( this ) == DialogResult.OK ) {
						_loadedConfigFile = new FileInfo ( sfd.FileName );
					}
				}
				BackupFileListViewItem bflvi = this.backupFilesList.SelectedItems[ 0 ] as BackupFileListViewItem;
				FileInfo backupFile = bflvi.FileInfo;
				FileInfo restoredFile = _loadedConfigFile;
				BackupFileEventArgs be = new BackupFileEventArgs ( backupFile, restoredFile );
				OnBeforeRestoreConfigurationFromBackup ( be );
				bflvi.RestoreBackup ( restoredFile );
				OnAfterRestoreConfigurationFromBackup ( be );
				Thread threadLoadBackupFiles = new Thread ( new ThreadStart ( LoadBackupsToListView ) );
				threadLoadBackupFiles.Start ();
			}
		}

		/// <summary>
		/// Handles the Click event of the viewBackupFileToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void viewBackupFileToolStripMenuItem_Click ( object sender, EventArgs e ) {
			if ( this.backupFilesList.SelectedItems.Count > 0 ) {
				BackupFileListViewItem bflvi = this.backupFilesList.SelectedItems[ 0 ] as BackupFileListViewItem;
				Process proc = new Process ();
				ProcessStartInfo psi = new ProcessStartInfo ( CCNetConfig.Core.Util.UserSettings.ExternalViewer, string.Format ( CCNetConfig.Core.Util.UserSettings.ExternalViewerArguments, bflvi.FileInfo.FullName ) );
				proc.StartInfo = psi;
				proc.Start ();
			}
		}


		/// <summary>
		/// Handles the Click event of the deleteBackupFileToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void deleteBackupFileToolStripMenuItem_Click ( object sender, EventArgs e ) {
			if ( backupFilesList.SelectedItems.Count > 0 ) {
				if ( MessageBox.Show ( this, Properties.Strings.ConfirmDeleteSelectedBackupsMessage, Properties.Strings.ConfirmDeleteBackupsTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes ) {
					for ( int x = 0; x < backupFilesList.SelectedItems.Count; x++ ) {
						BackupFileListViewItem lvi = backupFilesList.SelectedItems[ x ] as BackupFileListViewItem;
						try {
							lvi.FileInfo.Delete ();
							this.backupFilesList.Items.Remove ( lvi );
						} catch { }
					}
				}
			}
		}
		#endregion
		#region Backup File ListView Event Handlers
		/// <summary>
		/// Handles the MouseClick event of the backupFilesList control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		private void backupFilesList_MouseClick ( object sender, MouseEventArgs e ) {
			if ( this.backupFilesList.SelectedItems.Count > 0 ) {
				if ( e.Button == MouseButtons.Right )
					this.backupFilesContextMenu.Show ( backupFilesList, e.Location );
			}
		}

		/// <summary>
		/// Handles the MouseDoubleClick event of the backupFilesList control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		private void backupFilesList_MouseDoubleClick ( object sender, MouseEventArgs e ) {
			viewBackupFileToolStripMenuItem.PerformClick ();
		}


		/// <summary>
		/// Handles the KeyDown event of the backupFilesList control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
		private void backupFilesList_KeyDown ( object sender, KeyEventArgs e ) {
			if ( this.backupFilesList.SelectedItems.Count > 0 ) {
				switch ( e.KeyCode ) {
					case Keys.Delete:
						deleteBackupFileToolStripMenuItem.PerformClick ();
						break;
				}
			}
		}
		#endregion

		/// <summary>
		/// Handles the Click event of the applyButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void applyButton_Click ( object sender, EventArgs e ) {
			SaveUserSettings ();
		}


		internal class AssemblyListViewItem : ListViewItem {
			public AssemblyListViewItem ( Assembly assembly )
				: base ( assembly.GetName ().Name ) {
				this.Assembly = assembly;
				object[] attrs = this.Assembly.GetCustomAttributes ( true );
				string author = "Unknown";
				for ( int i = 0; i < attrs.Length; ++i ) {
					if ( attrs[ i ].GetType () == typeof ( UpdateFeedAttribute ) ) {
						UpdateFeedAttribute ufa = attrs[ i ] as UpdateFeedAttribute;
						this.HasUpdateUrl = true;
						this.UpdateUrl = ufa.Url;
					} else if ( attrs[ i ].GetType () == typeof ( AssemblyCompanyAttribute ) ) {
						AssemblyCompanyAttribute aca = attrs[ i ] as AssemblyCompanyAttribute;
						author = string.IsNullOrEmpty ( aca.Company ) ? "Unknown" : aca.Company;
					}
				}

				this.Version = assembly.GetName ().Version;

				this.SubItems.Add ( author );
			}
			public Version Version { get; private set; }
			public Assembly Assembly { get; private set; }
			public bool HasUpdateUrl { get; private set; }
			public Uri UpdateUrl { get; private set; }
		}

		private void updatePlugin_Click ( object sender, EventArgs e ) {
			if ( this.pluginsList.SelectedItems.Count > 0 ) {
				AssemblyListViewItem alvi = this.pluginsList.SelectedItems[ 0 ] as AssemblyListViewItem;
				ApplicationUpdater au = new ApplicationUpdater ();
				au.Version = alvi.Version;

				this.pluginUpdateStatus.Text = "Checking updates for " + alvi.Text;

				au.UpdateAvailable += new EventHandler<UpdatesAvailableEventArgs> ( delegate ( object s, UpdatesAvailableEventArgs uae ) {
					ApplicationUpdater ai = (ApplicationUpdater)s;
					ai.UpdateName = alvi.Text;
					this.pluginUpdateStatus.Text = "Updates available...";
					if ( ai.UpdatesAvailable ) {
						UpdateInformationForm uif = new UpdateInformationForm ( ai );
						if ( uif.ShowDialog ( null ) == DialogResult.OK ) {

							string thisPath = this.GetType ().Assembly.Location;
							string attrString = string.Format ( CCNetConfig.Core.Util.UserSettings.UpdateSettings.LaunchArgumentsFormat,
								CCNetConfig.Core.Util.UserSettings.UpdateSettings.UpdateCheckType, thisPath,
								ai.UpdateInfoList.GetLatestVersion () );
							Process.Start ( Path.Combine ( Application.StartupPath, CCNetConfig.Core.Util.UserSettings.UpdateSettings.UpdaterApplication ), attrString );
							Application.Exit ();
						}
					}
				} );

				au.UpdateException += new EventHandler<ExceptionEventArgs> ( delegate ( object s, ExceptionEventArgs ex ) {
					MessageBox.Show ( ex.Exception.Message, "Update of plugin error" );
				} );

				au.CheckForUpdatesByUrl ( alvi.UpdateUrl );
				this.pluginUpdateStatus.Text = string.Empty;
			}

		}

		void au_UpdateException ( object sender, ExceptionEventArgs e ) {
			throw new NotImplementedException ();
		}

		private void pluginsList_SelectedIndexChanged ( object sender, EventArgs e ) {
			if ( pluginsList.SelectedItems.Count > 0 ) {
				AssemblyListViewItem alvi = this.pluginsList.SelectedItems[ 0 ] as AssemblyListViewItem;
				this.updatePlugin.Enabled = alvi.HasUpdateUrl;
			}
		}
	}
}