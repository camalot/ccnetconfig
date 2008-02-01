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
  /// An Converter that takes a string and converts it in to a <see cref="CCNetConfig.Core.HiddenPassword"/>
  /// </summary>
  public class PasswordTypeConverter : TypeConverter {
    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you want to convert from.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {
      if ( sourceType == typeof (String) )
        return true;
      else
        return false;
    }

    /// <summary>
    /// Returns whether this converter can convert the object to the specified type, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
      if ( destinationType == typeof (String) )
        return true;
      else
        return false;
    }

    /// <summary>
    /// Converts the given object to the type of this converter, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"></see> to use as the current culture.</param>
    /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
    /// <returns>
    /// An <see cref="T:System.Object"></see> that represents the converted value.
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
    public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value ) {
      if ( value != null && value.GetType () == typeof (String) ) {
        HiddenPassword pwd = new HiddenPassword ();
        pwd.Password = (string)value;
        return pwd;
      } else
        throw new NotSupportedException ( string.Format ( "Can not convert from {0} to {1}", value.GetType ().ToString (), typeof ( HiddenPassword ).ToString () ) );
    }

    /// <summary>
    /// Converts the given value object to the specified type, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.</param>
    /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
    /// <param name="destinationType">The <see cref="T:System.Type"></see> to convert the value parameter to.</param>
    /// <returns>
    /// An <see cref="T:System.Object"></see> that represents the converted value.
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
    /// <exception cref="T:System.ArgumentNullException">The destinationType parameter is null. </exception>
    public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType ) {
      if ( destinationType == typeof (String) && value != null )
        return ( (HiddenPassword)value ).Password;
      else
        return null;
    }

    /// <summary>
    /// Returns whether the given value object is valid for this type and for the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
    /// <returns>
    /// true if the specified value is valid for this object; otherwise, false.
    /// </returns>
    public override bool IsValid( ITypeDescriptorContext context, object value ) {
      if ( value == null || value.GetType () == typeof (string) || value.GetType () == typeof (HiddenPassword) )
        return true;
      else
        return false;
    }
  }
}
