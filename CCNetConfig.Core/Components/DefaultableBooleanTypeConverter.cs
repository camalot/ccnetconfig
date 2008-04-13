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

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// Converts the nullable boolean to a string and back.
  /// </summary>
  public class DefaultableBooleanTypeConverter : TypeConverter {
    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you want to convert from.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertFrom ( ITypeDescriptorContext context, Type sourceType ) {
      if ( sourceType == typeof ( String ) || sourceType == typeof(NullValueEditorObject) )
        return true;
      else
        return base.CanConvertFrom ( context, sourceType );
    }

    /// <summary>
    /// Returns whether this converter can convert the object to the specified type, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertTo ( ITypeDescriptorContext context, Type destinationType ) {
      return destinationType == typeof ( string ) || destinationType == typeof ( bool );
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
    public override object ConvertTo ( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType ) {
      if ( destinationType == typeof ( String ) ) {
        if ( context == null || context.PropertyDescriptor.GetValue ( context.Instance ) == null )
          return DefaultableBooleanUIEditor.NULL_VALUE.Name;
        else
          return context.PropertyDescriptor.GetValue ( context.Instance ).ToString ( );
      } else
        throw new NotSupportedException ( string.Format ( "Unsupported type: {0}", destinationType.ToString ( ) ) );
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
    public override object ConvertFrom ( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value ) {
      if ( value == null )
        return DefaultableBooleanUIEditor.NULL_VALUE.Value;
      if ( value.GetType ( ) == typeof ( String ) ) {
        string val = ( string ) value;
        if ( string.Compare ( val, DefaultableBooleanUIEditor.NULL_VALUE.Name, 0 ) == 0 )
          return DefaultableBooleanUIEditor.NULL_VALUE.Value;
        else {
          bool bln = false;
          if ( bool.TryParse ( val, out bln ) )
            return bln;
          else
            return DefaultableBooleanUIEditor.NULL_VALUE.Value;
        }
      } else
        throw new NotSupportedException ( string.Format ( "Unsupported type: {0}", value.GetType ( ).ToString ( ) ) );
    }

    /// <summary>
    /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <returns>
    /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> should be called to find a common set of values the object supports; otherwise, false.
    /// </returns>
    public override bool GetStandardValuesSupported ( ITypeDescriptorContext context ) {
      return true;
    }

    /// <summary>
    /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
    /// <returns>
    /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
    /// </returns>
    public override StandardValuesCollection GetStandardValues ( ITypeDescriptorContext context ) {
      List<object> vals = new List<object> ( );
      vals.Add ( DefaultableBooleanUIEditor.NULL_VALUE.Value );
      vals.Add ( true );
      vals.Add ( false );
      return new StandardValuesCollection ( vals );
    }

    /// <summary>
    /// Returns whether the given value object is valid for this type and for the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
    /// <returns>
    /// true if the specified value is valid for this object; otherwise, false.
    /// </returns>
    public override bool IsValid ( ITypeDescriptorContext context, object value ) {
      if ( value == null )
        return true;
      else if ( value.GetType ( ) == typeof ( String ) ) {
        string val = ( string ) value;
        if ( string.Compare ( val, DefaultableBooleanUIEditor.NULL_VALUE.Name, 0 ) == 0 || string.Compare ( val, bool.TrueString, true ) == 0 ||
            string.Compare ( val, bool.FalseString, true ) == 0 )
          return true;
        else
          return false;
      } else if ( value.GetType ( ) == typeof ( bool ) )
        return true;
      else
        return false;
    }
  }
}
