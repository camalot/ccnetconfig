using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using CCNetConfig.Core.Serialization;
using CCNetConfig.Core;
using CCNetConfig.Exceptions;

namespace CCNetConfig.CCNet {
  /// <summary>
  /// The CommentFilter can be used to filter modifications on the basis of the comment that was supplied with the modification.
  /// </summary>
  [ReflectorName("commentFilter"), MinimumVersion("1.3.0.3052")]
  public class CommentFilter : Filter {
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentFilter"/> class.
    /// </summary>
    public CommentFilter ( ) : base("commentFilter") {

    }
    /// <summary>
    /// Gets or sets the pattern.
    /// </summary>
    /// <value>The pattern.</value>
    [ReflectorName("pattern"), Required, Category("Required"), DisplayName("(Pattern)"),
    Description("This is the pattern used to compare the modification comment against.")]
    public string Pattern { get; set; }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public override System.Xml.XmlElement Serialize ( ) {
      return new Serializer<CommentFilter> ( ).Serialize ( this );
    }

    /// <summary>
    /// Deserializes the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    public override void Deserialize ( System.Xml.XmlElement element ) {
      this.Pattern = string.Empty;
      string s = Util.GetElementOrAttributeValue ( "pattern", element );
      if ( !string.IsNullOrEmpty ( s ) ) {
        this.Pattern = s;
      } else {
        throw new RequiredAttributeException ( this, "pattern" );
      }
    }
  }
}
