/* Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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

namespace CCNetConfig {
  /// <summary>
  /// Command Buttons for a readonly config file
  /// </summary>
  enum ReadOnlyTaskDialogCommandButton {
    /// <summary>
    /// Save the file as a new file
    /// </summary>
    SaveAs = 0,
    /// <summary>
    /// Remove the readonly attribute
    /// </summary>
    RemoveReadOnly,
    /// <summary>
    /// Cancel the save
    /// </summary>
    Cancel
  }

  /// <summary>
  /// Command Buttons for Error Loading Config File
  /// </summary>
  enum ErrorLoadingConfigTaskDialogCommandButton {
    /// <summary>
    /// View the errors
    /// </summary>
    ViewErrors = 0,
    /// <summary>
    /// Manually edit using external editor
    /// </summary>
    ManuallyEdit,
    /// <summary>
    /// Ignore the errors and load anyway
    /// </summary>
    IgnoreErrors
  }

  enum ProjectNameExistsTaskDialogCommandButton {
    NewName = 0,
    Cancel
  }

	enum ConfigXmlErrorsTaskDialogCommandButton {
		ManuallyEdit = 0,
		ReportAsBug
	}

	enum CommonTaskDialogCommandButton {
		Abort = 0,
		Retry,
		ReportAsBug
	}
}
