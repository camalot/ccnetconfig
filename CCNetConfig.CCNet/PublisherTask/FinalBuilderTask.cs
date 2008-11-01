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
using CCNetConfig.Core.Components;
using CCNetConfig.Core;
using System.ComponentModel;
using System.Threading;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The FinalBuilder Task allows you to invoke FinalBuilder build projects as part of a CruiseControl.NET 
  /// integration project.
  /// </summary>
  /// <remarks>
  /// FinalBuilder is a commercial build and release management solution for Windows software developers and SCM
  /// professionals, developed and marketed by VSoft Technologies.
  /// </remarks>
  [MinimumVersion ( "1.3" ), DefaultProperty ( "ProjectFile" )]
  public class FinalBuilderTask : PublisherTask, ICCNetDocumentation {

    private string _projectFile = string.Empty;
    private int? _version = null;
    private CloneableList<NameValue> _variables = null;
    private bool? _showBanner = null;
    private int? _timeout = null;
    private bool? _dontWriteToLog = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinalBuilderTask"/> class.
    /// </summary>
    public FinalBuilderTask ()
      : base ( "FinalBuilder" ) {
      _variables = new CloneableList<NameValue> ();
    }

    /// <summary>
    /// The full path of the FinalBuilder project to run. This is the only required element. 
    /// </summary>
    /// <value>The project file.</value>
    [Description ( "The full path of the FinalBuilder project to run. This is the only required element." ),
   DefaultValue ( null ), DisplayName ( "(ProjectFile)" ), Category ( "Required" ),
   OpenFileDialogTitle ( "Select the FinalBuilder project to run." ), FileTypeFilter ( "FinalBuilder Projects|*.FBZ*|All Files (*.*)|*.*" ),
   Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) )]
    public string ProjectFile {
      get { return this._projectFile; }
      set { this._projectFile = Core.Util.CheckRequired ( this, "ProjectFile", value ); }
    }

    /// <summary>
    /// Use this element to explicitly specify a version of FinalBuilder to run (for instance, you could force a FinalBuilder 4 project to run in FinalBuilder 5.)
    /// </summary>
    /// <value>The version.</value>
    [Description ( "Use this element to explicitly specify a version of FinalBuilder " +
     "to run (for instance, you could force a FinalBuilder 4 project to run in FinalBuilder 5.)\n" +
     "If this element is not specified, the FinalBuilder version is determined automatically from the " +
     "project file name (recommended.)" ), DefaultValue ( null ), Category ( "Optional" )]
    public int? Version { get { return this._version; } set { this._version = value; } }

    /// <summary>
    /// Specify a variable to set inside the FinalBuilder project at runtime ("name"), and a value to set ("value").
    /// The variable should be defined in the project.
    /// </summary>
    /// <value>The variables.</value>
    [Description ( "Specify a variable to set inside the FinalBuilder project at runtime (\"name\"), and a " +
   "value to set (\"value\").\nThe variable should be defined in the project." ), DefaultValue ( null ),
    Category ( "Optional" ), TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<NameValue> Variables { get { return this._variables; } set { this._variables = value; } }


    /// <summary>
    /// Specify 'true' to enable the "banner" at the top of the FinalBuilder console output.
    /// </summary>
    /// <value>The show banner.</value>
    [Description ( "Specify 'true' to enable the \"banner\" at the top of the FinalBuilder console output." ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   DefaultValue ( null ), Category ( "Optional" )]
    public bool? ShowBanner { get { return this._showBanner; } set { this._showBanner = value; } }

    /// <summary>
    /// The number of seconds to wait before assuming that the FinalBuilder project has hung and should be killed.
    /// </summary>
    /// <value>The timeout.</value>
    /// <remarks>default is 0</remarks>
    [Description ( "The number of seconds to wait before assuming that the FinalBuilder project has hung and " +
      "should be killed." ), Category ( "Optional" ), DefaultValue ( null )]
    public int? Timeout { get { return this._timeout; } set { this._timeout = value; } }

    /// <summary>
    /// Disable output to the FinalBuilder project log file. 
    /// </summary>
    /// <value>if <c>true</c>, then do not write to the log.</value>
    [Description ( "Disable output to the FinalBuilder project log file." ), DefaultValue ( null ),
   Category ( "Optional" ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
   Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) )]
    public bool? DoNotWriteToLog { get { return this._dontWriteToLog; } set { this._dontWriteToLog = value; } }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));
      XmlElement ele = null;

      ele = doc.CreateElement ( "ProjectFile" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.ProjectFile );
      root.AppendChild ( ele );

      if ( Variables != null && Variables.Count > 0 ) {
        ele = doc.CreateElement ( "FBVariables" );
        foreach ( NameValue nv in this.Variables ) {
          XmlElement tele = doc.CreateElement ( "FBVariable" );
          tele.SetAttribute ( "name", nv.Name );
          tele.SetAttribute ( "value", nv.Value );
          ele.AppendChild ( tele );
        }
        root.AppendChild ( ele );
      }

      if ( this.ShowBanner.HasValue ) {
        ele = doc.CreateElement ( "ShowBanner" );
        ele.InnerText = this.ShowBanner.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.Timeout.HasValue ) {
        ele = doc.CreateElement ( "Timeout" );
        ele.InnerText = this.Timeout.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.DoNotWriteToLog.HasValue ) {
        ele = doc.CreateElement ( "DontWriteToLog" );
        ele.InnerText = this.DoNotWriteToLog.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.Version.HasValue ) {
        ele = doc.CreateElement ( "FBVersion" );
        ele.InnerText = this.Version.Value.ToString ();
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this._projectFile = string.Empty;
      this.Variables = new CloneableList<NameValue> ();
      this.ShowBanner = null;
      this.Timeout = null;
      this.DoNotWriteToLog = null;
      this.Version = null;

      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.ProjectFile = Util.GetElementOrAttributeValue ( "ProjectFile", element );
      string s = Util.GetElementOrAttributeValue ( "FBVersion", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse ( s, out i ) )
          this.Version = i;
      }

      s = Util.GetElementOrAttributeValue ( "DontWriteToLog", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.DoNotWriteToLog = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "ShowBanner", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ShowBanner = string.Compare ( s, bool.TrueString, true ) == 0;

      s = Util.GetElementOrAttributeValue ( "Timeout", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse ( s, out i ) )
          this.Timeout = i;
      }

      XmlElement subElement = element.SelectSingleNode ( "FBVariables" ) as XmlElement;
      if ( subElement != null ) {
        foreach ( XmlElement vEle in subElement.SelectNodes ( "./*" ) ) {
          NameValue nv = new NameValue ( vEle.GetAttribute ( "name" ) );
          nv.Value = vEle.GetAttribute ( "value" );
          this.Variables.Add ( nv );
        }
      }
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      return this.MemberwiseClone () as FinalBuilderTask;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/FinalBuilder+Task?decorator=printable" ); }
    }

    #endregion
  }
}
