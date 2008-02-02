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
using System.Windows.Forms;
using CCNetConfig.Core;

namespace CCNetConfig.Components {
  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class TriggersTreeNode : TreeNode {
    /// <summary>
    /// Initializes a new instance of the <see cref="TriggersTreeNode"/> class.
    /// </summary>
    /// <param name="imgIndex">Index of the img.</param>
    public TriggersTreeNode ( int imgIndex )
      : base ( "Triggers", imgIndex, imgIndex ) {

    }
  }
  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class TasksTreeNode : TreeNode {
    /// <summary>
    /// Initializes a new instance of the <see cref="TasksTreeNode"/> class.
    /// </summary>
    /// <param name="imgIndex">Index of the img.</param>
    public TasksTreeNode ( int imgIndex )
      : base ( "Tasks", imgIndex, imgIndex ) {

    }
  }
  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class PublishersTreeNode : TreeNode {
    /// <summary>
    /// Initializes a new instance of the <see cref="PublishersTreeNode"/> class.
    /// </summary>
    /// <param name="imgIndex">Index of the img.</param>
    public PublishersTreeNode ( int imgIndex )
      : base ( "Publishers", imgIndex, imgIndex ) {

    }
  }

  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class PrebuildTreeNode : TreeNode {
    /// <summary>
    /// Initializes a new instance of the <see cref="PrebuildTreeNode"/> class.
    /// </summary>
    /// <param name="imgIndex">Index of the img.</param>
    public PrebuildTreeNode ( int imgIndex )
      : base ( "Prebuild", imgIndex, imgIndex ) {

    }
  }
  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class ExtensionTreeNode : TreeNode {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExtensionTreeNode"/> class.
    /// </summary>
    /// <param name="imgIndex">Index of the img.</param>
    public ExtensionTreeNode (int imgIndex) : base ("Project Extensions",imgIndex,imgIndex) {
    }
  }

  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class ExtensionItemTreeNode : TreeNode {
    private ProjectExtension extension;
    /// <summary>
    /// Initializes a new instance of the <see cref="ExtensionItemTreeNode"/> class.
    /// </summary>
    /// <param name="extension">The extension.</param>
    /// <param name="imgIndex">Index of the img.</param>
    public ExtensionItemTreeNode (ProjectExtension extension, int imgIndex ) : base (extension.ToString(),imgIndex,imgIndex) {
      this.extension = extension;
    }

    /// <summary>
    /// Gets or sets the project extension.
    /// </summary>
    /// <value>The project extension.</value>
    public ProjectExtension ProjectExtension { get { return this.extension; } set { this.extension = value; } }
  }

  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class ProjectTreeNode : TreeNode {
    private Project proj;
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectTreeNode"/> class.
    /// </summary>
    /// <param name="proj">The proj.</param>
    /// <param name="imgIndex">Index of the img.</param>
    public ProjectTreeNode ( Project proj, int imgIndex )
      : base ( proj.Name, imgIndex, imgIndex ) {
      this.Tag = proj.GetHashCode ();
      this.proj = proj;
    }

    /// <summary>
    /// Gets the project.
    /// </summary>
    /// <value>The project.</value>
    public Project Project { get { return this.proj; } }
  }
  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class PublisherTaskItemTreeNode : TreeNode {
    private PublisherTask _pub = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="PublisherTaskItemTreeNode"/> class.
    /// </summary>
    /// <param name="pt">The pt.</param>
    /// <param name="imgIndex">Index of the img.</param>
    public PublisherTaskItemTreeNode ( PublisherTask pt, int imgIndex )
      : base ( pt.GetType ().Name, imgIndex, imgIndex ) {
      this._pub = pt;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PublisherTaskItemTreeNode"/> class.
    /// </summary>
    public PublisherTaskItemTreeNode () {

    }

    /// <summary>
    /// Gets the publisher task.
    /// </summary>
    /// <value>The publisher task.</value>
    public PublisherTask PublisherTask { get { return this._pub; } set { this._pub = value; } }
  }
  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class TriggerItemTreeNode : TreeNode {
    private Trigger _trig = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerItemTreeNode"/> class.
    /// </summary>
    /// <param name="trig">The trig.</param>
    /// <param name="imgIndex">Index of the img.</param>
    public TriggerItemTreeNode ( Trigger trig, int imgIndex )
      : base ( trig.GetType ().Name, imgIndex, imgIndex ) {
      this._trig = trig;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerItemTreeNode"/> class.
    /// </summary>
    public TriggerItemTreeNode () {

    }

    /// <summary>
    /// Gets the trigger.
    /// </summary>
    /// <value>The trigger.</value>
    public Trigger Trigger { get { return this._trig; } set { this._trig = value; } }
  }

  /// <summary>
  /// A Custom TreeNode
  /// </summary>
  public class ProjectQueuesTreeNode : TreeNode {
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectQueuesTreeNode"/> class.
    /// </summary>
    public ProjectQueuesTreeNode ( ) : base("Integration Queues") {

    }
    /// <summary>
    /// Refreshes the queue.
    /// </summary>
    public void RefreshQueue ( ) { 
    
    }

    /// <summary>
    /// Creates the queue.
    /// </summary>
    /// <param name="project">The project.</param>
    public void CreateQueue ( Project project ) {

    }
  }

  /// <summary>
  /// 
  /// </summary>
  public class IntegrationQueueTreeNode : TreeNode {
    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationQueueTreeNode"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    public IntegrationQueueTreeNode ( string name ) : base(name) {

    }
  }
}
