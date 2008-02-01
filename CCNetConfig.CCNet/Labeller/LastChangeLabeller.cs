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
using CCNetConfig.Core;
using System.ComponentModel;
using System.Xml;
using CCNetConfig.Core.Components;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Some source control systems (e.g., AccuRev) have a concept of a "change number", which the 
  /// Last Change Labeller can use to build a label. The following configuration would prefix all 
  /// labels with the string 'Foo-1-', so the build of change number 213 would be labelled 'Foo-1-213'
  /// </summary>
  [TypeConverter ( typeof ( ExpandableObjectConverter ) ),
  MinimumVersion ( "1.3" )]
  public class LastChangeLabeller : Labeller, ICCNetDocumentation {
    private string _prefix = string.Empty;
    /// <summary>
    /// Initializes a new instance of the <see cref="LastChangeLabeller"/> class.
    /// </summary>
    public LastChangeLabeller ()
      : base ( "lastChangeLabeller" ) {

    }

    /// <summary>
    /// Any string to be put in front of the change number.
    /// </summary>
    [Description ( "Any string to be put in front of the change number." ), 
    DefaultValue ( null ), Category ( "Optional" )]
    public string Prefix { get { return this._prefix; } set { this._prefix = value; } }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "labeller" );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      root.SetAttribute ( "type", this.TypeName );
      XmlElement ele = null;

      if ( !string.IsNullOrEmpty ( this.Prefix ) ) {
        ele = doc.CreateElement ( "prefix" );
        ele.InnerText = this.Prefix;
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.Prefix = string.Empty;
      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

      string s = Util.GetElementOrAttributeValue ( "prefix", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Prefix = s;
    }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone () {
      return this.MemberwiseClone () as LastChangeLabeller;
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [ Browsable( false ) ]
    public Uri DocumentationUri {
      get { return new Uri( "http://confluence.public.thoughtworks.org/display/CCNET/Last+Change+Labeller?decorator=printable" ); }
    }

    #endregion
  }
}
