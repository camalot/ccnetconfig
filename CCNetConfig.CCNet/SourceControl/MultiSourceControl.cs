/* Copyright (c) 2006, Ryan Conrad. All rights reserved.
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
using System.Xml;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Collections;

namespace CCNetConfig.CCNet {
	/// <summary>
	/// You can use the 'Multi' Source Control plugin to check for modifications from any number of source control repositories. You may want to 
	/// do this if (for example) you want to build if the source for your project changes, or if the binaries your project depends on change (which 
	/// may be stored on a file server).
	/// </summary>
	[MinimumVersion ( "1.0" )]
	public class MultiSourceControl : SourceControl, ICCNetDocumentation {
		private List<SourceControl> _sourceControls;
		private bool? _requireChangesFromAll = null;
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiSourceControl"/> class.
		/// </summary>
		public MultiSourceControl ()
			: base ( "multi" ) {
			_sourceControls = new List<SourceControl> ();
		}
		/// <summary>
		/// You can use the 'Multi' Source Control plugin to check for modifications from any number of source control repositories. 
		/// You may want to do this if (for example) you want to build if the source for your project changes, or if the binaries your 
		/// project depends on change (which may be stored on a file server).
		/// </summary>
		/// <value>The source controls.</value>
		[Description ( "You can use the 'Multi' Source Control plugin to check for modifications from any number of source control repositories. " +
			"You may want to do this if (for example) you want to build if the source for your project changes, or if the binaries your project depends " +
			"on change (which may be stored on a file server)." ),
		Editor ( typeof ( SourceControlListUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( IListTypeConverter ) ),
	 DefaultValue ( null ), DisplayName ( "(SourceControls)" ), Category ( "Required" )]
		public List<SourceControl> SourceControls { get { return this._sourceControls; } set { this._sourceControls = value; } }

		/// <summary>
		///If true, only return a list of modifications if all sourceControl sections return a non-empty list. Note that this 
		/// is short-circuiting, i.e. if the first sourceControl returns an empty list, the next won't be called (this can be useful for situations where 
		/// you have a slow source control server and you want to check a specific file first as a trigger)
		/// </summary>
		/// <value>The require changes from all.</value>
		[Description ( "If true, only return a list of modifications if all sourceControl sections return a non-empty list. Note that this " +
			"is short-circuiting, i.e. if the first sourceControl returns an empty list, the next won't be called (this can be useful for situations where " +
			"you have a slow source control server and you want to check a specific file first as a trigger)" ), DefaultValue ( null ),
	 Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
		Category ( "Optional" )]
		public bool? RequireChangesFromAll { get { return this._requireChangesFromAll; } set { this._requireChangesFromAll = value; } }

		/// <summary>
		/// Creates a copy of this object.
		/// </summary>
		/// <returns></returns>
		public override SourceControl Clone () {
			MultiSourceControl msc = this.MemberwiseClone () as MultiSourceControl;
			msc.SourceControls = new List<SourceControl> ();
			SourceControl[  ] sc= new SourceControl[ this.SourceControls.Count ];
			this.SourceControls.CopyTo ( sc );
			msc.SourceControls.AddRange ( sc );
			return msc;
		}



		/// <summary>
		/// Serializes this instance.
		/// </summary>
		/// <returns></returns>
		public override System.Xml.XmlElement Serialize () {
			XmlDocument doc = new XmlDocument ();
			XmlElement root = doc.CreateElement ( "sourcecontrol" );
			root.SetAttribute ( "type", this.TypeName );
			//root.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name ) );

			XmlElement scs = doc.CreateElement ( "sourceControls" );
			foreach ( SourceControl scItem in this._sourceControls ) {
				XmlElement sele = this.ConvertSourceControlToElement ( doc, scItem );
				if ( sele != null )
					scs.AppendChild ( sele );
			}
			root.AppendChild ( scs );
			return root;
		}

		/// <summary>
		/// Deserializes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public override void Deserialize ( System.Xml.XmlElement element ) {
			SourceControls = new List<SourceControl> ();

			XmlElement scs = (XmlElement)element.SelectSingleNode ( "sourceControls" );
			if ( scs != null ) {
				foreach ( XmlElement tele in scs.SelectNodes ( "./*" ) ) {
					SourceControl scItem = this.ConvertElementToSourceControl ( tele );
					if ( scItem != null )
						this._sourceControls.Add ( scItem );
				}
			}
			string s = Util.GetElementOrAttributeValue ( "requireChangesFromAll", element );
			if ( !string.IsNullOrEmpty ( s ) )
				this.RequireChangesFromAll = string.Compare ( s, bool.TrueString, true ) == 0;
		}

		/// <summary>
		/// Converts the source control to element.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="sc">The sc.</param>
		/// <returns></returns>
		/// <workitems>
		///		<workitem rel="16385">Checked if the node is null before attempting to import it.</workitem>
		/// </workitems>
		private XmlElement ConvertSourceControlToElement ( XmlDocument owner, SourceControl sc ) {
			XmlElement ele = sc.Serialize ();
			XmlElement tNew = owner.CreateElement ( sc.TypeName );
			//tNew.SetAttribute ( "ccnetconfigType", string.Format ( "{0}, {1}", sc.GetType ().FullName, sc.GetType ().Assembly.GetName ().Name ) );
			XmlElement tImport = (XmlElement)owner.ImportNode ( ele, true );
			foreach ( XmlElement tele in ele.SelectNodes ( "./*" ) ) {
				XmlNode n = owner.ImportNode ( tele, true );
				if ( n != null )
					tNew.AppendChild ( n );
			}

			return tNew;
		}

		/// <summary>
		/// Converts the element to source control.
		/// </summary>
		/// <param name="esc">The esc.</param>
		/// <returns></returns>
		private SourceControl ConvertElementToSourceControl ( XmlElement esc ) {
			XmlElement newEle = esc.OwnerDocument.CreateElement ( "sourcecontrol" );
			newEle.SetAttribute ( "type", esc.Name );
			//if ( !string.IsNullOrEmpty ( esc.GetAttribute ( "ccnetconfigType" ) ) )
			//newEle.SetAttribute ( "ccnetconfigType", esc.GetAttribute ( "ccnetconfigType" ) );

			foreach ( XmlElement ele in esc.SelectNodes ( "./*" ) ) {
				XmlElement clone = (XmlElement)ele.Clone ();
				newEle.AppendChild ( clone );
			}

			SourceControl sc = Util.GetSourceControlFromElement ( newEle );
			return sc;
		}

		#region ICCNetDocumentation Members
		/// <summary>
		/// Gets the documentation URI.
		/// </summary>
		/// <value>The documentation URI.</value>
		[Browsable ( false ), ReflectorIgnore ]
		public Uri DocumentationUri {
			get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Multi+Source+Control+Block?decorator=printable" ); }
		}

		#endregion
	}
}
