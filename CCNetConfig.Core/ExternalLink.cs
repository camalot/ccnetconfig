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

namespace CCNetConfig.Core {
  /// <summary>
  /// <para>Each of ExternalLink is used to display project related links on the project report page of the 
  /// <a href="http://confluence.public.thoughtworks.org/display/CCNET/Web+Dashboard">Web Dashboard</a>, and are meant as a convenient 
  /// shortcut to project-related web sites outside of <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a>.</para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// see <a href="http://confluence.public.thoughtworks.org/display/CCNET/ExternalLinks">ExternalLinks</a> documentation for more details
  /// </para>
  /// </remarks>
  public class ExternalLink : ISerialize, ICCNetDocumentation, ICCNetObject, ICloneable {
    private string _name = string.Empty;
    private Uri _url = null;
    /// <summary>
    /// <para>The text to display for the link</para>
    /// </summary>
    [Category ( "Link" ), Description ( "The text to display for the link" ), DisplayName ( "(Name)" )]
    public string Name { get { return this._name; } set { this._name = Util.CheckRequired ( this, "name", value ); } }
    /// <summary>
    /// <para>The URL to link to</para>
    /// </summary>
    [Category ( "Link" ), Description ( "The URL to link to." )]
    public Uri Url { get { return this._url; } set { this._url = Util.CheckRequired ( this, "url", value ); } }

    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "externalLink" );
      root.SetAttribute ( "name", Util.CheckRequired ( this, "name", this.Name ) );
      root.SetAttribute ( "url", Util.CheckRequired ( this, "url", this.Url.ToString () ) );
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public void Deserialize ( XmlElement element ) {
      this._name = string.Empty;
      this._url = null;

      if ( string.Compare ( element.Name, "externalLink", false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, "externalLink" ) );
      this.Name = Util.GetElementOrAttributeValue ( "name", element );
      this.Url = new Uri ( Util.GetElementOrAttributeValue ( "url", element ) );
    }

    #endregion


    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString () {
      return this.Name == string.Empty ? "new external link" : this.Name;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/ExternalLinks?decorator=printable" ); }
    }

    #endregion

    #region ICloneable Members

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public ExternalLink Clone () {
      ExternalLink el = this.MemberwiseClone () as ExternalLink;
      if ( this.Url != null )
        el.Url = new Uri ( this.Url.ToString () );
      return el;
    }
    object ICloneable.Clone () {
      return this.Clone ();
    }

    #endregion
  }
}
