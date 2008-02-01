/*
 * Copyright (c) 2007-2008, Ryan Conrad. All rights reserved.
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
using System.Windows.Forms;

namespace CCNetConfig.Components {
  /// <summary>
  /// This class is an implementation of the 'IComparer' interface.
  /// </summary>
  internal class ListViewColumnStringSorter : System.Collections.IComparer {
    /// <summary>
    /// Specifies the column to be sorted
    /// </summary>
    private int _columnToSort;
    /// <summary>
    /// Specifies the order in which to sort (i.e. 'Ascending').
    /// </summary>
    private SortOrder _orderOfSort;
    /// <summary>
    /// Class constructor.  Initializes various elements
    /// </summary>
    public ListViewColumnStringSorter () {
      // Initialize the column to '0'
      _columnToSort = 0;
      // Initialize the sort order to 'none'
      _orderOfSort = SortOrder.None;
    }

    /// <summary>
    /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
    /// </summary>
    /// <param name="x">First object to be compared</param>
    /// <param name="y">Second object to be compared</param>
    /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
    public int Compare ( object x, object y ) {
      int compareResult;
      ListViewItem listviewX = x as ListViewItem;
      ListViewItem listviewY = y as ListViewItem; ;
      // Compare the two items
      compareResult = string.Compare ( listviewX.SubItems[_columnToSort].Text, listviewY.SubItems[_columnToSort].Text,true );
      // Calculate correct return value based on object comparison
      if ( _orderOfSort == SortOrder.Ascending ) {
        // Ascending sort is selected, return normal result of compare operation
        return compareResult;
      } else if ( _orderOfSort == SortOrder.Descending ) {
        // Descending sort is selected, return negative result of compare operation
        return ( -compareResult );
      } else {
        // Return '0' to indicate they are equal
        return 0;
      }
    }

    /// <summary>
    /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
    /// </summary>
    public int SortColumn { set { _columnToSort = value; } get { return _columnToSort; } }

    /// <summary>
    /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
    /// </summary>
    public SortOrder Order { set { _orderOfSort = value; } get { return _orderOfSort; } }
  }
}
