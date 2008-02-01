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

namespace CCNetConfig.CCNet {
  /// <summary>
  /// This labeller retrieves the last successful integration label for a project using the project's state file. You can use this labeller if you 
  /// have split your build across multiple projects and you want to use a consistent version across all builds.
  /// </summary>
  [TypeConverter (typeof (ExpandableObjectConverter)), 
   MinimumVersion( "1.0" )]
  public class StateFileLabeller : Labeller, ICCNetDocumentation {
    private string _project = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="StateFileLabeller"/> class.
    /// </summary>
    public StateFileLabeller() : base("stateFileLabeller") {

    }
    /// <summary>
    ///  The project to retrieve the label from. 	
    /// </summary>
    [Description ( " The project to retrieve the label from." ), DefaultValue ( null ), DisplayName ( "(Project)" ), Category ( "required" )]
    public string Project { get { return this._project; } set { this._project = Util.CheckRequired ( this, "project", value ); } }

    /// <summary>
    /// creates a copy of the StateFileLabeller
    /// </summary>
    /// <returns></returns>
    public override Labeller Clone () {
      return this.MemberwiseClone(  ) as StateFileLabeller;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize() {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ("labeller");
      root.SetAttribute ("type", this.TypeName);
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      XmlElement ele = doc.CreateElement ("project");
      ele.InnerText = Util.CheckRequired ( this, "project", this.Project );
      root.AppendChild (ele);
      return root;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable(false)]
    public Uri DocumentationUri {
      get { return new Uri ( "http://ccnet.thoughtworks.net/display/CCNET/State+File+Labeller?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      if ( string.Compare (element.GetAttribute("type"), this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.GetAttribute("type"), this.TypeName));

      string s = Util.GetElementOrAttributeValue("project",element);
      if ( !string.IsNullOrEmpty ( s ) )
        this.Project = s;
      
    }
  }
}
