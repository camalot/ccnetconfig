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
using System.Xml;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Because of dependencies on Team Foundation assemblies which cannot be shipped with the Cruise Control source, 
  /// this plugin is available as a standalone plugin from the <a href="http://vstsplugins.sourceforge.net/">VSTSPlugins</a> project on Sourceforge. The plugin has been recently 
  /// updated to work with version 1.0 of the Team Foundation Server.
  /// </summary>
  [ MinimumVersion( "1.0" ), Plugin ]
  public class VSTeamFoundationServerSourceControl : SourceControl, ICCNetDocumentation {
    private string _server = string.Empty;
    private string _userName = string.Empty;
    private HiddenPassword _password = new HiddenPassword ();
    private string _domain = string.Empty;
    private bool? _autoGetSource = null;
    private bool? _applyLabel = null;
    private string _workingDirectory = string.Empty;
    private bool? _cleanCopy = null;
    private string _workspace = string.Empty;
    private bool? _deleteWorkspace = null;
    private string _project = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="VSTeamFoundationServerSourceControl"/> class.
    /// </summary>
    public VSTeamFoundationServerSourceControl() : base("vsts") {}
    /// <summary>
    /// The name or URL of the team foundation server. For example
    /// <para>http://vstsb3:8080</para>
    /// or
    /// <para>vstsb3</para>
    /// if it has already been registered on the machine.
    /// </summary>
    [Description ("The name or URL of the team foundation server.\nFor example: http://vstsb3:8080 or vstsb3 if it has already been registered on the machine"),
   DefaultValue ( null ), DisplayName ( "(Server)" ), Category ( "Required" )]
    public string Server { get { return this._server; } set { this._server = Util.CheckRequired(this,"server",value); } }
    /// <summary>
    /// Username that CCNet should use to authenticate with Team Foundation Server
    /// </summary>
    [Description("Username that CCNet should use to authenticate with Team Foundation Server"),
   DefaultValue ( null ), Category ( "Optional" )]
    public string UserName { get { return this._userName; } set { this._userName = value; } }

    /// <summary>
    /// Password for the Team Foundation user
    /// </summary>
    [Description("Password for the Team Foundation user"),
   TypeConverter ( typeof ( PasswordTypeConverter ) ), DefaultValue ( null ), Category ( "Optional" )]
    public HiddenPassword Password { get { return this._password; } set { this._password = value; } }

    /// <summary>
    /// Domain for the Team Foundation user
    /// </summary>
    [Description ( "Domain for the Team Foundation user" ), DefaultValue ( null ), Category ( "Optional" )]
    public string Domain { get { return this._domain; } set { this._domain = value; } }

    /// <summary>
    /// Specifies if CCNet should automatically retrieve the latest version of the source from the repository
    /// </summary>
    [Description("Specifies if CCNet should automatically retrieve the latest version of the source from the repository"),
   DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ), 
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    
    /// <summary>
    /// Specifies if CCNet should apply the build label to the repository
    /// </summary>
    [Description("Specifies if CCNet should apply the build label to the repository"),
    DefaultValue (null), Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? ApplyLabel { get { return this._applyLabel; } set { this._applyLabel = value; } }

    /// <summary>
    /// The root folder where the latest source will retrieved the Team Foundation Server. 
    /// This path can either be absolute or it can be relative to the CCNet project working directory.
    /// </summary>
    [Description("The root folder where the latest source will retrieved the Team Foundation Server. This path can either be absolute or it can be relative to " +
      "the CCNet project working directory."),
      DefaultValue ( null ), Category ( "Optional" ),
      Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
      BrowseForFolderDescription ( "Select path to the working directory." )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }

    /// <summary>
    /// The root folder to be monitored by CCNet
    /// </summary>
    [Description ( "The root folder to be monitored by CCNet" ), DefaultValue ( null ), Category ( "Optional" )]
    public string Project { get { return this._project; } set { this._project = value; } }

    /// <summary>
    /// Determines is the working directory and all its contents should be deleted before the latest version of the source is downloaded
    /// </summary>
    [Description ("Determines is the working directory and all its contents should be deleted before the latest version of the source is downloaded"),
     DefaultValue (null), Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
     TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? CleanCopy { get { return this._cleanCopy; } set { this._cleanCopy = value; } }
    
    /// <summary>
    /// Determines if the workspace should be deleted after source is downloaded using the autoGetSource flag. It is much more efficient to leave the 
    /// workspace because that way the TFS server can remember the state of the files on the CruiseControl.NET server and only send files that have changed 
    /// or inform the server of deleted / renamed files.
    /// </summary>
    [Description ("Determines if the workspace should be deleted after source is downloaded using the autoGetSource flag. It is much more efficient to leave the " +
      "workspace because that way the TFS server can remember the state of the files on the CruiseControl.NET server and only send files that have changed or " + 
      "inform the server of deleted / renamed files."),
     DefaultValue (null), Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
     TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? DeleteWorkspace { get { return this._deleteWorkspace; } set { this._deleteWorkspace = value; } }
    
    /// <summary>
    /// The name of the Workspace under which the source should be retrieved. This workspace is created at the start of a download, and deleted at the 
    /// end. You can normally omit the property unless you want to name a workspace to avoid conflicts on the server (i.e. when you have multiple projects 
    /// on one server talking to a Team Foundation Server)
    /// </summary>
    [Description ("The name of the Workspace under which the source should be retrieved. This workspace is created at the start of a download, and deleted at " +
      "the end. You can normally omit the property unless you want to name a workspace to avoid conflicts on the server (i.e. when you have multiple projects " +
      "on one server talking to a Team Foundation Server)"),
     DefaultValue ( null ), Category ( "Optional" )]
    public string Workspace { get { return this._workspace; } set { this._workspace = value; } }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      VSTeamFoundationServerSourceControl vssc = this.MemberwiseClone () as VSTeamFoundationServerSourceControl;
      vssc.Password = this.Password.Clone ();
      return vssc;
    }

    #region Serialization
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize() {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ("sourcecontrol");
      root.SetAttribute ("type", this.TypeName);
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = doc.CreateElement ("server");
      ele.InnerText = Util.CheckRequired (this, ele.Name, this.Server);
      root.AppendChild (ele);

      if ( this.ApplyLabel.HasValue ) {
        ele = doc.CreateElement ( "applyLabel" );
        ele.InnerText = this.ApplyLabel.Value.ToString ( );
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty (this.UserName) ) {
        ele = doc.CreateElement ("username");
        ele.InnerText = this.UserName;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Password.ToString ()) ) {
        ele = doc.CreateElement ("password");
        ele.InnerText = this.Password.GetPassword ();
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Domain) ) {
        ele = doc.CreateElement ("domain");
        ele.InnerText = this.Domain;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.WorkingDirectory) ) {
        ele = doc.CreateElement ("workingDirectory");
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Workspace) ) {
        ele = doc.CreateElement ("workspace");
        ele.InnerText = this.Workspace;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Project) ) {
        ele = doc.CreateElement ("project");
        ele.InnerText = this.Project;
        root.AppendChild (ele);
      }

      if ( this.CleanCopy.HasValue ) {
        ele = doc.CreateElement ("cleanCopy");
        ele.InnerText = this.CleanCopy.Value.ToString ();
        root.AppendChild (ele);
      }

      if ( this.DeleteWorkspace.HasValue ) {
        ele = doc.CreateElement ("deleteWorkspace");
        ele.InnerText = this.DeleteWorkspace.Value.ToString ();
        root.AppendChild (ele);
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.ApplyLabel = null;
      this.AutoGetSource = null;
      this.CleanCopy = null;
      this.DeleteWorkspace = null;
      this.Domain = string.Empty;
      this.Password = new HiddenPassword ();
      this.Project = string.Empty;
      this._server = string.Empty;
      this.UserName = string.Empty;
      this.WorkingDirectory = string.Empty;
      this.Workspace = string.Empty;

      if ( string.Compare (element.GetAttribute ("type"), this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute ("type"), this.TypeName));

      this.Server = Util.GetElementOrAttributeValue ("server", element);

      string s = Util.GetElementOrAttributeValue ("applyLabel", element);
      if ( !string.IsNullOrEmpty (s) )
        this.ApplyLabel = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ("autoGetSource", element);
      if ( !string.IsNullOrEmpty (s) )
        this.AutoGetSource = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("cleanCopy", element);
      if ( !string.IsNullOrEmpty (s) )
        this.CleanCopy = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("deleteWorkspace", element);
      if ( !string.IsNullOrEmpty (s) )
        this.DeleteWorkspace = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("domain", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Domain = s;

      s = Util.GetElementOrAttributeValue ("password", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Password.Password = s;

      s = Util.GetElementOrAttributeValue ("project", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Project = s;

      s = Util.GetElementOrAttributeValue ("username", element);
      if ( !string.IsNullOrEmpty (s) )
        this.UserName = s;

      s = Util.GetElementOrAttributeValue ("workingDirectory", element);
      if ( !string.IsNullOrEmpty (s) )
        this.WorkingDirectory = s;

      s = Util.GetElementOrAttributeValue ("workspace", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Workspace = s;
    }
    #endregion

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri("http://confluence.public.thoughtworks.org/display/CCNET/Visual+Studio+Team+Foundation+Server+Plugin?decorator=printable"); }
    }

    #endregion

  }
}
