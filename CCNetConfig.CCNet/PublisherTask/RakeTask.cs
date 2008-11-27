/*
 * Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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
using System.Xml;
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using CCNetConfig.Core.Serialization;

namespace CCNetConfig.CCNet {
	/// <summary>
	/// Make for Ruby
	/// </summary>
	[MinimumVersion("1.4.1"), ReflectorName("rake")]
	public class RakeTask : PublisherTask, ICCNetDocumentation {
		/// <summary>
		/// Initializes a new instance of the <see cref="RakeTask"/> class.
		/// </summary>
		public RakeTask () : base("rake") {

		}
		/// <summary>
		/// The path of the version of Rake you want to run. If this is relative, then must be relative 
		/// to either (a) the base directory, (b) the CCNet Server application, or (c) if the path doesn't 
		/// contain any directory details then can be available in the system or application's 'path' 
		/// environment variable
		/// </summary>
		[Category("Optional"), DefaultValue(null),
		Description("The path of the version of Rake you want to run. If this is relative, then must be " +
			"relative to (a) the base directort, (b) the CCNet Server Application, or (c) if the path doesn't" +
			" contain any directory details then can be available in the system or application's 'path' " +
			"environment variable."),
		ReflectorName("executable"), FileTypeFilter("Executable|*.exe;*.bat;*.cmd|All Files (*.*)|*.*"),
		Editor(typeof(OpenFileDialogUIEditor),typeof(UITypeEditor)),
		OpenFileDialogTitle("Select Rake Executable")]
		public string Executable { get; set; }
		/// <summary>
		/// The directory to run the Rake process in. If relative, is a subdirectory of the Project Working Directory
		/// </summary>
		/// <value>The working directory.</value>
		[Category("Optional"),DefaultValue(null),
		Description("The directory to run the Rake process in."),
		ReflectorName("baseDirectory"), Editor(typeof(BrowseForFolderUIEditor),typeof(UITypeEditor)),
		BrowseForFolderDescription("Select the Rake process working directory.")]
		public string WorkingDirectory { get; set; }
		/// <summary>
		/// The name of the Rakefile to run, relative to the WorkingDirectory. 
		/// </summary>
		[Category("Optional"),DefaultValue(null),
		Description("The name of the Rakefile to run, relative to the working directory."),
		ReflectorName("rakefile"),Editor(typeof(OpenFileDialogUIEditor),typeof(UITypeEditor)),
		FileTypeFilter("Rake File|*.rake|All Files (*.*)|*.*"),
		OpenFileDialogTitle("Select rake file.")]
		public string RakeFile { get; set; }
		/// <summary>
		/// Any arguments to pass through to Rake (e.g to specify build properties) 
		/// </summary>
		/// <value>The build args.</value>
		[Category("Optional"),DefaultValue(null),
		Description ( "Any arguments to pass through to Rake (e.g to specify build properties)" ),
		ReflectorName("buildArgs"),Editor(typeof(StringListUIEditor),typeof(UITypeEditor)),
		TypeConverter(typeof(StringListTypeConverter)), StringSeparator(" ")]
		public CloneableList<string> BuildArguments { get; set; }
		/// <summary>
		/// Number of seconds to wait before assuming that the process has hung and should be killed. 
		/// </summary>
		/// <value>The build timeout.</value>
		[Category("Optional"),DefaultValue(null),
		Description("Number of seconds to wait before assuming that the process has hung and should be killed."),
		ReflectorName("buildTimeoutSeconds"), Editor(typeof(NumericUpDownUIEditor),typeof(UITypeEditor)),
		MinimumValue(0)]
		public int? Timeout { get; set; }
		/// <summary>
		/// Gets or sets the targets.
		/// </summary>
		/// <value>The targets.</value>
		[ReflectorArray("target"),ReflectorName("targetList"), DefaultValue(null), Category("Optional"),
		Description ( "A list of targets to be called. CruiseControl.NET does not call Rake once for " +
			"each target, it uses the Rake feature of being able to specify multiple targets." ),
		Editor(typeof(StringListUIEditor),typeof(UITypeEditor)),
		TypeConverter(typeof(StringListTypeConverter))]	
		public CloneableList<string> Targets { get; set; }
		/// <summary>
		/// Gets or sets the trace.
		/// </summary>
		/// <value>The trace.</value>
		[Category("Optional"), DefaultValue(null),
		Description ( "Turns on invoke/execute tracing and enables full backtrace." ),
		ReflectorName("trace"), Editor(typeof(DefaultableBooleanUIEditor), typeof(UITypeEditor)),
		TypeConverter(typeof(DefaultableBooleanTypeConverter))]
		public bool? Trace { get; set; }
		/// <summary>
		/// Gets or sets the quiet.
		/// </summary>
		/// <value>The quiet.</value>
		[Category ( "Optional" ), DefaultValue ( null ),
		Description ( "Do not log messages to standard output." ),
		ReflectorName ( "quiet" ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
		public bool? Quiet { get; set; }
		/// <summary>
		/// Gets or sets the silent.
		/// </summary>
		/// <value>The silent.</value>
		[Category ( "Optional" ), DefaultValue ( null ),
		Description ( "Like quiet but also suppresses the 'in directory' announcement." ),
		ReflectorName ( "silent" ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
		public bool? Silent { get; set; }



		#region ICCNetDocumentation Members
		/// <summary>
		/// Gets the documentation URI.
		/// </summary>
		/// <value>The documentation URI.</value>
		[ReflectorIgnore, Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Uri DocumentationUri {
			get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Rake+Task?decorator=printable" ); }
		}

		#endregion

		/// <summary>
		/// Serializes this instance.
		/// </summary>
		/// <returns></returns>
		public override XmlElement Serialize () {
			return new Serializer<RakeTask> ().Serialize ( this );
		}

		/// <summary>
		/// Deserializes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public override void Deserialize ( XmlElement element ) {
			this.BuildArguments = new CloneableList<string> ();
			this.Executable = string.Empty;
			this.RakeFile = string.Empty;
			this.Targets = new CloneableList<string> ();
			this.Timeout = null;
			this.WorkingDirectory = string.Empty;
			this.Quiet = null;
			this.Silent = null;
			this.Trace = null;

			if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
				throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

			string s = Util.GetElementOrAttributeValue ( "executable", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Executable = s;

			s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.WorkingDirectory = s;

			s = Util.GetElementOrAttributeValue ( "rakefile", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.RakeFile = s;

			s = Util.GetElementOrAttributeValue ( "quiet", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Quiet = string.Compare(s,bool.TrueString,true) == 0;

			s = Util.GetElementOrAttributeValue ( "silent", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Silent = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "trace", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Trace = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "buildArgs", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				this.BuildArguments.AddRange ( s.Split ( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries ) );
			}

			XmlNodeList targetListItems = element.SelectNodes ( "targetList/*" );
			foreach ( XmlElement ele in targetListItems ) {
				this.Targets.Add ( ele.InnerText );
			}

			s = Util.GetElementOrAttributeValue ( "buildTimeoutSeconds", element );
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
		public override PublisherTask Clone () {
			RakeTask rt= this.MemberwiseClone () as RakeTask;
			rt.BuildArguments = this.BuildArguments.Clone ();
			rt.Targets = this.Targets.Clone ();
			return rt;
		}
	}
}
