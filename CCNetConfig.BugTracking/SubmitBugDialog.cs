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
using System.IO;

namespace CCNetConfig.BugTracking {
  /// <summary>
  /// The dialog used for user submission of bugs.
  /// </summary>
  public partial class SubmitBugDialog : Form {
    private FileInfo attachedFile = null;
    BugTracker _bugTracker = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="SubmitBugDialog"/> class.
    /// </summary>
    /// <param name="bugTracker">The bug tracker.</param>
    internal SubmitBugDialog ( BugTracker bugTracker ) {
      InitializeComponent ( );
      this._bugTracker = bugTracker;
      this._bugTracker.SubmissionCompleted += new EventHandler ( _bugTracker_SubmissionCompleted );
    }

    /// <summary>
    /// Handles the SubmissionCompleted event of the _bugTracker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void _bugTracker_SubmissionCompleted ( object sender, EventArgs e ) {
      ToggleProgressBar ( false );

      this.DialogResult = DialogResult.OK;
      this.Close ( );
    }

    /// <summary>
    /// Handles the Click event of the attachButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void attachButton_Click ( object sender, EventArgs e ) {
      OpenFileDialog ofd = new OpenFileDialog ( );
      ofd.Title = "Select File to attach...";
      ofd.Filter = "CCNet Configuration File|*.config;*.xml";
      ofd.InitialDirectory = Environment.GetFolderPath ( Environment.SpecialFolder.Desktop );
      ofd.CheckFileExists = true;
      if ( ofd.ShowDialog ( this ) == DialogResult.OK ) {
        attachedFile = new FileInfo ( ofd.FileName );
        attachedFileTextBox.Text = attachedFile.FullName;
      } else {
        attachedFile = null;
        attachedFileTextBox.Text = string.Empty;
      }
    }

    /// <summary>
    /// Handles the Click event of the submitButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void submitButton_Click ( object sender, EventArgs e ) {
      if ( string.IsNullOrEmpty ( this.descriptionTextBox.Text.Trim ( ) ) )
        errorInfo.SetError ( lblDescription, "Description is required." );

      if ( string.IsNullOrEmpty ( this.detailsTextBox.Text.Trim ( ) ) )
        errorInfo.SetError ( lblDetail, "Detail is required." );
      StringBuilder dataFile = new StringBuilder ( );
      if ( attachedFile != null && attachedFile.Exists ) {
        FileStream fs = new FileStream ( attachedFile.FullName, FileMode.Open, FileAccess.Read );
        StreamReader reader = new StreamReader ( fs );
        using ( fs ) {
          using ( reader ) {
            while ( !reader.EndOfStream ) {
              dataFile.AppendLine ( reader.ReadLine ( ) );
            }
          }
        }
      }

      if ( string.IsNullOrEmpty ( this.detailsTextBox.Text ) || string.IsNullOrEmpty ( this.descriptionTextBox.Text ) )
        return;

      string data = string.Format ( "Contact: {0}\n\nStack Trace:\n{1}\n\nConfig File:\n{2}", this.textemail.Text.Trim ( ), this.detailsTextBox.Text.Trim ( ), dataFile.ToString ( ).Trim ( ) );
      ToggleProgressBar ( true );
      try {
        this._bugTracker.SubmitBug ( this.descriptionTextBox.Text.Trim ( ), data, null );
      } catch ( Exception ex ) {
        MessageBox.Show ( this, string.Format ( "Unable to submit bug.\n{0}", ex.Message ), "Error Submitting Bug", MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
    }

    /// <summary>
    /// Toggles the progress bar.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    private void ToggleProgressBar ( bool enabled ) {
      submissionStatusProgressBar.Enabled = enabled;
      submissionStatusProgressBar.Style = enabled ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
      submissionStatusProgressBar.Value = 0;
      submitButton.Enabled = !enabled;
      cancelButton.Enabled = !enabled;
      descriptionTextBox.Enabled = !enabled;
      detailsTextBox.Enabled = !enabled;
      textemail.Enabled = !enabled;
    }

  }
}