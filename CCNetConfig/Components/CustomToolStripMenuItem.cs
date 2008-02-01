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
  /// A Tool Strip Menu Item that takes a <see cref="CCNetConfig.Core.PublisherTask"/>
  /// </summary>
  public class TaskToolStripMenuItem : ToolStripMenuItem {
    PublisherTaskItemTreeNode ptit = null;
    ProjectTreeNode ptn = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskToolStripMenuItem"/> class.
    /// </summary>
    /// <param name="textFormat">The text format.</param>
    /// <param name="ptit">The PublisherTaskItemTreeNode.</param>
    /// <param name="parentProject">The parent project.</param>
    public TaskToolStripMenuItem( string textFormat, PublisherTaskItemTreeNode ptit, ProjectTreeNode parentProject ) : base(string.Format(textFormat,ptit.PublisherTask.ToString(),parentProject.Project.ToString())) {
      ptn = parentProject;
      this.ptit = ptit;
    }

    /// <summary>
    /// Gets the project tree node.
    /// </summary>
    /// <value>The project tree node.</value>
    public ProjectTreeNode ProjectTreeNode { get { return this.ptn; } }
    /// <summary>
    /// Gets the publisher task item tree node.
    /// </summary>
    /// <value>The publisher task item tree node.</value>
    public PublisherTaskItemTreeNode PublisherTaskItemTreeNode { get { return this.ptit; } }
  }
  /// <summary>
  /// A Tool Strip Menu Item that takes a <see cref="CCNetConfig.Core.PublisherTask"/>
  /// </summary>
  public class PublisherToolStripMenuItem : TaskToolStripMenuItem {
    /// <summary>
    /// Initializes a new instance of the <see cref="PublisherToolStripMenuItem"/> class.
    /// </summary>
    /// <param name="textFormat">The text format.</param>
    /// <param name="ptit">The PublisherTaskItemTreeNode.</param>
    /// <param name="parentProject">The parent project.</param>
    public PublisherToolStripMenuItem ( string textFormat, PublisherTaskItemTreeNode ptit, ProjectTreeNode parentProject )
      : base ( textFormat, ptit, parentProject ) {

    }
  }
  /// <summary>
  /// A Tool Strip Menu Item that takes a <see cref="CCNetConfig.Core.Trigger"/>
  /// </summary>
  public class TriggerToolStripMenuItem : ToolStripMenuItem {
    ProjectTreeNode ptn = null;
    TriggerItemTreeNode titn = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerToolStripMenuItem"/> class.
    /// </summary>
    /// <param name="textFormat">The text format.</param>
    /// <param name="titn">The titn.</param>
    /// <param name="parentProject">The parent project.</param>
    public TriggerToolStripMenuItem (string textFormat, TriggerItemTreeNode titn, ProjectTreeNode parentProject ) : base(string.Format(textFormat,titn.Trigger.ToString(), parentProject.Project.Name) ) {
      this.ptn = parentProject;
      this.titn = titn;
    }

    /// <summary>
    /// Gets the trigger tree node.
    /// </summary>
    /// <value>The trigger tree node.</value>
    public TriggerItemTreeNode TriggerTreeNode { get { return this.titn; } }
    /// <summary>
    /// Gets the project tree node.
    /// </summary>
    /// <value>The project tree node.</value>
    public ProjectTreeNode ProjectTreeNode { get { return this.ptn; } }

  }
}
