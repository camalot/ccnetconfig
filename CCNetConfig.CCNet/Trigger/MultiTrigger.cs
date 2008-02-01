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
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// <para>The <see cref="CCNetConfig.CCNet.MultiTrigger">Multiple Trigger</see> is used to support the execution of multiple nested triggers. 
  /// Each <see cref="CCNetConfig.Core.Trigger">Trigger</see> will be executed sequentially in the order specified in the configuration file. 
  /// By default, if any of the triggers specify that a build should occur then a build will be triggered. The 
  /// <see cref="CCNetConfig.Core.Enums.BuildCondition">Build Condition</see> will be 
  /// <see cref="CCNetConfig.Core.Enums.BuildCondition.ForceBuild">ForceBuild</see> if any <see cref="CCNetConfig.Core.Trigger">Trigger</see> returns a 
  /// <see cref="CCNetConfig.Core.Enums.BuildCondition.ForceBuild">ForceBuild</see> condition. Otherwise, the 
  /// <see cref="CCNetConfig.Core.Enums.BuildCondition">Build Condition</see> will be 
  /// <see cref="CCNetConfig.Core.Enums.BuildCondition.IfModificationExists">IfModificationExists</see> if any trigger returns that condition. 
  /// <see cref="CCNetConfig.CCNet.MultiTrigger">Multiple Trigger</see>s can contain nested <see cref="CCNetConfig.CCNet.MultiTrigger">Multiple Trigger</see>s.</para>
  /// <para>It is possible to change the logical <see cref="CCNetConfig.Core.Enums.AndOr">operator</see> applied to assessing the 
  /// <see cref="CCNetConfig.Core.Enums.BuildCondition">Build Condition</see>s. If the <see cref="CCNetConfig.CCNet.MultiTrigger">Multiple Trigger</see>'s 
  /// <see cref="CCNetConfig.Core.Enums.AndOr">operator</see> property is set to "<see cref="CCNetConfig.Core.Enums.AndOr.And">And</see>" then
  /// if any <see cref="CCNetConfig.Core.Trigger">Trigger</see> says that a build should not happen, then the build will not happen. 
  /// This is particularly useful when using multiple <see cref="CCNetConfig.CCNet.FilterTrigger">Filter Trigger</see>s.</para>
  /// </summary>
  /// <seealso cref="CCNetConfig.Core.Trigger"/>
  /// <seealso cref="CCNetConfig.Core.Enums.BuildCondition"/>
  /// <seealso cref="CCNetConfig.Core.Enums.AndOr"/>
  [ MinimumVersion( "1.0" ) ]
  public class MultiTrigger : Trigger, ICCNetDocumentation {
    private Core.Enums.AndOr? _andOr = null;
    private CloneableList<Trigger> _triggers;
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiTrigger"/> class.
    /// </summary>
    public MultiTrigger() : base("multiTrigger") {
      _triggers = null;
    }

    /// <summary>
    /// Gets or sets the logical operator to apply to the results of the nested triggers. (Added in CCNet 1.1)
    /// </summary>
    [Description ("Gets or sets the logical operator to apply to the results of the nested triggers. (Added in CCNet 1.1)"),
      Editor (typeof (DefaultableEnumUIEditor), typeof (UITypeEditor)),
     TypeConverter ( typeof ( DefaultableEnumTypeConverter ) ), DefaultValue ( null ), Category ( "Optional" )]
    public Core.Enums.AndOr? Operator { get { return this._andOr; } set { this._andOr = value; } }
    /// <summary>
    /// A collection of nested <see cref="CCNetConfig.Core.Trigger">Trigger</see>s.
    /// </summary>
    [Editor(typeof(TriggerListUIEditor),typeof(UITypeEditor)), DefaultValue(null),
      TypeConverter (typeof (TriggerListTypeConverter)), Description ("A collection of nested Triggers."),Category("Optional")]
    public CloneableList<Trigger> Triggers { get { return this._triggers; } set { this._triggers = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override Trigger Clone () {
      MultiTrigger mt = this.MemberwiseClone () as MultiTrigger;
      mt.Triggers = this.Triggers.Clone ();
      return mt;
    }

    /// <summary>
    /// Serializes the object to a <see cref="System.Xml.XmlElement" />
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize() {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement (this.TypeName);
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      if ( this.Operator.HasValue )
        root.SetAttribute ("operator", this.Operator.Value.ToString ());

      if ( this.Triggers != null && this.Triggers.Count > 0 ) {
        XmlElement eleT = doc.CreateElement ("triggers");
        foreach ( Trigger t in this.Triggers ) {
          XmlElement trigEle = (XmlElement)doc.ImportNode (t.Serialize (),true);
          eleT.AppendChild (trigEle);
        }
        root.AppendChild (eleT);
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
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Multiple+Trigger?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.Triggers = new CloneableList<Trigger> ();
      this.Operator = null;

      if ( string.Compare (element.Name, this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.Name, this.TypeName));

      if ( !string.IsNullOrEmpty (element.GetAttribute ("operator")) ) {
        Core.Enums.AndOr ao = (Core.Enums.AndOr)Enum.Parse (typeof (Core.Enums.AndOr), element.GetAttribute ("operator"),true);
        this.Operator = ao;
      }

      XmlElement ele = (XmlElement)element.SelectSingleNode ("triggers");
      if ( ele != null ) {
        foreach ( XmlElement sele in ele.SelectNodes ("./*") ) {
          Trigger t = Util.GetTriggerFromElement (sele);
          if ( t != null )
            this.Triggers.Add (t);
        }
      }

    }
  }
}
