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
using System.Drawing.Design;
using CCNetConfig.Core.Components;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// AccuRev Source Control Block
  /// </summary>
  [ MinimumVersion( "1.3" ) ]
  public class AccuRevSourceControl : SourceControl, ICCNetDocumentation {
    private bool? _autoGetSource = null;
    private string _homeDir = string.Empty;
    private string _executable = string.Empty;
    private bool? _labelOnSuccess = null;
    private bool? _login = null;
    private HiddenPassword _password = null;
    private string _principal = string.Empty;
    private Timeout _timeout = null;
    private string _workspace = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccuRevSourceControl"/> class.
    /// </summary>
    public AccuRevSourceControl ()
      : base ( "accurev" ) {

    }

    /// <summary>
    /// Specifies whether the current version of the source should be retrieved from AccuRev.
    /// </summary>
    /// <value>The auto get source.</value>
    [Description ( "Specifies whether the current version of the source should be retrieved from AccuRev." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }

    /// <summary>
    /// Specifies the path to the AccuRev command line tool. You should only have to include this 
    /// element if the tool isn't in your path. By default, the AccuRev client installation process 
    /// names it accurev.exe and puts it in C:\Program Files\AccuRev\bin.
    /// </summary>
    /// <value>The executable.</value>
    [Description ( "Specifies the path to the AccuRev command line tool. You should only have to include " +
      "this element if the tool isn't in your path. By default, the AccuRev client installation process " +
      "names it accurev.exe and puts it in C:\\Program Files\\AccuRev\\bin." ), 
    DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), 
    FileTypeFilter ( "AccuRev|accurev.exe|All Files|*.*" ),
    OpenFileDialogTitle ( "Select AccuRev Executable" )]
    public string Executable { get { return this._executable; } set { this._executable = value; } }
    /// <summary>
    /// Specifies the location of the AccuRev home directory. The pathname can be either absolute or
    /// relative to the project artifact directory. If not specified, AccuRev will follow its rules
    /// for determining the location. The home directory itself is always named ".accurev".
    /// </summary>
    /// <value>The home directory.</value>
    [Description ( "Specifies the location of the AccuRev home directory. The pathname can be either " +
      "absolute or relative to the project artifact directory. If not specified, AccuRev will follow its " +
      "rules for determining the location. The home directory itself is always named \".accurev\"." ),
    DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
   BrowseForFolderDescription ( "Select the location of the AccuRev home directory." )]
    public string HomeDirectory { get { return this._homeDir; } set { this._homeDir = value; } }
    /// <summary>
    /// Specifies whether or not CCNet should create an AccuRev snapshot when the build is successful. 
    /// If set to true, CCNet will create a snapshot of the workspace's basis stream as of the starting 
    /// time of the build, naming it according to the build label.
    /// </summary>
    /// <value>The label on success.</value>
    [Description ( "Specifies whether or not CCNet should create an AccuRev snapshot when the build is " +
      "successful. If set to true, CCNet will create a snapshot of the workspace's basis stream as of the " + 
      "starting time of the build, naming it according to the build label." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? LabelOnSuccess { get { return this._labelOnSuccess; } set { this._labelOnSuccess = value; } }
    /// <summary>
    /// Specifies whether or not CCNet should log in to AccuRev using the specified principal 
    /// and password. If set to true, the principal and password elements are required, and CCNet 
    /// will use them to log in to AccuRev before executing any AccuRev commands.
    /// </summary>
    /// <value>if login.</value>
    [Description ( "Specifies whether or not CCNet should log in to AccuRev using the specified principal and password. If set to true, the principal and password elements are required, and CCNet will use them to log in to AccuRev before executing any AccuRev commands." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? Login { get { return this._login; } set { this._login = value; } }
    /// <summary>
    /// Specifies the password for the AccuRev "principal" (userid).
    /// </summary>
    /// <value>The password.</value>
    [Description ( "Specifies the password for the AccuRev \"principal\" (userid).\nIf Login = true, this " +
      "is required." ),
    DefaultValue ( null ), Category ( "Optional" ),
    TypeConverter ( typeof ( PasswordTypeConverter ) )]
    public HiddenPassword Password { get { return this._password; } set { this._password = value; } }
    /// <summary>
    /// Specifies the AccuRev "principal" (userid) to run under. If not specified, AccuRev will follow its 
    /// rules for determining the principal.
    /// </summary>
    /// <value>The principal.</value>
    [Description ( "Specifies the AccuRev \"principal\" (userid) to run under. If not specified, AccuRev " +
     "will follow its rules for determining the principal.\nIf Login = true, this is required." ),
   DefaultValue ( null ), Category ( "Optional" )]
    public string Principal { get { return this._principal; } set { this._principal = value; } }
    /// <summary>
    /// Sets the timeout period for the source control operation.
    /// </summary>
    /// <value>The timeout.</value>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) ),
    Description ( "Sets the timeout period for the source control operation." ), DefaultValue ( null ), 
    Category ( "Optional" )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }
    /// <summary>
    /// Specifies the location on disk of the AccuRev workspace that CCNet monitors for changes. 
    /// The pathname can be either absolute or relative to the project working directory, and must 
    /// identify the top-level directory of the workspace. Note that this is not the same as the 
    /// workspace name - AccuRev will determine the workspace name from the disk pathname.
    /// </summary>
    /// <value>The workspace.</value>
    [Description ( "Specifies the location on disk of the AccuRev workspace that CCNet monitors for changes. " +
    "The pathname can be either absolute or relative to the project working directory, and must identify " +
    "the top-level directory of the workspace. Note that this is not the same as the workspace name - " + 
     "AccuRev will determine the workspace name from the disk pathname." ),
    DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
   BrowseForFolderDescription ( "Select the location on disk of the AccuRev workspace." )]
    public string Workspace { get { return this._workspace; } set { this._workspace = value; } }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));
      //ele.InnerText = Util.CheckRequired ( this, "server", this.Server );

      XmlElement ele = doc.CreateElement ( "autoGetSource" );
      if ( this.AutoGetSource.HasValue ) {
        ele.InnerText = this.AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }

      ele = doc.CreateElement ( "executable" );
      if ( !string.IsNullOrEmpty ( this.Executable ) ) {
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      ele = doc.CreateElement ( "homeDir" );
      if ( !string.IsNullOrEmpty ( this.HomeDirectory ) ) {
        ele.InnerText = this.HomeDirectory;
        root.AppendChild ( ele );
      }

      ele = doc.CreateElement ( "labelOnSuccess" );
      if ( this.LabelOnSuccess.HasValue ) {
        ele.InnerText = this.LabelOnSuccess.Value.ToString(  );
        root.AppendChild ( ele );
      }

      ele = doc.CreateElement ( "login" );
      if ( this.Login.HasValue ) {
        ele.InnerText = this.Login.Value.ToString ();
        root.AppendChild ( ele );
      }

      // if login = true then this is required.
      ele = doc.CreateElement ( "principal" );
      if ( this.Login.HasValue && this.Login.Value ) {
        ele.InnerText = Util.CheckRequired ( this, "principal", this.Principal );
        root.AppendChild ( ele );
      } else {
        if ( !string.IsNullOrEmpty ( this.Principal ) ) {
          ele.InnerText = this.Principal;
          root.AppendChild ( ele );
        }
      }

      // if login = true then this is required.
      ele = doc.CreateElement ( "password" );
      if ( this.Login.HasValue && this.Login.Value ) {
        ele.InnerText = Util.CheckRequired ( this, "password", this.Password.GetPassword(  ) );
        root.AppendChild ( ele );
      } else {
        if ( this.Password != null && !string.IsNullOrEmpty ( this.Password.Password ) ) {
          ele.InnerText = this.Password.GetPassword(  );
          root.AppendChild ( ele );
        }
      }

      if ( this.Timeout != null ) {
        ele = this.Timeout.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }

      ele = doc.CreateElement ( "workspace" );
      if ( !string.IsNullOrEmpty ( this.Workspace ) ) {
        ele.InnerText = this.Workspace;
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.AutoGetSource = null;
      this.Executable = string.Empty;
      this.HomeDirectory = string.Empty;
      this.LabelOnSuccess = null;
      this.Login = null;
      this.Password = new HiddenPassword ();
      this.Principal = string.Empty;
      this.Timeout = new Timeout ();
      this.Workspace = string.Empty;

      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

      string s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "executable", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue ( "homeDir", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.HomeDirectory = s;

      s = Util.GetElementOrAttributeValue ( "labelOnSuccess", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.LabelOnSuccess = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "login", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Login = string.Compare ( s, bool.TrueString, true ) == 0;

      this.Password.Password = Util.GetElementOrAttributeValue ( "password", element );

      s = Util.GetElementOrAttributeValue ( "principal", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Principal = s;

      XmlElement ele = (XmlElement)element.SelectSingleNode ( "timeout" );
      if ( ele != null )
        this.Timeout.Deserialize ( ele );

      s = Util.GetElementOrAttributeValue ( "workspace", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Workspace = s;
    }

    /// <summary>
    /// Creates a copy of the source control object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      AccuRevSourceControl arsc = this.MemberwiseClone () as AccuRevSourceControl;
      this.Timeout = arsc.Timeout.Clone ();
      this.Password = arsc.Password.Clone ();
      return arsc;
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [ Browsable( false ) ]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/AccuRev+Source+Control+Block?decorator=printable" ); }
    }

    #endregion
  }
}
