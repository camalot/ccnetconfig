/*
 * Copyright (c) 2006 - 2007, Ryan Conrad. All rights reserved.
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
  [ Plugin, MinimumVersion( "1.3") ]
  public class BazaarSourceControl : SourceControl, ICCNetDocumentation {
    private Uri _branchUrl;
    private WebUrlBuilder _webUrlBuilder;
    private bool? _autoGetSource;
    private bool? _tagOnSuccess;
    private string _workingDirectory;
    private string _executable;

    public BazaarSourceControl ( ) : base( "bzr" ) {

    }
    /// <summary>
    /// Gets or sets the executable.
    /// </summary>
    /// <value>The executable.</value>
    [ Description( "Path to the Bazaar executable." ), Category( "Optional" ), DefaultValue( null ),
    FileTypeFilter( "*.exe|Application Files" ),Editor( typeof( OpenFileDialogUIEditor ),typeof( UITypeEditor ) ),
    OpenFileDialogTitle( "Select Bazaar Executable." )]
    public string Executable {
      get { return _executable; }
      set { _executable = value; }
    }
    /// <summary>
    /// Gets or sets the working directory.
    /// </summary>
    /// <value>The working directory.</value>
    [Description ( "The root folder where the latest source will be retrieved to." ), Category ( "Optional" ), DefaultValue ( null ),
   Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) )]
    public string WorkingDirectory {
      get { return _workingDirectory; }
      set { _workingDirectory = value; }
    }
    /// <summary>
    /// Gets or sets the label on success.
    /// </summary>
    /// <value>The label on success.</value>
    [Description ( "Specifies whether or not the repository should be labelled after a successful build." ), 
    Category ( "Optional" ), DefaultValue ( null ),
    Editor( typeof( DefaultableBooleanUIEditor ),typeof( UITypeEditor ) ), 
    TypeConverter( typeof( DefaultableBooleanTypeConverter ) )]
    public bool? LabelOnSuccess {
      get { return _tagOnSuccess; }
      set { _tagOnSuccess = value; }
    }
    /// <summary>
    /// Gets or sets the auto get source.
    /// </summary>
    /// <value>The auto get source.</value>
    [Description ( "Specifies whether the current version of the source should be retrieved." ),
    Category ( "Optional" ), DefaultValue ( null ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? AutoGetSource {
      get { return _autoGetSource; }
      set { _autoGetSource = value; }
    }
    /// <summary>
    /// Gets or sets the web URL builder.
    /// </summary>
    /// <value>The web URL builder.</value>
    [Description ( "The url builder section for the ViewCVS server." ), DefaultValue ( null ),
    TypeConverter ( typeof ( ExpandableObjectConverter ) ), Category ( "Optional" )]
    public WebUrlBuilder WebUrlBuilder {
      get { return _webUrlBuilder; }
      set { _webUrlBuilder = value; }
    }
    /// <summary>
    /// Gets or sets the branch URL.
    /// </summary>
    /// <value>The branch URL.</value>
    [Description ( "The url of the branch." ), DefaultValue ( null ), Category ( "Optional" )]
    public Uri BranchUrl {
      get { return _branchUrl; }
      set { _branchUrl = value; }
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );

      if ( !string.IsNullOrEmpty ( this.Executable ) ) {
        XmlElement ele = doc.CreateElement ( "executable" );
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      if ( this.BranchUrl != null ) {
        XmlElement ele = doc.CreateElement ( "branchUrl" );
        ele.InnerText = this.BranchUrl.ToString ( );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        XmlElement ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );
      }

      if ( this.AutoGetSource.HasValue ) {
        XmlElement ele = doc.CreateElement ( "autoGetSource" );
        ele.InnerText = this.AutoGetSource.Value.ToString ( );
        root.AppendChild ( ele );
      }

      if ( this.LabelOnSuccess.HasValue ) {
        XmlElement ele = doc.CreateElement ( "tagOnSuccess" );
        ele.InnerText = this.LabelOnSuccess.Value.ToString(  );
        root.AppendChild ( ele );
      }

      if ( this.WebUrlBuilder != null ) {
        XmlElement ele = this.WebUrlBuilder.Serialize ( );
        if ( ele != null ) {
          root.AppendChild ( doc.ImportNode ( ele, true ) );
        }
      }
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );
    
      this.AutoGetSource = null;
      this.BranchUrl = null;
      this.Executable = string.Empty;
      this.LabelOnSuccess = null;
      this.WebUrlBuilder = null;
      this.WorkingDirectory = string.Empty;

      string s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "tagOnSuccess", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.LabelOnSuccess = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "branchUrl", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.BranchUrl = new Uri ( s );

      s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.WorkingDirectory = s;

      XmlElement ele = element.SelectSingleNode ( "webUrlBuilder" ) as XmlElement;
      if ( ele != null )
        this.WebUrlBuilder.Deserialize ( ele );
    }

    /// <summary>
    /// Creates a copy of the source control object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone ( ) {
      BazaarSourceControl bsc = this.MemberwiseClone ( ) as BazaarSourceControl;
      bsc.WebUrlBuilder = this.WebUrlBuilder.Clone ( );
      bsc.BranchUrl = new Uri ( this.BranchUrl.ToString ( ) );
      return bsc;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [ EditorBrowsable( EditorBrowsableState.Never ),Browsable( false ) ]
    public Uri DocumentationUri {
      get { return new Uri( "http://www.sorn.net/projects/bazaar-ccnet/" ); }
    }

    #endregion
  }
}
