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
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// Provides a dropdown of all the <see cref="CCNetConfig.Core.SourceControl"/> objects that have been loaded.
  /// </summary>
  public class SourceControlUIEditor : UITypeEditor {
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
        List<SourceControlInfo> li = new List<SourceControlInfo> ();
        li.Add ( new SourceControlInfo ( typeof ( string ), "(none)" ) );
        foreach ( Type t in Util.SourceControls ) {

          if ( ( Util.UserSettings.Components.Contains ( t.FullName ) && !Util.UserSettings.Components[t.FullName].Display ) )
            continue;

          Version minVer = Util.GetMinimumVersion ( t );
          Version maxVer = Util.GetMaximumVersion ( t );
          Version exactVer = Util.GetExactVersion ( t );
          if ( Util.IsInVersionRange ( minVer, maxVer, Util.CurrentConfigurationVersion ) || Util.IsExactVersion ( exactVer, Util.CurrentConfigurationVersion ) ) {
            li.Add ( new SourceControlInfo ( t ) );
          }
        }
        lst.DataSource = li;
        frmsvr.DropDownControl ( lst );
        if ( lst.SelectedItem != null ) {
          if ( ( (SourceControlInfo)lst.SelectedItem ).Type == typeof ( String ) )
            return null;
          return Util.CreateInstanceOfType ( ( (SourceControlInfo)lst.SelectedItem ).Type );
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
    /// Stores info of the source control.
    /// </summary>
    internal class SourceControlInfo {
      Type t = null;
      string text = string.Empty;
      /// <summary>
      /// Initializes a new instance of the <see cref="SourceControlInfo"/> class.
      /// </summary>
      /// <param name="t">The t.</param>
      public SourceControlInfo ( Type t )
        : this ( t, t.Name ) {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SourceControlInfo"/> class.
      /// </summary>
      /// <param name="t">The t.</param>
      /// <param name="text">The text.</param>
      public SourceControlInfo ( Type t, string text ) {
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
}
