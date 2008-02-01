/*
 * Copyright (c) 2006-2007, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Produces labels that match the version numbers that Visual Studio creates.
  /// </summary>
  [Plugin, MinimumVersion ( "1.0" )]
  public class MSProjectLabeller : Labeller, ICCNetDocumentation {
    private bool? _incrementOnFailure = null;
    private int? _major = null;
    private int? _minor = null;
    private DateTime? _releaseDate = null;


    /// <summary>
    /// Initializes a new instance of the <see cref="MSProjectLabeller"/> class.
    /// </summary>
    public MSProjectLabeller ( )
      : base ( "msprojectlabeller" ) {

    }
    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    /// <value>The release date.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "The date of the iteration release" ),
   Editor ( typeof ( DatePickerUIEditor ), typeof ( UITypeEditor ) )]
    public DateTime? ReleaseDate {
      get { return _releaseDate; }
      set { _releaseDate = value; }
    }

    /// <summary>
    /// Gets or sets the minor.
    /// </summary>
    /// <value>The minor.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "The Minor value of the version." ),
   Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ), MinimumValue ( 0 )]
    public int? Minor {
      get { return _minor; }
      set { _minor = value; }
    }

    /// <summary>
    /// Gets or sets the major.
    /// </summary>
    /// <value>The major.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "The Major value of the version." ),
   Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ), MinimumValue ( 0 )]
    public int? Major {
      get { return _major; }
      set { _major = value; }
    }

    /// <summary>
    /// Gets or sets the increment on failure.
    /// </summary>
    /// <value>The increment on failure.</value>
    [Category ( "Optional" ), DefaultValue ( null ), Description ( "Indicates if the label should be incremented even if the build fails." ),
   Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
    public bool? IncrementOnFailure {
      get { return _incrementOnFailure; }
      set { _incrementOnFailure = value; }
    }


    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( "labeller" );

      root.SetAttribute ( "type", this.TypeName );
      XmlElement ele = null;
      if ( this.ReleaseDate.HasValue ) {
        ele = doc.CreateElement ( "releaseStartDate" );
        ele.InnerText = this.ReleaseDate.Value.ToString( "yyyy/MM/dd" );
        root.AppendChild ( ele );
      }

      if ( this.Major.HasValue ) {
        ele = doc.CreateElement ( "labelMajor" );
        ele.InnerText = this.Major.Value.ToString ( );
        root.AppendChild ( ele );
      }

      if ( this.Minor.HasValue ) {
        ele = doc.CreateElement ( "labelMinor" );
        ele.InnerText = this.Minor.Value.ToString ( );
        root.AppendChild ( ele );
      }

      if ( this.IncrementOnFailure.HasValue ) {
        ele = doc.CreateElement ( "incrementOnFailure" );
        ele.InnerText = this.IncrementOnFailure.Value.ToString ( );
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.IncrementOnFailure = null;
      this.Major = null;
      this.Minor = null;
      this.ReleaseDate = null;

      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

      string s = Util.GetElementOrAttributeValue ( "releaseStartDate", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ReleaseDate = DateTime.Parse( s );

      s = Util.GetElementOrAttributeValue ( "labelMinor", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse ( s, out i ) )
          this.Minor = i;
      }

      s = Util.GetElementOrAttributeValue ( "labelMajor", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse ( s, out i ) )
          this.Major = i;
      }

      s = Util.GetElementOrAttributeValue ( "incrementOnFailure", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        bool i = false;
        if ( bool.TryParse ( s, out i ) )
          this.IncrementOnFailure = i;
      }

    }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone ( ) {
      MSProjectLabeller mspl = this.MemberwiseClone ( ) as MSProjectLabeller;
      if ( this.ReleaseDate.HasValue )
        mspl.ReleaseDate = new DateTime ( this.ReleaseDate.Value.Ticks );
      return mspl;
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [ Browsable( false ),EditorBrowsable( EditorBrowsableState.Never ) ]
    public Uri DocumentationUri {
      get { return new Uri ( "http://nimtug.org/blogs/simon/pages/CCNet-MS-Project-Labeller.aspx" ); }
    }

    #endregion
  }
}
