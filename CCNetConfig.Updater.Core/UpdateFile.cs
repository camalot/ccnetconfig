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

namespace CCNetConfig.Updater.Core {
  /// <summary>
  /// Information about the update file.
  /// </summary>
  [Serializable]
  public class UpdateFile {
    private long _size = 0;
    private Uri _location = null;
    private bool _restart = false;
    private Version _version = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateFile"/> class.
    /// </summary>
    public UpdateFile () { }
    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    /// <value>The size.</value>
    [XmlAttribute ( "FileSize" )]
    public long Size { get { return this._size; } set { this._size = value; } }
    /// <summary>
    /// Gets or sets the location.
    /// </summary>
    /// <value>The location.</value>
    //[XmlAttribute ( "Url")]
    [XmlIgnore]
    public Uri Location { get { return this._location; } set { this._location = value; } }
    /// <summary>
    /// Gets or sets the location URI.
    /// </summary>
    /// <value>The location URI.</value>
    [XmlAttribute ( "Location" ), Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never ), Description ( "Used for the serializer, use Location instead." )]
    public string LocationUri { get { return this.Location.ToString (); } set { this.Location = new Uri ( value ); } }
    /// <summary>
    /// Gets or sets the restart.
    /// </summary>
    /// <value>The restart.</value>
    [XmlAttribute ( "Restart" )]
    //[XmlIgnore]
    public bool Restart { get { return this._restart; } set { this._restart = value; } }
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
  }
}
