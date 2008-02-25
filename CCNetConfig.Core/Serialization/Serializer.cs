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
      Type type = obj.GetType ( );
      try {
        // should the version of the object be checked before any other stuff is done? 
        // no need to do anything if it doesn't event belong in here anyhow.

        // should limit to public, instance, read/write properties... 
        PropertyInfo[ ] props = type.GetProperties ( BindingFlags.Public | BindingFlags.Instance | (BindingFlags.GetProperty & BindingFlags.SetProperty) | (BindingFlags.GetField & BindingFlags.SetField) );

        string rootTypeName = Util.GetReflectorNameAttributeValue ( type );
        Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( T ) );
        XmlDocument doc = new XmlDocument ( );
        XmlElement root = doc.CreateElement ( rootTypeName );

        foreach ( PropertyInfo pi in props ) {
          bool required = Util.GetCustomAttribute<RequiredAttribute> ( pi ) != null;
          bool ignore = Util.GetCustomAttribute<ReflectorIgnoreAttribute> ( pi ) != null;
          // get the version info for the property...
          Version minVer = Util.GetMinimumVersion ( pi );
          Version maxVer = Util.GetMaximumVersion ( pi );
          Version exactVer = Util.GetExactVersion ( pi );
          if ( ignore || ( !Util.IsExactVersion ( exactVer, versionInfo ) && !Util.IsInVersionRange ( minVer, maxVer, versionInfo ) ) ) {
            Console.WriteLine ( "Ignoring : {0}", pi.Name );
            continue;
          }
          // get node name
          string name = Util.GetReflectorNameAttributeValue ( pi );
          ReflectorNodeTypes nodeType = Util.GetReflectorNodeType ( pi );
          XmlNode node = null;
          object val = pi.GetValue ( obj, null );
          switch ( nodeType ) {
            case ReflectorNodeTypes.Attribute:
              node = doc.CreateAttribute ( name );
              if ( required ) {
                // checks if null or empty...
                // CheckRequired needs to be modified to account that the type is going to alway be
                // an object. 
                if ( val == null || val.ToString ( ) == string.Empty )
                  throw new RequiredAttributeException ( obj, name );
                node.InnerText = Util.CheckRequired ( obj, name, val ).ToString ( );
              } else {
                if ( val != null )
                  node.InnerText = val.ToString ( );
              }
              if ( ( Util.IsNullable ( pi.PropertyType ) && val != null ) || !Util.IsNullable ( pi.PropertyType ) && !string.IsNullOrEmpty ( val.ToString ( ) ) )
                root.Attributes.Append ( node as XmlAttribute );
              break;
            case ReflectorNodeTypes.Element:
              node = doc.CreateElement ( name );
              if ( required ) {
                // checks if null or empty...
                // CheckRequired needs to be modified to account that the type is going to alway be
                // an object. 
                if ( val == null || val.ToString ( ) == string.Empty )
                  throw new RequiredAttributeException ( obj, name );
                node.InnerText = Util.CheckRequired ( obj, name, val ).ToString ( );
              } else {
                if ( val != null ) {
                  Type valType = val.GetType ( );
                  if ( valType.IsPrimitive ) {
                    node.InnerText = val.ToString ( );
                  } else {
                    // handle clonable lists
                    if ( valType.IsGenericType && valType.GetGenericTypeDefinition ( ).Equals ( typeof ( CloneableList<> ) ) ) {
                      foreach ( ICCNetObject o in ( System.Collections.IList ) val ) {
                        node.AppendChild ( doc.ImportNode ( new Serializer<ICCNetObject> ( ).Serialize ( o as ICCNetObject ), true ) );
                      }
                    } else if ( valType.GetInterface ( typeof ( ICCNetObject ).FullName ) != null ) { // handle other ICCNetObjects
                      node.AppendChild ( doc.ImportNode ( new Serializer<ICCNetObject> ( ).Serialize ( val as ICCNetObject ), true ) );
                    } else if ( valType == typeof ( HiddenPassword ) ) { // handle the hidden password object
                      node.InnerText = ( ( HiddenPassword ) val ).GetPassword ( );
                    } else { // eveything else
                      string formatString = Util.GetFormatAttributeValue ( pi );
                      if ( valType.GetInterface ( typeof ( IFormattable ).FullName ) != null && !string.IsNullOrEmpty ( formatString ) ) {
                        node.InnerText = ( ( IFormattable ) val ).ToString ( formatString, null );
                      } else {
                        node.InnerText = val.ToString ( );
                      }
                    }
                  }
                }
              }
              // if it should be added, add it...
              if ( ( Util.IsNullable ( pi.PropertyType ) && val != null ) || !Util.IsNullable ( pi.PropertyType ) && val != null && !string.IsNullOrEmpty ( val.ToString ( ) ) )
                root.AppendChild ( node );
              break;
            case ReflectorNodeTypes.Value :
              root.AppendChild ( doc.CreateTextNode ( val.ToString ( ) ) );
              break;
          }
        }
        return root;
      } catch ( Exception ex )  {
        Console.WriteLine ( ex.ToString ( ) );
        throw; 
      }
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="baseObject">The base object.</param>
    public void Deserialize ( System.Xml.XmlElement element, T baseObject ) {
      Type type = baseObject.GetType ( );
      // have to get the properties so we can enumerate through them and set the values.
      PropertyInfo[ ] props = type.GetProperties ( BindingFlags.Public | BindingFlags.Instance );
      string rootTypeName = Util.GetReflectorNameAttributeValue ( type );
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( T ) );
      if ( string.Compare ( element.Name, rootTypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, rootTypeName ) );

      foreach ( PropertyInfo pi in props ) {
        bool required = Util.GetCustomAttribute<RequiredAttribute> ( pi ) != null;
        bool ignore = Util.GetCustomAttribute<ReflectorIgnoreAttribute> ( pi ) != null;
        if ( ignore )
          continue;
        string name = Util.GetReflectorNameAttributeValue ( pi );
        /*
        // may be easier for now to just reset the object in its method that calls this one...
        if ( Util.IsNullable ( pi.PropertyType ) ) {
          pi.SetValue ( baseObject, null, null );
        }
        */

        Type propType = pi.PropertyType;
        if ( propType.IsPrimitive ) {

        } else {
          if ( propType.IsGenericType && propType.GetGenericTypeDefinition ( ).Equals ( typeof ( CloneableList<> ) ) ) {
            // hmmmm how to do clonablelists???
            /*
            XmlNodeList nl = element.SelectNodes ( "*" );
            foreach ( XmlElement ele in nl ) {
              
            }
            */
          } else if ( propType.GetInterface ( typeof ( ICCNetObject ).FullName ) != null ) {
            ICCNetObject obj = pi.GetValue ( baseObject, null ) as ICCNetObject;
            new Serializer<ICCNetObject> ( ).Deserialize ( element.SelectSingleNode ( name ) as XmlElement, obj );
            pi.SetValue ( baseObject, obj, null );
          } else if ( propType == typeof ( HiddenPassword ) ) {

          } else {

          }
        }

      }
    }

    #endregion
  }
}
