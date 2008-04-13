/*
 * Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using CCNetConfig.Core.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// This supports Codice Software's Plastic SCM  source control system.
  /// </summary>
  [MinimumVersion ( "1.3" )]
  public class PlasticScmSourceControl : SourceControl, ICCNetDocumentation {
    /// <summary>
    /// Initializes a new instance of the <see cref="PlasticScmSourceControl"/> class.
    /// </summary>
    public PlasticScmSourceControl ( )
      : base ( "plasticscm" ) {
    }

    /// <summary>
    /// Gets or sets the branch.
    /// </summary>
    /// <value>The branch.</value>
    [Description ( "The Plastic SCM branch to monitor." ), DisplayName ( "(Branch)" ), DefaultValue ( null ),
    Category ( "Required" ),
    ReflectorName ( "branch" ), Required]
    public string Branch { get; set; }

    /// <summary>
    /// Gets or sets the executable.
    /// </summary>
    /// <value>The executable.</value>
    [Description ( "The local path for the Plastic SCM command-line client" ),
    Category ( "Optional" ), DefaultValue ( null ),
    ReflectorName ( "executable" ),
    OpenFileDialogTitle ( "Select the path for the executable" ),
    FileTypeFilter ( "Plastic SCM|cm.exe|All Executables|*.exe;*.bat;*.cmd;*.ps1" ),
    Editor ( typeof ( OpenFileDialogUIEditor ), typeof ( UITypeEditor ) )]
    public string Executable { get; set; }

    /// <summary>
    /// Gets or sets the forced.
    /// </summary>
    /// <value>The forced.</value>
    [Description ( "Do the update with the \"--forced\" option" ),
    Category ( "Optional" ), DefaultValue ( null ),
    ReflectorName ( "forced" ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) )]
    public bool? Forced { get; set; }

    /// <summary>
    /// Gets or sets the label prefix.
    /// </summary>
    /// <value>The label prefix.</value>
    [Description ( "Specifies the prefix label name." ),
    Category ( "Optional" ), DefaultValue ( null ),
    ReflectorName ( "labelPrefix" )]
    public string LabelPrefix { get; set; }

    /// <summary>
    /// Gets or sets the label on success.
    /// </summary>
    /// <value>The label on success.</value>
    [Description ( "Specifies whether or not CCNet should create an Plastic SCM baseline when the build is successful." ),
    Category ( "Optional" ), DefaultValue ( null ),
    ReflectorName ( "labelOnSuccess" ), TypeConverter ( typeof ( DefaultableBooleanTypeConverter ) ),
    Editor ( typeof ( DefaultableBooleanUIEditor ), typeof ( UITypeEditor ) )]
    public bool? LabelOnSuccess { get; set; }

    /// <summary>
    /// Gets or sets the repository.
    /// </summary>
    /// <value>The repository.</value>
    [Description ( "The Plastic SCM repository to monitor." ),
    Category ( "Optional" ), DefaultValue ( null ),
    ReflectorName ( "repository" )]
    public string Repository { get; set; }

    /// <summary>
    /// Gets or sets the timeout.
    /// </summary>
    /// <value>The timeout.</value>
    [Description ( "Sets the timeout period for the source control operation." ),
    Category ( "Optional" ), DefaultValue ( null ),
    ReflectorName ( "timeout" ),
    TypeConverter(typeof(ExpandableObjectConverter))]
    public Timeout Timeout { get; set; }

    /// <summary>
    /// Gets or sets the working directory.
    /// </summary>
    /// <value>The working directory.</value>
    [Description ( "Valid Plastic SCM workspace path." ),
    Category ( "Required" ), DisplayName("(WorkingDirectory)"),
    DefaultValue ( null ), Required,
    ReflectorName ( "workingDirectory" ),
    BrowseForFolderDescription("Select workspace path."),
    Editor(typeof(BrowseForFolderUIEditor),typeof(UITypeEditor))]
    public string WorkingDirectory { get; set; }

    #region ICCNetDocumentation Members

    /// <summary>
    /// Gets the documentation URI.
    /// </summary>
    /// <value>The documentation URI.</value>
    [ReflectorIgnore, Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
    public Uri DocumentationUri {
      get { return new Uri ( "http://confluence.public.thoughtworks.org/display/CCNET/PlasticSCM+Source+Control+Block?decorator=printable" ); }
    }

    #endregion

    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<PlasticScmSourceControl> ( ).Serialize ( this );
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.WorkingDirectory = string.Empty;
      this.Branch = string.Empty;


    }

    /// <summary>
    /// Creates a copy of the source control object
    /// </summary>
    /// <returns></returns>
    public override SourceControl Clone ( ) {
      PlasticScmSourceControl pscm = this.MemberwiseClone ( ) as PlasticScmSourceControl;
      pscm.Timeout = this.Timeout.Clone ( );
      return pscm;
    }
  }
}
