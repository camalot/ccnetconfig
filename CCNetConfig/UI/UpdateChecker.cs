/*
 * Copyright (c) 2006-2007, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Updater.Core;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using CCNetConfig.Core.Enums;

namespace CCNetConfig.UI {
  /// <summary>
  /// Checks for updates.
  /// </summary>
  public class UpdateChecker {
    private IWin32Window _owner = null;
    /// <summary>
    /// Checks for updates.
    /// </summary>
    /// <param name="owner">The owner.</param>
    public void Check (IWin32Window owner) {
      try {
        _owner = owner;
        ApplicationUpdater applicationUpdater = new ApplicationUpdater ();
        applicationUpdater.UpdateAvailable += new EventHandler<UpdatesAvailableEventArgs> ( applicationUpdater_UpdateAvailable );
        applicationUpdater.Version = new Version ( Application.ProductVersion );
        applicationUpdater.CheckForUpdate ( CCNetConfig.Core.Util.UserSettings.UpdateSettings.UpdateCheckType );
      } catch /*( Exception ex )*/ {

      }
    }

    /// <summary>
    /// Checks for updates.
    /// </summary>
    public void Check () {
      Check ( null );
    }

    /// <summary>
    /// Handles the UpdateAvailable event of the applicationUpdater control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="UpdatesAvailableEventArgs"/> instance containing the event data.</param>
    void applicationUpdater_UpdateAvailable ( object sender, UpdatesAvailableEventArgs e ) {
      ApplicationUpdater ai = (ApplicationUpdater)sender;
      if ( ai.UpdatesAvailable ) {
        UpdateInformationForm uif = new UpdateInformationForm ( ai );
        if ( uif.ShowDialog ( _owner ) == DialogResult.OK ) {
          string thisPath = this.GetType ( ).Assembly.Location;
          string attrString = string.Format ( CCNetConfig.Core.Util.UserSettings.UpdateSettings.LaunchArgumentsFormat, CCNetConfig.Core.Util.UserSettings.UpdateSettings.UpdateCheckType, thisPath, ai.UpdateInfoList.GetLatestVersion ( ) );
          Console.WriteLine ( attrString );
          Process.Start ( Path.Combine ( Application.StartupPath, CCNetConfig.Core.Util.UserSettings.UpdateSettings.UpdaterApplication ), attrString );
          Application.Exit ( );
        }
      } 
    }
  }
}
