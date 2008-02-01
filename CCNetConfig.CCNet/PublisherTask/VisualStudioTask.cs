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
using CCNetConfig.Core.Enums;
using System.Xml;
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// Most complex build processes use NAnt  or MSBuild to script out the build process. However, for simple projects that just need to 
  /// compile a Visual Studio.NET solution, the DevenvBuilder provides an easy way to compile your project using the 
  /// <see cref="CCNetConfig.CCNet.VisualStudioTask">VisualStudioTask</see>.
  /// </summary>
  [ MinimumVersion( "1.0" ) ]
  public class VisualStudioTask : PublisherTask, ICCNetDocumentation {
    private string _solutionFile = null;
    private string _configuration = string.Empty;
    private VSBuildType? _buildType = null;
    private string _project = string.Empty;
    private string _devenv = null;
    private int? _buildTimeout = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="VisualStudioTask"/> class.
    /// </summary>
    public VisualStudioTask() : base ("devenv") { }
    /// <summary>
    /// Path of the solution file to build. If relative, is relative to the <see cref="CCNetConfig.CCNet.MSBuildTask.WorkingDirectory">WorkingDirectory</see>
    /// </summary>
    [Description ("Path of the solution file to build. If relative, is relative to the WorkingDirectory."), DefaultValue (null),
  DisplayName ( "(SolutionFile)" ), Category ( "Required" ),
Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "Solution Files|*.sln|All Files|*.*" ),
OpenFileDialogTitle ( "Select solution file" )]
    public string SolutionFile { get { return this._solutionFile; } set { this._solutionFile = Util.CheckRequired (this, "solutionfile", value); } }
    /// <summary>
    /// Solution configuration to use with the build (Not case sensitive)
    /// </summary>
    [Description ( "Solution configuration to use with the build (Not case sensitive)." ), DefaultValue ( null ), DisplayName ( "(Configuration)" ), 
    Category ( "Required" )]
    public string Configuration { get { return this._configuration; } set { this._configuration = Util.CheckRequired (this, "configuration", value); } }
    /// <summary>
    /// The type of build you want to execute using the devenv executable.
    /// </summary>
    [Description ("The type of build you want to execute using the devenv executable."), DefaultValue (null),
   Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ), 
    Category ( "Optional" )]
    public VSBuildType? BuildType { get { return this._buildType; } set { this._buildType = value; } }
    /// <summary>
    /// A specific project name included in the solution that you want to compile (Not case sensitive).
    /// </summary>
    [Description ( "A specific project name included in the solution that you want to compile (Not case sensitive)." ), DefaultValue ( null ), 
    Category ( "Optional" )]
    public string Project { get { return this._project; } set { this._project = value; } }
    /// <summary>
    /// CC.NET will try to load this path from the registry. If both VS.NET 2003 and VS.NET 2002 are installed, CC.NET will try using 
    /// VS.NET 2003. In this case, if you need to use VS.NET 2002, you should specify this property to point to the location of "devenv.com" 
    /// for VS.NET 2002.
    /// </summary>
    [Description ("CC.NET will try to load this path from the registry. If both VS.NET 2003 and VS.NET 2002 are installed, CC.NET will try using " +
      "VS.NET 2003. In this case, if you need to use VS.NET 2002, you should specify this property to point to the location of \"devenv.com\" " +
   "for VS.NET 2002." ), DefaultValue ( null ), Category ( "Optional" ),
Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "Visual Studio Executable|devenv.exe" ),
OpenFileDialogTitle ( "Select Visual Studio Executable" )]
    public string Executable { get { return this._devenv; } set { this._devenv = value; } }
    /// <summary>
    /// Number of seconds to wait before assuming that the process has hung and should be killed.
    /// </summary>
    [Description ( "Number of seconds to wait before assuming that the process has hung and should be killed." ), DefaultValue ( null ), 
    Category ( "Optional" )]
    public int? BuildTimeoutSeconds { get { return this._buildTimeout; } set { this._buildTimeout = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      return this.MemberwiseClone () as VisualStudioTask;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize() {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement (this.TypeName);
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));


      XmlElement ele = doc.CreateElement ("solutionfile");
      ele.InnerText = Util.CheckRequired (this, "solutionfile", this.SolutionFile);
      root.AppendChild (ele);

      ele = doc.CreateElement ("configuration");
      ele.InnerText = Util.CheckRequired (this, "configuration", this.Configuration);
      root.AppendChild (ele);

      if ( BuildType.HasValue ) {
        ele = doc.CreateElement ("buildtype");
        ele.InnerText = BuildType.Value.ToString ();
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty (this.Project) ) {
        ele = doc.CreateElement ("project");
        ele.InnerText = this.Project;
        root.AppendChild (ele);
      }

      if ( !string.IsNullOrEmpty(this.Executable) ) {
        ele = doc.CreateElement ("executable");
        ele.InnerText = this.Executable;
        root.AppendChild (ele);
      }

      if ( this.BuildTimeoutSeconds.HasValue ) {
        ele = doc.CreateElement ("buildTimeoutSeconds");
        ele.InnerText = this.BuildTimeoutSeconds.Value.ToString ();
        root.AppendChild (ele);
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.BuildTimeoutSeconds = null;
      this.BuildType = null;
      this._configuration = string.Empty;
      this.Executable = string.Empty;
      this.Project = string.Empty;
      this._solutionFile = string.Empty;

      if ( string.Compare (element.Name, this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.Name, this.TypeName));

      string s = Util.GetElementOrAttributeValue ("solutionfile", element);
      this.SolutionFile = s;
      
      s = Util.GetElementOrAttributeValue ("configuration", element);
      this.Configuration = s;

      s = Util.GetElementOrAttributeValue ("buildtype", element);
      if ( !string.IsNullOrEmpty (s) ) {
        this.BuildType = (Core.Enums.VSBuildType)Enum.Parse(typeof(Core.Enums.VSBuildType),s,true);

      }

      s = Util.GetElementOrAttributeValue ("project", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Project = s;

      s = Util.GetElementOrAttributeValue ("executable", element);
      if ( !string.IsNullOrEmpty (s) )
        this.Executable = s;

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
    [Browsable (false)]
    public Uri DocumentationUri {
      get { return new Uri ("http://confluence.public.thoughtworks.org/display/CCNET/Visual+Studio+Task?decorator=printable"); }
    }

    #endregion
  }
}
