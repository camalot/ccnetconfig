/*
 * Copyright (c) 2006-2008, Ryan Conrad. All rights reserved.
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
  /// CruiseControl.NET supports integrating with the PVCS Source Control system via the pcli client.
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class PvcsSourceControl : SourceControl, ICCNetDocumentation {

    private string _executable = string.Empty;
    private string _project = string.Empty;
    private string _subproject = string.Empty;
    private string _username = string.Empty;
    private HiddenPassword _password = new HiddenPassword ();
    private string _workingDirectory = string.Empty;
    private bool? _recursive = null;
    private bool? _labelOnSuccess = null;
    private bool? _autoGetSource = null;
    private string _labelOrPromotionName = string.Empty;
    private Timeout _timeout = new Timeout();

    /// <summary>
    /// Initializes a new instance of the <see cref="PvcsSourceControl"/> class.
    /// </summary>
    public PvcsSourceControl () : base("pvcs") {}

    /// <summary>
    /// The PVCS client executable.
    /// </summary>
    /// <value>The executable.</value>
    [Description ( "The PVCS client executable." ), DisplayName ( "(Executable)" ), DefaultValue ( null ),
   Category ( "Required" ),
   Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ),
    OpenFileDialogTitle ( "Select PVCS client executable." ), FileTypeFilter ( "Executable|*.exe" )]
    public string Executable { get { return this._executable; } set { this._executable = Util.CheckRequired ( this, "executable", value ); } }
    /// <summary>
    /// The location of the PVCS project database.
    /// </summary>
    /// <value>The project.</value>
    [Description ( "The location of the PVCS project database." ), DisplayName ( "(Project)" ), DefaultValue ( null ), Category ( "Required" )]
    public string Project { get { return this._project; } set { this._project = Util.CheckRequired ( this, "project", value ); } }
    /// <summary>
    /// One ore more projects in PVCS that you wish to monitor. As long as each subproject is separated with a space 
    /// and a "/", you can monitor more than one subproject at a time.
    /// </summary>
    /// <value>The sub project.</value>
    [Description ( "One ore more projects in PVCS that you wish to monitor. As long as each subproject is separated with a space and a \" / \", " +
     "you can monitor more than one subproject at a time." ), DisplayName ( "(SubProject)" ), DefaultValue ( null ), Category ( "Required" )]
    public string SubProject { get { return this._subproject; } set { this._subproject = Util.CheckRequired ( this, "subproject", value ); } }
    /// <summary>
    /// Username for the user account to use to connect to PVCS.
    /// </summary>
    /// <value>The username.</value>
    [Description ( "Username for the user account to use to connect to PVCS." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Username { get { return this._username; } set { this._username = value; } }
    /// <summary>
    /// Password for the PVCS user account.
    /// </summary>
    /// <value>The password.</value>
    [Description ( "Password for the PVCS user account." ), DefaultValue ( null ),
   TypeConverter ( typeof ( PasswordTypeConverter ) ), Category ( "Optional" )]
    public HiddenPassword Password { get { return this._password; } set { this._password =value; } }
    /// <summary>
    /// The local directory containing the source from the repository.
    /// </summary>
    /// <value>The working directory.</value>
    [Description ( "The local directory containing the source from the repository." ), DefaultValue ( null ),
   Category ( "Optional" ), Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
  BrowseForFolderDescription ( "Select local directory containing the source from the repository." )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
    /// <summary>
    /// Whether to monitor all subfolders of the specified subproject.
    /// </summary>
    /// <value>The recursive.</value>
    [Description("Whether to monitor all subfolders of the specified subproject."),DefaultValue(null),
   Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), 
    Category ( "Optional" )]
    public bool? Recursive { get { return this._recursive; } set { this._recursive = value; } }
    /// <summary>
    /// Whether or not to apply a label to the repository after each successful build.
    /// </summary>
    /// <value>The label on success.</value>
    [Description ( "Whether or not to apply a label to the repository after each successful build." ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
    Category ( "Optional" )]
    public bool? LabelOnSuccess { get { return this._labelOnSuccess; } set { this._labelOnSuccess = value; } }
    /// <summary>
    /// Specifies whether the CCNet should take responsibility for retrieving the current version of the source from the repository.
    /// </summary>
    /// <value>The auto get source.</value>
    [Description ( "Specifies whether the CCNet should take responsibility for retrieving the current version of the source from the repository." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// The label to use as your code-base. If this is specified, this label will be called to get all code associated with it when a get is done. 
    /// When the build is successful, the good code will have this base label associated with it in turn promoting it into the label.
    /// Label to apply to repository. If a value is specified, labelOnSuccess will automatically be set to true.
    /// </summary>
    /// <value>The name of the label or promotion.</value>
    [Description ( "The label to use as your code-base. If this is specified, this label will be called to get all code associated with it when a " +
      "get is done. When the build is successful, the good code will have this base label associated with it in turn promoting it into the label. " +
     "Label to apply to repository. If a value is specified, labelOnSuccess will automatically be set to true." ), DefaultValue ( null ), 
    Category ( "Optional" )]
    public string LabelOrPromotionName { get { return this._labelOrPromotionName; } set { this._labelOrPromotionName = value; } }
    /// <summary>
    /// Sets the timeout period for the source control operation.
    /// </summary>
    /// <value>The timeout.</value>
    [Description("Sets the timeout period for the source control operation."),TypeConverter(typeof(ExpandableObjectConverter)),
   DefaultValue ( null ), Category ( "Optional" )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      PvcsSourceControl pvcs = this.MemberwiseClone () as PvcsSourceControl;
      pvcs.Password = this.Password.Clone ();
      pvcs.Timeout = this.Timeout.Clone ();
      return pvcs;
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

      ele = doc.CreateElement ( "project" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Project );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "subproject" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.SubProject );
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty(this.Username) ) {
        ele = doc.CreateElement ( "username" );
        ele.InnerText = Username;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Password.Password ) ) {
        ele = doc.CreateElement ( "password" );
        ele.InnerText = Password.Password;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = WorkingDirectory;
        root.AppendChild ( ele );
      }

      if ( Recursive.HasValue ) {
        ele = doc.CreateElement ( "recursive" );
        ele.InnerText = Recursive.Value.ToString();
        root.AppendChild ( ele );
      }

      if ( LabelOnSuccess.HasValue ) {
        ele = doc.CreateElement ( "labelOnSucces" );
        ele.InnerText = LabelOnSuccess.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.LabelOrPromotionName ) ) {
        ele = doc.CreateElement ( "labelOrPromotionName" );
        ele.InnerText = LabelOrPromotionName;
        root.AppendChild ( ele );
      }

      if ( this.Timeout != null ) {
        XmlElement tele = this.Timeout.Serialize ();
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
      this._project = string.Empty;
      this._subproject = string.Empty;


      this.Username = string.Empty;
      this.Password = new HiddenPassword ();
      this.AutoGetSource = null;
      this.LabelOnSuccess = null;
      this.LabelOrPromotionName = null;
      this.Recursive = null;
      this.Timeout = new Timeout ();
      this.WorkingDirectory = string.Empty;

      this.Executable = Util.GetElementOrAttributeValue ( "executable", element );
      this.Project = Util.GetElementOrAttributeValue ( "project", element );
      this.SubProject = Util.GetElementOrAttributeValue ( "subproject", element );

      string s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "username", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Username = s;

      s = Util.GetElementOrAttributeValue ( "password", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Password.Password = s;

      s = Util.GetElementOrAttributeValue ( "labelOnSuccess", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.LabelOnSuccess = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "recursive", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Recursive = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "recursive", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Recursive = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.WorkingDirectory = s;

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
      get { return new Uri("http://confluence.public.thoughtworks.org/display/CCNET/PVCS+Source+Control+Block?decorator=printable"); }
    }

    #endregion
  }
}
