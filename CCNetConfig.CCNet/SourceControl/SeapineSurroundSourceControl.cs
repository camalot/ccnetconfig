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
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// For Surround SCM you must specify the executable, branch, repository, working directory, and login. You may also specify the server, a 
  /// file match pattern, whether to use regular expressions or not, and whether to get recursively or not. The server defaults to connecting 
  /// to 127.0.0.1:4900. Regular expressions and recursive default to being turned off.
  /// </summary>
  /// <remarks>
  /// The Seapine Surround provider is designed to work with Surround 4.1. It may not work with earlier versions of Surround.
  /// </remarks>
  [ MinimumVersion( "1.0" ) ]
  public class SeapineSurroundSourceControl : SourceControl, ICCNetDocumentation {
    private string _executable = string.Empty;
    private string _serverconnect = string.Empty;
    private string _serverLoginUser = string.Empty;
    private HiddenPassword _serverLoginPassword = new HiddenPassword();
    private string _branch = string.Empty;
    private string _repository = string.Empty;
    private string _workingDirectory = string.Empty;
    private string _file = string.Empty;
    private bool? _recursive = null;
    private bool? _searchregex = null;
    private Timeout _timeout = new Timeout ();
    /// <summary>
    /// Initializes a new instance of the <see cref="SeapineSurroundSourceControl"/> class.
    /// </summary>
    public SeapineSurroundSourceControl () : base("surround") { }
    /// <summary>
    /// The local path for the Surround SCM command-line client (eg. C:\Program Files\Seapine\Surround SCM\sscm.exe)
    /// </summary>
    /// <value>The executable.</value>
    [Description(@"The local path for the Surround SCM command-line client (eg. C:\Program Files\Seapine\Surround SCM\sscm.exe)"),
   DisplayName ( "(Executable)" ), DefaultValue ( null ), Category ( "Required" ),
    Editor(typeof(OpenFileDialogUIEditor),typeof(UITypeEditor)),FileTypeFilter("Surround SCM|sscm.exe"),
    OpenFileDialogTitle("Select Surround SCM command-line client")]
    public string Executable { get { return this._executable; } set { this._executable = Util.CheckRequired ( this, "executable", value ); } }
    /// <summary>
    /// The IP address or machine name and port number of the Surround SCM server.
    /// </summary>
    /// <value>The server connect.</value>
    [Description ( "The IP address or machine name and port number of the Surround SCM server." ), DefaultValue ( null ), Category ( "Optional" )]
    public string ServerConnect { get { return this._serverconnect; } set { this._serverconnect = value; } }
    /// <summary>
    /// Surround SCM username that CCNet should use.
    /// </summary>
    /// <value>The username.</value>
    [Description ( "Surround SCM username that CCNet should use." ), DisplayName ( "(Username)" ), DefaultValue ( null ), Category ( "Required" )]
    public string Username { get { return this._serverLoginUser; } set { this._serverLoginUser = Util.CheckRequired ( this, "username", value ); } }
    /// <summary>
    /// Surround SCM password that CCNet should use.
    /// </summary>
    /// <value>The password.</value>
    [Description ( "Surround SCM password that CCNet should use." ), DisplayName("(Password)"),
   DefaultValue ( null ), TypeConverter ( typeof ( PasswordTypeConverter ) ), Category ( "Required" )]
    public HiddenPassword Password { get { return this._serverLoginPassword; } set { this._serverLoginPassword = Util.CheckRequired ( this, "password", value ); } }
    /// <summary>
    /// The Surround SCM branch to monitor.
    /// </summary>
    /// <value>The branch.</value>
    [Description ( "The Surround SCM branch to monitor." ), DefaultValue ( null ), DisplayName ( "(Branch)" ), Category ( "Required" )]
    public string Branch { get { return this._branch; } set { this._branch = Util.CheckRequired(this,"branch",value); } }
    /// <summary>
    /// The Surround SCM repository to monitor.
    /// </summary>
    /// <value>The respository.</value>
    [Description ( "The Surround SCM repository to monitor." ), DisplayName ( "(Repository)" ), DefaultValue ( null ), Category ( "Required" )]
    public string Repository { get { return this._repository; } set { this._repository = Util.CheckRequired ( this, "repository", value ); } }
    /// <summary>
    /// The local path to get files from Surround SCM to.
    /// </summary>
    /// <value>The respository.</value>
    [Description ( "The local path to get files from Surround SCM to." ), DisplayName ( "(WorkingDirectory)" ), DefaultValue ( null ),
    Category ( "Required" ), Editor(typeof(BrowseForFolderUIEditor),typeof(UITypeEditor)),
    BrowseForFolderDescription("Select path to the SCM repository.")]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = Util.CheckRequired ( this, "workingDirectory", value ); } }
    /// <summary>
    /// Monitor and retrieve all files in child repositories of the specified repository.
    /// </summary>
    /// <value>if <c>true</c>, recursive.</value>
    [Description ( "Monitor and retrieve all files in child repositories of the specified repository." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? Recursive { get { return this._recursive; } set { this._recursive = value; } }
    /// <summary>
    /// Treat the filename pattern as a regular expression.
    /// </summary>
    /// <value>if <c>true</c>, UseRegularExpression.</value>
    [Description ( "Treat the filename pattern as a regular expression." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? UseRegularExpression { get { return this._searchregex; } set { this._searchregex = value; } }
    /// <summary>
    /// A filename pattern to match to monitor and retrieve files.
    /// </summary>
    /// <value>The file.</value>
    [Description ( "A filename pattern to match to monitor and retrieve files." ), DefaultValue ( null ), Category ( "Optional" )]
    public string File { get { return this._file; } set { this._file = value; } }
    /// <summary>
    /// Sets the timeout period for the source control operation.
    /// </summary>
    /// <value>The timeout.</value>
    [Description ( "Sets the timeout period for the source control operation." ), DefaultValue ( null ),
   TypeConverter ( typeof ( ExpandableObjectConverter ) ), Category ( "Optional" )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      SeapineSurroundSourceControl sssc = this.MemberwiseClone () as SeapineSurroundSourceControl;
      sssc.Password = this.Password.Clone ();
      sssc.Timeout = this.Timeout.Clone ();
      return sssc;
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

      if ( !string.IsNullOrEmpty ( this.ServerConnect ) ) {
        ele = doc.CreateElement ( "serverconnect" );
        ele.InnerText = this.ServerConnect;
        root.AppendChild ( ele );
      }

      ele = doc.CreateElement ( "serverlogin" );
      ele.InnerText = string.Format ( "{0}:{1}", Util.CheckRequired ( this, "username", this.Username ), Util.CheckRequired ( this, "password", this.Password.GetPassword () ) );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "branch" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Branch );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "repository" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Repository );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "workingDirectory" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.WorkingDirectory );
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty ( this.File ) ) {
        ele = doc.CreateElement ( "file" );
        ele.InnerText = this.File;
        root.AppendChild ( ele );
      }

      // this is stupid, for some reason, the booleans in this provider uses ints
      if ( this.Recursive.HasValue ) {
        ele = doc.CreateElement ( "recursive" );
        ele.InnerText = Util.BooleanToInteger(this.Recursive.Value).ToString();
        root.AppendChild(ele);
      }
      // this is stupid, for some reason, the booleans in this provider uses ints
      if ( this.UseRegularExpression.HasValue ) {
        ele = doc.CreateElement ( "searchregexp" );
        ele.InnerText = Util.BooleanToInteger ( this.UseRegularExpression.Value ).ToString ();
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
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this._executable = string.Empty;
      this.ServerConnect = string.Empty;
      this._serverLoginUser = string.Empty;
      this._serverLoginPassword = new HiddenPassword ();
      this._branch = string.Empty;
      this._repository = string.Empty;
      this._workingDirectory = string.Empty;
      this.File = string.Empty;
      this.Recursive = null;
      this.UseRegularExpression = null;
      this.Timeout = new Timeout ();

      this.Executable = Util.GetElementOrAttributeValue ( "executable", element );
      string s = Util.GetElementOrAttributeValue ( "serverlogin", element );
      string[] sl = s.Split ( new char[] { ':' },2, StringSplitOptions.None );
      if ( sl.Length == 2 ) {
        this.Username = sl[0];
        this.Password.Password = sl[1];
      }

      s = Util.GetElementOrAttributeValue ( "serverconnect", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ServerConnect = s;

      this.Branch = Util.GetElementOrAttributeValue ( "branch", element );
      this.Repository = Util.GetElementOrAttributeValue ( "repository", element );
      this.WorkingDirectory = Util.GetElementOrAttributeValue ( "workingDirectory", element );

      s = Util.GetElementOrAttributeValue ( "recursive", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Recursive = string.Compare ( s, "1", true ) == 0 || string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "searchregex", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.UseRegularExpression = string.Compare ( s, "1", true ) == 0 || string.Compare ( s, bool.TrueString, true ) == 0;

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
      get { return new Uri("http://confluence.public.thoughtworks.org/display/CCNET/Seapine+Surround+Source+Control+Block?decorator=printable"); }
    }

    #endregion
  }
}
