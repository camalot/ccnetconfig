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
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The File Labeller is used to generate labels based on the content of a disk file. This file may 
  /// be created by your build process, or by some outside mechanism. The labeller is configured with 
  /// the location of the file, and it reads the file content to generate the label for CCNet.
  /// </summary>
  [TypeConverter ( typeof ( ExpandableObjectConverter ) ),
  MinimumVersion ( "1.3" )]
  public class FileLabeller : Labeller, ICCNetDocumentation {
    private string _prefix = string.Empty;
    private string _labelFilePath = string.Empty;
    private bool? _allowDuplicateSubsequentLabels = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileLabeller"/> class.
    /// </summary>
    public FileLabeller ( ) : base("fileLabeller") {

    }
    /// <summary>
    /// If true, a label which matches the immediately previous label will be generated as is. 
    /// Otherwise it will have a version number appended (e.g., prefixlabel-1, prefixlabel-2).
    /// </summary>
    /// <value>The allow duplicate subsequent labels.</value>
    [Description ( "If true, a label which matches the immediately previous label will be generated " +
      "as is. Otherwise it will have a version number appended (e.g., prefixlabel-1, prefixlabel-2)." ),
    Category("Optional"),DefaultValue(null)]
    public bool? AllowDuplicateSubsequentLabels {
      get { return _allowDuplicateSubsequentLabels; }
      set { _allowDuplicateSubsequentLabels = value; }
    }

    /// <summary>
    /// Gets or sets the label file path.
    /// </summary>
    /// <value>The label file path.</value>
    [Description("The pathname of the file to read. This can be the absolute " +
      "path or one relative to the project's working directory."), Category("Required"),
    DisplayName("(LabelFilePath)"),DefaultValue(null),Editor(typeof(OpenFileDialogUIEditor),typeof(UITypeEditor)),
    OpenFileDialogTitle("Select file to read."), FileTypeFilter("All Files (*.*)|*.*")]
    public string LabelFilePath {
      get { return _labelFilePath; }
      set { _labelFilePath = Util.CheckRequired(this,"labelFilePath", value); }
    }

    /// <summary>
    /// Any string to be put in front of all labels
    /// </summary>
    /// <value>The prefix.</value>
    [Description("Any string to be put in front of all labels"), Category("Optional"), DefaultValue(null)]
    public string Prefix {
      get { return _prefix; }
      set { _prefix = value; }
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( "labeller" );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      root.SetAttribute ( "type", this.TypeName );
      XmlElement ele = null;
      if ( !string.IsNullOrEmpty ( this.Prefix ) ) {
        ele = doc.CreateElement ( "prefix" );
        ele.InnerText = this.Prefix;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.LabelFilePath ) ) {
        ele = doc.CreateElement ( "labelFilePath" );
        ele.InnerText = this.LabelFilePath;
        root.AppendChild ( ele );
      }

      if ( this.AllowDuplicateSubsequentLabels.HasValue ) {
        ele = doc.CreateElement ( "allowDuplicateSubsequentLabels" );
        ele.InnerText = this.AllowDuplicateSubsequentLabels.Value.ToString ( );
        root.AppendChild ( ele );
      }
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.AllowDuplicateSubsequentLabels = null;
      this.Prefix = string.Empty;
      this._labelFilePath = string.Empty;

      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", 
          element.GetAttribute ( "type" ), this.TypeName ) );

      string s = Util.GetElementOrAttributeValue ( "prefix", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Prefix = s;

      s = Util.GetElementOrAttributeValue ( "labelFilePath", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.LabelFilePath = s;

      s = Util.GetElementOrAttributeValue ( "allowDuplicateSubsequentLabels", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        bool i = false;
        if ( bool.TryParse ( s, out i ) )
          this.AllowDuplicateSubsequentLabels = i;
      }
    }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone ( ) {
      FileLabeller fl = this.MemberwiseClone ( ) as FileLabeller;
      return fl;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/File+Labeller?decorator=printable" ); }
    }

    #endregion
  }
}
