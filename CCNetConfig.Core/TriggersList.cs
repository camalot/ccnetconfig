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
using System.Xml;

namespace CCNetConfig.Core {
  /// <summary>
  /// A collection of Triggers
  /// </summary>
  public class TriggersList: SerializableList<Trigger> {

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( "triggers" );
      foreach ( Trigger trigger in this )
        root.AppendChild ( doc.ImportNode ( trigger.Serialize ( ), true ) );
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      if ( string.Compare ( element.Name , "triggers", false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.GetType().Name ) );
      this.Clear ( );

      if ( element != null ) {
        foreach ( XmlElement trig in element.SelectNodes ( "./*" ) ) {
          Trigger trigger = Util.GetTriggerFromElement ( trig );
          if ( trigger != null )
            this.Add ( trigger );
        }
      }
    } 

    /// <summary>
    /// Clones this instance.
    /// </summary>
    /// <returns></returns>
    public new SerializableList<Trigger> Clone ( ) {
      TriggersList list = new TriggersList ( );
      foreach ( Trigger item in this ) {
        if ( typeof ( Trigger ).GetType ( ).GetInterface ( "System.ICloneable", true ) != null ) {
          ICloneable citem = item as ICloneable;
          if ( citem != null )
            list.Add ( ( Trigger ) citem.Clone ( ) );
          else
            list.Add ( ( Trigger ) citem );
        }
      }
      return list;
    }
  }
}
