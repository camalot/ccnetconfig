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
  /// Node type to create when serializing
  /// </summary>
  public enum ReflectorNodeTypes {
    /// <summary>
    /// Create attribute
    /// </summary>
    Attribute,
    /// <summary>
    /// Create an element
    /// </summary>
    Element,
    /// <summary>
    /// The value of an element
    /// </summary>
    Value
  }
  /// <summary>
  /// Indicates what type of node should be used when serializing
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public class ReflectorNodeTypeAttribute : Attribute {
    private ReflectorNodeTypes _type = ReflectorNodeTypes.Element;
    /// <summary>
    /// Initializes a new instance of the <see cref="ReflectorNodeTypeAttribute"/> class.
    /// </summary>
    /// <param name="type">The type.</param>
    public ReflectorNodeTypeAttribute ( ReflectorNodeTypes type) {
      this._type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReflectorNodeTypeAttribute"/> class.
    /// </summary>
    public ReflectorNodeTypeAttribute ( ) : this(ReflectorNodeTypes.Element) {

    }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    public ReflectorNodeTypes Type {
      get { return this._type; }
      set { this._type = value; } 
    }
  }
}
