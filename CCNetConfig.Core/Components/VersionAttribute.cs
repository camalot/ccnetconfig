/* Copyright (c) 2006, Ryan Conrad. All rights reserved.
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

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// A base class for indicating what version a property belongs to.
  /// </summary>
  [AttributeUsage ( AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field )]
  public abstract class VersionAttribute : Attribute, IComparable<Version> {
    private Version version = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="VersionAttribute"/> class.
    /// </summary>
    /// <param name="strVersion">The STR version.</param>
    internal VersionAttribute ( string strVersion ) {
      this.version = new Version ( strVersion );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionAttribute"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    internal VersionAttribute ( Version version ) {
      this.version = version;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    internal VersionAttribute ( int major, int minor ) {
      this.version = new Version ( major, minor );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="build">The build.</param>
    internal VersionAttribute ( int major, int minor, int build ) {
      this.version = new Version ( major, minor, build );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="build">The build.</param>
    /// <param name="revision">The revision.</param>
    internal VersionAttribute ( int major, int minor, int build, int revision ) {
      this.version = new Version ( major, minor, build, revision );
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <value>The version.</value>
    internal Version Version { get { return this.version; } }

    #region IComparable<Version> Members
    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
    /// </returns>
    public int CompareTo ( Version other ) {
      return other.CompareTo ( Version );
    }

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
    /// </returns>
    int IComparable<Version>.CompareTo ( Version other ) {
      return this.CompareTo ( other );
    }

    #endregion
  }

  /// <summary>
  /// Specifies the minimum version a property belongs to
  /// </summary>
  [AttributeUsage ( AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field )]
  public sealed class MinimumVersionAttribute : VersionAttribute {
    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumVersionAttribute"/> class.
    /// </summary>
    /// <param name="strVersion">The STR version.</param>
    public MinimumVersionAttribute ( string strVersion ) : base ( strVersion ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumVersionAttribute"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    public MinimumVersionAttribute ( Version version ) : base ( version ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    public MinimumVersionAttribute ( int major, int minor ) : base ( major, minor ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="build">The build.</param>
    public MinimumVersionAttribute ( int major, int minor, int build ) : base ( major, minor, build ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="build">The build.</param>
    /// <param name="revision">The revision.</param>
    public MinimumVersionAttribute ( int major, int minor, int build, int revision ) : base ( major, minor, build, revision ) { }
  }
  /// <summary>
  /// Specifies the maximum version a property belongs to
  /// </summary>
  [AttributeUsage ( AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field )]
  public sealed class MaximumVersionAttribute : VersionAttribute {
    /// <summary>
    /// Initializes a new instance of the <see cref="MaximumVersionAttribute"/> class.
    /// </summary>
    /// <param name="strVersion">The STR version.</param>
    public MaximumVersionAttribute ( string strVersion ) : base ( strVersion ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MaximumVersionAttribute"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    public MaximumVersionAttribute ( Version version ) : base ( version ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MaximumVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    public MaximumVersionAttribute ( int major, int minor ) : base ( major, minor ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MaximumVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="build">The build.</param>
    public MaximumVersionAttribute ( int major, int minor, int build ) : base ( major, minor, build ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="MaximumVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="build">The build.</param>
    /// <param name="revision">The revision.</param>
    public MaximumVersionAttribute ( int major, int minor, int build, int revision ) : base ( major, minor, build, revision ) { }
  }
  /// <summary>
  /// Specifies the exact version a property belongs to
  /// </summary>
  [AttributeUsage ( AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field )]
  public sealed class ExactVersionAttribute : VersionAttribute {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExactVersionAttribute"/> class.
    /// </summary>
    /// <param name="strVersion">The STR version.</param>
    public ExactVersionAttribute ( string strVersion ) : base ( strVersion ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExactVersionAttribute"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    public ExactVersionAttribute ( Version version ) : base ( version ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExactVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    public ExactVersionAttribute ( int major, int minor ) : base ( major, minor ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExactVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="build">The build.</param>
    public ExactVersionAttribute ( int major, int minor, int build ) : base ( major, minor, build ) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExactVersionAttribute"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="build">The build.</param>
    /// <param name="revision">The revision.</param>
    public ExactVersionAttribute ( int major, int minor, int build, int revision ) : base ( major, minor, build, revision ) { }
  }
}
