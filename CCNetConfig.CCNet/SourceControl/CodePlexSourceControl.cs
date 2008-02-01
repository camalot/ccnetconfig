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
using CCNetConfig.Core.Components;
using CCNetConfig.Core;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The CodePlex Source Control Block
  /// </summary>
  /// <remarks>
  /// The CodePlex sourcecontrol block requires the Microsoft Visual J# Version 2.0 Redistributable. 
  /// This is normally installed with Microsoft Visual Studio.
  /// </remarks>
  [MinimumVersion ( "1.2" ), Plugin]
  public class CodePlexSourceControl : SourceControl, ICCNetDocumentation {
    private Uri _url = null;
    private string _projectName = string.Empty;
    private string _projectPath = string.Empty;
    private string _workingDirectory = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodePlexSourceControl"/> class.
    /// </summary>
    public CodePlexSourceControl ( )
      : base ( "codeplex" ) {

    }
    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    [Category ( "Optional" ), DefaultValue ( "https://codeplex.com/" ),
    Description ( "This is the Source Control Service Base Url. The default should be fine in most cases." )]
    public Uri Url { get { return this._url; } set { this._url = value; } }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    /// <value>The name of the project.</value>
    [Category ( "Required" ), DefaultValue ( null ), DisplayName ( "(ProjectName)" ),
    Description ( "The url name of the project. If the url to your project is http://codeplex.com/ccnetconfig, then 'ccnetconfig' is the project name." )]
    public string ProjectName { get { return this._projectName; } set { this._projectName = Util.CheckRequired ( this, "ProjectName", value ); } }

    /// <summary>
    /// Gets or sets the project path.
    /// </summary>
    /// <value>The project path.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "The path where to start retrieving the source from." )]
    public string ProjectPath { get { return this._projectPath; } set { this._projectPath = value; } }


    /// <summary>
    /// Gets or sets the working directory.
    /// </summary>
    /// <value>The working directory.</value>
    [Category ( "Optional" ), DefaultValue ( null ),
    Description ( "The root folder where the latest source will be retrieved to from the CodePlex Server. " +
    "This path can either be absolute or it can be relative to the CCNet project working directory." ),
    Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
    BrowseForFolderDescription ( "Select path to the working directory." )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );

      XmlElement ele = doc.CreateElement ( "project" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.ProjectName );
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty ( this.ProjectPath ) ) {
        ele = doc.CreateElement ( "projectPath" );
        ele.InnerText = this.ProjectPath;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );
      }

      if ( this.Url != null ) {
        ele = doc.CreateElement ( "url" );
        ele.InnerText = this.Url.ToString ( );
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.Url = null;
      this.WorkingDirectory = string.Empty;
      this._projectName = string.Empty;
      this.ProjectPath = string.Empty;
      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

      string s = Util.GetElementOrAttributeValue ( "project", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ProjectName = s;

      s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.WorkingDirectory = s;

      s = Util.GetElementOrAttributeValue ( "projectPath", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ProjectPath = s;

      s = Util.GetElementOrAttributeValue ( "url", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Url = new Uri ( s );
    }

    /// <summary>
    /// Creates a copy of the source control object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone ( ) {
      CodePlexSourceControl cpsc = this.MemberwiseClone ( ) as CodePlexSourceControl;
      cpsc.Url = this.Url;
      return cpsc;
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://www.codeplex.com/CodePlex/Wiki/View.aspx?title=CodePlexAPI" ); }
    }

    #endregion
  }
}
