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
using System.Xml;
using System.ComponentModel;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// <para>The File State Manager is a State Manager that saves the state for one project to a file. The filename should be stored in either the 
  /// working directory for the project or in the explicitly specified directory. The filename will match the project name, but will have the 
  /// extension <strong>.state</strong>.</para>
  /// </summary>
  [TypeConverter ( typeof ( ExpandableObjectConverter ) ),
  MinimumVersion( "1.0" )]
  public class FileStateManager : State, ICCNetDocumentation {
    /// <summary>
    /// Initializes a new instance of the <see cref="FileStateManager"/> class.
    /// </summary>
    public FileStateManager () {

    }

    /// <summary>
    /// Creates a copy of the FileStateManager
    /// </summary>
    /// <returns></returns>
    public override State Clone () {
      return this.MemberwiseClone () as FileStateManager;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( "state" );
      root.SetAttribute ( "type", this.Type );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      if ( !string.IsNullOrEmpty(this.Directory) )
        root.SetAttribute ( "directory", this.Directory );

      return root;
    }

    /// <summary>
    /// Toes the string.
    /// </summary>
    /// <returns></returns>
    public override string ToString () {
      return this.GetType().Name;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/File+State+Manager?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.Directory = string.Empty;

      if ( string.Compare (element.GetAttribute ("type"), this.Type, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute("type"), this.Type));

      this.Directory = Util.GetElementOrAttributeValue ( "directory", element );
    }
  }
}
