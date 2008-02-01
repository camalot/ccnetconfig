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
using CCNetConfig.Core.Serialization;
using CCNetConfig.Core.Enums;
using System.Xml;
using System.Drawing.Design;
using System.ComponentModel;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core {
  /// <summary>
  /// Represents a measurement of time.
  /// </summary>
  [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
  public class Timeout : ISerialize, ICCNetObject, ICCNetDocumentation, ICloneable {
    private TimeoutUnit? _unit = null;
    private int? _duration = null;
    /// <summary>
    /// The unit how the duration is measured.
    /// </summary>
    /// <value>The unit.</value>
    [Description ( "The unit how the duration is measured." ), DefaultValue(null),
    Editor(typeof(DefaultableEnumUIEditor),typeof(UITypeEditor)),
    TypeConverter(typeof(DefaultableEnumTypeConverter))]
    public TimeoutUnit? Unit { get { return this._unit; } set { this._unit = value; } }
    /// <summary>
    /// The number of units to wait for a timeout.
    /// </summary>
    /// <value>The duration.</value>
    [Description ( "The number of units to wait for a timeout." ), DefaultValue(null)]
    public int? Duration {
      get { return this._duration; }
      set {
        if ( value.HasValue && value.Value < 0 )
          throw new ArgumentException ( "Duration must be greater then zero." );
        else 
          this._duration = value;
      }
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString () {
      return string.Format ( "{0} {1}", this.Duration.ToString (), this.Unit.ToString () );
    }

    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public System.Xml.XmlElement Serialize () {
      if ( _duration.HasValue ) {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( "timeout" );
        if ( this.Unit.HasValue )
          root.SetAttribute ( "units", this.Unit.Value.ToString ().ToLower () );
        root.InnerText = _duration.Value.ToString ();
        return root;
      } else
        return null;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public void Deserialize( XmlElement element ) {
      this.Duration = null;
      this.Unit = null;
      if ( element != null ) {
        if ( string.Compare ( element.Name, "timeout", false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, "timeout" ) );

        string s = Util.GetElementOrAttributeValue ( "units", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Unit = (Core.Enums.TimeoutUnit)Enum.Parse ( typeof ( Core.Enums.TimeoutUnit ), s, true );

        int d = 0;
        if ( int.TryParse ( element.InnerText, out d ) )
          this.Duration = d;
      }
    }

    #endregion

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://ccnet.thoughtworks.net/display/CCNET/Timeout+Configuration?decorator=printable" ); }
    }

    #endregion



    #region ICloneable Members
    /// <summary>
    ///  creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public Timeout Clone () {
      /*Timeout t = new Timeout ();
      t.Duration = this.Duration;
      t.Unit = this.Unit;
      return t;*/
      return this.MemberwiseClone () as Timeout;
    }
    object ICloneable.Clone () {
      return this.Clone ();
    }

    #endregion
  }
}
