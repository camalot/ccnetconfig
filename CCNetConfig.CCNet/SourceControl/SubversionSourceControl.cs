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
  /// CruiseControl.NET provides basic support for Subversion repositories. Checking for changes, running builds 
  /// (bootstrapped through NAnt), and tagging-by-copying are supported, but more advanced features such as using 
  /// Subversion revision numbers are not yet supported. Subversion support is under active development and will 
  /// improve over time.
  /// </summary>
  /// <remarks>
  /// <strong>A Working Copy Must Already Exist</strong><br />
  /// <para>The Subversion source control task requires that the working directory already contain a Subversion working copy. 
  /// If your build policy requires checking out a clean copy, then create a second one inside your build task. CruiseControl.Net 
  /// is not (yet) able to checkout a clean Subversion working copy from scratch.</para>
  /// </remarks>
  [ MinimumVersion( "1.0" ) ]
  public class SubversionSourceControl : SourceControl, ICCNetDocumentation {
    private Uri _trunkUrl = null;
    private string _workingDirectory = string.Empty;
    private string _executable = string.Empty;
    private string _username = string.Empty;
    private HiddenPassword _password = new HiddenPassword ();
    private bool? _autoGetSource = null;
    private WebUrlBuilder _webUrlBuilder = null;
    private bool? _tagOnSuccess = null;
    private Uri _tagBaseUrl = null;
    private Timeout _timeout = new Timeout ();

    /// <summary>
    /// Initializes a new instance of the <see cref="SubversionSourceControl"/> class.
    /// </summary>
    public SubversionSourceControl () : base ( "svn" ) { }

    /// <summary>
    /// The url for your repository (eg. svn://svnserver/)
    /// </summary>
    [Description ( "The url for your repository (eg. svn://svnserver/)" ), DefaultValue ( null ), DisplayName ( "(TrunkUrl)" ), Category ( "Required" )]
    public Uri TrunkUrl { get { return this._trunkUrl; } set { this._trunkUrl = Util.CheckRequired ( this, "trunkUrl", value ); } }
    /// <summary>
    /// The directory containing the locally checked out workspace.
    /// </summary>
    [Description ( "The directory containing the locally checked out workspace." ), DefaultValue ( null ),
   Category ( "Optional" ),
   Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
  BrowseForFolderDescription ( "Select path to the working directory." )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
    /// <summary>
    /// The location of the svn executable.
    /// </summary>
    [Description ( "The location of the svn executable." ), DefaultValue ( null ), Category ( "Optional" ),
Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "SubVersion Client|*.exe" ),
 OpenFileDialogTitle ( "Select SubVersion command-line client" )]
    public string Executable { get { return this._executable; } set { this._executable = value; } }
    /// <summary>
    /// The username to use for authentication when connecting to the repository.
    /// </summary>
    [Description ( "The username to use for authentication when connecting to the repository." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Username { get { return this._username; } set { this._username = value; } }
    /// <summary>
    /// The password to use for authentication when connecting to the repository.
    /// </summary>
    [Description ( "The password to use for authentication when connecting to the repository." ),
   DefaultValue ( null ), TypeConverter ( typeof ( PasswordTypeConverter ) ), Category ( "Optional" )]
    public HiddenPassword Password { get { return this._password; } set { this._password = value; } }
    /// <summary>
    /// Whether to retrieve the updates from Subversion for a particular build.
    /// </summary>
    [Description ( "Whether to retrieve the updates from Subversion for a particular build." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// The root url for the WebSVN site
    /// </summary>
    [Description ( "The root url for the WebSVN site" ), DefaultValue ( null ), Category ( "Optional" ),
    TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public WebUrlBuilder WebUrlBuilder { get { return this._webUrlBuilder; } set { this._webUrlBuilder = value; } }
    /// <summary>
    /// Indicates that the repository should be tagged if the build succeeds.
    /// </summary>
    [Description ( "Indicates that the repository should be tagged if the build succeeds." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? TagOnSuccess { get { return this._tagOnSuccess; } set { this._tagOnSuccess = value; } }
    /// <summary>
    /// The base url for tags in your repository.
    /// </summary>
    [Description ( "The base url for tags in your repository." ), DefaultValue ( null ), Category ( "Optional" )]
    public Uri TagBaseUrl { get { return this._tagBaseUrl; } set { this._tagBaseUrl = value; } }
    /// <summary>
    /// Sets the timeout period for the source control operation. See Timeout Configuration for details.
    /// </summary>
    [DefaultValue ( null ), Description ( "Sets the timeout period for the source control operation. See Timeout Configuration for details." ),
   TypeConverter ( typeof ( ExpandableObjectConverter ) ), Category ( "Optional" )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      SubversionSourceControl ssc = this.MemberwiseClone () as SubversionSourceControl;
      ssc.Password = this.Password.Clone ();
      if ( this.TagBaseUrl != null )
        ssc.TagBaseUrl = new Uri ( this.TagBaseUrl.ToString () );
      ssc.Timeout = this.Timeout.Clone ();
      if ( this.TrunkUrl != null )
        ssc.TrunkUrl = new Uri ( this.TrunkUrl.ToString () );
      ssc.WebUrlBuilder = this.WebUrlBuilder.Clone ();
      return ssc;
    }

    #region Serialization
    /// <summary>
    /// Serializes the object.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = doc.CreateElement ( "trunkUrl" );
      ele.InnerText = Util.UrlEncode(Util.CheckRequired ( this, ele.Name, this.TrunkUrl ).ToString(  ));
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Executable ) ) {
        ele = doc.CreateElement ( "executable" );
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Username ) ) {
        ele = doc.CreateElement ( "username" );
        ele.InnerText = this.Username;
        root.AppendChild ( ele );
      }

      if ( this.Password != null && !string.IsNullOrEmpty ( this.Password.Password ) ) {
        ele = doc.CreateElement ( "password" );
        ele.InnerText = this.Password.GetPassword ();
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.WebUrlBuilder != null ) {
        ele = this.WebUrlBuilder.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }

      if ( this.TagOnSuccess.HasValue ) {
        ele = doc.CreateElement ( "tagOnSuccess" );
        ele.InnerText = this.TagOnSuccess.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.TagBaseUrl != null ) {
        ele = doc.CreateElement ( "tagBaseUrl" );
        ele.InnerText = this.TagBaseUrl.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.Timeout != null ) {
        ele = this.Timeout.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      this._trunkUrl = null;
      this.AutoGetSource = null;
      this.Executable = string.Empty;
      this.Password = new HiddenPassword ();
      this.TagBaseUrl = null;
      this.TagOnSuccess = null;
      this.Timeout = new Timeout ();
      this.Username = string.Empty;
      this.WebUrlBuilder = new WebUrlBuilder ();
      this.WorkingDirectory = string.Empty;

      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

      this.TrunkUrl = new Uri ( Util.UrlDecode ( Util.GetElementOrAttributeValue ( "trunkUrl", element ) ) );

      string s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "executable", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue ( "password", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Password.Password = s;

      s = Util.GetElementOrAttributeValue ( "tagBaseUrl", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.TagBaseUrl = new Uri ( s );

      s = Util.GetElementOrAttributeValue ( "tagOnSuccess", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.TagOnSuccess = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "username", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Username = s;

      s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.WorkingDirectory = s;

      XmlElement ele = (XmlElement)element.SelectSingleNode ( "webUrlBuilder" );
      if ( ele != null )
        this.WebUrlBuilder.Deserialize ( ele );

      ele = (XmlElement)element.SelectSingleNode ( "timeout" );
      if ( ele != null )
        this.Timeout.Deserialize ( ele );
    }
    #endregion

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Subversion+Source+Control+Block?decorator=printable" ); }
    }
    #endregion

  }
}
