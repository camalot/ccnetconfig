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
  /// <para>The <see cref="CCNetConfig.CCNet.ScheduleTrigger">Schedule Trigger</see> is used to specify that an integration should be run at a 
  /// certain time on certain days. By default, an integration will only be triggered if modifications have been detected since the last integration. 
  /// The <see cref="CCNetConfig.Core.Trigger">Trigger</see> can be configured to force a build even if no modifications have occurred to source control.</para>
  /// </summary>
  /// <remarks>
  /// <para>NOTE: this class replaces the PollingScheduleTrigger and the ForceBuildScheduleTrigger. Use the <see cref="CCNetConfig.Core.Enums.BuildCondition">BuildCondition</see>
  /// property if you want to run a scheduled forced build.</para>
  /// </remarks>
  /// <seealso cref="CCNetConfig.Core.Trigger"/>
  /// <seealso cref="CCNetConfig.Core.Enums.BuildCondition"/>
  /// <seealso cref="System.DayOfWeek"/>
  [ MinimumVersion( "1.0" ) ]
  public class ScheduleTrigger : Trigger, ICCNetDocumentation {
    private TimeSpan _time;
    private Core.Enums.BuildCondition? _buildCondition = null;
    private CloneableList<DayOfWeek> _weekDays = null;
    private string _name = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduleTrigger"/> class.
    /// </summary>
    public ScheduleTrigger () : base("scheduleTrigger") {
      _time = new TimeSpan (6, 0, 0);
      _weekDays = null;
    }

    /// <summary>
    /// <para>The time of day that the build should run at.</para>
    /// </summary>
    [Editor (typeof (TimeUIEditor), typeof (UITypeEditor)), DefaultValue ("06:00:00"),
   Description ( "The time of day that the build should run at." ), DisplayName ( "(Time)" ), Category ( "Required" )]
    public TimeSpan Time { get { return this._time; } set { this._time = Util.CheckRequired ( this, "time", value ); } }
    /// <summary>
    /// <para>The condition that will be returned if a build is requested during the filter window. The default value is NoBuild indicating that no build will be performed.</para>
    /// </summary>
    [Description ("The condition that will be returned if a build is requested during the filter window. The default value is NoBuild indicating that no build will be performed."),
      Editor (typeof (DefaultableEnumUIEditor), typeof (UITypeEditor)),
      TypeConverter (typeof (DefaultableEnumTypeConverter)),
     DefaultValue ( null ), Category ( "Optional" )]
    public Core.Enums.BuildCondition? BuildCondition { get { return this._buildCondition; } set { this._buildCondition = value; } }
    /// <summary>
    /// The week days on which the build should be run (eg. Monday, Tuesday). By default, all days of the week are set.
    /// </summary>
    [Description("The week days on which the build should be run (eg. Monday, Tuesday). By default, all days of the week are set."),
    Editor (typeof (DayOfWeekUIEditor), typeof (UITypeEditor)),
   TypeConverter ( typeof ( DayOfWeekListTypeConverter ) ), DefaultValue ( null ), Category ( "Optional" )]
    public CloneableList<DayOfWeek> WeekDays { get { return this._weekDays; } set { this._weekDays = value; } }
    /// <summary>
    /// The name of the <see cref="CCNetConfig.Core.Trigger">Trigger</see>. This name is passed to external tools as a means to identify the 
    /// <see cref="CCNetConfig.Core.Trigger">Trigger</see> that requested the build. (Added in CCNet 1.1)
    /// </summary>
    [Description("The name of the Trigger. This name is passed to external tools as a means to identify the Trigger that requested the build. (Added in CCNet 1.1)"),
   DefaultValue ( null ), Category ( "Optional" )]
    public string Name { get { return this._name; } set { this._name = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override Trigger Clone () {
      ScheduleTrigger st = this.MemberwiseClone () as ScheduleTrigger;
      st.WeekDays = this.WeekDays.Clone ();
      st.Time = TimeSpan.Parse ( this.Time.ToString () );
      return st;
    }

    /// <summary>
    /// Serializes the object to a <see cref="System.Xml.XmlElement"/>
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      TimeSpan ts = Util.CheckRequired ( this, "time", this.Time );
      root.SetAttribute ( "time", string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours,ts.Minutes,ts.Seconds) );
      if ( this.BuildCondition.HasValue )
        root.SetAttribute ( "buildCondition", this.BuildCondition.Value.ToString () );
      if ( !string.IsNullOrEmpty ( this.Name ) )
        root.SetAttribute ( "name", this.Name );

      XmlElement ele = null;

      if ( this.WeekDays != null &&  this.WeekDays.Count > 0 ) {
        XmlElement edow = doc.CreateElement ( "weekDays" );
        foreach ( DayOfWeek dow in this.WeekDays ) {
          ele = doc.CreateElement ( "weekDay" );
          ele.InnerText = dow.ToString ();
          edow.AppendChild ( ele );
        }
        root.AppendChild ( edow );
      }
      return root;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable (false)]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Schedule+Trigger?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.WeekDays = new CloneableList<DayOfWeek> ();
      this.BuildCondition = null;

      if ( string.Compare (element.Name, this.TypeName, false) != 0 )
        throw new InvalidCastException (string.Format ("Unable to convert {0} to a {1}", element.Name, this.TypeName));

      TimeSpan ts = new TimeSpan (0,0,0);
      TimeSpan.TryParse (element.GetAttribute ("time"), out ts);
      this.Time = ts;

      if (!string.IsNullOrEmpty(element.GetAttribute("name")))
          this.Name = element.GetAttribute("name");

      if (!string.IsNullOrEmpty(element.GetAttribute("buildCondition")))
      {
        Core.Enums.BuildCondition bc = (Core.Enums.BuildCondition)Enum.Parse (typeof (Core.Enums.BuildCondition), element.GetAttribute ("buildCondition"),true);
        this.BuildCondition = bc;
      }

      XmlElement ele = (XmlElement)element.SelectSingleNode ("weekDays");
      if ( ele != null ) {
        foreach ( XmlElement wde in ele.SelectNodes ("weekDay") ) {
          DayOfWeek dow = (DayOfWeek)Enum.Parse (typeof (DayOfWeek), wde.InnerText,true);
          this.WeekDays.Add (dow);
        }
      }
    }
  }
}
