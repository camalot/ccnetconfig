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
using System.Xml.Serialization;
using System.Net;

namespace CCNetConfig.Core.Configuration {
  /// <summary>
  /// Proxy Information used when connecting to check for updates.
  /// </summary>
  [ XmlRoot( "ProxyInfo" ) ]
  public class UpdateProxySettings {
    private bool _useProxy = false;
    private string _host = string.Empty;
    private int _port = 8080;
    private string _user = string.Empty;
    private string _password = string.Empty;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProxySettings"/> class.
    /// </summary>
    public UpdateProxySettings () {

    }

    /// <summary>
    /// Gets or sets a value indicating whether [use proxy].
    /// </summary>
    /// <value><c>true</c> if [use proxy]; otherwise, <c>false</c>.</value>
    [ XmlAttribute( "UseProxy" ) ]
    public bool UseProxy { get { return this._useProxy; } set { this._useProxy = value; } }

    /// <summary>
    /// Gets or sets the proxy port.
    /// </summary>
    /// <value>The proxy port.</value>
    [ XmlElement( "ProxyPort" ) ]
    public int ProxyPort { get { return this._port; } set { this._port = value; } }
    /// <summary>
    /// Gets or sets the proxy server.
    /// </summary>
    /// <value>The proxy server.</value>
    [ XmlElement( "ProxyServer" ) ]
    public string ProxyServer { get { return this._host; } set { this._host = value; } }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    [ XmlElement( "Username" ) ]
    public string Username { get { return this._user; } set { this._user = value; } }

    /// <summary>
    /// Gets or sets the password encoded.
    /// </summary>
    /// <value>The password encoded.</value>
    [ XmlElement( "Password" ) ]
    public string PasswordEncoded { get { return Util.Encode ( this._password ); } set { this._password = Util.Decode(value); } }
    
    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [ XmlIgnore ]
    public string Password { get { return this._password; } set { this._password = value; } }

    /// <summary>
    /// Creates the proxy.
    /// </summary>
    /// <returns>The proxy client.</returns>
    public IWebProxy CreateProxy () {
      if ( !UseProxy )
        return null;

      WebProxy proxy = new WebProxy ( this._host,this._port );
      proxy.BypassProxyOnLocal = true;
      proxy.Credentials = new NetworkCredential ( this.Username, this.Password );
      return proxy;
    }
  }
}
