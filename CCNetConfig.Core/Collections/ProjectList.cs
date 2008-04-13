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
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core.Collections {
  /// <summary>
  /// A collection of <see cref="CCNetConfig.Core.Project">Project</see> objects.
  /// </summary>
  public class ProjectList : CloneableList<Project> {

    /// <summary>
    /// Gets or sets the <see cref="CCNetConfig.Core.Project"/> with the specified name.
    /// </summary>
    /// <value></value>
    [ReflectorIgnore]
    public Project this[ string name ] {
      get {
        if ( this.Contains ( name ) )
          return this[ IndexOf ( name ) ];
        else
          return null;
      }
      set {
        if ( this.Contains ( name ) )
          this[ IndexOf ( name ) ] = value;
        else
          throw new IndexOutOfRangeException ( );
      }
    }

    /// <summary>
    /// Determines whether this contains the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>
    /// 	<see langword="true"/> if this contains the specified name; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Contains ( string name ) {
      return this.IndexOf ( name ) >= 0;
    }

    /// <summary>
    /// Gets the index of the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public int IndexOf ( string name ) {
      for ( int i = 0; i < this.Count; i++ ) {
        Project proj = this[ i ];
        if ( string.Compare ( proj.Name, name, true ) == 0 )
          return i;
      }
      return -1;
    }

    /// <summary>
    /// Removes the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    public void Remove ( string name ) {
      if ( this.Contains(name) )
        this.Remove ( this[ name ] );
    }

    /// <summary>
    /// Gets the keys.
    /// </summary>
    /// <value>The keys.</value>
    [ReflectorIgnore]
    public List<string> Keys {
      get {
        List<string> keys = new List<string> ( );
        foreach ( Project proj in this )
          keys.Add ( proj.Name );
        return keys;
      }
    }

    /// <summary>
    /// Gets the number of projects that have the specified name. 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <remarks>Used to check if a duplicate project name was added.</remarks>
    public int GetCountByName ( string name ) {
      List<string> keys = new List<string> ( );
      foreach ( Project proj in this )
        if ( string.Compare(proj.Name,name,true) == 0 ) 
          keys.Add ( proj.Name );
      return keys.Count;
    }

    /// <summary>
    /// Compares <see cref="CCNetConfig.Core.Project"/> objects for sorting.
    /// </summary>
    public class ProjectComparer : IComparer<Project> {

      #region IComparer<Project> Members

      /// <summary>
      /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
      /// </summary>
      /// <param name="x">The first object to compare.</param>
      /// <param name="y">The second object to compare.</param>
      /// <returns>
      /// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
      /// </returns>
      public int Compare ( Project x, Project y ) {
        return string.Compare ( x.Name, y.Name );
      }

      #endregion
    }
  }
}
