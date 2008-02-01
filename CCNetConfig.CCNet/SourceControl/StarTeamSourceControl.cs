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
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// For StarTeam you must specify the executable, project, username and password. You may also specify the host, port and path. The host defaults 
  /// to 127.0.0.1. The port to 49201. The path to the empty string.
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class StarTeamSourceControl : SourceControl,ICCNetDocumentation {
    private string _executable = string.Empty;
    private string _username = string.Empty;
    private HiddenPassword _password = new HiddenPassword ();
    private string _host = string.Empty;
    private int? _port = null;
    private string _project = string.Empty;
    private string _path = string.Empty;
    private bool? _autoGetSource = null;
    private string _overrideViewWorkingDirectory = string.Empty;
    private string _overrideFolderWorkingDirectory = string.Empty;
    private string _folderRegex = string.Empty;
    private string _fileRegex = string.Empty;
    private string _fileHistoryRegex = string.Empty;
    private Timeout _timeout = new Timeout ();

    /// <summary>
    /// Initializes a new instance of the <see cref="StarTeamSourceControl"/> class.
    /// </summary>
    public StarTeamSourceControl () : base("starteam") { }

    /// <summary>
    /// The local path for the StarTeam command-line client (eg. c:\starteam\stcmd.exe).
    /// </summary>
    /// <value>The executable.</value>
    [Description ( @"The local path for the StarTeam command-line client (eg. c:\starteam\stcmd.exe)." ), DefaultValue ( null ),
  DisplayName ( "(Executable)" ), Category ( "Required" ),
 Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "StarTeam|stcmd.exe" ),
 OpenFileDialogTitle ( "Select StarTeam command-line client" )]
    public string Executable { get { return this._executable; } set { this._executable = Util.CheckRequired(this,"executable",value); } }
    /// <summary>
    /// StarTeam ID that CCNet should use.
    /// </summary>
    /// <value>The username.</value>
    [Description ( "StarTeam ID that CCNet should use." ), DefaultValue ( null ), DisplayName ( "(Username)" ), Category ( "Required" )]
    public string Username { get { return this._username; } set { this._username = Util.CheckRequired ( this, "username", value ); ; } }
    /// <summary>
    /// Password for the StarTeam user ID.
    /// </summary>
    /// <value>The password.</value>
    [Description ( "Password for the StarTeam user ID." ), DisplayName("(Password)"),
   DefaultValue ( null ), TypeConverter ( typeof ( PasswordTypeConverter ) ), Category ( "Required" )]
    public HiddenPassword Password { get { return this._password; } set { this._password = Util.CheckRequired ( this, "password", value ); ; } }
    /// <summary>
    /// The IP address or machine name of the StarTeam server.
    /// </summary>
    /// <value>The host.</value>
    [Description ( "The IP address or machine name of the StarTeam server." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Host { get { return this._host; } set { this._host = value; } }
    /// <summary>
    /// The port on the StarTeam server to connect to.
    /// </summary>
    /// <value>The port.</value>
    [Description ( "The port on the StarTeam server to connect to." ), DefaultValue ( null ), Category ( "Optional" )]
    public int? Port { get { return this._port; } set { this._port = value; } }
    /// <summary>
    /// The StarTeam project (and view) to monitor (eg. project/view)
    /// </summary>
    /// <value>The project.</value>
    [Description ( "The StarTeam project (and view) to monitor (eg. project/view)" ), DefaultValue ( null ), DisplayName ( "(Project)" ),
    Category ( "Required" )]
    public string Project { get { return this._project; } set { this._project = Util.CheckRequired ( this, "project", value ); ; } }
    /// <summary>
    /// The path to monitor.
    /// </summary>
    /// <value>The path.</value>
    [Description ( "The path to monitor." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Path { get { return this._path; } set { this._path = value; } }
    /// <summary>
    /// Instruct CCNet whether or not you want it to automatically retrieve the latest source from the repository.
    /// </summary>
    /// <value><c>true</c> or <c>false</c></value>
    [Description ( "Instruct CCNet whether or not you want it to automatically retrieve the latest source from the repository." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// If set, use the -rp option to use a different View Working Directory.
    /// </summary>
    /// <value>The overriden view working directory.</value>
    [Description ( "If set, use the -rp option to use a different View Working Directory" ), DefaultValue ( null ),
   Category ( "Optional" ),
   Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
  BrowseForFolderDescription ( "Select path to the view working directory." )]
    public string OverrideViewWorkingDirectory { get { return this._overrideViewWorkingDirectory; } set { this._overrideViewWorkingDirectory = value; } }
    /// <summary>
    /// If set, use the -fp option to use a different Folder Working Directory. Will not be used if OverrideViewWorkingDir is set.
    /// </summary>
    /// <value>The overriden view working directory.</value>
    [Description ( "If set, use the -fp option to use a different Folder Working Directory. Will not be used if OverrideViewWorkingDir is set." ),
  DefaultValue ( null ), Category ( "Optional" ),
   Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
  BrowseForFolderDescription ( "Select path to the folder working directory." )]
    public string OverrideFolderWorkingDirectory { get { return this._overrideFolderWorkingDirectory; } set { this._overrideFolderWorkingDirectory = value; } }
    /// <summary>
    /// Allows you to use your own RegEx to parse StarTeam's folder output.
    /// </summary>
    /// <value>The expression.</value>
    [Description ( "Allows you to use your own RegEx to parse StarTeam's folder output." ), DefaultValue ( null ), Category ( "Optional" )]
    public string FolderRegularExpression { get { return this._folderRegex; } set { this._folderRegex = value; } }
    /// <summary>
    /// Allows you to use your own RegEx to parse StarTeam's file output.
    /// </summary>
    /// <value>The expression.</value>
    [Description ( "Allows you to use your own RegEx to parse StarTeam's file output." ), DefaultValue ( null ), Category ( "Optional" )]
    public string FileRegularExpression { get { return this._fileRegex; } set { this._fileRegex = value; } }
    /// <summary>
    /// Allows you to use your own RegEx to parse StarTeam's file history.
    /// </summary>
    /// <value>The expression.</value>
    [Description ( "Allows you to use your own RegEx to parse StarTeam's file history." ), DefaultValue ( null ), Category ( "Optional" )]
    public string FileHistoryRegularExpression { get { return this._fileHistoryRegex; } set { this._fileHistoryRegex = value; } }
    /// <summary>
    /// Sets the timeout period for the source control operation.
    /// </summary>
    /// <value>The timeout.</value>
    /// <seealso cref="CCNetConfig.Core.Timeout"/>
    [Description ( "Sets the timeout period for the source control operation." ), DefaultValue ( null ), Category ( "Optional" )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    ///  Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      StarTeamSourceControl ssc = this.MemberwiseClone () as StarTeamSourceControl;
      ssc.Password = this.Password.Clone(  );
      ssc.Timeout = this.Timeout.Clone ();
      return ssc;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      //root.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name ) );

      XmlElement ele = doc.CreateElement ( "executable" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Executable );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "username" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Username );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "password" );
      ele.InnerText = Util.CheckRequired ( this,ele.Name, this.Password.GetPassword());
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "project" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Project );
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty ( this.Host ) ) {
        ele = doc.CreateElement ( "host" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Host );
        root.AppendChild ( ele );
      }

      if ( this.Port.HasValue ) {
        ele = doc.CreateElement ( "port" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Port.Value.ToString() );
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.AutoGetSource.Value.ToString () );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Path ) ) {
        ele = doc.CreateElement ( "path" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Path );
        root.AppendChild ( ele );
      }
      // workitem: 14757
      if ( !string.IsNullOrEmpty ( this.OverrideViewWorkingDirectory ) ) {
        ele = doc.CreateElement ( "overrideViewWorkingDir" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.OverrideViewWorkingDirectory );
        root.AppendChild ( ele );
      }
      // workitem: 14757
      if ( !string.IsNullOrEmpty ( this.OverrideFolderWorkingDirectory ) ) {
        ele = doc.CreateElement ( "overrideFolderWorkingDir" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.OverrideFolderWorkingDirectory );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.FolderRegularExpression ) ) {
        ele = doc.CreateElement ( "folderRegEx" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.FolderRegularExpression );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.FileRegularExpression ) ) {
        ele = doc.CreateElement ( "fileRegEx" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.FileRegularExpression );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.FileHistoryRegularExpression ) ) {
        ele = doc.CreateElement ( "fileHistoryRegEx" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.FileHistoryRegularExpression );
        root.AppendChild ( ele );
      }

      if ( this.Timeout != null ) {
        ele = this.Timeout.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele , true) );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      this._executable = string.Empty;
      this._username = string.Empty;
      this._password = new HiddenPassword ();
      this._project = string.Empty;
      this.Host = string.Empty;
      this.Port = null;
      this.Path = string.Empty;
      this.AutoGetSource = null;
      this.OverrideFolderWorkingDirectory = string.Empty;
      this.OverrideViewWorkingDirectory = string.Empty;
      this.FolderRegularExpression = string.Empty;
      this.FileHistoryRegularExpression = string.Empty;
      this.FileRegularExpression = string.Empty;
      this.Timeout = new Timeout ();

      string s = Util.GetElementOrAttributeValue ( "executable", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue ( "username", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Username = s;

      s = Util.GetElementOrAttributeValue ( "password", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Password.Password = s;

      s = Util.GetElementOrAttributeValue ( "project", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Project = s;

      s = Util.GetElementOrAttributeValue ( "host", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Host = s;

      s = Util.GetElementOrAttributeValue ( "port", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int port = 0;
        if ( int.TryParse(s,out port) )
          this.Port = port;
      }

      s = Util.GetElementOrAttributeValue ( "path", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Path = s;

      s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      // workitem: 14757
      s = Util.GetElementOrAttributeValue ( "overrideFolderWorkingDir", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.OverrideFolderWorkingDirectory = s;
      // workitem: 14757
      s = Util.GetElementOrAttributeValue ( "overrideViewWorkingDir", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.OverrideViewWorkingDirectory = s;

      s = Util.GetElementOrAttributeValue ( "folderRegEx", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.FolderRegularExpression = s;

      s = Util.GetElementOrAttributeValue ( "fileRegEx", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.FileRegularExpression = s;

      s = Util.GetElementOrAttributeValue ( "fileHistoryRegEx", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.FileHistoryRegularExpression = s;

      XmlElement ele = (XmlElement)element.SelectSingleNode ( "timeout" );
      if ( ele != null )
        this.Timeout.Deserialize(ele);
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/StarTeam+Source+Control+Block?decorator=printable" ); }
    }

    #endregion
  }
}
