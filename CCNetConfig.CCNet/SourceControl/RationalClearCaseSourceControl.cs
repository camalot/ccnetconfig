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
  /// By default, ClearCase returns a history for every file in every branch, even if the config spec limits to a single branch. 
  /// You must specify branch in order to limit which changes CCNet can see.
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class RationalClearCaseSourceControl : SourceControl, ICCNetDocumentation {
    private string _viewPath = string.Empty;
    private string _branch = string.Empty;
    private bool? _autoGetSource = null;
    private bool? _useLabel = null;
    private bool? _useBaseline = null;
    private string _executable = string.Empty;
    private string _projectVobName = string.Empty;
    private string _viewName = string.Empty;
    private Timeout _timeOut = new Timeout ();

    /// <summary>
    /// Initializes a new instance of the <see cref="RationalClearCaseSourceControl"/> class.
    /// </summary>
    public RationalClearCaseSourceControl () : base ( "clearCase" ) { }

    /// <summary>
    /// The path that CCNet will check for modifications and use to apply the label.
    /// </summary>
    /// <value>The view path.</value>
    [Description ( "The path that CCNet will check for modifications and use to apply the label." ), DisplayName ( "(ViewPath)" ),
  DefaultValue ( null ), Category ( "Required" ), Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
BrowseForFolderDescription ( "Select path that CCNet will check for modifications and use to apply the label." )]
    public string ViewPath { get { return this._viewPath; } set { this._viewPath = Util.CheckRequired ( this, "viewPath", value ); } }
    /// <summary>
    /// The name of the branch that CCNet will monitor for modifications. Note that the config 
    /// spec of the view being built from must also be set up to reference this branch.
    /// </summary>
    /// <value>The branch.</value>
    [Description ( "The name of the branch that CCNet will monitor for modifications. Note that the config spec of the view being built from " +
     "must also be set up to reference this branch." ), DefaultValue ( null ), Category ( "Optional" )]
    public string Branch { get { return this._branch; } set { this._branch = value; } }
    /// <summary>
    /// Specifies whether the current version of the source should be retrieved from ClearCase.
    /// </summary>
    /// <value><c>true</c> or <c>false</c></value>
    [Description ( "Specifies whether the current version of the source should be retrieved from ClearCase." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? AutoGetSource { get { return this._autoGetSource; } set { this._autoGetSource = value; } }
    /// <summary>
    /// Specifies whether a label should be applied when the build is successful.
    /// </summary>
    /// <value><c>true</c> or <c>false</c></value>
    [Description ( "Specifies whether a label should be applied when the build is successful." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? UseLabel { get { return this._useLabel; } set { this._useLabel = value; } }
    /// <summary>
    /// Specifies whether a baseline should be applied when the build is successful. Requires the VOB your view references to be a UCM VOB.
    /// Requires that you specify <see cref="CCNetConfig.CCNet.RationalClearCaseSourceControl.ViewName">ViewName</see> and 
    /// <see cref="CCNetConfig.CCNet.RationalClearCaseSourceControl.ProjectVobName">ProjectVobName</see>.
    /// </summary>
    /// <value><c>true</c> or <c>false</c></value>
    [Description ( "Specifies whether a baseline should be applied when the build is successful. Requires the VOB your view references to be a UCM " +
      "VOB. Requires that you specify ViewName and ProjectVobName." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" )]
    public bool? UseBaseline { get { return this._useBaseline; } set { this._useBaseline = value; } }
    /// <summary>
    /// Specifies the path to the ClearCase command line tool. You should only have to include this element if the tool isn't in your 
    /// path. By default, the ClearCase client installation puts cleartool in your path.
    /// </summary>
    /// <value>The executable.</value>
    [Description ( "Specifies the path to the ClearCase command line tool. You should only have to include this element if the tool isn't in " +
     "your path. By default, the ClearCase client installation puts cleartool in your path." ), DefaultValue ( null ),
   Category ( "Optional" ),
  Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ),
OpenFileDialogTitle ( "Select the ClearCase command line tool" ), FileTypeFilter ( "Executable|*.exe" )]
    public string Executable { get { return this._executable; } set { this._executable = value; } }

    /// <summary>
    /// The name of the project VOB that the view path uses. If UseBaseline is true, this property is required
    /// </summary>
    /// <value>The name of the project vob.</value>
    [Description ( "The name of the project VOB that the view path uses.\n*If UseBaseline is true, this property is required." ),
    DefaultValue ( null ),Category("Optional")]
    public string ProjectVobName {
      get { return this._projectVobName; }
      set {
        if ( this.UseBaseline.HasValue && this.UseBaseline.Value )
          this._projectVobName = Util.CheckRequired ( this, "projectVobName", value );
        else
          this._projectVobName = value;
      }
    }

    /// <summary>
    ///  	 The name of the view that you're using. If UseBaseline is true, this property is required
    /// </summary>
    /// <value>The name of the view.</value>
    [Description ( "The name of the view that you're using.\n*If UseBaseline is true, this property is required." ),
    DefaultValue ( null ),Category("Optional")]
    public string ViewName {
      get { return this._viewName; }
      set {
        if ( this.UseBaseline.HasValue && this.UseBaseline.Value )
          this._viewName = Util.CheckRequired ( this, "viewName", value );
        else
          this._viewName = value;
      }
    }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone () {
      RationalClearCaseSourceControl rsc = this.MemberwiseClone () as RationalClearCaseSourceControl;
      rsc.Timeout = this.Timeout.Clone ();
      return rsc;
    }

    /// <summary>
    /// Sets the timeout period for the source control operation.
    /// </summary>
    /// <value>The timeout.</value>
    [Description ( "Sets the timeout period for the source control operation." ), 
    DefaultValue ( null ), TypeConverter ( typeof ( ExpandableObjectConverter ) ),Category("Optional")]
    public Timeout Timeout { get { return this._timeOut; } set { this._timeOut = value; } }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      //root.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name ) );

      XmlElement ele = doc.CreateElement ( "viewPath" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.ViewPath );
      root.AppendChild ( ele );

      if ( !string.IsNullOrEmpty ( this.Branch ) ) {
        ele = doc.CreateElement ( "branch" );
        ele.InnerText = this.Branch;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Executable ) ) {
        ele = doc.CreateElement ( "executable" );
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.UseLabel.HasValue ) {
        ele = doc.CreateElement ( "useLabel" );
        ele.InnerText = this.UseLabel.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.UseBaseline.HasValue ) {
        ele = doc.CreateElement ( "useBaseline" );
        ele.InnerText = this.UseBaseline.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.ProjectVobName ) || ( this.UseBaseline.HasValue && this.UseBaseline.Value ) ) {
        ele = doc.CreateElement ( "projectVobName" );
        ele.InnerText = ( this.UseBaseline.HasValue && this.UseBaseline.Value ) ?
          Util.CheckRequired ( this, "projectVobName", this.ProjectVobName ) : this.ProjectVobName;
        if ( !string.IsNullOrEmpty ( this.ProjectVobName ) )
          root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.ViewName ) || ( this.UseBaseline.HasValue && this.UseBaseline.Value ) ) {
        ele = doc.CreateElement ( "viewName" );
        ele.InnerText = ( this.UseBaseline.HasValue && this.UseBaseline.Value ) ?
          Util.CheckRequired ( this, "viewName", this.ViewName ) : this.ViewName;
        if ( !string.IsNullOrEmpty ( this.ViewName ) )
          root.AppendChild ( ele );
      }

      if ( this.Timeout != null ) {
        ele = this.Timeout.Serialize ();
        if ( ele != null )
          root.AppendChild ( doc.ImportNode ( ele, true ) );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this._viewPath = string.Empty;
      this.Branch = string.Empty;
      this.AutoGetSource = null;
      this.UseLabel = null;
      this.UseBaseline = null;
      this.Executable = string.Empty;
      this.ProjectVobName = string.Empty;
      this.ViewName = string.Empty;
      this.Timeout = new Timeout ();

      this.ViewPath = Util.GetElementOrAttributeValue ( "viewPath", element );

      string s = Util.GetElementOrAttributeValue ( "branch", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Branch = s;

      s = Util.GetElementOrAttributeValue ( "executable", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue ( "projectVobName", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ProjectVobName = s;

      s = Util.GetElementOrAttributeValue ( "viewName", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ViewName = s;

      XmlElement ele = (XmlElement)element.SelectSingleNode ( "timeout" );
      if ( ele != null )
        this.Timeout.Deserialize ( ele );

      s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "useLabel", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.UseLabel = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "useBaseline", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.UseBaseline = string.Compare ( s, bool.TrueString, true ) == 0;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Rational+ClearCase+Source+Control+Block?decorator=printable" ); }
    }

    #endregion
  }
}
