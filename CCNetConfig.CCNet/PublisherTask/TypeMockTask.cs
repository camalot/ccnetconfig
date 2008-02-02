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
using CCNetConfig.Core.Components;
using CCNetConfig.Core;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Enables NUnit testing with TypeMock in CruiseControl.NET
  /// see <a href="http://www.typemock.com/community/viewtopic.php?t=380">CuriseControl.NET plugin for TypeMock</a>
  /// </summary>
  [MinimumVersion ( "1.0" ), Plugin ]
  public class TypeMockTask : PublisherTask, ICCNetDocumentation  {
    private CloneableList<string> _assemblies = null;
    private string _nunitPath = string.Empty;
    private string _outputFile = string.Empty;
    private int? _timeout = null;
    private string _runnerPath = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeMockTask"/> class.
    /// </summary>
    public TypeMockTask ( )
      : base ( "TMockRunner" ) {
      _assemblies = new CloneableList<string> ( );
    }

    /// <summary>
    /// Gets or sets the runner path.
    /// </summary>
    /// <value>The runner path.</value>
    [Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ),
FileTypeFilter ( "TypeMockRunner|tmockrunner.exe;" ),
Category ( "Optional" ),
OpenFileDialogTitle ( "Path to TypeMock runner." ), DefaultValue ( null )]
    public string RunnerPath {
      get { return _runnerPath; }
      set { _runnerPath = value; }
    }
	
    /// <summary>
    /// Gets or sets the timeout.
    /// </summary>
    /// <value>The timeout.</value>
    [ Category( "Optional" ), Description( "The time in milliseconds for the task to timeout. Use 0 for no timeout." ),
    Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ),
    MinimumValue( 0 ), DefaultValue( 0 )]
    public int? Timeout {
      get { return _timeout; }
      set { _timeout = value; }
    }

    /// <summary>
    /// Gets or sets the output file.
    /// </summary>
    /// <value>The output file.</value>
    [ Editor( typeof( OpenFileDialogUIEditor ), typeof (  UITypeEditor ) ),
    FileTypeFilter("Xml Files|*.xml|All Files (*.*)|*.*"),
    Category( "Optional" ),
    OpenFileDialogTitle( "Name of the output file." ), DefaultValue( null ) ]
    public string OutputFile {
      get { return _outputFile; }
      set { _outputFile = value; }
    }

    /// <summary>
    /// Gets or sets the NUnit path.
    /// </summary>
    /// <value>The NUnit path.</value>
    [ Category( "Required" ), DisplayName( "(NUnitPath)" ),
Description ( "Path of nunit-console.exe application." ), FileTypeFilter ( "NUnit|nunit-console.exe" ),
    Editor( typeof( OpenFileDialogUIEditor ), typeof (  UITypeEditor ) ),
  OpenFileDialogTitle ( "Select NUnit-Console Executable" ), DefaultValue ( null )]
    public string NUnitPath {
      get { return _nunitPath; }
      set { _nunitPath = Util.CheckRequired(this,"NUnitPath", value); }
    }

    /// <summary>
    /// Gets or sets the assemblies.
    /// </summary>
    /// <value>The assemblies.</value>
    [Category ( "Optional" ), Description ( "Assemblies to include in the tests." ), DefaultValue ( null ),
    Editor ( typeof ( StringListUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( StringListTypeConverter ) )]
    public CloneableList<string> Assemblies {
      get { return _assemblies; }
      set { _assemblies = value; }
    }


    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [ Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ) ]
    public Uri DocumentationUri {
      get { return new Uri ( "http://www.typemock.com/community/viewtopic.php?t=380" ); }
    }

    #endregion

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( this.TypeName );

      if ( !string.IsNullOrEmpty ( this.NUnitPath ) ) {
        XmlElement ele = doc.CreateElement ( "nunitPath" );
        ele.InnerText = this.NUnitPath;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.OutputFile ) ) {
        XmlElement ele = doc.CreateElement ( "outputfile" );
        ele.InnerText = this.OutputFile;
        root.AppendChild ( ele );
      }

      XmlElement asmE = doc.CreateElement ( "assemblies" );
      foreach ( string fi in this.Assemblies ) {
        XmlElement ele = doc.CreateElement ( "assembly" );
        ele.InnerText = fi;
        asmE.AppendChild ( ele );
      }
      root.AppendChild ( asmE );

      if ( this.Timeout.HasValue ) {
        XmlElement ele = doc.CreateElement ( "timeout" );
        ele.InnerText = this.Timeout.Value.ToString ( );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.RunnerPath ) ) {
        XmlElement ele = doc.CreateElement ( "runnerPath" );
        ele.InnerText = this.RunnerPath;
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this._assemblies = new CloneableList<string> ( );
      this.OutputFile = string.Empty;
      this._nunitPath = string.Empty;
      this.RunnerPath = string.Empty;
      this.Timeout = null;

      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      XmlElement ele = ( XmlElement ) element.SelectSingleNode ( "assemblies" );
      if ( ele != null ) {
        foreach ( XmlElement aele in ele.SelectNodes ( "assembly" ) )
          this.Assemblies.Add ( aele.InnerText );
      }

      string s = Util.GetElementOrAttributeValue ( "nunitPath", element );
      this.NUnitPath = s;

      s = Util.GetElementOrAttributeValue ( "runnerPath", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.RunnerPath = s;

      s = Util.GetElementOrAttributeValue ( "outputfile", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.OutputFile = s;

      s = Util.GetElementOrAttributeValue ( "timeout", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse ( s, out i ) )
          this.Timeout = i;
      }
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      TypeMockTask tmt = this.MemberwiseClone ( ) as TypeMockTask;
      tmt.Assemblies = this.Assemblies.Clone ( );
      return tmt;
    }
  }
}
