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
using CCNetConfig.Core.Components;
using CCNetConfig.Core;
using System.ComponentModel;
using System.Drawing.Design;
using CCNetConfig.Core.Serialization;

namespace CCNetConfig.CCNet {
	/// <summary>
	/// SVN Revision Labeller is a plugin for CruiseControl.NET that allows you to generate CruiseControl 
	/// labels for your builds, based upon the revision number of your Subversion working copy. This can be 
	/// customised with a prefix and/or major/minor version numbers. 
	/// </summary>
	[Plugin, MinimumVersion("1.1") ]
	public class SvnRevisionLabeller : Labeller, ICCNetDocumentation {

		/// <summary>
		/// Initializes a new instance of the <see cref="SvnRevisionLabeller"/> class.
		/// </summary>
		public SvnRevisionLabeller () : base ( "svnRevisionLabeller") {

		}

		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
		[Required, Category("Required"), DisplayName("(Url)"), DefaultValue(null),
		Description ( "The url to the subversion repository." ), ReflectorName ( "url" ),]
		public Uri Url { get; set; }
		/// <summary>
		/// Gets or sets the major.
		/// </summary>
		/// <value>The major.</value>
		[Required, Category ( "Required" ), DisplayName ( "(Major)" ), DefaultValue ( null ),
		Description ( "The major version portion of the version." ), ReflectorName ( "major" ),
		Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) )]
		public int Major { get; set; }
		/// <summary>
		/// Gets or sets the minor.
		/// </summary>
		/// <value>The minor.</value>
		[Required, Category ( "Required" ), DisplayName ( "(Minor)" ), DefaultValue ( null ),
		Description ( "The minor version portion of the version." ), ReflectorName ( "minor" ),
		Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) )]
		public int Minor { get; set; }

		/// <summary>
		/// Gets or sets the prefix.
		/// </summary>
		/// <value>The prefix.</value>
		[Category ( "Optional" ), DefaultValue ( null ),
		Description ( "A prefix for the version." ), ReflectorName ( "prefix" )]
		public string Prefix { get; set; }
		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		[Category ( "Optional" ), DefaultValue ( null ),
		Description ( "Username to login to the subversion repository." ), ReflectorName ( "username" )]
		public string Username { get; set; }
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		[Category ( "Optional" ), DefaultValue ( null ),
		Description ( "Password to login to the subversion repository." ), ReflectorName ( "password" ),
		TypeConverter(typeof(PasswordTypeConverter))]
		public HiddenPassword Password { get; set; }

		/// <summary>
		/// Serializes this instance.
		/// </summary>
		/// <returns></returns>
		public override System.Xml.XmlElement Serialize () {
			return new Serializer<SvnRevisionLabeller> ().Serialize ( this );
		}

		/// <summary>
		/// Deserializes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public override void Deserialize ( System.Xml.XmlElement element ) {

			this.Prefix = null;
			this.Major = 0;
			this.Minor = 0;
			this.Password = new HiddenPassword ();
			this.Url = null;
			this.Username = null;
			
			// here we check that the element type value == the type name specified in the constructor.
			if ( string.Compare ( element.GetAttribute ( "type" ), base.TypeName, false ) != 0 ) {
				throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}",
									element.GetAttribute ( "type" ), base.TypeName ) );
			}

			string s = Util.GetElementOrAttributeValue ( "url", element );
			this.Url = new Uri ( s );

			s = Util.GetElementOrAttributeValue ( "major", element );
			this.Major = int.Parse(s);

			s = Util.GetElementOrAttributeValue ( "minor", element );
			this.Minor = int.Parse ( s );

			s = Util.GetElementOrAttributeValue ( "prefix", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				this.Prefix = s;
			}

			s = Util.GetElementOrAttributeValue ( "password", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				this.Password.Password = s;
			}

			s = Util.GetElementOrAttributeValue ( "username", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				this.Username = s;
			}
		}

		/// <summary>
		/// Creates a copy of this object
		/// </summary>
		/// <returns></returns>
		public override Labeller Clone () {
			SvnRevisionLabeller srl = this.MemberwiseClone () as SvnRevisionLabeller;
			srl.Password = this.Password.Clone ();
			return srl;
		}

		#region ICCNetDocumentation Members
		/// <summary>
		/// Gets the documentation URI.
		/// </summary>
		/// <value>The documentation URI.</value>
		[Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never ), ReflectorIgnore]
		public Uri DocumentationUri {
			get { return new Uri ( "http://code.google.com/p/svnrevisionlabeller/" ); }
		}

		#endregion
	}
}
