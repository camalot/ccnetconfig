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
using CCNetConfig.Core.Serialization;
using System.Xml;
using System.ComponentModel;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core {
  /// <summary>
  /// Web Integration with ViewCVS and other CourceControl Types.
  /// </summary>
  [TypeConverter ( typeof ( ObjectOrNoneTypeConverter ) ), ReflectorName ( "webUrlBuilder" )]
  public class WebUrlBuilder : ISerialize, ICCNetObject, ICloneable {
    /// <summary>
    /// Initializes a new instance of the <see cref="WebUrlBuilder"/> class.
    /// </summary>
    public WebUrlBuilder ( ) { 
      this.Type = string.Empty;
    }
    /// <summary>
    /// Type of WebUrlBuilder to create.
    /// </summary>
    /// <value>The type.</value>
    [Description ( "Type of WebUrlBuilder to create." ), DefaultValue ( null ), Category ( "Optional" ),
    ReflectorName ( "type" ), ReflectorNodeType ( ReflectorNodeTypes.Attribute )]
    public string Type { get; set; }
    /// <summary>
    /// The url to link to.
    /// </summary>
    /// <value>The URL.</value>
    [Description ( "The url to link to." ), DefaultValue ( null ), DisplayName ( "(Url)" ),
    ReflectorName ( "url" ), Required, Category ( "Required" )]
    public Uri Url { get; set; }

    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public System.Xml.XmlElement Serialize ( ) {
      if ( this.Url != null && !string.IsNullOrEmpty ( this.Type ) )
        return new Serializer<WebUrlBuilder> ( ).Serialize ( this );
      else
        return null;
      /*
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "webUrlBuilder" );
      if ( !string.IsNullOrEmpty ( this.Type ) )
        root.SetAttribute ( "type", this.Type );

      if ( this.Url != null ) {
        XmlElement ele = doc.CreateElement ( "url" );
        ele.InnerText = this.Url.ToString ();
        root.AppendChild ( ele );
      }
      if ( root.SelectNodes ( "./*" ).Count > 0 )
        return root;
      else
        return null;
      */
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public void Deserialize ( System.Xml.XmlElement element ) {
      this.Url = null;
      this.Type = string.Empty;


      string s = Util.UrlDecode ( Util.GetElementOrAttributeValue ( "url", element ) );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Url = new Uri ( s );
      s = Util.GetElementOrAttributeValue ( "type", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Type = s;
    }

    #endregion
    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString ( ) {
      return this.GetType ( ).Name;
    }

    #region ICloneable Members
    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public WebUrlBuilder Clone ( ) {
      WebUrlBuilder wb = new WebUrlBuilder ( );
      if ( wb.Url != null )
        wb.Url = new Uri ( this.Url.ToString ( ) );
      return wb;
    }
    object ICloneable.Clone ( ) {
      return this.Clone ( );
    }

    #endregion
  }
}
