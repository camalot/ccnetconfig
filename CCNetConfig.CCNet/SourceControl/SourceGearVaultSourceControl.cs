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
using System.Xml;
using CCNetConfig.Core.Enums;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {

  /// <summary>
  /// Provide SourceGearVault SourceControl Access
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class SourceGearVaultSourceControl : SourceControl, ICCNetDocumentation {
    #region Members
    private string _executable = string.Empty;
    private string _username = string.Empty;
    private HiddenPassword _password = new HiddenPassword();
    private string _host = string.Empty;
    private string _repository = string.Empty;
    private string _folder = string.Empty;
    private bool? _ssl = null;
    private bool? _autoGetSource = null;
    private bool? _applyLabel = null;
    private bool? _useWorkingDirectory = null;
    private string _historyArgs = string.Empty;
    private Timeout _timeout = new Timeout();
    private string _workingDirectory = string.Empty;
    private bool? _cleanCopy = null;
    private SourceControlSetFileTime? _setFileTime = null;
    private string _proxyServer = string.Empty;
    private int? _proxyPort = null;
    private string _proxyUser = string.Empty;
    private HiddenPassword _proxyPassword = new HiddenPassword ();
    private string _proxyDomain = string.Empty;
    private int? _pollRetryAttempts = null;
    private int? _pollRetryWait = null;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceGearVaultSourceControl"/> class.
    /// </summary>
    public SourceGearVaultSourceControl() : base ("vault") { }
    /// <summary>
    /// The location of the Vault command-line executable
    /// </summary>
    [Description ( "The location of the Vault command-line executable" ), DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "Executable|*.exe" ),
   OpenFileDialogTitle ( "Select Vault command-line client" )]
    public string Executable { get { return this._executable; } set { this._executable = value; } }
    /// <summary>
    /// Vault user id that CCNet should use to authenticate
    /// </summary>
    [Description ( "Vault user id that CCNet should use to authenticate" ), DefaultValue ( null ), Category ( "Optional" )]
    public string Username { get { return this._username; } set { this._username = value; } }
    /// <summary>
    /// Password for the Vault user.
    /// </summary>
    [TypeConverter ( typeof ( PasswordTypeConverter ) ), DefaultValue ( null ), Description ( "Password for the Vault user" ), Category ( "Optional" )]
    public HiddenPassword Password { get { return this._password; } set { this._password = value; } }
    /// <summary>
    /// The name of the Vault server
    /// </summary>
    [Description ( "The name of the Vault server" ), DefaultValue ( null ), Category ( "Optional" )]
    public string Host { get { return this._host; } set { this._host = value; } }
    /// <summary>
    /// The name of the Vault repository to monitor.
    /// </summary>
    [Description ( "The name of the Vault repository to monitor." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Repository { get { return this._repository; } set { this._repository = value; } }
    /// <summary>
    /// The root folder to be monitored by CCNet
    /// </summary>
    [Description ( "The root folder to be monitored by CCNet" ), DefaultValue ( null ), Category ( "Optional" )]
    public string Folder { get { return this._folder; } set { this._folder = value; } }
    /// <summary>
    /// Should SSL be used to communicate with the Vault server.
    /// </summary>
    [Description ("Should SSL be used to communicate with the Vault server."), DefaultValue (null), 
    Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? UseSsl { get { return this._ssl; } set { this._ssl = value; } }
    /// <summary>
    /// Specifies if CCNet should automatically retrieve the latest version of the source from the repository
    /// </summary>
    [Description ("Specifies if CCNet should automatically retrieve the latest version of the source from the repository"), 
    DefaultValue (null), Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// Specifies if CCNet should apply the build label to the repository
    /// </summary>
		[Description ( "Specifies if CCNet should apply the build label to the repository" ), DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? ApplyLabel { get { return this._applyLabel; } set { this._applyLabel = value; } }
    /// <summary>
    /// <strong>CC.NET 1.0:</strong> Determines the working directory into which Vault files will be retrieved. Supply true if you want CCNet to use the Vault 
    /// folder working directory created for the build user using the Vault GUI (recommended). Supply false if CCNet should use the CCNet working directory. 
    /// <para>CC.NET 1.1: Determines if the source will be retrieved into a Vault Working Folder.</para>
    /// </summary>
    [Description ("CC.NET 1.0: Determines the working directory into which Vault files will be retrieved. Supply true if you want CCNet to use the Vault folder " +
      "working directory created for the build user using the Vault GUI (recommended). Supply false if CCNet should use the CCNet working directory. \n\n" +
      "CC.NET 1.1: Determines if the source will be retrieved into a Vault Working Folder."), DefaultValue (null),
    Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? UseWorkingDirectory { get { return this._useWorkingDirectory; } set { this._useWorkingDirectory = value; } }
    /// <summary>
    /// Extra arguments to be included in the history commandline
    /// </summary>
    [Description ( "Extra arguments to be included in the history commandline" ), DefaultValue ( null ), Category ( "Optional" )]
    public string HistoryArguments { get { return this._historyArgs; } set { this._historyArgs = value; } }
    /// <summary>
    /// Sets the timeout period for the source control operation. See Timeout Configuration for details.
    /// </summary>
    [Description ("Sets the timeout period for the source control operation. See Timeout Configuration for details."),
   DefaultValue ( null ), TypeConverter ( typeof ( ExpandableObjectConverter ) ), Category ( "Optional" )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }
    /// <summary>
    /// The root folder where the latest source will retrieved from Vault. This path can either be absolute or it can be relative to the CCNet project 
    /// working directory. 
    /// <para>
    /// CC.NET 1.1: If this element is missing or empty, Vault will attempt to use the directory set as the user's working folder. Note that this is 
    /// simply the destination path on disk. Whether or not this location is a Vault Working Folder is determined by the useWorkingFolder element. 
    /// To use the same path as the project, it is necessary to use "." (without the quotes) rather than leaving this empty, as you could in CC.NET 1.0.
    /// </para>
    /// </summary>
    [Description ("The root folder where the latest source will retrieved from Vault. This path can either be absolute or it can be relative to the " +
      "CCNet project working directory.\n\nCC.NET 1.1: If this element is missing or empty, Vault will attempt to use the directory set as the user's " +
      "working folder. Note that this is simply the destination path on disk. Whether or not this location is a Vault Working Folder is determined by the " +
      "useWorkingFolder element. To use the same path as the project, it is necessary to use \".\" (without the quotes) rather than leaving this empty, " +
    "as you could in CC.NET 1.0." ), Category ( "Optional" ), DefaultValue ( null ), 
    Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
   BrowseForFolderDescription ( "Select path to the working directory." )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
    /// <summary>
    /// If true, the source path will be emptied before retrieving new source.
    /// </summary>
    [Description ("If true, the source path will be emptied before retrieving new source."), DefaultValue (null),
     Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? CleanCopy { get { return this._cleanCopy; } set { this._cleanCopy = value; } }
    /// <summary>
    /// The modification date that retrieved source files will have.
    /// </summary>
    [Description ("The modification date that retrieved source files will have."), DefaultValue (null), Editor (typeof (DefaultableEnumUIEditor), typeof (UITypeEditor)),
   TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ), Category ( "Optional" )]
    public SourceControlSetFileTime? SetFileTime { get { return this._setFileTime; } set { this._setFileTime = value; } }
    /// <summary>
    /// The host name of the HTTP proxy Vault should use.
    /// </summary>
    [Description ( "The host name of the HTTP proxy Vault should use." ), DefaultValue ( null ), Category ( "Optional" )]
    public string ProxyServer { get { return this._proxyServer; } set { this._proxyServer = value; } }
    /// <summary>
    /// The port on the HTTP proxy Vault should use.
    /// </summary>
    [Description ( "The port on the HTTP proxy Vault should use." ), DefaultValue ( null ), Category ( "Optional" )]
    public int? ProxyPort { get { return this._proxyPort; } set { this._proxyPort = value; } }
    /// <summary>
    /// The user name for the HTTP proxy Vault should use.
    /// </summary>
    [Description ( "The user name for the HTTP proxy Vault should use." ), DefaultValue ( null ), Category ( "Optional" )]
    public string ProxyUser { get { return this._proxyUser; } set { this._proxyUser = value; } }
    /// <summary>
    /// The password for the HTTP proxy Vault should use.
    /// </summary>
    [TypeConverter ( typeof ( PasswordTypeConverter ) ), DefaultValue ( null ), Description ( "The password for the HTTP proxy Vault should use." ), 
    Category ( "Optional" )]
    public HiddenPassword ProxyPassword { get { return this._proxyPassword; } set { this._proxyPassword = value; } }
    /// <summary>
    /// The Windows domain of the HTTP proxy Vault should use.
    /// </summary>
    [Description ( "The Windows domain of the HTTP proxy Vault should use." ), DefaultValue ( null ), Category ( "Optional" )]
    public string ProxyDomain { get { return this._proxyDomain; } set { this._proxyDomain = value; } }
    /// <summary>
    /// The number of automatic retries when failing to check for modifications before an exception is thrown.
    /// </summary>
    [Description ("The number of automatic retries when failing to check for modifications before an exception is thrown."),
   DefaultValue ( null ), Category ( "Optional" )]
    public int? PollRetryAttempts { get { return this._pollRetryAttempts; } set { this._pollRetryAttempts = value; } }
    /// <summary>
    /// The number of seconds to wait between retries when a check for modifications fails.
    /// </summary>
    [Description ( "The number of seconds to wait between retries when a check for modifications fails." ), DefaultValue ( null ), 
    Category ( "Optional" )]
    public int? PollRetryWait { get { return this._pollRetryWait; } set { this._pollRetryWait = value; } }


    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      SourceGearVaultSourceControl sgv = this.MemberwiseClone () as SourceGearVaultSourceControl;
      sgv.Password = this.Password.Clone ();
      sgv.ProxyPassword = this.ProxyPassword.Clone ();
      sgv.Timeout = this.Timeout.Clone ();
      return sgv;
    }


    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize() {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ("sourcecontrol");
      root.SetAttribute ("type", this.TypeName);
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      if ( !string.IsNullOrEmpty (this.Executable) ) {
        XmlElement ele = doc.CreateElement ("executable");
        ele.InnerText = this.Executable;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Username) ) {
        XmlElement ele = doc.CreateElement ("username");
        ele.InnerText = this.Username;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty(this.Password.Password) ) {
        XmlElement ele = doc.CreateElement ("password");
        ele.InnerText = this.Password.GetPassword();
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Host) ) {
        XmlElement ele = doc.CreateElement ("host");
        ele.InnerText = this.Host;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Repository) ) {
        XmlElement ele = doc.CreateElement ("repository");
        ele.InnerText = this.Repository;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Folder) ) {
        XmlElement ele = doc.CreateElement ("folder");
        ele.InnerText = this.Folder;
        root.AppendChild (ele);
      }

      if ( this.UseSsl.HasValue ) {
        XmlElement ele = doc.CreateElement ("ssl");
        ele.InnerText = this.UseSsl.Value.ToString();
        root.AppendChild (ele);
      }

      if ( this.AutoGetSource.HasValue ) {
        XmlElement ele = doc.CreateElement ("autoGetSource");
        ele.InnerText = this.AutoGetSource.Value.ToString ();
        root.AppendChild (ele);
      }

      if ( this.ApplyLabel.HasValue ) {
        XmlElement ele = doc.CreateElement ("applyLabel");
        ele.InnerText = this.ApplyLabel.Value.ToString ();
        root.AppendChild (ele);
      }

      if ( this.UseWorkingDirectory.HasValue ) {
        XmlElement ele = doc.CreateElement ("useWorkingDirectory");
        ele.InnerText = this.UseWorkingDirectory.Value.ToString ();
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.HistoryArguments) ) {
        XmlElement ele = doc.CreateElement ("historyArgs");
        ele.InnerText = this.HistoryArguments;
        root.AppendChild (ele);
      }

      if ( this.Timeout != null ) {
        XmlElement tele = this.Timeout.Serialize ();
        if ( tele != null )
          root.AppendChild (doc.ImportNode (tele, true));
      }

      if ( !string.IsNullOrEmpty (this.WorkingDirectory) ) {
        XmlElement ele = doc.CreateElement ("workingDirectory");
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild (ele);
      }

      if ( CleanCopy.HasValue ) {
        XmlElement ele = doc.CreateElement ("cleanCopy");
        ele.InnerText = this.CleanCopy.Value.ToString();
        root.AppendChild (ele);
      }

      if ( SetFileTime.HasValue ) {
        XmlElement ele = doc.CreateElement ("setFileTime");
        ele.InnerText = this.SetFileTime.Value.ToString ();
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.ProxyServer) ) {
        XmlElement ele = doc.CreateElement ("proxyServer");
        ele.InnerText = this.ProxyServer;
        root.AppendChild (ele);
      }

      if ( ProxyPort.HasValue ) {
        XmlElement ele = doc.CreateElement ("proxyPort");
        ele.InnerText = this.ProxyPort.Value.ToString ();
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.ProxyUser) ) {
        XmlElement ele = doc.CreateElement ("proxyUser");
        ele.InnerText = this.ProxyUser;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.ProxyPassword.Password) ) {
        XmlElement ele = doc.CreateElement ("proxyPassword");
        ele.InnerText = this.ProxyPassword.GetPassword();
        root.AppendChild (ele);
      }

      if ( PollRetryAttempts.HasValue ) {
        XmlElement ele = doc.CreateElement ("pollRetryAttempts");
        ele.InnerText = this.PollRetryAttempts.Value.ToString ();
        root.AppendChild (ele);
      }

      if ( PollRetryWait.HasValue ) {
        XmlElement ele = doc.CreateElement ("pollRetryWait");
        ele.InnerText = this.PollRetryWait.Value.ToString ();
        root.AppendChild (ele);
      }

      return root;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable (false)]
    public Uri DocumentationUri {
      get { return new Uri ("http://ccnet.thoughtworks.net/display/CCNET/SourceGear+Vault+Source+Control+Block?decorator=printable"); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.ApplyLabel = null;
      this.AutoGetSource = null;
      this.CleanCopy = null;
      this.Executable = string.Empty;
      this.Folder = string.Empty;
      this.HistoryArguments = string.Empty;
      this.Host = string.Empty;
      this.Password = new HiddenPassword ();
      this.PollRetryAttempts = null;
      this.PollRetryWait = null;
      this.ProxyDomain = string.Empty;
      this.ProxyPassword = new HiddenPassword ();
      this.ProxyPort = null;
      this.ProxyServer = string.Empty;
      this.ProxyUser = string.Empty;
      this.Repository = string.Empty;
      this.SetFileTime = null;
      this.Timeout = new Timeout ();
      this.Username = string.Empty;
      this.UseSsl = null;
      this.UseWorkingDirectory = null;
      this.WorkingDirectory = string.Empty;

      if ( string.Compare (element.GetAttribute ("type"), this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute ("type"), this.TypeName));

      string s = Util.GetElementOrAttributeValue ("executable", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue("applyLabel",element);
      if ( !string.IsNullOrEmpty (s) )
        this.ApplyLabel = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("autoGetSource", element);
      if ( !string.IsNullOrEmpty (s) )
        this.AutoGetSource = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("cleanCopy", element);
      if ( !string.IsNullOrEmpty (s) )
        this.CleanCopy = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("folder", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Folder = s;

      s = Util.GetElementOrAttributeValue ("historyArgs", element);
      if ( !string.IsNullOrEmpty (s) )
        this.HistoryArguments = s;

      s = Util.GetElementOrAttributeValue ("host", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Host = s;

      s = Util.GetElementOrAttributeValue ("password", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Password.Password = s;

      s = Util.GetElementOrAttributeValue ("pollRetryAttempts", element);
      if ( !string.IsNullOrEmpty (s) ) {
        int i = 0;
        if ( int.TryParse (s, out i) )
          this.PollRetryAttempts = i;
      }

      s = Util.GetElementOrAttributeValue ("pollRetryWait", element);
      if ( !string.IsNullOrEmpty (s) ) {
        int i = 0;
        if ( int.TryParse (s, out i) )
          this.PollRetryWait = i;
      }

      s = Util.GetElementOrAttributeValue ("proxyDomain", element);
      if ( !string.IsNullOrEmpty (s) )
        this.ProxyDomain = s;

      s = Util.GetElementOrAttributeValue ("proxyPassword", element);
      if ( !string.IsNullOrEmpty (s) )
        this.ProxyPassword.Password = s;

      s = Util.GetElementOrAttributeValue ("proxyPort", element);
      if ( !string.IsNullOrEmpty (s) ) {
        int i = 0;
        if ( int.TryParse (s, out i) )
          this.ProxyPort = i;
      }

      s = Util.GetElementOrAttributeValue ("proxyServer", element);
      if ( !string.IsNullOrEmpty (s) )
        this.ProxyServer = s;

      s = Util.GetElementOrAttributeValue ("proxyUser", element);
      if ( !string.IsNullOrEmpty (s) )
        this.ProxyUser = s;

      s = Util.GetElementOrAttributeValue ("repository", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Repository = s;

      s = Util.GetElementOrAttributeValue ("setFileTime", element);
      if ( !string.IsNullOrEmpty (s) )
        this.SetFileTime = (Core.Enums.SourceControlSetFileTime)Enum.Parse (typeof (Core.Enums.SourceControlSetFileTime), s, true);

      XmlElement ele = (XmlElement)element.SelectSingleNode ("timeout");
      if ( ele != null )
        this.Timeout.Deserialize (ele);

      s = Util.GetElementOrAttributeValue ("username", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Username = s;

      s = Util.GetElementOrAttributeValue ("ssl", element);
      if ( !string.IsNullOrEmpty (s) )
        this.UseSsl = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("useWorkingDirectory", element);
      if ( !string.IsNullOrEmpty (s) )
        this.UseWorkingDirectory = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("workingDirectory", element);
      if ( !string.IsNullOrEmpty (s) )
        this.WorkingDirectory = s;
    }
  }
}
