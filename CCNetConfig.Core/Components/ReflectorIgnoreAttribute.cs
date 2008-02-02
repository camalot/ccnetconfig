using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core.Components {
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class ReflectorIgnoreAttribute : Attribute {
  }
}
