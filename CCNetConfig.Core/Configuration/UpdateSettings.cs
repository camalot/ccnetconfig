/*
 * Copyright (c) 2006-2007, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core.Enums;

namespace CCNetConfig.Core.Configuration {
  /// <summary>
  /// The update settings for CCNetConfig
  /// </summary>
  [XmlRoot ( "UpdateSettings" )]
  public class UpdateSettings {
    private bool _checkOnStartup = true;
    private UpdateCheckType _checkType = UpdateCheckType.ReleaseBuilds;
    private UpdateProxySettings _proxySettings = null;
    private bool _showOnlyLatestVersion = true;
    private string _launchArgumentsFormat = "/mode={0} /version={2} /app={1}";
    private string _updaterApplication = "CCNetConfig.Updater.exe";
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSettings"/> class.
    /// </summary>
    public UpdateSettings () {
      _proxySettings = new UpdateProxySettings ();
    }
    /// <summary>
    /// Gets or sets the updater application.
    /// </summary>
    /// <value>The updater application.</value>
    [ XmlElement( "UpdaterApplication" ) ]
    public string UpdaterApplication { get { return this._updaterApplication; } set { this._updaterApplication = value; } }

    /// <summary>
    /// Gets or sets the launch arguments format.
    /// </summary>
    /// <value>The launch arguments format.</value>
    [ XmlElement( "LaunchArgumentsFormat" ) ]
    public string LaunchArgumentsFormat { get { return this._launchArgumentsFormat; } set { this._launchArgumentsFormat = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether [check on startup].
    /// </summary>
    /// <value><c>true</c> if [check on startup]; otherwise, <c>false</c>.</value>
    [XmlElement ( "CheckOnStartup" )]
    public bool CheckOnStartup { get { return this._checkOnStartup; } set { this._checkOnStartup = value; } }

    /// <summary>
    /// Gets or sets the type of the update check.
    /// </summary>
    /// <value>The type of the update check.</value>
    [XmlElement ( "CheckType" )]
    public UpdateCheckType UpdateCheckType { get { return this._checkType; } set { this._checkType = value; } }

    /// <summary>
    /// Gets or sets the proxy settings.
    /// </summary>
    /// <value>The proxy settings.</value>
    [XmlElement ( "ProxySettings" )]
    public UpdateProxySettings ProxySettings {
      get { return this._proxySettings; }
      set { this._proxySettings = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to only show the latest version information in the update dialog.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [show only latest version]; otherwise, <c>false</c>.
    /// </value>
    [XmlElement ( "ShowOnlyLatestVersion" )]
    public bool ShowOnlyLatestVersion { get { return this._showOnlyLatestVersion; } set { this._showOnlyLatestVersion = value; } }
  }
}
