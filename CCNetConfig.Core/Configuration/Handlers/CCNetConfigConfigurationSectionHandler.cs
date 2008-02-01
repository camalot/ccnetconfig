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
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace CCNetConfig.Core.Configuration.Handlers {
  /// <summary>
  /// Handles the configuration blocks in a configuration file.
  /// </summary>
  public class CCNetConfigConfigurationSectionHandler : IConfigurationSectionHandler {
    /// <summary>
    /// Creates the specified parent.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="configContext">The config context.</param>
    /// <param name="section">The section.</param>
    /// <returns></returns>
    public CCNetConfigConfiguration Create ( object parent, object configContext, XmlElement section ) {
      CCNetConfigConfiguration ccncc = null;
      XmlSerializer serializer = new XmlSerializer ( typeof ( CCNetConfigConfiguration ) );
      XmlDocument doc = new XmlDocument ();
      XmlElement ele = (XmlElement)doc.ImportNode ( section, true );
      doc.AppendChild ( doc.CreateXmlDeclaration ( "1.0", "utf-8", string.Empty ) );
      doc.AppendChild ( ele );
      MemoryStream strm = new MemoryStream ();
      using ( strm ) {
        doc.Save ( strm );
        strm.Position = 0;
        ccncc = (CCNetConfigConfiguration)serializer.Deserialize ( strm );
      }
      return ccncc;
    }
    
    #region IConfigurationSectionHandler Members

    /// <summary>
    /// Creates a configuration section handler.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="configContext">Configuration context object.</param>
    /// <param name="section"></param>
    /// <returns>The created section handler object.</returns>
    object IConfigurationSectionHandler.Create ( object parent, object configContext, System.Xml.XmlNode section ) {
      return this.Create ( parent, configContext, (XmlElement)section );
    }

    #endregion
  }
}
