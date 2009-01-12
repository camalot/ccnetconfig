/*
 * Copyright (c) 2006-2008, Ryan Conrad. All rights reserved.
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
using System.Reflection;
using System.Xml;
using CCNetConfig.Core.Components;
using System.Collections;
using CCNetConfig.Exceptions;
using System.Collections.Generic;

namespace CCNetConfig.Core.Serialization {
	/// <summary>
	/// Provides serialization and deserialation of ICCNetObjects
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Serializer<T> where T : ICCNetObject {
		#region ISerialize Members

		/// <summary>
		/// Serializes the specified obj.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public System.Xml.XmlElement Serialize ( T obj ) {
			Type type = obj.GetType ();
			try {
				// should the version of the object be checked before any other stuff is done? 
				// no need to do anything if it doesn't event belong in here anyhow.

				// should limit to public, instance, read/write properties... 
				PropertyInfo[ ] props = type.GetProperties ( BindingFlags.Public | BindingFlags.Instance | ( BindingFlags.GetProperty & BindingFlags.SetProperty ) | ( BindingFlags.GetField & BindingFlags.SetField ) );

				string rootTypeName = Util.GetReflectorNameAttributeValue ( type );
				Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( T ) );
				XmlDocument doc = new XmlDocument ();
				XmlElement root = doc.CreateElement ( rootTypeName );

				foreach ( PropertyInfo pi in props ) {
					bool required = Util.GetCustomAttribute<RequiredAttribute> ( pi ) != null;
					bool ignore = Util.GetCustomAttribute<ReflectorIgnoreAttribute> ( pi ) != null;
					// get the version info for the property...
					Version minVer = Util.GetMinimumVersion ( pi );
					Version maxVer = Util.GetMaximumVersion ( pi );
					Version exactVer = Util.GetExactVersion ( pi );
					if ( ignore || ( !Util.IsExactVersion ( exactVer, versionInfo ) && !Util.IsInVersionRange ( minVer, maxVer, versionInfo ) ) ) {
						continue;
					}
					// get node name
					string name = Util.GetReflectorNameAttributeValue ( pi );
					ReflectorNodeTypes nodeType = Util.GetReflectorNodeType ( pi );
					XmlNode node = null;
					object val = pi.GetValue ( obj, null ); ;

					// check if it has a serializervalue attribute and get the value if it does.
					if ( pi.PropertyType.IsEnum || ( Util.IsNullable ( pi.PropertyType ) && Nullable.GetUnderlyingType ( pi.PropertyType ).IsEnum ) && val != null ) {
						Type enumType = pi.PropertyType;
						if ( Util.IsNullable ( enumType ) ) {
							enumType = Nullable.GetUnderlyingType ( enumType );
						}
						FieldInfo fi = enumType.GetField ( val.ToString () );
						SerializerValueAttribute sv = Util.GetSerializerValue ( fi );
						if ( sv != null ) {
							val = sv.Value;
						}
					}

					switch ( nodeType ) {
						case ReflectorNodeTypes.Attribute:
							node = doc.CreateAttribute ( name );
							if ( required ) {
								// checks if null or empty...
								if ( val == null || val.ToString () == string.Empty )
									throw new RequiredAttributeException ( obj, name );
								node.InnerText = Util.CheckRequired ( obj, name, val ).ToString ();
							} else {
								if ( val != null )
									node.InnerText = val.ToString ();
							}
							if ( ( Util.IsNullable ( pi.PropertyType ) && val != null ) || ( val != null && !Util.IsNullable ( pi.PropertyType ) && !string.IsNullOrEmpty ( val.ToString () ) ) )
								root.Attributes.Append ( node as XmlAttribute );
							break;
						case ReflectorNodeTypes.Element:
							node = doc.CreateElement ( name );

							if ( required ) {
								// checks if null or empty...
								if ( val == null || val.ToString () == string.Empty )
									throw new RequiredAttributeException ( obj, name );
							}

							if ( val != null ) {
								Type valType = val.GetType ();
								if ( valType.IsPrimitive || valType == typeof ( string ) ) {
									node.InnerText = val.ToString ();
								} else {
									// handle clonable lists
									if ( valType.IsGenericType && valType.GetGenericTypeDefinition ().Equals ( typeof ( CloneableList<> ) ) ) {
										System.Collections.IList vlist = val as IList;
										foreach ( object o in vlist ) {
											if ( o is ICCNetObject ) {
												XmlNode tn = ( (ICCNetObject)o ).Serialize ();
												if ( tn != null )
													node.AppendChild ( doc.ImportNode ( tn, true ) );
											} else {
												Type o1 = o.GetType ();
												if ( o1.IsSerializable && o1.IsClass ) {
													try {
														string arrayItemName = Util.GetReflectorArrayAttributeValue ( pi );

														XmlElement tn = doc.CreateElement ( arrayItemName );
														tn.InnerText = o.ToString ();
														if ( tn != null )
															node.AppendChild ( doc.ImportNode ( tn, true ) );
													} catch {
														try {
															string ss = Util.GetStringSeparatorAttributeValue ( pi );
															XmlElement ele = node as XmlElement;
															ele.InnerText += string.Format ( "{0}{1}", o.ToString (), ss );
														} catch { }
													}
												}
											}
										}
									} else if ( val is HiddenPassword ) { // handle the hidden password object
										node.InnerText = ( val as HiddenPassword ).GetPassword ();
									} else if ( valType.GetInterface ( typeof ( ICCNetObject ).FullName ) != null ) { // handle other ICCNetObjects
										XmlNode tn = ( (ICCNetObject)val ).Serialize ();
										if ( tn != null ) {
											// ignore the node that the object creates, use the one the property creates.
											// then import all the attributes and child nodes
											foreach ( XmlAttribute attr in tn.Attributes ) {
												node.Attributes.Append ( doc.ImportNode ( attr, true ) as XmlAttribute );
											}
											foreach ( XmlElement ele in tn.SelectNodes ( "./*" ) ) {
												node.AppendChild ( doc.ImportNode ( ele, true ) );
											}
										}
									} else { // eveything else
										string formatString = Util.GetFormatAttributeValue ( pi );
										if ( valType.GetInterface ( typeof ( IFormattable ).FullName ) != null && !string.IsNullOrEmpty ( formatString ) ) {
											node.InnerText = ( (IFormattable)val ).ToString ( formatString, null );
										} else {
											node.InnerText = val.ToString ();
										}
									}
								}

							}
							// if it should be added, add it...
							if ( node != null && ( ( Util.IsNullable ( pi.PropertyType ) && val != null ) || !Util.IsNullable ( pi.PropertyType ) && val != null && !string.IsNullOrEmpty ( val.ToString () ) ) )
								root.AppendChild ( node );
							break;
						case ReflectorNodeTypes.Value:
							if ( val != null )
								root.AppendChild ( doc.CreateTextNode ( val.ToString () ) );
							break;
					}
				}
				return root;
			} catch ( Exception ex ) {
				Console.WriteLine ( ex.ToString () );
				throw;
			}
		}

		/// <summary>
		/// Deserializes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="baseObject">The base object.</param>
		public void Deserialize ( XmlElement element, T baseObject ) {
			// this dynamically resets all the reflector properties only if they
			// are writeable and public
			Util.ResetObjectProperties<T> ( baseObject );
			Type type = baseObject.GetType ();

			// get the reflector name for the type
			string rootTypeName = Util.GetReflectorNameAttributeValue ( type );
			// get the version info for the type
			Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( T ) );

			// throw exception if we can not convert from the element to the type.
			if ( string.Compare ( element.Name, rootTypeName, false ) != 0 )
				throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, rootTypeName ) );

			List<PropertyInfo> props = GetReflectorProperies ( baseObject );

			foreach ( PropertyInfo pi in props ) {
				string rname = Util.GetReflectorNameAttributeValue ( pi );
				RequiredAttribute ra = Util.GetCustomAttribute<RequiredAttribute> ( pi );
				Type valType = pi.PropertyType;
				XmlNode subElement = element.SelectSingleNode ( rname ) as XmlNode;
				if ( subElement == null ) {
					subElement = element.SelectSingleNode ( string.Format ( "@{0}", rname ) );
				}
				bool required = ra != null;
				// property is required but it doesn't exist
				if ( required && subElement == null ) {
					throw new RequiredAttributeException ( baseObject, rname );
				}
				// element doesn't exist but it's not required so we can move on
				if ( subElement == null ) {
					continue;
				}

				if ( Util.IsNullable ( valType ) ) {
					Type nullType = Nullable.GetUnderlyingType ( valType );
					valType = nullType;
				}

				if ( valType.IsPrimitive || valType == typeof ( string ) ) {
					pi.SetValue ( baseObject, StringTypeConverter.Convert ( subElement.InnerText, valType ), null );
				} else {
					if ( valType.IsAssignableFrom ( typeof ( ICCNetObject ) ) ) {
						ConstructorInfo ci = valType.GetConstructor ( null );
						if ( ci == null ) {
							throw new ArgumentException ( string.Format ( "Unable to locate default constructor for type {0}", valType.Name ) );
						} else {
							ICCNetObject valObject = ci.Invoke ( null ) as ICCNetObject;
							valObject.Deserialize ( subElement as XmlElement);
						}
					} else if ( valType.IsGenericType && valType.GetGenericTypeDefinition ().Equals ( typeof ( CloneableList<> ) ) ) {
						IList vlist = pi.GetValue ( baseObject, null ) as IList;
						Type gtype = valType.GetGenericArguments ()[ 0 ];

						if ( vlist == null ) {
							// create a new instance since it's null
							ConstructorInfo ci = valType.GetConstructor ( null );
							if ( ci == null ) {
								throw new ArgumentException ( string.Format ( "Unable to locate default constructor for type {0}", valType.Name ) );
							} else {
								vlist = ci.Invoke ( null ) as IList;
							}
						}

						ReflectorArrayAttribute raa = Util.GetCustomAttribute<ReflectorArrayAttribute> ( pi );
						if ( raa != null ) {
							XmlNodeList nodes = subElement.SelectNodes ( raa.ItemName );
							foreach ( XmlElement itemElement in nodes ) {
								if ( gtype.IsPrimitive || gtype == typeof ( string ) ) {
									vlist.Add ( StringTypeConverter.Convert ( itemElement.InnerText, gtype ) );
								} else if ( gtype.IsAssignableFrom ( typeof ( ICCNetObject ) ) ) {
									ICCNetObject obj = null;
									ConstructorInfo ci = gtype.GetConstructor ( null );
									if ( ci == null ) {
										throw new ArgumentException ( string.Format ( "Unable to locate default constructor for type {0}", valType.Name ) );
									} else {
										obj = ci.Invoke ( null ) as ICCNetObject;
									}

									obj.Deserialize ( itemElement );
									vlist.Add ( obj );
								}
							}
						} else {
							// it doesn't have an reflector array attribute, so we can assume that all items are ICCNetObects.
							XmlNodeList nodes = subElement.SelectNodes ( "*" );
							foreach ( XmlElement itemElement in nodes ) {
								if ( gtype.IsAssignableFrom ( typeof ( ICCNetObject ) ) ) {
									ICCNetObject obj = null;
									ConstructorInfo ci = gtype.GetConstructor ( null );
									if ( ci == null ) {
										throw new ArgumentException ( string.Format ( "Unable to locate default constructor for type {0}", valType.Name ) );
									} else {
										obj = ci.Invoke ( null ) as ICCNetObject;
									}

									obj.Deserialize ( itemElement );
									vlist.Add ( obj );
								} else {
									throw new ArgumentException ( string.Format ( "Type {0} is not assignable from {1}", gtype.Name, typeof ( ICCNetObject ).Name ) );
								}
							}
						}
					} else if ( valType == typeof ( HiddenPassword ) ) {
						HiddenPassword hp = pi.GetValue ( baseObject, null ) as HiddenPassword;
						if ( hp == null ) {
							// need to create it.
							ConstructorInfo ci = valType.GetConstructor ( null );
							if ( ci == null ) {
								throw new ArgumentException ( string.Format ( "Unable to locate default constructor for type {0}", valType.Name ) );
							} else {
								hp = ci.Invoke ( null ) as HiddenPassword;
							}
						}
						hp.Password = subElement.InnerText;
					}
				}
			}
		}

		/// <summary>
		/// Gets a list of properties for the specified object that contain a reflector name attribute,
		/// do not have a reflector ignore attribute and are publicly readable and writeable.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public List<PropertyInfo> GetReflectorProperies ( T obj ) {
			List<PropertyInfo> props = new List<PropertyInfo> ();
			Type objType = obj.GetType ();
			PropertyInfo [] propArray = objType.GetProperties ( BindingFlags.Instance | BindingFlags.Public );
			foreach ( PropertyInfo pi in propArray ) {
				// see if the property has a reflector name attribute
				ReflectorNameAttribute rna = Util.GetCustomAttribute<ReflectorNameAttribute> ( pi );
				// make sure it doesn't have a reflector ignore attribute
				ReflectorIgnoreAttribute ria = Util.GetCustomAttribute<ReflectorIgnoreAttribute> ( pi );
				// we also need to check that both the get and set methods of the property are public.
				bool canRead = pi.CanRead && pi.GetGetMethod ( false ) != null;
				bool canWrite = pi.CanRead && pi.GetSetMethod ( false ) != null;
				if ( rna != null && ria == null && canRead && canWrite ) {
					props.Add ( pi );
				}
			}
			return props;
		}
		#endregion
	}
}
