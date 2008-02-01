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
using System.ComponentModel;

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// A <see cref="System.ComponentModel.CustomTypeDescriptor" /> used to display properties 
  /// that are only related to a specific version.
  /// </summary>
  public class VersionBasedTypeDescriptor : CustomTypeDescriptor {
    private Type _objectType;
    private VersionBasedTypeDescriptionProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionBasedTypeDescriptor"/> class.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="descriptor">The descriptor.</param>
    /// <param name="objectType">Type of the object.</param>
    public VersionBasedTypeDescriptor ( VersionBasedTypeDescriptionProvider provider, 
        ICustomTypeDescriptor descriptor, Type objectType ) : base ( descriptor ) {
      if ( provider == null )
        throw new ArgumentNullException ( "provider" );
      if ( descriptor == null )
        throw new ArgumentNullException ( "descriptor" );
      if ( objectType == null )
        throw new ArgumentNullException ( "objectType" );
      _objectType = objectType;
      _provider = provider;
    }

    /// <summary>
    /// Returns a collection of property descriptors for the object represented by this type descriptor.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"></see> containing the property 
    /// descriptions for the object represented by this type descriptor. The default is 
    /// <see cref="F:System.ComponentModel.PropertyDescriptorCollection.Empty"></see>.
    /// </returns>
    public override PropertyDescriptorCollection GetProperties () {
      return GetProperties ( null );
    }

    /// <summary>
    /// Returns a filtered collection of property descriptors for the object represented by this type 
    /// descriptor.
    /// </summary>
    /// <param name="attributes">An array of attributes to use as a filter. This can be null.</param>
    /// <returns>
    /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"></see> containing the property 
    /// descriptions for the object represented by this type descriptor. The default is 
    /// <see cref="F:System.ComponentModel.PropertyDescriptorCollection.Empty"></see>.
    /// </returns>
    public override PropertyDescriptorCollection GetProperties ( Attribute[] attributes ) {
      PropertyDescriptorCollection props = new PropertyDescriptorCollection ( null );
      PropertyDescriptorCollection defaultProperties = base.GetProperties ( attributes ).Sort ();

      foreach ( PropertyDescriptor pd in defaultProperties ) {
        bool isValidVersion = true;
        MinimumVersionAttribute minVersion = pd.Attributes[typeof ( MinimumVersionAttribute )] as MinimumVersionAttribute;
        if ( minVersion != null ) {
          if ( minVersion.CompareTo ( _provider.Version ) <= -1 )
            isValidVersion = false;
        }
        MaximumVersionAttribute maxVersion = pd.Attributes[typeof ( MaximumVersionAttribute )] as MaximumVersionAttribute;
        if ( maxVersion != null ) {
          if ( maxVersion.CompareTo ( _provider.Version ) >= 1 )
            isValidVersion = false;
        }
        ExactVersionAttribute exactVersion = pd.Attributes[typeof ( ExactVersionAttribute )] as ExactVersionAttribute;
        if ( exactVersion != null ) {
          if ( exactVersion.CompareTo ( _provider.Version ) != 0 )
            isValidVersion = false;
        }

        // if the property is in the version range or does not have a specific version add it.
        if ( isValidVersion )
          props.Add ( pd );
      }

      return props;
    }
  }
}
