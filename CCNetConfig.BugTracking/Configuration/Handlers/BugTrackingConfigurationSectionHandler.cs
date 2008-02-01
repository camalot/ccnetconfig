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
using System.Configuration;
using System.Xml;

namespace CCNetConfig.BugTracking.Configuration.Handlers {
  /// <summary>
  /// Loads the <see cref="CCNetConfig.BugTracking.BugTracker">BugTracker</see> from a config file.
  /// </summary>
  public class BugTrackingConfigurationSectionHandler : IConfigurationSectionHandler {
    /// <summary>
    /// Creates the <see cref="CCNetConfig.BugTracking.BugTracker">BugTracker</see>.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="configContextm">The config contextm.</param>
    /// <param name="section">The section.</param>
    /// <returns></returns>
    public BugTracker Create( object parent, object configContextm, XmlElement section ) {
      BugTracker bt = new BugTracker ();
      bt.Enabled = string.Compare (section.GetAttribute ("Enabled"), bool.TrueString, true) == 0;
      bt.ServiceUri = new Uri (section.GetAttribute ("ServiceUri"));

      XmlElement ele = (XmlElement)section.SelectSingleNode ("ProjectId");
      if ( ele != null ) {
        int id = 0;
        int.TryParse (ele.GetAttribute ("Value"), out id);
        bt.ProjectId = id;
      }

      ele = ( XmlElement ) section.SelectSingleNode ( "TfsServer" );
      if ( ele != null ) {
        bt.TfsServer = ele.GetAttribute ( "Value" );
      }


      ele = (XmlElement)section.SelectSingleNode ("Categories");
      if ( ele != null ) {
        foreach ( XmlElement tele in ele.SelectNodes ("./*") ) {
          BugTracker.Category cat = new BugTracker.Category ();
          int id = 0;
          int.TryParse (tele.GetAttribute ("Id"), out id);
          cat.Id = id;
          cat.Name = tele.GetAttribute ("Name");
          cat.Assembly = tele.GetAttribute ("Assembly");
          bt.Categories.Add (cat.Name,cat);
        }
      }
      return bt;
    }
    #region IConfigurationSectionHandler Members

    /// <summary>
    /// Creates a configuration section handler.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="configContext">Configuration context object.</param>
    /// <param name="section"></param>
    /// <returns>The created section handler object.</returns>
    object IConfigurationSectionHandler.Create( object parent, object configContext, System.Xml.XmlNode section ) {
      return this.Create (parent, configContext, (XmlElement)section);
    }

    #endregion
  }
}
