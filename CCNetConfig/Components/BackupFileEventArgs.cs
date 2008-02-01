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
using System.IO;

namespace CCNetConfig.Components {
  /// <summary>
  /// Event args for a backup taking place.
  /// </summary>
  public class BackupFileEventArgs : EventArgs {
    private FileInfo _restoredFile = null;
    private FileInfo _backupFile = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackupFileEventArgs"/> class.
    /// </summary>
    /// <param name="backupFile">The backup file.</param>
    /// <param name="outputFile">The output file.</param>
    public BackupFileEventArgs (FileInfo backupFile, FileInfo outputFile ) : base() {
      this._restoredFile = outputFile;
      this._backupFile = backupFile;
    }

    /// <summary>
    /// Gets the backup file.
    /// </summary>
    /// <value>The backup file.</value>
    public FileInfo BackupFile { get { return this._backupFile; } }
    /// <summary>
    /// Gets the restored file.
    /// </summary>
    /// <value>The restored file.</value>
    public FileInfo RestoredFile { get { return this._restoredFile; } }
  }
}
