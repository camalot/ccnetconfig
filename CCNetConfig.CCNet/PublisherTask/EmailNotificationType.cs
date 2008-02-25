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
using CCNetConfig.Core;
using CCNetConfig.Core.Serialization;
using CCNetConfig.Core.Components;
using CCNetConfig.Core.Enums;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// 
  /// </summary>
  [ReflectorName("NotificationType"), MinimumVersion("1.4")]
  public class EmailNotificationType : ICCNetObject, ISerialize, ICloneable{
    // The reflector name attribute is needed to serialize this property, but since
    // its a value type, it doesn't matter what is in the name.
    /// <summary>
    /// The type of notification
    /// </summary>
    [ReflectorNodeType(ReflectorNodeTypes.Value),ReflectorName("value")]
    public NotificationType? Value { get; set; }
    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public System.Xml.XmlElement Serialize ( ) {
      return new Serializer<EmailNotificationType> ( ).Serialize ( this );
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public void Deserialize ( System.Xml.XmlElement element ) {
      try {
        string s = Util.GetElementOrAttributeValue ( ".", element );
        if ( Enum.IsDefined ( typeof ( NotificationType ), s ) )
          this.Value = ( NotificationType ) Enum.Parse ( typeof ( NotificationType ), s );
      } catch {
        throw;
      }
    }

    #endregion

    #region ICloneable Members

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    public EmailNotificationType Clone ( ) {
      return this.MemberwiseClone ( ) as EmailNotificationType;
    }
    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    object ICloneable.Clone ( ) {
      return this.Clone ( );
    }

    #endregion
  }
}
