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
  /// <para>The <see cref="CCNetConfig.CCNet.FilterTrigger">Filter Trigger</see> allows you to prevent builds from occurring at certain times 
  /// (such as when your source control repository is undergoing backup). It is used to decorate an existing <see cref="CCNetConfig.Core.Trigger">Trigger</see>. 
  /// For example, if you have set up a <see cref="CCNetConfig.CCNet.IntervalTrigger">Interval Trigger</see>Interval Trigger to cause a new build every 
  /// 5 minutes, you can use the <see cref="CCNetConfig.CCNet.FilterTrigger">Filter Trigger</see> to create a window during which the build will not run.</para>
  /// <para>If the start time is greater than the end time then the filtered time will span across midnight. For example, if the start time is 
  /// 23:00 and the end time is 3:00 then builds will be suppressed over this interval. </para>
  /// </summary>
  /// <seealso cref="CCNetConfig.Core.Trigger"/>
  /// <seealso cref="CCNetConfig.CCNet.IntervalTrigger"/>
  /// <seealso cref="System.DayOfWeek"/>
  /// <seealso cref="CCNetConfig.Core.Enums.BuildCondition"/>
  [ MinimumVersion( "1.0" ) ]
  public class FilterTrigger : Trigger, ICCNetDocumentation {
    private Trigger _trigger = null;
    private TimeSpan? _startTime;
    private TimeSpan? _endTime;
    private CloneableList<DayOfWeek> _weekDays;
    private Core.Enums.BuildCondition? _buildCondition;
    /// <summary>
    /// Initializes a new instance of the <see cref="FilterTrigger"/> class.
    /// </summary>
    public FilterTrigger ()
      : base ( "filterTrigger" ) {
      _weekDays = null;
      _startTime = new TimeSpan(0,0,0);
      _endTime = new TimeSpan ( 23, 59, 59 );
    }

    /// <summary>
    /// <para>The start of the filter window. Builds will not occur after this time and before the <see cref="CCNetConfig.CCNet.FilterTrigger.EndTime">EndTime</see>.</para>
    /// </summary>
    [Description ( "The start of the filter window. Builds will not occur after this time and before the EndTime." ),
      DisplayName ( "(StartTime)" ), Editor ( typeof ( TimeUIEditor ), typeof ( UITypeEditor ) ),
    TypeConverter ( typeof ( TimeTypeConverter ) ), DefaultValue ( "00:00:00" ), Category ( "Required" )]
    public TimeSpan StartTime { get { return this._startTime.Value; } set { this._startTime = Util.CheckRequired ( this, "startTime", value ); } }
    /// <summary>
    /// <para>The end of the filter window. Builds will not occur before this time and after the <see cref="CCNetConfig.CCNet.FilterTrigger.StartTime">StartTime</see>.</para>
    /// </summary>
    [Description ( "The end of the filter window. Builds will not occur before this time and after the StartTime." ),
   DisplayName ( "(EndTime)" ), Editor ( typeof ( TimeUIEditor ), typeof ( UITypeEditor ) ), DefaultValue ( "23:59:59" ), Category ( "Required" )]
    public TimeSpan EndTime { get { return this._endTime.Value; } set { this._endTime = Util.CheckRequired ( this, "endTime", value ); } }
    /// <summary>
    /// The <see cref="CCNetConfig.Core.Trigger">Trigger</see> to filter.
    /// </summary>
    [Editor ( typeof ( TriggerSelectorUIEditor ), typeof ( UITypeEditor ) ),
    Description ( "The trigger to filter." ),
  TypeConverter ( typeof ( ExpandableObjectConverter ) ), DefaultValue ( null ), Category ( "Optional" )]
    public Trigger Trigger { get { return this._trigger; } set { this._trigger = value; } }
    /// <summary>
    /// The week days on which the filter should be applied (eg. Monday, Tuesday). By default, all days of the week are set.
    /// </summary>
    [Description ( "The week days on which the filter should be applied (eg. Monday, Tuesday). By default, all days of the week are set." ),
    Editor ( typeof ( DayOfWeekUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DayOfWeekListTypeConverter ) ), DefaultValue ( null ), Category ( "Optional" )]
    public CloneableList<DayOfWeek> WeekDays { get { return this._weekDays; } set { this._weekDays = value; } }
    /// <summary>
    /// <para>The condition that will be returned if a build is requested during the filter window. The default value is NoBuild indicating that no build will be performed.</para>
    /// </summary>
    [Description ( "The condition that will be returned if a build is requested during the filter window. The default value is NoBuild indicating that no build will be performed." ),
   Editor ( typeof ( DefaultableEnumUIEditor ), typeof ( UITypeEditor ) ), 
    TypeConverter(typeof(DefaultableEnumTypeConverter)),
   DefaultValue ( null ), Category ( "Optional" )]
    public Core.Enums.BuildCondition? BuildCondition { get { return this._buildCondition; } set { this._buildCondition = value; } }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    /// <returns></returns>
    public override Trigger Clone () {
      FilterTrigger ft = this.MemberwiseClone () as FilterTrigger;
      ft.EndTime = TimeSpan.Parse ( this.EndTime.ToString () );
      ft.StartTime = TimeSpan.Parse ( this.StartTime.ToString () );
      ft.WeekDays = this.WeekDays.Clone ();
      return ft;
    }


    /// <summary>
    /// Serializes the object to a <see cref="System.Xml.XmlElement" />
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize () {
      XmlDocument doc = new XmlDocument ();
      XmlElement root = doc.CreateElement ( this.TypeName );
      //root.SetAttribute ("ccnetconfigType", string.Format ("{0}, {1}", this.GetType ().FullName, this.GetType ().Assembly.GetName ().Name));

      root.SetAttribute ( "startTime", Util.CheckRequired ( this, "startTime", this.StartTime ).ToString () );
      root.SetAttribute ( "endTime", Util.CheckRequired ( this, "endTime", this.EndTime ).ToString () );
      XmlElement ele = null;
      if ( this.Trigger != null ) {
        ele = doc.CreateElement ( "trigger" );
        ele.SetAttribute ( "type", this.Trigger.TypeName );
        XmlElement tele = Trigger.Serialize ();
        foreach ( XmlAttribute attrib in tele.Attributes )
          ele.SetAttribute ( attrib.Name, attrib.InnerText );

        foreach ( XmlElement cele in tele.SelectNodes ( "./*" ) ) {
          XmlElement nele = (XmlElement)doc.ImportNode ( cele, true );
          ele.AppendChild ( nele );
        }
        root.AppendChild ( ele );
      }

      if ( this.WeekDays != null && this.WeekDays.Count > 0 ) {
        XmlElement edow = doc.CreateElement ( "weekDays" );
        foreach ( DayOfWeek dow in this.WeekDays ) {
          ele = doc.CreateElement ( "weekDay" );
          ele.InnerText = dow.ToString ();
          edow.AppendChild ( ele );
        }
        root.AppendChild ( edow );
      }

      if ( this.BuildCondition.HasValue ) {
        ele = doc.CreateElement ( "buildCondition" );
        ele.InnerText = this.BuildCondition.Value.ToString ();
        root.AppendChild ( ele );
      }
      return root;
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/Filter+Trigger?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize( XmlElement element ) {
      this.Trigger = null;
      this.WeekDays = new CloneableList<DayOfWeek> ();

      if( string.Compare(element.Name,this.TypeName,false) != 0 )
        throw new InvalidCastException(string.Format("Unable to convert {0} to a {1}",element.Name,this.TypeName));

      TimeSpan ts = new TimeSpan (0);
      TimeSpan.TryParse (element.GetAttribute ("startTime"), out ts);
      this.StartTime = ts;
      TimeSpan.TryParse (element.GetAttribute ("endTime"), out ts);
      this.EndTime = ts;

      // this is the filter trigger
      XmlElement ele = (XmlElement)element.SelectSingleNode("trigger");
      if ( ele != null ) {
        XmlElement nElement = element.OwnerDocument.CreateElement (ele.GetAttribute ("type"));
        foreach ( XmlAttribute attrib in ele.Attributes )
          nElement.SetAttribute (attrib.Name, attrib.InnerText);
        this.Trigger = Util.GetTriggerFromElement (nElement);
      }

      // get the weekdays
      ele = (XmlElement)element.SelectSingleNode ("weekDays");
      if ( ele != null ) {
        foreach ( XmlElement wde in ele.SelectNodes ("weekDay") ) {
          DayOfWeek dow = (DayOfWeek)Enum.Parse (typeof (DayOfWeek), wde.InnerText,true);
          this.WeekDays.Add (dow);
        }
      }

      // get the build condition
      ele = (XmlElement)element.SelectSingleNode ("buildCondition");
      if ( ele != null ) {
        Core.Enums.BuildCondition bc = (Core.Enums.BuildCondition)Enum.Parse (typeof (Core.Enums.BuildCondition), ele.InnerText,true);
        this.BuildCondition = bc;
      }
    }
  }
}
