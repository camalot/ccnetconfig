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
using CCNetConfig.Core;
using System.IO;

namespace CCNetConfig.UI {
  /// <summary>
  /// Shows information about the configuration.
  /// </summary>
  public partial class ConfigurationPropertiesForm : Form {
    private CruiseControl _cruiseControl = null;
    private FileInfo _configFile = null;
    private List<Version> _versions = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationPropertiesForm"/> class.
    /// </summary>
    public ConfigurationPropertiesForm (CruiseControl cc, FileInfo cf, List<Version> versions ) {
      _configFile = cf;
      _cruiseControl = cc;
      _versions = versions;
      InitializeComponent ();
      this.versionComboBox.DataSource = _versions;

      int initialIndex = this._versions.IndexOf ( _cruiseControl.Version );
      this.versionComboBox.SelectedIndex = initialIndex;

      if ( this._configFile.Exists ) {
        this.lastModifiedDate.Text = this._configFile.LastWriteTime.ToString ();
        this.createdDateLabel.Text = this._configFile.CreationTime.ToString ();
        this.fileNameLabel.Text = Path.GetFileName ( this._configFile.FullName );
      }

      this.numberOfProjectsLabel.Text = this._cruiseControl.Projects.Count.ToString();
    }

    /// <summary>
    /// Handles the Click event of the okButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void okButton_Click ( object sender, EventArgs e ) {
      if ( this._cruiseControl.Version.CompareTo ( this.versionComboBox.SelectedItem ) != 0 )
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
      this.Close ();
    }

    /// <summary>
    /// Gets the selected version.
    /// </summary>
    /// <value>The selected version.</value>
    public Version SelectedVersion { get { return this.versionComboBox.SelectedItem as Version; } }
  }
}