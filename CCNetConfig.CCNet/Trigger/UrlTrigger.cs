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
using System.Xml;
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Threading;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// <para>The <see cref="CCNetConfig.CCNet.UrlTrigger">Url Trigger</see> is used to trigger a CCNet build when the page at a particular <see cref="System.Uri">url</see> changes. 
  /// The <see cref="CCNetConfig.CCNet.UrlTrigger">Url Trigger</see> will poll the specified url according to a configured polling interval to detect 
  /// if the last modified date of the page has changed since the last integration.</para>
  /// <para>This <see cref="CCNetConfig.Core.Trigger">Trigger</see> is especially useful in reducing the load on your source control system caused by 
  /// the polling for modifications performed by an <see cref="CCNetConfig.CCNet.IntervalTrigger">Interval Trigger</see>. If your source control system 
  /// supports trigger scripts (such as the use of commitinfo scripts in CVS), you can use create a <see cref="CCNetConfig.Core.Trigger">Trigger</see> 
  /// to touch the page that is being monitored by CCNet to start a new integration.</para>
  /// </summary>
  /// <seealso cref="CCNetConfig.Core.Trigger"/>
  /// <seealso cref="CCNetConfig.CCNet.IntervalTrigger"/>
  /// <seealso cref="CCNetConfig.Core.Enums.BuildCondition"/>
  /// <seealso cref="System.Uri"/>
  [ MinimumVersion( "1.0" ) ]
  public class UrlTrigger : Trigger, ICCNetDocumentation {
    private Uri _url = null;
    private int? _seconds = null;
    private Core.Enums.BuildCondition? _buildCOndition = null;
    private string _name = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlTrigger"/> class.
    /// </summary>
    public UrlTrigger ()
      : base ( "urlTrigger" ) {

    }
    /// <summary>
    /// <para>The <see cref="System.Uri">url</see> to poll for changes.</para>
    /// </summary>
    [Description ( "The url to poll for changes." ), DefaultValue ( null ), DisplayName ( "(Url)" ), Category ( "Required" )]
    public Uri Url { get { return this._url; } set { this._url = Util.CheckRequired ( this, "url", value ); } }
    /// <summary>
    /// <para>The number of seconds after an integration cycle completes before triggering the next integration cycle.</para>
    /// </summary>
    [Description ( "The number of seconds after an intergration cycle completes before triggering the next intergration cycle." ),
    DefaultValue ( null ), Category ( "Optional" )]
    public int? Seconds { get { return this._seconds; } set { this._seconds = value; } }
    /// <summary>
    /// <para>The condition that should be used to launch the integration. By default, this value is 
    /// <see cref="CCNetConfig.Core.Enums.BuildCondition.IfModificationExists">IfModificationExists</see>, 
    /// meaning that an integration will only be triggered if modifications have been detected. 
    /// Set this attribute to <see cref="CCNetConfig.Core.Enums.BuildCondition.ForceBuild">ForceBuild</see> 
    /// in order to ensure that a build should be launched regardless of whether new modifications are detected.</para>
    /// </summary>
    [Description ( "The condition that should be used to launch the integration." ),
      Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ),
     TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ), DefaultValue ( null ), Category ( "Optional" )]
    public Core.Enums.BuildCondition? BuildCondition { get { return this._buildCOndition; } set { this._buildCOndition = value; } }
    /// <summary>
    /// <para>The name of the trigger. This name is passed to external tools as a means to identify the trigger that requested the build. (Added in CCNet 1.1)</para>
    /// </summary>
    [Description ( "The name of the Trigger. This name is passed to external tools as a means to identify the Trigger that requested the build. (Added in CCNet 1.1)" ),
   DefaultValue ( null ), Category ( "Optional" )]
    public string Name { get { return this._name; } set { this._name = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override Trigger Clone () {
      UrlTrigger ut = this.MemberwiseClone () as UrlTrigger;
      if ( this.Url != null )
        ut.Url = new Uri ( this.Url.ToString () );
      return ut;
    }

    /// <summary>
    /// Serializes the object to a <see cref="System.Xml.XmlElement" />
    /// </summary>
    /// <returns></returns>
    public override XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      root.SetAttribute ( "url", Util.CheckRequired ( this, "url", this.Url ).ToString () );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      if ( this.Seconds.HasValue )
        root.SetAttribute ( "seconds", this.Seconds.Value.ToString () );
      if ( this.BuildCondition.HasValue )
        root.SetAttribute ( "buildCondition", this.BuildCondition.Value.ToString () );
      if ( !string.IsNullOrEmpty ( this.Name ) )
        root.SetAttribute ( "name", this.Name );
      return root;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Url+Trigger?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      this.BuildCondition = null;
      this.Seconds = null;
      this.Name = string.Empty;
      this._url = null;

      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.Url = new Uri ( element.GetAttribute ( "url" ) );

      if ( !string.IsNullOrEmpty ( element.GetAttribute ( "name" ) ) )
        this.Name = element.GetAttribute ( "name" );

      if ( !string.IsNullOrEmpty ( element.GetAttribute ( "seconds" ) ) ) {
        int i = 0;
        if ( int.TryParse ( element.GetAttribute ( "seconds" ), out i ) )
          this.Seconds = i;
        else
          this.Seconds = null;
      }

      if ( !string.IsNullOrEmpty ( element.GetAttribute ( "buildCondition" ) ) ) {
        Core.Enums.BuildCondition bc = (Core.Enums.BuildCondition)Enum.Parse ( typeof ( Core.Enums.BuildCondition ), element.GetAttribute ( "buildCondition" ), true );
        this.BuildCondition = bc;
      }

    }
  }
}
