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

namespace CCNetConfig.CCNet {
  /// <summary>
  /// 
  /// <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a> 
  /// expects NAnt to generate its output as Xml so that the build results can be parsed and rendered appropriately. 
  /// To accomplish this, <a href="http://confluence.public.thoughtworks.org/display/CCNET">CruiseControl.NET</a>
  /// will, by default, launch NAnt using the "<c>-logger:NAnt.Core.XmlLogger</c>" argument. If you want to 
  /// override this behaviour, specify the logger property in the NAntBuilder configuration in the <c>ccnet.config</c> file. If this element is 
  /// specified but is empty then NAnt will be started with the default logger (though this may cause some problems for 
  /// <a href="http://confluence.public.thoughtworks.org/display/CCNET">CCNET</a>). It is also 
  /// possible to instruct NAnt to log its output to an Xml file and then merge the file into the build using the 
  /// <see cref="CCNetConfig.CCNet.FileMergeTaskPublisher">File Merge Task</see>.
  /// 
  /// </summary>
  /// <remarks>
  /// 
  /// see <a href="http://confluence.public.thoughtworks.org/display/CCNET/NAnt+Task">NAnt Task</a> documentation.
  /// 
  /// </remarks>
  [ MinimumVersion( "1.0" ) ]
  public class NAntTask : PublisherTask, ICCNetDocumentation  {
    private string _executable = null;
    private string _baseDirectory = null;
    private string _buildArgs = string.Empty;
    private string _buildFile = string.Empty;
    private bool? _nologo = null;
    private string _logger = string.Empty;
    private int? _timeout = null;
    private CloneableList<string> _targets;

    /// <summary>
    /// Initializes a new instance of the <see cref="NAntTask"/> class.
    /// </summary>
    public NAntTask () : base ( "nant" ) {
      _targets = new CloneableList<string> ();
    }

    /// <summary>
    /// The path of the version of nant.exe you want to run. If this is relative, then must be relative to either (a) the base directory, 
    /// (b) the CCNet Server application, or (c) if the path doesn't contain any directory details then can be available in the system or 
    /// application's 'path' environment variable
    /// </summary>
    [Description ("The path of the version of nant.exe you want to run. If this is relative, then must be relative to either (a) the base directory," +
     "(b) the CCNet Server application, or (c) if the path doesn't contain any directory details then can be available in the system or" +
   "application's 'path' environment variable" ), DefaultValue ( null ), Category ( "Optional" ),
   Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "NAnt|nant.exe" ),
   OpenFileDialogTitle ( "Select NAnt executable." )]
    public string Executable { get { return this._executable; } set { this._executable = value; } }
    /// <summary>
    /// The directory to run the NAnt process in. If relative, is a subdirectory of the 
    /// <see cref="CCNetConfig.Core.Project.WorkingDirectory">Project Working Directory</see>
    /// </summary>
    [Description ( "The directory to run the NAnt process in. If relative, is a subdirectory of the Project Working Directory." ), DefaultValue ( null ),
   Category ( "Optional" ),
Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
BrowseForFolderDescription ( "Select path to the base directory." )]
    public string BaseDirectory { get { return this._baseDirectory; } set { this._baseDirectory = value; } }
    /// <summary>
    /// Any arguments to pass through to NAnt (e.g to specify build properties)
    /// </summary>
    [Description ( "Any arguments to pass through to NAnt (e.g to specify build properties)" ), DefaultValue ( null ), Category ( "Optional" )]
    public string BuildArguments { get { return this._buildArgs; } set { this._buildArgs = value; } }
    /// <summary>
    /// The name of the build file to run, relative to the baseDirectory.
    /// </summary>
    [Description ( "The name of the build file to run, relative to the baseDirectory." ), DefaultValue ( null ),
   Category ( "Optional" ),
  Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "Build Scripts|*.build|All Files|*.*" ),
  OpenFileDialogTitle ( "Select NAnt script" )]
    public string BuildFile { get { return this._buildFile; } set { this._buildFile = value; } }
    /// <summary>
    /// Whether to use the -nologo argument when calling NAnt
    /// </summary>
    [Description ("Whether to use the -nologo argument when calling NAnt."), DefaultValue (null),
   Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), 
    Category ( "Optional" )]
    public bool? NoLogo { get { return this._nologo; } set { this._nologo = value; } }
    /// <summary>
    /// The NAnt logger to use. If you are using a version of NAnt prior to 0.8.3, you may need to specify this as <c>SourceForge.NAnt.XmlLogger</c>.
    /// </summary>
    [Description ("The NAnt logger to use. If you are using a version of NAnt prior to 0.8.3, you may need to specify this as SourceForge.NAnt.XmlLogger."),
   DefaultValue ( null ), Category ( "Optional" )]
    public string Logger { get { return this._logger; } set { this._logger = value; } }
    /// <summary>
    /// Number of seconds to wait before assuming that the process has hung and should be killed.
    /// </summary>
    [Description ( "Number of seconds to wait before assuming that the process has hung and should be killed." ), DefaultValue ( null ), Category ( "Optional" )]
    public int? BuildTimeoutSeconds { get { return this._timeout; } set { this._timeout = value; } }
    /// <summary>
    /// A list of targets to be called. CruiseControl.NET does not call NAnt once for each target, 
    /// it uses the NAnt feature of being able to specify multiple targets.
    /// </summary>
    [Description ("A list of targets to be called. CruiseControl.NET does not call NAnt once for each target," +
      "it uses the NAnt feature of being able to specify multiple targets."), DefaultValue (null),
   Editor ( typeof ( StringListUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( StringListTypeConverter ) ), Category ( "Optional" )]
    public CloneableList<string> Targets { get { return this._targets; } set { this._targets = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      NAntTask nt = this.MemberwiseClone () as NAntTask;
      nt.Targets = this.Targets.Clone ();
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

      if ( !string.IsNullOrEmpty(this.Executable) ) {
        XmlElement ele = doc.CreateElement ( "executable" );
        ele.InnerText = this.Executable;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty(this.BaseDirectory) ) {
        XmlElement ele = doc.CreateElement ( "baseDirectory" );
        ele.InnerText = this.BaseDirectory;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty(this.BuildArguments) ) {
        XmlElement ele = doc.CreateElement ( "buildArgs" );
        ele.InnerText = this.BuildArguments;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty(this.BuildFile) ) {
        XmlElement ele = doc.CreateElement ( "buildFile" );
        ele.InnerText = this.BuildFile;
        root.AppendChild ( ele );
      }

      if ( this.NoLogo.HasValue) {
        XmlElement ele = doc.CreateElement ( "nologo" );
        ele.InnerText = this.NoLogo.Value.ToString();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty(this.Logger) ) {
        XmlElement ele = doc.CreateElement ( "logger" );
        ele.InnerText = this.Logger;
        root.AppendChild ( ele );
      }

      if ( this.BuildTimeoutSeconds.HasValue ) {
        XmlElement ele = doc.CreateElement ( "buildTimeoutSeconds" );
        ele.InnerText = this.BuildTimeoutSeconds.Value.ToString();
        root.AppendChild ( ele );
      }

      if ( this.Targets.Count > 0 ) {
        XmlElement ele = doc.CreateElement ( "targetList" );
        foreach ( string s in this.Targets ) {
          XmlElement te = doc.CreateElement ( "target" );
          te.InnerText = s;
          ele.AppendChild ( te );
        }
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.BaseDirectory = string.Empty;
      this.BuildArguments = string.Empty;
      this.BuildFile = string.Empty;
      this.BuildTimeoutSeconds = null;
      this.Executable = string.Empty;
      this.Logger = string.Empty;
      this.NoLogo = null;
      this._targets = new CloneableList<string> ();

      if ( string.Compare (element.Name, this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.Name, this.TypeName));

      string s = Util.GetElementOrAttributeValue ("executable", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Executable = s;

      s = Util.GetElementOrAttributeValue ("baseDirectory", element);
      if ( !string.IsNullOrEmpty (s) )
        this.BaseDirectory = s;

      s = Util.GetElementOrAttributeValue ("buildFile", element);
      if ( !string.IsNullOrEmpty (s) )
        this.BuildFile = s;

      s = Util.GetElementOrAttributeValue ("buildArgs", element);
      if ( !string.IsNullOrEmpty (s) )
        this.BuildArguments = s;

      s = Util.GetElementOrAttributeValue ("logger", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Logger = s;

      s = Util.GetElementOrAttributeValue ("nologo", element);
      if ( !string.IsNullOrEmpty (s) )
        this.NoLogo = string.Compare(s,bool.TrueString,true) == 0;


      XmlElement ele = (XmlElement) element.SelectSingleNode("targetList");
      if ( ele != null ) {
        foreach ( XmlElement sele in ele.SelectNodes ("target") )
          this.Targets.Add (sele.InnerText);
      }

      s = Util.GetElementOrAttributeValue ("buildTimeoutSeconds", element);
      if ( !string.IsNullOrEmpty (s) ) {
        int i = 0;
        if ( int.TryParse (s, out i) )
          this.BuildTimeoutSeconds = i;
      }
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri("http://confluence.public.thoughtworks.org/display/CCNET/NAnt+Task?decorator=printable"); }
    }

    #endregion
  }
}
