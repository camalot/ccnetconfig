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
using System.IO;

namespace CCNetConfig.Core.Configuration {
  /// <summary>
  /// Settings for backing up the config file.
  /// </summary>
  [ XmlRoot( "Backup" ) ]
  public class BackupSettings {
    private bool _enabled = true;
    private int _numberOfBackups = 0;
    private DirectoryInfo _savePath = null;
    private DirectoryInfo _defaultPath = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="BackupSettings"/> class.
    /// </summary>
    public BackupSettings () {
      _defaultPath = new DirectoryInfo ( Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments ), Properties.Resources.BackupDefaultFolderName ) );
      _savePath = _defaultPath;
    }

    /// <summary>
    /// Gets or sets the save path as a string.
    /// </summary>
    /// <value>The save path as a string.</value>
    [ XmlElement( "SavePath" ) ]
    public string SavePathString {
      get { return this._savePath.FullName; }
      set { this._savePath = new DirectoryInfo ( value ); }
    }

    /// <summary>
    /// Gets or sets the save path.
    /// </summary>
    /// <value>The save path.</value>
    [ XmlIgnore ]
    public DirectoryInfo SavePath {
      get { return this._savePath; }
      set { this._savePath = value ?? _defaultPath ; } 
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="BackupSettings"/> is enabled.
    /// </summary>
    /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
    [ XmlAttribute( "Enabled" ) ]
    public bool Enabled {
      get { return this._enabled; }
      set { this._enabled = value; }
    }

    /// <summary>
    /// Gets or sets the number of backups to keep.
    /// </summary>
    /// <value>The number of backups to keep.</value>
    [ XmlElement( "NumberOfBackupsToKeep" ) ]
    public int NumberOfBackupsToKeep {
      get { return this._numberOfBackups; }
      set { this._numberOfBackups = value; }
    }
  }
}
