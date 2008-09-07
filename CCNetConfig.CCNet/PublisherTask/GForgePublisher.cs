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
using System.IO;
using System.Xml;
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using CCNetConfig.Core.Serialization;

namespace CCNetConfig.CCNet {
	/// <summary>
	/// 
	/// </summary>
	[Plugin, MinimumVersion ( "1.0" ), ReflectorName ( "gforge" )]
	public class GForgePublisher : PublisherTask, ICCNetDocumentation {

		/// <summary>
		/// Initializes a new instance of the <see cref="GForgePublisher"/> class.
		/// </summary>
		public GForgePublisher () : base ("gforge") {

		}
		/// <summary>
		/// Gets or sets the cruise control id.
		/// </summary>
		/// <value>The cruise control id.</value>
		[Required, Category ( "Required" ), DisplayName ( "(CruiseControlId)" ),
		ReflectorName ( "cruisecontrolId" ), DefaultValue ( null ),
		Description ( "The CruiseControl Id" ),
		Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) )]
		public int CruiseControlId { get; set; }
		/// <summary>
		/// Gets or sets the hash.
		/// </summary>
		/// <value>The hash.</value>
		[Required, DisplayName ( "(Hash)" ), DefaultValue ( null ), ReflectorName ( "hash" ),
		Editor ( typeof ( MultilineStringUIEditor ), typeof ( UITypeEditor ) ),
		Description ( "The GForge Hash String" )]
		public string Hash { get; set; }
		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		/// <value>The host.</value>
		[Required, ReflectorName ( "host" ), DefaultValue ( null ), DisplayName ( "(Host)" ),
		Description ( "The GForge Host name." )]
		public string Host { get; set; }
		/// <summary>
		/// Gets or sets the port.
		/// </summary>
		/// <value>The port.</value>
		[DefaultValue ( null ), Description ( "" ),
		Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ), ReflectorName ( "port" ),
		ReflectorNodeType ( ReflectorNodeTypes.Attribute )]
		public int? Port { get; set; }
		/// <summary>
		/// Gets or sets the project id.
		/// </summary>
		/// <value>The project id.</value>
		[Required, ReflectorName ( "projectId" ), DisplayName ( "(ProjectId)" ),
		Description ( "The GForge project Id" ),
		Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ), DefaultValue ( null )]
		public int ProjectId { get; set; }
		/// <summary>
		/// Gets or sets the use SSL.
		/// </summary>
		/// <value>The use SSL.</value>
		[ReflectorName ( "ssl" ), Description ( "Should SSL be used when connecting." ),
		Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
		TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
		public bool? UseSsl { get; set; }

		/// <summary>
		/// Serializes this instance.
		/// </summary>
		/// <returns></returns>
		public override XmlElement Serialize () {
			return new Serializer<GForgePublisher> ().Serialize ( this );
		}

		/// <summary>
		/// Deserializes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public override void Deserialize ( XmlElement element ) {
			if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
				throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );
			this.CruiseControlId = 0;
			this.Hash = string.Empty;
			this.Host = string.Empty;
			this.ProjectId = 0;
			this.Port = null;
			this.UseSsl = null;

			this.CruiseControlId = int.Parse ( Util.GetElementOrAttributeValue ( "cruisecontrolId", element ) );
			this.Hash = Util.GetElementOrAttributeValue ( "hash", element );
			this.Host = Util.GetElementOrAttributeValue ( "host", element );
			this.ProjectId = int.Parse ( Util.GetElementOrAttributeValue ( "projectId", element ) );

			string s = Util.GetElementOrAttributeValue ( "ssl", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.UseSsl = string.Compare ( s, bool.TrueString, true ) == 0;

			s = Util.GetElementOrAttributeValue ( "port", element );
			if ( !string.IsNullOrEmpty ( s ) ) {
				int x = -1;
				int.TryParse ( s, out x );
				if ( x > 0 ) {
					this.Port = x;
				}
			}
		}

		/// <summary>
		/// Creates a copy of this object.
		/// </summary>
		/// <returns></returns>
		public override PublisherTask Clone () {
			return this.MemberwiseClone () as GForgePublisher;
		}

		#region ICCNetDocumentation Members
		/// <summary>
		/// Gets the documentation URI.
		/// </summary>
		/// <value>The documentation URI.</value>
		[Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never ), ReflectorIgnore]
		public Uri DocumentationUri {
			get { return new Uri ( "http://gforge.com/gf/project/cruisecontrol/wiki/?pagename=CruiseControlDotNet" ); }
		}

		#endregion
	}
}
