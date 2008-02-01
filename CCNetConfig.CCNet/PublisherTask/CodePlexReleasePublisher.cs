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
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Collections;
using CCNetConfig.Core.Serialization;
using System.Drawing.Design;
using System.Xml;
using System.IO;
using CCNetConfig.Core.Enums;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// A publisher for CruiseControl.NET that wraps the CodePlex API to submit a release. In some cases, 
  /// a publisher may be a better candidate then using MSBuild task because the CodePlex API 
  /// requires a user name and password to submit the release. The ccnet.config file usually wont be 
  /// controlled by CodePlex Source Control but the MSBuild script usually is. 
  /// </summary>
  [MinimumVersion ( "1.2" ), Plugin]
  public class CodePlexReleasePublisher : PublisherTask, ICCNetDocumentation {
    private string _username = string.Empty;
    private HiddenPassword _password = null;
    private CloneableList<ReleaseItem> _releases = null;
    private string _projectName = string.Empty;
    /// <summary>
    /// Initializes a new instance of the <see cref="CodePlexReleasePublisher"/> class.
    /// </summary>
    public CodePlexReleasePublisher ( )
      : base ( "codeplexRelease" ) {
      _password = new HiddenPassword ( );
      _releases = new CloneableList<ReleaseItem> ( );
    }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    [Category ( "Required" ), DisplayName ( "(Username)" ), DefaultValue ( null ),
    Description ( "The username used to log in to CodePlex. This username must have access to create a release for the project." )]
    public string Username { get { return this._username; } set { this._username = Util.CheckRequired ( this, "Username", value ); } }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [Category ( "Required" ), DisplayName ( "(Password)" ), DefaultValue ( null ),
    Description ( "The password used to log in to CodePlex." ), TypeConverter ( typeof ( PasswordTypeConverter ) )]
    public HiddenPassword Password { get { return this._password; } set { this._password.Password = Util.CheckRequired ( this, "Password", value.GetPassword ( ) ); } }

    /// <summary>
    /// Gets or sets the releases.
    /// </summary>
    /// <value>The releases.</value>
    [Category ( "Required" ), DisplayName ( "(Releases)" ), DefaultValue ( null ),
    Description ( "The releases to create. At least 1 release is required." ), TypeConverter ( typeof ( IListTypeConverter ) )]
    public CloneableList<ReleaseItem> Releases { get { return this._releases; } set { this._releases = Util.CheckRequired<ReleaseItem> ( this, "Releases", value ); } }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    /// <value>The name of the project.</value>
    [Category ( "Optional" ), DefaultValue ( null ),
    Description ( "The CodePlex Project name. This is the url name of the project. If left empty, the CCNet Project name will be used as all lowercase. This is case sensitive." )]
    public string ProjectName { get { return this._projectName; } set { this._projectName = value; } }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      XmlDocument doc = new XmlDocument ( );
      XmlElement root = doc.CreateElement ( this.TypeName );

      if ( !string.IsNullOrEmpty ( this.ProjectName ) )
        root.SetAttribute ( "projectName", this.ProjectName );

      XmlElement ele = doc.CreateElement ( "userName" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Username );
      root.AppendChild ( ele );

      ele = doc.CreateElement ( "password" );
      ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Password.GetPassword ( ) );
      root.AppendChild ( ele );

      if ( this.Releases.Count > 0 ) {
        ele = doc.CreateElement ( "releases" );
        foreach ( ReleaseItem ri in this.Releases ) {
          XmlElement tele = ri.Serialize ( ) as XmlElement;
          if ( tele != null )
            ele.AppendChild ( doc.ImportNode ( tele, true ) );
        }
        root.AppendChild ( ele );
      }

      return root;
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this._username = string.Empty;
      this._password = new HiddenPassword ( );
      this.ProjectName = string.Empty;
      this.Releases.Clear ( );

      string s = Util.GetElementOrAttributeValue ( "userName", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Username = s;

      s = Util.GetElementOrAttributeValue ( "password", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Password.Password = s;

      s = Util.GetElementOrAttributeValue ( "projectName", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.ProjectName = s;

      XmlElement rele = element.SelectSingleNode ( "releases" ) as XmlElement;
      if ( rele != null ) {
        foreach ( XmlElement re in rele.SelectNodes ( "release" ) ) {
          ReleaseItem ri = new ReleaseItem ( );
          ri.Deserialize ( re );
          this.Releases.Add ( ri );
        }
      }

    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      CodePlexReleasePublisher cprp = this.MemberwiseClone ( ) as CodePlexReleasePublisher;
      cprp.Password = this.Password.Clone ( );
      cprp.Releases = this.Releases.Clone ( );
      return cprp;
    }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://codeplex.com/cprp/" ); }
    }

    #endregion



    /// <summary>
    /// Represents a new release that is going to be created.
    /// </summary>
    public class ReleaseItem : ICCNetObject, ICloneable, ISerialize {
      /// <summary>
      /// The status of the release.
      /// </summary>
      public enum ReleaseStatus {
        /// <summary>
        /// The release is not available to users
        /// </summary>
        Planned,
        /// <summary>
        /// The release is publically available
        /// </summary>
        Released,
      }

      /// <summary>
      /// The type of release
      /// </summary>
      public enum ReleaseType {
        /// <summary>
        /// An alpha release
        /// </summary>
        Alpha,
        /// <summary>
        /// A beta release
        /// </summary>
        Beta,
        /// <summary>
        /// A nightly release
        /// </summary>
        Nightly,
        /// <summary>
        /// A production release
        /// </summary>
        Production
      }

      private string _releaseName = string.Empty;
      private string _description = string.Empty;
      private CloneableList<ReleaseFile> _files = null;
      private ReleaseStatus? _releaseStatus = null;
      private DateTime? _releaseDate = null;
      private bool? _isDefaultRelease = null;
      private bool? _showOnHomePage = null;
      private bool? _showToPublic = null;
      private ReleaseType? _releaseType = null;
      private PublishBuildCondition? _buildCondition = null;

      /// <summary>
      /// Initializes a new instance of the <see cref="ReleaseItem"/> class.
      /// </summary>
      public ReleaseItem ( ) {
        this._files = new CloneableList<ReleaseFile> ( );
      }

      /// <summary>
      /// Gets or sets the name of the release.
      /// </summary>
      /// <value>The name of the release.</value>
      [Category ( "Required" ), DefaultValue ( null ), DisplayName ( "(ReleaseName)" ),
      Description ( "The name of the release. This MUST be a unique name." )]
      public string ReleaseName { get { return this._releaseName; } set { this._releaseName = Util.CheckRequired ( this, "ReleaseName", value ); } }

      /// <summary>
      /// Gets or sets the description.
      /// </summary>
      /// <value>The description.</value>
      [Category ( "Required" ), DefaultValue ( null ), DisplayName ( "(Description)" ),
      Description ( "A description of the release." ), MaximumStringLength ( 4000 ),
      Editor ( typeof ( MultilineStringUIEditor ), typeof ( UITypeEditor ) )]
      public string Description { get { return this._description; } set { this._description = Util.CheckRequired ( this, "Description", value ); } }

      /// <summary>
      /// Gets or sets the build condition.
      /// </summary>
      /// <value>The build condition.</value>
      [Category ( "Optional" ), DefaultValue ( null ), Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ),
      Description ( "The build condition in which a publish should take place." )]
      public PublishBuildCondition? BuildCondition {
        get { return this._buildCondition; }
        set { this._buildCondition = value; }
      }

      /// <summary>
      /// Gets or sets the files.
      /// </summary>
      /// <value>The files.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "The files to add to the release." ),
      TypeConverter ( typeof ( IListTypeConverter ) )]
      public CloneableList<ReleaseFile> Files { get { return this._files; } set { this._files = value; } }

      /// <summary>
      /// Gets or sets the status.
      /// </summary>
      /// <value>The status.</value>
      [Category ( "Optional" ), DefaultValue ( null ), Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ),
      Description ( "The status of the release. The default is 'Planned'." )]
      public ReleaseStatus? Status { get { return this._releaseStatus; } set { this._releaseStatus = value; } }

      /// <summary>
      /// Gets or sets the release date.
      /// </summary>
      /// <value>The release date.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "If the release status is 'Released', this is the date the release will be or was released." ),
      Editor ( typeof ( DatePickerUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DateTimeConverter ) )]
      public DateTime? ReleaseDate { get { return this._releaseDate; } set { this._releaseDate = value; } }

      /// <summary>
      /// Gets or sets the is default release.
      /// </summary>
      /// <value>The is default release.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "If true, this release will be set as the default release. The default is true." ),
      Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
      public bool? IsDefaultRelease { get { return this._isDefaultRelease; } set { this._isDefaultRelease = value; } }

      /// <summary>
      /// Gets or sets the show on home page.
      /// </summary>
      /// <value>The show on home page.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "If true, this release will be shown on the home page. The default is true." ),
      Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
      public bool? ShowOnHomePage { get { return this._showOnHomePage; } set { this._showOnHomePage = value; } }

      /// <summary>
      /// Gets or sets the show to public.
      /// </summary>
      /// <value>The show to public.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "If true, this release will be shown to the public. The default is true." ),
      Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) )]
      public bool? ShowToPublic { get { return this._showToPublic; } set { this._showToPublic = value; } }

      /// <summary>
      /// Gets or sets the name of the release type.
      /// </summary>
      /// <value>The name of the release type.</value>
      [Category ( "Optional" ), DefaultValue ( null ), DisplayName ( "ReleaseType" ),
      Description ( "The type of the release. This was added to supply additional information in the release name." ),
      Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableEnumTypeConverter ) )]
      public ReleaseType? ReleaseTypeName { get { return this._releaseType; } set { this._releaseType = value; } }
      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      public ReleaseItem Clone ( ) {
        ReleaseItem ri = this.MemberwiseClone ( ) as ReleaseItem;
        ri.ReleaseDate = this.ReleaseDate;
        ri.Status = this.Status;
        ri.Files = this.Files;
        ri.ReleaseTypeName = this.ReleaseTypeName;
        ri.BuildCondition = this.BuildCondition;
        return ri;
      }


      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      object ICloneable.Clone ( ) {
        return this.Clone ( );
      }

      #endregion

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public System.Xml.XmlElement Serialize ( ) {
        XmlDocument doc = new XmlDocument ( );
        XmlElement root = doc.CreateElement ( "release" );
        root.SetAttribute ( "releaseName", Util.CheckRequired ( this, "releaseName", this.ReleaseName ) );

        XmlElement ele = doc.CreateElement ( "description" );
        ele.InnerText = Util.CheckRequired ( this, ele.Name, this.Description );
        root.AppendChild ( ele );

        if ( this.Files.Count > 0 ) {
          ele = doc.CreateElement ( "releaseFiles" );
          foreach ( ReleaseFile rf in this.Files ) {
            XmlElement tele = rf.Serialize ( ) as XmlElement;
            if ( tele != null )
              ele.AppendChild ( doc.ImportNode ( tele, true ) );
          }
          root.AppendChild ( ele );
        }

        if ( this.BuildCondition.HasValue ) {
          ele = doc.CreateElement ( "buildCondition" );
          ele.InnerText = this.BuildCondition.Value.ToString ( );
          root.AppendChild ( ele );
        }

        if ( this.Status.HasValue ) {
          ele = doc.CreateElement ( "releaseStatus" );
          ele.InnerText = this.Status.Value.ToString ( );
          root.AppendChild ( ele );
        }

        if ( this.ReleaseTypeName.HasValue ) {
          ele = doc.CreateElement ( "releaseType" );
          ele.InnerText = this.ReleaseTypeName.Value.ToString ( );
          root.AppendChild ( ele );
        }

        if ( this.ReleaseDate.HasValue ) {
          ele = doc.CreateElement ( "releaseDate" );
          ele.InnerText = this.ReleaseDate.Value.ToShortDateString ( );
          root.AppendChild ( ele );
        }

        if ( this.IsDefaultRelease.HasValue ) {
          ele = doc.CreateElement ( "isDefaultRelease" );
          ele.InnerText = this.IsDefaultRelease.Value.ToString ( );
          root.AppendChild ( ele );
        }

        if ( this.ShowToPublic.HasValue ) {
          ele = doc.CreateElement ( "showToPublic" );
          ele.InnerText = this.ShowToPublic.Value.ToString ( );
          root.AppendChild ( ele );
        }

        if ( this.ShowOnHomePage.HasValue ) {
          ele = doc.CreateElement ( "showOnHomePage" );
          ele.InnerText = this.ShowOnHomePage.Value.ToString ( );
          root.AppendChild ( ele );
        }

        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( System.Xml.XmlElement element ) {
        this._releaseName = string.Empty;
        this._description = string.Empty;
        this.Files.Clear ( );
        this.Status = null;
        this.ReleaseDate = null;
        this.IsDefaultRelease = null;
        this.ShowOnHomePage = null;
        this.ShowToPublic = null;

        string s = Util.GetElementOrAttributeValue ( "releaseName", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.ReleaseName = s;

        s = Util.GetElementOrAttributeValue ( "description", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Description = s;

        s = Util.GetElementOrAttributeValue ( "buildCondition", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.BuildCondition = ( PublishBuildCondition ) Enum.Parse ( typeof ( PublishBuildCondition ), s, true );

        XmlElement rele = element.SelectSingleNode ( "releaseFiles" ) as XmlElement;
        if ( rele != null ) {
          foreach ( XmlElement re in rele.SelectNodes ( "releaseFile" ) ) {
            ReleaseFile rf = new ReleaseFile ( );
            rf.Deserialize ( re );
            this.Files.Add ( rf );
          }
        }

        s = Util.GetElementOrAttributeValue ( "releaseStatus", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Status = ( ReleaseStatus ) Enum.Parse ( typeof ( ReleaseStatus ), s, true );

        s = Util.GetElementOrAttributeValue ( "releaseType", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.ReleaseTypeName = ( ReleaseType ) Enum.Parse ( typeof ( ReleaseType ), s, true );

        s = Util.GetElementOrAttributeValue ( "releaseDate", element );
        if ( !string.IsNullOrEmpty ( s ) ) {
          DateTime date = DateTime.Now;
          if ( DateTime.TryParse ( s, out date ) )
            this.ReleaseDate = date;
        }

        s = Util.GetElementOrAttributeValue ( "isDefaultRelease", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.IsDefaultRelease = string.Compare ( s, bool.TrueString, true ) == 0;

        s = Util.GetElementOrAttributeValue ( "showOnHomePage", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.ShowOnHomePage = string.Compare ( s, bool.TrueString, true ) == 0;

        s = Util.GetElementOrAttributeValue ( "showToPublic", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.ShowToPublic = string.Compare ( s, bool.TrueString, true ) == 0;
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString ( ) {
        return string.IsNullOrEmpty ( this.ReleaseName ) ? this.GetType ( ).Name : this.ReleaseName;
      }
    }

    /// <summary>
    /// Represents a file that will be added to the release.
    /// </summary>
    public class ReleaseFile : ICCNetObject, ICloneable, ISerialize {
      /// <summary>
      /// The types of files that can be uploaded.
      /// </summary>
      public enum FileType {
        /// <summary>
        /// A binary file
        /// </summary>
        RuntimeBinary,
        /// <summary>
        /// Source code file
        /// </summary>
        SourceCode,
        /// <summary>
        /// Documentation file
        /// </summary>
        Documentation,
        /// <summary>
        /// Example file
        /// </summary>
        Example,
      }
      private string _fileName = string.Empty;
      private FileType _fileType = FileType.RuntimeBinary;
      private string _mimetype = string.Empty;
      private string _name = string.Empty;

      /// <summary>
      /// Initializes a new instance of the <see cref="ReleaseFile"/> class.
      /// </summary>
      public ReleaseFile ( ) {

      }

      /// <summary>
      /// Gets or sets the name of the file.
      /// </summary>
      /// <value>The name of the file.</value>
      [Category ( "Required" ), DisplayName ( "(FileName)" ), DefaultValue ( null ),
      Description ( "The full path to the file." ), Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ),
      OpenFileDialogTitle ( "Select the file to upload" ),
      FileTypeFilter ( "Compressed Files|*.zip;*.7z;*.rar;*.jar;*.z;*.tar;*.gzip|All Files|*.*" )]
      public string FileName { get { return this._fileName; } set { this._fileName = Util.CheckRequired ( this, "FileName", value ); } }

      /// <summary>
      /// Gets or sets the type of the file.
      /// </summary>
      /// <value>The type of the file.</value>
      [Category ( "Required" ), DefaultValue ( CodePlexReleasePublisher.ReleaseFile.FileType.RuntimeBinary ),
      DisplayName ( "(FileType)" ), Description ( "The type of file this is." )]
      public ReleaseFile.FileType ReleaseFileType { get { return this._fileType; } set { this._fileType = value; } }

      /// <summary>
      /// Gets or sets the type of the MIME.
      /// </summary>
      /// <value>The type of the MIME.</value>
      [Category ( "Optional" ), DefaultValue ( "application/octet-stream" ),
      Description ( "The MIME type associated with the file. If not specified, the default value is application/octet-stream." )]
      public string MimeType { get { return this._mimetype; } set { this._mimetype = value; } }

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      [Category ( "Optional" ), DefaultValue ( null ),
      Description ( "The display name associated with the file. If not specified, the FileName will be displayed." )]
      public string Name { get { return this._name; } set { this._name = value; } }
      #region ICloneable Members

      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      public ReleaseFile Clone ( ) {
        ReleaseFile rf = this.MemberwiseClone ( ) as ReleaseFile;
        rf.ReleaseFileType = this.ReleaseFileType;
        return rf;
      }


      /// <summary>
      /// Creates a new object that is a copy of the current instance.
      /// </summary>
      /// <returns>
      /// A new object that is a copy of this instance.
      /// </returns>
      object ICloneable.Clone ( ) {
        return this.Clone ( );
      }

      #endregion

      #region ISerialize Members

      /// <summary>
      /// Serializes this instance.
      /// </summary>
      /// <returns></returns>
      public System.Xml.XmlElement Serialize ( ) {
        XmlDocument doc = new XmlDocument ( );
        XmlElement root = doc.CreateElement ( "releaseFile" );
        root.SetAttribute ( "fileName", Util.CheckRequired ( this, "fileName", this.FileName ) );
        root.SetAttribute ( "fileType", this.ReleaseFileType.ToString ( ) );
        if ( !string.IsNullOrEmpty ( this.MimeType ) )
          root.SetAttribute ( "mimeType", this.MimeType );

        if ( !string.IsNullOrEmpty ( this.Name ) )
          root.SetAttribute ( "name", this.Name );

        return root;
      }

      /// <summary>
      /// Deserializes the specified element.
      /// </summary>
      /// <param name="element">The element.</param>
      public void Deserialize ( System.Xml.XmlElement element ) {
        this._fileName = string.Empty;
        this.ReleaseFileType = FileType.RuntimeBinary;
        this.MimeType = string.Empty;
        this.Name = string.Empty;

        string s = Util.GetElementOrAttributeValue ( "fileName", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.FileName = s;

        s = Util.GetElementOrAttributeValue ( "fileType", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.ReleaseFileType = ( FileType ) Enum.Parse ( typeof ( FileType ), s, true );

        s = Util.GetElementOrAttributeValue ( "mimeType", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.MimeType = s;

        s = Util.GetElementOrAttributeValue ( "name", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.Name = s;
      }

      #endregion

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString ( ) {
        return string.IsNullOrEmpty ( this.Name ) ? Path.GetFileName ( this.FileName ) : this.Name;
      }
    }
  }
}
