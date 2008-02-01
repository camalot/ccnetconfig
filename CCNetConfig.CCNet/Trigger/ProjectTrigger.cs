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
using System.Xml;
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {

  /// <summary>
  /// <para>The <see cref="CCNetConfig.CCNet.ProjectTrigger">Project Trigger</see> is used to trigger a build when the specified dependent project has 
  /// completed its build. This <see cref="CCNetConfig.Core.Trigger">Trigger</see> can help you split your build process across projects and servers. 
  /// For example, you could have a CCNet project that will trigger the regression test suite once the main development build has completed successfully. 
  /// This dependent build could be running on either a local or a remote CCNet server.</para>
  /// <para>The <see cref="CCNetConfig.CCNet.ProjectTrigger">Project Trigger</see> works by using .NET remoting to poll the status of the dependent 
  /// project. Whenever it detects that the dependent project has completed a build, the <see cref="CCNetConfig.CCNet.ProjectTrigger">Project Trigger</see> 
  /// will fire. The <see cref="CCNetConfig.CCNet.ProjectTrigger">Project Trigger</see> can be configured to fire when the dependent project 
  /// build succeeded, failed or threw an exception. In order to avoid hammering the remote project through polling, the 
  /// <see cref="CCNetConfig.CCNet.ProjectTrigger">Project Trigger</see> is composed of an 
  /// <see cref="CCNetConfig.CCNet.IntervalTrigger">Interval Trigger</see> that will set a polling interval to 5 seconds. 
  /// This <see cref="CCNetConfig.CCNet.ProjectTrigger.InnerTrigger">inner trigger</see> can be adjusted through changing the configuration.</para>
  /// </summary>
  /// <seealso cref="CCNetConfig.CCNet.IntervalTrigger"/>
  /// <seealso cref="CCNetConfig.Core.Trigger"/>
  [ MinimumVersion( "1.0" ) ]
  public class ProjectTrigger : Trigger, ICCNetDocumentation {
    private string _project = string.Empty;
    private Core.Enums.IntegrationStatus? _triggerStatus = null;
    private Trigger _innerTrigger = null;
    private Uri _serverUri = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectTrigger"/> class.
    /// </summary>
    public ProjectTrigger ()
      : base ( "projectTrigger" ) {

    }

    /// <summary>
    /// The status of the dependent project that will be used to trigger the build. 
    /// For example, if this value is set to Success then a build will be triggered when the dependent project 
    /// completes a successful build.
    /// </summary>
    [Description ( "The status of the dependent project that will be used to trigger the build. For example, if this value is set to Success then a build will be triggered when the dependent project completes a successful build." ),
    Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ), DefaultValue ( null ), Category ( "Optional" )]
    public Core.Enums.IntegrationStatus? TriggerStatus { get { return this._triggerStatus; } set { this._triggerStatus = value; } }
    /// <summary>
    /// The name of the dependent project to trigger a build from.
    /// </summary>
    [Description ( "The name of the dependent project to trigger a build from." ),
   DefaultValue ( null ), DisplayName ( "(Project)" ), Category ( "Required" )]
    public string Project { get { return this._project; } set { this._project = Util.CheckRequired ( this, "project", value ); } }
    /// <summary>
    /// <para>The trigger used to modulate the polling interval for the ProjectTrigger.</para>
    /// </summary>
    [Description ( "The trigger used to modulate the polling interval for the ProjectTrigger." ),
      Editor ( typeof ( TriggerSelectorUIEditor ), typeof ( UITypeEditor ) ),
     TypeConverter ( typeof ( ExpandableObjectConverter ) ), DefaultValue ( null ), Category ( "Optional" )]
    public Trigger InnerTrigger { get { return this._innerTrigger; } set { this._innerTrigger = value; } }
    /// <summary>
    /// The <see cref="System.Uri">Uri</see> for the CCNet server containing the dependent project.
    /// </summary>
    [Description ( "The Uri for the CCNet server containing the dependent project." ), DefaultValue ( null ), Category ( "Optional" )]
    public Uri ServerUri { get { return this._serverUri; } set { this._serverUri = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override Trigger Clone () {
      ProjectTrigger pt = this.MemberwiseClone () as ProjectTrigger;
      pt.InnerTrigger = this.InnerTrigger.Clone ();
      if ( this.ServerUri != null )
        pt.ServerUri = new Uri ( this.ServerUri.ToString () );
      return pt;
    }

    /// <summary>
    /// Serializes the object to a <see cref="System.Xml.XmlElement" />
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      root.SetAttribute ( "project", Util.CheckRequired ( this, "project", this.Project ) );
      if ( this.ServerUri != null )
        root.SetAttribute ( "serverUri", this.ServerUri.ToString () );

      XmlElement ele = null;
      if ( this.TriggerStatus.HasValue ) {
        ele = doc.CreateElement ( "triggerStatus" );
        ele.InnerText = this.TriggerStatus.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( InnerTrigger != null ) {
        ele = doc.CreateElement ( "innerTrigger" );
        ele.SetAttribute ( "type", InnerTrigger.TypeName );
        XmlElement tele = InnerTrigger.Serialize ();
        foreach ( XmlAttribute attrib in tele.Attributes )
          ele.SetAttribute ( attrib.Name, attrib.InnerText );

        foreach ( XmlElement cele in tele.SelectNodes ( "./*" ) ) {
          XmlElement nele = (XmlElement)doc.ImportNode ( cele, true );
          ele.AppendChild ( nele );
        }
        root.AppendChild ( ele );
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
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Project+Trigger?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      this.InnerTrigger = null;
      this.ServerUri = null;
      this.TriggerStatus = null;

      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.Project = element.GetAttribute ( "project" );

      if ( !string.IsNullOrEmpty ( element.GetAttribute ( "serverUri" ) ) )
        this.ServerUri = new Uri ( element.GetAttribute ( "serverUri" ) );

      XmlElement ele = (XmlElement)element.SelectSingleNode ( "innerTrigger" );
      if ( ele != null ) {
        XmlElement nElement = element.OwnerDocument.CreateElement ( ele.GetAttribute ( "type" ) );
        foreach ( XmlAttribute attrib in ele.Attributes )
          nElement.SetAttribute ( attrib.Name, attrib.InnerText );
        this.InnerTrigger = Util.GetTriggerFromElement ( nElement );
      }

      // get the build condition
      ele = (XmlElement)element.SelectSingleNode ( "triggerStatus" );
      if ( ele != null ) {
        Core.Enums.IntegrationStatus bc = (Core.Enums.IntegrationStatus)Enum.Parse ( typeof ( Core.Enums.IntegrationStatus ), ele.InnerText, true );
        this.TriggerStatus = bc;
      }
    }

  }
}
