/*
 * Copyright (c) 2007-2008, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace CCNetConfig.Components {
  /// <summary>
  /// Provides access to copy and paste objects to and from the clipboard.
  /// </summary>
  public static class ClipboardManager {

    /// <summary>
    /// Copies specified obect to clipboard.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object to copy.</param>
    public static void CopyToClipboard<T> ( T obj ) where T : ISerialize {
      if ( obj != null ) {
        XmlElement ele = obj.Serialize ( );
        string formatedXml = FormatXml ( ele.OuterXml );
        try {
          Clipboard.SetDataObject ( formatedXml, false, 10, 100 );
        } catch  { }
      }
    }

    /// <summary>
    /// Pastes the object from clipbard.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetObjectFromClipbard<T> ( ) where T : ISerialize, new ( ) {
      T obj = new T ( );
      try {
        string strData = Clipboard.GetText ( );
        XmlElement eleObj = null;
        if ( ContainsXmlElement ( out eleObj ) ) {
          obj.Deserialize ( eleObj );
          return obj;
        } else {
          throw new CannotDeserialzeXmlException ( );
        }
      } catch {
        throw new CannotDeserialzeXmlException ( );
      }
      return default ( T );
    }

    /// <summary>
    /// Determines whether the clipboard contains a serializable object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element">The element, if return <see langword="true" />.</param>
    /// <returns>
    /// 	<see langword="true"/> if the clipboard contains a serializable object.; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ContainsXmlElement ( out XmlElement element ) {
      if ( Clipboard.ContainsText ( ) ) {
        try {
          string strData = Clipboard.GetText ( );
          XmlDocument doc = new XmlDocument ( );
          doc.LoadXml ( strData );
          element = doc.DocumentElement;
          return true;
        } catch {
          element = null;
          return false;
        }
      } else {
        element = null;
        return false;
      }
    }

    /// <summary>
    /// Determines whether the clipboard contains a serializable object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    /// 	<see langword="true"/> if the clipboard contains a serializable object; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ContainsXmlElement ( ) {
      XmlElement ele = null;
      return ContainsXmlElement ( out ele );
    }

    /// <summary>
    /// Formats the XML.
    /// </summary>
    /// <param name="unformattedXml">The unformatted XML.</param>
    /// <returns></returns>
    private static string FormatXml ( string unformattedXml ) {
      //load unformatted xml into a dom
      XmlDocument xd = new XmlDocument ( );
      xd.LoadXml ( unformattedXml );
      StringBuilder sb = new StringBuilder ( );
      StringWriter sw = new StringWriter ( sb );
      XmlTextWriter xtw = null;
      try {
        using ( sw ) {
          xtw = new XmlTextWriter ( sw );
          using ( xtw ) {
            xtw.Formatting = Formatting.Indented;
            xd.WriteTo ( xtw );
          }
        }
      } catch { }
      return sb.ToString ( );
    }
  }
}
