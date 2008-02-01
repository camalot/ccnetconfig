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
using System.Xml;
using System.Xml.Serialization;

namespace CCNetConfig.Core.Configuration {
  /// <summary>
  /// Configuration Object for CCNetConfig
  /// </summary>
  [Serializable,XmlRoot("CCNetConfigConfiguration")]
  public sealed class CCNetConfigConfiguration {
    private List<ConfigurationFile> _files;
    /// <summary>
    /// Initializes a new instance of the <see cref="CCNetConfigConfiguration"/> class.
    /// </summary>
    public CCNetConfigConfiguration () {
      _files = new List<ConfigurationFile> ();
    }

    /// <summary>
    /// Gets or sets the files.
    /// </summary>
    /// <value>The files.</value>
    [XmlArray("Files"),XmlArrayItem("File")]
    public List<ConfigurationFile> Files { 
      get {
        return _files; 
      } 
      set {
        _files = value;
      } 
    }
    /// <summary>
    /// Gets the <see cref="CCNetConfig.Core.Configuration.CCNetConfigConfiguration.ConfigurationFile"/> with the specified key.
    /// </summary>
    /// <value></value>
    [XmlIgnore]
    public ConfigurationFile this[string key] {
      get {
        foreach ( ConfigurationFile cf in this._files ) {
          if ( string.Compare(key,cf.Name,true) == 0 )
            return cf;
        }

        throw new KeyNotFoundException ( string.Format ( "The key {0} was not found.", key ) );
      }
    }
    /// <summary>
    /// Gets the <see cref="CCNetConfig.Core.Configuration.CCNetConfigConfiguration.ConfigurationFile"/> at the specified index.
    /// </summary>
    /// <value></value>
    [XmlIgnore]
    public ConfigurationFile this[int index] {
      get { return this.Files[index]; }
    }

    /// <summary>
    /// contains information about a configuration file.
    /// </summary>
    public sealed class ConfigurationFile {
      /// <summary>
      /// Initializes a new instance of the <see cref="ConfigurationFile"/> class.
      /// </summary>
      public ConfigurationFile () {

      }
      private string _name = string.Empty;
      private string _path = string.Empty;
      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      [XmlAttribute("Name")]
      public string Name { get { return this._name; } set { this._name = value; } }
      /// <summary>
      /// Gets or sets the path.
      /// </summary>
      /// <value>The path.</value>
      [XmlAttribute("Path")]
      public string Path { get { return this._path; } set { this._path = value; } }
    }
  }

}
