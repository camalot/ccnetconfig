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
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// <para>he Xml Log Publisher is used to create the log files used by the 
  /// <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a> 
  /// <a href="http://confluence.public.thoughtworks.org/display/CCNET/Web+Dashboard">Web Dashboard</a>, so if you don't define an 
  /// <see cref="CCNetConfig.CCNet.XmlLogPublisher"/> section the <a href="http://confluence.public.thoughtworks.org/display/CCNET/Web+Dashboard">Dashboard</a> 
  /// will not function correctly.</para>
  /// </summary>
  /// <remarks>
  /// see <a href="http://confluence.public.thoughtworks.org/display/CCNET/Xml+Log+Publisher">Xml Log Publisher</a> documentation for more info.
  /// </remarks>
  [ MinimumVersion( "1.0" ) ]
  public class XmlLogPublisher : PublisherTask, ICCNetDocumentation {
    private string _logDir = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlLogPublisher"/> class.
    /// </summary>
    public XmlLogPublisher() : base ("xmllogger") { }

    /// <summary>
    /// The directory to save log files to. If relative, then relative to the 
    /// <see cref="CCNetConfig.Core.Project.ArtifactDirectory">Project Artifact Directory</see>
    /// </summary>
    [Description ( "The directory to save log files to. If relative, then relative to the Project Artifact Directory." ), DefaultValue ( null ),
   Category ( "Optional" ),
Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
BrowseForFolderDescription ( "Select path to the log file output directory." )]
    public string LogDirectory { get { return this._logDir; } set { this._logDir = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      return this.MemberwiseClone () as XmlLogPublisher;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      if ( !string.IsNullOrEmpty(LogDirectory) )
        root.SetAttribute ( "logDir", this.LogDirectory );
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.LogDirectory = string.Empty;

      if ( string.Compare (element.Name, this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.Name, this.TypeName));

      string s = Util.GetElementOrAttributeValue ("logDir", element);
      if ( !string.IsNullOrEmpty (s) )
        this.LogDirectory = s;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ("http://confluence.public.thoughtworks.org/display/CCNET/Xml+Log+Publisher?decorator=printable"); }
    }

    #endregion
  }
}
