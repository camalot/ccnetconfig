using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core.Components {
	/// <summary>
	/// Converts a string to another datatype.					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );

	/// </summary>
	public static class StringTypeConverter {
		/// <summary>
		/// Converts the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="newType">The new type.</param>
		/// <returns></returns>
		public static object Convert ( string value, Type newType ) {
		  if ( newType.IsEnum ) {
				if ( !Enum.IsDefined ( newType, value ) )
					value = Util.GetRealNameFromSerializerValue ( newType, value );
				return Enum.Parse ( newType, value );
		  } else if ( newType == typeof ( string ) ) {
				return value;
			} else if ( newType == typeof ( DateTime ) ) {
				DateTime dt;
				if ( DateTime.TryParse ( value, out dt ) )
					return dt;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else if ( newType == typeof ( bool ) ) {
				return string.Compare ( value, bool.TrueString, true ) == 0;
			} else if ( newType == typeof ( int ) ) {
				int i = 0;
				if ( int.TryParse ( value, out i ) )
					return i;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else if ( newType == typeof ( long ) ) {
				long i = 0;
				if ( long.TryParse ( value, out i ) )
					return i;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else if ( newType == typeof ( short ) ) {
				short i = 0;
				if ( short.TryParse ( value, out i ) )
					return i;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else if ( newType == typeof ( byte ) ) {
				byte i = 0;
				if ( byte.TryParse ( value, out i ) )
					return i;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else if ( newType == typeof ( char ) ) {
				char i = (char)0;
				if ( char.TryParse ( value, out i ) )
					return i;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else if ( newType == typeof ( float ) ) {
				float i = 0F;
				if ( float.TryParse ( value, out i ) )
					return i;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else if ( newType == typeof ( decimal ) ) {
				decimal i = 0M;
				if ( decimal.TryParse ( value, out i ) )
					return i;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else if ( newType == typeof ( double ) ) {
				double i = 0D;
				if ( double.TryParse ( value, out i ) )
					return i;
				else
					throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			} else {
				throw new ArgumentException ( string.Format ( "Unable to convert {0} to a {1}", value, newType.Name ) );
			}
		}
	}
}
