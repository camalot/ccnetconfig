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
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// For Visual Source Safe you must specify the executable, project, username and password. 
  /// You may also specify the SourceSafeDirectory. If SourceSafeDirectory is not set the default or the SSDIR environment variable will be used.
  /// </summary>
  /// <remarks>
  /// <para>CCNet periodically reports the following error when connecting to VSS: "Unable to open user login file 
  /// \SourceSafe\Vss60\data\loggedin\&lt;userid&gt;.log." What gives?</para>
  /// <para>If you have set CCNet up to manage multiple projects that all connect to the VSS repository using the same user id then you may 
  /// sporadically receive this failure. Our analysis suggests that the root of the problem is caused by the fact that VSS will create the 
  /// &lt;userid&gt;.log file when a user logs into VSS and delete it when the user logs out again. If a second build is using the repository 
  /// concurrently with the same user, when that second build logs out it looks for &lt;userid&gt;.log, but it's gone. Hence the error.</para>
  /// <para>There are three approaches that you can take to deal with this:
  ///   <ul>
  ///     <li>Log into VSS using different users for each CCNet project.</li>
  ///     <li>You can keep CCNet from publishing exceptions , so this exception will just get logged into the ccnet.log file.</li>
  ///     <li>Leave the VSS GUI open on the integration server. This will mean the userid.log file never gets deleted.</li>
  ///   </ul>
  /// </para>
  /// <para></para>
  /// <para>If you're using a labeller that returns a label equal to one already applied in the repository, the old label will be deleted when the 
  /// new one is added.</para>
  /// <para>This is because of a quirk in how VSS deals with labels of the same name; it should not be a problem with the default labeller.</para>
  /// <para>This problem usually occurs when someone is using a custom labeller (a class that implements ILabeller) and that custom labeller 
  /// returns a constant value.</para>
  /// <para>Workaround: If you use a custom labeller, make sure each label is unique.</para>
  /// </remarks>
  [ MinimumVersion( "1.0" ) ]
  public class VisualSourceSafeSourceControl : SourceControl, ICCNetDocumentation {
    private string _project = string.Empty;
    private string _username = string.Empty;
    private HiddenPassword _password = new HiddenPassword ();
    private string _ssdir = string.Empty;
    private string _executable = string.Empty;
    private bool? _autoGetSource = null;
    private bool? _applyLabel = null;
    private string _workingDirectory = string.Empty;
    private Timeout _timeout = new Timeout ();
    private string _cultire = string.Empty;
    private bool? _cleanCopy = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualSourceSafeSourceControl"/> class.
    /// </summary>
    public VisualSourceSafeSourceControl () : base ( "vss" ) { }
    /// <summary>
    /// The project in the repository to be monitored.
    /// </summary>
    /// <value>The project.</value>
    [Description ( "The project in the repository to be monitored." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Project { get { return this._project; } set { this._project = value; } }
    /// <summary>
    /// VSS user ID that CCNet should use to authenticate. If the username is unspecified, the VSS client will attempt to authenticate using the NT user.
    /// </summary>
    /// <value>The username.</value>
    [Description ( "VSS user ID that CCNet should use to authenticate. If the username is unspecified, the VSS " +
      "client will attempt to authenticate using the NT user." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Username { get { return this._username; } set { this._username = value; } }
    /// <summary>
    /// Password for the VSS user ID.
    /// </summary>
    /// <value>The password.</value>
    [Description ( "Password for the VSS user ID." ), DefaultValue ( null ), Category ( "Optional" ), TypeConverter ( typeof ( PasswordTypeConverter ) )]
    public HiddenPassword Password { get { return this._password; } set { this._password = value; } }
    /// <summary>
    /// The directory containing SRCSAFE.INI. If this SSDIR environment variable is already set then this property may be omitted.
    /// </summary>
    /// <value>The source safe directory.</value>
    [Description ( "The directory containing SRCSAFE.INI. If this SSDIR environment variable is already set then this property may be omitted." ),
   DefaultValue ( null ), Category ( "Optional" ),
  Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
 BrowseForFolderDescription ( "Select path to the SourceSafe directory." )]
    public string SourceSafeDirectory { get { return this._ssdir; } set { this._ssdir = value; } }
    /// <summary>
    /// The location of SS.EXE. If VSS is installed on the integration server, the location of VSS will be read from the registry
    /// and this element may be omitted.
    /// </summary>
    /// <value>The executable.</value>
    [Description ( "The location of SS.EXE. If VSS is installed on the integration server, the location of VSS will be read from the registry " +
     "and this element may be omitted." ), DefaultValue ( null ), Category ( "Optional" ),
Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "SourceSafe Client|ss.exe" ),
OpenFileDialogTitle ( "Select SourceSafe client" )]
    public string Executable { get { return this._executable; } set { this._executable = value; } }
    /// <summary>
    /// Specifies whether the current version of the source should be retrieved from VSS
    /// </summary>
    /// <value>The auto get source.</value>
    [Description ( "Specifies whether the current version of the source should be retrieved from VSS" ), DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// Specifies whether the current CCNet label should be applied to all source files under the current project in VSS. Note: the 
    /// specified VSS username must have write access to the repository.
    /// </summary>
    /// <value>The apply label.</value>
    [Description ( "Specifies whether the current CCNet label should be applied to all source files under the current project in VSS. Note: the " +
      "specified VSS username must have write access to the repository." ), DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? ApplyLabel { get { return this._applyLabel; } set { this._applyLabel = value; } }
    /// <summary>
    /// The folder into which the source should be retrived from VSS. If this folder does not exist, it will be automatically created.
    /// </summary>
    /// <value>The working directory.</value>
    [Description ( "The folder into which the source should be retrived from VSS. If this folder does not exist, it will be automatically created." ),
   DefaultValue ( null ), Category ( "Optional" ),
  Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
 BrowseForFolderDescription ( "Select path to the working directory." )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
    /// <summary>
    /// Sets the timeout period for the source control operation.
    /// </summary>
    /// <value>The timeout.</value>
    [Description ( "Sets the timeout period for the source control operation." ), DefaultValue ( null ), Category ( "Optional" ),
    TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }
    /// <summary>
    /// The culture under which VSS is running. This value must match the culture of the VSS installation for CCNet to work with VSS. Most of the time 
    /// the default is OK and you may omit this item. If you are using the US version of VSS on a machine that is not set to the US culture, 
    /// you should set the culture to "en-US"
    /// </summary>
    /// <value>The culture.</value>
    [Description ( "The culture under which VSS is running. This value must match the culture of the VSS installation for CCNet to work with VSS. " +
      "Most of the time the default is OK and you may omit this item. If you are using the US version of VSS on a machine that is not set to the US " +
      "culture, you should set the culture to \"en-US\"." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Culture { get { return this._cultire; } set { this._cultire = value; } }
    /// <summary>
    /// Controls whether or not VSS gets a clean copy (overwrites modified files) when getting the latest source.
    /// </summary>
    /// <value>The clean copy.</value>
    [Description ( "Controls whether or not VSS gets a clean copy (overwrites modified files) when getting the latest source." ), 
    DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? CleanCopy { get { return this._cleanCopy; } set { this._cleanCopy = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      VisualSourceSafeSourceControl vss = this.MemberwiseClone () as VisualSourceSafeSourceControl;
      vss.Password = this.Password.Clone ();
      vss.Timeout = this.Timeout.Clone ();
      return vss;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc= new XmlDocument();
      XmlElement root = doc.CreateElement("sourcecontrol");
      root.SetAttribute("type",this.TypeName);
      //root.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name ) );


      if ( !string.IsNullOrEmpty ( this.Project ) ) {
        XmlElement ele = doc.CreateElement ( "project" );
        ele.InnerText = this.Project;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Username ) ) {
        XmlElement ele = doc.CreateElement ( "username" );
        ele.InnerText = this.Username;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Password.GetPassword() ) ) {
        XmlElement ele = doc.CreateElement ( "password" );
        ele.InnerText = this.Password.GetPassword();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Executable ) ) {
        XmlElement ele = doc.CreateElement ( "executable" );
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.SourceSafeDirectory ) ) {
        XmlElement ele = doc.CreateElement ( "ssdir" );
        ele.InnerText = this.SourceSafeDirectory;
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        XmlElement ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString();
        root.AppendChild ( ele );
      }

      if ( this.ApplyLabel.HasValue ) {
        XmlElement ele = doc.CreateElement ( "applyLabel" );
        ele.InnerText = this.ApplyLabel.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        XmlElement ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );
      }
      // fix to workitem:13284
      if ( this.Timeout != null ) {
        XmlElement tele = this.Timeout.Serialize ( );
        if ( tele != null )
          root.AppendChild ( doc.ImportNode ( tele, true ) );
      }
      if ( !string.IsNullOrEmpty ( this.Culture ) ) {
        XmlElement ele = doc.CreateElement ( "culture" );
        ele.InnerText = this.Culture;
        root.AppendChild ( ele );
      }

      if ( this.CleanCopy.HasValue ) {
        XmlElement ele = doc.CreateElement ( "cleanCopy" );
        ele.InnerText = this.CleanCopy.Value.ToString ();
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.ApplyLabel = null;
      this.AutoGetSource = null;
      this.CleanCopy = null;
      this.Culture = string.Empty;
      this.Executable = string.Empty;
      this.Password = new HiddenPassword ();
      this.Project = string.Empty;
      this.SourceSafeDirectory = string.Empty;
      this.Timeout = new Timeout ();
      this.Username = string.Empty;
      this.WorkingDirectory = string.Empty;


      string s = Util.GetElementOrAttributeValue ( "applyLabel", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ApplyLabel = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "cleanCopy", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.CleanCopy = string.Compare ( s, bool.TrueString, true ) == 0;

      this.Culture = Util.GetElementOrAttributeValue ( "culture", element );
      this.Executable = Util.GetElementOrAttributeValue ( "executable", element );
      this.Password.Password = Util.GetElementOrAttributeValue ( "password", element );
      this.Project = Util.GetElementOrAttributeValue ( "project", element );
      this.SourceSafeDirectory = Util.GetElementOrAttributeValue ( "ssdir", element );
      this.Username = Util.GetElementOrAttributeValue ( "username", element );
      this.WorkingDirectory = Util.GetElementOrAttributeValue ( "workingDirectory", element );

      XmlElement ele = (XmlElement)element.SelectSingleNode ( "timeout" );
      this.Timeout.Deserialize ( ele );


    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Visual+Source+Safe+Source+Control+Block?decorator=printable" ); }
    }

    #endregion
  }
}
