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
using CCNetConfig.Updater.Core;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace CCNetConfig.Updater {
  public partial class MainForm : Form {
    ApplicationUpdater au = null;
    private delegate void GenericDelegate ( );
    private delegate void UpdateLabel ( string text );
    private delegate void UpdateProgressBar ( int value );
    private delegate void ExceptionMessageDelegate ( Exception ex );
    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm ( ) {
      InitializeComponent ( );
      this.progressBar1.Maximum = 100;
      this.progressBar1.Minimum = 0;
      this.progressBar1.Value = 0;

      au = new ApplicationUpdater ( );
			au.Version = Program.ProductVersion;
      if ( !string.IsNullOrEmpty ( Program.CallingApplication ) )
        au.OwnerApplication = new FileInfo ( Program.CallingApplication );

      au.UpdateAvailable += new EventHandler<UpdatesAvailableEventArgs> ( au_UpdateAvailable );
      au.UpdateException += new EventHandler<ExceptionEventArgs> ( au_UpdateException );
      au.DownloadProgressChanged += new EventHandler<DownloadProgressEventArgs> ( au_DownloadProgressChanged );
      au.UpdateScriptCreated += new EventHandler ( au_UpdateScriptCreated );
      au.UpdateException += new EventHandler<ExceptionEventArgs> ( au_UpdateException );
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Load"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnLoad ( EventArgs e ) {
      base.OnLoad ( e );
			if ( Program.UpdateUrl != null ) {
				au.CheckForUpdatesByUrl ( Program.UpdateUrl );
			} else {
				au.CheckForUpdate ( Program.UpdateCheckType );
			}
    }

    /// <summary>
    /// Handles the UpdateScriptCreated event of the au control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void au_UpdateScriptCreated ( object sender, EventArgs e ) {
      if ( au.ScriptFile.Exists )
        System.Diagnostics.Process.Start ( au.ScriptFile.FullName );
      else
        MessageBox.Show ( string.Format ( "Error running update script.\n\nCould not find file {0}", au.ScriptFile.FullName ), "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1 );
      if ( this.InvokeRequired )
        this.Invoke ( new GenericDelegate ( this.Close ), new object[ ] { } );
      else
        this.Close ( );

    }

    /// <summary>
    /// Handles the DownloadProgressChanged event of the au control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="CCNetConfig.Updater.Core.DownloadProgressEventArgs"/> instance containing the event data.</param>
    void au_DownloadProgressChanged ( object sender, DownloadProgressEventArgs e ) {
      string fileName = e.UpdateFile.Location.ToString ( );
      fileName = fileName.Substring ( fileName.LastIndexOf ( "/" ) );
      if ( this.InvokeRequired ) {
				this.Invoke ( new UpdateLabel ( this.UpdateStatusLabel ), new object[] { string.Format ( "Downloading {0} : {1}%", Program.GetUpdateVersion, e.Percentage.ToString () ) } );
        this.Invoke ( new UpdateProgressBar ( this.UpdateStatusProgressBar ), new object[ ] { e.Percentage } );
      } else {
        this.UpdateStatusLabel ( string.Format ( "Downloading {0} : {1}%", Program.GetUpdateVersion, e.Percentage.ToString ( ) ) );
        this.UpdateStatusProgressBar ( e.Percentage );
      }
    }


    /// <summary>
    /// Handles the UpdateException event of the au control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="CCNetConfig.Updater.Core.ExceptionEventArgs"/> instance containing the event data.</param>
    void au_UpdateException ( object sender, ExceptionEventArgs e ) {
      //Console.WriteLine ( e.Exception.ToString () );
      if ( this.InvokeRequired )
        this.Invoke ( new ExceptionMessageDelegate ( this.ShowExceptionMessage ), new object[ ] { e.Exception } );
      else
        ShowExceptionMessage ( e.Exception );
    }

    /// <summary>
    /// Shows the exception message.
    /// </summary>
    /// <param name="ex">The ex.</param>
    private void ShowExceptionMessage ( Exception ex ) {
      MessageBox.Show ( this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );

    }

    /// <summary>
    /// Handles the UpdateAvailable event of the au control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void au_UpdateAvailable ( object sender, UpdatesAvailableEventArgs e ) {
      Version ver = au.UpdateInfoList.GetLatestVersion ( );
      Thread t = new Thread ( new ParameterizedThreadStart ( GetUpdates ) );
      t.Start ( ver );
    }

    /// <summary>
    /// Gets the updates.
    /// </summary>
    /// <param name="version">The version.</param>
    void GetUpdates ( object version ) {
      if ( version.GetType ( ) == typeof ( Version ) )
        au.GetUpdates ( version as Version );
    }

    /// <summary>
    /// Updates the status label.
    /// </summary>
    /// <param name="text">The text.</param>
    private void UpdateStatusLabel ( string text ) {
      this.lblStatus.Text = text;
    }

    /// <summary>
    /// Updates the status progress bar.
    /// </summary>
    /// <param name="val">The val.</param>
    private void UpdateStatusProgressBar ( int val ) {
      this.progressBar1.Value = val;
    }
  }
}