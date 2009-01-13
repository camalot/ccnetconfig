using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.UI;
using System.ComponentModel;
using CCNetConfig.Core;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// Root node for integration queues.
    /// </summary>
    public class ProjectQueuesTreeNode
        : TreeNode, IInteractiveTreeNode
    {
        #region Private fields
        private readonly QueueImageKeys imageKeys;
        private CruiseControl configuration;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectQueuesTreeNode"/> class.
        /// </summary>
        /// <param name="value">The underlying configuration.</param>
        /// <param name="imageKeys">The keys to the images to use.</param>
        public ProjectQueuesTreeNode(CruiseControl value, QueueImageKeys imageKeys)
            : base("Integration Queues")
        {
            this.configuration = value;
            this.imageKeys = imageKeys;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// The configuration.
        /// </summary>
        public virtual CruiseControl Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }
        #endregion

        #region Public methods
        #region HandleMouseClick()
        /// <summary>
        /// Handles a mouse click.
        /// </summary>
        /// <param name="form">The form that is processing the mouse clic.</param>
        /// <param name="args">The mouse click arguments.</param>
        public virtual void HandleMouseClick(MainForm form, TreeNodeMouseClickEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            {
                Information value = new Information(Nodes);
                form.DisplayItem(value);
            }
            else if (args.Button == MouseButtons.Right)
            {
                form.ChangeContextMenu(GenerateContextMenu());
            }
        }
        #endregion
        #endregion

        #region Private methods
        #region GenerateContextMenu()
        /// <summary>
        /// Generates the context menu.
        /// </summary>
        /// <returns></returns>
        private ContextMenuStrip GenerateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            menu.Items.Add(GenerateValidateMenu());
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(GenerateCreateQueueMenu());

            return menu;
        }
        #endregion

        #region GenerateCreateQueueMenu()
        /// <summary>
        /// Generates the create queue menu item.
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem GenerateCreateQueueMenu()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Add a new queue");
            menuItem.Image = CCNetConfig.Properties.Resources.queueadd_16x16;
            menuItem.Click += delegate(object sender, EventArgs e)
            {
                // Generate the new node and select it
                Queue newQueue = new Queue();
                configuration.Queues.Add(newQueue);
                TreeNode newNode = new IntegrationQueueTreeNode(newQueue, imageKeys);
                Nodes.Add(newNode);
                newNode.EnsureVisible();
                TreeView.SelectedNode = newNode;
            };
            return menuItem;
        }
        #endregion

        #region GenerateValidateMenu()
        /// <summary>
        /// Generates the validate menu item.
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem GenerateValidateMenu()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Validate all queues");
            menuItem.Image = CCNetConfig.Properties.Resources.checkmark_16x16;
            menuItem.Click += delegate(object sender, EventArgs e)
            {
                MainForm mainForm = (TreeView.FindForm() as MainForm);
                for (int loop = 0; loop < Nodes.Count; loop++)
                {
                    mainForm.QueueValidation(Nodes[loop]);
                }
                mainForm.StartValidation();
            };
            return menuItem;
        }
        #endregion
        #endregion

        #region Contained classes
        /// <summary>
        /// Holds information on the total numbers of queues.
        /// </summary>
        public class Information
        {
            #region Fields
            private int totalQueues;
            private int definedQueues;
            #endregion

            #region Constructors
            /// <summary>
            /// Generates a new instance of the information.
            /// </summary>
            /// <param name="nodes">The child nodes to use.</param>
            public Information(TreeNodeCollection nodes)
            {
                totalQueues = nodes.Count;
                definedQueues = 0;
                foreach (IntegrationQueueTreeNode node in nodes)
                {
                    if (node.Queue.HasConfig) definedQueues++;
                }
            }
            #endregion

            #region Public properties
            #region TotalQueues
            /// <summary>
            /// The total number of queues that have been defined.
            /// </summary>
            [DisplayName("Total Number of Queues")]
            [Category("Information")]
            [Description("The total number of queues that have been defined. This does not include implicit " +
                "queues - queues that are generated for projects without a defined queue.")]
            public int TotalQueues
            {
                get { return totalQueues; }
            }
            #endregion

            #region DefinedQueues
            /// <summary>
            /// The number of queues that have been configured.
            /// </summary>
            [DisplayName("Number of Configured Queues")]
            [Category("Information")]
            [Description("The total number of queues that have been configured. This only includes queues that " +
                "are marked as having configuration.")]
            public int DefinedQueues
            {
                get { return definedQueues; }
            }
            #endregion
            #endregion
        }
        #endregion
    }
}
