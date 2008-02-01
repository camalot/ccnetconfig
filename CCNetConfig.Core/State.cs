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
using System.IO;
using CCNetConfig.Core.Serialization;
using System.Xml;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.Core {
  /// <summary>
  /// CruiseControl.NET needs to store state about a project. This is data such as the last build label, the time of the last build, 
  /// and the outcome of the build, etc. The State Manager allows you to specify how and where this data is stored.
  /// </summary>
  [Editor ( typeof ( StateUIEditor ), typeof ( UITypeEditor ) )]
  [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
  public abstract class State : ISerialize, ICCNetObject, ICloneable {
    private string _type = "state";
    private string _directory = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="State"/> class.
    /// </summary>
    public State () { }
    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <value>The type.</value>
    [Description ( "Gets the type." ), Category ( "Required" )]
    public string Type { get { return this._type; } }
    /// <summary>
    /// Gets or sets the directory.
    /// </summary>
    /// <value>The directory.</value>
    [Description ( "The directory where the state file should be saved." ), Category ( "Optional" ), Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ), BrowseForFolderDescription("Select the directory where the state file should be saved.")]
    public string Directory { get { return _directory; } set { this._directory = value; } }


    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString () {
      return GetType ().Name;
    }
    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public abstract System.Xml.XmlElement Serialize();
    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public abstract void Deserialize( System.Xml.XmlElement element );
      

    #endregion

    #region ICloneable Members
    /// <summary>
    /// creates a copy of the state object.
    /// </summary>
    /// <returns></returns>
    public abstract State Clone ();

    object ICloneable.Clone () {
      return this.Clone ();
    }

    #endregion
  }
}
