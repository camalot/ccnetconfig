/* Copyright (c) 2006, Ryan Conrad. All rights reserved.
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
using System.Xml;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// BitKeeper support added by Harold L Hunt II of StarNet Communications Corp.
  /// </summary>
  /// <remarks>SSH Access Not Supported</remarks>
  [ MinimumVersion( "1.0" ) ]
  public class BitKeeperSourceControl : SourceControl, ICCNetDocumentation {
    private string _executable = string.Empty;
    private string _workingDirectory = string.Empty;
    private bool? _autoGetSource = null;
    private bool? _tagOnSuccess = null;
    private string _cloneTo = string.Empty;
    private bool? _fileHistory = null;
    private Timeout _timeout = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="BitKeeperSourceControl"/> class.
    /// </summary>
    public BitKeeperSourceControl () : base("bitkeeper") {
      _timeout = new Timeout ();
    }
    /// <summary>
    /// Absolute, DOS-style, path to bk.exe
    /// </summary>
    [Description ( "Absolute, DOS-style, path to bk.exe" ), DefaultValue ( null ), Category ( "Optional" ),
   Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "BitKeeper|bk.exe" ),
    OpenFileDialogTitle("Select bk.exe")]
    public string Executable { get { return this._executable; } set { this._executable = value; } }

    /// <summary>
    ///  
    /// </summary>
    [Description ( "Absolute, DOS-style, path to permanent BitKeeper repository." ), DefaultValue ( null ),
    DisplayName("(WorkingDirectory)"), Category("Required"), Editor(typeof(BrowseForFolderUIEditor),typeof(UITypeEditor)),
   BrowseForFolderDescription ( "Select path to permanent BitKeeper repository" )]
    public string WorkingDirectory { get { return this._workingDirectory; } set { this._workingDirectory = Util.CheckRequired ( this, "workingDirectory", value ); } }

    /// <summary>
    /// Automatically pull latest source into permanent BitKeeper repository.
    /// </summary>
    [Description ( "Automatically pull latest source into permanent BitKeeper repository." ), 
    DefaultValue ( null ), Category ( "Optional" ),
    Editor(typeof(DefaultableBooleanUIEditor),typeof(UITypeEditor)),
    TypeConverter(typeof(DefaultableBooleanTypeConverter))]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }

    /// <summary>
    /// Add BitKeeper tag on successful build.
    /// </summary>
    [Description ( "Add BitKeeper tag on successful build." ),
    DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? TagOnSuccess { get { return this._tagOnSuccess; } set { this._tagOnSuccess = value; } }

    /// <summary>
    ///  	 Include history of each file, rather than just ChangeSets.
    /// </summary>
    [Description ( "Include history of each file, rather than just ChangeSets." ),
    DefaultValue ( null ), Category ( "Optional" ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? FileHistory { get { return this._fileHistory; } set { this._fileHistory = value; } }

    /// <summary>
    /// Make a clone of the permanent BK repository into the designated path.
    /// </summary>
    [Description("Make a clone of the permanent BK repository into the designated path."),
   DefaultValue ( null ), Category ( "Optional" )]
    public string CloneTo { get { return this._cloneTo; } set { this._cloneTo = value; } }

    /// <summary>
    /// Sets the timeout period for the source control operation.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Description ( "Sets the timeout period for the source control operation." ), DefaultValue ( null ), Category ( "Optional" )]
    public Timeout Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      BitKeeperSourceControl bsc = this.MemberwiseClone(  ) as BitKeeperSourceControl ;
      bsc.Timeout = this.Timeout.Clone(  );
      return bsc;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourceControl" );
      root.SetAttribute ( "type", TypeName );
      // this helps make deserialization easier
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = null;
      
      if ( !string.IsNullOrEmpty ( this.Executable ) ) {
        ele = doc.CreateElement ( "executable" );
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      ele = doc.CreateElement ( "workingDirectory" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.WorkingDirectory );
      root.AppendChild ( ele );

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString() ;
        root.AppendChild ( ele );
      }

      if ( this.TagOnSuccess.HasValue ) {
        ele = doc.CreateElement ( "tagOnSuccess" );
        ele.InnerText = this.TagOnSuccess.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.FileHistory.HasValue ) {
        ele = doc.CreateElement ( "fileHistory" );
        ele.InnerText = this.FileHistory.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty(this.CloneTo) ) {
        ele = doc.CreateElement ( "cloneTo" );
        ele.InnerText = this.CloneTo;
        root.AppendChild ( ele );
      }

      if ( this.Timeout != null ) {
        ele = Timeout.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }
      return root;
    }


    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this._workingDirectory = string.Empty;
      this.AutoGetSource = null;
      this.CloneTo = string.Empty;
      this.Executable = string.Empty;
      this.FileHistory = null;
      this.TagOnSuccess = null;
      this.Timeout = new Timeout ();
      if ( string.Compare (element.GetAttribute ("type"), this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute ("type"), this.TypeName));

      this.WorkingDirectory = Util.GetElementOrAttributeValue ("workingDirectory", element);
      
      string s = Util.GetElementOrAttributeValue ("autoGetSource", element);
      if ( !string.IsNullOrEmpty (s) )
        this.AutoGetSource = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("executable", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue ("fileHistory", element);
      if ( !string.IsNullOrEmpty (s) )
        this.FileHistory = string.Compare (s, bool.TrueString, true) == 0;

      s = Util.GetElementOrAttributeValue ("tagOnSuccess", element);
      if ( !string.IsNullOrEmpty (s) )
        this.TagOnSuccess = string.Compare (s, bool.TrueString, true) == 0;

      XmlElement ele = (XmlElement)element.SelectSingleNode ("timeout");
      if ( ele != null )
        this.Timeout.Deserialize (ele);
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ( "http://ccnet.thoughtworks.net/display/CCNET/BitKeeper+Source+Control+Block?decorator=printable" ); }
    }

    #endregion
  }
}
