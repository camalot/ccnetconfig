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
using System.ComponentModel.Design;
using System.ComponentModel;

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// 
  /// </summary>
  public class CollectionEditor : System.ComponentModel.Design.CollectionEditor {
    private CollectionForm _collectionForm;

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionEditor"/> class.
    /// </summary>
    /// <param name="type">The type of the collection for this editor to edit.</param>
    public CollectionEditor ( Type type ) : base ( type ) {

    }

    /// <summary>
    /// Edits the value.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="provider">The provider.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public override object EditValue ( ITypeDescriptorContext context, IServiceProvider provider, object value ) {
      /*if ( this._collectionForm != null && this._collectionForm.Visible ) {
        CollectionEditor editor = new CollectionEditor ( base.CollectionType );
        return editor.EditValue ( context, provider, value );
      } else*/ 
        return base.EditValue ( context, provider, value );
    }

    /// <summary>
    /// Creates a new form to display and edit the current collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.ComponentModel.Design.CollectionEditor.CollectionForm"></see> to provide as the user interface for editing the collection.
    /// </returns>
    protected override System.ComponentModel.Design.CollectionEditor.CollectionForm CreateCollectionForm () {
      this._collectionForm = base.CreateCollectionForm (); ;
      return this._collectionForm;
    }

  }
}
