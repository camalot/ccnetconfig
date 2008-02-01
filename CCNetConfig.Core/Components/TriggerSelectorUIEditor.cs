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
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// A dropdown of <see cref="CCNetConfig.Core.Trigger"/> objects.
  /// </summary>
  public class TriggerSelectorUIEditor : UITypeEditor {
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
        ListBox lst = new ListBox ();
        lst.BorderStyle = BorderStyle.None;
        lst.SelectedIndexChanged += new EventHandler ( OnListBoxChanged );
        List<TriggerInfo> li = new List<TriggerInfo> ();
        li.Add ( new TriggerInfo ( typeof ( string ), "(none)" ) );
        // TODO: Change to allow dynamic location
        foreach ( Type t in Util.Triggers ) {
          if ( ( Util.UserSettings.Components.Contains ( t.FullName ) && !Util.UserSettings.Components[t.FullName].Display ) )
            continue;

          Version minVer = Util.GetMinimumVersion ( t );
          Version maxVer = Util.GetMaximumVersion ( t );
          Version exactVer = Util.GetExactVersion ( t );
          if ( Util.IsInVersionRange ( minVer, maxVer, Util.CurrentConfigurationVersion ) || Util.IsExactVersion ( exactVer, Util.CurrentConfigurationVersion ) )
          li.Add ( new TriggerInfo ( t ) );
        }
        lst.DataSource = li;
        frmsvr.DropDownControl ( lst );
        if ( lst.SelectedItem != null ) {
          if ( ( (TriggerInfo)lst.SelectedItem ).Type == typeof ( String ) )
            return null;
          return Util.CreateInstanceOfType ( ( (TriggerInfo)lst.SelectedItem ).Type );
        } else
          return value;
      }
      return value;
    }

    /// <summary>
    /// Called when [list box changed].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OnListBoxChanged ( object sender, EventArgs e ) {
      frmsvr.CloseDropDown ();
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

    /// <summary>
    /// stores info about the trigger.
    /// </summary>
    internal class TriggerInfo {
      Type t = null;
      string text = string.Empty;
      /// <summary>
      /// Initializes a new instance of the <see cref="TriggerInfo"/> class.
      /// </summary>
      /// <param name="t">The t.</param>
      public TriggerInfo ( Type t )
        : this ( t, t.Name ) {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TriggerInfo"/> class.
      /// </summary>
      /// <param name="t">The t.</param>
      /// <param name="text">The text.</param>
      public TriggerInfo ( Type t, string text ) {
        this.t = t;
        this.text = text;
      }

      /// <summary>
      /// Gets the type.
      /// </summary>
      /// <value>The type.</value>
      public Type Type { get { return this.t; } }
      /// <summary>
      /// Gets the text.
      /// </summary>
      /// <value>The text.</value>
      public string Text { get { return this.text; } }

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return this.text;
      }
    }
  }

  /// <summary>
  /// Converts a collection of triggers to a string that shows that there are triggers in the collection.
  /// </summary>
  public class TriggerListTypeConverter : TypeConverter {
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
      if ( destinationType == typeof (String) ) {
        List<Trigger> trigs = (List<Trigger>)context.PropertyDescriptor.GetValue (context.Instance);
        if ( trigs != null && trigs.Count > 0 ) {
          return "(Triggers)";
        } else
          return string.Empty;
      } else
        throw new NotSupportedException ( string.Format ( "Unsupported type: {0}", destinationType.ToString () ) );
    }
  }
}
