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
using System.Xml;
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// This task writes the detected modifications for the current integration to a file as xml. This enables the modifications to be used by 
  /// external programs, such as within a NAnt build script.
  /// </summary>
  /// <remarks>
  /// see <a href="http://confluence.public.thoughtworks.org/display/CCNET/Modification+Writer+Task">Modification Writer Task</a> documentation for more info.
  /// </remarks>
  [ MinimumVersion( "1.0" ) ]
  public class ModificationWriterTask : PublisherTask, ICCNetDocumentation {
    private string _fileName = string.Empty;
    private string _path = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="ModificationWriterTask"/> class.
    /// </summary>
    public ModificationWriterTask () : base("modificationWriter") { }

    /// <summary>
    /// The directory to write the xml file to.
    /// </summary>
    [Description ( "The directory to write the xml file to." ), DefaultValue ( null ), Category ( "Optional" ),
 Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
BrowseForFolderDescription ( "Select path to the output directory." )]
    public string Path { get { return this._path; } set { this._path = value; } }
    /// <summary>
    /// The filename for the file containing the modifications.
    /// </summary>
    [Description ("The filename for the file containing the modifications."), DefaultValue (null),Category("Optional")]
    public string FileName { get { return this._fileName; } set { this._fileName = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      return this.MemberwiseClone () as ModificationWriterTask;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      if ( !string.IsNullOrEmpty ( this.FileName ) ) {
        XmlElement ele = doc.CreateElement ( "filename" );
        ele.InnerText = this.FileName.Trim ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Path ) ) {
        XmlElement ele = doc.CreateElement ( "path" );
        ele.InnerText = this.Path.Trim ();
        root.AppendChild ( ele );
      }
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.FileName = string.Empty;
      this.Path = string.Empty;

      if ( string.Compare (element.Name, this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.Name, this.TypeName));

      string s = Util.GetElementOrAttributeValue ("filename", element);
      if ( !string.IsNullOrEmpty (s) )
        this.FileName = s;

      s = Util.GetElementOrAttributeValue ("path", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Path = s;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ("http://confluence.public.thoughtworks.org/display/CCNET/Modification+Writer+Task?decorator=printable"); }
    }

    #endregion
  }
}
