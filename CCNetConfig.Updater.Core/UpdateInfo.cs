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
using System.Xml.Serialization;
using System.ComponentModel;
using CCNetConfig.Core.Enums;

namespace CCNetConfig.Updater.Core {
  

  /// <summary>
  /// Update information
  /// </summary>
  [XmlRoot ( "UpdateInfo", IsNullable = false )]
  public class UpdateInfo {

    private List<UpdateFile> _files;
    private Version _version = null;
    private List<string> _commands = null;
    private UpdateMode _updateMode = UpdateMode.UNKNOWN;
    private DateTime _updateDate = DateTime.Now;
    private string _updateComments = string.Empty;
    private int _changeSet = 0;


    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInfo"/> class.
    /// </summary>
    public UpdateInfo () {
      this._files = new List<UpdateFile> ();
      _commands = new List<string> ();
    }

    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    /// <value>The comments.</value>
    [XmlElement ( "Comments" )]
    public string Comments {
      get { return this._updateComments; }
      set { this._updateComments = value; }
    }

    /// <summary>
    /// Gets or sets the updated date. When being deserialized, the date must be formated in the following
    /// format string 'yyyy-MM-ddTHH:mm:ss.fffffzzz'
    /// </summary>
    /// <value>The updated date.</value>
    [XmlAttribute ( "UpdatedDate" )]
    public DateTime UpdatedDate { get { return this._updateDate; } set { this._updateDate = value; } }

    /// <summary>
    /// Gets the files.
    /// </summary>
    /// <value>The files.</value>
    [XmlArray ( "Files" ),
    XmlArrayItem ( "File" )]
    public List<UpdateFile> Files { get { return this._files; } set { this._files = value; } }

    /// <summary>
    /// Gets or sets the update mode.
    /// </summary>
    /// <value>The mode string.</value>
    [XmlAttribute ( "Mode" )]
    public string ModeString { get { return this.UpdateMode.ToString (); } set { this.UpdateMode = StringToMode ( value ); } }
    /// <summary>
    /// Gets or sets the update mode.
    /// </summary>
    /// <value>The update mode.</value>
    [XmlIgnore]
    public UpdateMode UpdateMode { get { return this._updateMode; } set { this._updateMode = value; } }
    /// <summary>
    /// Gets or sets the commands.
    /// </summary>
    /// <value>The commands.</value>
    [XmlArray ( "Commands" ),
    XmlArrayItem ( "Command" )]
    public List<string> Commands { get { return this._commands; } set { this._commands = value; } }
    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>The version.</value>
    [XmlIgnore]
    public Version Version { get { return this._version; } set { this._version = value; } }
    /// <summary>
    /// Gets or sets the version string.
    /// </summary>
    /// <value>The version string.</value>
    [XmlAttribute ( "Version" ), Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never ), Description ( "Used for the serializer, use Version instead." )]
    public string VersionString { get { return this.Version.ToString (); } set { this.Version = new Version ( value ); } }

    /// <summary>
    /// Gets or sets the change set.
    /// </summary>
    /// <value>The change set.</value>
    [ XmlAttribute( "ChangeSet" ) ]
    public int ChangeSet {
      get { return this._changeSet; }
      set { this._changeSet = value; }
    }
    /// <summary>
    /// String to mode.
    /// </summary>
    /// <param name="val">The val.</param>
    /// <returns></returns>
    private UpdateMode StringToMode ( string val ) {
      object obj = Enum.Parse ( typeof ( UpdateMode ), val );
      if ( obj != null )
        return (UpdateMode)obj;
      else
        return UpdateMode.UNKNOWN;
    }
  }
}
