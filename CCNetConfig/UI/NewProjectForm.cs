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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CCNetConfig.UI {
  /// <summary>
  /// A Form that gets the name of the new project.
  /// </summary>
  public partial class NewProjectForm : Form {
    private string _projectName = string.Empty;
    /// <summary>
    /// Initializes a new instance of the <see cref="NewProjectForm"/> class.
    /// </summary>
    public NewProjectForm() {
      InitializeComponent ();
      this.StartPosition = FormStartPosition.CenterParent;
    }

    /// <summary>
    /// Gets the name of the project.
    /// </summary>
    /// <value>The name of the project.</value>
    public string ProjectName { get { return this._projectName; } }

    /// <summary>
    /// Handles the Click event of the OkButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OkButton_Click( object sender, EventArgs e ) {
      if ( !string.IsNullOrEmpty (this.projectNameTextBox.Text.Trim ()) ) {
        this._projectName = this.projectNameTextBox.Text.Trim ();
        this.DialogResult = DialogResult.OK;
        this.Close ();
      } else {
        this.errorProvider.SetIconAlignment (projectNameTextBox, ErrorIconAlignment.MiddleRight);
        this.errorProvider.SetIconPadding (projectNameTextBox, 4);
        this.errorProvider.SetError (this.projectNameTextBox, "Project Name can not be blank.");
      }
    }

    /// <summary>
    /// Handles the Click event of the cancelButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void cancelButton_Click( object sender, EventArgs e ) {

    }

    /// <summary>
    /// Handles the Load event of the NewProjectForm control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void NewProjectForm_Load ( object sender, EventArgs e ) {
      this.projectNameTextBox.Select ();
    }
  }
}