/*
 * Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core.Components;
using CCNetConfig.Core;
using System.ComponentModel;

namespace CCNetConfig.CCNet {
  [MinimumVersion ( "1.4" ), ReflectorName ( "artifactcleanup" )]
  public class ArtifactCleanupTask : PublisherTask, ICCNetDocumentation {

    public ArtifactCleanupTask ( )
      : base ( "artifactcleanup" ) {

    }
    [ReflectorName ( "cleanUpValue" ), Category ( "Required" ), Required,
    DefaultValue ( null ), DisplayName ( "(Value)" ),
    Description ( "Defines the value for the clean up method." )]
    public int Value { get; set; }
    [ReflectorName ( "cleanUpMethod" ), Category ( "Required" ), Required,
    DisplayName ( "(Method)" ), DefaultValue ( null ),
    Description ( "Defines the procedure to use for cleaning up the artifact folder." )]
    public ArtifactCleanUpMethods Method { get; set; }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<ArtifactCleanupTask> ( ).Serialize ( this );
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.Method = Enum.Parse ( typeof ( ArtifactCleanUpMethods ), Util.GetElementOrAttributeValue ( "cleanUpMethod", element ) );
      int i = 0;
      int.TryParse ( Util.GetElementOrAttributeValue ( "cleanUpValue", element ), out i );
      this.Value = i;
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      return this.MemberwiseClone ( ) as ArtifactCleanupTask;
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [ReflectorIgnore, Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Artifact+Cleanup?decorator=printable" ); }
    }

    #endregion
  }
}
