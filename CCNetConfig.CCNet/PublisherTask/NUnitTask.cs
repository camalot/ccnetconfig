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
using System.IO;
using System.Xml;
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Collections;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// <para>This task enables you to instruct CCNet to run the unit tests contained within a collection of assemblies. 
  /// The results of the unit tests will be automatically included in the CCNet build results. This can be useful if you have 
  /// some unit tests that you want to run as part of the integration process, but you don't need as part of your developer 
  /// build process. For example, if you have a set of integration tests that you want to run in a separate build process, 
  /// it is easy to set up a project to use this task.</para>
  /// <para>If you are using the <see cref="CCNetConfig.CCNet.VisualStudioTask">Visual Studio Task</see> and you want to run unit tests then you probably want to use this task. Alternatively 
  /// you can run NUnit using post-build tasks in your Visual Studio project properties.</para>
  /// </summary>
  /// <remarks>
  /// See 
  /// <a href="http://confluence.public.thoughtworks.org/display/CCNET/Using+CruiseControl.NET+with+NUnit">Using CruiseControl.NET with NUnit</a> 
  /// for more details.
  /// </remarks>
  [  MinimumVersion( "1.0" ) ]
  public class NUnitTask : PublisherTask, ICCNetDocumentation {
    private string _path = null;
    private string _outfile = string.Empty;
    private CloneableList<string> _assemblies;
    private int? _timeout = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="NUnitTask"/> class.
    /// </summary>
    public NUnitTask () : base ("nunit") {
      _assemblies = new CloneableList<string> ();
    }

    /// <summary>
    /// Path of nunit-console.exe application.
    /// </summary>
    [Description ( "Path of nunit-console.exe application." ), DefaultValue ( null ), Category ( "Optional" ),
 Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "NUnit|nunit-console.exe" ),
    OpenFileDialogTitle( "Select NUnit-Console Executable" )]
    public string Path { get { return this._path; } set { this._path = value; } }
    /// <summary>
    /// The file that NUnit will write the test results to.
    /// </summary>
    [Description ( "The file that NUnit will write the test results to." ), DefaultValue ( null ),
   Category ( "Optional" ),
 Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "All Files|*.*" ),
OpenFileDialogTitle ( "Select NUnit output file" )]
    public string OutputFile { get { return this._outfile; } set { this._outfile = value; } }
    /// <summary>
    /// List of the paths to the assemblies containing the NUnit tests to be run.
    /// </summary>
    [Description ("List of the paths to the assemblies containing the NUnit tests to be run."), DefaultValue (null), DisplayName("(Assemblies)"),
   Editor ( typeof ( StringListUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( StringListTypeConverter ) ), Category ( "Required" )]
    public CloneableList<string> Assemblies { get { return this._assemblies; } set { this._assemblies = Util.CheckRequired ( this, "assemblies", value ); } }
    /// <summary>
    /// The number of seconds that the nunit process will run before timing out.
    /// </summary>
    [Description ( "The number of seconds that the nunit process will run before timing out." ), DefaultValue ( null ), Category ( "Optional" )]
    public int? Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      NUnitTask nt = this.MemberwiseClone () as NUnitTask;
      nt.Assemblies = this.Assemblies.Clone ();
      return nt;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      if ( !string.IsNullOrEmpty(this.Path) ) {
        XmlElement ele = doc.CreateElement ( "path" );
        ele.InnerText = this.Path;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty(this.OutputFile) ) {
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
        ele.InnerText = this.Timeout.Value.ToString();
        root.AppendChild ( ele );
      }
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this._assemblies = new CloneableList<string> ();
      this.OutputFile = string.Empty;
      this.Path = string.Empty;
      this.Timeout = null;

      if ( string.Compare (element.Name, this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.Name, this.TypeName));

      XmlElement ele = (XmlElement)element.SelectSingleNode ("assemblies");
      if ( ele != null ) {
        foreach ( XmlElement aele in ele.SelectNodes ("assembly") )
          this.Assemblies.Add (aele.InnerText);
      }

      string s = Util.GetElementOrAttributeValue ("path", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Path = s;

      s = Util.GetElementOrAttributeValue ("outputfile", element);
      if ( !string.IsNullOrEmpty (s) )
        this.OutputFile = s;

      s = Util.GetElementOrAttributeValue ("timeout", element);
      if ( !string.IsNullOrEmpty (s) ) {
        int i = 0;
        if ( int.TryParse (s, out i) )
          this.Timeout = i;
      }
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ("http://confluence.public.thoughtworks.org/display/CCNET/NUnit+Task?decorator=printable"); }
    }

    #endregion
  }
}
