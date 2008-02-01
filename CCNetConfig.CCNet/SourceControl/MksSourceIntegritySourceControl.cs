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
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// For MKS Source Integrity you must specify the executable, user, password, hostname, sandboxroot and sandboxfile. You may also specify the port.
  /// </summary>
  /// <example>
  /// <code>
  /// <![CDATA[
  /// <sourceControl type=""mks"">
  /// 	<executable>C:\MKS\bin\si.exe</executable>
  /// 	<user>CCNetUser</user>
  /// 	<password>CCNetPassword</password>
  /// 	<hostname>hostname</hostname>
  /// 	<port>8722</port>
  /// 	<sandboxroot>C:\MyProject</sandboxroot>
  /// 	<sandboxfile>myproject.pj</sandboxfile>
  ///   <autoGetSource>true</autoGetSource>
  /// </sourceControl>
  /// ]]>
  /// </code>
  /// </example>
  [ MinimumVersion( "1.0" ) ]
  public class MksSourceIntegritySourceControl : SourceControl, ICCNetDocumentation {
    private string _executable = string.Empty;
    private string _username = string.Empty;
    private HiddenPassword _password = new HiddenPassword ();
    private string _hostname = string.Empty;
    private int? _port = null;
    private string _sandboxroot = string.Empty;
    private string _sandboxfile = string.Empty;
    private bool? _autoGetSource = null;
    private Timeout _timeout = new Timeout ();

    /// <summary>
    /// Initializes a new instance of the <see cref="MksSourceIntegritySourceControl"/> class.
    /// </summary>
    public MksSourceIntegritySourceControl () : base ( "mks" ) { }


    /// <summary>
    /// The local path for the MKS source integrity command-line client (eg. c:\Mks\bin\si.exe)
    /// </summary>
    /// <value>The executable.</value>
    [Description ( @"The local path for the MKS source integrity command-line client (eg. c:\Mks\bin\si.exe)" ),
    DisplayName ( "(Executable)" ), DefaultValue ( null ), Category ( "Required" ),
    Editor(typeof(OpenFileDialogUIEditor),typeof(UITypeEditor)),
    OpenFileDialogTitle("Select the MKS executable..."),FileTypeFilter("MKS Executable|si.exe")]
    public string Executable { get { return this._executable; } set { this._executable = Util.CheckRequired ( this, "executable", value ); } }
    /// <summary>
    /// MKS Source Integrity user ID that CCNet should use
    /// </summary>
    /// <value>The username.</value>
    [Description ( "MKS Source Integrity user ID that CCNet should use" ),
    DisplayName ( "(Username)" ), DefaultValue ( null ), Category ( "Required" )]
    public string Username { get { return this._username; } set { this._username = Util.CheckRequired ( this, "username", value ); } }

    /// <summary>
    /// Password for the MKS Source Integrity user ID.
    /// </summary>
    /// <value>The password.</value>
    [Description ( "Password for the MKS Source Integrity user ID." ),
    DisplayName ( "(Password)" ), DefaultValue ( null ), Category ( "Required" ),
    TypeConverter(typeof(PasswordTypeConverter))]
    public HiddenPassword Password { get { return this._password; } set { this._password = Util.CheckRequired ( this, "password", value ); } }


    /// <summary>
    /// The IP address or machine name of the MKS Source Integrity server.
    /// </summary>
    /// <value>The name of the host.</value>
    [Description ( "The IP address or machine name of the MKS Source Integrity server." ),
    DisplayName ( "(HostName)" ), DefaultValue ( null ), Category ( "Required" )]
    public string HostName { get { return this._hostname; } set { this._hostname = Util.CheckRequired ( this, "hostname", value ); } }

    /// <summary>
    /// The port on the MKS Source Integrity server to connect to.
    /// </summary>
    /// <value>The port.</value>
    [Description ( "The port on the MKS Source Integrity server to connect to." ),
    DefaultValue ( null ), Category ( "Optional" )]
    public int? Port { get { return this._port; } set { this._port = value; } }

    /// <summary>
    /// The local path MKS Source Integrity sandbox root corresponds to.
    /// </summary>
    /// <value>The sandbox root.</value>
    [Description ( "The local path MKS Source Integrity sandbox root corresponds to." ),
    DefaultValue ( null ), Category ( "Optional" ), Editor(typeof(BrowseForFolderUIEditor),typeof(UITypeEditor)),
    BrowseForFolderDescription("Select path to the SandboxRoot")]
    public string SandboxRoot { get { return this._sandboxroot; } set { this._sandboxroot = value; } }

    /// <summary>
    /// The project file.
    /// </summary>
    /// <value>The sandbox file.</value>
    [Description ( "The project file." ),
    DefaultValue ( null ), Category ( "Optional" )]
    public string SandboxFile { get { return this._sandboxfile; } set { this._sandboxfile = value; } }

    /// <summary>
    /// Instruct CCNet whether or not you want it to automatically retrieve the latest source from the repository.
    /// </summary>
    /// <value>The auto get source.</value>
    [Description ( "Instruct CCNet whether or not you want it to automatically retrieve the latest source from the repository." ),
    DefaultValue ( null ), Category ( "Optional" ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }

    /// <summary>
    /// Sets the <see cref="CCNetConfig.Core.Timeout">Timeout</see> period for the source control operation
    /// </summary>
    /// <value>The timeout.</value>
    [Description ( "Sets the Timeout period for the source control operation." ), DefaultValue ( null ),
    TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      MksSourceIntegritySourceControl msc = this.MemberwiseClone () as MksSourceIntegritySourceControl;
      msc.Password = this.Password.Clone ();
      msc.Timeout = this.Timeout.Clone ();
      return msc;
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

      XmlElement ele = doc.CreateElement ( "executable" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Executable );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "username" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Username );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "password" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Password.GetPassword () );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "hostname" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.HostName );
      root.AppendChild ( ele );

      if ( this.Port.HasValue ) {
        ele = doc.CreateElement ( "port" );
        ele.InnerText = this.Port.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.SandboxFile ) ) {
        ele = doc.CreateElement ( "sandboxfile" );
        ele.InnerText = this.SandboxFile;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.SandboxRoot ) ) {
        ele = doc.CreateElement ( "sandboxroot" );
        ele.InnerText = this.SandboxRoot;
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }
      if ( this.Timeout != null ) {
      XmlElement tele = this.Timeout.Serialize ();
      if ( tele != null )
        root.AppendChild ( doc.ImportNode ( tele, true ) );
      }
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      string s = Util.GetElementOrAttributeValue ( "executable", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue ( "username", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Username = s;

      s = Util.GetElementOrAttributeValue ( "password", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Password.Password = s;

      s = Util.GetElementOrAttributeValue ( "hostname", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.HostName = s;

      s = Util.GetElementOrAttributeValue ( "executable", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse(s,out i) )
          this.Port = i;
      }

      s = Util.GetElementOrAttributeValue ( "sandboxroot", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.SandboxRoot = s;

      s = Util.GetElementOrAttributeValue ( "sandboxfile", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.SandboxFile = s;

      s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      XmlElement ele = (XmlElement)element.SelectSingleNode ( "timeout" );
      if ( ele != null )
        this.Timeout.Deserialize ( ele );
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/MKS+Source+Integrity+Source+Control+Block?decorator=printable" ); }
    }

    #endregion
  }
}
