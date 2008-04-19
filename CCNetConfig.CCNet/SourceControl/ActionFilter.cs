/* Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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
using System.ComponentModel;
using CCNetConfig.Core.Components;
using CCNetConfig.Core;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// An Action Filter
  /// </summary>
  [TypeConverter ( typeof ( ExpandableObjectConverter ) ), ReflectorName("actionFilter")]
  public class ActionFilter : Filter {
    /// <summary>
    /// Initializes a new instance of the <see cref="UserFilter"/> class.
    /// </summary>
    public ActionFilter ( )
      : base ( "actionFilter" ) {
      this.Actions = new CloneableList<string> ( );
    }

    /// <summary>
    /// This element consists of multiple name elements for each username to be filtered.
    /// </summary>
    /// <value>The names.</value>
    [Description ( "This element consists of multiple name elements for each username to be filtered." ), 
    DefaultValue ( null ), ReflectorArray("action"),ReflectorName("actions"),
    Editor ( typeof ( StringListUIEditor ), typeof ( UITypeEditor ) ), 
    TypeConverter ( typeof ( StringListTypeConverter ) )]
    public CloneableList<string> Actions { get; set; }

    #region Serialization

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override XmlElement Serialize ( ) {
      // TODO: implement serializer<T> for this class
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( this.TypeName );

      XmlElement namesEle = doc.CreateElement ( "actions" );
      if ( this.Actions != null && this.Actions.Count > 0 ) {
        foreach ( string n in this.Actions ) {
          XmlElement nameEle = doc.CreateElement ( "action" );
          nameEle.InnerText = n;
          namesEle.AppendChild ( nameEle );
        }
        root.AppendChild ( namesEle );
        return root;
      }
      return null;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      this.Actions = new CloneableList<string> ( );
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );
      XmlElement ele = ( XmlElement ) element.SelectSingleNode ( "actions" );
      if ( ele != null )
        foreach ( XmlElement tele in ele.SelectNodes ( "action" ) )
          this.Actions.Add ( tele.InnerText );
    }

    #endregion
  }
}
