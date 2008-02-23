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
using System.ComponentModel;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using CCNetConfig.Core.Serialization;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The <see cref="CCNetConfig.CCNet.BuildPublisher">Build Publisher</see> lets you copy any arbitrary files on a successful build.
  /// </summary>
  [MinimumVersion ( "1.0" ), ReflectorName("buildpublisher")]
  public class BuildPublisher : PublisherTask, ICCNetDocumentation {
    private string _sourceDir = string.Empty;
    private string _destDir = string.Empty;
    private bool? _useLabelSubDirectory = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="BuildPublisher"/> class.
    /// </summary>
    public BuildPublisher ( ) : base ( "buildpublisher" ) { }
    /// <summary>
    /// The source Directory to copy from
    /// </summary>
    [Description ( "The source Directory to copy from" ), DefaultValue ( null ),
    Category ( "Optional" ), ReflectorName ( "sourceDir" ),
    Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
    BrowseForFolderDescription ( "Select path to the source directory." )]
    public string SourceDirectory { get { return this._sourceDir; } set { this._sourceDir = value; } }
    /// <summary>
    /// The directory to copy to. A subdirectory called the current build's label will be created, and the contents of <see cref="CCNetConfig.CCNet.BuildPublisher.SourceDirectory">SourceDirectory</see> will be copied to it
    /// </summary>
    [Description ( "The directory to copy to. A subdirectory called the current build's label will be created, and the contents of SourceDirectory will " +
     "be copied to it" ), DefaultValue ( null ),
    Category ( "Optional" ), ReflectorName ( "publishDir" ),
    Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
    BrowseForFolderDescription ( "Select path to the publish directory." )]
    public string PublishDirectory { get { return this._destDir; } set { this._destDir = value; } }
    /// <summary>
    ///If set to true (the default value), files will be copied to subdirectory under the publishDir which will be named with the label for the current integration.
    /// </summary>
    /// <value>The use label sub directory.</value>
    [Description ( "If set to true (the default value), files will be copied to subdirectory under the publishDir which will be named with the label for the current integration." ),
    Category ( "Optional" ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
    MinimumVersion ( "1.2" ), ReflectorName ( "useLabelSubDirectory" )]
    public bool? UseLabelSubDirectory {
      get { return this._useLabelSubDirectory; }
      set { this._useLabelSubDirectory = value; }
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      return this.MemberwiseClone ( ) as BuildPublisher;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<BuildPublisher> ( ).Serialize ( this );
      /*XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = doc.CreateElement ( "sourceDir" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.SourceDirectory );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "publishDir" );
      ele.InnerText = Util.CheckRequired (this, ele.Name, this.PublishDirectory);
      root.AppendChild ( ele );

      if ( this.UseLabelSubDirectory.HasValue ) {
        ele = doc.CreateElement ( "useLabelSubDirectory" );
        ele.InnerText = this.UseLabelSubDirectory.Value.ToString ( );
        root.AppendChild ( ele );
      }
      return root;*/
    }


    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false ), ReflectorIgnore]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Build+Publisher?decorator=printable" ); }
    }
    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.SourceDirectory = Util.GetElementOrAttributeValue ( "sourceDir", element );
      this.PublishDirectory = Util.GetElementOrAttributeValue ( "publishDir", element );
      string s = Util.GetElementOrAttributeValue ( "useLabelSubDirectory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.UseLabelSubDirectory = string.Compare ( s, bool.TrueString, true ) == 0;
    }
  }
}
