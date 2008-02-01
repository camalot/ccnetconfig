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
using CCNetConfig.Core.Serialization;
using System.Xml;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The FilteredSourceControl allows you to filter out modifications that are used to trigger a build. If for example, you have certain files 
  /// (such as web pages or document files) under source control that you don't want to have trigger the build, you can use this class to ensure 
  /// that their changes will keep a new build from launching.
  ///
  /// <para>The FilteredSourceControl works together with all of the source controls supported by CCNet (including the Multi Source Control Block). 
  /// It can also be included under the Multi Source Control Block provider so that you could have multiple FilterSourceControls each filtering a 
  /// different set of modifications from different source control providers. Essentially, it acts as a decorator (or an example of the pipes and 
  /// filters pattern ), wrapping around the specific SourceControl provider that you want to use.</para>
  /// 
  /// <para>The FilteredSourceControl includes both inclusion and exclusion filters for specifying what modifications should be included/excluded. 
  /// Multiple inclusion and exclusion filters can be specified or, alternately, no inclusion or exclusion filter could be specified. If a 
  /// modification is matched by both the inclusion and exclusion filter, then the exclusion filter will take preference and the modification will 
  /// not be included in the modification set. At this point, CCNet only supports three types of filters: PathFilters, UserFilters, and 
  /// ActionFilters. It is relatively straightforward to build new filters, (such as one to filter modifications based on email address).</para>
  /// </summary>
  [MinimumVersion ( "1.0" )]
  public class FilteredSourceControl : SourceControl, ICCNetDocumentation {
    private SourceControl _sourceControlProvider = null;
    private InclusionFilterGroup _inclusionFilters = null;
    private ExclusionFilterGroup _exclusionFilters = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilteredSourceControl"/> class.
    /// </summary>
    public FilteredSourceControl ( )
      : base ( "filtered" ) {
      this._exclusionFilters = new ExclusionFilterGroup ( );
      this._inclusionFilters = new InclusionFilterGroup ( );
    }

    /// <summary>
    /// This element is used to specify the type of source control provider to retrieve modifications from. With the exception of the element name, 
    /// the configuration for this element is identical to the xml configuration for the specific source control provider you intend to use.
    /// </summary>
    /// <value>The source control provider.</value>
    [Description ( "This element is used to specify the type of source control provider to retrieve modifications from. With the exception of the " +
      "element name, the configuration for this element is identical to the xml configuration for the specific source control provider you " +
      "intend to use." ), DefaultValue ( null ), DisplayName ( "(SourceControlProvider)" ), TypeConverter ( typeof ( ExpandableObjectConverter ) ),
   Editor ( typeof ( SourceControlUIEditor ), typeof ( UITypeEditor ) ), Category ( "Required" )]
    public SourceControl SourceControlProvider {
      get { return this._sourceControlProvider; }
      set { this._sourceControlProvider = Util.CheckRequired ( this, "sourceControlProvider", value ); }
    }


    /// <summary>
    /// Specifies the filters that should be used to determine which modifications should be included. This element should 
    /// contain the xml configuration for one or more filters.
    /// </summary>
    /// <value>The inclusion filters.</value>
    [Description ( "Specifies the filters that should be used to determine which modifications should be included. This element should " +
     "contain the xml configuration for one or more filters." ), DefaultValue ( null ), TypeConverter ( typeof ( ExpandableObjectConverter ) ),
    Category ( "Optional" )]
    public InclusionFilterGroup InclusionFilters {
      get { return this._inclusionFilters; }
      set { this._inclusionFilters = value; }
    }
    /// <summary>
    /// Specifies the filters that should be used to determine which modifications should be excluded. This element should 
    /// contain the xml configuration for one or more filters.
    /// </summary>
    /// <value>The exclusion filters.</value>
    [Description ( "Specifies the filters that should be used to determine which modifications should be excluded. This element should " +
      "contain the xml configuration for one or more filters." ), DefaultValue ( null ),
   TypeConverter ( typeof ( ExpandableObjectConverter ) ), Category ( "Optional" )]
    public ExclusionFilterGroup ExclusionFilters {
      get { return this._exclusionFilters; }
      set { this._exclusionFilters = value; }
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone ( ) {
      FilteredSourceControl fsc = this.MemberwiseClone ( ) as FilteredSourceControl;
      fsc.ExclusionFilters = ( ExclusionFilterGroup ) this.ExclusionFilters.Clone ( );
      fsc.InclusionFilters = ( InclusionFilterGroup ) this.InclusionFilters.Clone ( );
      fsc.SourceControlProvider = this.SourceControlProvider.Clone ( );
      return fsc;
    }

    #region Serialization

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( "sourcecontrol" );
      root.SetAttribute ( "type", this.TypeName );
      XmlElement scele = this.SourceControlProvider.Serialize ( );
      XmlElement scpele = doc.CreateElement ( "sourceControlProvider" );
      for ( int i = 0; i < scele.Attributes.Count; i++ )
        scpele.Attributes.Append ( ( XmlAttribute ) doc.ImportNode ( scele.Attributes[ i ], true ) );

      foreach ( XmlElement subEle in scele.SelectNodes ( "./*" ) )
        scpele.AppendChild ( ( XmlElement ) doc.ImportNode ( subEle, true ) );

      root.AppendChild ( scpele );
      XmlNode tnode = doc.ImportNode ( this.InclusionFilters.Serialize ( ), true );
      if ( tnode != null )
        root.AppendChild ( tnode );

      tnode = doc.ImportNode ( this.ExclusionFilters.Serialize ( ), true );
      root.AppendChild ( tnode );

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this._sourceControlProvider = null;
      this._exclusionFilters = new ExclusionFilterGroup ( );
      this._inclusionFilters = new InclusionFilterGroup ( );

      if ( string.Compare ( element.GetAttribute ( "type" ), this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.GetAttribute ( "type" ), this.TypeName ) );

      XmlElement ele = ( XmlElement ) element.SelectSingleNode ( "sourceControlProvider" );
      if ( ele != null ) {
        SourceControl sc = Util.GetSourceControlFromElement ( ele );
        if ( sc != null ) {
          this.SourceControlProvider = sc;
          this.SourceControlProvider.Deserialize ( ele );
        }
      }

      ele = ( XmlElement ) element.SelectSingleNode ( "inclusionFilters" );
      if ( ele != null )
        this.InclusionFilters.Deserialize ( ele );

      ele = ( XmlElement ) element.SelectSingleNode ( "exclusionFilters" );
      if ( ele != null )
        this.ExclusionFilters.Deserialize ( ele );
    }

    #endregion

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Filtered+Source+Control+Block?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// A base clase for the filters.
    /// </summary>
    public abstract class Filter : ICCNetObject, ISerialize, ICloneable {
      private string _typeName = string.Empty;
      /// <summary>
      /// Initializes a new instance of the <see cref="Filter"/> class.
      /// </summary>
      /// <param name="typeName">Name of the type.</param>
      public Filter ( string typeName ) {
        this._typeName = typeName;
      }

      /// <summary>
      /// Gets or sets the name of the type.
      /// </summary>
      /// <value>The name of the type.</value>
      [Browsable ( false )]
      public string TypeName { get { return _typeName; } }

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public abstract System.Xml.XmlElement Serialize ( );

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public abstract void Deserialize ( System.Xml.XmlElement element );

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString ( ) {
        return string.Empty;
      }

      #region ICloneable Members
      /// <summary>
      /// Creates a copy of this object
      /// </summary>
      /// <returns></returns>
      public Filter Clone ( ) {
        return this.MemberwiseClone ( ) as Filter;
      }

      object ICloneable.Clone ( ) {
        return this.Clone ( );
      }

      #endregion
    }

    /// <summary>
    /// A Path Filter
    /// </summary>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public class PathFilter : Filter {
      private string _pattern;
      /// <summary>
      /// Initializes a new instance of the <see cref="PathFilter"/> class.
      /// </summary>
      public PathFilter ( ) : base ( "pathFilter" ) { }

      /// <summary>
      /// This is the pattern used to compare the modification path against. The pattern should match the path of the files in the repository 
      /// (not the path of the files in the working directory). See below for examples of the syntax for this element. Each PathFilter contains a 
      /// single pattern element.
      /// </summary>
      /// <value>The pattern.</value>
      [Description ( "This is the pattern used to compare the modification path against. The pattern should match the path of the files in the repository" +
      " (not the path of the files in the working directory). See below for examples of the syntax for this element. Each PathFilter contains a single" +
      " pattern element." ), DefaultValue ( null ), DisplayName ( "(Pattern)" )]
      public string Pattern { get { return _pattern; } set { _pattern = Util.CheckRequired ( this, "pattern", value ); } }

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public override System.Xml.XmlElement Serialize ( ) {
        XmlDocument doc = new XmlDocument ( );
        XmlElement root = doc.CreateElement ( this.TypeName );
        XmlElement pattern = doc.CreateElement ( "pattern" );
        pattern.InnerText = Util.CheckRequired ( this, pattern.Name, this.Pattern );
        root.AppendChild ( pattern );
        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public override void Deserialize ( System.Xml.XmlElement element ) {
        this._pattern = string.Empty;
        if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );
        this.Pattern = Util.GetElementOrAttributeValue ( "pattern", element );
      }

      #endregion
    }

    /// <summary>
    /// A User filter
    /// </summary>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public class UserFilter : Filter {
      private List<string> _names;
      /// <summary>
      /// Initializes a new instance of the <see cref="UserFilter"/> class.
      /// </summary>
      public UserFilter ( )
        : base ( "userFilter" ) {
        this._names = new List<string> ( );
      }

      /// <summary>
      /// This element consists of multiple name elements for each username to be filtered.
      /// </summary>
      /// <value>The names.</value>
      [Description ( "This element consists of multiple name elements for each username to be filtered." ), DefaultValue ( null ),
      Editor ( typeof ( StringListUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( StringListTypeConverter ) )]
      public List<string> Names {
        get { return _names; }
        set { _names = value; }
      }

      #region Serialization

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public override XmlElement Serialize ( ) {
        XmlDocument doc = new XmlDocument ( );
        XmlElement root = doc.CreateElement ( this.TypeName );

        XmlElement namesEle = doc.CreateElement ( "names" );
        if ( this.Names != null && this.Names.Count > 0 ) {
          foreach ( string n in this.Names ) {
            XmlElement nameEle = doc.CreateElement ( "name" );
            nameEle.InnerText = n;
            namesEle.AppendChild ( nameEle );
          }
          root.AppendChild ( namesEle );
          return root;
        }
        return null;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public override void Deserialize ( XmlElement element ) {
        this.Names = new List<string> ( );
        if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );
        XmlElement ele = ( XmlElement ) element.SelectSingleNode ( "names" );
        if ( ele != null )
          foreach ( XmlElement tele in ele.SelectNodes ( "name" ) )
            this.Names.Add ( tele.InnerText );
      }

      #endregion
    }

    /// <summary>
    /// An Action Filter
    /// </summary>
    [TypeConverter ( typeof ( ExpandableObjectConverter ) )]
    public class ActionFilter : Filter {
      private List<string> _action;
      /// <summary>
      /// Initializes a new instance of the <see cref="UserFilter"/> class.
      /// </summary>
      public ActionFilter ( )
        : base ( "actionFilter" ) {
        this._action = new List<string> ( );
      }

      /// <summary>
      /// This element consists of multiple name elements for each username to be filtered.
      /// </summary>
      /// <value>The names.</value>
      [Description ( "This element consists of multiple name elements for each username to be filtered." ), DefaultValue ( null ),
      Editor ( typeof ( StringListUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( StringListTypeConverter ) )]
      public List<string> Actions {
        get { return _action; }
        set { _action = value; }
      }

      #region Serialization

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public override XmlElement Serialize ( ) {
        XmlDocument doc = new XmlDocument ( );
        XmlElement root = doc.CreateElement ( this.TypeName );

        XmlElement namesEle = doc.CreateElement ( "actions" );
        if ( this.Actions != null && this.Actions.Count > 0 ) {
          foreach ( string n in this.Actions ) {
            XmlElement nameEle = doc.CreateElement ( "action" );
            nameEle.InnerText = n;
            namesEle.AppendChild ( nameEle );
          }
          root.AppendChild ( namesEle );
          return root;
        }
        return null;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public override void Deserialize ( XmlElement element ) {
        this.Actions = new List<string> ( );
        if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );
        XmlElement ele = ( XmlElement ) element.SelectSingleNode ( "actions" );
        if ( ele != null )
          foreach ( XmlElement tele in ele.SelectNodes ( "action" ) )
            this.Actions.Add ( tele.InnerText );
      }

      #endregion
    }

    /// <summary>
    /// A group that contains a set of filters.
    /// </summary>
    public class FilterGroup : ISerialize, ICCNetObject, ICloneable {
      #region Private Members
      private string _typeName = string.Empty;
      private UserFilter _userFilter = null;
      private List<PathFilter> _pathFilter = null;
      private ActionFilter _actionFilter = null;
      #endregion

      /// <summary>
      /// Initializes a new instance of the <see cref="FilterGroup"/> class.
      /// </summary>
      /// <param name="typeName">Name of the type.</param>
      public FilterGroup ( string typeName ) {
        this._typeName = typeName;
        this._actionFilter = new ActionFilter ( );
        this._pathFilter = new List<PathFilter> ( );
        this._userFilter = new UserFilter ( );
      }

      /// <summary>
      /// Gets the name of the type.
      /// </summary>
      /// <value>The name of the type.</value>
      [Browsable ( false )]
      public string TypeName { get { return this._typeName; } }

      /// <summary>
      /// The UserFilter can be used to filter modifications on the basis of the username that committed the changes.
      /// </summary>
      /// <value>The user filter.</value>
      [Description ( "The UserFilter can be used to filter modifications on the basis of the username that committed the changes." ),
      DefaultValue ( null )]
      public UserFilter UserFilter { get { return this._userFilter; } set { this._userFilter = value; } }
      /// <summary>
      /// The PathFilter can be used to filter modifications on the basis of their file path.
      /// </summary>
      /// <value>The path filter.</value>
      [Description ( "The PathFilter can be used to filter modifications on the basis of their file path." ),
      DefaultValue ( null ), TypeConverter ( typeof ( IListTypeConverter ) )]
      public List<PathFilter> PathFilters { get { return this._pathFilter; } set { this._pathFilter = value; } }
      /// <summary>
      /// The ActionFilter can be used to filter modifications on the basis of the type of modification that was committed. Modification types are 
      /// specific to each source control provider. Consult each source control provider for the list of actions to filter.
      /// </summary>
      /// <value>The action filter.</value>
      [Description ( "The ActionFilter can be used to filter modifications on the basis of the type of modification that was committed. Modification " +
       "types are specific to each source control provider. Consult each source control provider for the list of actions to filter." ),
      DefaultValue ( null )]
      public ActionFilter ActionFilter { get { return this._actionFilter; } set { this._actionFilter = value; } }

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
        XmlDocument doc = new XmlDocument ( );
        XmlElement root = doc.CreateElement ( this.TypeName );
        if ( ( this.PathFilters == null || this.PathFilters.Count == 0 ) &&
            ( this.UserFilter == null || this.UserFilter.Names.Count == 0 ) &&
            ( this.ActionFilter == null || this.ActionFilter.Actions.Count == 0 ) )
          return null;
        
        XmlNode tnode = this.ActionFilter.Serialize ( );
        if ( tnode != null )
          root.AppendChild ( doc.ImportNode ( tnode, true) );

        tnode =this.UserFilter.Serialize ( );
        if ( tnode != null )
          root.AppendChild (  doc.ImportNode ( tnode, true ) );
        foreach ( PathFilter pfi in this.PathFilters ) {
          tnode = pfi.Serialize ( );
          if ( tnode != null )
            root.AppendChild ( doc.ImportNode ( tnode, true ) );
        }
        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this._actionFilter = new ActionFilter ( );
        this._userFilter = new UserFilter ( );
        this._pathFilter = new List<PathFilter> ( );
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
        this.PathFilters = new List<PathFilter> ( );
        PathFilter[ ] pf = new PathFilter[ this.PathFilters.Count ];
        this.PathFilters.CopyTo ( pf );
        fs.PathFilters.AddRange ( pf );
        return fs;
      }

      object ICloneable.Clone ( ) {
        return this.Clone ( );
      }

      #endregion
    }

    /// <summary>
    /// A Filter Group that will be included
    /// </summary>
    public class InclusionFilterGroup : FilterGroup {
      /// <summary>
      /// Initializes a new instance of the <see cref="InclusionFilterGroup"/> class.
      /// </summary>
      public InclusionFilterGroup ( )
        : base ( "inclusionFilters" ) {

      }
    }

    /// <summary>
    /// A Filter Group that will be excluded
    /// </summary>
    public class ExclusionFilterGroup : FilterGroup {

      /// <summary>
      /// Initializes a new instance of the <see cref="ExclusionFilterGroup"/> class.
      /// </summary>
      public ExclusionFilterGroup ( )
        : base ( "exclusionFilters" ) {

      }
    }
  }

}
