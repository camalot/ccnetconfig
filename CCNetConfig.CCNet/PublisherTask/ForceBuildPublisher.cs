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

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The <see cref="CCNetConfig.CCNet.ForceBuildPublisher">ForceBuildPublisher</see> forces a build on a local or remote build server. 
  /// It uses .NET <see cref="System.Runtime.Remoting">Remoting</see> to invoke a forced build on the 
  /// <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a>
  /// server at the specified <see cref="System.Uri">Uri</see>. The forced build runs asynchronously, i.e. the 
  /// <see cref="CCNetConfig.CCNet.ForceBuildPublisher">ForceBuildPublisher</see> does not wait for the forced build to finish. The 
  /// <see cref="CCNetConfig.CCNet.ForceBuildPublisher">ForceBuildPublisher</see>is a great way to help split the build.
  /// For <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a> version 1.0 and later, an alternative to the 
  /// <see cref="CCNetConfig.CCNet.ForceBuildPublisher">ForceBuildPublisher</see> is the 
  /// <see cref="CCNetConfig.CCNet.ProjectTrigger">ProjectTrigger</see>. The main difference is that the 
  /// <see cref="CCNetConfig.CCNet.ForceBuildPublisher">ForceBuildPublisher</see>is placed in the configuration for the primary project, while the 
  /// <see cref="CCNetConfig.CCNet.ProjectTrigger">ProjectTrigger</see> is is placed in the configuration for the dependent project.
  /// </summary>
  /// <seealso cref="CCNetConfig.CCNet.ProjectTrigger" />
  /// <remarks>
  /// see CCNet <a href="http://confluence.public.thoughtworks.org/display/CCNET/ForceBuildPublisher" target="_blank">ForceBuildPublisher</a> documentation.
  /// </remarks>
  [MinimumVersion ( "1.0" )]
  public class ForceBuildPublisher : PublisherTask, ICCNetDocumentation {
    private string _project = string.Empty;
    private Uri _serverUri = null;
    private Core.Enums.IntegrationStatus? _integrationStatus = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForceBuildPublisher"/> class.
    /// </summary>
    public ForceBuildPublisher () : base ( "forcebuild" ) { }

    /// <summary>
    /// The CCNet project to force to build
    /// </summary>
    [Description ( "The CCNet project to force to build" ), DefaultValue ( null ), DisplayName ( "(Project)" ), Category ( "Required" )]
    public string Project { get { return this._project; } set { this._project = Util.CheckRequired ( this, "project", value ); } }
    /// <summary>
    /// The <see cref="System.Uri">Uri</see> for the local or remote server managing the project to build. 
    /// The default value is the default <see cref="System.Uri">Uri</see> for the local build server.
    /// </summary>
    [Description ( "The Uri for the local or remote server managing the project to build.\n The default value is the default Uri for the local build server." ),
   DefaultValue ( null ), Category ( "Optional" )]
    public Uri ServerUri { get { return this._serverUri; } set { this._serverUri = value; } }
    /// <summary>
    /// The condition determining whether or not the remoting call should be made.
    /// </summary>
    [Description ( "The condition determining whether or not the remoting call should be made." ), DefaultValue ( null ), Category ( "Optional" )]
    public Core.Enums.IntegrationStatus? IntergrationStatus { get { return this._integrationStatus; } set { this._integrationStatus = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      ForceBuildPublisher fbp = this.MemberwiseClone () as ForceBuildPublisher;
      if ( this.ServerUri != null )
        fbp.ServerUri = new Uri ( this.ServerUri.ToString () );
      return fbp;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      if ( this.IntergrationStatus.HasValue )
        root.SetAttribute ( "intergrationStatus", this.IntergrationStatus.Value.ToString () );

      XmlElement ele = doc.CreateElement ( "project" );
      ele.InnerText = Util.CheckRequired ( this, "project", this.Project );
      root.AppendChild ( ele );

      if ( this.ServerUri != null ) {
        ele = doc.CreateElement ( "serverUri" );
        ele.InnerText = this.ServerUri.ToString ();
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      this.IntergrationStatus = null;
      this._project = string.Empty;

      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.Project = Util.GetElementOrAttributeValue ( "project", element );

      string s = Util.GetElementOrAttributeValue ( "serverUri", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ServerUri = new Uri ( s );

      s = Util.GetElementOrAttributeValue ( "intergrationStatus", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.IntergrationStatus = (Core.Enums.IntegrationStatus)Enum.Parse ( typeof ( Core.Enums.IntegrationStatus ), s, true );
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/ForceBuildPublisher?decorator=printable" ); }
    }

    #endregion
  }
}
