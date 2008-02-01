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
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// CC.NET task which announces new build results on Twitter. The project manager creates a 
  /// special Twitter account and configures CC.NET to post build results as updates for that user. 
  /// The developers then just have to add that user to their friend list, and will get the 
  /// announcements in the Twitter front-end of their choice.
  /// </summary>
  [MinimumVersion("1.1"),Plugin]
  class TwitterPublisher : PublisherTask, ICCNetDocumentation  {
    private string _userName = string.Empty;
    private HiddenPassword _password = null;

    public TwitterPublisher ( ) : base("twitter") {
      _password = new HiddenPassword ( );
    }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    [Category ( "Required" ), DisplayName ( "(Username)" ), DefaultValue ( null ), Description ( "Twitter account username" )]
    public string Username { get { return this._userName; } set { this._userName = Util.CheckRequired ( this, "user", value ); } }
    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [TypeConverter ( typeof ( PasswordTypeConverter ) ), DefaultValue ( null ), Description ( "Twitter account Password" ), Category ( "Required" ), DisplayName("(Password)")]
    public HiddenPassword Password { get { return this._password; } set { this._password = Util.CheckRequired ( this, "password", value ); } }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( this.TypeName );

      XmlElement ele = doc.CreateElement ( "user" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Username );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "password" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Password.GetPassword() );
      root.AppendChild ( ele );

      return root;

    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.Username = Util.GetElementOrAttributeValue ( "user", element );
      this.Password.Password = Util.GetElementOrAttributeValue ( "password", element );

    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      TwitterPublisher tp = this.MemberwiseClone ( ) as TwitterPublisher;
      tp.Password = this.Password.Clone ( );
      return tp;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public Uri DocumentationUri {
      get { return new Uri("http://thomasfreudenberg.com/blog/archive/2007/06/17/twitter-publisher-for-cruisecontrol-net.aspx"); }
    }

    #endregion
  }
}
