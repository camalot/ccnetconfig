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
using CCNetConfig.Core.Serialization;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// This publisher generates a rss file reporting the latest results for a Project.
  /// </summary>
  [MinimumVersion ( "1.0" ), ReflectorName("rss")]
  public class RssPublisher : PublisherTask, ICCNetDocumentation {
    private string _filename = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="RssPublisher"/> class.
    /// </summary>
    public RssPublisher (  ) : base("rss") {

    }

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    /// <value>The name of the file.</value>
    [Description("The file name for the rss data."), Category("Required"),
    MinimumVersion("1.0"), MaximumVersion("1.3"), DisplayName("(FileName)"),
    DefaultValue(null), FileTypeFilter("Xml Files|*.xml;*.rss"), OpenFileDialogTitle("Select File Name."),
    Editor(typeof(OpenFileDialogUIEditor), typeof(UITypeEditor)),
    ReflectorName("filename"), Required,ReflectorNodeType(ReflectorNodeTypes.Attribute)]
    public string FileName { get { return this._filename; } set { this._filename = value; } }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<RssPublisher>().Serialize(this);
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.FileName = string.Empty;
      new Serializer<RssPublisher>().Deserialize(element,this);
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      return this.MemberwiseClone ( ) as RssPublisher;
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false),ReflectorIgnore]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Welcome+to+CruiseControl.NET" ); }
    }

    #endregion
  }
}
