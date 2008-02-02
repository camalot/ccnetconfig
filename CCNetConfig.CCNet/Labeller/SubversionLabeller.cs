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
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// 
  /// </summary>
  [
  Plugin, // indicates to CCNetConfig that this is not a core CCNet Object and it is a plugin
  MinimumVersion ( "1.2" ), // this tells CCNetConfig what the minimum version of a ccnet.config must be
  // MaximumVersion("1.2.1") // we can also set the maximum version that is allowed
  // , ExactVersion("1.2") // or if it only works with a specific version, we can use this attribute to say that.
  ]
  public class SubversionLabeller : Labeller, ICCNetDocumentation {
    private int? _buildNumber = null;
    private int? _majorVersion = null;
    private int? _minorVersion = null;
    private string _svnExecutable;
    private string _workingDirectory = null;

    // we pass the name of the labeller to the base constructor.
    /// <summary>
    /// Initializes a new instance of the <see cref="SubversionLabeller"/> class.
    /// </summary>
    public SubversionLabeller ( ) : base ( "SvnLabeller" ) {

    }

    /// <summary>
    /// Gets or sets the working directory.
    /// </summary>
    /// <value>The working directory.</value>
    [Category ( "Optional" ), // set the category for the property
    DefaultValue ( null ), // set a default value
    Description ( "The working directory of the source." ), // a description of the property
    // this is the editor for the property. This allows a user to browse for a folder
    Editor ( typeof ( BrowseForFolderUIEditor ), typeof ( UITypeEditor ) ),
    // sets the description for the browse for folder dialog
    BrowseForFolderDescription ( "Select the working directory" )
    ]
    public string WorkingDirectory {
      get { return _workingDirectory; }
      set { _workingDirectory = value; }
    }

    /// <summary>
    /// Gets or sets the executable.
    /// </summary>
    /// <value>The executable.</value>
    [Category ( "Required" ), // set the category for the property as required
    DisplayName ( "(Executable)" ), // set the display name of the property if you don't want the default.
    DefaultValue ( null ), // set the default value
    Description ( "The path to the svn executable." ), // a description of the property
    Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) ),
    FileTypeFilter ( "SVN Executable|svn.exe" ), // sets the filter for the open file dialog
    OpenFileDialogTitle ( "Select SVN executable" ) ] //sets the title of the open file dialog
    public string Executable {
      get { return _svnExecutable; }
      set {
        // since this is a required property, I use the Util.CheckRequired method to ensure
        // the property is set with a value. If it's not, this method throws an exception
        // that CCNetConfig then displays to the user.
        _svnExecutable = Util.CheckRequired ( this, "svnLabeller", value );
      }
    }

    /// <summary>
    /// Gets or sets the minor version.
    /// </summary>
    /// <value>The minor version.</value>
    [Category ( "Optional" ), // set the category
    DefaultValue ( null ), // set the dfefault value
    Description ( "The minor version number" ), // set the description of the property
    // this editor display a numeric up/down box to the user.
    Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ),
    MinimumValue ( 0 )] // set a minimum value for the numeric up/down field.
    public int? MinorVersion {
      get { return _minorVersion; }
      set { _minorVersion = value; }
    }

    /// <summary>
    /// Gets or sets the major version.
    /// </summary>
    /// <value>The major version.</value>
    [Category ( "Optional" ), // set the category
    DefaultValue ( null ), // set the dfefault value
    Description ( "The major version number" ), // set the description of the property
    // this editor display a numeric up/down box to the user.
    Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ),
    MinimumValue ( 0 )] // set a minimum value for the numeric up/down field.
    public int? MajorVersion {
      get { return _majorVersion; }
      set { _majorVersion = value; }
    }

    /// <summary>
    /// Gets or sets the build number.
    /// </summary>
    /// <value>The build number.</value>
    [Category ( "Optional" ), // set the category
    DefaultValue ( null ), // set the dfefault value
    Description ( "The build number" ), // set the description of the property
    // this editor display a numeric up/down box to the user.
    Editor ( typeof ( NumericUpDownUIEditor ), typeof ( UITypeEditor ) ),
    MinimumValue ( 0 )] // set a minimum value for the numeric up/down field.
    public int? BuildNumber {
      get { return _buildNumber; }
      set { _buildNumber = value; }
    }

    // this method is inherited from Labeller and must be implemented. 
    // it is used when copying a project or labeller to the clipboard or to the new project.
    // this method should perform a deep copy of the object.
    // All CCNetConfig objects support cloning. 
    // If using a List for your properties, you should instead use the ClonableList in 
    // CCNetConfig.Core. This list already implements the Clone method
    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone ( ) {
      // create a shallow clone of the image.
      SubversionLabeller svnl = this.MemberwiseClone ( ) as SubversionLabeller;
      // since this plugin doesn't have anything that must use a deep clone, 
      // we can just return the shallow clone.
      return svnl;
    }

    // This method takes the XmlElement from the configuration file and sets the values of the object.
    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      // first we want to reset the values of the instance to ensure they do not contain old values.

      // since this property is required we want to set the private field and not the public property 
      // or it will throw an exception.
      this._svnExecutable = null;
      // the rest we can just reset the public property.
      this.WorkingDirectory = null;
      this.MajorVersion = null;
      this.MinorVersion = null;
      this.BuildNumber = null;

      // here we check that the element type value == the type name specified in the constructor.
      if ( string.Compare ( element.GetAttribute ( "type" ), base.TypeName, false ) != 0 ) {
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", 
                  element.GetAttribute ( "type" ), base.TypeName ) );
      }

      // since CC.NET configuration values can either be an element or an attribute of the 
      // base element, we use the Util.GetElementOrAttributeValue to find the value of 
      // the property. The first argument is the property name, the second is the 
      // base element, in this case the labeller element.
      string s = Util.GetElementOrAttributeValue ( "executable", element );
      // since this is a required proeprty, we don't check the results of 
      // what the method returned. We let the CheckRequired do that in the 
      // property setter.
      this.Executable = s;

      s = Util.GetElementOrAttributeValue ( "workingDirectory", element );
      // check if there is a value, if so set it.
      if ( !string.IsNullOrEmpty ( s ) ) {
        this.WorkingDirectory = s;
      }

      // get the minor version value
      s = Util.GetElementOrAttributeValue ( "MinorVersion", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        // since this property expects an integer, we try to parse the string
        if ( int.TryParse ( s, out i ) ) {
          this.MinorVersion = i;
        }
      }

      // get the major version value
      s = Util.GetElementOrAttributeValue ( "MajorVersion", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse ( s, out i ) ) {
          this.MajorVersion = i;
        }
      }

      // get the build number value
      s = Util.GetElementOrAttributeValue ( "BuildNumber", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse ( s, out i ) ) {
          this.BuildNumber = i;
        }
      }
    }

    // this method takes the values of the object and converts them in to an XmlElement.
    // The element is then imported in to the configuration document when the user
    // saves their confiuration.
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      // create a base xml document for use.
      XmlDocument doc = new XmlDocument ( );
      // create the xml element that we will return
      XmlElement root = doc.CreateElement ( "labeller" );
      // the labeller element defines what type of labeller by a type attribute,
      // so we add that attribute to the element.
      root.SetAttribute ( "type", base.TypeName );

      // CCNetConfig sets all properties as elements, but this is not required
      // since when deserializing, it will check for elements or attributes.
      // We create the executable element.
      XmlElement ele = doc.CreateElement ( "executable" );
      // Again we check the value because this is a required property. 
      // if the user never attempted to change this value, then it would have not been 
      // checked, so we have to do it here too.
      ele.InnerText = Util.CheckRequired ( this, "executable", this.Executable );
      // add the new element to the root element.
      root.AppendChild ( ele );

      // check that we should even add this property. 
      if ( !string.IsNullOrEmpty ( this.WorkingDirectory ) ) {
        ele = doc.CreateElement ( "workingDirectory" );
        ele.InnerText = this.WorkingDirectory;
        root.AppendChild ( ele );
      }

      // check that we should even add this property. 
      if ( this.MinorVersion.HasValue ) {
        ele = doc.CreateElement ( "MinorVersion" );
        ele.InnerText = this.MinorVersion.Value.ToString ( );
        root.AppendChild ( ele );
      }

      // check that we should even add this property. 
      if ( this.MajorVersion.HasValue ) {
        ele = doc.CreateElement ( "MajorVersion" );
        ele.InnerText = this.MajorVersion.Value.ToString ( );
        root.AppendChild ( ele );
      }

      // check that we should even add this property. 
      if ( this.BuildNumber.HasValue ) {
        ele = doc.CreateElement ( "BuildNumber" );
        ele.InnerText = this.BuildNumber.Value.ToString ( );
        root.AppendChild ( ele );
      }

      return root;
    }

    #region ICCNetDocumentation Members
    // we dont want this visible so we set attributes to hide it.
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
    // this property comes when you implement ICCNetDocumentation
    // this tells CCNetConfig where documentation for the object is located and
    // loads the url in the browser docked on the bottom of the configuration
    // window.
    public Uri DocumentationUri {
      get { return new Uri ( "http://www.jcxsoftware.com/jcx/node/1635/print?build=1.2.0.3498" ); }
    }

    #endregion
  }
}
