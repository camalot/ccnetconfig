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

namespace CCNetConfig.Updater.Core.Configuration {
  /// <summary>
  /// A list of <see cref="UpdateInfo"/> objects
  /// </summary>
  public class UpdateInfoList : List<UpdateInfo> {

    /// <summary>
    /// </summary>
    /// <value></value>
    public UpdateInfo this[Version version] {
      get {
        if ( this.Contains ( version ) )
          return this[IndexOf ( version )];
        else
          return null;
      }
      set {
        if ( this.Contains ( version ) )
          this[IndexOf ( version )] = value;
      }
    }

    /// <summary>
    /// Determines whether this contains the specified version.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <returns>
    /// 	<c>true</c> if [contains] [the specified version]; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains ( Version version ) {
      return IndexOf ( version ) > -1;
    }

    /// <summary>
    /// Gets the index of the UpdateInfo object from the Version
    /// </summary>
    /// <param name="version">The version.</param>
    /// <returns></returns>
    public int IndexOf ( Version version ) {
      for ( int x = 0; x < this.Count; x++ ) {
        UpdateInfo uia = this[x];
        if ( uia.Version.CompareTo ( version ) == 0 )
          return x;
      }
      return -1;
    }

    /// <summary>
    /// Gets the latest version.
    /// </summary>
    /// <returns></returns>
    public Version GetLatestVersion () {
      Version maxVersion = new Version( "0.0.0.0" );
      foreach ( UpdateInfo ui in this )
        if ( ui.Version.CompareTo ( maxVersion ) >= 1 )
          maxVersion = ui.Version;
      return maxVersion;
    }
  }
}
