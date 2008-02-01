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
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// A dropdown that allows a user to enter multiple lines of strings, each line is then converted in to a collection.
  /// </summary>
  public class StringListUIEditor : UITypeEditor {
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
    public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value ) {
      frmsvr = (IWindowsFormsEditorService)provider.GetService (typeof (IWindowsFormsEditorService));
      if ( frmsvr != null ) {
        Panel panel = new Panel ();
        panel.Dock = DockStyle.Fill;
        Label lbl = new Label ();
        lbl.Text = "Enter Strings, 1 item per line:";
        lbl.AutoSize = true;
        lbl.Dock = DockStyle.Top;

        TextBox tb = new TextBox ();
        tb.Multiline = true;
        tb.ScrollBars = ScrollBars.Both;
        tb.AcceptsReturn = true;
        tb.Dock = DockStyle.Fill;
        panel.Controls.Add (tb);
        panel.Controls.Add (lbl);

        if ( value != null ) {
          List<string> lst = (List<string>)value;
          foreach(string s in lst)
            tb.AppendText(string.Format("{0}\r\n",s));
        }
        

        frmsvr.DropDownControl (panel);

        string result = tb.Text.Trim ();
        if ( !string.IsNullOrEmpty (result) ) {
          string[] array = result.Split (new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
          CloneableList<string> lst = new CloneableList<string> ();
          lst.AddRange (array);
          return lst;
        } else
          return value;

      }
      return value;
    }
    /// <summary>
    /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
    /// <returns>
    /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
    /// </returns>
    public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context ) {
      return UITypeEditorEditStyle.DropDown;
    }
  }

  /// <summary>
  /// Converts a string to a collection of strings and back to a string.
  /// </summary>
  public class StringListTypeConverter : TypeConverter {
    /// <summary>
    /// Returns whether this converter can convert the object to the specified type, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
      return destinationType == typeof(String);
    }

    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you want to convert from.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {
      return sourceType == typeof (String) || sourceType.GetInterface ("System.Collection.IList") != null;
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
      if ( value != null ) {
        if ( destinationType == typeof (String) ) {
          CloneableList<string> lst = (CloneableList<string>)value;
          StringBuilder sb = new StringBuilder();
          foreach ( string s in lst )
            sb.Append (string.Format ("{0};", s));
          if ( sb.Length > 0 )
            sb.Length = sb.Length - 1;
          return sb.ToString ();
        } else
          throw new NotSupportedException ( string.Format ( "Can not convert {0} to {1}", destinationType.ToString (), value.GetType ().ToString () ) );
      } else {
        return string.Empty;
      }
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
      if ( value != null ) {
        if ( value.GetType () == typeof (String) ) {
          string[] split = ( (string)value ).Split (new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
          CloneableList<string> lst = new CloneableList<string> ();
          lst.AddRange (split);
          return lst;
        } else
          return value;
      } else
        return value;
    }
  }
}
