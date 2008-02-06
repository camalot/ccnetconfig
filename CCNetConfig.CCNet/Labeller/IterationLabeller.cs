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
using System.Xml;
using System.ComponentModel;
using CCNetConfig.Core;
using System.Drawing.Design;
using CCNetConfig.Core.Components;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The Iteration Labeller is similar to the <see cref="CCNetConfig.CCNet.DefaultLabeller">DefaultLabeller</see>; however, it maintains a revision 
  /// number that is incremented by one for each iteration from the release start date. For example, if the release start date was June 1, 2005 and the 
  /// iteration duration was 2 weeks, the iteration number on July 1, 2005 would be 3. This would create a label of &lt;prefix&gt;.3.&lt;build number&gt;.
  /// </summary>
  [TypeConverter (typeof (ExpandableObjectConverter)),
  MinimumVersion ( "1.0" )]
  public class IterationLabeller : Labeller, ICCNetDocumentation {
    private string _prefix = string.Empty;
    private int? _duration = null;
    private DateTime? _releaseStartDate = DateTime.Now.Date;
    private string _separator = string.Empty;
    private bool? _incrementOnFailure = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="IterationLabeller"/> class.
    /// </summary>
    public IterationLabeller()
      : base ("iterationlabeller") {

    }

    /// <summary>
    /// Any string to be put in front of all labels
    /// </summary>
    [Description ( "Any string to be put in front of all labels" ), DefaultValue ( null ), Category ( "Optional" ),
    ReflectorName("prefix")]
    public string Prefix { get { return this._prefix; } set { this._prefix = value; } }
    /// <summary>
    /// The duration of the iteration in weeks
    /// </summary>
    [Description ( "The duration of the iteration in weeks" ), DefaultValue ( null ), Category ( "Optional" ),
    ReflectorName("duration")]
    public int? Duration { get { return this._duration; } set { this._duration = value; } }
    /// <summary>
    /// The start date for the release (the start date of iteration one)
    /// </summary>
    [Editor ( typeof ( DatePickerUIEditor ), typeof ( UITypeEditor ) ), Category ( "Required" ),DisplayName("(ReleaseStartDate)"),
    DefaultValue (null), Description ("The start date for the release (the start date of iteration one)"),
    ReflectorName("releaseStartDate"), FormatProvider("yyyy/MM/dd")]
    public DateTime ReleaseStartDate { get { return this._releaseStartDate.Value; } set { this._releaseStartDate = Util.CheckRequired (this, "releaseStartDate", value); } }
    /// <summary>
    /// The separator between the iteration number and the build number.
    /// </summary>
    [Description ( "The separator between the iteration number and the build number." ), DefaultValue ( null ),
    Category ( "Optional" ), ReflectorName ( "separator" )]
    public string Separator { get { return this._separator; } set { this._separator = value; } }
    /// <summary>
    /// If true, the label will be incremented even if the build fails. Otherwise it will only be incremented if the build succeeds. (Added in CCNet 1.1)
    /// </summary>
    [Description ("If true, the label will be incremented even if the build fails. Otherwise it will only be incremented if the build succeeds. (Added in CCNet 1.1)"),
    Editor (typeof (DefaultableBooleanUIEditor), typeof (UITypeEditor)),
      TypeConverter (typeof (DefaultableBooleanTypeConverter)), ReflectorName("incrementOnFailure"),
     DefaultValue ( null ), Category ( "Optional" ), MinimumVersion( "1.1" )]
    public bool? IncrementOnFailure { get { return this._incrementOnFailure; } set { this._incrementOnFailure = value; } }

    /// <summary>
    /// Creates a copy of the IterationLabeller.
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone () {
      return this.MemberwiseClone ( ) as IterationLabeller;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize() {
      return new CCNetConfig.Core.Serialization.Serializer<IterationLabeller> ( ).Serialize ( this );
      
      /*XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ("labeller");
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      root.SetAttribute ("type", this.TypeName);
      XmlElement ele = null;

      if ( !string.IsNullOrEmpty (this.Prefix) ) {
        ele = doc.CreateElement ("prefix");
        ele.InnerText = this.Prefix;
        root.AppendChild (ele);
      }

      if ( this.Duration.HasValue ) {
        ele = doc.CreateElement ("duration");
        ele.InnerText = this.Duration.Value.ToString ();
        root.AppendChild (ele);
      }

      ele = doc.CreateElement ("releaseStartDate");
      ele.InnerText = this.ReleaseStartDate.ToString ("yyyy/MM/dd");
      root.AppendChild (ele);

      if ( !string.IsNullOrEmpty (this.Separator) ) {
        ele = doc.CreateElement ("separator");
        ele.InnerText = this.Separator;
        root.AppendChild (ele);
      }

      if ( this.IncrementOnFailure.HasValue ) {
        ele = doc.CreateElement ("incrementOnFailure");
        ele.InnerText = this.IncrementOnFailure.Value.ToString ();
        root.AppendChild (ele);
      }
      return root;*/
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable (false), ReflectorIgnore]
    public Uri DocumentationUri {
      get { return new Uri ("http://ccnet.thoughtworks.net/display/CCNET/Iteration+Labeller?decorator=printable"); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.Duration = null;
      this.IncrementOnFailure = null;
      this.Prefix = string.Empty;
      this.ReleaseStartDate = DateTime.Now;
      this.Separator = string.Empty;

      if ( string.Compare (element.GetAttribute ("type"), this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute ("type"), this.TypeName));
      DateTime dt = DateTime.Now;
      string s = Util.GetElementOrAttributeValue ( "releaseStartDate", element );
      if ( !string.IsNullOrEmpty(s) )
        DateTime.TryParse (s, out dt);
      this.ReleaseStartDate = dt;

      s = Util.GetElementOrAttributeValue ("prefix",element);
      if ( !string.IsNullOrEmpty ( s ) )
        this.Prefix = s;

      s = Util.GetElementOrAttributeValue ( "separator", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.Separator = s;

      s = Util.GetElementOrAttributeValue ( "duration", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        int i = 0;
        if ( int.TryParse (s, out i) )
          this.Duration = i;
      }

      s = Util.GetElementOrAttributeValue ( "incrementOnFailure", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        bool i = false;
        if ( bool.TryParse (s, out i) )
          this.IncrementOnFailure = i;
      }

    }
  }
}
