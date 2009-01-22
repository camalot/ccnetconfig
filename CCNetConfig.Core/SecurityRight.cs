using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core
{
    /// <summary>
    /// The security levels.
    /// </summary>
    /// <remarks>
    /// This type has come directly from the CruiseControl.Net project.
    /// </remarks>
    public enum SecurityRight
    {
        /// <summary>
        /// The security right is allowed.
        /// </summary>
        Allow,
        /// <summary>
        /// The security right is denied.
        /// </summary>
        Deny,
        /// <summary>
        /// The security right will be inherited.
        /// </summary>
        Inherit
    }
}
