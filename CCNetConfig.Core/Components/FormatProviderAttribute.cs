using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core.Components {
  [AttributeUsage ( AttributeTargets.Property | AttributeTargets.Field )]
  public class FormatProviderAttribute : Attribute {
    string _format = string.Empty;
    public FormatProviderAttribute ( string format ) {
      this._format = format;
    }

    public string Format { get { return this._format; } set { this._format = value; } }
  }
}
