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
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// 
  /// </summary>
  [Plugin, MinimumVersion("1.3")]
  public class LastChangeVersionLabeller : Labeller{
    private int? _major = 1;
    private int? _minor = 0;
    private string _separator = ".";
    private bool? _incrementOnFailure = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="LastChangeVersionLabeller"/> class.
    /// </summary>
    public LastChangeVersionLabeller ( ) : base("lastChangeVersionLabeller") {

    }
    /// <summary>
    /// Gets or sets the separator.
    /// </summary>
    /// <value>The separator.</value>
    [Description("String used to separate the sections of the label. default = '.'"),
    Category("Optional"),DefaultValue(".")]
    public string Separator { get { return this._separator; } set { this._separator = value; } }
    /// <summary>
    /// Gets or sets the major.
    /// </summary>
    /// <value>The major.</value>
    [Description("The version Major version number."),Category("Required"),Editor(typeof(NumericUpDownUIEditor),typeof(UITypeEditor)),
  MinimumValue ( 0 ), MaximumValue ( int.MaxValue ), DisplayName ( "(Major)" ), DefaultValue ( 0 )]
    public int Major { get { return this._major.Value; } set { this._major = Util.CheckRequired(this,"major",value); } }
    /// <summary>
    /// Gets or sets the minor.
    /// </summary>
    /// <value>The minor.</value>
    [Description ( "The version Minor version number." ), Category ( "Required" ), Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ),
    MinimumValue ( 0 ), MaximumValue ( int.MaxValue ),DisplayName("(Minor)"),DefaultValue(0)]
    public int Minor { get { return this._minor.Value; } set { this._minor = Util.CheckRequired ( this, "minor", value ); } }
    /// <summary>
    /// Gets or sets the increment on failure.
    /// </summary>
    /// <value>The increment on failure.</value>
    [Description ( "If true, the label will be incremented even if the build fails. Otherwise it will only be incremented if the build succeeds." ),
    Category("Optional"),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
    DefaultValue(null)]
    public bool? IncrementOnFailure { get { return this._incrementOnFailure; } set { this._incrementOnFailure = value; } }
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
      if ( !string.IsNullOrEmpty ( this.Separator ) ) {
        ele = doc.CreateElement ( "separator" );
        ele.InnerText = this.Separator;
        root.AppendChild ( ele );
      }

      ele = doc.CreateElement ( "major" );
      ele.InnerText = this.Major.ToString ( );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "minor" );
      ele.InnerText = this.Minor.ToString ( );
      root.AppendChild ( ele );

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
      this.Separator = ".";
      this.IncrementOnFailure = null;
      this.Major = 1;
      this.Minor = 0;

      string s = Util.GetElementOrAttributeValue ( "separator", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Separator = s;
      s = Util.GetElementOrAttributeValue ( "incrementOnFailure", element );
      if ( !string.IsNullOrEmpty ( s ) ) 
        this.IncrementOnFailure = string.Compare ( s, bool.TrueString, true ) == 0;

      int i = 0;
      if ( int.TryParse ( Util.GetElementOrAttributeValue ( "major", element ), out i ) )
        this.Major = i;
      if ( int.TryParse ( Util.GetElementOrAttributeValue ( "minor", element ), out i ) )
        this.Minor = i;
      

    }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone ( ) {
      LastChangeVersionLabeller lcvl = this.MemberwiseClone ( ) as LastChangeVersionLabeller;
      return lcvl;
    }

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://ccnetconfig.org" ); }
    }
  }
}
