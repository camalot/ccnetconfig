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
using System.IO;
using System.Reflection;
using System.Net;
using System.Xml;
using CCNetConfig.BugTracking.org.ccnetconfig.services;

namespace CCNetConfig.BugTracking {
  /// <summary>
  /// Class represents information used to connect to the bug tracker.
  /// </summary>
  public class BugTracker {
    private bool _enabled = false;
    private int _projectId = 0;
    private Dictionary<string, Category> _categories;
    private Uri _serviceUri = null;
    private WebProxy _proxy = null;
    private string _tfsServer;



    /// <summary>
    /// Event raised after the submission of the bug is complete.
    /// </summary>
    public event EventHandler SubmissionCompleted;
    /// <summary>
    /// Initializes a new instance of the <see cref="BugTracker"/> class.
    /// </summary>
    public BugTracker() {
      _categories = new Dictionary<string, Category> ();
    }

    /// <summary>
    /// Gets or sets the TFS server.
    /// </summary>
    /// <value>The TFS server.</value>
    public string TfsServer {
      get { return _tfsServer; }
      set { _tfsServer = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="BugTracker"/> is enabled.
    /// </summary>
    /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
    public bool Enabled { get { return this._enabled; } set { this._enabled = value; } }
    /// <summary>
    /// Gets the categories.
    /// </summary>
    /// <value>The categories.</value>
    public Dictionary<string,Category> Categories { get { return this._categories; } }
    /// <summary>
    /// Gets or sets the project id.
    /// </summary>
    /// <value>The project id.</value>
    public int ProjectId { get { return this._projectId; } set { this._projectId = value; } }
    /// <summary>
    /// Gets or sets the service URI.
    /// </summary>
    /// <value>The service URI.</value>
    public Uri ServiceUri { get { return this._serviceUri; } set { this._serviceUri = value; } }
    /// <summary>
    /// Gets or sets the proxy.
    /// </summary>
    /// <value>The proxy.</value>
    public WebProxy Proxy { get { return this._proxy; } set { this._proxy = value; } }
    /// <summary>
    /// Represents a category for the bug to be reported.
    /// </summary>
    public class Category {
      private string _name = string.Empty;
      private int _id = 0;
      private string _assembly = string.Empty;
      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      public string Name { get { return this._name; } set { this._name = value; } }
      /// <summary>
      /// Gets or sets the id.
      /// </summary>
      /// <value>The id.</value>
      public int Id { get { return this._id; } set { this._id = value; } }
      /// <summary>
      /// Gets or sets the assembly.
      /// </summary>
      /// <value>The assembly.</value>
      public string Assembly { get { return this._assembly; } set { this._assembly = value; } }
    }

    /// <summary>
    /// Shows the submit bug dialog
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <returns></returns>
    public DialogResult SubmitBugDialog( IWin32Window owner ) {
      return new SubmitBugDialog (this).ShowDialog (owner);
    }

    /// <summary>
    /// Submit exception dialog.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="ex">The ex.</param>
    /// <param name="configDoc">The config doc.</param>
    /// <returns></returns>
    public DialogResult SubmitExceptionDialog ( IWin32Window owner, Exception ex, XmlDocument configDoc ) {
      CCNetConfig.BugTracking.SubmitException submitException = new CCNetConfig.BugTracking.SubmitException ( this, ex, configDoc );
      return submitException.ShowDialog ( owner );
    }

    /// <summary>
    /// Submits the bug.
    /// </summary>
    /// <param name="item">The item.</param>
    public void SubmitBug( org.ccnetconfig.services.TfsWorkItem item ) {
      org.ccnetconfig.services.BugTracker webService = new org.ccnetconfig.services.BugTracker ( );
      Assembly asm = Assembly.GetCallingAssembly ();
      webService.Proxy = this.Proxy;
      webService.UserAgent = string.Format ( "{0} v{1}",asm.GetName().Name, asm.GetName().Version.ToString() );
      webService.Url = this.ServiceUri.ToString();
      webService.CookieContainer = new System.Net.CookieContainer ();
      webService.CreateWorkItemCompleted += new CCNetConfig.BugTracking.org.ccnetconfig.services.CreateWorkItemCompletedEventHandler ( webService_CreateWorkItemCompleted );
      webService.CreateWorkItemAsync ( Properties.Resources.WebServiceApplicationId, item );
    }

    /// <summary>
    /// Handles the CreateWorkItemCompleted event of the webService control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/> instance containing the event data.</param>
    void webService_CreateWorkItemCompleted ( object sender, System.ComponentModel.AsyncCompletedEventArgs e ) {
      OnSubmissionCompleted ( EventArgs.Empty );
    }

    /// <summary>
    /// Submits the bug.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="asm">The asm.</param>
    public void SubmitBug( string title, string description, /*FileInfo attachedFile,*/ Assembly asm ) {
      org.ccnetconfig.services.TfsWorkItem workItem = new org.ccnetconfig.services.TfsWorkItem ( );
      workItem.TfsServer = this.TfsServer;
      workItem.ProjectId = this.ProjectId;
      workItem.Description = description;
      workItem.Title = title;
      workItem.WorkItemTypeName = "Work Item";

      List<TfsField> fields = new List<TfsField>(  );
      TfsField f= new CCNetConfig.BugTracking.org.ccnetconfig.services.TfsField(  );
      f.ReferenceName = "CodePlex.UserVotes";
      f.Value = "1";
      fields.Add ( f );

      f = new CCNetConfig.BugTracking.org.ccnetconfig.services.TfsField ( );
      f.ReferenceName = "System.AreaPath";
      f.Value = this.GetCategoryNameByAssembly( asm );
      fields.Add ( f );

      f = new CCNetConfig.BugTracking.org.ccnetconfig.services.TfsField ( );
      f.ReferenceName = "CodeStudio.Rank";
      f.Value = "Medium";
      fields.Add ( f );

      f = new CCNetConfig.BugTracking.org.ccnetconfig.services.TfsField ( );
      f.ReferenceName = "CodeStudio.WorkItemType";
      f.Value = "Issue";
      fields.Add ( f );

      workItem.Fields = fields.ToArray ( );
      SubmitBug (workItem);
    }

    /// <summary>
    /// Raises the <see cref="E:SubmissionCompleted"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OnSubmissionCompleted( EventArgs e ) {
      if ( this.SubmissionCompleted != null )
        this.SubmissionCompleted (this, e);
    }

    private string GetCategoryNameByAssembly( Assembly asm ) {
      if ( asm != null  && Categories.ContainsKey (asm.GetName ().Name) )
        return Categories[asm.GetName ().Name].Name;
      else
        return Properties.Resources.AreaPathDefault;
    }

  }
}
