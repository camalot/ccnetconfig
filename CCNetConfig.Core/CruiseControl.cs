/*
 * Copyright (c) 2006, Ryan Conrad. All rights reserved.
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
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using CCNetConfig.Core.Serialization;
using System.ComponentModel;
using CCNetConfig.Core.Exceptions;
using CCNetConfig.Core.Collections;

namespace CCNetConfig.Core {
  /// <summary>
  /// The Cruise Control object. All CCNet Config interaction starts with this object.
  /// </summary>
  public class CruiseControl : ISerialize, ICCNetDocumentation {
    private Version _configurationVersion = null;
    //private Dictionary<string, Project> projects;
    private ProjectList projects = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="CruiseControl"/> class.
    /// </summary>
    public CruiseControl ( )
      : this ( new Version ( "1.2" ) ) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CruiseControl"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    public CruiseControl ( Version version ) {
      //projects = new Dictionary<string, Project> ();
      projects = new ProjectList ( );
      _configurationVersion = version;
    }

    /// <summary>
    /// Gets the projects.
    /// </summary>
    /// <value>The projects.</value>
    //public Dictionary<string, Project> Projects { get { return this.projects; } }
    public ProjectList Projects { get { return this.projects; } }
    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>The version.</value>
    public Version Version { get { return this._configurationVersion; } set { this._configurationVersion = value; } }
    /// <summary>
    /// Saves the config.
    /// </summary>
    /// <param name="stream">The stream.</param>
    public void SaveConfig ( Stream stream ) {
      XmlElement ccElement = this.Serialize ( );
      XmlDocument doc = new XmlDocument ( );

      XmlComment versionComment = doc.CreateComment ( Util.CreateProjectComments ( this ).OuterXml );
      doc.AppendChild ( versionComment );

      doc.AppendChild ( doc.ImportNode ( ccElement, true ) );
      using ( stream ) {
        doc.Save ( stream );
      }
    }

    /// <summary>
    /// Determines whether this is a valid configuration.
    /// </summary>
    /// <param name="ex">The ex.</param>
    /// <returns>
    /// 	<c>true</c> if this is a valid configuration; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValidConfiguration ( out Exception ex ) {
      try {
        XmlElement ccElement = this.Serialize ( );
        if ( ccElement != null ) {
          ex = null;
          return true;
        } else {
          ex = new Exception ( "Root node missing." );
          return false;
        }
      } catch ( Exception oex ) {
        ex = oex;
        return false;
      }
    }

    #region ISerialize Members
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement ele = doc.CreateElement ( "cruisecontrol" );
      doc.AppendChild ( ele );
      foreach ( Project proj in this.projects )
        ele.AppendChild ( doc.ImportNode ( proj.Serialize ( ), true ) );

      return ele;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    void ISerialize.Deserialize ( XmlElement element ) {
      this.Deserialize ( element.OwnerDocument );
    }

    /// <summary>
    /// Deserializes the specified <see cref="System.IO.Stream">Stream</see>.
    /// </summary>
    /// <param name="strm">The Stream.</param>
    public void Deserialize ( Stream strm ) {
      if ( strm != null ) {
        XmlDocument doc = new XmlDocument ( );
        using ( strm ) {
          doc.Load ( strm );
        }
        this.Deserialize ( doc );
      }
    }

    /// <summary>
    /// Deserializes the specified <see cref="System.Xml.XmlDocument">XmlDocument</see>.
    /// </summary>
    /// <param name="ccnetConfig">The ccnet config.</param>
    /// <exception cref="CCNetConfig.Core.Exceptions.DuplicateProjectNameException">DuplicateProjectNameException</exception>
    public void Deserialize ( XmlDocument ccnetConfig ) {
      this.Projects.Clear ( );
      Version v = Util.GetConfigFileVersion ( ccnetConfig );
      if ( v != null )
        this.Version = v;
      if ( ccnetConfig.DocumentElement != null && string.Compare ( ccnetConfig.DocumentElement.Name, "cruisecontrol", false ) == 0 ) {
        foreach ( XmlElement proj in ccnetConfig.DocumentElement.SelectNodes ( "project" ) ) {
          try {
            Project p = new Project ( );
            p.Deserialize ( proj );
            if ( !this.Projects.Contains ( p.Name ) )
              this.Projects.Add ( p );
            else
              throw new DuplicateProjectNameException ( p.Name );
          } catch ( DuplicateProjectNameException ) {
            throw;
          } catch ( Exception ) {
            throw;
          }
        }
        if ( Util.UserSettings.SortProject )
          this.projects.Sort ( new ProjectList.ProjectComparer ( ) );
      } else
        throw new InvalidCastException ( string.Format ( "Can not convert {0} to a cruisecontrol", ccnetConfig.DocumentElement != null ? ccnetConfig.DocumentElement.Name : "UNKNOWN" ) );
    }

    /// <summary>
    /// Deserializes the specified <see cref="System.IO.FileInfo">file</see>.
    /// </summary>
    /// <param name="file">The file.</param>
    public void Deserialize ( FileInfo file ) {
      if ( file.Exists ) {
        XmlDocument doc = new XmlDocument ( );
        doc.Load ( file.FullName );
        Deserialize ( doc );
      } else
        throw new FileNotFoundException ( string.Format ( "The file {0} was not found.", file.FullName ) );
    }

    /// <summary>
    /// Deserializes the specified file.
    /// </summary>
    /// <param name="file">The file.</param>
    public void Deserialize ( string file ) {
      Deserialize ( new FileInfo ( file ) );
    }

    #endregion

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET?decorator=printable" ); }
    }

    #endregion
  }
}