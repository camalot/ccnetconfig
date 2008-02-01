/*
 * Copyright (c) 2007-2008, Ryan Conrad. All rights reserved.
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
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CCNetConfig.Core.Configuration {
  /// <summary>
  /// Represents the settings for CCNetConfig
  /// </summary>
  [XmlRoot ( "CCNetConfig.Settings" )]
  public class CCNetConfigSettings {
    private ComponentSettingsList _components = null;
    private UpdateSettings _updateSettings = null;
    private BackupSettings _backupSettings = null;

    private string _externalViewer = string.Empty;
    private string _externalViewerArguments = string.Empty;
    private bool _minimizeToTray = false;
    private bool _minimizeOnClose = false;
    private bool _watchForChanges = false;
    private bool _sortProjects = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="CCNetConfigSettings"/> class.
    /// </summary>
    public CCNetConfigSettings () {
      _components = new ComponentSettingsList ();
      _updateSettings = new UpdateSettings ();
      _backupSettings = new BackupSettings ();
      _externalViewer = Properties.Resources.DefaultExternalViewer;
      _externalViewerArguments = Properties.Resources.DefaultExternalViewerArguments;
    }

    /// <summary>
    /// Gets or sets the components.
    /// </summary>
    /// <value>The components.</value>
    [XmlArray ( "Components" ), XmlArrayItem ( "Component" )]
    public ComponentSettingsList Components { get { return this._components; } set { this._components = value; } }

    /// <summary>
    /// Gets or sets the update settings.
    /// </summary>
    /// <value>The update settings.</value>
    [XmlElement ( "UpdateSettings" )]
    public UpdateSettings UpdateSettings { get { return this._updateSettings; } set { this._updateSettings = value; } }

    /// <summary>
    /// Gets or sets the backup settings.
    /// </summary>
    /// <value>The backup settings.</value>
    [XmlElement ( "BackupSettings" )]
    public BackupSettings BackupSettings { get { return this._backupSettings; } set { this._backupSettings = value; } }

    /// <summary>
    /// Gets or sets the external viewer.
    /// </summary>
    /// <value>The external viewer.</value>
    [XmlElement ( "ExternalViewer" )]
    public string ExternalViewer { get { return this._externalViewer; } set { this._externalViewer = string.IsNullOrEmpty(value) ? Properties.Resources.DefaultExternalViewer : value; } }

    /// <summary>
    /// Gets or sets the external viewer arguments.
    /// </summary>
    /// <value>The external viewer arguments.</value>
    [XmlElement ( "ExternalViewerArguments" )]
    public string ExternalViewerArguments { get { return this._externalViewerArguments; } set { this._externalViewerArguments = string.IsNullOrEmpty(value) ? Properties.Resources.DefaultExternalViewerArguments : value; } }

    /// <summary>
    /// Gets or sets a value indicating whether [minimize to tray].
    /// </summary>
    /// <value><c>true</c> if [minimize to tray]; otherwise, <c>false</c>.</value>
    [XmlElement ( "MinimizeToTray" )]
    public bool MinimizeToTray { get { return this._minimizeToTray; } set { this._minimizeToTray = value; } }

    /// <summary>
    /// Gets or sets a value indicating whether [minimize to tray on close].
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [minimize to tray on close]; otherwise, <c>false</c>.
    /// </value>
    [XmlElement ( "MinimizeToTrayOnClose" )]
    public bool MinimizeToTrayOnClose { get { return this._minimizeOnClose; } set { this._minimizeOnClose = value; } }

    /// <summary>
    /// Gets or sets a value indicating whether [watch for file changes].
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [watch for file changes]; otherwise, <c>false</c>.
    /// </value>
    [XmlElement ( "WatchForFileChanges" )]
    public bool WatchForFileChanges { get { return this._watchForChanges; } set { this._watchForChanges = value; } }

    /// <summary>
    /// Gets or sets a value indicating whether to sort the project.
    /// </summary>
    /// <value>
    /// 	<see langword="true"/> if projects should be sorted; otherwise, <see langword="false"/>.
    /// </value>
    [XmlElement ( "SortProjects" )]
    public bool SortProject { get { return this._sortProjects; } set { this._sortProjects = value; } }
  }
}
