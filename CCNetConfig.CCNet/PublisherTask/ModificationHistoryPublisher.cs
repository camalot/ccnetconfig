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
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// This publisher logs all modifications for each build in a file.
  /// </summary>
  [ReflectorName ( "modificationHistory" ), MinimumVersion ( "1.3.0.2981" )]
  public class ModificationHistoryPublisher : PublisherTask, ICCNetDocumentation {
    /// <summary>
    /// Initializes a new instance of the <see cref="ModificationHistoryPublisher"/> class.
    /// </summary>
    public ModificationHistoryPublisher ( )
      : base ( "modificationHistory" ) {

    }

    /// <summary>
    /// When true, the history file will only be updated when the build contains modifications
    /// This setting is mainly for keeping the file small when there are a lot builds without modifications
    /// For example : like CCNet, there is a public website where everybody can force a build
    /// </summary>
    /// <value>The only log when changes found.</value>
    [ReflectorName ( "onlyLogWhenChangesFound" ), Category ( "Optional" ), DefaultValue ( null ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    Description ( "When true, the history file will only be updated when the build contains modifications. This setting is mainly for keeping the file small when there are a lot builds without modifications. For example : like CCNet, there is a public website where everybody can force a build" )]
    public bool? OnlyLogWhenChangesFound { get; set; }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<ModificationHistoryPublisher> ( ).Serialize ( this );
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      string s = Util.GetElementOrAttributeValue ( "onlyLogWhenChangesFound", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.OnlyLogWhenChangesFound = string.Compare ( s, bool.TrueString, true ) == 0;

    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      return this.MemberwiseClone as ModificationHistoryPublisher;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [ReflectorIgnore, Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
    public Uri DocumentationUri {
      get { throw new NotImplementedException ( ); }
    }

    #endregion
  }
}
