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
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Collections;

namespace CCNetConfig.Updater.Core {
  /// <summary>
  /// Command line parser.  
  /// </summary>
  public class CommandLineArguments {
    // Variables
    private new Dictionary<string,string> namedParameters;
    private List<string> unnamedParameters;
    /// <summary>
    /// Creates a <see cref="CommandLineArguments"/> object to parse
    /// command lines.
    /// </summary>
    /// <param name="args">The command line to parse.</param>
    public CommandLineArguments ( string[] args ) {
      namedParameters = new Dictionary<string,string> ();
      unnamedParameters = new List<string> ();
      Regex splitter = new Regex ( Properties.Strings.ArgumentsSplitterRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled );
      Regex remover = new Regex ( Properties.Strings.ArgumentsRemoverRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled );
      string parameter = null;
      string[] parts;

      foreach ( string str in args ) {
        // Do we have a parameter (starting with -, /, or --)?
        if ( str.StartsWith ( "-" ) || str.StartsWith ( "/" ) ) {
          // Look for new parameters (-,/ or --) and a possible
          // enclosed value (=,:)
          parts = splitter.Split ( str, 3 );
          switch ( parts.Length ) {
            // Found a value (for the last parameter found
            // (space separator))
            case 1:
              if ( parameter != null ) {
                if ( !namedParameters.ContainsKey ( parameter ) ) {
                  parts[0] =
                      remover.Replace ( parts[0], "$1" );
                  namedParameters.Add (
                      parameter, parts[0] );
                }
                parameter = null;
              }
              // else Error: no parameter waiting for a value
              // (skipped)
              break;
            // Found just a parameter
            case 2:
              // The last parameter is still waiting. With no
              // value, set it to true.
              if ( parameter != null ) {
                if ( !namedParameters.ContainsKey ( parameter ) )
                  namedParameters.Add ( parameter, "true" );
              }
              parameter = parts[1];
              break;
            // parameter with enclosed value
            case 3:
              // The last parameter is still waiting. With no
              // value, set it to true.
              if ( parameter != null ) {
                if ( !namedParameters.ContainsKey ( parameter ) )
                  namedParameters.Add ( parameter, "true" );
              }
              parameter = parts[1];
              // Remove possible enclosing characters (",')
              if ( !namedParameters.ContainsKey ( parameter ) ) {
                parts[2] = remover.Replace ( parts[2], "$1" );
                namedParameters.Add ( parameter, parts[2] );
              }
              parameter = null;
              break;
          }
        } else {
          unnamedParameters.Add ( str );
        }
      }
      // In case a parameter is still waiting
      if ( parameter != null ) {
        if ( !namedParameters.ContainsKey ( parameter ) )
          namedParameters.Add ( parameter, "true" );
      }
    }

    /// <summary>
    /// Retrieves the parameter with the specified name.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter. The name is case insensitive.
    /// </param>
    /// <returns>
    /// The parameter or <c>null</c> if it can not be found.
    /// </returns>
    public string this[string name] {
      get {
        return ( namedParameters[name] );
      }
    }

    /// <summary>
    /// Retrieves an unnamed parameter (that did not start with '-'
    /// or '/').
    /// </summary>
    /// <param name="index">The index of the unnamed parameter.</param>
    /// <returns>The unnamed parameter or <c>null</c> if it does not
    /// exist.</returns>
    /// <remarks>
    /// Primarily used to retrieve filenames which extension has been
    /// associated to the application.
    /// </remarks>
    public string this[int index] {
      get {
        return (string)( index < unnamedParameters.Count ?
            unnamedParameters[index] :
        null );
      }
    }

    /// <summary>
    /// Gets the keys.
    /// </summary>
    /// <value>The keys.</value>
    public Dictionary<string, string>.KeyCollection Keys {
      get { return namedParameters.Keys; }
    }

    /// <summary>
    /// Gets the values.
    /// </summary>
    /// <value>The values.</value>
    public Dictionary<string, string>.ValueCollection Values {
      get { return this.namedParameters.Values; }
    }

    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>The count.</value>
    public int Count { get { return this.UnnamedParametersCount + this.NamedParametersCount; } }

    /// <summary>
    /// Gets the unnamed parameters count.
    /// </summary>
    /// <value>The unnamed parameters count.</value>
    public int UnnamedParametersCount { get { return this.unnamedParameters.Count; } }

    /// <summary>
    /// Gets the named parameters count.
    /// </summary>
    /// <value>The named parameters count.</value>
    public int NamedParametersCount { get { return this.namedParameters.Count; } }

    /// <summary>
    /// Converts unnamed parameters to array.
    /// </summary>
    /// <returns></returns>
    public string[] UnnamedParametersToArray(  ) {
      return this.unnamedParameters.ToArray ();
    }

    /// <summary>
    /// Determines whether this contains specified param.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <returns>
    /// 	<c>true</c> if this contains specified param; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsParam ( string param ) {
      return this.namedParameters.ContainsKey ( param );
    }
  }
}
