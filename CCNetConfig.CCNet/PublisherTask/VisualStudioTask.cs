/*
 * Copyright (c) 2006 - 2009, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core.Serialization;

namespace CCNetConfig.CCNet {
	/// <summary>
	/// Most complex build processes use NAnt  or MSBuild to script out the build process. However, for simple projects that just need to 
	/// compile a Visual Studio.NET solution, the DevenvBuilder provides an easy way to compile your project using the 
	/// <see cref="CCNetConfig.CCNet.VisualStudioTask">VisualStudioTask</see>.
	/// </summary>
	[MinimumVersion ( "1.0" ), ReflectorName ( "devenv" )]
	public class VisualStudioTask : PublisherTask, ICCNetDocumentation {
		/// <summary>
		/// Initializes a new instance of the <see cref="VisualStudioTask"/> class.
		/// </summary>
		public VisualStudioTask () : base ( "devenv" ) { }
		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>The version.</value>
		[ReflectorName ( "version" ), DefaultValue ( null ),
		Description ( "The version of Visual Studio to run" ),
		Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableEnumTypeConverter ) )]
		public VisualStudioTaskVersions? Version { get; set; }
		/// <summary>
		/// Path of the solution file to build. If relative, is relative to the <see cref="CCNetConfig.CCNet.MSBuildTask.WorkingDirectory">WorkingDirectory</see>
		/// </summary>
		[Description ( "Path of the solution file to build. If relative, is relative to the WorkingDirectory." ), DefaultValue ( null ),
		DisplayName ( "(SolutionFile)" ), Category ( "Required" ),
		Required, ReflectorName ( "solutionfile" ),
		Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "Solution Files|*.sln|All Files|*.*" ),
		OpenFileDialogTitle ( "Select solution file" )]
		public string SolutionFile { get; set; }
		/// <summary>
		/// Solution configuration to use with the build (Not case sensitive)
		/// </summary>
		[Description ( "Solution configuration to use with the build (Not case sensitive)." ), DefaultValue ( null ), DisplayName ( "(Configuration)" ),
		Category ( "Required" ), Required, ReflectorName ( "configuration" )]
		public string Configuration { get; set; }
		/// <summary>
		/// The type of build you want to execute using the devenv executable.
		/// </summary>
		[Description ( "The type of build you want to execute using the devenv executable." ), DefaultValue ( null ),
		Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ),
		Category ( "Optional" ), ReflectorName ( "buildtype" )]
		public VSBuildType? BuildType { get; set; }
		/// <summary>
		/// A specific project name included in the solution that you want to compile (Not case sensitive).
		/// </summary>
		[Description ( "A specific project name included in the solution that you want to compile (Not case sensitive)." ), DefaultValue ( null ),
		Category ( "Optional" ), ReflectorName ( "project" )]
		public string Project { get; set; }
		/// <summary>
		/// CC.NET will try to load this path from the registry. If both VS.NET 2003 and VS.NET 2002 are installed, CC.NET will try using 
		/// VS.NET 2003. In this case, if you need to use VS.NET 2002, you should specify this property to point to the location of "devenv.com" 
		/// for VS.NET 2002.
		/// </summary>
		[Description ( "CC.NET will try to load this path from the registry. If both VS.NET 2003 and VS.NET 2002 are installed, CC.NET will try using " +
			"VS.NET 2003. In this case, if you need to use VS.NET 2002, you should specify this property to point to the location of \"devenv.com\" " +
	 "for VS.NET 2002." ), DefaultValue ( null ), Category ( "Optional" ),
		ReflectorName ( "executable" ),
		Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ), FileTypeFilter ( "Visual Studio Executable|devenv.exe" ),
		OpenFileDialogTitle ( "Select Visual Studio Executable" )]
		public string Executable { get; set; }
		/// <summary>
		/// Number of seconds to wait before assuming that the process has hung and should be killed.
		/// </summary>
		[Description ( "Number of seconds to wait before assuming that the process has hung and should be killed." ), DefaultValue ( null ),
		Category ( "Optional" ), ReflectorName ( "buildTimeoutSeconds" ),
		Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ),
		MinimumValue ( 0 )]
		public int? BuildTimeoutSeconds { get; set; }

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
		public override System.Xml.XmlElement Serialize () {
			return new Serializer<VisualStudioTask> ().Serialize ( this );
		}

		/// <summary>
		/// Deserializes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public override void Deserialize ( XmlElement element ) {
			Util.ResetObjectProperties<VisualStudioTask> ( this );

			if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
				throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

			string s = Util.GetElementOrAttributeValue ( "solutionfile", element );
			this.SolutionFile = s;

			s = Util.GetElementOrAttributeValue ( "configuration", element );
			this.Configuration = s;

			s = Util.GetElementOrAttributeValue ( "buildtype", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				this.BuildType = (Core.Enums.VSBuildType)Enum.Parse ( typeof ( Core.Enums.VSBuildType ), s, true );

			}

			s = Util.GetElementOrAttributeValue ( "project", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Project = s;

			s = Util.GetElementOrAttributeValue ( "executable", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Executable = s;

			s = Util.GetElementOrAttributeValue ( "buildTimeoutSeconds", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				int i = 0;
				if ( int.TryParse ( s, out i ) )
					this.BuildTimeoutSeconds = i;
			}

			s = Util.GetElementOrAttributeValue ( "version", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				string name = s;
				if ( !Enum.IsDefined ( typeof ( VisualStudioTaskVersions ), s ) )
					name = Util.GetRealNameFromSerializerValue<VisualStudioTaskVersions> ( s );
				this.Version = (VisualStudioTaskVersions)Enum.Parse ( typeof ( VisualStudioTaskVersions ), name );
			}

		}

		#region ICCNetDocumentation Members
		/// <summary>
		/// Gets the documentation URI.
		/// </summary>
		/// <value>The documentation URI.</value>
		[Browsable ( false ), ReflectorIgnore, EditorBrowsable ( EditorBrowsableState.Never )]
		public Uri DocumentationUri {
			get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Visual+Studio+Task?decorator=printable" ); }
		}

		#endregion
	}
}
