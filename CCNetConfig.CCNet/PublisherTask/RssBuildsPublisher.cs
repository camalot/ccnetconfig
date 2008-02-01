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
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using CCNetConfig.Core.Enums;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using CCNetConfig.Core.Serialization;
using System.ComponentModel.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// A publisher that taskes successful builds and generates a rolling rss feed.
  /// </summary>
  [Plugin (), MinimumVersion ( "1.2.1" )]
  public class RssBuildsPublisher : PublisherTask, ICCNetDocumentation {
    

    private string _outputPath = string.Empty;
    private string _encoding = string.Empty;
    private int? _maxHistory = null;
    private string _urlFormat = string.Empty;
    private string _fileName = string.Empty;
    private string _enclosureUrlFormat = string.Empty;
    private bool? _addEnclosure = null;
    private PublishBuildCondition? _buildCondition = null;
    private string _channelUrlFormat = string.Empty;
    private CloneableList<RssElement> _feedElements = null;
    private CloneableList<RssElement> _itemElements = null;
    private CloneableList<Namespace> _namespaces = null;
    private FeedImage _feedImage = null;
    private string _feedTitleFormat = string.Empty;
    private string _feedDescriptionFormat = string.Empty;
    private string _descriptionHeader = string.Empty;
    private string _descriptionFooter = string.Empty;
    private CloneableList<Category> _categories = null;
    private CloneableList<PingItem> _pingItems = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="RssBuildsPublisher"/> class.
    /// </summary>
    public RssBuildsPublisher ()
      : base ( "rssBuilds" ) {
      _feedElements = new CloneableList<RssElement> ();
      _itemElements = new CloneableList<RssElement> ();
      _namespaces = new CloneableList<Namespace> ();
      _feedImage = new FeedImage ();
      _categories = new CloneableList<Category> ();
      _pingItems = new CloneableList<PingItem> ();
    }

    /// <summary>
    /// Gets or sets the output path.
    /// </summary>
    /// <value>The output path.</value>
    [BrowseForFolderDescription ( "Select output path." ), Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
   DefaultValue ( null ), Category ( "Optional" ),
   Description ( "The path to output the rss file. This can be an absolute path or a relative path based on the artifact directory" )]
    public string OutputPath { get { return this._outputPath; } set { this._outputPath = value; } }

    /// <summary>
    /// Gets or sets the encoding.
    /// </summary>
    /// <value>The encoding.</value>
    [Description ( "The encoding used in the rss document. The default is \"UTF-8\"" ), DefaultValue ( null ), Category ( "Optional" )]
    public string Encoding {
      get { return this._encoding; }
      set {
        if ( string.IsNullOrEmpty ( value ) ) {
          this._encoding = value;
          return;
        } else {
          Encoding encoding = System.Text.Encoding.GetEncoding ( value );
          if ( encoding != null )
            this._encoding = encoding.WebName;
          else
            throw new ArgumentException ( "Encoding not found." );
        }
      }
    }



    /// <summary>
    /// Gets or sets the channel URL format.
    /// </summary>
    /// <value>The channel URL format.</value>
    [Description ( "The format string used to create the url for the channel." ),
   DefaultValue ( null ), Category ( "Optional" )]
    public string ChannelUrl {
      get { return this._channelUrlFormat; }
      set { this._channelUrlFormat = value; }
    }

    /// <summary>
    /// Gets or sets the max history.
    /// </summary>
    /// <value>The max history.</value>
    [Description ( "The max number of items to keep in the main feed, older items will be moved to the history feed. The default is 25" ),
 DefaultValue ( null ), Category ( "Optional" )]
    public int? MaxHistory {
      get { return this._maxHistory; }
      set { this._maxHistory = value > 0 ? value : 25; }
    }

    /// <summary>
    /// Gets or sets the URL format.
    /// </summary>
    /// <value>The URL format.</value>
    [Description ( "The format string used to create the url for each item." ),
 DefaultValue ( null ), Category ( "Optional" )]
    public string ItemUrl {
      get { return this._urlFormat; }
      set { this._urlFormat = value; }
    }

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    /// <value>The name of the file.</value>
    [Description ( "The file name (without the extension)" ), DefaultValue ( null ), Category ( "Optional" )]
    public string FileName { get { return this._fileName; } set { this._fileName = value; } }

    /// <summary>
    /// Gets or sets the enclosure URL format.
    /// </summary>
    /// <value>The enclosure URL format.</value>
    [Description ( "This is the format string used to generate the enclosure url." ),
 DefaultValue ( null ), Category ( "Optional" )]
    public string EnclosureUrl {
      get { return this._enclosureUrlFormat; }
      set { this._enclosureUrlFormat = value; }
    }

    /// <summary>
    /// Gets or sets the add enclosure.
    /// </summary>
    /// <value>The add enclosure.</value>
    [Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
 Description ( "If true, then an enclosure will be added to the feed using the EnclosureUrl" ), DefaultValue ( null ), Category ( "Optional" )]
    public bool? AddEnclosure {
      get { return this._addEnclosure; }
      set { this._addEnclosure = value; }
    }

    /// <summary>
    /// Gets or sets the build condition.
    /// </summary>
    /// <value>The build condition.</value>
    [Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ), TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ),
 Description ( "The build condition in which the info should be added to the feed." ), DefaultValue ( null ), Category ( "Optional" )]
    public PublishBuildCondition? BuildCondition {
      get { return this._buildCondition; }
      set { this._buildCondition = value; }
    }

    /// <summary>
    /// Gets or sets the feed elements.
    /// </summary>
    /// <value>The feed elements.</value>
    [Category ( "Optional" ), Description ( "A collection of elements that are added to the feed channel." ), DefaultValue ( null ),
   TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<RssElement> FeedElements {
      get { return this._feedElements; }
      set { this._feedElements = value; }
    }

    /// <summary>
    /// Gets or sets the item elements.
    /// </summary>
    /// <value>The item elements.</value>
    [Category ( "Optional" ), Description ( "A collection of elements that are added to the feed items." ), DefaultValue ( null ),
    TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<RssElement> ItemElements {
      get { return this._itemElements; }
      set { this._itemElements = value; }
    }

    /// <summary>
    /// Gets or sets the ping items.
    /// </summary>
    /// <value>The ping items.</value>
    [Category ( "Optional" ), Description ( "A collection of hosts to notify when the feed is updated." ), DefaultValue ( null ),
    TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<PingItem> PingItems {
      get { return this._pingItems; }
      set { this._pingItems = value; }
    }

    /// <summary>
    /// Gets or sets the namespaces.
    /// </summary>
    /// <value>The namespaces.</value>
    [Category ( "Optional" ), Description ( "A collection of namespaces that are added to the feed." ), DefaultValue ( null ),
    TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<Namespace> Namespaces {
      get { return this._namespaces; }
      set { this._namespaces = value; }
    }

    /// <summary>
    /// Gets or sets the categories.
    /// </summary>
    /// <value>The categories.</value>
    [Category ( "Optional" ), Description ( "A collection of categories that are added to each item." ), DefaultValue ( null ),
TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<Category> Categories {
      get { return this._categories; }
      set { this._categories = value; }
    }

    /// <summary>
    /// Gets or sets the feed image item.
    /// </summary>
    /// <value>The feed image item.</value>
    [Category ( "Optional" ), Description ( "Represents the image that is associated with the feed.\nTo not use a feed image set all of the value to empty." ), DefaultValue ( null ),
    TypeConverter ( typeof ( ExpandableObjectConverter ) ), DisplayName ( "FeedImage" )]
    public FeedImage FeedImageItem {
      get { return this._feedImage; }
      set { this._feedImage = value; }
    }

    /// <summary>
    /// Gets or sets the description footer.
    /// </summary>
    /// <value>The description footer.</value>
    [Category ( "Optional" ),
    Editor ( typeof ( MultilineStringUIEditor ), typeof ( UITypeEditor ) ),
    Description ( "String that is used to build the item description. This is added to the description after the modification comments." )]
    public string DescriptionFooter {
      get { return this._descriptionFooter; }
      set { this._descriptionFooter = value; }
    }

    /// <summary>
    /// Gets or sets the description header.
    /// </summary>
    /// <value>The description footer.</value>
    [Category ( "Optional" ),
   Editor ( typeof ( MultilineStringUIEditor ), typeof ( UITypeEditor ) ),
    Description ( "String that is used to build the item description. This is added to the description before the modification comments." )]
    public string DescriptionHeader {
      get { return this._descriptionHeader; }
      set { this._descriptionHeader = value; }
    }

    /// <summary>
    /// Gets or sets the feed title format.
    /// </summary>
    /// <value>The feed title format.</value>
    [Category ( "Optional" ), Description ( "Format use to create the feed title.\n{0} = Project Name\n{1} = Status\n{2} = BuildCondition" )]
    public string FeedTitle {
      get { return this._feedTitleFormat; }
      set { this._feedTitleFormat = value; }
    }

    /// <summary>
    /// Gets or sets the feed description format.
    /// </summary>
    /// <value>The feed description format.</value>
    [Category ( "Optional" ), Description ( "Format use to create the feed description.\n{0} = Project Name\n{1} = Status\n{2} = BuildCondition" )]
    public string FeedDescription {
      get { return this._feedDescriptionFormat; }
      set { this._feedDescriptionFormat = value; }
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://www.codeplex.com/rssbuildspublisher/" ); }
    }

    #endregion

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));
      if ( !string.IsNullOrEmpty ( this.OutputPath ) ) {
        XmlElement ele = doc.CreateElement ( "outputPath" );
        ele.InnerText = this.OutputPath;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.Encoding ) ) {
        XmlElement ele = doc.CreateElement ( "encoding" );
        ele.InnerText = this.Encoding;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.EnclosureUrl ) ) {
        XmlElement ele = doc.CreateElement ( "enclosureUrl" );
        ele.InnerText = this.EnclosureUrl;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.FileName ) ) {
        XmlElement ele = doc.CreateElement ( "fileName" );
        ele.InnerText = this.FileName;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.ChannelUrl ) ) {
        XmlElement ele = doc.CreateElement ( "channelUrl" );
        ele.InnerText = this.ChannelUrl;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.DescriptionHeader ) ) {
        XmlElement ele = doc.CreateElement ( "descriptionHeader" );
        ele.AppendChild ( doc.CreateCDataSection ( this.DescriptionHeader ) );
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.DescriptionFooter ) ) {
        XmlElement ele = doc.CreateElement ( "descriptionFooter" );
        ele.AppendChild ( doc.CreateCDataSection ( this.DescriptionFooter ) );
        root.AppendChild ( ele );
      }

      if ( this.MaxHistory.HasValue ) {
        XmlElement ele = doc.CreateElement ( "maxHistory" );
        ele.InnerText = this.MaxHistory.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.AddEnclosure.HasValue ) {
        XmlElement ele = doc.CreateElement ( "addEnclosure" );
        ele.InnerText = this.AddEnclosure.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.ItemUrl ) ) {
        XmlElement ele = doc.CreateElement ( "itemUrl" );
        ele.InnerText = this.ItemUrl;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.FeedTitle ) ) {
        XmlElement ele = doc.CreateElement ( "feedTitle" );
        ele.InnerText = this.FeedTitle;
        root.AppendChild ( ele );
      }

      if ( !string.IsNullOrEmpty ( this.FeedDescription ) ) {
        XmlElement ele = doc.CreateElement ( "feedDescription" );
        ele.InnerText = this.FeedDescription;
        root.AppendChild ( ele );
      }

      if ( this.BuildCondition.HasValue ) {
        XmlElement ele = doc.CreateElement ( "buildCondition" );
        ele.InnerText = this.BuildCondition.Value.ToString ();
        root.AppendChild ( ele );
      }

      if ( this.PingItems != null && this.PingItems.Count > 0 ) {
        XmlElement ele = doc.CreateElement ( "pingItems" );
        foreach( PingItem pi in this.PingItems )
          ele.AppendChild ( doc.ImportNode ( pi.Serialize (), true ) );
        root.AppendChild ( ele );
      }

      if ( this.FeedElements != null && this.FeedElements.Count > 0 ) {
        XmlElement ele = doc.CreateElement ( "feedElements" );
        foreach ( RssElement element in this.FeedElements )
          ele.AppendChild ( doc.ImportNode ( element.Serialize (), true ) );
        root.AppendChild ( ele );
      }

      if ( this.ItemElements != null && this.ItemElements.Count > 0 ) {
        XmlElement ele = doc.CreateElement ( "itemElements" );
        foreach ( RssElement element in this.ItemElements )
          ele.AppendChild ( doc.ImportNode ( element.Serialize (), true ) );
        root.AppendChild ( ele );
      }

      if ( this.Namespaces != null && this.Namespaces.Count > 0 ) {
        XmlElement ele = doc.CreateElement ( "rssExtensions" );
        foreach ( Namespace ns in this.Namespaces )
          ele.AppendChild ( doc.ImportNode ( ns.Serialize (), true ) );
        root.AppendChild ( ele );
      }

      if ( this.Categories != null && this.Categories.Count > 0 ) {
        XmlElement ele = doc.CreateElement ( "categories" );
        foreach ( Category cat in this.Categories )
          ele.AppendChild ( doc.ImportNode ( cat.Serialize (), true ) );
        root.AppendChild ( ele );
      }

      XmlElement eleFI = this.FeedImageItem.Serialize ();
      if ( eleFI != null )
        root.AppendChild ( doc.ImportNode ( eleFI, true ) );

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.OutputPath = string.Empty;
      this.FileName = string.Empty;
      this.AddEnclosure = null;
      this.BuildCondition = null;
      this.EnclosureUrl = string.Empty;
      this.Encoding = string.Empty;
      this.MaxHistory = null;
      this.ItemUrl = string.Empty;
      this.ChannelUrl = string.Empty;
      this.ItemElements.Clear ();
      this.FeedElements.Clear ();
      this.Namespaces.Clear ();
      this.FeedImageItem = new FeedImage ();
      this.FeedDescription = string.Empty;
      this.FeedTitle = string.Empty;
      this.DescriptionFooter = string.Empty;
      this.DescriptionHeader = string.Empty;
      this.Categories.Clear ();
      this.PingItems.Clear ();

      string s = Util.GetElementOrAttributeValue ( "outputPath", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.OutputPath = s;

      s = Util.GetElementOrAttributeValue ( "fileName", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.FileName = s;

      s = Util.GetElementOrAttributeValue ( "feedTitle", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.FeedTitle = s;

      s = Util.GetElementOrAttributeValue ( "feedDescription", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.FeedDescription = s;

      s = Util.GetElementOrAttributeValue ( "channelUrl", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ChannelUrl = s;

      s = Util.GetElementOrAttributeValue ( "descriptionHeader", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.DescriptionHeader = s;

      s = Util.GetElementOrAttributeValue ( "descriptionFooter", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.DescriptionFooter = s;

      s = Util.GetElementOrAttributeValue ( "addEnclosure", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.AddEnclosure = string.Compare ( s, bool.TrueString, true ) == 0;


      s = Util.GetElementOrAttributeValue ( "buildCondition", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.BuildCondition = (PublishBuildCondition)Enum.Parse ( typeof ( PublishBuildCondition ), s, true );

      s = Util.GetElementOrAttributeValue ( "enclosureUrl", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.EnclosureUrl = s;

      s = Util.GetElementOrAttributeValue ( "encoding", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Encoding = s;

      s = Util.GetElementOrAttributeValue ( "maxHistory", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int val = 25;
        int.TryParse ( s, out val );
        this.MaxHistory = val;
      }

      s = Util.GetElementOrAttributeValue ( "itemUrl", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ItemUrl = s;

      XmlElement cele = element.SelectSingleNode ( "categories" ) as XmlElement;
      if ( cele != null ) {
        foreach ( XmlElement re in cele.SelectNodes ( "category" ) ) {
          Category c = new Category ();
          c.Deserialize ( re );
          this.Categories.Add ( c );
        }
      }

      XmlElement fele = element.SelectSingleNode ( "feedElements" ) as XmlElement;
      if ( fele != null ) {
        foreach ( XmlElement re in fele.SelectNodes ( "rssElement" ) ) {
          RssElement f = new RssElement ();
          f.Deserialize ( re );
          this.FeedElements.Add ( f );
        }
      }

      XmlElement iele = element.SelectSingleNode ( "itemElements" ) as XmlElement;
      if ( iele != null ) {
        foreach ( XmlElement re in iele.SelectNodes ( "rssElement" ) ) {
          RssElement i = new RssElement ();
          i.Deserialize ( re );
          this.ItemElements.Add ( i );
        }
      }

      XmlElement pele = element.SelectSingleNode ( "pingItems" ) as XmlElement;
      if ( pele != null ) {
        foreach ( XmlElement pe in pele.SelectNodes ( "pingItem" ) ) {
          PingItem pi = new PingItem ();
          pi.Deserialize ( pe );
          this.PingItems.Add ( pi );
        }
      }

      XmlElement nele = element.SelectSingleNode ( "rssExtensions" ) as XmlElement;
      if ( nele != null ) {
        foreach ( XmlElement ns in nele.SelectNodes ( "namespace" ) ) {
          Namespace n = new Namespace ();
          n.Deserialize ( ns );
          this.Namespaces.Add ( n );
        }
      }

      XmlElement fiEle = element.SelectSingleNode ( "feedImage" ) as XmlElement;
      if ( fiEle != null )
        this.FeedImageItem.Deserialize ( fiEle );
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone () {
      return this.MemberwiseClone () as RssBuildsPublisher;
    }

    /// <summary>
    /// Represents an XmlElement that is added to the XmlDocument.
    /// </summary>
    public class RssElement : ISerialize, ICCNetObject, ICCNetDocumentation, ICloneable {
      private string _prefix = string.Empty;
      private string _name = string.Empty;
      private string _value = string.Empty;
      private CloneableList<RssElementAttribute> _attributes = null;
      private CloneableList<RssElement> _subElements = null;

      /// <summary>
      /// Initializes a new instance of the <see cref="RssElement"/> class.
      /// </summary>
      public RssElement () {
        _subElements = new CloneableList<RssElement> ();
        _attributes = new CloneableList<RssElementAttribute> ();
      }

      /// <summary>
      /// Gets or sets the prefix.
      /// </summary>
      /// <value>The prefix.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "The prefix to use for the element. This prefix MUST exist in the namespaces defined in the rss extensions" )]
      public string Prefix {
        get { return this._prefix; }
        set { this._prefix = value; }
      }

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      [Category ( "Required" ), DisplayName ( "(Name)" ), DefaultValue ( null ),
     Description ( "The name of the element." )]
      public string Name {
        get { return this._name; }
        set { this._name = Util.CheckRequired ( this, "name", value ); }
      }

      /// <summary>
      /// Gets or sets the value.
      /// </summary>
      /// <value>The value.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "The value of the element." ),
     Editor ( typeof ( MultilineStringEditor ), typeof ( UITypeEditor ) )]
      public string Value {
        get { return this._value; }
        set { this._value = value; }
      }

      /// <summary>
      /// Gets or sets the child elements.
      /// </summary>
      /// <value>The child elements.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
     Description ( "Child nodes to add to this element." ),
     TypeConverter ( typeof ( IListTypeConverter ) )]
      public CloneableList<RssElement> ChildElements {
        get { return this._subElements; }
        set { this._subElements = value; }
      }

      /// <summary>
      /// Gets or sets the attributes.
      /// </summary>
      /// <value>The attributes.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
     Description ( "Attributes to add to this element." ),
     TypeConverter ( typeof ( IListTypeConverter ) )]
      public CloneableList<RssElementAttribute> Attributes {
        get { return this._attributes; }
        set { this._attributes = value; }
      }
      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( "rssElement" );

        root.SetAttribute ( "name", Util.CheckRequired ( this, "name", this.Name ) );
        if ( !string.IsNullOrEmpty ( this.Prefix ) )
          root.SetAttribute ( "prefix", this.Prefix );

        if ( !string.IsNullOrEmpty ( this.Value ) && this.ChildElements.Count == 0 )
          root.SetAttribute ( "value", this.Value );
        else {
          if ( this.ChildElements.Count > 0 ) {
            XmlElement childEle = doc.CreateElement ( "childElements" );
            foreach ( RssElement child in this.ChildElements )
              childEle.AppendChild ( doc.ImportNode ( child.Serialize (), true ) );
            root.AppendChild ( childEle );
          }
        }
        if ( this.Attributes.Count > 0 ) {
          XmlElement attrEle = doc.CreateElement ( "attributes" );
          foreach ( RssElementAttribute rea in this.Attributes )
            attrEle.AppendChild ( doc.ImportNode ( rea.Serialize (), true ) );
          root.AppendChild ( attrEle );
        }
        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this.Prefix = string.Empty;
        this._name = string.Empty;
        this.Value = string.Empty;
        this.ChildElements.Clear ();
        this.Attributes.Clear ();
        if ( string.Compare ( element.Name, "rssElement", false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to an {1}", element.Name, this.GetType ().Name ) );
        string s = Util.GetElementOrAttributeValue ( "name", element );
        this.Name = s;

        XmlNodeList subNodes = element.SelectNodes ( string.Format ( "childElements/{0}", element.Name ) );
        foreach ( XmlElement ele in subNodes ) {
          RssElement rsse = new RssElement ();
          rsse.Deserialize ( ele );
          this.ChildElements.Add ( rsse );
        }

        if ( this.ChildElements.Count == 0 ) {
          s = Util.GetElementOrAttributeValue ( "value", element );
          this.Value = s;
        }

        s = Util.GetElementOrAttributeValue ( "prefix", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Prefix = s;

        subNodes = element.SelectNodes ( "attributes/attribute" );
        foreach ( XmlElement ele in subNodes ) {
          RssElementAttribute rea = new RssElementAttribute ();
          rea.Deserialize ( ele );
          this.Attributes.Add ( rea );
        }
      }

      #endregion

      #region ICCNetDocumentation Members
      /// <summary>
      /// Gets the documentation URI.
      /// </summary>
      /// <value>The documentation URI.</value>
      [Browsable ( false )]
      public Uri DocumentationUri {
        get { return new Uri ( "http://www.codeplex.com/rssbuildspublisher/" ); }
      }

      #endregion

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      public RssElement Clone () {
        RssElement re = this.MemberwiseClone () as RssElement;
        re.ChildElements = this.ChildElements.Clone ();
        re.Attributes = this.Attributes.Clone ();
        return re;
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return this.Name;
      }

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      object ICloneable.Clone () {
        return this.Clone ();
      }

      #endregion
    }

    /// <summary>
    /// Represents an attribtue for an xml element
    /// </summary>
    public class RssElementAttribute : ISerialize, ICCNetObject, ICCNetDocumentation, ICloneable {
      private string _prefix = string.Empty;
      private string _name = string.Empty;
      private string _value = string.Empty;

      /// <summary>
      /// Gets or sets the prefix.
      /// </summary>
      /// <value>The prefix.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "The prefix to use for the element. This prefix MUST exist in the namespaces defined in the rss extensions" )]
      public string Prefix {
        get { return this._prefix; }
        set { this._prefix = value; }
      }

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      [Category ( "Required" ), DisplayName ( "(Name)" ), DefaultValue ( null ),
     Description ( "The name of the element." )]
      public string Name {
        get { return this._name; }
        set { this._name = Util.CheckRequired ( this, "name", value ); }
      }

      /// <summary>
      /// Gets or sets the value.
      /// </summary>
      /// <value>The value.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "The value of the element." )]
      public string Value {
        get { return this._value; }
        set { this._value = value; }
      }

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( "attribute" );

        root.SetAttribute ( "name", Util.CheckRequired ( this, "name", this.Name ) );
        if ( !string.IsNullOrEmpty ( this.Prefix ) )
          root.SetAttribute ( "prefix", this.Prefix );

        if ( !string.IsNullOrEmpty ( this.Value ) )
          root.SetAttribute ( "value", this.Value );

        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this.Prefix = string.Empty;
        this._name = string.Empty;
        this.Value = string.Empty;
        if ( string.Compare ( element.Name, "attribute", false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.GetType ().Name ) );
        string s = Util.GetElementOrAttributeValue ( "name", element );
        this.Name = s;

        s = Util.GetElementOrAttributeValue ( "value", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Value = s;

        s = Util.GetElementOrAttributeValue ( "prefix", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Prefix = s;
      }

      #endregion

      #region ICCNetDocumentation Members

      /// <summary>
      /// Gets the documentation URI.
      /// </summary>
      /// <value>The documentation URI.</value>
      [Browsable ( false )]
      public Uri DocumentationUri {
        get { return new Uri ( "http://www.codeplex.com/rssbuildspublisher/" ); }
      }


      #endregion

      #region ICloneable Members

      /// <summary>
      /// Clones this instance.
      /// </summary>
      /// <returns></returns>
      public RssElementAttribute Clone () {
        return this.MemberwiseClone () as RssElementAttribute;
      }

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      object ICloneable.Clone () {
        return this.Clone ();
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return this.Name;
      }
    }

    /// <summary>
    /// Represents the images that is associated with the feed.
    /// </summary>
    public class FeedImage : ISerialize, ICCNetObject, ICCNetDocumentation, ICloneable {
      private Uri _image = null;
      private string _title = string.Empty;
      private Uri _link = null;

      /// <summary>
      /// Gets or sets the image.
      /// </summary>
      /// <value>The image.</value>
      [Category ( "Required" ), DisplayName ( "(Image)" ), DefaultValue ( null ),
  Description ( "The source of the image." )]
      public Uri Image {
        get { return this._image; }
        set { this._image = Util.CheckRequired ( this, "image", value ); }
      }

      /// <summary>
      /// Gets or sets the title.
      /// </summary>
      /// <value>The title.</value>
      [Category ( "Required" ), DisplayName ( "(Title)" ), DefaultValue ( null ),
  Description ( "The title attribute applied to the image." )]
      public string Title {
        get { return this._title; }
        set { this._title = Util.CheckRequired ( this, "title", value ); }
      }

      /// <summary>
      /// Gets or sets the link.
      /// </summary>
      /// <value>The link.</value>
      [Category ( "Required" ), DisplayName ( "(Link)" ), DefaultValue ( null ),
  Description ( "The url to navigate a user to when clicked." )]
      public Uri Link {
        get { return this._link; }
        set { this._link = Util.CheckRequired ( this, "link", value ); }
      }

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        if ( _link != null && _image != null && !string.IsNullOrEmpty ( _title ) ) {
          XmlElement root = doc.CreateElement ( "feedImage" );

          root.SetAttribute ( "link", Util.CheckRequired ( this, "link", this.Link ).ToString () );
          root.SetAttribute ( "title", Util.CheckRequired ( this, "title", this.Title ) );
          root.SetAttribute ( "url", Util.CheckRequired ( this, "url", this.Image ).ToString () );

          return root;
        } else
          return null;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this._image = null;
        this._link = null;
        this._title = string.Empty;

        if ( string.Compare ( element.Name, "feedImage", false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.GetType ().Name ) );
        string s = Util.GetElementOrAttributeValue ( "url", element );
        this.Image = new Uri ( s );

        s = Util.GetElementOrAttributeValue ( "link", element );
        this.Link = new Uri ( s );

        s = Util.GetElementOrAttributeValue ( "title", element );
        this.Title = s;
      }

      #endregion

      #region ICCNetDocumentation Members

      /// <summary>
      /// Gets the documentation URI.
      /// </summary>
      /// <value>The documentation URI.</value>
      [Browsable ( false )]
      public Uri DocumentationUri {
        get { return new Uri ( "http://www.codeplex.com/rssbuildspublisher/" ); }
      }

      #endregion

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      public FeedImage Clone () {
        return this.MemberwiseClone () as FeedImage;
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return string.Format ( "({0})", this.Image == null && this.Link == null && string.IsNullOrEmpty ( this.Title ) ? "null" : this.GetType ().Name );
      }

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      object ICloneable.Clone () {
        return this.Clone ();
      }

      #endregion
    }

    /// <summary>
    /// Represents a namespace that is added to the feed.
    /// </summary>
    public class Namespace : ISerialize, ICCNetObject, ICCNetDocumentation, ICloneable {
      private string _prefix = string.Empty;
      private Uri _uri = null;

      /// <summary>
      /// Initializes a new instance of the <see cref="Namespace"/> class.
      /// </summary>
      public Namespace () {

      }


      /// <summary>
      /// Gets or sets the prefix.
      /// </summary>
      /// <value>The prefix.</value>
      public string Prefix {
        get { return this._prefix; }
        set { this._prefix = Util.CheckRequired ( this, "prefix", value ); }
      }
      /// <summary>
      /// Gets or sets the namespace URI.
      /// </summary>
      /// <value>The namespace URI.</value>
      public Uri NamespaceUri {
        get { return this._uri; }
        set { this._uri = Util.CheckRequired ( this, "namespaceURI", value ); }
      }

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( "namespace" );
        root.SetAttribute ( "prefix", Util.CheckRequired ( this, "prefix", this.Prefix ) );
        root.SetAttribute ( "namespaceURI", Util.CheckRequired ( this, "namespaceURI", this.NamespaceUri ).ToString () );
        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this._uri = null;
        this._prefix = string.Empty;

        if ( string.Compare ( element.Name, "namespace", false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.GetType ().Name ) );
        string s = Util.GetElementOrAttributeValue ( "prefix", element );
        this.Prefix = s;

        s = Util.GetElementOrAttributeValue ( "namespaceURI", element );
        this.NamespaceUri = new Uri ( s );
      }

      #endregion

      #region ICCNetDocumentation Members

      /// <summary>
      /// Gets the documentation URI.
      /// </summary>
      /// <value>The documentation URI.</value>
      [Browsable ( false )]
      public Uri DocumentationUri {
        get { return new Uri ( "http://www.codeplex.com/rssbuildspublisher/" ); }
      }

      #endregion

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      public Namespace Clone () {
        return this.MemberwiseClone () as Namespace;
      }

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      object ICloneable.Clone () {
        return this.Clone ();
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return this.Prefix;
      }
    }

    /// <summary>
    /// Represents a feed item category
    /// </summary>
    public class Category : ISerialize, ICCNetObject, ICCNetDocumentation, ICloneable {
      private string _name = string.Empty;

      /// <summary>
      /// Initializes a new instance of the <see cref="Category"/> class.
      /// </summary>
      public Category () {

      }

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      [Description ( "The name of the category." ), DefaultValue ( null ), Category ( "Required" ), DisplayName ( "(Name)" )]
      public string Name {
        get { return this._name; }
        set { this._name = Util.CheckRequired ( this, "name", value ); }
      }

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( "category" );
        root.SetAttribute ( "name", Util.CheckRequired ( this, "name", this.Name ) );
        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this._name = string.Empty;

        if ( string.Compare ( element.Name, "category", false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.GetType ().Name ) );
        string s = Util.GetElementOrAttributeValue ( "name", element );
        this.Name = s;
      }

      #endregion

      #region ICCNetDocumentation Members

      /// <summary>
      /// Gets the documentation URI.
      /// </summary>
      /// <value>The documentation URI.</value>
      [Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
      public Uri DocumentationUri {
        get { return new Uri ( "http://www.codeplex.com/rssbuildspublisher/" ); }
      }

      #endregion

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      public Category Clone () {
        return this.MemberwiseClone () as Category;
      }

      #endregion

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      object ICloneable.Clone () {
        return this.Clone ();
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return this.Name;
      }
    }

    /// <summary>
    /// Represents a feed item category
    /// </summary>
    public class PingItem : ISerialize, ICCNetObject, ICCNetDocumentation, ICloneable {
      private string _feedName = string.Empty;
      private Uri _feedUri = null;
      private Uri _pingUrl = null;
      /// <summary>
      /// Initializes a new instance of the <see cref="Category"/> class.
      /// </summary>
      public PingItem () {

      }

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      [Description ( "The name of the feed." ), DefaultValue ( null ), Category ( "Required" ), DisplayName ( "(FeedName)" )]
      public string FeedName {
        get { return this._feedName; }
        set { this._feedName = Util.CheckRequired ( this, "feedName", value ); }
      }

      /// <summary>
      /// Gets or sets the feed URL.
      /// </summary>
      /// <value>The feed URL.</value>
      [Description ( "The URL to the feed." ), DefaultValue ( null ), Category ( "Required" ), DisplayName ( "(FeedUrl)" )]
      public Uri FeedUrl {
        get { return this._feedUri; }
        set { this._feedUri = Util.CheckRequired ( this, "feedUrl", value ); }
      }

      /// <summary>
      /// Gets or sets the ping URL.
      /// </summary>
      /// <value>The ping URL.</value>
      [Description ( "The URL to the host to ping." ), DefaultValue ( null ), Category ( "Required" ), DisplayName ( "(PingUrl)" )]
      public Uri PingUrl { get { return this._pingUrl; } set { this._pingUrl = Util.CheckRequired ( this, "pingUrl", value ); } }


      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public XmlElement Serialize () {
        XmlDocument doc = new XmlDocument ();
        XmlElement root = doc.CreateElement ( "pingItem" );

        XmlElement ele = doc.CreateElement ( "feedName" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.FeedName );
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "feedUrl" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.FeedUrl ).ToString ();
        root.AppendChild ( ele );

        ele = doc.CreateElement ( "pingUrl" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.PingUrl ).ToString ();
        root.AppendChild ( ele );
        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( XmlElement element ) {
        this._feedName = string.Empty;
        this._feedUri = null;
        this._pingUrl = null;

        if ( string.Compare ( element.Name, "pingItem", false ) != 0 )
          throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, "pingItem" ) );
        string s = Util.GetElementOrAttributeValue ( "feedUrl", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.FeedUrl = new Uri ( s );

        s = Util.GetElementOrAttributeValue ( "pingUrl", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.PingUrl = new Uri ( s );

        s = Util.GetElementOrAttributeValue ( "feedName", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.FeedName = s;
      }

      #endregion

      #region ICCNetDocumentation Members

      /// <summary>
      /// Gets the documentation URI.
      /// </summary>
      /// <value>The documentation URI.</value>
      [Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
      public Uri DocumentationUri {
        get { return new Uri ( "http://www.codeplex.com/rssbuildspublisher/" ); }
      }

      #endregion

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      public PingItem Clone () {
        PingItem pi = this.MemberwiseClone () as PingItem;
        pi.PingUrl = new Uri ( this.PingUrl.ToString () );
        pi.FeedUrl = new Uri ( this.FeedUrl.ToString () );
        return pi;
      }

      #endregion

      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      object ICloneable.Clone () {
        return this.Clone ();
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return this.PingUrl != null ? this.PingUrl.ToString(  ) : this.GetType(  ).Name;
      }
    }
  }
}
