/*
 * Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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
  /// Allows an property to either be an object or null.
  /// </summary>
  public class ObjectOrNoneUIEditor : UITypeEditor {
    /// <summary>
    /// 
    /// </summary>
    public static readonly NullValueEditorObject NULL_VALUE = new NullValueEditorObject ( "(None)" );
    /// <summary>
    /// 
    /// </summary>
    IWindowsFormsEditorService frmsvr = null;
    /// <summary>
    /// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> method.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
    /// <param name="provider">An <see cref="T:System.IServiceProvider"/> that this editor can use to obtain services.</param>
    /// <param name="value">The object to edit.</param>
    /// <returns>
    /// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
    /// </returns>
    public override object EditValue ( ITypeDescriptorContext context, IServiceProvider provider, object value ) {
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( PublisherTask ) );
      frmsvr = ( IWindowsFormsEditorService ) provider.GetService ( typeof ( IWindowsFormsEditorService ) );
      if ( frmsvr != null ) {
        ListBox lst = new ListBox ( );
        lst.BorderStyle = BorderStyle.None;
        lst.SelectedIndexChanged += new EventHandler ( OnListBoxChanged );
        List<object> li = new List<object> ( );
        li.Add ( ObjectOrNoneUIEditor.NULL_VALUE );
        Type objectType = context.PropertyDescriptor.PropertyType;

        Version min = Util.GetMinimumVersion ( objectType );
        Version max = Util.GetMaximumVersion ( objectType );
        Version exact = Util.GetExactVersion ( objectType );
        if ( Util.IsExactVersion ( exact, versionInfo ) || Util.IsInVersionRange ( min, max, versionInfo ) )
          li.Add ( objectType );

        lst.DataSource = li;

        frmsvr.DropDownControl ( lst );
        if ( lst.SelectedItem is Type )
          return Util.CreateInstanceOfType (lst.SelectedItem as Type);
        else
          return ObjectOrNoneUIEditor.NULL_VALUE.Value;
      }
      return value;

    }

    /// <summary>
    /// Called when [list box changed].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OnListBoxChanged ( object sender, EventArgs e ) {
      frmsvr.CloseDropDown ( );
    }

    /// <summary>
    /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/> method.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
    /// <returns>
    /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"/> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"/> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"/>.
    /// </returns>
    public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context ) {
      return UITypeEditorEditStyle.DropDown;
    }
  }
}
