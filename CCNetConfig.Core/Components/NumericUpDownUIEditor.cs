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

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// 
  /// </summary>
  public class NumericUpDownUIEditor : UITypeEditor {
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
    public override object EditValue ( System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value ) {
      frmsvr = ( IWindowsFormsEditorService ) provider.GetService ( typeof ( IWindowsFormsEditorService ) );
      if ( frmsvr != null ) {
        System.Windows.Forms.NumericUpDown nud = new System.Windows.Forms.NumericUpDown ( );
        foreach ( Attribute attr in context.PropertyDescriptor.Attributes ) {
          if ( attr.GetType ( ) == typeof ( MinimumValueAttribute ) )
            nud.Minimum = ( int ) ( ( MinimumValueAttribute ) attr ).Value;
          else if ( attr.GetType ( ) == typeof ( MaximumValueAttribute ) )
            nud.Maximum = (int)( ( MaximumValueAttribute ) attr ).Value;
        }
        frmsvr.DropDownControl ( nud );
        return (int)nud.Value;
      }
      return value;
    }

    /// <summary>
    /// Gets the edit style.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context ) {
      return UITypeEditorEditStyle.DropDown;
    }
  }
}
