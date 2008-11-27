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
using CCNetConfig.Core.Serialization;
using System.Xml;
using CCNetConfig.Core.Enums;
using CCNetConfig.Core;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The <see cref="CCNetConfig.CCNet.EmailPublisher">Email Publisher</see> can be used to send email to any number of users. It is common to include one user who gets an email for every 
  /// build and then also send email to every developer who checked code in for this build.
  ///</summary>
  /// <remarks>
  /// People tend to prefer to use <a href="http://confluence.public.thoughtworks.org/display/CCNET/CCTray" target="_blank">CCTray</a> rather than 
  /// email for instant notification these days.
  /// </remarks>
  /// <a href="http://confluence.public.thoughtworks.org/display/CCNET/Email+Publisher">Email Publisher CCNet Documentation</a>
  [MinimumVersion ( "1.0" ), ReflectorName("email")]
  public class EmailPublisher : PublisherTask, ICCNetDocumentation {
    private string _smtpServer = string.Empty;
    private string _from = string.Empty;
    private bool? _includeDetails = null;
    private CloneableList<User> _users = null;
    private CloneableList<Group> _groups = null;
    private CloneableList<Converter> _converters = null;
    private string _mailHostUserName = string.Empty;
    private HiddenPassword _mailHostPassword = new HiddenPassword ( );

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailPublisher"/> class.
    /// </summary>
    public EmailPublisher ( )
      : base ( "email" ) {
      _users = new CloneableList<User> ( );
      _groups = new CloneableList<Group> ( );
      this.Converters = new CloneableList<Converter> ( );
      this.ModifierNotificationTypes = new CloneableList<EmailNotificationType> ( );
    }

    /// <summary>
    /// The SMTP server that CruiseControl.NET will connect to to send email.
    /// </summary>
    [Description ( "The SMTP server that CruiseControl.NET will connect to to send email." ), DefaultValue ( null ),
    DisplayName ( "(MailHost)" ), Category ( "Required" ), ReflectorName("mailhost"), Required, ReflectorNodeType(ReflectorNodeTypes.Attribute) ]
    public string MailHost { get { return this._smtpServer; } set { this._smtpServer = Util.CheckRequired ( this, "mailHost", value ); } }

    /// <summary>
    /// The user name to provide to the SMTP server.
    /// </summary>
    /// <value>The name of the mail host user.</value>
    [Description ( "The user name to provide to the SMTP server." ),
    DefaultValue ( null ), Category ( "Optional" ), MinimumVersion ( "1.1.1" ), ReflectorName("mailhostUsername")]
    public string MailHostUserName { get { return this._mailHostUserName; } set { this._mailHostUserName = value; } }

    /// <summary>
    /// Gets or sets the mail port.
    /// </summary>
    /// <value>The mail port.</value>
    /// <remarks>Added 2/23/2008: workitem : 15008</remarks>
    [Description ( "The SMTP server port number" ),
    DefaultValue(null), Category("Optional"), MinimumVersion("1.4"), 
    ReflectorName("mailport")]
    public int? MailPort { get; set; }

    /// <summary>
    /// The password to provide to the SMTP server.
    /// </summary>
    /// <value>The name of the mail host user.</value>
    [Description ( "The password to provide to the SMTP server." ),
    DefaultValue ( null ), Category ( "Optional" ), MinimumVersion ( "1.1.1" ),
    TypeConverter ( typeof ( PasswordTypeConverter ) ), ReflectorName("mailhostPassword")]
    public HiddenPassword MailHostPassword { get { return this._mailHostPassword; } set { this._mailHostPassword = value; } }

    /// <summary>
    /// The email address that email will be marked as coming from.
    /// </summary>
    [Description ( "The email address that email will be marked as coming from." ), DefaultValue ( null ), DisplayName ( "(From)" ),
    Category ( "Required" ), ReflectorName ( "from" ), Required, ReflectorNodeType ( ReflectorNodeTypes.Attribute )]
    public string From { get { return this._from; } set { this._from = Util.CheckRequired ( this, "from", value ); } }
    /// <summary>
    /// Whether to send a full report or not. If not, just sends a simple status message with a link to the build report
    /// </summary>
    [Description ( "Whether to send a full report or not. If not, just sends a simple status message with a link to the build report." ),
    DefaultValue ( null ), Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) ),
   TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ), Category ( "Optional" ), ReflectorName("includeDetails")]
    public bool? IncludeDetails { get { return this._includeDetails; } set { this._includeDetails = value; } }
    /// <summary>
    /// A collection of <see cref="CCNetConfig.CCNet.User">User</see>s that define who to send emails to.
    /// </summary>
    [Description ( "A collection of usersthat define who to send emails to." ), DefaultValue ( null ),
    TypeConverter ( typeof ( IListTypeConverter ) ), Category ( "Optional" ), ReflectorName("users") ]
    public CloneableList<User> Users { get { return this._users; } set { this._users = value; } }
    /// <summary>
    /// A collection of <see cref="CCNetConfig.CCNet.User">Group</see>s that identify which the notification policy for a set of
    /// <see cref="CCNetConfig.CCNet.User">User</see>s.
    /// </summary>
    [Description ( "A collection of Groups that identify which the notification policy for a set of Users" ), 
    DefaultValue ( null ),ReflectorName("groups"),
    TypeConverter ( typeof ( IListTypeConverter ) ), Category ( "Optional" )]
    public CloneableList<Group> Groups { get { return this._groups; } set { this._groups = value; } }

    /// <summary>
    /// Gets or sets the converters.
    /// </summary>
    /// <value>The converters.</value>
    [Description ( "Allows transformations to be performed on the names of the modifiers for making an email address." ), 
    DefaultValue ( null ),
    TypeConverter ( typeof ( IListTypeConverter ) ), 
    Category ( "Optional" ), MinimumVersion("1.4"),
    ReflectorName("converters")]
    public CloneableList<Converter> Converters { get; set; }

    /// <summary>
    /// Gets or sets the modifier notification types.
    /// </summary>
    /// <value>The modifier notification types.</value>
    [Description("A set of NotificationTypes, specifying build states for which CruiseControl.Net should send an email to the comitters of the build"),
    ReflectorName("modifierNotificationTypes"), 
    TypeConverter(typeof(IListTypeConverter)), 
    Category("Optional"), MinimumVersion("1.4"),DefaultValue(null)]
    public CloneableList<EmailNotificationType> ModifierNotificationTypes { get; set; }
    /// <summary>
    /// Creates a copy of this object
    /// </summary>
    /// <returns></returns>
    public override PublisherTask Clone ( ) {
      EmailPublisher ep = this.MemberwiseClone ( ) as EmailPublisher;
      ep.Groups = this.Groups.Clone ( );
      ep.Users = this.Users.Clone ( );
      ep.MailHostPassword = this.MailHostPassword.Clone ( );
      return ep;
    }

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<EmailPublisher> ( ).Serialize ( this );
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( XmlElement element ) {
      this.Users = new CloneableList<User> ( );
      this.Groups = new CloneableList<Group> ( );
      this._from = string.Empty;
      this.IncludeDetails = null;
      this._smtpServer = string.Empty;
      this.MailHostUserName = string.Empty;
      this.MailHostPassword = new HiddenPassword ( );

      if ( string.Compare ( element.Name, this.TypeName, false ) != 0 )
        throw new InvalidCastException ( string.Format ( "Unable to convert {0} to a {1}", element.Name, this.TypeName ) );

      this.From = Util.GetElementOrAttributeValue ( "from", element );
      this.MailHost = Util.GetElementOrAttributeValue ( "mailhost", element );

      string s = Util.GetElementOrAttributeValue ( "mailhostUsername", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.MailHostUserName = s;

      s = Util.GetElementOrAttributeValue ( "mailhostPassword", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.MailHostPassword.Password = s;

      s = Util.GetElementOrAttributeValue ( "mailport", element );
      int port = 0;
      if ( !string.IsNullOrEmpty(s) && int.TryParse ( s, out port ) ) {
        this.MailPort = port;
      }

      XmlElement cele = element.SelectSingleNode ( "converters" ) as XmlElement;
      if ( cele != null ) {
        foreach ( XmlElement ce in cele.SelectNodes ( "./*" ) ) {
          Converter conv = new Converter ( );
          conv.Deserialize ( ce );
          this.Converters.Add ( conv );
        }
      }

      XmlElement mele = element.SelectSingleNode ( "modifierNotificationTypes" ) as XmlElement;
      if ( mele != null ) {
        foreach ( XmlElement me in mele.SelectNodes ( "./*" ) ) {
          EmailNotificationType ent = new EmailNotificationType ( );
          ent.Deserialize ( me );
          if ( ent.Value.HasValue )
            this.ModifierNotificationTypes.Add ( ent );
        }
      }

      s = Util.GetElementOrAttributeValue ( "includeDetails", element );
      if ( !string.IsNullOrEmpty ( s ) )
        this.IncludeDetails = string.Compare ( s, bool.TrueString, false ) == 0;

      XmlElement gele = ( XmlElement ) element.SelectSingleNode ( "groups" );
      if ( gele != null ) {
        foreach ( XmlElement groupE in gele.SelectNodes ( "group" ) ) {
          Group g = new Group ( );
          g.Deserialize ( groupE );
          this.Groups.Add ( g );
        }
      }

      XmlElement uele = ( XmlElement ) element.SelectSingleNode ( "users" );
      if ( uele != null ) {
        foreach ( XmlElement userE in uele.SelectNodes ( "user" ) ) {
          User u = new User ( );
          u.Deserialize ( userE );
          this.Users.Add ( u );
        }
      }
    }

    #region ICCNetDocumentation Members
    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [Browsable ( false ), ReflectorIgnore]
    public Uri DocumentationUri {
      get { return new Uri ( "http://ccnet.thoughtworks.net/display/CCNET/Email+Publisher?decorator=printable" ); }
    }
    #endregion


  }

}
