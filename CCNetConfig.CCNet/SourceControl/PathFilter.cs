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
using System.Xml;
using CCNetConfig.Core.Serialization;
using CCNetConfig.Core;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// A Path Filter
  /// </summary>
  [TypeConverter ( typeof ( ExpandableObjectConverter ) ), ReflectorName("pathFilter")]
  public class PathFilter : Filter {
    /// <summary>
    /// Initializes a new instance of the <see cref="PathFilter"/> class.
    /// </summary>
    public PathFilter ( ) : base ( "pathFilter" ) { }

    /// <summary>
    /// This is the pattern used to compare the modification path against. The pattern should match the path of the files in the repository 
    /// (not the path of the files in the working directory). See below for examples of the syntax for this element. Each PathFilter contains a 
    /// single pattern element.
    /// </summary>
    /// <value>The pattern.</value>
    [Description ( "This is the pattern used to compare the modification path against. The pattern should match the path of the files in the repository" +
    " (not the path of the files in the working directory). See below for examples of the syntax for this element. Each PathFilter contains a single" +
    " pattern element." ), DefaultValue ( null ), DisplayName ( "(Pattern)" ),Required,
    ReflectorName("pattern")]
    public string Pattern { get; set; }

    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<PathFilter> ( ).Serialize ( this );

      /*XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( this.TypeName );
      XmlElement pattern = doc.CreateElement ( "pattern" );
      pattern.InnerText = Util.CheckRequired ( this, pattern.Name, this.Pattern );
      root.AppendChild ( pattern );
      return root;*/
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.Pattern = string.Empty;
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );
      this.Pattern = Util.GetElementOrAttributeValue ( "pattern", element );
    }

    #endregion
  }
}
