/*
 * Copyright (c) 2006-2008, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core.Components;
using CCNetConfig.Core;
using System.ComponentModel;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// 
  /// </summary>
  [MinimumVersion ( "1.0" )]
  public class RssPublisher : PublisherTask, ICCNetDocumentation {
    private string _filename = string.Empty;

    public RssPublisher (  ) : base("rss") {

    }

    [Description("The file name for the rss data."), Category("Required"),
    MinimumVersion("1.0"), MaximumVersion("1.3"), DisplayName("(FileName)"),
    DefaultValue(null), FileTypeFilter("Xml Files|*.xml;*.rss"), OpenFileDialogTitle("Select File Name."),
    Editor(typeof(OpenFileDialogUIEditor), typeof(UITypeEditor)),
    ReflectorName("filename")]
    public string FileName { get { return this._filename; } set { this._filename = value; } }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      throw new NotImplementedException ( );
    }

    public override void Deserialize ( System.Xml.XmlElement element ) {
      throw new NotImplementedException ( );
    }

    public override PublisherTask Clone ( ) {
      throw new NotImplementedException ( );
    }

    #region ICCNetDocumentation Members

    public Uri DocumentationUri {
      get { throw new NotImplementedException ( ); }
    }

    #endregion
  }
}
