/*
 * Copyright (c) 2006-2008, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core.Serialization;
using System.ComponentModel;
using CCNetConfig.Core.Components;

namespace CCNetConfig.CCNet {

  /// <summary>
  /// Defines who to send emails to
  /// </summary>
  [ReflectorName("user")]
  public class User : ISerialize, ICCNetObject, ICCNetDocumentation, ICloneable {
    private string _name = string.Empty;
    private string _group = string.Empty;
    private string _email = string.Empty;

    /// <summary>
    /// The user name of a user. For 'real' users, this should match the user name in Source Control.
    /// </summary>
    [Description ( "The user name of a user. For 'real' users, this should match the user name in Source Control." ),
   DefaultValue ( null ), DisplayName ( "(Name)" ), Category ( "Required" ), ReflectorName("name"),
    Required, ReflectorNodeType ( ReflectorNodeTypes.Attribute )]
    public string Name { get { return this._name; } set { this._name = Util.CheckRequired ( this, "name", value ); } }
    /// <summary>
    /// The group that the user is in.
    /// </summary>
    // TODO: need to find a way to get all the groups from the email publisher here...
    [Description ( "The group that the user is in." ), DefaultValue ( null ), Category ( "Optional" ),
    ReflectorName("group"), ReflectorNodeType(ReflectorNodeTypes.Attribute)]
    public string Group { get { return this._group; } set { this._group = value; } }
    /// <summary>
    /// The (internet form) email address of the user.
    /// </summary>
    [Description ( "The (internet form) email address of the user." ), DefaultValue ( null ), 
    DisplayName ( "(Address)" ), Category ( "Required" ), ReflectorNodeType(ReflectorNodeTypes.Attribute),
    ReflectorName("address"),Required ]
    public string Address { get { return this._email; } set { this._email = Util.CheckRequired ( this, "address", value ); } }


    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false ), ReflectorIgnore]
    public Uri DocumentationUri {
      get { return new Uri ( "http://ccnet.thoughtworks.net/display/CCNET/Email+Publisher?decorator=printable" ); }
    }

    #endregion

    #region ISerialize Members
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( "user" );
      root.SetAttribute ( "name", Util.CheckRequired ( this, "name", this.Name ) );
      root.SetAttribute ( "address", Util.CheckRequired ( this, "address", this.Address ) );
      if ( this.Group != null )
        root.SetAttribute ( "group", this.Group );

      return root;
    }


    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public void Deserialize ( XmlElement element ) {
      this._email = string.Empty;
      this._name = string.Empty;
      this.Group = string.Empty;

      if ( string.Compare ( element.Name, "user", false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, "user" ) );

      string s = Util.GetElementOrAttributeValue ( "name", element );
      this.Name = s;

      s = Util.GetElementOrAttributeValue ( "address", element );
      this.Address = s;

      s = Util.GetElementOrAttributeValue ( "group", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Group = s;
    }

    #endregion

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString ( ) {
      return string.IsNullOrEmpty ( this.Name ) ? "New User" : this.Name;
    }

    #region ICloneable Members
    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public User Clone ( ) {
      return this.MemberwiseClone ( ) as User;
    }

    object ICloneable.Clone ( ) {
      return this.Clone ( );
    }

    #endregion
  }
}
