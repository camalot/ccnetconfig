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
using CCNetConfig.Core.Configuration;

namespace CCNetConfig.Components {
  internal class BackupControler {
    /// <summary>
    /// Initializes a new instance of the <see cref="BackupControler"/> class.
    /// </summary>
    public BackupControler () {
    }

    /// <summary>
    /// Backup the file.
    /// </summary>
    /// <param name="file">The file.</param>
    public void BackupFile ( FileInfo file ) {
      BackupSettings settings = CCNetConfig.Core.Util.UserSettings.BackupSettings;
      if ( settings.Enabled ) {
        try {
          if ( !settings.SavePath.Exists )
            settings.SavePath.Create ();
          FileInfo randomFile = new FileInfo ( Path.Combine ( settings.SavePath.FullName,
              string.Format ( "{0}.config", Path.GetFileNameWithoutExtension ( Path.GetRandomFileName() ) ) ) );
          if ( file.Exists ) {
            file.CopyTo ( randomFile.FullName, false );
            RemoveExcessBackupFiles ( );
          }
        } catch { throw; }
      }
    }

    /// <summary>
    /// Removes the excess backup files.
    /// </summary>
    private void RemoveExcessBackupFiles () {
      BackupSettings settings = CCNetConfig.Core.Util.UserSettings.BackupSettings;
      // if zero, we want them all.
      if ( settings.NumberOfBackupsToKeep == 0 )
        return;

      List<FileInfo> files = new List<FileInfo> ();
      files.AddRange ( settings.SavePath.GetFiles ( "*.config" ) );
      files.Sort ( new FileInfoDateTimeListSorterDescending () );

      for ( int x = settings.NumberOfBackupsToKeep; x < files.Count; x++ ) {
        try {
          files[x].Delete ();
        } catch { }
      }
    }

    /// <summary>
    /// Sorts FileInfo objects by date ascending
    /// </summary>
    internal class FileInfoDateTimeListSorterAscending : IComparer<FileInfo> {
      #region IComparer<FileInfo> Members

      /// <summary>
      /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
      /// </summary>
      /// <param name="x">The first object to compare.</param>
      /// <param name="y">The second object to compare.</param>
      /// <returns>
      /// Value Condition Less than zero, x is less than y. Zero x equals y. Greater than zero x is greater than y.
      /// </returns>
      public int Compare ( FileInfo x, FileInfo y ) {
        return x.CreationTime.CompareTo ( y.CreationTime );
      }

      #endregion
    }

    /// <summary>
    /// Sorts FileInfo objects by date descending
    /// </summary>
    internal class FileInfoDateTimeListSorterDescending : IComparer<FileInfo> {
      #region IComparer<FileInfo> Members

      /// <summary>
      /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
      /// </summary>
      /// <param name="x">The first object to compare.</param>
      /// <param name="y">The second object to compare.</param>
      /// <returns>
      /// Value Condition Less than zero, x is less than y. Zero x equals y. Greater than zero x is greater than y.
      /// </returns>
      public int Compare ( FileInfo x, FileInfo y ) {
        return -(x.CreationTime.CompareTo ( y.CreationTime ));
      }

      #endregion
    }
  }
}
