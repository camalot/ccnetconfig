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
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Drawing.Design;
using CCNetConfig.Core.Serialization;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// 
  /// </summary>
  [MinimumVersion ( "1.3.0.3053" )]
  public class ExternalSourceControl : SourceControl, ICCNetDocumentation {

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSourceControl"/> class.
    /// </summary>
    public ExternalSourceControl ( )
      : base ( "external" ) {
      Environment = new CloneableList<EnvironmentVariable> ( );
    }

    /// <summary>
    /// Gets or sets the executable.
    /// </summary>
    /// <value>The executable.</value>
    [ReflectorName ( "executable" ), Required, DefaultValue ( null ),
    Description ( "Specifies the path to the source control command." ),
    DisplayName ( "(Executable)" ), Category ( "Required" ),
    Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ),
    OpenFileDialogTitle ( "Select the source control executable" ),
    FileTypeFilter ( "Executables|*.exe;*.cmd;*.bat|All Files|*.*" )]
    public string Executable { get; set; }

    /// <summary>
    /// Gets or sets the arguments.
    /// </summary>
    /// <value>The arguments.</value>
    [ReflectorName ( "args" ), Description ( "Specifies the command line arguments to be passed to the source control command." ),
    DefaultValue ( null ), Category ( "Optional" )]
    public string Arguments { get; set; }

    /// <summary>
    /// Gets or sets the auto get source.
    /// </summary>
    /// <value>The auto get source.</value>
    [ReflectorName ( "autoGetSource" ),
    Description ( "Specifies whether the current version of the source should be retrieved from the source control system." ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    DefaultValue ( null ), Category ( "Optional" )]
    public bool? AutoGetSource { get; set; }

    /// <summary>
    /// Gets or sets the label on success.
    /// </summary>
    /// <value>The label on success.</value>
    [ReflectorName ( "labelOnSuccess" ),
    Description ( "Specifies whether or not CruiseControl.Net should ask the source control system to label the source when the build is successful." ),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
    DefaultValue ( null ), Category ( "Optional" )]
    public bool? LabelOnSuccess { get; set; }
    /// <summary>
    /// Gets or sets the timeout.
    /// </summary>
    /// <value>The timeout.</value>
    [ReflectorName ( "timeout" ), Category ( "Optional" ),
    TypeConverter ( typeof ( ObjectOrNoneTypeConverter ) ),
    Editor ( typeof ( ObjectOrNoneUIEditor ), typeof ( UITypeEditor ) ),
    DefaultValue ( null ), Description ( "Sets the timeout period for the source control operation." )]
    public Timeout Timeout { get; set; }

    /// <summary>
    /// Gets or sets the environment.
    /// </summary>
    /// <value>The environment.</value>
    [ReflectorArray ( "variable" ), ReflectorName ( "environment" ), Editor ( typeof ( CollectionEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<EnvironmentVariable> Environment { get; set; }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<ExternalSourceControl> ( ).Serialize ( this );
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.Arguments = string.Empty;
      this.AutoGetSource = null;
      this.Environment = new CloneableList<EnvironmentVariable> ( );
      this.LabelOnSuccess = null;
      this.Timeout = new Timeout ( );

      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

      this.Executable = Util.GetElementOrAttributeValue ( "executable", element );

      string s = Util.GetElementOrAttributeValue ( "labelOnSuccess", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.LabelOnSuccess = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "args", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Arguments = s;

      XmlElement ele = ( XmlElement ) element.SelectSingleNode ( "timeout" );
      if ( ele != null )
        this.Timeout.Deserialize ( ele );

      ele = ( XmlElement ) element.SelectSingleNode ( "environment" );
      if ( ele != null ) {
        foreach ( XmlElement ele1 in ele.SelectNodes ( "./*" ) ) {
          EnvironmentVariable ev = new EnvironmentVariable ( );
          ev.Deserialize ( ele1 );
          this.Environment.Add ( ev );
        }
      }

    }

    /// <summary>
    /// Creates a copy of the source control object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone ( ) {
      ExternalSourceControl esc = this.MemberwiseClone ( ) as ExternalSourceControl;
      esc.Timeout = this.Timeout.Clone ( );

      return esc;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [EditorBrowsable ( EditorBrowsableState.Never ), Browsable ( false ), ReflectorIgnore]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/External+Source+Control?decorator=printable" ); }
    }

    #endregion
  }
}
