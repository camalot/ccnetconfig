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
using System.Xml;
using System.IO;

namespace CCNetConfig.BugTracking {
  public partial class SubmitException : Form {
    Exception _exception = null;
    BugTracker _bugTracker = null;
    XmlDocument _configDoc = null;
    FileInfo _loadedFile = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="SubmitException"/> class.
    /// </summary>
    /// <param name="bugTracker">The bug tracker.</param>
    /// <param name="ex">The ex.</param>
    public SubmitException ( BugTracker bugTracker, Exception ex ) {
      InitializeComponent ();
      this.Size = new Size ( 471, 152 );
      this._exception = ex;
      this.textdetails.Text = ex.ToString ();
      this.errorMessage.Text = string.Format ( "An error has occured in {0}.\n{1}\n\nDo you want to submit the error to the development group so this issue can be resolved?", ex.Source, ex.Message );
      this._bugTracker = bugTracker;
      this._bugTracker.SubmissionCompleted += new EventHandler ( _bugTracker_SubmissionCompleted );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubmitException"/> class.
    /// </summary>
    /// <param name="bugTracker">The bug tracker.</param>
    /// <param name="ex">The ex.</param>
    /// <param name="configDoc">The config doc.</param>
    public SubmitException ( BugTracker bugTracker, Exception ex, XmlDocument configDoc )
      : this ( bugTracker, ex ) {
      _configDoc = configDoc;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubmitException"/> class.
    /// </summary>
    /// <param name="bugTracker">The bug tracker.</param>
    /// <param name="ex">The ex.</param>
    /// <param name="loadedFile">The loaded file.</param>
    public SubmitException ( BugTracker bugTracker, Exception ex, FileInfo loadedFile )
      : this ( bugTracker, ex ) {
      _loadedFile = loadedFile;
    }

    /// <summary>
    /// Handles the Click event of the submitBug control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void submitBug_Click ( object sender, EventArgs e ) {
      this.Cursor = Cursors.WaitCursor;
      this.noSubmit.Enabled = false;
      this.submitBug.Enabled = false;
      string format = "{4}{3}{3}{5}{3}{3}{0}{3}{3}{1}{3}{3}{2}";
      StringBuilder loadedFileData = new StringBuilder ( );
      if ( this._loadedFile != null && this._loadedFile.Exists ) {
        try {
          StreamReader reader = this._loadedFile.OpenText ( );
          using ( reader ) {
            while ( !reader.EndOfStream ) {
              loadedFileData.AppendLine ( reader.ReadLine ( ) );
            }
          }
        } catch { }
      }
      string configXml = string.Empty;
      if ( _configDoc != null && includeConfig.Checked )
        configXml = _configDoc.OuterXml;
      string details = string.Format ( format, _exception.ToString ( ), configXml, 
        loadedFileData.ToString(), Environment.NewLine, Utils.GetOSInformation(), Utils.GetAssemblyInformation());
      this._bugTracker.SubmitBug ( _exception.Message, details, null );
    }

    /// <summary>
    /// Handles the Click event of the noSubmit control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void noSubmit_Click ( object sender, EventArgs e ) {
      this.DialogResult = DialogResult.Cancel;
      this.Close ();
    }

    /// <summary>
    /// Handles the SubmissionCompleted event of the _bugTracker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void _bugTracker_SubmissionCompleted ( object sender, EventArgs e ) {
      this.DialogResult = DialogResult.OK;
      this.Close ();
      this.noSubmit.Enabled = true;
      this.submitBug.Enabled = true;
      this.Cursor = Cursors.Default;
    }

    private void showDetails_Click ( object sender, EventArgs e ) {
			if ( this.Size != new Size ( 471, 177 ) ) {
				this.Size = new Size ( 471, 177 );
        this.showDetails.Text = "Show &Details";
        this.textdetails.Visible = false;
      } else {
				this.Size = new Size ( 471, 336 );
        this.showDetails.Text = "Hide &Details";
        this.textdetails.Visible = true;
      }
    }
  }
}