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
using CCNetConfig.Core.Components;
using CCNetConfig.Core.Serialization;
using System.ComponentModel;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Represents an environment variable used by the external source control.
  /// </summary>
  [ReflectorName ( "var" )]
  public class EnvironmentVariable : NameValue, ICCNetObject {

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [ReflectorIgnore, DisplayName("(Name)"), Category("Required")]
    public override string Name { get { return base.Name; } set { base.Name = value; } }
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    [ReflectorIgnore]
    public override string Value { get { return base.Value; } set { base.Value = value; } }

    /// <summary>
    /// Gets or sets the output.
    /// </summary>
    /// <value>The output.</value>
    [ReflectorNodeType(ReflectorNodeTypes.Value),ReflectorName("text"),
    EditorBrowsable ( EditorBrowsableState.Never ), Browsable ( false ), Required]
    public string Output {
      get {
        return string.Format ( "{0}={1}", this.Name, this.Value );
      }
      set {
        if ( string.IsNullOrEmpty ( value ) )
          throw new ArgumentNullException ( "value can not be null or empty" );
        this.StringToProperties ( value );
      }
    }

    private void StringToProperties ( string value ) {
      string[ ] vals = value.Split ( new char[ ] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries );
      this.Name = vals[ 0 ];
      if ( vals.Length > 1 ) {
        this.Value = vals[ 1 ];
      }
    }

    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public System.Xml.XmlElement Serialize ( ) {
      if ( string.IsNullOrEmpty ( this.Name ) )
        throw new ArgumentNullException ( "Name is required." );
      return new Serializer<EnvironmentVariable> ( ).Serialize ( this );
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public void Deserialize ( System.Xml.XmlElement element ) {
      this.Name = string.Empty;
      this.Value = string.Empty;

      string s = element.InnerText;
      if ( !string.IsNullOrEmpty ( s ) ) {
        this.StringToProperties ( s );
      }
    }

    #endregion
  }
}
