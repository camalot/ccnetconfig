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

/*
 * Added 10/10/2007
 * Ryan Conrad
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
  /// Jedi VCS is a source version management developped in open source licensing. It works with Firebird, 
  /// Oracle, SQL Server, or ISAM databases.
  /// </summary>
  [Plugin,MinimumVersion("1.3")]
  public class JediVcsSourceControl : SourceControl, ICCNetDocumentation {
    /// <summary>
    /// 
    /// </summary>
    public enum GetSourceMethod {
      /// <summary>
      /// 
      /// </summary>
      Synchronize = 0,
      /// <summary>
      /// 
      /// </summary>
      GetModule
    }
    private string _executable = string.Empty;
    private string _server = string.Empty;
    private string _executablePath = string.Empty;
    private string _username = string.Empty;
    private HiddenPassword _password = null;
    private int? _port = null;
    private string _projectName = string.Empty;
    private int? _projectId = null;
    private string _projectType = string.Empty;
    private string _workingDirectory = string.Empty;
    private bool? _labelOnSuccess = null;
    private GetSourceMethod? _methodGetSource = null;
    private string _branch = string.Empty;
    /// <summary>
    /// Initializes a new instance of the <see cref="JediVcsSourceControl"/> class.
    /// </summary>
    public JediVcsSourceControl ( )
      : base ( "jedivcs" ) {
      this._password = new HiddenPassword ( );
    }

    /// <summary>
    /// The branch name for the project
    /// </summary>
    /// <value>The branch.</value>
    [Description ( "The branch name for the project." ),
    Category ( "Optional" ), DefaultValue ( null )]
    public string Branch {
      get { return _branch; }
      set { _branch = value; }
    }

    /// <summary>
    /// The method to get source
    /// </summary>
    /// <value>The method.</value>
    [Description ( "The method to get source." ),
    Category ( "Optional" ), DefaultValue ( null ),
    TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ),
    Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) )]
    public GetSourceMethod? Method {
      get { return _methodGetSource; }
      set { _methodGetSource = value; }
    }

    /// <summary>
    /// Apply label if successful
    /// </summary>
    /// <value>The label on success.</value>
    [Description ( "Apply label if successful." ),
    Category ( "Optional" ), DefaultValue ( null ),
    TypeConverter(typeof(DefaultableBooleanTypeConverter)),
    Editor(typeof(DefaultableBooleanUIEditor),typeof(UITypeEditor))]
    public bool? LabelOnSuccess {
      get { return _labelOnSuccess; }
      set { _labelOnSuccess = value; }
    }

    /// <summary>
    /// The working directory
    /// </summary>
    /// <value>The working directory.</value>
    [Description ( "The working directory." ),
    Category ( "Optional" ), DefaultValue ( null )]
    public string WorkingDirectory {
      get { return _workingDirectory; }
      set { _workingDirectory = value; }
    }

    /// <summary>
    /// The project type
    /// </summary>
    /// <value>The type of the project.</value>
    [Description ( " The project type." ),
     Category ( "Optional" ), DefaultValue ( null )]
    public string ProjectType {
      get { return _projectType; }
      set { _projectType = value; }
    }

    /// <summary>
    /// The project unique ID
    /// </summary>
    /// <value>The project id.</value>
    [Description ( "The project unique ID." ),
     Category ( "Optional" ), DefaultValue ( null )]
    public int? ProjectId {
      get { return _projectId; }
      set { _projectId = value; }
    }

    /// <summary>
    /// The jedi VCS project name
    /// </summary>
    /// <value>The name of the project.</value>
    [Description ( "The jedi VCS project name." ),
     Category ( "Optional" ), DefaultValue ( null )]
    public string ProjectName {
      get { return _projectName; }
      set { _projectName = value; }
    }

    /// <summary>
    /// The server port
    /// </summary>
    /// <value>The port.</value>
    [Description ( "The server port." ),
   Category ( "Optional" ), DefaultValue ( 2106 )]
    public int? Port {
      get { return _port; }
      set { _port = value; }
    }

    /// <summary>
    /// The password of username account 
    /// </summary>
    /// <value>The password.</value>
    [Description ( "The password of username account." ),
    Category ( "Optional" ), DefaultValue ( null ),TypeConverter(typeof(PasswordTypeConverter))]
    public HiddenPassword Password {
      get { return _password; }
      set { _password = value; }
    }

    /// <summary>
    /// The username account for use Jedi VCS with CCNET
    /// </summary>
    /// <value>The username.</value>
    [Description ( "The username account for use Jedi VCS with CCNET." ),
    Category ( "Optional" ), DefaultValue ( null )]
    public string Username {
      get { return _username; }
      set { _username = value; }
    }

    /// <summary>
    /// The path for Jedi VCS client install
    /// </summary>
    /// <value>The jedi VCS path.</value>
    [Description ( "The path for Jedi VCS client install." ),
    Category ( "Optional" ), DefaultValue ( null )]
    public string JediVcsPath {
      get { return _executablePath; }
      set { _executablePath = value; }
    }

    /// <summary>
    /// The server name or ip adress string
    /// </summary>
    /// <value>The server.</value>
    [Category ( "Required" ), DisplayName ( "(Server)" ), DefaultValue ( null ),
   Description ( "The server name or ip adress string" )]
    public string Server {
      get { return _server; }
      set { _server = Util.CheckRequired ( this, "server", value ); }
    }

    /// <summary>
    /// The Jedi VCS command line executable
    /// </summary>
    /// <value>The executable.</value>
    [Description("The Jedi VCS command line executable."),
    Category("Optional"),DefaultValue(null)]
    public string Executable {
      get { return _executable; }
      set { _executable = value; }
    }


    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );

      XmlElement ele = doc.CreateElement ( "server" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Server );
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty ( this.Executable ) ) {
        ele = doc.CreateElement ( "executable" );
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.JediVcsPath ) ) {
        ele = doc.CreateElement ( "jedivcspath" );
        ele.InnerText = this.JediVcsPath;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Username ) ) {
        ele = doc.CreateElement ( "userName" );
        ele.InnerText = this.Username;
        root.AppendChild ( ele );
      }

      if ( this.Password != null && !string.IsNullOrEmpty ( this.Password.Password ) ) {
        ele = doc.CreateElement ( "password" );
        ele.InnerText = this.Password.GetPassword ( );
        root.AppendChild ( ele );
      }

      if ( this.Port.HasValue ) {
        ele = doc.CreateElement ( "port" );
        ele.InnerText = this.Port.Value.ToString ( );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.ProjectName ) ) {
        ele = doc.CreateElement ( "projectName" );
        ele.InnerText = this.ProjectName;
        root.AppendChild ( ele );
      }

      if ( this.ProjectId.HasValue ) {
        ele = doc.CreateElement ( "projectId" );
        ele.InnerText = this.ProjectId.Value.ToString ( );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.ProjectType ) ) {
        ele = doc.CreateElement ( "projectType" );
        ele.InnerText = this.ProjectType;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );
      }

      if ( this.LabelOnSuccess.HasValue ) {
        ele = doc.CreateElement ( "labelOnSuccess" );
        ele.InnerText = this.LabelOnSuccess.Value.ToString ( ).ToLower ( );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Branch ) ) {
        ele = doc.CreateElement ( "branch" );
        ele.InnerText = this.Branch;
        root.AppendChild ( ele );
      }

      if ( this.Method.HasValue ) {
        ele = doc.CreateElement ( "methodGetSource" );
        ele.InnerText = this.Method.Value.ToString ( ).ToLower ( );
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this._server = string.Empty;
      this.Branch = string.Empty;
      this.Executable = string.Empty;
      this.JediVcsPath = string.Empty;
      this.LabelOnSuccess = null;
      this.Method = null;
      this.Password = new HiddenPassword ( );
      this.Port = null;
      this.ProjectId = null;
      this.ProjectName = string.Empty;
      this.ProjectType = string.Empty;
      this.Username = string.Empty;
      this.WorkingDirectory = string.Empty;

      this.Server = Util.GetElementOrAttributeValue ( "server", element );
      this.Branch = Util.GetElementOrAttributeValue ( "branch", element );
      this.Executable = Util.GetElementOrAttributeValue ( "executable", element );
      this.JediVcsPath = Util.GetElementOrAttributeValue ( "jedivcspath", element );
      string s = Util.GetElementOrAttributeValue ( "labelOnSuccess", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.LabelOnSuccess = string.Compare ( s, bool.TrueString, true ) == 0;
      s = Util.GetElementOrAttributeValue ( "methodGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Method = Util.StringToEnum<GetSourceMethod> ( s );
      this.Password.Password = Util.GetElementOrAttributeValue ( "password", element );
      int i = 0;
      s = Util.GetElementOrAttributeValue ( "port", element );
      if ( int.TryParse ( s, out i ) )
        this.Port = i;
      s = Util.GetElementOrAttributeValue ( "projectId", element );
      if ( int.TryParse ( s, out i ) )
        this.ProjectId = i;
      this.ProjectName = Util.GetElementOrAttributeValue ( "projectName", element );
      this.ProjectType = Util.GetElementOrAttributeValue ( "projectType", element );
      this.Username = Util.GetElementOrAttributeValue ( "userName", element );
      this.WorkingDirectory = Util.GetElementOrAttributeValue ( "workingDirectory", element );
    }

    /// <summary>
    /// Creates a copy of the source control object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone ( ) {
      JediVcsSourceControl jvsc = this.MemberwiseClone ( ) as JediVcsSourceControl;
      jvsc.Password = this.Password.Clone ( );
      return jvsc;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNETCOMM/Jedi+VCS+plugin?decorator=printable" ); }
    }

    #endregion
  }
}
