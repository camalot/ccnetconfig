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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace CCNetConfig.UI {
  /// <summary>
  /// About CCNetConfig
  /// </summary>
  public partial class AboutForm : Form {
    /// <summary>
    /// Initializes a new instance of the <see cref="AboutForm"/> class.
    /// </summary>
    public AboutForm () {
      InitializeComponent ();
      ProjectInfo pi = null;
      this.nameLabel.Text = string.IsNullOrEmpty ( Application.ProductName ) ? "CCNetConfig" : Application.ProductName;
      this.versionLabel.Text = string.IsNullOrEmpty ( Application.ProductVersion ) ? "0.0.0.0" : Application.ProductVersion;
			this.homeLink.Text += DateTime.Now.Year.ToString ();

      try {
        XmlSerializer ser = new XmlSerializer ( typeof ( ProjectInfo ) );
        FileStream fs = new FileStream ( Path.Combine ( Application.StartupPath, Program.Configuration["Contributors"].Path ), FileMode.Open, FileAccess.Read );
        using ( fs ) {
          pi = ser.Deserialize ( fs ) as ProjectInfo;
        }

        foreach ( Contributor c in pi.Contributors )
          this.lstContributors.Items.Add ( c );

        List<FileInfo> files = new List<FileInfo> ();
        DirectoryInfo tDir = new DirectoryInfo ( Application.StartupPath );
        files.AddRange ( tDir.GetFiles ( "*.dll" ) );
        foreach ( FileInfo fInfo in files ) {
          try {
            Assembly asm = Assembly.LoadFile ( fInfo.FullName );
            if ( asm != null ) 
              lstModules.Items.Add ( new AssemblyInfo ( asm ) );
          } catch { }
        }
        try {
          ser = new XmlSerializer ( typeof ( List<ModuleInfo> ), new XmlRootAttribute ( "AdditionalModules" ) );
          fs = new FileStream ( Path.Combine ( Application.StartupPath, Program.Configuration["AdditionalModules"].Path ), FileMode.Open, FileAccess.Read );
          List<ModuleInfo> modules = new List<ModuleInfo> ();
          using ( fs ) {
            modules = ser.Deserialize ( fs ) as List<ModuleInfo>;
          }

          foreach ( ModuleInfo mod in modules ) {
            lstModules.Items.Add ( mod );
          }
        } catch ( Exception ex1 ) {
          Console.WriteLine ( ex1.ToString () );
        }
      } catch ( Exception ex ) { Console.WriteLine ( ex.ToString () ); }
    }

    /// <summary>
    /// Handles the LinkClicked event of the codePlexLink control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
    private void codePlexLink_LinkClicked ( object sender, LinkLabelLinkClickedEventArgs e ) {
      Process.Start ( "http://www.codeplex.com/ccnetconfig" );
    }

    /// <summary>
    /// Handles the LinkClicked event of the homeLink control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
    private void homeLink_LinkClicked ( object sender, LinkLabelLinkClickedEventArgs e ) {
      Process.Start ( "http://ccnetconfig.org/" );
    }

    /// <summary>
    /// Handles the Click event of the okButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void okButton_Click ( object sender, EventArgs e ) {
      this.Close ();
    }

    /// <summary>
    /// Information about the project.
    /// </summary>
    [Serializable, XmlRoot ( "ProjectInfo" )]
    public sealed class ProjectInfo {
      List<Contributor> _contribs = new List<Contributor> ();
      /// <summary>
      /// Gets or sets the contributors.
      /// </summary>
      /// <value>The contributors.</value>
      [XmlArray ( "Contributors" ), XmlArrayItem ( "Contributor" )]
      public List<Contributor> Contributors { get { return this._contribs; } set { this._contribs = value; } }
    }

    /// <summary>
    /// Information about the contributors to the project.
    /// </summary>
    public sealed class Contributor {
      private string _name = string.Empty;
      private string _email = string.Empty;
      private string _blog = string.Empty;
      private string _url = string.Empty;
      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      [XmlElement ( "Name" )]
      public string Name { get { return this._name; } set { this._name = value; } }
      /// <summary>
      /// Gets or sets the email.
      /// </summary>
      /// <value>The email.</value>
      [XmlElement ( "Email" )]
      public string Email { get { return this._email; } set { this._email = value; } }
      /// <summary>
      /// Gets or sets the blog.
      /// </summary>
      /// <value>The blog.</value>
      [XmlElement ( "Blog" )]
      public string Blog { get { return this._blog; } set { this._blog = value; } }
      /// <summary>
      /// Gets or sets the URL.
      /// </summary>
      /// <value>The URL.</value>
      [XmlElement ( "Url" )]
      public string Url { get { return this._url; } set { this._url = value; } }

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return this.Name;
      }
    }

    /// <summary>
    /// Represents information about a module or code that is not assembly or located in the application directory.
    /// </summary>
    [XmlRoot ( "ModuleInfo" )]
    public sealed class ModuleInfo {
      private string _name = string.Empty;
      private string _company = string.Empty;
      private string _copyright = string.Empty;
      private string _url = string.Empty;
      private string _desc = string.Empty;
      /// <summary>
      /// Initializes a new instance of the <see cref="ModuleInfo"/> class.
      /// </summary>
      public ModuleInfo () {

      }

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      [ XmlAttribute( "Name" ) ]
      public string Name { get { return this._name; } set { this._name = value; } }
      /// <summary>
      /// Gets or sets the company.
      /// </summary>
      /// <value>The company.</value>
      [XmlAttribute ( "Company" )]
      public string Company { get { return this._company; } set { this._company = value; } }
      /// <summary>
      /// Gets or sets the copyright.
      /// </summary>
      /// <value>The copyright.</value>
      [XmlElement ( "Copyright" )]
      public string Copyright { get { return this._copyright; } set { this._copyright = value; } }
      /// <summary>
      /// Gets or sets the URL.
      /// </summary>
      /// <value>The URL.</value>
      [XmlElement ( "Url" )]
      public string Url { get { return this._url; } set { this._url = value; } }

      /// <summary>
      /// Gets or sets the description.
      /// </summary>
      /// <value>The description.</value>
    [ XmlElement( "Description" ) ]
      public string Description { get { return this._desc; } set { this._desc = value; } }
      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return string.Format ( "{0} - {1}", this.Name, this.Company );
      }

    }
    /// <summary>
    /// Information about the assemblies that are part of the application.
    /// </summary>
    public sealed class AssemblyInfo {
      private string _name = string.Empty;
      private string _version = string.Empty;
      private string _company = string.Empty;
      private string _copyright = string.Empty;

      private Assembly _assembly = null;

      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyInfo"/> class.
      /// </summary>
      /// <param name="assembly">The assembly.</param>
      public AssemblyInfo (Assembly assembly ) {
        this._assembly = assembly;
        this._name = _assembly.GetName ().Name;
        this._version = _assembly.GetName ().Version.ToString ();

        AssemblyCompanyAttribute[] aca = (AssemblyCompanyAttribute[])_assembly.GetCustomAttributes ( typeof ( AssemblyCompanyAttribute ),true );
        if ( aca != null && aca.Length > 0 )
          this._company = aca[0].Company;

        AssemblyCopyrightAttribute[] acra = (AssemblyCopyrightAttribute[])_assembly.GetCustomAttributes ( typeof ( AssemblyCopyrightAttribute ), true );
        if ( acra != null && acra.Length > 0 )
          this._copyright = acra[0].Copyright;
      }

      /// <summary>
      /// Gets the name.
      /// </summary>
      /// <value>The name.</value>
      public string Name { get { return this._name; } }
      /// <summary>
      /// Gets the version.
      /// </summary>
      /// <value>The version.</value>
      public string Version { get { return this._version; } }
      /// <summary>
      /// Gets the company.
      /// </summary>
      /// <value>The company.</value>
      public string Company { get { return this._company; } }
      /// <summary>
      /// Gets the copyright.
      /// </summary>
      /// <value>The copyright.</value>
      public string Copyright { get { return this._copyright; } }

      /// <summary>
      /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
      /// </returns>
      public override string ToString () {
        return string.Format ( "{0} v{1} - {2}", this.Name, this.Version, this.Company );
      }
    }

    /// <summary>
    /// Handles the SelectedIndexChanged event of the lstModules control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void lstModules_SelectedIndexChanged ( object sender, EventArgs e ) {
     StringBuilder sb = new StringBuilder ();
      if ( lstModules.SelectedItem != null ) {
        if ( lstModules.SelectedItem.GetType () == typeof ( AssemblyInfo ) ) {
          AssemblyInfo ai = lstModules.SelectedItem as AssemblyInfo;
          sb.AppendLine ( ai.Copyright );
        } else if ( lstModules.SelectedItem.GetType () == typeof ( ModuleInfo ) ) {
          ModuleInfo mod = lstModules.SelectedItem as ModuleInfo;
          sb.AppendLine ( mod.Copyright );
          sb.AppendLine ( mod.Url );
          sb.AppendLine ( mod.Description );
        }
      }
      detailsTextBox.Text = sb.ToString ();
    }

    /// <summary>
    /// Handles the SelectedIndexChanged event of the lstContributors control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void lstContributors_SelectedIndexChanged ( object sender, EventArgs e ) {
      StringBuilder sb = new StringBuilder ();
      if ( lstContributors.SelectedItem != null ) {
        Contributor c = lstContributors.SelectedItem as Contributor;
        sb.AppendLine ( string.Format("Email: {0}", c.Email) );
        sb.AppendLine ( string.Format ( "Url: {0}", c.Url ) );
        sb.AppendLine ( string.Format ( "Blog: {0}", c.Blog ) );
      }
      detailsTextBox.Text = sb.ToString ();
    }

		private void donate_Click ( object sender, EventArgs e ) {
			ProcessStartInfo psi = new ProcessStartInfo ( "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=2485230", string.Empty );
			Process p = new Process ();
			p.StartInfo = psi;
			p.Start ( );
		}
  }
}