using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Drawing.Design;
using CCNetConfig.Core.Serialization;

namespace CCNetConfig.CCNet {
	/// <summary>
	/// The Fortress Item Comment Publisher will add a comment to each Fortress item referenced in the 
	/// checkin comments of the modifications for that build.
	/// </summary>
	[Plugin, ReflectorName("fortressplugin"), MinimumVersion("1.0")]
	public class SourceGearFortressPublisher : PublisherTask, ICCNetDocumentation{

		/// <summary>
		/// Initializes a new instance of the <see cref="SourceGearFortressPublisher"/> class.
		/// </summary>
		public SourceGearFortressPublisher () : base ("fortressplugin") {
			this.Password = new HiddenPassword ();
			this.ProxyPassword = new HiddenPassword ();
		}

		/// <summary>
		/// Gets or sets the only on success.
		/// </summary>
		/// <value>The only on success.</value>
		[Description ( "Whether comments should be added to items only when the build succeeds or always." ), DefaultValue ( null ),
		Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
		ReflectorName ( "onlyOnSuccess" )]
		public bool? OnlyOnSuccess { get; set; }
		/// <summary>
		/// Gets or sets the use label.
		/// </summary>
		/// <value>The use label.</value>
		[Description ( "Whether the comment will use cc.net's label in the comment or the folder version." ), DefaultValue ( null ),
		Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ),
		ReflectorName ( "useLabel" )]
		public bool? UseLabel { get; set; }
		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		[Description ( "Vault user id that CCNet should use to authenticate" ),
		DefaultValue ( null ), Category ( "Optional" ),
		ReflectorName ( "username" )]
		public string Username { get; set; }
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		[TypeConverter ( typeof ( PasswordTypeConverter ) ), DefaultValue ( null ),
		Description ( "Password for the Vault user" ), Category ( "Optional" ),
		ReflectorName ( "password" )]
		public HiddenPassword Password { get; set; }
		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		/// <value>The host.</value>
		[Description ( "The name of the Vault server" ), DefaultValue ( null ),
		Category ( "Optional" ), ReflectorName ( "host" )]
		public string Host { get; set; }
		/// <summary>
		/// Gets or sets the repository.
		/// </summary>
		/// <value>The repository.</value>
		[Description ( "The name of the Fortress repository to monitor." ), DefaultValue ( null ),
		Category ( "Optional" ), ReflectorName ( "repository" )]
		public string Repository { get; set; }
		/// <summary>
		/// Gets or sets the proxy server.
		/// </summary>
		/// <value>The proxy server.</value>
		[Description ( "The host name of the HTTP proxy Fortress should use." ), DefaultValue ( null ),
		Category ( "Optional" ), ReflectorName ( "proxyServer" )]
		public string ProxyServer { get; set; }
		/// <summary>
		/// Gets or sets the proxy port.
		/// </summary>
		/// <value>The proxy port.</value>
		[Description ( "The port on the HTTP proxy Fortress should use." ), DefaultValue ( null ),
		Category ( "Optional" ), Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ),
		MinimumValue ( 0 ), MaximumValue ( Int16.MaxValue ), ReflectorName ( "proxyPort" )]
		public int? ProxyPort { get; set; }
		/// <summary>
		/// Gets or sets the proxy user.
		/// </summary>
		/// <value>The proxy user.</value>
		[Description ( "The user name for the HTTP proxy Fortress should use." ), DefaultValue ( null ),
		Category ( "Optional" ), ReflectorName ( "proxyUser" )]
		public string ProxyUser { get; set; }
		/// <summary>
		/// Gets or sets the proxy password.
		/// </summary>
		/// <value>The proxy password.</value>
		[TypeConverter ( typeof ( PasswordTypeConverter ) ), DefaultValue ( null ),
		Description ( "The password for the HTTP proxy Fortress should use." ),
		Category ( "Optional" ), ReflectorName ( "proxyPassword" )]
		public HiddenPassword ProxyPassword { get; set; }
		/// <summary>
		/// Gets or sets the proxy domain.
		/// </summary>
		/// <value>The proxy domain.</value>
		[Description ( "The Windows domain of the HTTP proxy Vault should use." ), DefaultValue ( null ),
		Category ( "Optional" ), ReflectorName ( "proxyDomain" )]
		public string ProxyDomain { get; set; }

		/// <summary>
		/// Serializes this instance.
		/// </summary>
		/// <returns></returns>
		public override System.Xml.XmlElement Serialize () {
			return new Serializer<SourceGearFortressPublisher> ().Serialize ( this );
		}

		/// <summary>
		/// Deserializes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public override void Deserialize ( System.Xml.XmlElement element ) {
			Util.ResetObjectProperties<SourceGearFortressPublisher> ( this );

			if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
				throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

			string s = Util.GetElementOrAttributeValue ( "onlyOnSuccess", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.OnlyOnSuccess = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "useLabel", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.UseLabel = string.Compare ( s, bool.TrueString, true ) == 0;

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
		}

		/// <summary>
		/// Creates a copy of this object.
		/// </summary>
		/// <returns></returns>
		public override PublisherTask Clone () {
			SourceGearFortressPublisher sgfp = this.MemberwiseClone () as SourceGearFortressPublisher;
			sgfp.Password = this.Password.Clone ();
			sgfp.ProxyPassword = this.ProxyPassword.Clone ();
			return sgfp;
		}

		#region ICCNetDocumentation Members

		/// <summary>
		/// Gets the documentation URI.
		/// </summary>
		/// <value>The documentation URI.</value>
		[Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never ), ReflectorIgnore]
		public Uri DocumentationUri {
			get { return new Uri ( "http://www.sourcegear.com/vault/downloads.html" ); }
		}

		#endregion
	}
}
