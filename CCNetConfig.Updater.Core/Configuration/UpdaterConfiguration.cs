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
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CCNetConfig.Updater.Core.Configuration {
  /// <summary>
  /// Information for the updater.
  /// </summary>
  [ XmlRoot( "CCNetConfig.Updater.Configuration" ) ]
  public class UpdaterConfiguration {
    private Uri _allBuildsUpdateInfoUri = null;
    private Uri _betaBuildsUpdateInfoUri = null;
    private Uri _releaseBuildsUpdateInfoUri = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdaterConfiguration"/> class.
    /// </summary>
    public UpdaterConfiguration () {

    }

    /// <summary>
    /// Gets or sets all builds update info URI.
    /// </summary>
    /// <value>All builds update info URI.</value>
    [ XmlElement( "AllBuildsFeedUri" ), Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ) ]
    public string AllBuildsUpdateInfoUriString {
      get { return this._allBuildsUpdateInfoUri.ToString(  ); }
      set { this._allBuildsUpdateInfoUri = new Uri(value); }
    }

    /// <summary>
    /// Gets or sets all builds update info URI.
    /// </summary>
    /// <value>All builds update info URI.</value>
    [ XmlIgnore ]
    public Uri AllBuildsUpdateInfoUri {
      get { return this._allBuildsUpdateInfoUri; }
      set { this._allBuildsUpdateInfoUri = value; }
    }

    /// <summary>
    /// Gets or sets the beta builds URI.
    /// </summary>
    /// <value>The beta builds URI.</value>
    [XmlElement ( "BetaBuildsFeedUri" ), Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
    public string BetaBuildsUpdateInfoUriString {
      get { return this._betaBuildsUpdateInfoUri.ToString(  ); }
      set { this._betaBuildsUpdateInfoUri = new Uri(value); }
    }

    /// <summary>
    /// Gets or sets the beta builds update info URI.
    /// </summary>
    /// <value>The beta builds update info URI.</value>
    [ XmlIgnore ]
    public Uri BetaBuildsUpdateInfoUri {
      get { return this._betaBuildsUpdateInfoUri; }
      set { this._betaBuildsUpdateInfoUri = value; }
    }

    /// <summary>
    /// Gets or sets the release build update info URI.
    /// </summary>
    /// <value>The release build update info URI.</value>
    [XmlElement ( "ReleaseBuildsFeedUri" ), Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
    public string ReleaseBuildUpdateInfoUriString {
      get { return this._releaseBuildsUpdateInfoUri.ToString(  ); }
      set { this._releaseBuildsUpdateInfoUri = new Uri(value); }
    }

    /// <summary>
    /// Gets or sets the release build update info URI.
    /// </summary>
    /// <value>The release build update info URI.</value>
    [ XmlIgnore ]
    public Uri ReleaseBuildUpdateInfoUri {
      get { return this._releaseBuildsUpdateInfoUri; }
      set { this._releaseBuildsUpdateInfoUri = value; }
    }
    /// <summary>
    /// Gets the temp directory.
    /// </summary>
    /// <value>The temp directory.</value>
    [XmlIgnore]
    public DirectoryInfo TempDirectory {
      get { return new DirectoryInfo( Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.LocalApplicationData ), string.IsNullOrEmpty ( Properties.Strings.UpdaterTempPathFolder ) ? @"CCNetConfigApplicationUpdater\" : Properties.Strings.UpdaterTempPathFolder) ); }
    }
  }
}