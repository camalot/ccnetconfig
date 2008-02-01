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
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Serialization;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Telelogic's Synergy SCM product suite, specifically CM Synergy as well as ChangeSynergy. Detection of modifications is 
  /// entirely task based rather than object based, which may present problems for pre-6.3 lifecycles. Successful integration may 
  /// be published through shared manual task folders and/or baselining.
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class TelelogicSynergySourceControl : SourceControl, ICCNetDocumentation {

    private TelelogicSynergySourceControl.ConnectionSection _connection = null;
    private TelelogicSynergySourceControl.ProjectSection _project = null;
    private TelelogicSynergySourceControl.ChangeSynergySection _changeSynergy = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="TelelogicSynergySourceControl"/> class.
    /// </summary>
    public TelelogicSynergySourceControl ()
      : base ( "synergy" ) {
      _connection = new ConnectionSection ();
      _project = new ProjectSection ();
      _changeSynergy = new ChangeSynergySection ();
    }

    /// <summary>
    /// Gets the connection.
    /// </summary>
    /// <value>The connection.</value>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public ConnectionSection Connection { get { return this._connection; } }

    /// <summary>
    /// Gets the project.
    /// </summary>
    /// <value>The project.</value>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public ProjectSection Project { get { return this._project; } set { this._project = value; } }

    /// <summary>
    /// Gets the change synergy.
    /// </summary>
    /// <value>The change synergy.</value>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public ChangeSynergySection ChangeSynergy {
      get { return this._changeSynergy; }
      set { this._changeSynergy = value; }
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      TelelogicSynergySourceControl tsc = this.MemberwiseClone () as TelelogicSynergySourceControl;
      tsc.ChangeSynergy = this.ChangeSynergy.Clone ();
      tsc.Project = this.Project.Clone ();
      return tsc;
    }


    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      //root.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name ) );
      XmlElement connectionElement = this._connection.Serialize ();
      if ( connectionElement != null )
        root.AppendChild ( doc.ImportNode ( connectionElement, true ) );
      else
        root.AppendChild ( doc.CreateElement ( "connection" ) );
      XmlElement projectElement = this._project.Serialize ();
      if ( projectElement != null )
        root.AppendChild ( doc.ImportNode ( projectElement, true ) );

      XmlElement csElement = this._changeSynergy.Serialize ();
      if ( csElement != null )
        root.AppendChild ( doc.ImportNode ( csElement, true ) );
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      XmlElement ele = (XmlElement)element.SelectSingleNode ( "connection" );
      if ( ele != null )
        _connection.Deserialize ( ele );

      ele = (XmlElement)element.SelectSingleNode ( "project" );
      if ( ele != null )
        _project.Deserialize ( ele );

      ele = (XmlElement)element.SelectSingleNode ( "changeSynergy" );
      if ( ele != null )
        _changeSynergy.Deserialize ( ele );
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Telelogic+Synergy?decorator=printable" ); }
    }

    #endregion

    #region ConnectionSection
    /// <summary>
    /// Contains the Connection information for the Synergy Source Control
    /// </summary>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public sealed class ConnectionSection : ISerialize, ICCNetObject, ICloneable {
      private string _host = string.Empty;
      private string _database = string.Empty;
      private string _username = string.Empty;
      private char _delimiter;
      private string _executable = string.Empty;
      private HiddenPassword _password = new HiddenPassword ();
      private string _role = string.Empty;
      private string _homeDirectory = string.Empty;
      private string _workingDirectory = string.Empty;
      private string _clientDatabaseDirectory = string.Empty;
      private bool? _polling = null;
      private int? _timeout = null;

      private string _typeName = string.Empty;
      /// <summary>
      /// Initializes a new instance of the <see cref="Connection"/> class.
      /// </summary>
      public ConnectionSection () {
        this._typeName = "connection";
      }

      /// <summary>
      /// The hostname of the Synergy server.
      /// </summary>
      /// <value>The host.</value>
      [Description ( "The hostname of the Synergy server." ), DefaultValue ( null ), Category ( "Optional" )]
      public string Host { get { return this._host; } set { this._host = value; } }
      /// <summary>
      /// The physical path to the Informix database for the Synergy database.
      /// </summary>
      /// <value>The database.</value>
      [Description ( "The physical path to the Informix database for the Synergy database." ), DefaultValue ( null ), Category ( "Optional" )]
      public string Database { get { return this._database; } set { this._database = value; } }
      /// <summary>
      /// The username for the Synergy session.
      /// </summary>
      /// <value>The username.</value>
      [Description ( "The username for the Synergy session." ), DefaultValue ( null ), Category ( "Optional" )]
      public string Username { get { return this._username; } set { this._username = value; } }
      /// <summary>
      /// The Synergy password for the associated username.
      /// </summary>
      /// <value>The password.</value>
      [Description ( "The Synergy password for the associated username." ), DefaultValue ( null ),
      Category ( "Optional" ), TypeConverter ( typeof ( PasswordTypeConverter ) )]
      public HiddenPassword Password { get { return this._password; } set { this._password = value; } }
      /// <summary>
      /// The executable filename/path for the CM Synergy command line interface.
      /// </summary>
      /// <value>The executable.</value>
      [Description ( "The executable filename/path for the CM Synergy command line interface." ), DefaultValue ( null ),
     Category ( "Optional" ),
Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "CM Synergy Client|*.exe" ),
OpenFileDialogTitle ( "Select CM Synergy command-line client" )]
      public string Executable { get { return this._executable; } set { this._executable = value; } }
      /// <summary>
      /// The role to use for the Synergy session.
      /// </summary>
      /// <value>The role.</value>
      [Description ( "The role to use for the Synergy session." ), DefaultValue ( null ), Category ( "Optional" )]
      public string Role { get { return this._role; } set { this._role = value; } }
      /// <summary>
      /// The full physical path of the home directory for the associated username on the client machine.
      /// </summary>
      /// <value>The home directory.</value>
      [Description ( "The full physical path of the home directory for the associated username on the client machine." ),
     DefaultValue ( null ), Category ( "Optional" ),
  Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
 BrowseForFolderDescription ( "Select path to the home directory." )]
      public string HomeDirectory { get { return this._homeDirectory; } set { this._homeDirectory = value; } }
      /// <summary>
      /// The directory to execute all CM Synergy commands from.
      /// </summary>
      /// <value>The working directory.</value>
      [Description ( "The directory to execute all CM Synergy commands from." ), DefaultValue ( null ),
     Category ( "Optional" ),
  Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
 BrowseForFolderDescription ( "Select path to the working directory." )]
      public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
      /// <summary>
      /// Path for the remote client session to copy database information to.
      /// </summary>
      /// <value>The client database directory.</value>
      [Description ( "Path for the remote client session to copy database information to." ), DefaultValue ( null ),
     Category ( "Optional" ),
  Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
 BrowseForFolderDescription ( "Select path to the client database directory." )]
      public string ClientDatabaseDirectory { get { return this._clientDatabaseDirectory; } set { this._clientDatabaseDirectory = value; } }
      /// <summary>
      /// The configured database delimiter for object and project specifications.
      /// </summary>
      /// <value>The delimiter.</value>
      [Description ( "The configured database delimiter for object and project specifications." ), DefaultValue ( null ), Category ( "Optional" )]
      public char Delimiter { get { return this._delimiter; } set { this._delimiter = value; } }
      /// <summary>
      /// If enabled, queues commands while the server is offline.
      /// </summary>
      /// <value></value>
      /// <remarks>The polling feature is useful if your Synergy installation routinely goes offline (i.e., "protected mode"). 
      /// Long runing builds may inadventently conflict with the routine downtime schedules. For example, polling allows your build to 
      /// queue CM Synergy commands until the nightly backup completes.</remarks>
      [Description ( "If enabled, queues commands while the server is offline." ), DefaultValue ( null ),
      Category ( "Optional" ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
      public bool? Polling { get { return this._polling; } set { this._polling = value; } }
      /// <summary>
      /// Timeout in seconds for all Synergy commands.
      /// </summary>
      /// <value>The timeout.</value>
      [Description ( "Timeout in seconds for all Synergy commands." ), DefaultValue ( null ), Category ( "Optional" )]
      public int? Timeout { get { return this._timeout; } set { this._timeout = value; } }

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public System.Xml.XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( this._typeName );
        root.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name ) );

        XmlElement ele = doc.CreateElement ( "host" );
        if ( !string.IsNullOrEmpty ( this.Host ) )
          ele.InnerText = this.Host;
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "database" );
        if ( !string.IsNullOrEmpty ( this.Database ) )
          ele.InnerText = this.Database;
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "username" );
        if ( !string.IsNullOrEmpty ( this.Username ) )
          ele.InnerText = this.Username;
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "delimiter" );
        if ( this.Delimiter != 0 )
          ele.InnerText = this.Delimiter.ToString ();
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "executable" );
        if ( !string.IsNullOrEmpty ( this.Executable ) )
          ele.InnerText = this.Executable;
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "password" );
        if ( !string.IsNullOrEmpty ( this.Password.GetPassword () ) )
          ele.InnerText = this.Password.GetPassword ();
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "role" );
        if ( !string.IsNullOrEmpty ( this.Role ) )
          ele.InnerText = this.Role;
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "homeDirectory" );
        if ( !string.IsNullOrEmpty ( this.HomeDirectory ) )
          ele.InnerText = this.HomeDirectory;
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "workingDirectory" );
        if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) )
          ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "clientDatabaseDirectory" );
        if ( !string.IsNullOrEmpty ( this.ClientDatabaseDirectory ) )
          ele.InnerText = this.ClientDatabaseDirectory;
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "polling" );
        if ( this.Polling.HasValue )
          ele.InnerText = this.Polling.Value.ToString ();
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "timeout" );
        if ( this.Timeout.HasValue )
          ele.InnerText = this.Timeout.Value.ToString ();
        root.AppendChild ( ele );

        return doc.DocumentElement;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( System.Xml.XmlElement element ) {
        this.ClientDatabaseDirectory = string.Empty;
        this.Database = string.Empty;
        this.Delimiter = (char)0;
        this.Executable = string.Empty;
        this.HomeDirectory = string.Empty;
        this.Host = string.Empty;
        this.Password = new HiddenPassword ();
        this.Polling = null;
        this.Role = string.Empty;
        this.Timeout = null;
        this.Username = string.Empty;
        this.WorkingDirectory = string.Empty;

        this.ClientDatabaseDirectory = Util.GetElementOrAttributeValue ( "clientDatabaseDirectory", element );
        this.Database = Util.GetElementOrAttributeValue ( "database", element );
        string s = Util.GetElementOrAttributeValue ( "delimiter", element );
        if ( !string.IsNullOrEmpty ( s ) ) {
          char c = (char)0;
          char.TryParse ( s, out c );
          this.Delimiter = c;
        }
        this.Executable = Util.GetElementOrAttributeValue ( "executable", element );
        this.HomeDirectory = Util.GetElementOrAttributeValue ( "homeDirectory", element );
        this.Host = Util.GetElementOrAttributeValue ( "host", element );
        this.Password.Password = Util.GetElementOrAttributeValue ( "password", element );
        s = Util.GetElementOrAttributeValue ( "polling", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Polling = string.Compare ( s, bool.TrueString, true ) == 0;
        this.Role = Util.GetElementOrAttributeValue ( "role", element );
        s = Util.GetElementOrAttributeValue ( "timeout", element );
        if ( !string.IsNullOrEmpty ( s ) ) {
          int t = 0;
          if ( int.TryParse ( s, out t ) )
            this.Timeout = t;
        }

        this.Username = Util.GetElementOrAttributeValue ( "username", element );
        this.WorkingDirectory = Util.GetElementOrAttributeValue ( "workingDirectory", element );
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return string.Empty;
      }

      #region ICloneable Members
      /// <summary>
      /// Creates a copy of this object.
      /// </summary>
      /// <returns></returns>
      public ConnectionSection Clone () {
        ConnectionSection cs = this.MemberwiseClone () as ConnectionSection;
        cs.Password = this.Password.Clone ();
        return cs;
      }

      object ICloneable.Clone () {
        return this.Clone ();
      }

      #endregion
    }
    #endregion

    #region ProjectSection
    /// <summary>
    /// Contains the Project information for the Synergy Source Control
    /// </summary>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public sealed class ProjectSection : ISerialize, ICCNetObject, ICloneable {
      private string _typeName = string.Empty;
      private string _release = string.Empty;
      private string _projectSpecification = string.Empty;
      private int? _taskFolder = null;
      private bool? _baseLine = null;
      private string _purpose = string.Empty;
      private bool? _template = null;

      /// <summary>
      /// Initializes a new instance of the <see cref="ProjectSection"/> class.
      /// </summary>
      public ProjectSection () {
        this._typeName = "project";
      }
      /// <summary>
      /// The component + version specification.
      /// </summary>
      /// <value>The release.</value>
      [Description ( "The component + version specification." ), DefaultValue ( null ), Category ( "Required" ), DisplayName ( "(Release)" )]
      public string Release { get { return this._release; } set { this._release = Util.CheckRequired ( this, "release", value ); } }
      /// <summary>
      /// The Synergy project specification for the integration project.
      /// </summary>
      /// <value>The project specification.</value>
      [Description ( "The Synergy project specification for the integration project." ), DefaultValue ( null ),
      Category ( "Required" ), DisplayName ( "(ProjectSpecification)" )]
      public string ProjectSpecification {
        get { return this._projectSpecification; }
        set { this._projectSpecification = Util.CheckRequired ( this, "projectSpecification", value ); }
      }
      /// <summary>
      /// The folder specification for the shared folder which will be used to "manually" add successfully integrated tasks added to.
      /// </summary>
      /// <value>The task folder.</value>
      [Description ( "The folder specification for the shared folder which will be used to \"manually\" add successfully integrated tasks added to." ),
     DefaultValue ( null ), Category ( "Optional" ),
  Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
 BrowseForFolderDescription ( "Select path to the task directory." )]
      public int? TaskFolder { get { return this._taskFolder; } set { this._taskFolder = value; } }
      /// <summary>
      /// Flag to creates a new baseline of the project configuration after a successful integration.
      /// </summary>
      /// <value>The base line.</value>
      [Description ( "Flag to creates a new baseline of the project configuration after a successful integration." ),
      DefaultValue ( null ), Category ( "Optional" ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
      public bool? BaseLine { get { return this._baseLine; } set { this._baseLine = value; } }
      /// <summary>
      /// The Synergy purpose specification for baselines created by CruiseControl.NET.
      /// </summary>
      /// <value>The purpose.</value>
      [Description ( "The Synergy purpose specification for baselines created by CruiseControl.NET." ),
      DefaultValue ( null ), Category ( "Optional" )]
      public string Purpose { get { return this._purpose; } set { this._purpose = value; } }
      /// <summary>
      /// Flag to reset the reconfigure properties for this project and all subprojects to use the reconfigure template.
      /// </summary>
      /// <value>The template.</value>
      [Description ( "Flag to reset the reconfigure properties for this project and all subprojects to use the reconfigure template." ),
      DefaultValue ( null ), Category ( "Optional" ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
      public bool? Template { get { return this._template; } set { this._template = value; } }

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( this._typeName );
        root.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name ) );

        XmlElement ele = doc.CreateElement ( "projectSpecification" );
        ele.InnerText = Util.CheckRequired ( this, "projectSpecification", this.ProjectSpecification );
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "release" );
        ele.InnerText = Util.CheckRequired ( this, "release", this.Release );
        root.AppendChild ( ele );

        if ( this.BaseLine.HasValue ) {
          ele = doc.CreateElement ( "baseline" );
          ele.InnerText = this.BaseLine.Value.ToString ();
          root.AppendChild ( ele );
        }

        if ( !string.IsNullOrEmpty ( this.Purpose ) ) {
          ele = doc.CreateElement ( "purpose" );
          ele.InnerText = this.Purpose;
          root.AppendChild ( ele );
        }

        if ( this.TaskFolder.HasValue ) {
          ele = doc.CreateElement ( "taskFolder" );
          ele.InnerText = this.TaskFolder.Value.ToString ();
          root.AppendChild ( ele );
        }

        if ( this.Template.HasValue ) {
          ele = doc.CreateElement ( "template" );
          ele.InnerText = this.Template.Value.ToString ();
          root.AppendChild ( ele );
        }
        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this.BaseLine = null;
        this._projectSpecification = string.Empty;
        this.Purpose = string.Empty;
        this._release = string.Empty;
        this.TaskFolder = null;
        this.Template = null;
        string s = Util.GetElementOrAttributeValue ( "baseline", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.BaseLine = string.Compare ( s, bool.TrueString, true ) == 0;
        this.ProjectSpecification = Util.GetElementOrAttributeValue ( "projectSpecification", element );
        this.Purpose = Util.GetElementOrAttributeValue ( "purpose", element );
        this.Release = Util.GetElementOrAttributeValue ( "release", element );
        s = Util.GetElementOrAttributeValue ( "taskFolder", element );
        if ( !string.IsNullOrEmpty ( s ) ) {
          int i = 0;
          if ( int.TryParse ( s, out i ) )
            this.TaskFolder = i;
        }

        s = Util.GetElementOrAttributeValue ( "template", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Template = string.Compare ( s, bool.TrueString, true ) == 0;
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return string.Empty;
      }

      #region ICloneable Members
      /// <summary>
      /// Creates a copy of this object.
      /// </summary>
      /// <returns></returns>
      public ProjectSection Clone () {
        ProjectSection ps = this.MemberwiseClone () as ProjectSection;
        return ps;
      }
      object ICloneable.Clone () {
        return this.Clone ();
      }

      #endregion
    }
    #endregion

    #region ChangeSynergySection
    /// <summary>
    /// Contains the Change Synergy information for the Synergy Source Control
    /// </summary>
    /// <remarks>
    /// Be careful about specifying a ChangeSynergy username and password. If you do not specify these optional values, the end-user will be 
    /// prompted by ChangeSynergy to enter valid credentials. If an anonymous access account is used, specify a user that has only read-only 
    /// permissions within ChangeSynergy. This will prevent someone from modifying objects, such as tasks, through ChangeSynergy. If you specify 
    /// an impersonation account with write permissions, a malicious user could bypass auditing in ChangeSynergy.
    /// </remarks>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public sealed class ChangeSynergySection : ISerialize, ICCNetObject, ICloneable {
      private string _typeName = string.Empty;
      private Uri _url = null;
      private string _username = string.Empty;
      private HiddenPassword _password = new HiddenPassword ();
      private string _role = string.Empty;

      /// <summary>
      /// Initializes a new instance of the <see cref="ChangeSynergySection"/> class.
      /// </summary>
      public ChangeSynergySection () {
        this._typeName = "changeSynergy";
      }
      /// <summary>
      /// The base HTTP URL for your ChangeSynergy installation.
      /// </summary>
      /// <value>The URL.</value>
      [Description ( "The base HTTP URL for your ChangeSynergy installation" ), DefaultValue ( null ), DisplayName ( "(Url)" ), Category ( "Required" )]
      public Uri Url { get { return this._url; } set { this._url = Util.CheckRequired ( this, "url", value ); } }
      /// <summary>
      /// The <b>optional</b> username for ChangeSynergy anonymous access.
      /// </summary>
      /// <value>The username.</value>
      [Description ( "The optional username for ChangeSynergy anonymous access." ), DefaultValue ( null ), Category ( "Optional" )]
      public string Username { get { return this._username; } set { this._username = value; } }
      /// <summary>
      /// The <b>optional</b> password for ChangeSynergy anonymous access.
      /// </summary>
      /// <value>The password.</value>
      [Description ( "The optional password for ChangeSynergy anonymous access." ), DefaultValue ( null ), Category ( "Optional" ),
      TypeConverter ( typeof ( PasswordTypeConverter ) )]
      public HiddenPassword Password { get { return this._password; } set { this._password = value; } }
      /// <summary>
      /// The role to use for the Synergy session.
      /// </summary>
      /// <value>The role.</value>
      [Description ( "The role to use for the Synergy session." ), DefaultValue ( null ), Category ( "Optional" )]
      public string Role { get { return this._role; } set { this._role = value; } }
      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( this._typeName );
        root.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name ) );
        XmlElement ele = doc.CreateElement ( "url" );
        ele.InnerText = Util.CheckRequired ( this, "url", this.Url ).ToString ();
        root.AppendChild ( ele );

        if ( !string.IsNullOrEmpty ( this.Username ) ) {
          ele = doc.CreateElement ( "username" );
          ele.InnerText = Util.CheckRequired ( this, "username", this.Username );
          root.AppendChild ( ele );
        }

        if ( !string.IsNullOrEmpty ( this.Password.GetPassword () ) ) {
          ele = doc.CreateElement ( "password" );
          ele.InnerText = Util.CheckRequired ( this, "password", this.Password.GetPassword () );
          root.AppendChild ( ele );
        }

        if ( !string.IsNullOrEmpty ( this.Role ) ) {
          ele = doc.CreateElement ( "role" );
          ele.InnerText = Util.CheckRequired ( this, "role", this.Role );
          root.AppendChild ( ele );
        }
        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this._url = null;
        this.Username = string.Empty;
        this.Password = new HiddenPassword ();
        this.Role = string.Empty;

        this.Url = Util.CheckRequired ( this, "url", new Uri ( Util.GetElementOrAttributeValue ( "url", element ) ) );
        this.Username = Util.GetElementOrAttributeValue ( "username", element );
        this.Password.Password = Util.GetElementOrAttributeValue ( "password", element );
        this.Role = Util.GetElementOrAttributeValue ( "role", element );
      }

      #endregion
      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return string.Empty;
      }

      #region ICloneable Members

      object ICloneable.Clone () {
        return this.Clone ();
      }

      /// <summary>
      /// Creates a copy of this object
      /// </summary>
      /// <returns></returns>
      public ChangeSynergySection Clone () {
        ChangeSynergySection css = this.MemberwiseClone () as ChangeSynergySection;
        css.Password = this.Password.Clone ();
        if ( this.Url != null )
          css.Url = new Uri ( this.Url.ToString () );
        return css;
      }

      #endregion
    }
    #endregion
  }
}
