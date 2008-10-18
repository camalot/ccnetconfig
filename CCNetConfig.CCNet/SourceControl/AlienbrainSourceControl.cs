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
  /// Alienbrain Source Control Block
  /// </summary>
  /// <remarks>
  /// 
  /// <p>Current implementation of alienbrain use the following commands:</p>
  /// <table class="">
  /// <tbody><tr>
  /// <th class=""> Action 	</th>
  /// <th class=""> command</th>
  /// </tr>
  /// <tr>
  /// <td class=""> Branch 	</td>
  /// <td class=""> ab setactivebranch "Root Branch" -s "MyServer" -d "MyProject Database" -u "MyUsername" -p "MyPassword" </td>
  /// </tr>
  /// <tr>
  /// <td class=""> Modifications </td>
  /// <td class=""> ab find "ab://MyPath" -s "MyServer" -d
  /// "MyProject Database" -u "MyUsername" -p "MyPassword" -regex "SCIT &gt;
  /// 127771952139476549 AND SCIT &lt; 127771952139476550" -format
  /// "#CheckInComment#|#Name#|#DbPath#|#SCIT#|#Mime
  /// Type#|#LocalPath#|#Changed By#|#NxN_VersionNumber#" </td>
  /// </tr>
  /// <tr>
  /// <td class=""> Label 	</td>
  /// <td class=""> ab setlabel "ab://MyPath" -s "MyServer" -d
  /// "MyProject Database" -u "MyUsername" -p "MyPassword" -name
  /// "LabelnameprovidedbyCCNET" -comment "This label is brought to you by
  /// CruiseControl.NET" </td>
  /// </tr>
  /// <tr>
  /// <td class=""> Get Latest 	</td>
  /// <td class=""> ab getlatest "ab://MyPath" -s "MyServer" -d
  /// "MyProject Database" -u "MyUsername" -p "MyPassword" -localpath
  /// "Workingdir" -overwritewritable replace -overwritecheckedout replace
  /// -response:GetLatest.PathInvalid y -response:GetLatest.Writable y
  /// -response:GetLatest.CheckedOut y</td></tr></tbody></table>
  /// 
  /// see <a href="http://confluence.public.thoughtworks.org/display/CCNET/Alienbrain+Source+Control+Block">Alienbrain Source Control Block</a> for details.
  /// </remarks>
  [ MinimumVersion( "1.0" ) ]
  public class AlienbrainSourceControl : SourceControl, ICCNetDocumentation {
    private string _server = string.Empty;
    private string _database = string.Empty;
    private string _userName = string.Empty;
    private HiddenPassword _password = new HiddenPassword();
    private AlienbrainUri _project = null;
    private string _workingDirectory = null;
    private string _branch = string.Empty;
    private bool? _autoGetSource = null;
    private bool? _labelOnSuccess = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="AlienbrainSourceControl"/> class.
    /// </summary>
    public AlienbrainSourceControl () : base ( "alienbrain" ) { }
    /// <summary>
    /// Alienbrain server hostname or ip adress. The list of valid server name and ip adresses are listed in the File, Connect to project database, 
    /// Step 1, list box.
    /// </summary>
    [Description ( "Alienbrain server hostname or ip adress. The list of valid server name and ip adresses are listed in the File, Connect to project database, Step 1, list box." )]
    [DisplayName ( "(Server)" ), DefaultValue ( null ), Category ( "Required" )]
    public string Server { get { return this._server; } set { this._server = Util.CheckRequired ( this, "server", value ); } }
    /// <summary>
    /// Alienbrain project database name. The list of valid project databases are listed in the File, Connect to project database, Step 2, list box.
    /// </summary>
    [Description ( "Alienbrain project database name. The list of valid project databases are listed in the File, Connect to project database, Step 2, list box." )]
    [DisplayName ( "(Database)" ), DefaultValue ( null ), Category ( "Required" )]
    public string Database { get { return this._database; } set { this._database = Util.CheckRequired (this, "database", value); } }
    /// <summary>
    /// The name of the user you want to use to connect to the server project database
    /// </summary>
    [Description ( "The name of the user you want to use to connect to the server project database." )]
    [DisplayName ( "(Username)" ), DefaultValue ( null ), Category ( "Required" )]
    public string Username { get { return this._userName; } set { this._userName = Util.CheckRequired (this, "username", value); } }
    /// <summary>
    /// The password of the user you want to use to connect to the server project database
    /// </summary>
    [Description ( "The password of the user you want to use to connect to the server project database." ),
    DisplayName ( "(Password)" ), DefaultValue ( null ), Category ( "Required" ),
    TypeConverter(typeof(PasswordTypeConverter))]
    public HiddenPassword Password { get { return this._password; } set { this._password.Password = Util.CheckRequired (this, "password", value.GetPassword()); } }
    /// <summary>
    /// This is the path of to monitor the file changes. Use alienbrain://Code or ab://Code project path format
    /// </summary>
    [Description ( "This is the path of to monitor the file changes. Use alienbrain://Code or ab://Code project path format." ),
   DefaultValue ( null ), Category ( "Required" ),DisplayName("(Project)")]
    public AlienbrainUri Project { get { return this._project; } set { this._project = Util.CheckRequired ( this, "project", value ); } }
    /// <summary>
    /// The path where the get latest will update the files.
    /// </summary>
    [Description ("The path where the get latest will update the files."),
    DefaultValue ( null ), Category ( "Optional" ), 
    Editor(typeof(BrowseForFolderUIEditor),typeof(UITypeEditor)),
    BrowseForFolderDescription("Select the path where the get latest will update the files.")]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
    /// <summary>
    /// The path of the branch specification. to enumarate the name of the branches, use the ab enumbranch command line.
    /// </summary>
    [Description ("The path of the branch specification. to enumarate the name of the branches, use the ab enumbranch command line."),
   DefaultValue ( null ), Category ( "Optional" )]
    public string Branch { get { return this._branch; } set { this._branch = value; } }
    /// <summary>
    /// Specifies whether the current version of the source should be retrieved from Alienbrain.
    /// </summary>
    [Description ("Specifies whether the current version of the source should be retrieved from Alienbrain.Specifies whether the current version of the source should be retrieved from Alienbrain."),
    DefaultValue (null), Editor(typeof(DefaultableBooleanUIEditor),typeof(UITypeEditor)),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// Specifies whether or not the repository should be labelled after a successful build
    /// </summary>
    [Description ("Specifies whether or not the repository should be labelled after a successful build"),
    DefaultValue (null), Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? LabelOnSuccess { get { return this._labelOnSuccess; } set { this._labelOnSuccess = value; } }

    /// <summary>
    /// Creates a copy of the AlienbrainSourceControl
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      AlienbrainSourceControl asc = this.MemberwiseClone(  ) as AlienbrainSourceControl;
      asc.Password = this.Password.Clone(  );
      return asc;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = doc.CreateElement ( "server" );
      ele.InnerText = Util.CheckRequired ( this, "server", this.Server );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "database" );
      ele.InnerText = Util.CheckRequired ( this, "database", this.Database );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "username" );
      ele.InnerText = Util.CheckRequired ( this, "username", this.Username );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "password" );
      ele.InnerText = Util.CheckRequired ( this, "password", this.Password.GetPassword() );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "project" );
      ele.InnerText = Util.CheckRequired ( this, "project", this.Project.ToString () );
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty(this.WorkingDirectory) ) {
        ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Branch ) ) {
        ele = doc.CreateElement ( "branch" );
        ele.InnerText = this.Branch;
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.LabelOnSuccess.HasValue ) {
        ele = doc.CreateElement ( "labelOnSuccess" );
        ele.InnerText = this.LabelOnSuccess.Value.ToString ();
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.AutoGetSource = null;
      this.Branch = string.Empty;
      this._database = string.Empty;
      this._userName = string.Empty;
      this._password = new HiddenPassword ();
      this._project = null;
      this._server = string.Empty;
      this.LabelOnSuccess = null;
      this.WorkingDirectory = string.Empty;
      if ( string.Compare (element.GetAttribute("type"), this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute ("type"), this.TypeName));

      this.Server = Util.GetElementOrAttributeValue ("server", element);
      this.Database = Util.GetElementOrAttributeValue ("database", element);
      this.Username = Util.GetElementOrAttributeValue ("username", element);
      this.Password.Password = Util.GetElementOrAttributeValue ("password", element);
      this.Project = new AlienbrainUri(Util.GetElementOrAttributeValue ("project", element));

      string s = Util.GetElementOrAttributeValue ("autoGetSource", element);
      if ( !string.IsNullOrEmpty (s) )
        this.AutoGetSource = string.Compare (s, bool.TrueString, true) == 0;
      
      s = Util.GetElementOrAttributeValue("branch",element);
      if ( !string.IsNullOrEmpty (s) )
        this.Branch = s;

      s = Util.GetElementOrAttributeValue ("labelOnSuccess", element);
      if ( !string.IsNullOrEmpty (s) )
        this.LabelOnSuccess = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("workingDirectory", element);
      if ( !string.IsNullOrEmpty (s) )
        this.WorkingDirectory = s;

    }
    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Alienbrain+Source+Control+Block?decorator=printable" ); }
    }
    #endregion


  }
}
