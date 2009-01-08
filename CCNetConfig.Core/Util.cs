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
using CCNetConfig.Exceptions;
using System.IO;
using System.Collections;
using CCNetConfig.Core.Enums;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using CCNetConfig.Core.Configuration;
using System.Configuration;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Web;
using CCNetConfig.Core.Configuration.Handlers;

namespace CCNetConfig.Core {
	/// <summary>
	/// A Static class that contains helper methods and properties.
	/// </summary>
	public static class Util {

		private static List<Type> _publisherTasks;
		private static List<Type> _sourceControl;
		private static List<Type> _triggers;
		private static List<Type> _labellers;
		private static List<Type> _states;
		private static List<Type> _projectExtensions;

		private static CCNetConfigSettings _userSettings = null;
		private static Dictionary<Type, TypeDescriptionProvider> _typeDescriptionProviders = null;

		private static Version _currentConfigurationVersion = new Version ( "1.0" );
		private static Version _lastConfigurationVersion = new Version ( "1.0" );

		/// <summary>
		/// Gets the user settings.
		/// </summary>
		/// <value>The user settings.</value>
		public static CCNetConfigSettings UserSettings {
			get {
				if ( _userSettings == null ) {
					FileInfo file = UserSettingsFile;
					if ( !file.Exists ) {
						string tpath = Path.Combine ( Path.GetDirectoryName ( typeof ( Util ).Assembly.Location ), @"Data\DefaultSettings.config" );
						FileInfo tfile = new FileInfo ( tpath );
						if ( !file.Directory.Exists )
							file.Directory.Create ();
						tfile.CopyTo ( file.FullName );
						file = new FileInfo ( UserSettingsFile.FullName );
						if ( file.Exists && file.IsReadOnly )
							file.IsReadOnly = false;
					}

					CCNetConfigSettingsConfigurationSectionHandler ccscsh = new CCNetConfigSettingsConfigurationSectionHandler ();
					_userSettings = ccscsh.Create ( null, null, file );
				}
				return _userSettings;
			}
		}

		/// <summary>
		/// Gets the display name.
		/// </summary>
		/// <param name="mi">The mi.</param>
		/// <returns></returns>
		public static SerializerValueAttribute GetSerializerValue ( MemberInfo mi ) {
			SerializerValueAttribute dna = GetCustomAttribute<SerializerValueAttribute> ( mi );
			return dna;
		}

		/// <summary>
		/// Gets the real name from serializer value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static string GetRealNameFromSerializerValue<T> ( string name ) {
			Type t = typeof ( T );
			MemberInfo[] mis = t.GetMembers ( /*BindingFlags.DeclaredOnly | BindingFlags.IgnoreCase | BindingFlags.GetField*/ );
			foreach ( MemberInfo mi in mis ) {
				SerializerValueAttribute dna = GetSerializerValue ( mi );
				if ( dna != null && string.Compare ( name, dna.Value , true ) == 0 )
					return mi.Name;
			}

			throw new ArgumentException ( string.Format ( "Member not found for type of {0} with a name of {1}", typeof ( T ).Name, name ) );
		}

		/// <summary>
		/// Refresh the user settings.
		/// </summary>
		public static void RefreshUserSettings () {
			_userSettings = null;
		}

		/// <summary>
		/// Gets the user settings file.
		/// </summary>
		/// <value>The user settings file.</value>
		public static FileInfo UserSettingsFile {
			get { return new FileInfo ( Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ), @"CCNetConfig\Settings.config" ) ); }
		}
		/// <summary>
		/// Gets the type description providers.
		/// </summary>
		/// <value>The type description providers.</value>
		public static Dictionary<Type, TypeDescriptionProvider> TypeDescriptionProviders {
			get {
				if ( _typeDescriptionProviders == null )
					_typeDescriptionProviders = new Dictionary<Type, TypeDescriptionProvider> ();
				return _typeDescriptionProviders;
			}
		}

		/// <summary>
		/// Gets the publisher tasks.
		/// </summary>
		/// <value>The publisher tasks.</value>
		public static List<Type> PublisherTasks {
			get {
				if ( _publisherTasks == null )
					_publisherTasks = CCNetConfig.Core.Util.GetAllPublisherTasks ( new DirectoryInfo ( Application.StartupPath ) );
				return _publisherTasks;
			}
		}

		/// <summary>
		/// Gets the filtered publisher tasks types.
		/// </summary>
		/// <returns></returns>
		public static List<Type> GetFilteredPublisherTaskTypes () {
			return GetFilteredTypes ( PublisherTasks );
		}

		/// <summary>
		/// Gets the source controls.
		/// </summary>
		/// <value>The source controls.</value>
		public static List<Type> SourceControls {
			get {
				if ( _sourceControl == null )
					_sourceControl = CCNetConfig.Core.Util.GetAllSourceControls ( new DirectoryInfo ( Application.StartupPath ) );
				return _sourceControl;
			}
		}

		/// <summary>
		/// Gets the filtered source control types.
		/// </summary>
		/// <returns></returns>
		public static List<Type> GetFilteredSourceControlTypes () {
			return GetFilteredTypes ( SourceControls );
		}

		/// <summary>
		/// Gets the labellers.
		/// </summary>
		/// <value>The labellers.</value>
		public static List<Type> Labellers {
			get {
				if ( _labellers == null )
					_labellers = CCNetConfig.Core.Util.GetAllLabellers ( new DirectoryInfo ( Application.StartupPath ) );
				return _labellers;
			}
		}

		/// <summary>
		/// Gets the filtered source control types.
		/// </summary>
		/// <returns></returns>
		public static List<Type> GetFilteredLabellerTypes () {
			return GetFilteredTypes ( Labellers );
		}


		/// <summary>
		/// Gets the triggers.
		/// </summary>
		/// <value>The triggers.</value>
		public static List<Type> Triggers {
			get {
				if ( _triggers == null )
					_triggers = CCNetConfig.Core.Util.GetAllTriggers ( new DirectoryInfo ( Application.StartupPath ) );
				return _triggers;
			}
		}

		/// <summary>
		/// Gets the filtered triggers types.
		/// </summary>
		/// <returns></returns>
		public static List<Type> GetFilteredTriggerTypes () {
			return GetFilteredTypes ( Triggers );
		}

		/// <summary>
		/// Gets the states.
		/// </summary>
		/// <value>The states.</value>
		public static List<Type> States {
			get {
				if ( _states == null )
					_states = CCNetConfig.Core.Util.GetAllStates ( new DirectoryInfo ( Application.StartupPath ) );
				return _states;
			}
		}

		/// <summary>
		/// Gets the filtered state types.
		/// </summary>
		/// <returns></returns>
		public static List<Type> GetFilteredStateTypes () {
			return GetFilteredTypes ( States );
		}

		/// <summary>
		/// Gets the project extensions.
		/// </summary>
		/// <value>The project extensions.</value>
		public static List<Type> ProjectExtensions {
			get {
				if ( _projectExtensions == null )
					_projectExtensions = Util.GetAllProjectExtensions ( new DirectoryInfo ( Application.StartupPath ) );
				return _projectExtensions;
			}
		}

		/// <summary>
		/// Filters the list of types based on the user settings and the minimum, maximum &amp; exact version attributes.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <returns></returns>
		private static List<Type> GetFilteredTypes ( List<Type> types ) {
			List<Type> ret = new List<Type> ();
			foreach ( Type t in types ) {
				if ( ( !UserSettings.Components.Contains ( t.FullName ) ) || ( UserSettings.Components.Contains ( t.FullName ) && UserSettings.Components[ t.FullName ].Display ) ) {
					if ( Util.IsInVersionRange ( Util.GetMinimumVersion ( t ), Util.GetMaximumVersion ( t ), _currentConfigurationVersion ) || Util.IsExactVersion ( GetExactVersion ( t ), _currentConfigurationVersion ) )
						ret.Add ( t );
				}
			}
			return ret;
		}

		/// <summary>
		/// Gets or sets the current configuration version.
		/// </summary>
		/// <value>The current configuration version.</value>
		public static Version CurrentConfigurationVersion {
			get { return _currentConfigurationVersion; }
			set {
				LastConfigurationVersion = CurrentConfigurationVersion;
				_currentConfigurationVersion = value;
			}
		}

		/// <summary>
		/// Gets or sets the last configuration version.
		/// </summary>
		/// <value>The last configuration version.</value>
		private static Version LastConfigurationVersion {
			get { return _lastConfigurationVersion; }
			set { _lastConfigurationVersion = value; }
		}

		/// <summary>
		/// Url encodes the string
		/// </summary>
		/// <param name="uri">The uri.</param>
		/// <returns></returns>
		public static string UrlEncode ( Uri uri ) {
			return HttpUtility.UrlPathEncode ( uri.ToString () );
		}

		/// <summary>
		/// Url encodes the string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>Url Encoded string</returns>
		public static string UrlEncode ( string value ) {
			return HttpUtility.UrlPathEncode ( value );
		}

		/// <summary>
		/// Decodes the Url Encoded string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string UrlDecode ( string value ) {
			return UrlDecode ( value, Encoding.Default );
		}

		/// <summary>
		/// Decodes the Url Encoded string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static string UrlDecode ( string value, Encoding encoding ) {
			return HttpUtility.UrlDecode ( value, encoding );
		}

		#region CheckRequired

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static object CheckRequired ( ICCNetObject owner, string name, object val ) {
			if ( val.GetType () == typeof ( HiddenPassword ) ) {
				return Util.CheckRequired ( owner, name, ( val as HiddenPassword ) ).GetPassword ();
			} else if ( val is IList ) {
				return Util.CheckRequired ( owner, name, ( val as IList ) );
			} else {
				if ( val == null )
					throw new RequiredAttributeException ( owner, name );
				else
					return val;
			}
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static int CheckRequired ( ICCNetObject owner, string name, int? val ) {
			if ( !val.HasValue )
				throw new RequiredAttributeException ( owner, name );
			else
				return val.Value;
		}
		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static string CheckRequired ( ICCNetObject owner, string name, string val ) {
			if ( string.IsNullOrEmpty ( val ) )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static HiddenPassword CheckRequired ( ICCNetObject owner, string name, HiddenPassword val ) {
			if ( val != null && string.IsNullOrEmpty ( val.Password ) )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static SourceControl CheckRequired ( ICCNetObject owner, string name, SourceControl val ) {
			if ( val == null )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}


		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static DateTime CheckRequired ( ICCNetObject owner, string name, DateTime? val ) {
			if ( !val.HasValue )
				throw new RequiredAttributeException ( owner, name );
			else
				return val.Value;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static Uri CheckRequired ( ICCNetObject owner, string name, Uri val ) {
			if ( val == null )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static AlienbrainUri CheckRequired ( ICCNetObject owner, string name, AlienbrainUri val ) {
			if ( val == null )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static TimeSpan CheckRequired ( ICCNetObject owner, string name, TimeSpan? val ) {
			if ( !val.HasValue )
				throw new RequiredAttributeException ( owner, name );
			else {
				if ( val.Value.Hours > 23 )
					throw new OverflowException ( "TimeSpan can be no longer then 24 hours. (00:00:00 - 23:59:59)" );
				return val.Value;
			}
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static DirectoryInfo CheckRequired ( ICCNetObject owner, string name, DirectoryInfo val ) {
			if ( val == null )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static FileInfo CheckRequired ( ICCNetObject owner, string name, FileInfo val ) {
			if ( val == null )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static IList CheckRequired ( ICCNetObject owner, string name, IList val ) {
			if ( val == null || val.Count == 0 )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static CloneableList<T> CheckRequired<T> ( ICCNetObject owner, string name, CloneableList<T> val ) {
			return CheckRequired<T> ( owner, name, val, 1 );
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <param name="min">The min.</param>
		/// <returns></returns>
		public static CloneableList<T> CheckRequired<T> ( ICCNetObject owner, string name, CloneableList<T> val, int min ) {
			if ( val == null || val.Count < min )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static List<string> CheckRequired ( ICCNetObject owner, string name, List<string> val ) {
			if ( val == null || val.Count == 0 )
				throw new RequiredAttributeException ( owner, name );
			else
				return val;
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static CloneableList<string> CheckRequired ( ICCNetObject owner, string name, CloneableList<string> val ) {
			return Util.CheckRequired<string> ( owner, name, val, 1 );
		}

		/// <summary>
		/// Checks the required.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static NotificationType CheckRequired ( ICCNetObject owner, string name, NotificationType? val ) {
			if ( !val.HasValue )
				throw new RequiredAttributeException ( owner, name );
			else
				return val.Value;
		}
		#endregion

		/// <summary>
		/// Gets the config file version.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		public static Version GetConfigFileVersion ( FileInfo file ) {
			if ( file.Exists ) {
				try {
					XmlDocument doc = new XmlDocument ();
					doc.Load ( file.FullName );
					return GetConfigFileVersion ( doc );
				} catch {
					return null;
				}
			} else
				return null;
		}

		/// <summary>
		/// Gets the config file version.
		/// </summary>
		/// <param name="doc">The doc.</param>
		/// <returns></returns>
		public static Version GetConfigFileVersion ( XmlDocument doc ) {
			try {
				if ( doc.ChildNodes != null && doc.ChildNodes.Count > 0 && doc.ChildNodes[ 0 ].NodeType == XmlNodeType.Comment ) {
					string commentData = doc.ChildNodes[ 0 ].InnerText;
					doc = new XmlDocument ();
					doc.LoadXml ( commentData );
					XmlElement vele = doc.DocumentElement.SelectSingleNode ( "configurationVersion" ) as XmlElement;
					return new Version ( vele.InnerText );
				} else
					return null;
			} catch {
				return null;
			}
		}
		/// <summary>
		/// Registers the type descriptor providers.
		/// </summary>
		/// <param name="version">The version.</param>
		public static void RegisterTypeDescriptionProviders ( Version version ) {
			RegisterTypeDescriptionProvider ( typeof ( Project ), version );
			RegisterTypeDescriptionProvider ( typeof ( SourceControl ), version );
			RegisterTypeDescriptionProvider ( typeof ( State ), version );
			RegisterTypeDescriptionProvider ( typeof ( Labeller ), version );
			RegisterTypeDescriptionProvider ( typeof ( PublisherTask ), version );
			RegisterTypeDescriptionProvider ( typeof ( Trigger ), version );
		}

		/// <summary>
		/// Registers the type descriptor provider.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="version">The version.</param>
		internal static void RegisterTypeDescriptionProvider ( Type type, Version version ) {
			if ( TypeDescriptionProviders.ContainsKey ( type ) ) {
				TypeDescriptor.RemoveProvider ( TypeDescriptionProviders[ type ], type );
				TypeDescriptionProviders[ type ] = new VersionBasedTypeDescriptionProvider ( type, version );
			} else {
				TypeDescriptionProviders.Add ( type, new VersionBasedTypeDescriptionProvider ( type, version ) );
			}
			TypeDescriptor.AddProvider ( TypeDescriptionProviders[ type ], type );
		}


		/// <summary>
		/// Gets the type description provider.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static TypeDescriptionProvider GetTypeDescriptionProvider ( Type type ) {
			if ( TypeDescriptionProviders.ContainsKey ( type ) )
				return TypeDescriptionProviders[ type ];
			else
				return TypeDescriptor.GetProvider ( type );
		}

		/// <summary>
		/// Gets the type description provider version.
		/// </summary>
		/// <remarks>Will attempt to fall back to a base type until object if it can not find the version
		/// info from the passed type. Will default to the version info of the project object if all else fails.</remarks>
		/// <param name="type">The type.</param>
		/// <returns>The version selected for the configuration.</returns>
		public static Version GetTypeDescriptionProviderVersion ( Type type ) {
			TypeDescriptionProvider tdp = GetTypeDescriptionProvider ( type );
			if ( tdp.GetType () == typeof ( VersionBasedTypeDescriptionProvider ) ) {
				VersionBasedTypeDescriptionProvider vtdp = tdp as VersionBasedTypeDescriptionProvider;
				return vtdp.Version;
			} else {
				if ( type.BaseType != null ) {
					return GetTypeDescriptionProviderVersion ( type.BaseType );
				} else {
					return GetTypeDescriptionProviderVersion ( typeof ( Project ) );
				}
			}
		}

		/// <summary>
		/// Gets the minimum version attribute for a property.
		/// </summary>
		/// <param name="mi">The mi.</param>
		/// <returns></returns>
		public static Version GetMinimumVersion ( MemberInfo mi ) {
			if ( mi == null )
				return null;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties ( mi.DeclaringType );
			PropertyDescriptor pd = properties.Find ( mi.Name, false );
			if ( pd != null )
				return GetMinimumVersion ( pd );
			else
				return null;
		}

		/// <summary>
		/// Gets the minimum version attribute for a property.
		/// </summary>
		/// <param name="pd">The pd.</param>
		/// <returns></returns>
		public static Version GetMinimumVersion ( PropertyDescriptor pd ) {
			MinimumVersionAttribute minVersion = pd.Attributes[ typeof ( MinimumVersionAttribute ) ] as MinimumVersionAttribute;
			if ( minVersion != null )
				return minVersion.Version;
			else
				return null;
		}

		/// <summary>
		/// Gets the minimum version attribute for a property.
		/// </summary>
		/// <param name="mi">The mi.</param>
		/// <returns></returns>
		public static Version GetMaximumVersion ( MemberInfo mi ) {
			if ( mi == null )
				return null;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties ( mi.DeclaringType );
			PropertyDescriptor pd = properties.Find ( mi.Name, false );
			if ( pd != null )
				return GetMaximumVersion ( pd );
			else
				return null;
		}

		/// <summary>
		/// Gets the maximum version attribute for a property.
		/// </summary>
		/// <param name="pd">The pd.</param>
		/// <returns></returns>
		public static Version GetMaximumVersion ( PropertyDescriptor pd ) {
			MaximumVersionAttribute maxVersion = pd.Attributes[ typeof ( MaximumVersionAttribute ) ] as MaximumVersionAttribute;
			if ( maxVersion != null )
				return maxVersion.Version;
			else
				return null;
		}

		/// <summary>
		/// Gets the minimum version attribute for a property.
		/// </summary>
		/// <param name="mi">The mi.</param>
		/// <returns></returns>
		public static Version GetExactVersion ( MemberInfo mi ) {
			if ( mi == null )
				return null;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties ( mi.DeclaringType );
			PropertyDescriptor pd = properties.Find ( mi.Name, false );
			if ( pd != null )
				return GetExactVersion ( pd );
			else
				return null;
		}

		/// <summary>
		/// Gets the exact version attribute for a property.
		/// </summary>
		/// <param name="pd">The pd.</param>
		/// <returns></returns>
		public static Version GetExactVersion ( PropertyDescriptor pd ) {
			ExactVersionAttribute exactVersion = pd.Attributes[ typeof ( ExactVersionAttribute ) ] as ExactVersionAttribute;
			if ( exactVersion != null )
				return exactVersion.Version;
			else
				return null;
		}

		/// <summary>
		/// Gets the minimum version.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static Version GetMinimumVersion ( Type type ) {
			Attribute[ ] attr = type.GetCustomAttributes ( typeof ( MinimumVersionAttribute ), true ) as Attribute[];
			if ( attr != null && attr.Length > 0 ) {
				MinimumVersionAttribute mva = attr[ 0 ] as MinimumVersionAttribute;
				return mva.Version;
			} else
				return null;
		}

		/// <summary>
		/// Gets the maximum version.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static Version GetMaximumVersion ( Type type ) {
			Attribute[ ] attr = type.GetCustomAttributes ( typeof ( MaximumVersionAttribute ), true ) as Attribute[];
			if ( attr != null && attr.Length > 0 ) {
				MaximumVersionAttribute mva = attr[ 0 ] as MaximumVersionAttribute;
				return mva.Version;
			} else
				return null;
		}

		/// <summary>
		/// Gets the exact version.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static Version GetExactVersion ( Type type ) {
			Attribute[ ] attr = type.GetCustomAttributes ( typeof ( ExactVersionAttribute ), true ) as Attribute[];
			if ( attr != null && attr.Length > 0 ) {
				ExactVersionAttribute mva = attr[ 0 ] as ExactVersionAttribute;
				return mva.Version;
			} else
				return null;
		}

		/// <summary>
		/// Gets the property descriptor.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>The property descriptor</returns>
		public static PropertyDescriptor GetPropertyDescriptor ( Type type, string propertyName ) {
			return GetPropertyDescriptor ( type, propertyName, true );
		}

		/// <summary>
		/// Gets the property descriptor.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
		/// <returns>The property descriptor</returns>
		public static PropertyDescriptor GetPropertyDescriptor ( Type type, string propertyName, bool ignoreCase ) {
			PropertyDescriptorCollection pdc = null;
			if ( TypeDescriptionProviders.ContainsKey ( type ) )
				pdc = TypeDescriptionProviders[ type ].GetTypeDescriptor ( type ).GetProperties ();
			else
				pdc = TypeDescriptor.GetProperties ( type );
			PropertyDescriptor pd = pdc.Find ( propertyName, ignoreCase );
			//if (pd == null)
			//throw new ArgumentException ( string.Format ( "Property '{0}' was not found.", propertyName ), "propertyName" );
			return pd;
		}

		/// <summary>
		/// Determines whether compare is within the version range of min and max.
		/// </summary>
		/// <param name="min">The min.</param>
		/// <param name="max">The max.</param>
		/// <param name="compare">The compare.</param>
		/// <returns>
		/// 	<c>true</c> if is in version range; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInVersionRange ( Version min, Version max, Version compare ) {
			return ( compare.CompareTo ( min ) >= 0 || min == null ) && ( compare.CompareTo ( max ) <= 0 || max == null );
		}

		/// <summary>
		/// Determines whether the specified version is exact version.
		/// </summary>
		/// <param name="exact">The exact.</param>
		/// <param name="compare">The compare.</param>
		/// <returns>
		/// 	<c>true</c> if the specified version is exactversion; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsExactVersion ( Version exact, Version compare ) {
			if ( exact != null )
				return compare.CompareTo ( exact ) == 0;
			else
				return false;
		}

		public static string GetFormatAttributeValue ( MemberInfo mi ) {
			FormatProviderAttribute fpa = Util.GetCustomAttribute<FormatProviderAttribute> ( mi );
			if ( fpa != null ) {
				return fpa.Format;
			} else {
				return string.Empty;
			}
		}

		/// <summary>
		/// Converts a Boolean to an integer.
		/// </summary>
		/// <param name="val">if set to <c>true</c> 1, otherwise 0.</param>
		/// <returns>0 or 1</returns>
		public static int BooleanToInteger ( bool val ) {
			return val ? 1 : 0;
		}

		/// <summary>
		/// Gets the reflector array attribute value.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static string GetReflectorArrayAttributeValue ( Type type ) {
			ReflectorArrayAttribute raa = Util.GetCustomAttribute<ReflectorArrayAttribute> ( type );
			if ( raa != null )
				return raa.ItemName;
			else {
				throw new ArgumentException ( string.Format ( "Type ({0}) does not contain a ReflectorArray Attribute", type.Name ) );
			}
		}

		/// <summary>
		/// Gets the reflector array attribute value.
		/// </summary>
		/// <param name="mi">The mi.</param>
		/// <returns></returns>
		public static string GetReflectorArrayAttributeValue ( MemberInfo mi ) {
			ReflectorArrayAttribute raa = Util.GetCustomAttribute<ReflectorArrayAttribute> ( mi );
			if ( raa != null ) {
				return raa.ItemName;
			} else {
				throw new ArgumentException ( string.Format ( "Member ({0}) does not contain a ReflectorArray Attribute", mi.Name ) );
			}
		}

		/// <summary>
		/// Gets the reflector name attribute value.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">Throws if the type does not contain the attribute.</exception>
		public static string GetReflectorNameAttributeValue ( Type type ) {
			ReflectorNameAttribute rna = Util.GetCustomAttribute<ReflectorNameAttribute> ( type );
			if ( rna != null )
				return rna.Name;
			else {
				if ( type.BaseType != typeof ( Object ) )
					return Util.GetReflectorNameAttributeValue ( type.BaseType );
				else
					throw new ArgumentException ( string.Format ( "Type ({0}) does not contain a ReflectorName Attribute", type.Name ) );
			}
		}
		/// <summary>
		/// Gets the reflector name attribute value.
		/// </summary>
		/// <param name="mi">The member info.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">Throws if the type does not contain the attribute.</exception>
		public static string GetReflectorNameAttributeValue ( MemberInfo mi ) {
			ReflectorNameAttribute rna = Util.GetCustomAttribute<ReflectorNameAttribute> ( mi );
			if ( rna != null )
				return rna.Name;
			else
				throw new ArgumentException ( string.Format ( "Member ({0}) does not contain a ReflectorName Attribute", mi.Name ) );
		}

		/// <summary>
		/// Gets the type of the reflector node.
		/// </summary>
		/// <param name="mi">The mi.</param>
		/// <returns></returns>
		public static ReflectorNodeTypes GetReflectorNodeType ( MemberInfo mi ) {
			ReflectorNodeTypeAttribute rnta = Util.GetCustomAttribute<ReflectorNodeTypeAttribute> ( mi );
			if ( rnta != null )
				return rnta.Type;
			else
				return ReflectorNodeTypes.Element;
		}

		/// <summary>
		/// Gets the string separator.
		/// </summary>
		/// <param name="mi">The member info.</param>
		/// <returns></returns>
		public static string GetStringSeparatorAttributeValue ( MemberInfo mi ) {
			StringSeparatorAttribute ssa = Util.GetCustomAttribute<StringSeparatorAttribute> ( mi );
			if ( ssa != null )
				return ssa.Separator;
			else
				return ";";
		}

		/// <summary>
		/// Gets the attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static T GetCustomAttribute<T> ( Type type ) where T : Attribute {
			object[ ] attr = type.GetCustomAttributes ( typeof ( T ), true ) as Attribute[];
			if ( attr != null && attr.Length > 0 ) {
				return (T)attr[ 0 ];
			} else
				return default ( T );
		}

		/// <summary>
		/// Gets the attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="mi">The mi.</param>
		/// <returns></returns>
		public static T GetCustomAttribute<T> ( MemberInfo mi ) where T : Attribute {
			Attribute attrib = Attribute.GetCustomAttribute ( mi, typeof ( T ) );
			if ( attrib != null ) {
				return (T)attrib;
			}
			object[ ] attr = mi.GetCustomAttributes ( typeof ( T ), true ) as Attribute[];
			if ( attr != null && attr.Length > 0 ) {
				return (T)attr[ 0 ];
			} else
				return default ( T );
		}

		/// <summary>
		/// Gets all publisher tasks.
		/// </summary>
		/// <param name="pluginPath">The plugin path.</param>
		/// <returns></returns>
		public static List<Type> GetAllPublisherTasks ( DirectoryInfo pluginPath ) {
			return GetItemsByBaseType ( pluginPath, typeof ( PublisherTask ) );
			;
		}

		/// <summary>
		/// Gets all source controls.
		/// </summary>
		/// <param name="pluginPath">The plugin path.</param>
		/// <returns></returns>
		public static List<Type> GetAllSourceControls ( DirectoryInfo pluginPath ) {
			return GetItemsByBaseType ( pluginPath, typeof ( SourceControl ) );
			;
		}

		/// <summary>
		/// Gets all triggers.
		/// </summary>
		/// <param name="pluginPath">The plugin path.</param>
		/// <returns></returns>
		public static List<Type> GetAllTriggers ( DirectoryInfo pluginPath ) {
			return GetItemsByBaseType ( pluginPath, typeof ( Trigger ) );
			;
		}

		/// <summary>
		/// Gets all states.
		/// </summary>
		/// <param name="pluginPath">The plugin path.</param>
		/// <returns></returns>
		public static List<Type> GetAllStates ( DirectoryInfo pluginPath ) {
			return GetItemsByBaseType ( pluginPath, typeof ( State ) );
			;
		}

		/// <summary>
		/// Gets all labellers.
		/// </summary>
		/// <param name="pluginPath">The plugin path.</param>
		/// <returns></returns>
		public static List<Type> GetAllLabellers ( DirectoryInfo pluginPath ) {
			return GetItemsByBaseType ( pluginPath, typeof ( Labeller ) );
			;
		}

		/// <summary>
		/// Gets all project extensions.
		/// </summary>
		/// <param name="pluginPath">The plugin path.</param>
		/// <returns></returns>
		public static List<Type> GetAllProjectExtensions ( DirectoryInfo pluginPath ) {
			return GetItemsByBaseType ( pluginPath, typeof ( ProjectExtension ) );
		}

		/// <summary>
		/// Gets the type of the items by base.
		/// </summary>
		/// <param name="dir">The dir.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		private static List<Type> GetItemsByBaseType ( DirectoryInfo dir, Type type ) {
			List<Type> pt = new List<Type> ();
			Assembly thisAsm = typeof ( Util ).Assembly;

			foreach ( Type t in thisAsm.GetTypes () ) {
				if ( t.IsSubclassOf ( type ) )
					pt.Add ( t );
			}

			foreach ( FileInfo fi in dir.GetFiles ( "*.exe" ) ) {
				try {
					Assembly asm = Assembly.LoadFile ( fi.FullName );
					if ( asm != null && asm != thisAsm ) {
						foreach ( Type t in asm.GetTypes () ) {
							if ( t.IsSubclassOf ( type ) )
								pt.Add ( t );
						}
					}
				} catch ( Exception ex ) { Console.WriteLine ( ex.ToString () ); }
			}

			foreach ( FileInfo fi in dir.GetFiles ( "*.dll" ) ) {
				try {
					Assembly asm = Assembly.LoadFile ( fi.FullName );
					if ( asm != null && asm != thisAsm ) {
						foreach ( Type t in asm.GetTypes () ) {
							if ( t.IsSubclassOf ( type ) )
								pt.Add ( t );
						}
					}
				} catch ( Exception ) { }
			}
			pt.Sort ( new TypeComparer () );
			return pt;
		}

		/// <summary>
		/// Gets the type of the items by interface.
		/// </summary>
		/// <param name="dir">The dir.</param>
		/// <param name="type">The type.</param>
		/// <returns>List of types</returns>
		public static List<Type> GetItemsByInterfaceType ( DirectoryInfo dir, Type type ) {
			List<Type> pt = new List<Type> ();
			Assembly thisAsm = typeof ( Util ).Assembly;

			foreach ( Type t in thisAsm.GetTypes () ) {
				if ( t.GetInterface ( type.FullName, true ) != null )
					pt.Add ( t );
			}

			foreach ( FileInfo fi in dir.GetFiles ( "*.exe" ) ) {
				try {
					Assembly asm = Assembly.LoadFile ( fi.FullName );
					if ( asm != null && asm != thisAsm ) {
						foreach ( Type t in asm.GetTypes () ) {
							if ( t.GetInterface ( type.FullName, true ) != null )
								pt.Add ( t );
						}
					}
				} catch ( Exception ex ) { Console.WriteLine ( ex.ToString () ); }
			}

			foreach ( FileInfo fi in dir.GetFiles ( "*.dll" ) ) {
				try {
					Assembly asm = Assembly.LoadFile ( fi.FullName );
					if ( asm != null && asm != thisAsm ) {
						foreach ( Type t in asm.GetTypes () ) {
							if ( t.GetInterface ( type.FullName, true ) != null )
								pt.Add ( t );
						}
					}
				} catch ( Exception ex ) { Console.WriteLine ( ex.ToString () ); }
			}
			pt.Sort ( new TypeComparer () );
			return pt;
		}

		/// <summary>
		/// Creates an instance of a Type
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static object CreateInstanceOfType ( Type t ) {
			Assembly asm = t.Assembly;
			return asm.CreateInstance ( t.FullName, true );
		}


		/// <summary>
		/// Creates the type from string.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static Type CreateTypeFromString ( string type ) {
			string[ ] split = type.Split ( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			if ( split.Length == 2 ) {
				Assembly asm = Assembly.Load ( split[ 1 ].Trim () );
				if ( asm != null ) {
					return asm.GetType ( split[ 0 ].Trim (), true, true );
				} else
					throw new ArgumentException ( string.Format ( "Unable to load assembly {0}", split[ 1 ].Trim () ) );
			} else
				throw new ArgumentException ( "type string is an invalid format." );
		}

		/// <summary>
		/// This creates a <see cref="CCNetConfig.Core.Trigger">Trigger</see> from an <see cref="System.Xml.XmlElement">XmlElement</see>.
		/// </summary>
		/// <param name="element"><see cref="System.Xml.XmlElement">XmlElement</see></param>
		/// <returns><see cref="CCNetConfig.Core.Trigger">Trigger</see></returns>
		/// <remarks>This uses 2 different methods to convert the <see cref="System.Xml.XmlElement">XmlElement</see> to a
		/// <see cref="CCNetConfig.Core.Trigger">Trigger</see>. If CCNetConfig saved the configuration file before, it added an attribute to some of the
		/// <see cref="System.Xml.XmlElement">XmlElement</see>s in the config file. This attribute tells CCNetConfig exactly what type of object to 
		/// load. If the attribute does not exists, or it is empty, then all of the SubClasses of <see cref="CCNetConfig.Core.Trigger">Trigger</see>
		/// are compared to the element by <see cref="CCNetConfig.Core.Trigger.TypeName">TypeName</see>. If these match, then the <see cref="CCNetConfig.Core.Trigger">Trigger</see>
		/// was found and we create an instance of it and return it.
		/// </remarks>
		public static Trigger GetTriggerFromElement ( XmlElement element ) {
			if ( !string.IsNullOrEmpty ( element.GetAttribute ( "ccnetconfigType" ) ) ) {
				Type t = CreateTypeFromString ( element.GetAttribute ( "ccnetconfigType" ) );
				if ( t.IsSubclassOf ( typeof ( Trigger ) ) ) {
					Trigger trig = (Trigger)Util.CreateInstanceOfType ( t );
					trig.Deserialize ( element );
					return trig;
				} else
					throw new ArgumentException ( string.Format ( "Unable to load type {0}", t.FullName ) );
			} else {
				foreach ( Type t in Util.Triggers ) {
					Trigger trigger = (Trigger)Util.CreateInstanceOfType ( t );
					if ( string.Compare ( element.Name, trigger.TypeName, false ) == 0 ) {
						// we found our trigger
						trigger.Deserialize ( element );
						return trigger;
					} else
						trigger = null;
				}
			}
			return null;
		}


		/// <summary>
		/// This creates a <see cref="CCNetConfig.Core.PublisherTask">PublisherTask</see> from an <see cref="System.Xml.XmlElement">XmlElement</see>.
		/// </summary>
		/// <param name="element"><see cref="System.Xml.XmlElement">XmlElement</see></param>
		/// <returns><see cref="CCNetConfig.Core.PublisherTask">PublisherTask</see></returns>
		/// <remarks>This uses 2 different methods to convert the <see cref="System.Xml.XmlElement">XmlElement</see> to a 
		/// <see cref="CCNetConfig.Core.PublisherTask">PublisherTask</see>. If CCNetConfig saved the configuration file before, it added an attribute to some of the
		/// <see cref="System.Xml.XmlElement">XmlElement</see>s in the config file. This attribute tells CCNetConfig exactly what type of object to 
		/// load. If the attribute does not exists, or it is empty, then all of the SubClasses of <see cref="CCNetConfig.Core.PublisherTask">PublisherTask</see>
		/// are compared to the element by <see cref="CCNetConfig.Core.PublisherTask.TypeName">TypeName</see>. If these match, then the 
		/// <see cref="CCNetConfig.Core.PublisherTask">PublisherTask</see>
		/// was found and we create an instance of it and return it.
		/// </remarks>
		public static PublisherTask GetPublisherTaskFromElement ( XmlElement element ) {
			if ( !string.IsNullOrEmpty ( element.GetAttribute ( "ccnetconfigType" ) ) ) {
				Type t = CreateTypeFromString ( element.GetAttribute ( "ccnetconfigType" ) );
				if ( t.IsSubclassOf ( typeof ( PublisherTask ) ) ) {
					PublisherTask pt = (PublisherTask)Util.CreateInstanceOfType ( t );
					pt.Deserialize ( element );
					return pt;
				} else
					throw new ArgumentException ( string.Format ( "Unable to load type {0}", t.FullName ) );
			} else {
				foreach ( Type t in Util.PublisherTasks ) {
					PublisherTask pt = (PublisherTask)Util.CreateInstanceOfType ( t );
					if ( string.Compare ( element.Name, pt.TypeName, false ) == 0 ) {
						// we found our trigger
						pt.Deserialize ( element );
						return pt;
					} else
						pt = null;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the source control from element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		public static SourceControl GetSourceControlFromElement ( XmlElement element ) {
			if ( !string.IsNullOrEmpty ( element.GetAttribute ( "ccnetconfigType" ) ) ) {
				Type t = CreateTypeFromString ( element.GetAttribute ( "ccnetconfigType" ) );
				if ( t.IsSubclassOf ( typeof ( SourceControl ) ) ) {
					SourceControl sc = (SourceControl)Util.CreateInstanceOfType ( t );
					sc.Deserialize ( element );
					return sc;
				} else
					throw new ArgumentException ( string.Format ( "Unable to load type {0}", t.FullName ) );
			} else {
				foreach ( Type t in Util.SourceControls ) {
					SourceControl sc = (SourceControl)Util.CreateInstanceOfType ( t );
					if ( string.Compare ( element.GetAttribute ( "type" ), sc.TypeName, false ) == 0 ) {
						// we found our trigger
						sc.Deserialize ( element );
						return sc;
					} else
						sc = null;
				}
			}
			return null;
		}

		/// <summary>
		/// This creates a <see cref="CCNetConfig.Core.Labeller">Labeller</see> from an <see cref="System.Xml.XmlElement">XmlElement</see>.
		/// </summary>
		/// <param name="element"><see cref="System.Xml.XmlElement">XmlElement</see></param>
		/// <returns><see cref="CCNetConfig.Core.Labeller">Labeller</see></returns>
		/// <remarks>This uses 2 different methods to convert the <see cref="System.Xml.XmlElement">XmlElement</see> to a 
		/// <see cref="CCNetConfig.Core.Labeller">Labeller</see>. If CCNetConfig saved the configuration file before, it added an attribute to some of the
		/// <see cref="System.Xml.XmlElement">XmlElement</see>s in the config file. This attribute tells CCNetConfig exactly what type of object to 
		/// load. If the attribute does not exists, or it is empty, then all of the SubClasses of <see cref="CCNetConfig.Core.Trigger">Trigger</see>
		/// are compared to the element by <see cref="CCNetConfig.Core.Labeller.TypeName">TypeName</see>. If these match, then the 
		/// <see cref="CCNetConfig.Core.Labeller">Labeller</see>
		/// was found and we create an instance of it and return it.
		/// </remarks>
		public static Labeller GetLabellerFromElement ( XmlElement element ) {
			if ( !string.IsNullOrEmpty ( element.GetAttribute ( "ccnetconfigType" ) ) ) {
				Type t = CreateTypeFromString ( element.GetAttribute ( "ccnetconfigType" ) );
				if ( t.IsSubclassOf ( typeof ( Labeller ) ) ) {
					Labeller lbllr = (Labeller)Util.CreateInstanceOfType ( t );
					lbllr.Deserialize ( element );
					return lbllr;
				} else
					throw new ArgumentException ( string.Format ( "Unable to load type {0}", t.FullName ) );
			} else {
				foreach ( Type t in Util.Labellers ) {
					Labeller labeller = (Labeller)Util.CreateInstanceOfType ( t );
					if ( string.Compare ( element.GetAttribute ( "type" ), labeller.TypeName, false ) == 0 ) {
						// we found our trigger
						labeller.Deserialize ( element );
						return labeller;
					} else
						labeller = null;
				}
			}
			return null;
		}


		/// <summary>
		/// This creates a <see cref="CCNetConfig.Core.State">State</see> from an <see cref="System.Xml.XmlElement">XmlElement</see>.
		/// </summary>
		/// <param name="element"><see cref="System.Xml.XmlElement">XmlElement</see></param>
		/// <returns><see cref="CCNetConfig.Core.State">State</see></returns>
		/// <remarks>This uses 2 different methods to convert the <see cref="System.Xml.XmlElement">XmlElement</see> to a
		/// <see cref="CCNetConfig.Core.State">State</see>. If CCNetConfig saved the configuration file before, it added an attribute to some of the
		/// <see cref="System.Xml.XmlElement">XmlElement</see>s in the config file. This attribute tells CCNetConfig exactly what type of object to 
		/// load. If the attribute does not exists, or it is empty, then all of the SubClasses of <see cref="CCNetConfig.Core.State">State</see>
		/// are compared to the element by <see cref="CCNetConfig.Core.State.Type">Type</see>. If these match, then the <see cref="CCNetConfig.Core.State">State</see>
		/// was found and we create an instance of it and return it.
		/// </remarks>
		public static State GetStateFromElement ( XmlElement element ) {
			// this is no longer needed. it was used by early versions.
			if ( !string.IsNullOrEmpty ( element.GetAttribute ( "ccnetconfigType" ) ) ) {
				Type t = CreateTypeFromString ( element.GetAttribute ( "ccnetconfigType" ) );
				if ( t.IsSubclassOf ( typeof ( State ) ) ) {
					State state = (State)Util.CreateInstanceOfType ( t );
					state.Deserialize ( element );
					return state;
				} else
					throw new ArgumentException ( string.Format ( "Unable to load type {0}", t.FullName ) );
			} else {
				foreach ( Type t in Util.States ) {
					State state = (State)Util.CreateInstanceOfType ( t );
					if ( string.Compare ( element.GetAttribute ( "type" ), state.Type, false ) == 0 ) {
						// we found our trigger
						state.Deserialize ( element );
						return state;
					} else
						state = null;
				}
			}
			return null;
		}

		/// <summary>
		/// Since CCNet seems to support the property values as <see cref="System.Xml.XmlAttribute">XmlAttribute</see>s or 
		/// <see cref="System.Xml.XmlElement">XmlElement</see>s, this looks for the 
		/// property name as an <see cref="System.Xml.XmlAttribute">XmlAttribute</see> and as an <see cref="System.Xml.XmlElement">XmlElement</see>.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="element"></param>
		/// <returns></returns>
		public static string GetElementOrAttributeValue ( string name, XmlElement element ) {
			if ( !string.IsNullOrEmpty ( element.GetAttribute ( name ) ) )
				return element.GetAttribute ( name );

			XmlElement ele = (XmlElement)element.SelectSingleNode ( name );
			if ( ele != null )
				return ele.InnerText;

			return string.Empty;
		}

		/// <summary>
		/// Creates the project comments.
		/// </summary>
		/// <param name="ccnet">The ccnet.</param>
		/// <returns></returns>
		public static XmlDocument CreateProjectComments ( CruiseControl ccnet ) {
			XmlDocument doc = new XmlDocument ();
			doc.AppendChild ( doc.CreateElement ( "ccnetconfig" ) );
			XmlElement ele = doc.CreateElement ( "configurationVersion" );
			ele.InnerText = ccnet.Version.ToString ();
			doc.DocumentElement.AppendChild ( ele );

			return doc;
		}

		/// <summary>
		/// Resets the object properties automatically based on the type and custom attributes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		public static void ResetObjectProperties<T> ( T obj ) {
			PropertyInfo[] props = obj.GetType ().GetProperties ( BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty );
			foreach ( PropertyInfo pi in props ) {
				if ( !pi.CanWrite ) {
					continue;
				}
				DefaultValueAttribute dva = Util.GetCustomAttribute<DefaultValueAttribute> ( pi );
				// check if the object allows nulls
				TypeConverterAttribute tca = Util.GetCustomAttribute<TypeConverterAttribute> ( pi );
				// check the property for the nullorobject attribute
				NullOrObjectAttribue nooa = Util.GetCustomAttribute<NullOrObjectAttribue> ( pi );
				// check the property type for the nullorobject attribute
				NullOrObjectAttribue nooat = Util.GetCustomAttribute<NullOrObjectAttribue> ( pi.PropertyType );
				bool allowsNull = ( tca != null && string.Compare ( tca.ConverterTypeName, typeof ( ObjectOrNoneTypeConverter ).Name ) == 0 ) || nooa != null || nooat != null;
				ConstructorInfo constructorInfo = pi.PropertyType.GetConstructor ( new Type[] { } );
				if ( dva != null ) {
					if ( pi.PropertyType.IsClass && !Util.IsNullable ( pi.PropertyType ) && !allowsNull && constructorInfo != null ) {
						try {
							object tobj = constructorInfo.Invoke ( new object[] { } );
							pi.SetValue ( obj, tobj, null );
						} catch ( Exception ex ) {
							throw;
						}
					} else {
						pi.SetValue ( obj, dva.Value, null );
					}
				} else {
					if ( pi.PropertyType.IsClass && !Util.IsNullable ( pi.PropertyType ) && !allowsNull && constructorInfo != null ) {
						try {
							object tobj = constructorInfo.Invoke ( new object[] { } );
							pi.SetValue ( obj, tobj, null );
						} catch ( Exception ex ) {
							throw;
						}
					} else {
						pi.SetValue ( obj, null, null );
					}
				}
			}
		}

		/// <summary>
		/// Decodes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string Decode ( string value ) {
			System.Security.Cryptography.TripleDESCryptoServiceProvider des = new System.Security.Cryptography.TripleDESCryptoServiceProvider ();
			des.Mode = System.Security.Cryptography.CipherMode.CBC;
			des.IV = Convert.FromBase64String ( Properties.Resources.InitializeVectorCode );
			des.Key = Convert.FromBase64String ( Properties.Resources.HashCode );

			System.Security.Cryptography.ICryptoTransform trans = des.CreateDecryptor ();
			byte[ ] buffer = Convert.FromBase64String ( value );
			byte[ ] decData = trans.TransformFinalBlock ( buffer, 0, buffer.Length );
			return System.Text.Encoding.Default.GetString ( decData );
		}

		/// <summary>
		/// Encodes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string Encode ( string value ) {
			System.Security.Cryptography.TripleDESCryptoServiceProvider des = new System.Security.Cryptography.TripleDESCryptoServiceProvider ();
			des.Mode = System.Security.Cryptography.CipherMode.CBC;
			des.IV = Convert.FromBase64String ( Properties.Resources.InitializeVectorCode );
			des.Key = Convert.FromBase64String ( Properties.Resources.HashCode );

			System.Security.Cryptography.ICryptoTransform trans = des.CreateEncryptor ();
			byte[ ] buffer = System.Text.Encoding.Default.GetBytes ( value );
			byte[ ] encData = trans.TransformFinalBlock ( buffer, 0, buffer.Length );
			return Convert.ToBase64String ( encData, Base64FormattingOptions.None );
		}

		/// <summary>
		/// Converts a string to an enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static T StringToEnum<T> ( string val ) {
			if ( string.IsNullOrEmpty ( val ) )
				return default ( T );
			else {
				return (T)Enum.Parse ( typeof ( T ), val, true );
			}
		}

		/// <summary>
		/// Determines whether the specified t is nullable.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns>
		/// 	<c>true</c> if the specified t is nullable; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullable ( Type t ) {
			return ( t.IsGenericType && t.GetGenericTypeDefinition ().Equals ( typeof ( Nullable<> ) ) );
		}
	}

	/// <summary>
	/// Compares 2 types of objects.
	/// </summary>
	internal class TypeComparer : Comparer<Type> {
		/// <summary>
		/// When overridden in a derived class, performs a comparison of two objects of the same type and returns a value indicating whether one object is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// Value Condition Less than zero x is less than y.Zero x equals y.Greater than zero x is greater than y.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">Type T does not implement either the <see cref="T:System.IComparable`1"></see> generic interface or the <see cref="T:System.IComparable"></see> interface.</exception>
		public override int Compare ( Type x, Type y ) {
			return string.Compare ( x.Name, y.Name );
		}
	}



}
