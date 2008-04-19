/* Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// A group that contains a set of filters.
  /// </summary>

  public class FilterGroup : ISerialize, ICCNetObject, ICloneable {
    /// <summary>
    /// Initializes a new instance of the <see cref="FilterGroup"/> class.
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    public FilterGroup ( string typeName ) {
      this.TypeName = typeName;
      this.ActionFilter = new ActionFilter ( );
      this.PathFilters = new CloneableList<PathFilter> ( );
      this.UserFilter = new UserFilter ( );
      this.CommentFilters = new CloneableList<CommentFilter> ( );
    }

    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    [Browsable ( false ), ReflectorIgnore]
    public string TypeName { get; private set; }

    /// <summary>
    /// The UserFilter can be used to filter modifications on the basis of the username that committed the changes.
    /// </summary>
    /// <value>The user filter.</value>
    [Description ( "The UserFilter can be used to filter modifications on the basis of the username that committed the changes." ),
    DefaultValue ( null ), ReflectorName ( "userFilter" )]
    public UserFilter UserFilter { get; set; }
    /// <summary>
    /// The PathFilter can be used to filter modifications on the basis of their file path.
    /// </summary>
    /// <value>The path filter.</value>
    [Description ( "The PathFilter can be used to filter modifications on the basis of their file path." ),
    DefaultValue ( null ), TypeConverter ( typeof ( IListTypeConverter ) ), ReflectorName ( "pathFilterFilter" )]
    public CloneableList<PathFilter> PathFilters { get; set; }
    /// <summary>
    /// The ActionFilter can be used to filter modifications on the basis of the type of modification that was committed. Modification types are 
    /// specific to each source control provider. Consult each source control provider for the list of actions to filter.
    /// </summary>
    /// <value>The action filter.</value>
    [Description ( "The ActionFilter can be used to filter modifications on the basis of the type of modification that was committed. Modification " +
     "types are specific to each source control provider. Consult each source control provider for the list of actions to filter." ),
    DefaultValue ( null ), ReflectorName ( "actionFilter" )]
    public ActionFilter ActionFilter { get; set; }

    [Description ( "The CommentFilter can be used to filter modifications on the basis of the comment that was supplied with the modification." ),
    DefaultValue ( null ), ReflectorName ( "commentFilter" ), TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<CommentFilter> CommentFilters { get; set; }

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString ( ) {
      return string.Empty;
    }
    #region ISerialize Members

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public XmlElement Serialize ( ) {
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( SourceControl ) );
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( this.TypeName );
      if ( ( this.PathFilters == null || this.PathFilters.Count == 0 ) &&
          ( this.UserFilter == null || this.UserFilter.Names.Count == 0 ) &&
          ( this.ActionFilter == null || this.ActionFilter.Actions.Count == 0 ) )
        return null;

      XmlNode tnode = this.ActionFilter.Serialize ( );
      if ( tnode != null )
        root.AppendChild ( doc.ImportNode ( tnode, true ) );

      tnode = this.UserFilter.Serialize ( );
      if ( tnode != null )
        root.AppendChild ( doc.ImportNode ( tnode, true ) );
      foreach ( PathFilter pfi in this.PathFilters ) {
        tnode = pfi.Serialize ( );
        if ( tnode != null )
          root.AppendChild ( doc.ImportNode ( tnode, true ) );
      }
      Version requiredVersion = new Version ( "1.3.0.3052" );

      if ( Util.IsInVersionRange ( requiredVersion, null, versionInfo ) ) {
        foreach ( CommentFilter cf in this.CommentFilters ) {
          tnode = cf.Serialize ( );
          if ( tnode != null )
            root.AppendChild ( doc.ImportNode ( tnode, true ) );
        }
      }
      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public void Deserialize ( XmlElement element ) {
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( SourceControl ) );
      this.ActionFilter = new ActionFilter ( );
      this.UserFilter = new UserFilter ( );
      this.PathFilters = new CloneableList<PathFilter> ( );
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      XmlNodeList nl = element.SelectNodes ( "pathFilter" );
      foreach ( XmlElement pfe in nl ) {
        PathFilter pf = new PathFilter ( );
        pf.Deserialize ( pfe );
        this.PathFilters.Add ( pf );
      }

      XmlElement ele = ( XmlElement ) element.SelectSingleNode ( "userFilter" );
      if ( ele != null )
        this.UserFilter.Deserialize ( ele );

      ele = ( XmlElement ) element.SelectSingleNode ( "actionFilter" );
      if ( ele != null )
        this.ActionFilter.Deserialize ( ele );

      Version requiredVersion = new Version ( "1.3.0.3052" );

      if ( Util.IsInVersionRange ( requiredVersion, null, versionInfo ) ) {
        nl = element.SelectNodes ( "commentFilter" );
        foreach ( XmlElement cfe in nl ) {
          CommentFilter cf = new CommentFilter ( );
          cf.Deserialize ( cfe );
          this.CommentFilters.Add ( cf );
        }
      }
    }

    #endregion

    #region ICloneable Members
    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public FilterGroup Clone ( ) {
      FilterGroup fs = this.MemberwiseClone ( ) as FilterGroup;
      fs.ActionFilter = this.ActionFilter.Clone ( ) as ActionFilter;
      fs.UserFilter = this.UserFilter.Clone ( ) as UserFilter;
      this.PathFilters = this.PathFilters.Clone ( );
      return fs;
    }

    object ICloneable.Clone ( ) {
      return this.Clone ( );
    }

    #endregion
  }
}
