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

namespace CCNetConfig.Core.Components {
  /// <summary>
  /// Provides a method of applying a format string to a value that implements IFormattable.
  /// </summary>
  [AttributeUsage ( AttributeTargets.Property | AttributeTargets.Field )]
  public class FormatProviderAttribute : Attribute {
    string _format = string.Empty;
    /// <summary>
    /// Initializes a new instance of the <see cref="FormatProviderAttribute"/> class.
    /// </summary>
    /// <param name="format">The format.</param>
    public FormatProviderAttribute ( string format ) {
      this._format = format;
    }

    /// <summary>
    /// Gets or sets the format.
    /// </summary>
    /// <value>The format.</value>
    public string Format { get { return this._format; } set { this._format = value; } }
  }
}
