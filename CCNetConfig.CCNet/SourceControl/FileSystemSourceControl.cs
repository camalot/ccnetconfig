/* Copyright (c) 2006, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core;
using System.ComponentModel;
using System.Xml;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Use the 'File' Source Control plugin to check for modifications on a directory accessable by the build server. You can use either directories on 
  /// 'mapped' drives (local or remote), or UNC paths (remote).
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class FileSystemSourceControl : SourceControl, ICCNetDocumentation {
    private string _repositoryRoot = string.Empty;
    private bool? _autoGetSource = null;
    private bool? _ignoreMissingRoot = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemSourceControl"/> class.
    /// </summary>
    public FileSystemSourceControl () : base("filesystem") { }

    /// <summary>
    /// The directory to check for changes. This directory will be checked recursively.
    /// </summary>
    [Description("The directory to check for changes. This directory will be checked recursively."),
  DefaultValue ( null ), DisplayName ( "(RepositoryRoot)" ), Category ( "Required" ),
    Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
BrowseForFolderDescription ( "Select directory to check for changes." )]
    public string RepositoryRoot { 
      get { return this._repositoryRoot; } 
      set { this._repositoryRoot = Util.CheckRequired ( this, "repositoryRoot", value ); } 
    }
    /// <summary>
    /// Whether to automatically (recursively) copy the contents of the repositoryRoot directory to the Project Working Directory
    /// </summary>
    [Description("Whether to automatically (recursively) copy the contents of the repositoryRoot directory to the Project Working Directory"),
    DefaultValue(null),Editor(typeof(DefaultableBooleanUIEditor),typeof(UITypeEditor)),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// Whether to not fail if the repository doesn't exist
    /// </summary>
    [Description ( "Whether to not fail if the repository doesn't exist" ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? IgnoreMissingRoot { get { return this._ignoreMissingRoot; } set { this._ignoreMissingRoot = value; } }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      return this.MemberwiseClone () as FileSystemSourceControl;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = doc.CreateElement ( "repositoryRoot" );
      ele.InnerText = Util.CheckRequired ( this, "repositoryRoot", this.RepositoryRoot );
      root.AppendChild ( ele );

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.IgnoreMissingRoot.HasValue ) {
        ele = doc.CreateElement ( "ignoreMissingRoot" );
        ele.InnerText = this.IgnoreMissingRoot.Value.ToString ();
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.AutoGetSource = null;
      this.IgnoreMissingRoot = null;
      this._repositoryRoot = string.Empty;

      if ( string.Compare (element.GetAttribute ("type"), this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute ("type"), this.TypeName));

      this.RepositoryRoot = Util.GetElementOrAttributeValue ("repositoryRoot", element);

      string s = Util.GetElementOrAttributeValue ("autoGetSource", element);
      if ( !string.IsNullOrEmpty (s) ) 
        this.AutoGetSource = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("ignoreMissingRoot", element);
      if ( !string.IsNullOrEmpty (s) )
        this.IgnoreMissingRoot = string.Compare (s, bool.TrueString, true) == 0;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable (false)]
    public Uri DocumentationUri {
      get { return new Uri ("http://ccnet.thoughtworks.net/display/CCNET/Filesystem+Source+Control+Block?decorator=printable"); }
    }
    #endregion
  }
}
