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

    /// <summary>
    /// Initializes a new instance of the <see cref="FilteredSourceControl"/> class.
    /// </summary>
    public FilteredSourceControl ( )
      : base ( "filtered" ) {
      this.ExclusionFilters = new ExclusionFilterGroup ( );
      this.InclusionFilters = new InclusionFilterGroup ( );
    }

    /// <summary>
    /// This element is used to specify the type of source control provider to retrieve modifications from. With the exception of the element name, 
    /// the configuration for this element is identical to the xml configuration for the specific source control provider you intend to use.
    /// </summary>
    /// <value>The source control provider.</value>
    [Description ( "This element is used to specify the type of source control provider to retrieve modifications from. With the exception of the " +
      "element name, the configuration for this element is identical to the xml configuration for the specific source control provider you " +
      "intend to use." ), DefaultValue ( null ), DisplayName ( "(SourceControlProvider)" ), TypeConverter ( typeof ( ExpandableObjectConverter ) ),
   Editor ( typeof ( SourceControlUIEditor ), typeof ( UITypeEditor ) ), Category ( "Required" ), Required,
    ReflectorName ( "sourceControlProvider" )]
    public SourceControl SourceControlProvider { get; set; }


    /// <summary>
    /// Specifies the filters that should be used to determine which modifications should be included. This element should 
    /// contain the xml configuration for one or more filters.
    /// </summary>
    /// <value>The inclusion filters.</value>
    [Description ( "Specifies the filters that should be used to determine which modifications should be included. This element should " +
     "contain the xml configuration for one or more filters." ), DefaultValue ( null ), TypeConverter ( typeof ( ExpandableObjectConverter ) ),
    Category ( "Optional" ), ReflectorName("inclusionFilters")]
    public InclusionFilterGroup InclusionFilters { get; set; }
    /// <summary>
    /// Specifies the filters that should be used to determine which modifications should be excluded. This element should 
    /// contain the xml configuration for one or more filters.
    /// </summary>
    /// <value>The exclusion filters.</value>
    [Description ( "Specifies the filters that should be used to determine which modifications should be excluded. This element should " +
      "contain the xml configuration for one or more filters." ), DefaultValue ( null ), ReflectorName ( "exclusionFilters" ),
    TypeConverter ( typeof ( ExpandableObjectConverter ) ), Category ( "Optional" )]
    public ExclusionFilterGroup ExclusionFilters { get; set; }

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
      // workitem : 15267
      // added check if the imported node is null before appending
      for ( int i = 0; i < scele.Attributes.Count; i++ ) {
        XmlAttribute attr = doc.ImportNode ( scele.Attributes[ i ], true ) as XmlAttribute;
        if ( attr != null )
          scpele.Attributes.Append ( attr );
      }

      foreach ( XmlElement subEle in scele.SelectNodes ( "./*" ) ) {
        XmlElement ele = doc.ImportNode ( subEle, true ) as XmlElement;
        if ( ele != null )
          scpele.AppendChild ( ele );
      }

      root.AppendChild ( scpele );
      XmlNode tnode = doc.ImportNode ( this.InclusionFilters.Serialize ( ), true );
      if ( tnode != null )
        root.AppendChild ( tnode );

      tnode = doc.ImportNode ( this.ExclusionFilters.Serialize ( ), true );
      if ( tnode != null )
        root.AppendChild ( tnode );

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.SourceControlProvider = null;
      this.ExclusionFilters = new ExclusionFilterGroup ( );
      this.InclusionFilters = new InclusionFilterGroup ( );

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
  }

}
