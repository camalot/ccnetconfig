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

namespace CCNetConfig.Core {
  /// <summary>
  /// An object that returns a password as "*". This is for a property grid, so the password is not displayed.
  /// </summary>
  public class HiddenPassword : ICloneable {
    private string _password = string.Empty;
    /// <summary>
    /// Initializes a new instance of the <see cref="HiddenPassword"/> class.
    /// </summary>
    public HiddenPassword() { }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    public string Password { 
      get {
        StringBuilder sb = new StringBuilder (_password.Length);
        for ( int x = 0; x < this._password.Length; x++ )
          sb.Append ("*");
        return sb.ToString ();
      }
      set {
        this._password = value;
      }
    }
    /// <summary>
    /// Gets the password actual value;
    /// </summary>
    /// <returns></returns>
    public string GetPassword() {
      return this._password;
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString() {
      return this.Password;
    }

    #region ICloneable Members
    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public HiddenPassword Clone () {
      return this.MemberwiseClone () as HiddenPassword;
    }
    object ICloneable.Clone () {
      return this.Clone ();
    }

    #endregion
  }
}
