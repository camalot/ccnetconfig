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
using System.Text;

namespace CCNetConfig.Updater.Core {
  /// <summary>
  /// Event Arguments that contains information about thie download progress
  /// </summary>
  public class DownloadProgressEventArgs : EventArgs {
    private long _bytesR = 0;
    private long _totalBytes = 0;
    private UpdateFile _ufile = null;

    /// <summary>
    /// Gets or sets the bytes received.
    /// </summary>
    /// <value>The bytes received.</value>
    public long BytesReceived { get { return this._bytesR; } set { this._bytesR = value; } }
    /// <summary>
    /// Gets or sets the total bytes received.
    /// </summary>
    /// <value>The total bytes received.</value>
    public long TotalBytesReceived { get { return this._totalBytes; } set { this._totalBytes = value; } }
    /// <summary>
    /// Gets or sets the percentage.
    /// </summary>
    /// <value>The percentage.</value>
    public int Percentage { get { return this.UpdateFile.Size > 0 ? (int)( ( (double)this.TotalBytesReceived / (double)this.UpdateFile.Size ) * 100d ) : 0; } }
    /// <summary>
    /// Gets or sets the update file.
    /// </summary>
    /// <value>The update file.</value>
    public UpdateFile UpdateFile { get { return _ufile; } set { this._ufile = value; } }
  }
}
