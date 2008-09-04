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
using System.IO;
using System.Xml;
using System.ComponentModel;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// <para>For cvs you must define where the cvs executable (if you give a relative path, it must be relative to the ccnet.exe application) 
  /// is and the working directory for checked out code. You may optionally specify the cvsroot.</para>
  /// </summary>
  /// <remarks>
  /// see <a href="http://confluence.public.thoughtworks.org/display/CCNET/CVS+Source+Control+Block">CVS Source Control</a> documentation for more info.
  /// </remarks>
  [ MinimumVersion( "1.0" ) ]
  public class CvsSourceControl : SourceControl, ICCNetDocumentation {
    private string _executable = string.Empty;
    private string _workingDirectory = null;
    private string _cvsRoot = string.Empty;
    private bool? _restrictLogins = null;
    private string _branch = string.Empty;
    private WebUrlBuilder _webUrlBuilder = null;
    private bool? _autoGetSource = null;
    private bool? _labelOnSuccess = null;
    private string _tagPrefix = string.Empty;
    private bool? _cleanCopy = null;
    private bool? _useHistory = null;
    private Timeout _timeout = null;

    private string _module = string.Empty;
    private bool? _suppressRevisionHeader = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="CvsSourceControl"/> class.
    /// </summary>
    public CvsSourceControl ()
      : base ( "cvs" ) {
      this._timeout = new Timeout ();
      this.WebUrlBuilder = new WebUrlBuilder ();
    }
    /// <summary>
    /// The location of the cvs.exe executable.
    /// </summary>
    /// <value>The executable.</value>
    [Description ( "The location of the cvs.exe executable." ), DefaultValue ( null ), DisplayName ( "(Executable)" ),
    Category ( "Required" ), Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "CVS|cvs.exe" ),
    OpenFileDialogTitle ( "Select cvs.exe" )]
    public string Executable { get { return this._executable; } set { this._executable = Util.CheckRequired ( this, "executable", value ); } }
    /// <summary>
    /// The folder that the source has been checked out into.
    /// </summary>
    /// <value>The working directory.</value>
    [Description ( "The folder that the source has been checked out into." ), DefaultValue ( null ),
   Category ( "Optional" ), Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
 BrowseForFolderDescription ( "Select path that the source has been checked out into." )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
    /// <summary>
    /// The cvs connection string. If this is unspecified, the CVS client will automatically calculate the correct root
    /// based on your working directory. This is only required for version 1.2 and later.
    /// </summary>
    /// <value>The CVS root.</value>
    [Description ( "The cvs connection string. If this is unspecified, the CVS client will automatically calculate the correct root " +
    "based on your working directory. This is only required for version 1.2 and later." ), DefaultValue ( null ),
    Category ( "Required" ), DisplayName ( "(CvsRoot)" )]
    public string CvsRoot {
      get { return this._cvsRoot; }
      set {
        Version versionInfo = Util.GetTypeDescriptionProviderVersion ( this.GetType () );
        Version requiredVersion = new Version ( "1.2" );
        if ( versionInfo != null && versionInfo.CompareTo ( requiredVersion ) >= 0 )
          this._cvsRoot = Util.CheckRequired ( this, "cvsRoot", value );
        else
          this._cvsRoot = value;
      }
    }
    /// <summary>
    /// The cvs module to monitor. This element is used both when checking for modifications and when 
    /// checking out the source into an empty working directory.
    /// </summary>
    /// <value>The module.</value>
    [Description ( "The cvs module to monitor. This element is used both when checking for modifications and when checking out the " +
    "source into an empty working directory. This is only required for version 1.2 and later." ), Category ( "Required" ), DisplayName ( "(Module)" ),
    DefaultValue ( null )]
    public string Module {
      get { return this._module; }
      set {
        // this checks if the version is 
        Version versionInfo = Util.GetTypeDescriptionProviderVersion ( this.GetType () );
        Version requiredVersion = new Version ( "1.2" );
        if ( versionInfo != null && versionInfo.CompareTo ( requiredVersion ) >= 0 )
          this._module = Util.CheckRequired ( this, "module", value );
        else
          this._module = value; 
      }
    }
    /// <summary>
    /// Suppresses headers that do not have revisions within the specified modification window. 
    /// Setting this option to true will reduce the time that it takes for CCNet to poll CVS for 
    /// changes. Only fairly recent versions of CVS support this option. Run cvs --help log to 
    /// see if the -S option is listed.
    /// </summary>
    /// <value>The suppress revision header.</value>
    [Description ( "Suppresses headers that do not have revisions within the specified modification " +
      "window. Setting this option to true will reduce the time that it takes for CCNet to poll CVS " +
      "for changes. Only fairly recent versions of CVS support this option. Run cvs --help log to see " +
      "if the -S option is listed." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
    MinimumVersion ( "1.2" )]
    public bool? SuppressRevisionHeader { get { return this._suppressRevisionHeader; } set { this._suppressRevisionHeader = value; } }
    /// <summary>
    /// Only list modifications checked in by specified logins
    /// </summary>
    /// <value>The restrict logins.</value>
    [Description ( "Only list modifications checked in by specified logins." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? RestrictLogins { get { return this._restrictLogins; } set { this._restrictLogins = value; } }
    /// <summary>
    /// The url builder section for the ViewCVS server. (see <a href="http://confluence.public.thoughtworks.org/display/CCNET/Using+CruiseControl.NET+with+CVS">Using CruiseControl.NET with CVS</a>)
    /// </summary>
    /// <value>The web URL builder.</value>
    [Description ( "The url builder section for the ViewCVS server." ), DefaultValue ( null ),
   TypeConverter ( typeof ( ExpandableObjectConverter ) ), Category ( "Optional" )]
    public WebUrlBuilder WebUrlBuilder { get { return this._webUrlBuilder; } set { this._webUrlBuilder = value; } }
    /// <summary>
    /// The branch to check for modifications on.
    /// </summary>
    /// <value>The branch.</value>
    [Description ( "The branch to check for modifications on." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Branch { get { return this._branch; } set { this._branch = value; } }
    /// <summary>
    /// Specifies whether the current version of the source should be retrieved from CVS.
    /// </summary>
    /// <value>The auto get source.</value>
    [Description ( "Specifies whether the current version of the source should be retrieved from CVS." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// Specifies whether or not the repository should be labelled after a successful build.
    /// </summary>
    /// <value>The label on success.</value>
    [Description ( "Specifies whether or not the repository should be labelled after a successful build." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? LabelOnSuccess { get { return this._labelOnSuccess; } set { this._labelOnSuccess = value; } }
    /// <summary>
    /// By default the CVS tag name used when <see cref="CCNetConfig.CCNet.CvsSourceControl.LabelOnSuccess">LabelOnSuccess</see> is set to <c>true</c>
    /// is ver-BuildLabel. If you specify this property, the prefix ver- will be replaced with the value you specify.
    /// </summary>
    /// <value>The tag prefix.</value>
    [Description ( "By default the CVS tag name used when LabelOnSuccess is set to true is ver-BuildLabel. " +
    "If you specify this property, the prefix ver- will be replaced with the value you specify." ), DefaultValue ( null ), Category ( "Optional" )]
    public string TagPrefix { get { return this._tagPrefix; } set { this._tagPrefix = value; } }
    /// <summary>
    /// Specifies whether or not a clean copy should be retrieved.
    /// </summary>
    /// <value>The clean copy.</value>
    [Description ( "Specifies whether or not a clean copy should be retrieved." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? CleanCopy { get { return this._cleanCopy; } set { this._cleanCopy = value; } }
    /// <summary>
    /// Specifies whether or not to use the cvs history command to speed up modification checks.
    /// </summary>
    /// <value>The use history.</value>
    [Description ( "Specifies whether or not to use the cvs history command to speed up modification checks." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
    MaximumVersion( "1.1.9.9" )]
    public bool? UseHistory { get { return this._useHistory; } set { this._useHistory = value; } }
    /// <summary>
    /// Sets the timeout period for the source control operation.
    /// </summary>
    /// <value>The timeout.</value>
    /// <seealso cref="CCNetConfig.Core.Timeout"/>
    [Description ( "Sets the timeout period for the source control operation." ), DefaultValue ( null ), Category ( "Optional" )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    /// Creates a copy of the CvsSourceControl
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      CvsSourceControl cvs = this.MemberwiseClone () as CvsSourceControl;
      cvs.AutoGetSource = this.AutoGetSource;
      cvs.Timeout = this.Timeout.Clone ();
      cvs.WebUrlBuilder = this.WebUrlBuilder.Clone ();
      return cvs;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof( SourceControl ) );
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = doc.CreateElement ( "executable" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Executable );
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = WorkingDirectory;
        root.AppendChild ( ele );
      }

      Version requiredVersion = new Version ( "1.2" );
      if ( versionInfo != null && versionInfo.CompareTo ( requiredVersion ) >= 0 ) {
        ele = doc.CreateElement ( "cvsroot" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.CvsRoot );
        root.AppendChild ( ele );
      } else {
        if ( !string.IsNullOrEmpty ( CvsRoot ) ) {
          ele = doc.CreateElement ( "cvsroot" );
          ele.InnerText = CvsRoot;
          root.AppendChild ( ele );
        }
      }

      if ( versionInfo != null && versionInfo.CompareTo ( requiredVersion ) >= 0 ) {
        ele = doc.CreateElement ( "module" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Module );
        root.AppendChild ( ele );
      } else {
        if ( !string.IsNullOrEmpty ( Module ) ) {
          ele = doc.CreateElement ( "module" );
          ele.InnerText = Module;
          root.AppendChild ( ele );
        }
      }

      PropertyDescriptor pd = Util.GetPropertyDescriptor ( this.GetType (), "SuppressRevisionHeader", true );
      if ( pd != null ) {
        Version minVersion = Util.GetMinimumVersion ( pd );
        Version maxVersion = Util.GetMaximumVersion ( pd );
        if ( Util.IsInVersionRange ( minVersion, maxVersion, versionInfo ) ) {
          if ( this.SuppressRevisionHeader.HasValue ) {
            ele = doc.CreateElement ( "suppressRevisionHeader" );
            ele.InnerText = SuppressRevisionHeader.Value.ToString ();
            root.AppendChild ( ele );
          }
        }
      }

      if ( this.RestrictLogins.HasValue ) {
        ele = doc.CreateElement ( "restrictLogins" );
        ele.InnerText = RestrictLogins.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.WebUrlBuilder != null ) {
        ele = this.WebUrlBuilder.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }

      if ( !string.IsNullOrEmpty ( this.Branch ) ) {
        ele = doc.CreateElement ( "branch" );
        ele.InnerText = this.Branch;
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( LabelOnSuccess.HasValue ) {
        ele = doc.CreateElement ( "labelOnSuccess" );
        ele.InnerText = LabelOnSuccess.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.TagPrefix ) ) {
        ele = doc.CreateElement ( "tagPrefix" );
        ele.InnerText = TagPrefix;
        root.AppendChild ( ele );
      }

      if ( CleanCopy.HasValue ) {
        ele = doc.CreateElement ( "cleanCopy" );
        ele.InnerText = CleanCopy.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( UseHistory.HasValue ) {
        ele = doc.CreateElement ( "useHistory" );
        ele.InnerText = UseHistory.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.Timeout != null ) {
        ele = Timeout.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }
      return root;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/CVS+Source+Control+Block?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      this._executable = string.Empty;
      this.AutoGetSource = null;
      this.Branch = string.Empty;
      this.CleanCopy = null;
      this._cvsRoot = string.Empty;
      this.LabelOnSuccess = null;
      this.RestrictLogins = null;
      this.TagPrefix = string.Empty;
      this.Timeout = new Timeout ();
      this.UseHistory = null;
      this.WebUrlBuilder = null;
      this.WorkingDirectory = string.Empty;
      this._module = string.Empty;
      this.SuppressRevisionHeader = null;

      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

      this.Executable = Util.GetElementOrAttributeValue ( "executable", element );

      string s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "branch", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Branch = s;

      s = Util.GetElementOrAttributeValue ( "cleanCopy", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.CleanCopy = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "labelOnSuccess", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.LabelOnSuccess = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "restrictLogins", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.RestrictLogins = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "useHistory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.UseHistory = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "cvsroot", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this._cvsRoot = s;

      s = Util.GetElementOrAttributeValue ( "module", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this._module = s;

      s = Util.GetElementOrAttributeValue ( "suppressRevisionHeader", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.SuppressRevisionHeader = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "tagPrefix", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.TagPrefix = s;

      s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.WorkingDirectory = s;

      XmlElement ele = (XmlElement)element.SelectSingleNode ( "timeout" );
      if ( ele != null && ( ele.HasAttributes || ele.HasChildNodes ) )
      {
        this.Timeout = new Timeout ();
        this.Timeout.Deserialize ( ele );
      }

      ele = (XmlElement)element.SelectSingleNode ( "webUrlBuilder" );
      if ( ele != null && ( ele.HasAttributes || ele.HasChildNodes ) )
      {
        // this doesn't protect against malformed Uri's, but I haven't seen checks for those anywhere else
        this.WebUrlBuilder = new WebUrlBuilder ();
        this.WebUrlBuilder.Deserialize ( ele );
      }
    }
  }
}
