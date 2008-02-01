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
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// <para>Most build processes interact with external tools that write their output to file (eg. NUnit or FxCop or NCover). In order to make the 
  /// output of these tools available to <a href="http://confluence.public.thoughtworks.org/display/CCNET/">CruiseControl.NET</a> 
  /// to be used in the build process or displayed in the <a href="http://confluence.public.thoughtworks.org/display/CCNET/">CCNet</a>
  /// web page or included in <a href="http://confluence.public.thoughtworks.org/display/CCNET/">CCNet</a> emails, these files need to be 
  /// merged into the <a href="http://confluence.public.thoughtworks.org/display/CCNET/">CCNet</a> integration.</para>
  /// <para>To merge these files, you need to include a <see cref="CCNetConfig.CCNet.FileMergeTaskPublisher">File Merge Task</see>
  /// in your <a href="http://confluence.public.thoughtworks.org/display/CCNET/">CCNet</a>
  /// project.</para>
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class FileMergeTaskPublisher : PublisherTask, ICCNetDocumentation {
    private CloneableList<string> _files;
    /// <summary>
    /// Initializes a new instance of the <see cref="FileMergeTaskPublisher"/> class.
    /// </summary>
    public FileMergeTaskPublisher () : base("merge") {
      _files = new CloneableList<string> ();
    }
    /// <summary>
    /// The file can be specified using an asterisk ("*") wildcard in order to include multiple files that match the specified pattern 
    /// (ie. "*-results.xml" will merge all files ending with the suffix "-results.xml"). The asterisk wildcard can only be used in the filename, 
    /// not in the path.
    /// </summary>
    [Description("The file can be specified using an asterisk (\"*\") wildcard in order to include multiple files that match the specified pattern "+ 
      "(ie. \"*-results.xml\" will merge all files ending with the suffix \"-results.xml\"). The asterisk wildcard can only be used in the filename, " +
      "not in the path."),
    DefaultValue(null),Editor(typeof(StringListUIEditor),typeof(UITypeEditor)),
    TypeConverter(typeof(StringListTypeConverter)),Category("Required"),DisplayName("(Files)")]
    public CloneableList<string> Files { get { return this._files; } set { this._files = Util.CheckRequired ( this, "files", value ); } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      FileMergeTaskPublisher fmtp = this.MemberwiseClone () as FileMergeTaskPublisher;
      fmtp.Files = this.Files.Clone ();
      return fmtp;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = doc.CreateElement ( "files" );
      foreach ( string f in _files ) {
        XmlElement te = doc.CreateElement ( "file" );
        te.InnerText = f;
        ele.AppendChild ( te );
      }
      root.AppendChild ( ele );
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this._files = new CloneableList<string> ();

      if ( string.Compare (element.Name, this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.Name, this.TypeName));

      XmlElement ele = (XmlElement)element.SelectSingleNode ("files");
      if ( ele != null ) {
        foreach ( XmlElement fele in ele.SelectNodes ("file") )
          this._files.Add (fele.InnerText);
      }
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ("http://confluence.public.thoughtworks.org/display/CCNET/File+Merge+Task?decorator=printable"); }
    }

    #endregion
  }
}
