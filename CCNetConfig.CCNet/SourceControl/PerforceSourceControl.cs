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
  /// Perforce cannot apply purely numeric labels, which is what CCNet uses by default. Therefore, if you have 
  /// 'applyLabel' set to true, you must also setup a custom Labeller in your project, e.g. by using the Default Labeller.
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class PerforceSourceControl : SourceControl,ICCNetDocumentation {
    private string _view = string.Empty;
    private string _executable = string.Empty;
    private string _client = string.Empty;
    private string _user = string.Empty;
    private HiddenPassword _password = new HiddenPassword ();
    private string _port = string.Empty;
    private double? _timeZoneOffset = null;
    private bool? _applyLabel = null;
    private bool? _autoGetSource = null;
    private bool? _forceSync = null;
    private string _p4WebUrlFormat = string.Empty;
    /// <summary>
    /// Initializes a new instance of the <see cref="PerforceSourceControl"/> class.
    /// </summary>
    public PerforceSourceControl () : base("p4") { }

    /// <summary>
    /// The perforce 'view' to check for changes. For 'multi-line' views, use a comma-separated list. 'Exclusionary' view lines 
    /// starting with - cannot be used. Use a Filtered Source Control Block to achieve this behaviour.
    /// </summary>
    /// <value>The view.</value>
    [Description ( "The perforce 'view' to check for changes. For 'multi-line' views, use a comma-separated list. 'Exclusionary' view lines " +
      "starting with - cannot be used. Use a Filtered Source Control Block to achieve this behaviour." ),
   DisplayName ( "(View)" ), DefaultValue ( null ), Category ( "Required" )]
    public string View { get { return this._view; } set { this._view = Util.CheckRequired ( this, "view", value ); } }
    /// <summary>
    /// The location of the Perforce command line client executable.
    /// </summary>
    /// <value>The executable.</value>
    [Description ( "The location of the Perforce command line client executable." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Executable { get { return this._executable; } set { this._executable = value; } }
    /// <summary>
    /// The perforce 'client' to use.
    /// </summary>
    /// <value>The client.</value>
    [Description ( "The perforce 'client' to use." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Client { get { return this._client; } set { this._client = value; } }
    /// <summary>
    /// The perforce user to use.
    /// </summary>
    /// <value>The user.</value>
    [Description ( "The perforce user to use." ), DefaultValue ( null ), Category ( "Optional" )]
    public string User { get { return this._user; } set { this._user = value; } }
    /// <summary>
    /// The perforce password to use.
    /// </summary>
    /// <value>The password.</value>
    [Description ( "The perforce password to use." ), DefaultValue ( null ),
   TypeConverter ( typeof ( PasswordTypeConverter ) ), Category ( "Optional" )]
    public HiddenPassword Password { get { return this._password; } set { this._password = value; } }

    /// <summary>
    /// The perforce hostname and port to use.
    /// </summary>
    /// <value>The port.</value>
    [Description ( "The perforce hostname and port to use." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Port { get { return this._port; } set { this._port = value; } }
    /// <summary>
    /// How many hours ahead your Perforce Server is from your build server. E.g. if your build server is in London, and your Perforce 
    /// server is in New York, this value would be '-5'.
    /// </summary>
    /// <value>The time zone offset.</value>
    [Description ( "How many hours ahead your Perforce Server is from your build server. E.g. if your build server is in London, and your Perforce " +
     "server is in New York, this value would be '-5'." ), DefaultValue ( null ), Category ( "Optional" )]
    public double? TimeZoneOffset { get { return this._timeZoneOffset; } set { this._timeZoneOffset = value; } }
    /// <summary>
    /// The perforce hostname and port to use.
    /// </summary>
    /// <value><c>true</c> or <c>false</c></value>
    [Description ( "The perforce hostname and port to use." ), DefaultValue ( null ),
   Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), 
    Category ( "Optional" )]
    public bool? ApplyLabel { get { return this._applyLabel; } set { this._applyLabel = value; } }
    /// <summary>
    /// The perforce hostname and port to use.
    /// </summary>
    /// <value><c>true</c> or <c>false</c></value>
    [Description ( "The perforce hostname and port to use." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// The perforce hostname and port to use.
    /// </summary>
    /// <value><c>true</c> or <c>false</c></value>
    [Description ( "The perforce hostname and port to use." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? ForceSync { get { return this._forceSync; } set { this._forceSync = value; } }
    /// <summary>
    /// Creates a link to the P4Web change list page for each detected modification. The specified value is transformed using
    /// String.Format where the first argument ({0}) will be the substituted change list number.
    /// </summary>
    /// <value>The p4 web URL format.</value>
    [Description ( "Creates a link to the P4Web change list page for each detected modification. The specified value is transformed using " +
     "String.Format where the first argument ({0}) will be the substituted change list number." ), DefaultValue ( null ), Category ( "Optional" )]
    public string P4WebUrlFormat { get { return this._p4WebUrlFormat; } set { this._p4WebUrlFormat = value; } }

    /// <summary>
    /// Creates a copy of this obejct.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      PerforceSourceControl psc = this.MemberwiseClone () as PerforceSourceControl;
      psc.Password = this.Password.Clone ();
      return psc;
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

      XmlElement ele = doc.CreateElement ( "view" );
      ele.InnerText = Util.CheckRequired(this,ele.Name,this.View);
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty ( this.Executable ) ) {
        ele = doc.CreateElement ( "executable" );
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Client ) ) {
        ele = doc.CreateElement ( "client" );
        ele.InnerText = this.Client;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.User ) ) {
        ele = doc.CreateElement ( "user" );
        ele.InnerText = this.User;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Password.Password ) ) {
        ele = doc.CreateElement ( "password" );
        ele.InnerText = this.Password.GetPassword();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Port ) ) {
        ele = doc.CreateElement ( "port" );
        ele.InnerText = this.Port;
        root.AppendChild ( ele );
      }

      if ( this.TimeZoneOffset.HasValue ) {
        ele = doc.CreateElement ( "timeZoneOffset" );
        ele.InnerText = this.TimeZoneOffset.Value.ToString();
        root.AppendChild ( ele );
      }

      if ( this.ApplyLabel.HasValue ) {
        ele = doc.CreateElement ( "applyLabel" );
        ele.InnerText = this.ApplyLabel.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.ForceSync.HasValue ) {
        ele = doc.CreateElement ( "forceSync" );
        ele.InnerText = this.ForceSync.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.P4WebUrlFormat ) ) {
        ele = doc.CreateElement ( "p4WebURLFormat" );
        ele.InnerText = this.P4WebUrlFormat;
        root.AppendChild ( ele );
      }

      return root; ;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this._view = string.Empty;
      this.Executable = string.Empty;
      this.Client = string.Empty;
      this.User = string.Empty;
      this.Password = new HiddenPassword ();
      this.Port = string.Empty;
      this.TimeZoneOffset = null;
      this.ApplyLabel = null;
      this.AutoGetSource = null;
      this.ForceSync = null;
      this.P4WebUrlFormat = string.Empty;

      string s = Util.GetElementOrAttributeValue ( "view", element );
      this.View = s;

      s = Util.GetElementOrAttributeValue ( "executable", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue ( "client", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Client = s;

      s = Util.GetElementOrAttributeValue ( "user", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.User = s;

      s = Util.GetElementOrAttributeValue ( "password", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Password.Password = s;

      s = Util.GetElementOrAttributeValue ( "port", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Port = s;

      s = Util.GetElementOrAttributeValue ( "timeZoneOffset", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        double db = 0d;
        if ( double.TryParse(s,out db) )
          this.TimeZoneOffset = db;
      }

      s = Util.GetElementOrAttributeValue ( "applyLabel", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ApplyLabel = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "forceSync", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ForceSync = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "p4WebURLFormat", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.P4WebUrlFormat = s;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri("http://confluence.public.thoughtworks.org/display/CCNET/Perforce+Source+Control+Block?decorator=printable"); }
    }

    #endregion
  }
}
