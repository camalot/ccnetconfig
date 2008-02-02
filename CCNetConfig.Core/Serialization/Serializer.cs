using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core.Serialization {
  /// <summary>
  /// Provides serialization and deserialation of ICCNetObjects
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class Serializer<T> where T : ICCNetObject {
    #region ISerialize Members

    public System.Xml.XmlElement Serialize ( T obj ) {
      Type type = obj.GetType ( );
      PropertyInfo[ ] props = type.GetProperties ( BindingFlags.Public | System.Reflection.BindingFlags.Instance );
      try {
        string rootTypeName = Util.GetReflectorNameAttributeValue ( type );
        Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( T ) );
        XmlDocument doc = new XmlDocument ( );
        XmlElement root = doc.CreateElement ( rootTypeName );

        foreach ( PropertyInfo pi in props ) {
          bool required = Util.GetCustomAttribute<RequiredAttribute> ( pi ) != null;
          bool ignore = Util.GetCustomAttribute<ReflectorIgnoreAttribute> ( pi ) != null;
          if ( ignore )
            continue;
          string name = Util.GetReflectorNameAttributeValue ( pi );
          ReflectorNodeTypes nodeType = Util.GetReflectorNodeType ( pi );
          XmlNode node = null;
          Console.WriteLine ( name );
          object val = pi.GetValue ( obj, null );
          switch ( nodeType ) {
            case ReflectorNodeTypes.Attribute:
              node = doc.CreateAttribute ( name );
              if ( required ) {
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
                node.InnerText = Util.CheckRequired ( obj, name, val ).ToString ( );
              } else {
                if ( val != null ) {
                  Type valType = val.GetType ( );
                  if ( valType.IsPrimitive ) {
                    Console.WriteLine ( "{0} is Primitive", name );
                    node.InnerText = val.ToString ( );
                  } else {
                    if ( valType.IsGenericType && valType.GetGenericTypeDefinition ( ).Equals ( typeof ( CloneableList<> ) ) ) {
                      foreach ( ICCNetObject o in ( System.Collections.IList ) val ) {
                        Console.WriteLine ( "o = {0}", o.ToString ( ) );
                        node.AppendChild ( doc.ImportNode ( new Serializer<ICCNetObject> ( ).Serialize ( o as ICCNetObject ), true ) );
                      }
                    } else if ( valType.GetInterface ( typeof ( ICCNetObject ).FullName ) != null ) {
                      node.AppendChild ( doc.ImportNode ( new Serializer<ICCNetObject> ( ).Serialize ( val as ICCNetObject ), true ) );
                    } else if ( valType == typeof ( HiddenPassword ) ) {
                      node.InnerText = ( ( HiddenPassword ) val ).GetPassword ( );
                    } else {
                      if ( val != null )
                        node.InnerText = val.ToString ( );
                    }
                  }
                }
              }
              if ( ( Util.IsNullable ( pi.PropertyType ) && val != null || !Util.IsNullable ( pi.PropertyType ) && !string.IsNullOrEmpty ( val.ToString ( ) ) ) )
                root.AppendChild ( node );
              break;
          }
        }
        return root;
      } catch { throw; }
    }

    public T Deserialize ( System.Xml.XmlElement element ) {
      throw new NotImplementedException ( );
    }

    #endregion
  }
}
