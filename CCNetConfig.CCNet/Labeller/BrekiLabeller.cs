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
using System.Drawing.Design;
using CCNetConfig.Core.Components;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Enables projects to be labelled in the form of a version number (Major.Minor.Build.Revision, e.g. 
  /// 2.5.22.1233), with the optional project prefix (e.g. MyProject-1.3.24.5). In some respects, 
  /// the labeller follows labeling schemes used in the NAnt Contrib &lt;version&gt; task, but there are 
  /// some differences.
  /// </summary>
  [TypeConverter ( typeof ( ExpandableObjectConverter ) ),
  MinimumVersion ( "1.0" ), Plugin]
  public class BrekiLabeller : Labeller, ICCNetDocumentation {
    /// <summary>
    /// Defines how the build number (x.x.B.x) will be calculated.
    /// </summary>
    public enum BuildNumberSchemeType : int {
      /// <summary>
      /// increments an existing build number
      /// </summary>
      Increment = 1,
      /// <summary>
      /// uses an existing build number and does not change it
      /// </summary>
      NoIncrement = 2,
      /// <summary>
      /// generates build number based on the release start date and formula: 
      /// number of months since release start * 100 + current day in month
      /// </summary>
      MonthDaySinceReleaseStart = 0,
    }

    /// <summary>
    /// Defines the algorithm for generating release numbers (x.x.x.R)
    /// </summary>
    public enum RevisionNumberSchemeType : int {
      /// <summary>
      /// increments an existing revision number if the previous build was on the same day as the new 
      /// one or resets it to 0 if those days differ
      /// </summary>
      DailyIncremental = 0,
      /// <summary>
      /// number of seconds since the start of today / 10
      /// </summary>
      SecondsSinceMidnight = 1,
    }

    private string _versionFilePath = string.Empty;
    private int? _majorNum = null;
    private int? _minorNum = null;
    private DateTime? _releaseStartDate = null;
    private BuildNumberSchemeType? _buildNumberScheme = null;
    private RevisionNumberSchemeType? _revisionNumberScheme = null;
    private string _prefix = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrekiLabeller"/> class.
    /// </summary>
    public BrekiLabeller ()
      : base ( "BrekiLabeller" ) {

    }

    /// <summary>
    /// Path to the file containing the version information. This text file should only contain version, 
    /// which should be in the format "Major.Minor.Build.Revision" (NOTE: build and release numbers are 
    /// ignored, but are necessary for the labeller to be able to parse the file). This information is 
    /// used to extract the major and minor numbers and release start date (labeller uses last modified 
    /// time of the file as a release start date).
    /// </summary>
    /// <value>The version file path.</value>
    [Description ( "Path to the file containing the version information. This text file should only " +
     "contain version, which should be in the format \"Major.Minor.Build.Revision\" " +
     "(NOTE: build and release numbers are ignored, but are necessary for the labeller to be able to " +
     "parse the file). This information is used to extract the major and minor numbers and release start " +
     "date (labeller uses last modified time of the file as a release start date)." ), Category ( "Optional" ),
   DefaultValue ( null ),Editor( typeof( OpenFileDialogUIEditor ),typeof( UITypeEditor ) ), FileTypeFilter( "All Files (*.*)|*.*" ),
    OpenFileDialogTitle ( "Select Version File." ), ReflectorName ( "VersionFilePath" )]
    public string VersionFilePath { get { return this._versionFilePath; } set { this._versionFilePath = value; } }
    /// <summary>
    /// Gets or sets the major number of the build.
    /// </summary>
    /// <value>The major number.</value>
    [Description ( "Major number for the build." ), Category ( "Optional" ), DefaultValue ( null ),
    ReflectorName ( "MajorNumber" )]
    public int? MajorNumber { get { return this._majorNum; } set { this._majorNum = value; } }
    /// <summary>
    /// Gets or sets the minor number of the build.
    /// </summary>
    /// <value>The minor number.</value>
    [Description ( "Minor number for the build." ), Category ( "Optional" ), DefaultValue ( null ),
    ReflectorName ( "MinorNumber" )]
    public int? MinorNumber { get { return this._minorNum; } set { this._minorNum = value; } }
    /// <summary>
    /// Defines how the build number (x.x.B.x) will be calculated.
    /// </summary>
    /// <value>The build number scheme.</value>
    [Description ( "Defines how the build number (x.x.B.x) will be calculated." ),
   Category ( "Optional" ), DefaultValue ( null ), TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ),
   Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ), ReflectorName ( "BuildNumberScheme" )]
    public BuildNumberSchemeType? BuildNumberScheme { get { return this._buildNumberScheme; } set { this._buildNumberScheme = value; } }
    /// <summary>
    /// Defines the algorithm for generating release numbers (x.x.x.R).
    /// </summary>
    /// <value>The build number scheme.</value>
    [Description ( "Defines the algorithm for generating release numbers (x.x.x.R)." ),
   Category ( "Optional" ), DefaultValue ( null ), TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ),
   Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ), ReflectorName ( "RevisionNumberScheme" )]
    public RevisionNumberSchemeType? RevisionNumberScheme { get { return this._revisionNumberScheme; } set { this._revisionNumberScheme = value; } }
    /// <summary>
    /// Release date to use to set the muild/revision.
    /// </summary>
    /// <value>The release start date.</value>
    [Description ( "Release date to use to set the muild/revision." ),
   Category ( "Optional" ), DefaultValue ( null ), FormatProvider("M/d/yyyy"),
   Editor ( typeof ( DatePickerUIEditor ), typeof ( UITypeEditor ) ), ReflectorName ( "ReleaseStartDate" )]
    public DateTime? ReleaseStartDate { get { return this._releaseStartDate; } set { this._releaseStartDate = value; } }
    /// <summary>
    /// defines a text which will be prefixed to the version number to form a label (example: 
    /// MyProject-1.3.43.2). This is useful when there are several projects building in the same 
    /// source control repository and you want to be able to distinguish labels from different 
    /// projects.
    /// </summary>
    /// <value>The prefix.</value>
    [Description ( "defines a text which will be prefixed to the version number to form a label " +
      "(example: MyProject-1.3.43.2). This is useful when there are several projects building in " +
      "the same source control repository and you want to be able to distinguish labels from different " +
      "projects." ), ReflectorName("Prefix"),
    Category ( "Optional" ), DefaultValue ( null )]
    public string Prefix { get { return this._prefix; } set { this._prefix = value; } }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      return new CCNetConfig.Core.Serialization.Serializer<BrekiLabeller> ( ).Serialize(this);
      /*
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "labeller" );
      root.SetAttribute ( "type", this.TypeName );
      XmlElement ele = null;

      if ( !string.IsNullOrEmpty ( this.Prefix ) ) {
        ele = doc.CreateElement ( "Prefix" );
        ele.InnerText = this.Prefix;
        root.AppendChild ( ele );
      }

      if ( this.BuildNumberScheme.HasValue ) {
        ele = doc.CreateElement ( "BuildNumberScheme" );
        ele.InnerText = this.BuildNumberScheme.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.RevisionNumberScheme.HasValue ) {
        ele = doc.CreateElement ( "RevisionNumberScheme" );
        ele.InnerText = this.RevisionNumberScheme.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.VersionFilePath ) ) {
        ele = doc.CreateElement ( "VersionFilePath" );
        ele.InnerText = this.VersionFilePath;
        root.AppendChild ( ele );
      }

      if ( this.MajorNumber.HasValue ) {
        ele = doc.CreateElement ( "MajorNumber" );
        ele.InnerText = this.MajorNumber.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.MinorNumber.HasValue ) {
        ele = doc.CreateElement ( "MinorNumber" );
        ele.InnerText = this.MinorNumber.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.ReleaseStartDate.HasValue ) {
        ele = doc.CreateElement ( "ReleaseStartDate" );
        ele.InnerText = this.ReleaseStartDate.Value.ToString ( "M/d/yyyy" );
        root.AppendChild ( ele );
      }

      return root;
      */
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.BuildNumberScheme = null;
      this.MajorNumber = null;
      this.MinorNumber = null;
      this.Prefix = string.Empty;
      this.ReleaseStartDate = null;
      this.RevisionNumberScheme = null;
      this.VersionFilePath = string.Empty;

      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );
      
      DateTime dt = DateTime.Now;
      string s = Util.GetElementOrAttributeValue ( "ReleaseStartDate", element );
      if ( !string.IsNullOrEmpty ( s ) )
        DateTime.TryParse ( s, out dt );
      this.ReleaseStartDate = dt;

      s = Util.GetElementOrAttributeValue ( "Prefix", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Prefix = s;

      int val = 0;
      s = Util.GetElementOrAttributeValue( "MajorNumber",element );
      if ( int.TryParse ( s, out val ) )
        this.MajorNumber = val;

      s = Util.GetElementOrAttributeValue ( "MinorNumber", element );
      if ( int.TryParse ( s, out val ) )
        this.MinorNumber = val;

      s = Util.GetElementOrAttributeValue ( "VersionFilePath", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.VersionFilePath = s;

      s = Util.GetElementOrAttributeValue( "BuildNumberScheme",element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.BuildNumberScheme = (BrekiLabeller.BuildNumberSchemeType)Enum.Parse ( typeof ( BrekiLabeller.BuildNumberSchemeType ), s );

      s = Util.GetElementOrAttributeValue( "RevisionNumberScheme",element );
      if (!string.IsNullOrEmpty( s ) )
        this.RevisionNumberScheme = (BrekiLabeller.RevisionNumberSchemeType)Enum.Parse( typeof( BrekiLabeller.RevisionNumberSchemeType ), s) ;
    }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone () {
      return this.MemberwiseClone () as BrekiLabeller;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false ), ReflectorIgnore]
    public Uri DocumentationUri {
      get { return new Uri ( "http://www.igorbrejc.com/content/view/14/27/" ); }
    }

    #endregion
  }
}
