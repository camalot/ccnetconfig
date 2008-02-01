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
using System.Windows.Forms;
using CCNetConfig.Core;
using System.IO;
using System.Configuration;
using System.Net;
using CCNetConfig.BugTracking;
using CCNetConfig.BugTracking.Configuration.Handlers;
using System.Xml;
using CCNetConfig.Core.Configuration;
using CCNetConfig.Updater.Core;
using CCNetConfig.UI;
namespace CCNetConfig {
/// <summary>
/// The static class that starts the application
/// </summary>
  public static class Program {

    private static CCNetConfigConfiguration _configuration = null;
    private static BugTracker _bugTracker = null;
    private static CommandLineArguments arguments = null;
    /// <summary>
    /// Start the application
    /// </summary>
    /// <param name="args">The args.</param>
    [STAThread()]
    static void Main ( string[] args ) {
      try {
        Application.SetUnhandledExceptionMode ( UnhandledExceptionMode.CatchException, false );
      } catch { }

      Application.EnableVisualStyles ();
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler ( Program.OnUnhandledException );

      arguments = new CommandLineArguments ( args );
      MainForm form = new MainForm ();
      try {
        Application.Run ( form );
      } catch ( Exception ex ) {
        XmlDocument doc = null;
        if (form != null && !form.IsDisposed )
          doc = CruiseControlToXmlDocument ( form.CruiseControl );
        Program.BugTracker.SubmitExceptionDialog ( null, ex, doc );
      }
    }

    /// <summary>
    /// Called when unhandled exception occurs.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
    private static void OnUnhandledException ( object sender, UnhandledExceptionEventArgs e ) {
      Exception ex = e.ExceptionObject as Exception;
      Program.BugTracker.SubmitExceptionDialog ( null, ex, null );
      //Environment.Exit ( 1 );
    }

    /// <summary>
    /// Gets the bug tracker.
    /// </summary>
    /// <value>The bug tracker.</value>
    public static BugTracker BugTracker {
      get {
        if ( _bugTracker == null ) {
          BugTrackingConfigurationSectionHandler btcsh = new BugTrackingConfigurationSectionHandler ();
          XmlDocument bugTrackerDoc = new XmlDocument ();
          bugTrackerDoc.Load (Path.Combine (Application.StartupPath, Program.Configuration["BugTracking"].Path));
          _bugTracker = btcsh.Create (null, null, bugTrackerDoc.DocumentElement);
        }

        return _bugTracker;
      }
    }

    /*public static CCNetConfigConfiguration Configuration {
      get {
        return CCNetConfig.Core.Util.Configuration;
      }
    }*/

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    /// <value>The configuration.</value>
    public static CCNetConfigConfiguration Configuration {
      get {
        if ( _configuration == null )
          _configuration = (CCNetConfigConfiguration)ConfigurationManager.GetSection ( "CCNetConfigConfiguration" );
        return _configuration;
      }
    }

    /// <summary>
    /// Gets the command line arguments.
    /// </summary>
    /// <value>The command line arguments.</value>
    public static CommandLineArguments CommandLineArguments { get { return arguments; } }

    /// <summary>
    /// Cruises the control to XML document.
    /// </summary>
    /// <param name="cc">The cc.</param>
    /// <returns></returns>
    public static XmlDocument CruiseControlToXmlDocument ( CruiseControl cc ) {
      XmlDocument tdoc = new XmlDocument ();
      try {
        if ( cc != null ) {
          XmlElement ele = cc.Serialize ();
          if ( ele != null )
            tdoc.ImportNode ( ele, true );
        }
      } catch { }
      return tdoc;
    }
  }
}
