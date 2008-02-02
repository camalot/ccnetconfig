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
using System.Xml;
using System.ComponentModel;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// By default, CCNet uses a plain incrementing build number as a build label. Some source controls (e.g. Perforce Source Control Block) 
  /// require you to use a different naming scheme if you want CCNet to apply labels to source control on successful builds. You can do this by 
  /// specifying your own configuration of the default labeller in your project. The following configuration would prefix all labels with the string 
  /// 'Foo-1-', so the 213th build would be labelled 'Foo-1-213'
  /// </summary>
  [TypeConverter (typeof (ExpandableObjectConverter)),
  MinimumVersion ( "1.0" )]
  public class DefaultLabeller : Labeller, ICCNetDocumentation {
    private string _prefix;
    private bool? _incrementOnFailure = null;
    private string _labelFormat = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultLabeller"/> class.
    /// </summary>
    public DefaultLabeller() : base("defaultlabeller") {

    }

    /// <summary>
    /// Any string to be put in front of all labels
    /// </summary>
    [Description ( "Any string to be put in front of all labels" ), DefaultValue ( null ), Category ( "Optional" )]
    public string Prefix { get { return this._prefix; } set { this._prefix = value; } }
    /// <summary>
    /// If true, the label will be incremented even if the build fails. Otherwise it will only be 
    /// incremented if the build succeeds.
    /// </summary>
    [Description ( "If true, the label will be incremented even if the build fails. Otherwise it will only be incremented if the build succeeds." ),
      Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
      TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
     DefaultValue ( null ), Category ( "Optional" ), MinimumVersion( "1.1" )]
    public bool? IncrementOnFailure { get { return this._incrementOnFailure; } set { this._incrementOnFailure = value; } }

    /// <summary>
    /// Gets or sets the label format.
    /// </summary>
    /// <value>The label format.</value>
    [Description("A format applied to the buildnumber."), DefaultValue("0"), Category("Optional"), 
    MinimumVersion("1.3")]
    public string LabelFormat { get { return this._labelFormat; } set { this._labelFormat = value; } }
    /// <summary>
    /// Creates a copy of the DefaultLabeller object.
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone () {
      DefaultLabeller dl = this.MemberwiseClone(  ) as  DefaultLabeller;
      return dl;
    }


    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize() {
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( Labeller ) );
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ("labeller");
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      root.SetAttribute ("type", this.TypeName);
      XmlElement ele = null;
      if ( !string.IsNullOrEmpty (this.Prefix) ) {
        ele = doc.CreateElement ("prefix");
        ele.InnerText = this.Prefix;
        root.AppendChild (ele);
      }

      if ( this.IncrementOnFailure.HasValue ) {
        ele = doc.CreateElement ("incrementOnFailure");
        ele.InnerText = this.IncrementOnFailure.Value.ToString ();
        root.AppendChild (ele);
      }

      Version propVersion = Util.GetMinimumVersion ( this.GetType ( ).GetProperty ( "LabelFormat" ) );
      if ( Util.IsInVersionRange ( propVersion, null, versionInfo ) ) {
        if ( !string.IsNullOrEmpty ( this.LabelFormat ) ) {
          ele = doc.CreateElement ( "labelFormat" );
          ele.InnerText = this.LabelFormat;
          root.AppendChild ( ele );
        }
      }
      return root;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Default+Labeller?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      Version versionInfo = Util.GetTypeDescriptionProviderVersion ( typeof ( Labeller ) );

      this.IncrementOnFailure = null;
      this.Prefix = null;
     
      if ( string.Compare (element.GetAttribute ("type"), this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute ("type"), this.TypeName));

      string s = Util.GetElementOrAttributeValue("prefix",element);
      if ( !string.IsNullOrEmpty ( s ) )
        this.Prefix = s;

      s = Util.GetElementOrAttributeValue ("incrementOnFailure", element);
      if ( !string.IsNullOrEmpty ( s ) ) {
        bool i = false;
        if ( bool.TryParse (s, out i) )
          this.IncrementOnFailure = i;
      }
      Version propVersion = Util.GetMinimumVersion ( this.GetType ( ).GetProperty ( "LabelFormat" ) );
      if ( Util.IsInVersionRange ( propVersion, null, versionInfo ) ) {
        s = Util.GetElementOrAttributeValue ( "labelFormat", element );
        if ( !string.IsNullOrEmpty ( s ) )
          this.LabelFormat = s;
      }
    }
  }
}
