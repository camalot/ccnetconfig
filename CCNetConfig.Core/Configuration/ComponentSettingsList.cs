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

namespace CCNetConfig.Core.Configuration {
  /// <summary>
  /// Collection of <see cref="ComponentSettings"/>
  /// </summary>
  public class ComponentSettingsList : List<ComponentSettings> {
    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentSettingsList"/> class.
    /// </summary>
    public ComponentSettingsList () {

    }
    /// <summary>
    /// Determines whether the object contains the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>
    /// 	<c>true</c> if the object contains the specified name.; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains ( string name ) {
      return IndexOf ( name ) > -1;
    }

    /// <summary>
    /// Gets the index of a Component or Plugin by name
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public int IndexOf ( string name ) {
      for ( int x = 0; x < this.Count; x++ ) {
        ComponentSettings pcs = this[x];
        if ( string.Compare ( pcs.Name, name, true ) == 0 )
          return x;
      }
      return -1;
    }

    /// <summary>
    /// Gets the <see cref="CCNetConfig.Core.Configuration.ComponentSettings"/> with the specified name.
    /// </summary>
    /// <value></value>
    public ComponentSettings this[string name] {
      get { return this.Contains ( name ) ? this[IndexOf ( name )] : null; }
      set {
        if ( this.Contains ( name ) )
          this[IndexOf ( name )] = value;
      }
    }
  }
}
