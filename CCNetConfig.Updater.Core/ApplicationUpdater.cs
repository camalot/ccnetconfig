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
using CCNetConfig.Updater.Core.Configuration;
using System.Configuration;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Net;
using CCNetConfig.Updater.Core.Configuration.Handlers;
using CCNetConfig.Core.Enums;
using System.Threading;

namespace CCNetConfig.Updater.Core {
  /// <summary>
  /// Performs the update process
  /// </summary>
  public class ApplicationUpdater {
    /// <summary>
    /// Raised when then update download completes.
    /// </summary>
    public event EventHandler UpdateDownloadsCompleted;
    /// <summary>
    /// Raised when updates are available
    /// </summary>
    public event EventHandler<UpdatesAvailableEventArgs> UpdateAvailable;
    /// <summary>
    /// Raised when there is an exception durring the update process
    /// </summary>
    public event EventHandler<ExceptionEventArgs> UpdateException;
    /// <summary>
    /// Raised when the progress of the update download changes.
    /// </summary>
    public event EventHandler<DownloadProgressEventArgs> DownloadProgressChanged;
    /// <summary>
    /// Raised when the script that unzips the files is created.
    /// </summary>
    public event EventHandler UpdateScriptCreated;
    private UpdaterConfiguration _configuration = null;
    private XmlSerializer _serializer = null;
    private List<FileInfo> _tempFiles = null;

    private Version _version = null;
    private FileInfo _ownerApplication = null;
    private FileInfo _scriptFile = null;

    private bool _updatesAvailable = false;

    private long _totalBytesReceived = 0;
    private UpdateInfoList _updateList = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationUpdater"/> class.
    /// </summary>
    public ApplicationUpdater () {
      try {
        XmlDocument doc = new XmlDocument ();
        string startUpPath = this.GetType ().Assembly.Location;
        startUpPath = startUpPath.Substring ( 0, startUpPath.LastIndexOf ( "\\" ) );
        doc.Load ( Path.Combine ( startUpPath, Properties.Strings.UpdaterConfigurationFile ) );
        UpdaterConfigurationSectionHandler ucsh = new UpdaterConfigurationSectionHandler ();
        _configuration = ucsh.Create ( null, null, doc.DocumentElement );
      } catch ( Exception ex ) {
        throw new ApplicationException ( Properties.Strings.UpdaterConfigLoadErrorMessage, ex );
      }
      _updateList = new UpdateInfoList ();
      _serializer = new XmlSerializer ( typeof ( UpdateInfo ) );
      _tempFiles = new List<FileInfo> ();
    }

    /// <summary>
    /// Checks for update.
    /// </summary>
    /// <param name="feedType">Type of the feed.</param>
    public void CheckForUpdate ( UpdateCheckType feedType ) {
      switch ( feedType ) {
        case UpdateCheckType.BetaBuilds:
          CheckForUpdatesByUrl ( _configuration.BetaBuildsUpdateInfoUri );
          break;
        case UpdateCheckType.AllBuilds:
          CheckForUpdatesByUrl ( _configuration.AllBuildsUpdateInfoUri );
          break;
        case UpdateCheckType.ReleaseBuilds:
        default:
          CheckForUpdatesByUrl ( _configuration.ReleaseBuildUpdateInfoUri );
          break;
      }
    }

    /// <summary>
    /// Checks for update.
    /// </summary>
    public void CheckForUpdate () {
      CheckForUpdate ( UpdateCheckType.ReleaseBuilds );
    }

    /// <summary>
    /// Checks the update via RSS.
    /// </summary>
    /// <param name="uri">The URI.</param>
    private void CheckUpdateViaRss ( Uri uri ) {
      _updateList = new UpdateInfoList ();
      XmlDocument doc = new XmlDocument ();
      try {
        Stream strm = GetRssStream ( uri );
        using ( strm ) {
          doc.Load ( strm );
        }
      } catch { throw;  }
      XmlNamespaceManager nsm = CreateXmlNamespaceManager ( doc );

      XmlNodeList updateInfoNodes = doc.SelectNodes ( string.Format(Properties.Strings.UpdateInfoRootXPath, Properties.Strings.UpdateInfoNamespacePrefix), nsm );
      // Console.WriteLine ( "{0} Update info items found", updateInfoNodes.Count );

      foreach ( XmlElement uie in updateInfoNodes ) {
        try {
          XmlParserContext xpc = new XmlParserContext ( doc.NameTable, nsm, string.Empty, XmlSpace.Preserve );
          XmlTextReader reader = new XmlTextReader ( uie.OuterXml, XmlNodeType.Element, xpc );

          XmlSerializer serializer = new XmlSerializer ( typeof ( UpdateInfo ), nsm.LookupNamespace ( Properties.Strings.UpdateInfoNamespacePrefix ) );
          _updateList.Add ( serializer.Deserialize ( reader ) as UpdateInfo );
        } catch ( Exception ex ) {
          Console.WriteLine ( ex.ToString () );
        }
      }

      _updateList.Sort ( new UpdateInfoVersionComparer () );
      if ( _updateList.Count > 0 ) {
        UpdateInfo _updateInfo = UpdateInfoList[UpdateInfoList.GetLatestVersion(  )];
        foreach ( UpdateFile uf in _updateInfo.Files ) {
          if ( uf.Version.CompareTo ( this.Version ) > 0 )
            _updatesAvailable = true;
        }
        if ( _updatesAvailable || _updateInfo.Version.CompareTo ( this.Version ) > 0 )
          OnUpdateAvailable ( new UpdatesAvailableEventArgs ( _updateInfo.Version ) );
      }
    }

    /// <summary>
    /// Gets the RSS stream.
    /// </summary>
    /// <param name="uri">The URI.</param>
    /// <returns></returns>
    private Stream GetRssStream ( Uri uri ) {
      HttpWebRequest req = HttpWebRequest.Create ( uri ) as HttpWebRequest;
      req.Proxy = CCNetConfig.Core.Util.UserSettings.UpdateSettings.ProxySettings.CreateProxy ();
      req.UserAgent = string.Format ( Properties.Strings.UserAgent, this.GetType ().Assembly.GetName ().Version.ToString () );
      HttpWebResponse resp = req.GetResponse () as HttpWebResponse;
      return resp.GetResponseStream ();
    }

    /// <summary>
    /// Creates the XML namespace manager.
    /// </summary>
    /// <param name="doc">The doc.</param>
    /// <returns></returns>
    private XmlNamespaceManager CreateXmlNamespaceManager ( XmlDocument doc ) {
      XmlNamespaceManager nsmgr = new XmlNamespaceManager ( doc.NameTable );
      foreach ( XmlAttribute attr in doc.SelectSingleNode ( "/*" ).Attributes )
        if ( string.Compare ( attr.Prefix, "xmlns" ) == 0 )
          nsmgr.AddNamespace ( attr.LocalName, attr.Value );
      return nsmgr;
    }


    /// <summary>
    /// Gets the updates.
    /// </summary>
    public void GetUpdates () {
      GetUpdates ( UpdateInfoList.GetLatestVersion () );
    }

    /// <summary>
    /// Gets the updates.
    /// </summary>
    /// <param name="version">The version.</param>
    public void GetUpdates ( Version version ) {
      if ( _updatesAvailable && UpdateInfoList.Contains( version ) ) {
        UpdateInfo ui = UpdateInfoList[version];
        foreach ( UpdateFile uf in ui.Files ) {
          if ( Version.CompareTo ( uf.Version ) < 0 )
            DownloadFile ( uf );
        }
        OnUpdateDownloadsCompleted ( EventArgs.Empty );
        buildScript ( ui );
        OnUpdateScriptCreated ( EventArgs.Empty );
      }
    }

    /// <summary>
    /// Builds the script.
    /// </summary>
    private void buildScript (UpdateInfo updateInfo) {
      try {
        _scriptFile = new FileInfo ( Path.Combine ( this.OwnerApplication.Directory.FullName, "update.bat" ) );
        //_scriptFile = new FileInfo ( Path.Combine ( 
        //    Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ), @"CCNetConfig\update.bat") );
        if ( !_scriptFile.Directory.Exists )
          _scriptFile.Directory.Create ( );

        string cmd = "unzip.exe -o \"{0}\" -d \"{1}\" >> update.log";
        string outPath = _scriptFile.Directory.FullName;
        if ( _scriptFile.Exists )
          _scriptFile.Delete ();

        FileStream fs = new FileStream ( _scriptFile.FullName, FileMode.Create, FileAccess.Write );
        StreamWriter sw = new StreamWriter ( fs );
        using ( fs ) {
          using ( sw ) {
            sw.WriteLine ( "@echo off" );
            //sw.WriteLine ( "sleep 2" );
            foreach ( FileInfo uf in this._tempFiles ) {
              sw.WriteLine ( string.Format ( cmd, uf.FullName, "." ) );
            }

            foreach ( string command in updateInfo.Commands )
              sw.WriteLine ( command );

            // if owner app set write it to open.
            if ( this.OwnerApplication != null ) {
              string format = "start {0}";
              if ( this.OwnerApplication.Name.Trim ().IndexOf ( " " ) > 0 )
                format = "start \"{0}\"";
              sw.WriteLine ( format, this.OwnerApplication.Name );
            }

          }
        }
      } catch ( Exception ex ) {
        OnUpdateException ( new ExceptionEventArgs ( ex ) );
      }
    }

    /// <summary>
    /// Downloads the file.
    /// </summary>
    /// <param name="uf">The uf.</param>
    private void DownloadFile ( UpdateFile uf ) {
      _totalBytesReceived = 0;
      HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create ( uf.Location );
      req.Proxy = CCNetConfig.Core.Util.UserSettings.UpdateSettings.ProxySettings.CreateProxy ();
      req.UserAgent = string.Format ( Properties.Strings.UserAgent, this.GetType ().Assembly.GetName ().Version.ToString () );
      HttpWebResponse resp = (HttpWebResponse)req.GetResponse ();
      
      string dispHeader = resp.GetResponseHeader ( "Content-Disposition" );

      string fileName = Path.GetFileName ( uf.Location.LocalPath );
      if ( !string.IsNullOrEmpty ( dispHeader ) ) {
        string[] ts = dispHeader.Split ( new char[] { '=' }, 2 );
        if ( ts.Length == 2 && !string.IsNullOrEmpty ( ts[1] ) )
          fileName = ts[1];
      }
      if ( resp.ContentLength > 0 )
        uf.Size = resp.ContentLength;
      FileInfo tempFile = new FileInfo ( Path.Combine ( _configuration.TempDirectory.FullName, fileName ) );
      Stream strm = resp.GetResponseStream ();
      byte[] buffer = new byte[1024];
      int i = 0;
      FileStream fs = new FileStream ( tempFile.FullName, FileMode.CreateNew, FileAccess.Write );
      using ( fs ) {
        using ( strm ) {
          while ( ( i = strm.Read ( buffer, 0, buffer.Length ) ) > 0 ) {
            DownloadProgressEventArgs dev = new DownloadProgressEventArgs ();
            dev.BytesReceived = i;
            _totalBytesReceived += i;
            dev.TotalBytesReceived = _totalBytesReceived;
            dev.UpdateFile = uf;
            OnDownloadProgressChangedEventArgs ( dev );
            fs.Write ( buffer, 0, i );
          }
        }
      }
      _tempFiles.Add ( tempFile );
    }

    /// <summary>
    /// Checks for updates by URL.
    /// </summary>
    /// <param name="feed">The feed.</param>
    private void CheckForUpdatesByUrl ( Uri feed ) {
      try {
        CheckUpdateViaRss ( feed );
      } catch ( Exception ex ) {
        OnUpdateException ( new ExceptionEventArgs ( ex ) );
      }
    }

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>The version.</value>
    public Version Version { get { return this._version; } set { this._version = value; } }
    /// <summary>
    /// Gets or sets the owner application.
    /// </summary>
    /// <value>The owner application.</value>
    public FileInfo OwnerApplication { get { return this._ownerApplication; } set { this._ownerApplication = value; } }
    /// <summary>
    /// Gets a value indicating whether [updates available].
    /// </summary>
    /// <value><c>true</c> if [updates available]; otherwise, <c>false</c>.</value>
    public bool UpdatesAvailable { get { return this._updatesAvailable; } }
    /// <summary>
    /// Gets the script file.
    /// </summary>
    /// <value>The script file.</value>
    public FileInfo ScriptFile { get { return this._scriptFile; } }

    /// <summary>
    /// Gets the update info list.
    /// </summary>
    /// <value>The update info list.</value>
    public UpdateInfoList UpdateInfoList {
      get { return this._updateList; }
    }
    /// <summary>
    /// Raises the <see cref="E:UpdateException"/> event.
    /// </summary>
    /// <param name="e">The <see cref="CCNetConfig.Updater.Core.ExceptionEventArgs"/> instance containing the event data.</param>
    protected void OnUpdateException ( ExceptionEventArgs e ) {
      if ( this.UpdateException != null )
        this.UpdateException ( this, e );
    }

    /// <summary>
    /// Raises the <see cref="E:UpdateAvailable"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void OnUpdateAvailable ( UpdatesAvailableEventArgs e ) {
      if ( this.UpdateAvailable != null )
        this.UpdateAvailable ( this, e );
    }

    /// <summary>
    /// Raises the <see cref="E:UpdateDownloadsCompleted"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void OnUpdateDownloadsCompleted ( EventArgs e ) {
      if ( this.UpdateDownloadsCompleted != null )
        this.UpdateDownloadsCompleted ( this, e );
    }

    /// <summary>
    /// Raises the <see cref="E:DownloadProgressChangedEventArgs"/> event.
    /// </summary>
    /// <param name="e">The <see cref="CCNetConfig.Updater.Core.DownloadProgressEventArgs"/> instance containing the event data.</param>
    protected void OnDownloadProgressChangedEventArgs ( DownloadProgressEventArgs e ) {
      if ( DownloadProgressChanged != null )
        this.DownloadProgressChanged ( this, e );
    }

    /// <summary>
    /// Raises the <see cref="E:LaunchExtract"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void OnUpdateScriptCreated ( EventArgs e ) {
      if ( UpdateScriptCreated != null )
        this.UpdateScriptCreated ( this, e );
    }
  }
}
