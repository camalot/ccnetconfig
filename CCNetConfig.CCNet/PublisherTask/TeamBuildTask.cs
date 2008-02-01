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
using CCNetConfig.Core.Components;
using CCNetConfig.Core;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using System.Drawing;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Team Build provides a mechanism for passing custom MSBuild properties into it via a source 
  /// controlled file called TFSBuild.rsp.  This file consists of 1 or more lines each containing a 
  /// valid MSBuild commandline switch (e.g. /p:PropName=value, would set the property 'PropName' to 
  /// be 'value').  With that in mind, I developed the plug-in to allow these switches to be specified 
  /// in the CCNet server configuration file (on a per CCNet project basis) and have it update the 
  /// '.rsp' (in source control) before calling the build web service.
  /// </summary>
  [MinimumVersion ( "1.2" ), Plugin]
  public class TeamBuildTask : PublisherTask, ICCNetDocumentation {
    private Uri _tfsServer = null;
    private string _server = string.Empty;
    private string _project = string.Empty;
    private string _buildType = string.Empty;
    private string _workspace = string.Empty;
    private string _dropsLocation = string.Empty;
    private CloneableList<string> _buildParams = null;
    private bool? _useMsTest = null;
    private bool? _useMsTestCodeCoverage = null;
    private bool? _useXmlLogger = null;


    /// <summary>
    /// Initializes a new instance of the <see cref="TeamBuildTask"/> class.
    /// </summary>
    public TeamBuildTask ( )
      : base ( "teambuild" ) {
      _buildParams = new CloneableList<string> ( );
    }


    /// <summary>
    /// Gets or sets the use XML logger.
    /// </summary>
    /// <value>The use XML logger.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "Use Xml Logger" ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? UseXmlLogger {
      get { return _useXmlLogger; }
      set { _useXmlLogger = value; }
    }

    /// <summary>
    /// Gets or sets the TFS server.
    /// </summary>
    /// <value>The TFS server.</value>
    [Category ( "Required" ), DisplayName ( "(TFSServer)" ), Description ( "The TFS Server Uri" ), DefaultValue ( null )]
    public Uri TFSServer { get { return this._tfsServer; } set { this._tfsServer = Util.CheckRequired ( this, "tfsServerUri", value ); } }

    /// <summary>
    /// Gets or sets the build server.
    /// </summary>
    /// <value>The build server.</value>
    [Category ( "Required" ), DisplayName ( "(BuildServer)" ), Description ( "The Build Server" ), DefaultValue ( null )]
    public string BuildServer { get { return this._server; } set { this._server = Util.CheckRequired ( this, "teamBuildServer", value ); } }
    // <summary>
    // Gets or sets the drops location.
    // </summary>
    // <value>The drops location.</value>
    /*[Category ( "Optional" ), Description ( "The Drops location." ), DefaultValue ( null ),
     BrowseForFolderDescription ( "Select Drop Location." ), Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) )]
    public string DropsLocation { get { return this._dropsLocation; } set { this._dropsLocation = value; } }
    */
    /// <summary>
    /// Gets or sets the type of the build.
    /// </summary>
    /// <value>The type of the build.</value>
    [Category ( "Required" ), DisplayName ( "(BuildType)" ), Description ( "The Build Type" ), DefaultValue ( null )]
    public string BuildType { get { return this._buildType; } set { this._buildType = Util.CheckRequired ( this, "teamBuildType", value ); } }
    /// <summary>
    /// Gets or sets the project.
    /// </summary>
    /// <value>The project.</value>
    [Category ( "Required" ), DisplayName ( "(Project)" ), Description ( "The Team Project" ), DefaultValue ( null )]
    public string Project { get { return this._project; } set { this._project = Util.CheckRequired ( this, "teamProject", value ); } }
    /// <summary>
    /// Gets or sets the use MS test.
    /// </summary>
    /// <value>The use MS test.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "Use MSTest" ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? UseMSTest { get { return this._useMsTest; } set { this._useMsTest = value; } }
    /// <summary>
    /// Gets or sets the use ms test code coverage.
    /// </summary>
    /// <value>The use ms test code coverage.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "Use MSTest Code Coverage" ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? UseMsTestCodeCoverage { get { return this._useMsTestCodeCoverage; } set { this._useMsTestCodeCoverage = value; } }

    /// <summary>
    /// Gets or sets the workspace.
    /// </summary>
    /// <value>The workspace.</value>
    [Category ( "Required" ), DisplayName ( "(Workspace)" ), Description ( "The TFS Workspace name" ), DefaultValue ( null )]
    public string Workspace { get { return this._workspace; } set { this._workspace = Util.CheckRequired ( this, "workspace", value ); } }

    /// <summary>
    /// Gets or sets the build parameters.
    /// </summary>
    /// <value>The build parameters.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "MSBuild Parameters" ),
    TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<string> BuildParameters { get { return this._buildParams; } set { this._buildParams = value; } }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( this.TypeName );

      XmlElement ele = doc.CreateElement ( "tfsServerUri" );
      ele.InnerText = this.TFSServer.ToString ( );
      root.AppendChild ( ele );

      /*if ( !string.IsNullOrEmpty ( this.DropsLocation ) ) {
        ele = doc.CreateElement ( "dropsLocation" );
        ele.InnerText = this.DropsLocation;
        root.AppendChild ( ele );
      }*/

      ele = doc.CreateElement ( "teamBuildServer" );
      ele.InnerText = this.BuildServer;
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "teamBuildType" );
      ele.InnerText = this.BuildType;
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "teamProject" );
      ele.InnerText = this.Project;
      root.AppendChild ( ele );

      if ( this.UseMSTest.HasValue ) {
        ele = doc.CreateElement ( "useMsTest" );
        ele.InnerText = this.UseMSTest.Value.ToString();
        root.AppendChild ( ele );
      }

      if ( this.UseXmlLogger.HasValue ) {
        ele = doc.CreateElement ( "useXmlLogger" );
        ele.InnerText = this.UseXmlLogger.Value.ToString ( );
        root.AppendChild ( ele );
      }


      if ( this.UseMsTestCodeCoverage.HasValue ) {
        ele = doc.CreateElement ( "useMsTestCodeCoverage" );
        ele.InnerText = this.UseMsTestCodeCoverage.Value.ToString ( );
        root.AppendChild ( ele );
      }

      ele = doc.CreateElement ( "workspace" );
      ele.InnerText = this.Workspace;
      root.AppendChild ( ele );

      if ( this.BuildParameters.Count > 0 ) {
        ele = doc.CreateElement ( "msBuildParameters" );
        foreach ( string s in this.BuildParameters ) {
          XmlElement tele = doc.CreateElement ( "msBuildParameter" );
          tele.InnerText = s;
        }
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.BuildParameters = new CloneableList<string> ( );
      this._workspace = string.Empty;
      this.UseMSTest = null;
      this.UseMsTestCodeCoverage = null;
      this._project = string.Empty;
      //this.DropsLocation = string.Empty;
      this._server = string.Empty;
      this._buildType = string.Empty;
      this._project = string.Empty;
      this._tfsServer = null;
      this.UseXmlLogger = null;

      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      string s = Util.GetElementOrAttributeValue ( "workspace", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Workspace = s;

      XmlElement paramElement = element.SelectSingleNode ( "msBuildParameters" ) as XmlElement;
      foreach ( XmlElement tele in paramElement.SelectNodes("./*") )
        this.BuildParameters.Add ( tele.InnerText );

      s = Util.GetElementOrAttributeValue ( "useMsTest", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.UseMSTest = string.Compare(s,bool.TrueString,true) == 0;

      s = Util.GetElementOrAttributeValue ( "useXmlLogger", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.UseXmlLogger = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "useMsTestCodeCoverage", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.UseMsTestCodeCoverage = string.Compare ( s, bool.TrueString, true ) == 0;

     /* s = Util.GetElementOrAttributeValue ( "dropsLocation", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.DropsLocation = s;
      */
      s = Util.GetElementOrAttributeValue ( "teamBuildServer", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.BuildServer = s;

      s = Util.GetElementOrAttributeValue ( "teamBuildType", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.BuildType = s;

      s = Util.GetElementOrAttributeValue ( "teamProject", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Project = s;

      s = Util.GetElementOrAttributeValue ( "tfsServerUri", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.TFSServer = new Uri(s);
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      TeamBuildTask tbt = this.MemberwiseClone ( ) as TeamBuildTask;
      tbt.TFSServer = new Uri ( this.TFSServer.ToString ( ) );
      tbt.BuildParameters = this.BuildParameters.Clone ( );
      return tbt;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://www.codeplex.com/CcNetTeamBuildTask" ); }
    }

    #endregion
  }
}
