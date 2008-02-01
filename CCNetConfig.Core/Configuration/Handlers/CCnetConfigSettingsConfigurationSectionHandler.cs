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
using System.Configuration;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace CCNetConfig.Core.Configuration.Handlers {
  /// <summary>
  /// Creates the Settings Object.
  /// </summary>
  public class CCNetConfigSettingsConfigurationSectionHandler : IConfigurationSectionHandler {

    /// <summary>
    /// Initializes a new instance of the <see cref="CCNetConfigSettingsConfigurationSectionHandler"/> class.
    /// </summary>
    public CCNetConfigSettingsConfigurationSectionHandler () {
      
    }

    /// <summary>
    /// Creates a configuration section handler.
    /// </summary>
    /// <param name="parent">Parent object.</param>
    /// <param name="configContext">Configuration context object.</param>
    /// <param name="section">Section XML element.</param>
    /// <returns>The created section handler object.</returns>
    public CCNetConfigSettings Create ( object parent, object configContext, XmlElement section ) {
      try {
        XmlDocument doc = new XmlDocument ( );
        if ( section != null )
          doc.AppendChild ( doc.ImportNode ( section, true ) );
        return this.Create ( parent, configContext, doc );
      } catch { throw; } 
    }

    /// <summary>
    /// Creates a configuration section handler.
    /// </summary>
    /// <param name="parent">Parent object.</param>
    /// <param name="configContext">Configuration context object.</param>
    /// <param name="doc">The XML document.</param>
    /// <returns>The created section handler object.</returns>
    public CCNetConfigSettings Create ( object parent, object configContext, XmlDocument doc ) {
      try {
        MemoryStream ms = new MemoryStream ( );
        CCNetConfigSettings settings = null;
        using ( ms ) {
          doc.Save ( ms );
          ms.Position = 0;
          settings = this.Create ( parent, configContext, ms );
        }
        return settings;
      } catch { throw; }
    }

    /// <summary>
    /// Creates a configuration section handler.
    /// </summary>
    /// <param name="parent">Parent object.</param>
    /// <param name="configContext">Configuration context object.</param>
    /// <param name="file">The file.</param>
    /// <returns>The created section handler object.</returns>
    public CCNetConfigSettings Create ( object parent, object configContext, FileInfo file ) {
      try {
        if ( file.Exists ) {
          if ( file.IsReadOnly )
            file.IsReadOnly = false;
          return Create ( parent, configContext, file.OpenRead ( ) );
        } else
          return null;
      } catch {
        throw;
      }
    }

    /// <summary>
    /// Creates a configuration section handler.
    /// </summary>
    /// <param name="parent">Parent object.</param>
    /// <param name="configContext">Configuration context object.</param>
    /// <param name="stream">The stream.</param>
    /// <returns>The created section handler object.</returns>
    public CCNetConfigSettings Create ( object parent, object configContext, Stream stream ) {
      try {
        CCNetConfigSettings settings = null;
        using ( stream ) {
          XmlSerializer serializer = new XmlSerializer ( typeof ( CCNetConfigSettings ) );
          settings = serializer.Deserialize ( stream ) as CCNetConfigSettings;
        }
        return settings;
      } catch { throw; } 
    }

    /// <summary>
    /// Saves the settings.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public static void SaveSettings ( CCNetConfigSettings settings ) {
      try {
        FileInfo file = CCNetConfig.Core.Util.UserSettingsFile;
        if ( file.Exists && file.IsReadOnly )
          file.IsReadOnly = false;
        XmlSerializer serializer = new XmlSerializer ( typeof ( CCNetConfigSettings ) );
        FileStream fs = new FileStream ( file.FullName, FileMode.Create, FileAccess.Write );
        using ( fs ) {
          serializer.Serialize ( fs, settings );
        }
      } catch { throw; }
    }

    #region IConfigurationSectionHandler Members

    /// <summary>
    /// Creates a configuration section handler.
    /// </summary>
    /// <param name="parent">Parent object.</param>
    /// <param name="configContext">Configuration context object.</param>
    /// <param name="section">Section XML node.</param>
    /// <returns>The created section handler object.</returns>
    object IConfigurationSectionHandler.Create ( object parent, object configContext, System.Xml.XmlNode section ) {
      try {
        return this.Create ( parent, configContext, section as XmlElement );
      } catch { throw; }
    }

    #endregion
  }
}
