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
using System.Xml.Serialization;
using CCNetConfig.Core.Serialization;
using CCNetConfig.Exceptions;
using System.Xml;
using System.IO;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.Core {
  /// <summary>
  /// defines all the configuration for one project running in a CruiseControl.NET server
  /// </summary>
  public class Project : ISerialize, ICCNetDocumentation, ICCNetObject, ICloneable {
    private string _name = string.Empty;
    private string _workingDirectory = string.Empty;
    private string _artifactDirectory = string.Empty;
    private Uri _webUrl = null;
    private int? _modificationDelaySeconds = null;
    private bool? _publishExceptions = null;
    private SourceControl _sourceControl = null;
    private TriggersList _triggers;
    private State _state = null;
    private Labeller _labeller = null;
    private TasksList _tasks;
    private PublishersList _publishers;
    private CloneableList<ExternalLink> _externalLinks;
    private PrebuildsList _prebuild;
    private string _category = string.Empty;
    private int? _queuePriority = null;
    private string _queueId = string.Empty;

    private CloneableList<ProjectExtension> _extensions;
    /// <summary>
    /// Initializes a new instance of the <see cref="Project"/> class.
    /// </summary>
    public Project () {
      _tasks = new TasksList ();
      _triggers = new TriggersList ( );
      _publishers = new PublishersList ( );
      _externalLinks = new CloneableList<ExternalLink> ();
      _extensions = new CloneableList<ProjectExtension> ();
      _prebuild = new PrebuildsList ( );

      foreach ( Type type in Util.ProjectExtensions )
        this.ProjectExtensions.Add ( (ProjectExtension)Util.CreateInstanceOfType ( type ) );
    }

    #region Public Properties
    /// <summary>
    /// <para>The name of your project - this must be unique for any given 
    /// <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a> server</para>
    /// </summary>
    [Description ( "The name of your project. This must be unique for any given CruiseControl.NET server" ),
    DisplayName ( "(Name)" ), Category ( "Required" ), DefaultValue ( null )]
    public string Name { get { return this._name; } set { this._name = Util.CheckRequired ( this, "name", value ); } }
    /// <summary>
    /// <para>The Working Directory for the project (this is used by other blocks). Relative paths are relative to a directory called the project Name in 
    /// the directory where the <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a> server was launched from. 
    /// The Working Directory is meant to contain the checked out version of the project under integration.</para>
    /// </summary>
    [Browsable ( true ), Description ( "The Working Directory for the project (this is used by other blocks). Relative paths are relative to a directory "
    + "called the project Name in the directory where the CruiseControl.NET server was launched from. The Working Directory is meant to contain the checked " +
     "out version of the project under integration." ), Category ( "Optional" ), DefaultValue ( null ),
   Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
  BrowseForFolderDescription ( "Select the Working Directory for the project" )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = value; } }
    /// <summary>
    /// <para>The Artifact Directory for the project (this is used by other blocks). Relative paths are relative to a directory called the project Name in the 
    /// directory where the <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a> server was launched from. 
    /// The Artifact Directory is meant to be a persistence location for anything you want saved from the results of the build, e.g. build logs, 
    /// distributables, etc.</para>
    /// </summary>
    [Description ( "The Artifact Directory for the project (this is used by other blocks). Relative paths are relative to a directory called the project Name " +
      "in the directory where the CruiseControl.NET server was launched from. The Artifact Directory is meant to be a persistence location for anything you " +
     "want saved from the results of the build, e.g. build logs, distributables, etc." ), Category ( "Optional" ),
     DefaultValue ( null ),
     Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
    BrowseForFolderDescription ( "Select the Artifact Directory for the project" )]
    public string ArtifactDirectory { get { return this._artifactDirectory; } set { this._artifactDirectory = value; } }
    /// <summary>
    /// <para>A reporting URL for this project. This is used by CCTray and the Email Publisher. Typically you should navigate to the Project Report on the 
    /// Dashboard, and use its URL, but make sure to replace any ampersands with &amp;amp;</para>
    /// </summary>
    [Description ( "A reporting URL for this project. This is used by CCTray and the Email Publisher. Typically you should navigate to the Project Report on the " +
   "Dashboard, and use its URL, but make sure to replace any ampersands with &amp;" ), Category ( "Optional" ), DefaultValue ( null )]
    public Uri WebUrl { get { return this._webUrl; } set { this._webUrl = value; } }
    /// <summary>
    /// <para>The minimum number of seconds allowed between the last check in and the start of a valid build.
    /// If any modifications are found within this interval the system will sleep long enough so the last checkin is just outside this interval. 
    /// For example if the modification delay is set to 10 seconds and the last checkin was 7 seconds ago the system will sleep for 3 seconds and check again. 
    /// This process will repeat until no modifications have been found within the modification delay window.
    /// This feature is in <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a> for Source Control systems, like CVS, 
    /// that do not support atomic checkins since starting a build half way through 
    /// someone checking in their work could result in invalid 'logical' passes or failures. The property is optional though so if you are using a source 
    /// control system with atomic checkins, leave it out (and it will default to '0')</para>
    /// </summary>
    [Description ( "The minimum number of seconds allowed between the last check in and the start of a valid build. " +
      "If any modifications are found within this interval the system will sleep long enough so the last checkin is just outside this interval. " +
      "For example if the modification delay is set to 10 seconds and the last checkin was 7 seconds ago the system will sleep for 3 seconds and check again.  " +
      "This process will repeat until no modifications have been found within the modification delay window. " +
      "This feature is in CruiseControl.NETfor Source Control systems, like CVS, " +
      "that do not support atomic checkins since starting a build half way through " +
      "someone checking in their work could result in invalid 'logical' passes or failures. The property is optional though so if you are using a source " +
     "control system with atomic checkins, leave it out (and it will default to '0')" ), Category ( "Optional" ), DefaultValue ( null )]
    public int? ModificationDelaySeconds { get { return this._modificationDelaySeconds; } set { this._modificationDelaySeconds = value; } }
    /// <summary>
    /// <para>If you are experiencing periodic failures in your build process it is possible to configure the 
    /// <a href="http://confluence.public.thoughtworks.org/display/CCNET">CCNet</a>
    /// server to not publish the exception as a failed build. Set this value to true to publish exceptions to the build log, or false to just send them to 
    /// the server log file</para>
    /// </summary>
    [Description ( "If you are experiencing periodic failures in your build process (such as sporadic " +
      "Visual Source Safe Source Control Block or CVS Source Control Block connection problems), it is possible to configure the " +
      "CCNet server to not publish the exception as a failed build. Set this value to true to publish exceptions to the build log, or false to just " +
      "send them to the server log file" ), DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
     TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? PublishExceptions { get { return this._publishExceptions; } set { this._publishExceptions = value; } }
    /// <summary>
    /// <para>The <see cref="CCNetConfig.Core.SourceControl">Source Control</see> used for this project</para>
    /// </summary>
    [Description ( "The SourceControl used for this project." ), Category ( "Optional" ), DefaultValue ( null )]
    public SourceControl SourceControl { get { return this._sourceControl; } set { this._sourceControl = value; } }
    /// <summary>
    /// <para>The <see cref="CCNetConfig.Core.Trigger">Trigger</see>s used for this project</para>
    /// </summary>
    [Browsable ( false )]
    public TriggersList Triggers { get { return this._triggers; } }
    /// <summary>
    /// <para>The <see cref="CCNetConfig.Core.State">State</see> manager used for this project</para>
    /// </summary>
    [Description ( "The StateManager used for this project." ), Category ( "Optional" ), DefaultValue ( null )]
    public State State { get { return this._state; } set { this._state = value; } }
    /// <summary>
    /// <para>The <see cref="CCNetConfig.Core.Labeller">Labeller</see> used for this project</para>
    /// </summary>
    [Description ( "The Labeller used for this project" ), Category ( "Optional" ), DefaultValue ( null )]
    public Labeller Labeller { get { return this._labeller; } set { this._labeller = value; } }
    /// <summary>
    /// <para>A set of <see cref="CCNetConfig.Core.PublisherTask">Task</see>s to run as part of the build. A failed task will fail the build and any 
    /// subsequent tasks will not run. 
    /// Tasks are run sequentially, in the order they appear in the configuration.</para>
    /// </summary>
    [Browsable ( false )]
    public TasksList Tasks { get { return this._tasks; } }
    /// <summary>
    /// <para>A set of <see cref="CCNetConfig.Core.PublisherTask">Task</see>s that are run after the build is complete. These tasks are used primarily to 
    /// clean up after the build and to publish and report on the build results. All tasks in this section will always run regardless of whether previous 
    /// tasks fail or the build is broken. You should always set an XmlLogPublisher in this section so that your 
    /// <a href="http://confluence.public.thoughtworks.org/display/CCNET/Web+Dashboard">Web Dashboard</a> 
    /// will be able to report results.</para>
    /// </summary>
    [Browsable ( false )]
    public PublishersList Publishers { get { return this._publishers; } }

    /// <summary>
    /// Gets the project extensions.
    /// </summary>
    /// <value>The project extensions.</value>
    [Browsable ( false )]
    public CloneableList<ProjectExtension> ProjectExtensions { get { return this._extensions; } }


    /// <summary>
    /// <para>The <see cref="CCNetConfig.Core.ExternalLink">ExternalLinks</see>s used for this project</para>
    /// </summary>
    [Description ( "A list of external links used as \"shortcuts\" to information related to this project." ), Category ( "Optional" ),
    DefaultValue ( null ), TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<ExternalLink> ExternalLinks { get { return this._externalLinks; } }

    /// <summary>
    /// A set of Tasks to run before the build starts and before the source is updated. A failed task will 
    /// fail the build and any subsequent tasks will not run. Tasks are run sequentially, in the order 
    /// they appear in the configuration.
    /// </summary>
    [ Browsable( false ), MinimumVersion( "1.1" ),
    Description( "A set of Tasks to run before the build starts and before the source is updated. " +
      "A failed task will fail the build and any subsequent tasks will not run. Tasks are run sequentially, " +
      "in the order they appear in the configuration." )]
    public PrebuildsList PreBuild { get { return this._prebuild; } }

    /// <summary>
    /// Gets or sets the queue priority.
    /// </summary>
    /// <value>The queue priority.</value>
    [Browsable(true), MinimumVersion("1.3"), DefaultValue(0), MinimumValue(0),
    Editor(typeof(NumericUpDownUIEditor),typeof(UITypeEditor))]
    public int? QueuePriority { get { return this._queuePriority; } set { this._queuePriority = value; } }
    /// <summary>
    /// Gets or sets the queue.
    /// </summary>
    /// <value>The queue.</value>
    [Browsable ( true ), MinimumVersion ( "1.3" ), DefaultValue(null)]
    public string Queue { get { return this._queueId; } set { this._queueId = value; } }

    /// <summary>
    /// A general category for this project. This will be used by CCTray and the dashboard in the 
    /// future to provide groupings to the project. The groupings can span servers in the farm.
    /// </summary>
    /// <value>The category.</value>
    [Description ( "A general category for this project. This will be used by CCTray and the dashboard in the " +
      "future to provide groupings to the project. The groupings can span servers in the farm." ),
    Category("Optional"),MinimumVersion("1.3"),DefaultValue(null)]
    public string Category {
      get { return this._category; }
      set { this._category = value; } 
    }

    #endregion

    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public XmlElement Serialize () {
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( this.GetType () );

      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "project" );
      root.SetAttribute ( "name", Util.CheckRequired ( this, "name", this.Name ) );

      // 1.3 introduced the queue and queue priority
      if ( versionInfo.CompareTo ( new Version ( "1.3" ) ) >= 0 ) {
        if ( !string.IsNullOrEmpty ( this.Queue ) )
          root.SetAttribute ( "queue", this.Queue );
        if(this.QueuePriority.HasValue )
          root.SetAttribute ( "queuePriority", this.QueuePriority.Value.ToString ( ) );
        XmlElement tele = doc.CreateElement ( "category" );
        tele.InnerText = this.Category;
      }

      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        XmlElement ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.ArtifactDirectory ) ) {
        XmlElement ele = doc.CreateElement ( "artifactDirectory" );
        ele.InnerText = this.ArtifactDirectory;
        root.AppendChild ( ele );
      }

      if ( this.WebUrl != null ) {
        XmlElement ele = doc.CreateElement ( "webURL" );
        ele.InnerText = this.WebUrl.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.ModificationDelaySeconds.HasValue ) {
        XmlElement ele = doc.CreateElement ( "modificationDelaySeconds" );
        ele.InnerText = this.ModificationDelaySeconds.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.PublishExceptions.HasValue ) {
        XmlElement ele = doc.CreateElement ( "publishExceptions" );
        ele.InnerText = this.PublishExceptions.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( SourceControl != null )
        root.AppendChild ( doc.ImportNode ( SourceControl.Serialize (), true ) );

      XmlElement triggersEle = this.Triggers.Serialize ( );
      root.AppendChild ( doc.ImportNode ( triggersEle,true ) );

      if ( this.Tasks.Count > 0 ) {
        XmlElement ele = this.Tasks.Serialize ( );
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele,true ) );
      }

      if ( this.Publishers.Count > 0 ) {
        XmlElement ele = this.Publishers.Serialize ( );
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }

      if ( versionInfo.CompareTo ( new Version ( "1.1" ) ) >= 0 ) {
        if ( this.PreBuild.Count > 0 ) {
          XmlElement ele = this.PreBuild.Serialize ( );
          if ( ele != null )
            root.AppendChild ( doc.ImportNode ( ele, true ) );
        }
      }

      if ( this.State != null )
        root.AppendChild ( doc.ImportNode ( this.State.Serialize (), true ) );

      if ( this.Labeller != null )
        root.AppendChild ( doc.ImportNode ( this.Labeller.Serialize (), true ) );

      if ( this.ExternalLinks.Count > 0 ) {
        XmlElement ele = doc.CreateElement ( "externalLinks" );
        foreach ( ExternalLink lnk in this.ExternalLinks )
          ele.AppendChild ( doc.ImportNode ( lnk.Serialize (), true ) );
        root.AppendChild ( ele );
      }

      // Extended items
      foreach ( ProjectExtension ipe in this._extensions ) {
        XmlElement ele = ipe.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }

      return root;
    }


    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public void Deserialize ( XmlElement element ) {
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( this.GetType () );

      this.ArtifactDirectory = string.Empty;
      this._externalLinks = new CloneableList<ExternalLink> ();
      this.Labeller = null;
      this.ModificationDelaySeconds = null;
      this._name = string.Empty;
      this.Publishers.Clear ();
      this.PublishExceptions = null;
      this.SourceControl = null;
      this.State = null;
      this.Tasks.Clear ();
      this.Triggers.Clear ();
      this.PreBuild.Clear ();
      this.WebUrl = null;
      this.WorkingDirectory = string.Empty;
      this._extensions = new CloneableList<ProjectExtension> ();
      this.QueuePriority = null;
      this.Queue = string.Empty;
      this.Category = string.Empty;

      if ( string.Compare ( element.Name, "project", false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to deserialize a project from a {0} element.", element.Name ) );

      this.Name = Util.GetElementOrAttributeValue ( "name", element );

      if ( versionInfo.CompareTo ( new Version ( "1.3" ) ) >= 0 ) {
        this.Queue = Util.GetElementOrAttributeValue ( "queue", element );
        int qp = 0;
        if ( int.TryParse ( Util.GetElementOrAttributeValue ( "queuePriority", element ), out qp ) )
          this.QueuePriority = qp;
        this.Category = Util.GetElementOrAttributeValue ( "category", element );
      }

      string s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.WorkingDirectory = s;

      s = Util.GetElementOrAttributeValue ( "artifactDirectory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ArtifactDirectory = s;

      s = Util.GetElementOrAttributeValue ( "webURL", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.WebUrl = new Uri ( s );

      s = Util.GetElementOrAttributeValue ( "modificationDelaySeconds", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int mds = 0;
        if ( int.TryParse ( s, out mds ) )
          this.ModificationDelaySeconds = mds;
      }

      s = Util.GetElementOrAttributeValue ( "publishExceptions", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.PublishExceptions = string.Compare ( s, bool.TrueString, true ) == 0;

      XmlElement ele = element.SelectSingleNode ( "triggers" ) as XmlElement;
      if ( ele != null ) {
        foreach ( XmlElement trig in ele.SelectNodes ( "./*" ) ) {
          Trigger trigger = Util.GetTriggerFromElement ( trig );
          if ( trigger != null )
            this.Triggers.Add ( trigger );
        }
      }

      ele = element.SelectSingleNode ( "sourcecontrol" ) as XmlElement;
      if ( ele != null ) {
        SourceControl sc = Util.GetSourceControlFromElement ( ele );
        if ( sc != null )
          this.SourceControl = sc;
      }

      ele = element.SelectSingleNode ( "labeller" ) as XmlElement;
      if ( ele != null ) {
        Labeller labeller = Util.GetLabellerFromElement ( ele );
        if ( labeller != null )
          this.Labeller = labeller;
      }

      ele = element.SelectSingleNode ( "state" ) as XmlElement;
      if ( ele != null ) {
        State state = Util.GetStateFromElement ( ele );
        this.State = state;
      }

      ele = element.SelectSingleNode ( "tasks" ) as XmlElement;
      if ( ele != null ) {
        foreach ( XmlElement t in ele.SelectNodes ( "./*" ) ) {
          PublisherTask task = Util.GetPublisherTaskFromElement ( t );
          if ( task != null )
            this.Tasks.Add ( task );
        }
      }

      ele = element.SelectSingleNode ( "publishers" ) as XmlElement;
      if ( ele != null ) {
        foreach ( XmlElement t in ele.SelectNodes ( "./*" ) ) {
          PublisherTask pub = Util.GetPublisherTaskFromElement ( t );
          if ( pub != null )
            this.Publishers.Add ( pub );
        }
      }

      ele = element.SelectSingleNode ( "preBuild" ) as XmlElement;
      if ( ele == null ) // reported that either are acceptable.
        ele = element.SelectSingleNode ( "prebuild" ) as XmlElement;

      if ( ele != null ) {
        foreach ( XmlElement t in ele.SelectNodes ( "./*" ) ) {
          PublisherTask task = Util.GetPublisherTaskFromElement ( t );
          if ( task != null )
            this.PreBuild.Add ( task );
        }
      }

      ele = element.SelectSingleNode ( "externalLinks" ) as XmlElement;
      if ( ele != null ) {
        foreach ( XmlElement eleUrl in ele.SelectNodes ( "externalLink" ) ) {
          ExternalLink el = new ExternalLink ();
          el.Deserialize ( eleUrl );
          this.ExternalLinks.Add ( el );
        }
      }

      foreach ( Type type in Util.ProjectExtensions )
        this.ProjectExtensions.Add ( (ProjectExtension)Util.CreateInstanceOfType ( type ) );
    }
    #endregion

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Project+Configuration+Block?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString () {
      return Name;
    }

    #region ICloneable Members
    /// <summary>
    /// Creates a copy of the project
    /// </summary>
    /// <returns>Copy of the project</returns>
    public Project Clone () {
      // TODO: the cloning of SerializableList Fails!!!
      // FIX: FIX THIS!
      Project project = this.MemberwiseClone () as Project;
      project._externalLinks = this.ExternalLinks.Clone ();
      project.Labeller = this.Labeller != null ? this.Labeller.Clone () : null;
      project._extensions = this.ProjectExtensions.Clone ();
      project._publishers = this.Publishers.Clone ( ) as PublishersList;
      project.SourceControl = this.SourceControl != null ? this.SourceControl.Clone () : null;
      project.State = this.State != null ? this.State.Clone () : null;
      project._prebuild = this.PreBuild.Clone () as PrebuildsList;
      project._tasks = this.Tasks.Clone ( ) as TasksList;
      project._triggers = this.Triggers.Clone () as TriggersList;
      if ( this.WebUrl != null )
        project.WebUrl = new Uri ( this.WebUrl.ToString () );
      return project;
    }

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    object ICloneable.Clone () {
      return this.Clone ();
    }

    #endregion
  }
}
