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
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// A dropdown that allows the user to select a time
  /// </summary>
  public class TimeUIEditor : UITypeEditor {
    IWindowsFormsEditorService frmsvr = null;
    /// <summary>
    /// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> method.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
    /// <param name="provider">An <see cref="T:System.IServiceProvider"></see> that this editor can use to obtain services.</param>
    /// <param name="value">The object to edit.</param>
    /// <returns>
    /// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
    /// </returns>
    public override object EditValue ( ITypeDescriptorContext context, IServiceProvider provider, object value ) {
      frmsvr = (IWindowsFormsEditorService)provider.GetService ( typeof ( IWindowsFormsEditorService ) );
      if ( frmsvr != null ) {
        DateTimePicker dtp = new DateTimePicker ();
        dtp.Value = DateTime.Today.Add ( (TimeSpan)value );
        dtp.Format = DateTimePickerFormat.Custom;
        dtp.CustomFormat = "hh:mm:ss tt";
        dtp.ShowUpDown = true;
        dtp.TextChanged += new EventHandler ( dtp_TextChanged );
        
        frmsvr.DropDownControl ( dtp );
        return TimeSpan.Parse(string.Format ( "{0:D2}:{1:D2}:{2:D2}", dtp.Value.TimeOfDay.Hours, dtp.Value.TimeOfDay.Minutes, dtp.Value.TimeOfDay.Seconds) );
      }
      return value;
    }

    /// <summary>
    /// Handles the TextChanged event of the dtp control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void dtp_TextChanged ( object sender, EventArgs e ) {
      
    }


    /// <summary>
    /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
    /// <returns>
    /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
    /// </returns>
    public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context ) {
      return UITypeEditorEditStyle.DropDown;
    }
  }

  /// <summary>
  /// Converts a <see cref="System.TimeSpan"/> to a <see langword="string"/> and back.
  /// </summary>
  public class TimeTypeConverter : TypeConverter {
    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you want to convert from.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertFrom ( ITypeDescriptorContext context, Type sourceType ) {
      if ( sourceType == typeof ( TimeSpan ) )
        return true;
      else if ( sourceType == typeof ( DateTime ) )
        return true;
      else if ( sourceType == typeof ( String ) )
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
    public override bool CanConvertTo ( ITypeDescriptorContext context, Type destinationType ) {
      if ( destinationType == typeof ( TimeSpan ) )
        return true;
      else if ( destinationType == typeof ( DateTime ) )
        return true;
      else if ( destinationType == typeof ( String ) )
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
    public override object ConvertFrom ( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value ) {
      if ( value.GetType() == typeof ( TimeSpan ) )
        return value;
      else if ( value.GetType () == typeof ( DateTime ) )
        return ((DateTime)value).TimeOfDay;
      else if ( value.GetType () == typeof ( String ) ) {
        TimeSpan ts = TimeSpan.Zero;
        if ( TimeSpan.TryParse ( value.ToString (), out ts ) )
          return ts;
        else
          throw new NotSupportedException ( string.Format ( "Can not convert '{0}' to a TimeSpan", value.ToString ()));
      } else
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
    public override bool IsValid ( ITypeDescriptorContext context, object value ) {
      if ( value.GetType () == typeof ( TimeSpan ) )
        return true;
      else if ( value.GetType () == typeof ( DateTime ) )
        return true;
      else if ( value.GetType () == typeof ( String ) ) {
        TimeSpan ts = TimeSpan.Zero;
        return TimeSpan.TryParse ( value.ToString (), out ts );
      } else
        return false;
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString () {
      return base.ToString ();
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
      return base.ConvertTo ( context, culture, value, destinationType );
    }
  }
}
