/* Copyright (c) 2006, Ryan Conrad. All rights reserved.
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

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// A <see cref="System.ComponentModel.TypeDescriptionProvider" /> used to display properties based on version.
  /// </summary>
  public class VersionBasedTypeDescriptionProvider : TypeDescriptionProvider {
    private TypeDescriptionProvider _baseProvider;
    private Version _version = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionBasedTypeDescriptionProvider"/> class.
    /// </summary>
    /// <param name="t">The t.</param>
    /// <param name="version">The version.</param>
    public VersionBasedTypeDescriptionProvider ( Type t, Version version ) {
      this._version = version;
      _baseProvider = TypeDescriptor.GetProvider ( t );
    }

    /// <summary>
    /// Gets a custom type descriptor for the given type and object.
    /// </summary>
    /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
    /// <param name="instance">An instance of the type. Can be null if no instance was passed to the <see cref="T:System.ComponentModel.TypeDescriptor"></see>.</param>
    /// <returns>
    /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor"></see> that can provide metadata for the type.
    /// </returns>
    public override ICustomTypeDescriptor GetTypeDescriptor ( Type objectType, object instance ) {
      return new VersionBasedTypeDescriptor ( this, _baseProvider.GetTypeDescriptor ( objectType, instance ),
        objectType );
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <value>The version.</value>
    public Version Version { get { return this._version; } }
  }
}
