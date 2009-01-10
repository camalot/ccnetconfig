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
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using CCNetConfig.Core.Serialization;

namespace CCNetConfig.CCNet {
	/// <summary>
	/// This is for the SourceGears official plugin.
	/// </summary>
	/// <workitems>
	///		<workitem rel="20524">Add Support for SourceGearVaultSourceControl Plugin</workitem>
	/// </workitems>
	[Plugin, MinimumVersion("1.0")]
	public class SourceGearVaultPluginSourceControl : SourceControl, ICCNetDocumentation {

		/// <summary>
		/// Initializes a new instance of the <see cref="SourceGearVaultPluginSourceControl"/> class.
		/// </summary>
		public SourceGearVaultPluginSourceControl () : base("vaultplugin") {
			this.Password = new HiddenPassword ();
			this.ProxyPassword = new HiddenPassword ();
		}
		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		[Description ( "Vault user id that CCNet should use to authenticate" ), 
		DefaultValue ( null ), Category ( "Optional" ),
		ReflectorName("username")]
		public string Username { get; set; }
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		[TypeConverter ( typeof ( PasswordTypeConverter ) ), DefaultValue ( null ), 
		Description ( "Password for the Vault user" ), Category ( "Optional" ),
		ReflectorName("password")]
		public HiddenPassword Password { get; set; }
		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		/// <value>The host.</value>
		[Description ( "The name of the Vault server" ), DefaultValue ( null ), 
		Category ( "Optional" ), ReflectorName("host")]
		public string Host { get; set; }
		/// <summary>
		/// Gets or sets the repository.
		/// </summary>
		/// <value>The repository.</value>
		[Description ( "The name of the Vault repository to monitor." ), DefaultValue ( null ), 
		Category ( "Optional" ), ReflectorName("repository")]
		public string Repository { get; set; }
		/// <summary>
		/// Gets or sets the folder.
		/// </summary>
		/// <value>The folder.</value>
		[Description ( "The root folder to be monitored by CCNet" ), DefaultValue ( null ), 
		Category ( "Required" ), ReflectorName("folder"), DisplayName("(Folder)"), Required]
		public string Folder { get; set; }
		/// <summary>
		/// Gets or sets the use SSL.
		/// </summary>
		/// <value>The use SSL.</value>
		[Description ( "Should SSL be used to communicate with the Vault server." ), DefaultValue ( null ),
		Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
		ReflectorName("ssl")]
		public bool? UseSsl { get; set; }
		/// <summary>
		/// Gets or sets the auto get source.
		/// </summary>
		/// <value>The auto get source.</value>
		[Description ( "Specifies if CCNet should automatically retrieve the latest version of the source from the repository" ),
		DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
		ReflectorName("autoGetSource")]
		public bool? AutoGetSource { get; set; }
		/// <summary>
		/// Gets or sets the apply label.
		/// </summary>
		/// <value>The apply label.</value>
		[Description ( "Specifies if CCNet should apply the build label to the repository" ), DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
		ReflectorName("applyLabel")]
		public bool? ApplyLabel { get; set; }
		/// <summary>
		/// Gets or sets the use working directory.
		/// </summary>
		/// <value>The use working directory.</value>
		[Description ( "CC.NET 1.0: Determines the working directory into which Vault files will be retrieved. Supply true if you want CCNet to use the Vault folder " +
			"working directory created for the build user using the Vault GUI (recommended). Supply false if CCNet should use the CCNet working directory. \n\n" +
			"CC.NET 1.1: Determines if the source will be retrieved into a Vault Working Folder." ), DefaultValue ( null ),
		Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
		ReflectorName("useWorkingDirectory")]
		public bool? UseWorkingDirectory { get; set; }
		/// <summary>
		/// Gets or sets the clean copy.
		/// </summary>
		/// <value>The clean copy.</value>
    [Description ("If true, the source path will be emptied before retrieving new source."), DefaultValue (null),
    Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
    TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
		ReflectorName ( "cleanCopy" )]
		public bool? CleanCopy { get; set; }
		/// <summary>
		/// Gets or sets the working directory.
		/// </summary>
		/// <value>The working directory.</value>
		[Description ( "The root folder where the latest source will retrieved from Vault. This path can either be absolute or it can be relative to the " +
			"CCNet project working directory.\n\nCC.NET 1.1: If this element is missing or empty, Vault will attempt to use the directory set as the user's " +
			"working folder. Note that this is simply the destination path on disk. Whether or not this location is a Vault Working Folder is determined by the " +
			"useWorkingFolder element. To use the same path as the project, it is necessary to use \".\" (without the quotes) rather than leaving this empty, " +
		"as you could in CC.NET 1.0." ), Category ( "Optional" ), DefaultValue ( null ),
		Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
		BrowseForFolderDescription ( "Select path to the working directory." ),
		ReflectorName("workingDirectory")]
		public string WorkingDirectory { get; set; }
		/// <summary>
		/// Gets or sets the proxy server.
		/// </summary>
		/// <value>The proxy server.</value>
		[Description ( "The host name of the HTTP proxy Vault should use." ), DefaultValue ( null ), 
		Category ( "Optional" ), ReflectorName("proxyServer")]
		public string ProxyServer { get; set; }
		/// <summary>
		/// Gets or sets the proxy port.
		/// </summary>
		/// <value>The proxy port.</value>
		[Description ( "The port on the HTTP proxy Vault should use." ), DefaultValue ( null ), 
		Category ( "Optional" ), Editor(typeof(NumericUpDownUIEditor), typeof(UITypeEditor)),
		MinimumValue(0), MaximumValue(Int16.MaxValue), ReflectorName("proxyPort")]
		public int? ProxyPort { get; set; }
		/// <summary>
		/// Gets or sets the proxy user.
		/// </summary>
		/// <value>The proxy user.</value>
		[Description ( "The user name for the HTTP proxy Vault should use." ), DefaultValue ( null ), 
		Category ( "Optional" ), ReflectorName("proxyUser")]
		public string ProxyUser { get; set; }
		/// <summary>
		/// Gets or sets the proxy password.
		/// </summary>
		/// <value>The proxy password.</value>
		[TypeConverter ( typeof ( PasswordTypeConverter ) ), DefaultValue ( null ), 
		Description ( "The password for the HTTP proxy Vault should use." ),
		Category ( "Optional" ),ReflectorName("proxyPassword")]
		public HiddenPassword ProxyPassword { get; set; }
		/// <summary>
		/// Gets or sets the proxy domain.
		/// </summary>
		/// <value>The proxy domain.</value>
		[Description ( "The Windows domain of the HTTP proxy Vault should use." ), DefaultValue ( null ), 
		Category ( "Optional" ), ReflectorName("proxyDomain")]
		public string ProxyDomain { get; set; }


		/// <summary>
		/// Serializes this instance.
		/// </summary>
		/// <returns></returns>
		public override System.Xml.XmlElement Serialize () {
			return new Serializer<SourceGearVaultPluginSourceControl> ().Serialize ( this );
		}

		/// <summary>
		/// Deserializes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public override void Deserialize ( System.Xml.XmlElement element ) {
			Util.ResetObjectProperties<SourceGearVaultPluginSourceControl> ( this );

			if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
				throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

			string s = Util.GetElementOrAttributeValue ( "applyLabel", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.ApplyLabel = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "autoGetSource", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.AutoGetSource = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "cleanCopy", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.CleanCopy = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "folder", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Folder = s;

			s = Util.GetElementOrAttributeValue ( "host", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Host = s;

			s = Util.GetElementOrAttributeValue ( "password", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Password.Password = s;

			s = Util.GetElementOrAttributeValue ( "proxyDomain", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.ProxyDomain = s;

			s = Util.GetElementOrAttributeValue ( "proxyPassword", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.ProxyPassword.Password = s;

			s = Util.GetElementOrAttributeValue ( "proxyPort", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				int i = 0;
				if ( int.TryParse ( s, out i ) )
					this.ProxyPort = i;
			}

			s = Util.GetElementOrAttributeValue ( "proxyServer", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.ProxyServer = s;

			s = Util.GetElementOrAttributeValue ( "proxyUser", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.ProxyUser = s;

			s = Util.GetElementOrAttributeValue ( "repository", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Repository = s;

			s = Util.GetElementOrAttributeValue ( "username", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.Username = s;

			s = Util.GetElementOrAttributeValue ( "ssl", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.UseSsl = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "useWorkingDirectory", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.UseWorkingDirectory = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.WorkingDirectory = s;
		}

		/// <summary>
		/// Creates a copy of the source control object
		/// </summary>
		/// <returns></returns>
		public override SourceControl Clone () {
			SourceGearVaultPluginSourceControl sgvpsc = this.MemberwiseClone () as SourceGearVaultPluginSourceControl;
			sgvpsc.Password = this.Password.Clone ();
			sgvpsc.ProxyPassword = this.ProxyPassword.Clone ();
			return sgvpsc;
		}

		#region ICCNetDocumentation Members
		/// <summary>
		/// Gets the documentation URI.
		/// </summary>
		/// <value>The documentation URI.</value>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never),ReflectorIgnore]
		public Uri DocumentationUri {
			get { return new Uri ( "http://www.sourcegear.com/vault/downloads.html" ); }
		}

		#endregion
	}
}
