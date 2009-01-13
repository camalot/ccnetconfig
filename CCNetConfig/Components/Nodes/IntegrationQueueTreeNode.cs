using System.ComponentModel;
using System.Windows.Forms;
using CCNetConfig.Core;
using CCNetConfig.UI;
using System;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// Displays an integration queue in the tree.
    /// </summary>
    public class IntegrationQueueTreeNode
        : ValidatingTreeNode, IInteractiveTreeNode
    {
        #region Private fields
        private readonly Queue queue;
        private readonly QueueImageKeys imageKeys;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationQueueTreeNode"/> class.
        /// </summary>
        /// <param name="value">The queue.</param>
        /// <param name="imageKeys">The keys to the images to use.</param>
        public IntegrationQueueTreeNode(Queue value, QueueImageKeys imageKeys)
            : base(value, value.Name)
        {
            Tag = value.GetHashCode();
            queue = value;
            this.imageKeys = imageKeys;
            UpdateImage();
            UpdateProjects();
            value.PropertyChanged += new PropertyChangedEventHandler(OnQueuePropertyChanged);
        }
        #endregion

        #region Public properties
        #region Queue
        /// <summary>
        /// The associated queue.
        /// </summary>
        public Queue Queue
        {
            get { return queue; }
        }
        #endregion
        #endregion

        #region Public methods
        #region UpdateImage()
        /// <summary>
        /// Changes the image of the node to match the configuration.
        /// </summary>
        public void UpdateImage()
        {
            int imageIndex = imageKeys.BaseImageIndex;
            if (queue.HasConfig) imageIndex = imageKeys.ConfigImageIndex;
            ImageIndex = imageIndex;
            SelectedImageIndex = imageIndex;
        }
        #endregion

        #region UpdateProjects()
        /// <summary>
        /// Updates all the associated projects.
        /// </summary>
        public void UpdateProjects()
        {
            Nodes.Clear();
            foreach (Project proj in queue.Projects)
            {
                Nodes.Add(new ProjectTreeNode(proj, imageKeys.ProjectImageIndex));
            }
        }
        #endregion

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
                form.DisplayItem(queue);
            }
            else if (args.Button == MouseButtons.Right)
            {
                form.ChangeContextMenu(GenerateContextMenu());
            }
        }
        #endregion
        #endregion

        #region Private methods
        #region value_PropertyChanged()
        /// <summary>
        /// Updates the node with any relevant changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQueuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    Text = queue.Name;
                    break;
                case "HasConfig":
                    UpdateImage();
                    break;
                case "Projects":
                    UpdateProjects();
                    break;
            }
        }
        #endregion

        #region GenerateContextMenu()
        /// <summary>
        /// Generates the context menu.
        /// </summary>
        /// <returns></returns>
        private ContextMenuStrip GenerateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            menu.Items.Add(GenerateAssociateProjectsMenu());
            menu.Items.Add(GenerateValidateMenu());
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(GenerateRemoveQueueMenu());

            return menu;
        }
        #endregion

        #region GenerateRemoveQueueMenu()
        /// <summary>
        /// Generates the remove queue menu.
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem GenerateRemoveQueueMenu()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Remove queue");
            menuItem.Image = CCNetConfig.Properties.Resources.queuedelete_16x16;
            menuItem.Click += delegate(object sender, EventArgs e)
            {
                if (MessageBox.Show(MainForm,
                    string.Format("Are you sure you want to delete the queue '{0}'", queue.Name),
                    "Confirm delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    foreach (Project project in queue.Projects)
                    {
                        project.Queue = null;
                    }
                    ProjectQueuesTreeNode parent = Parent as ProjectQueuesTreeNode;
                    parent.Configuration.Queues.Remove(queue);
                    Parent.Nodes.Remove(this);
                }
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
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Validate queue");
            menuItem.Image = CCNetConfig.Properties.Resources.checkmark_16x16;
            menuItem.Click += delegate(object sender, EventArgs e)
            {
                MainForm.QueueValidation(this);
                MainForm.StartValidation();
            };
            return menuItem;
        }
        #endregion

        #region GenerateAssociateProjectsMenu()
        /// <summary>
        /// Generates the associate projects menu item.
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem GenerateAssociateProjectsMenu()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Associate projects...");
            menuItem.Image = CCNetConfig.Properties.Resources.queueprojectlink_16x16;
            menuItem.Click += delegate(object sender, EventArgs e)
            {
                ProjectQueuesTreeNode ParentNode = (Parent as ProjectQueuesTreeNode);
                QueueProjectLinkForm form = new QueueProjectLinkForm();
                form.LoadProjects(queue, ParentNode.Configuration);
                form.Saved += delegate(object source, EventArgs args)
                {
                    foreach (Project proj in queue.Projects)
                    {
                        if (proj.Queue == queue.Name) proj.Queue = null;
                    }
                    queue.Projects.Clear();
                    queue.Projects.AddRange(form.SelectedProjects);
                    foreach (Project proj in queue.Projects)
                    {
                        if (!string.IsNullOrEmpty(proj.Queue) && (proj.Queue != queue.Name))
                        {
                            ParentNode.Configuration.Queues[proj.Queue].RemoveProject(proj);
                        }
                        if (proj.Queue != queue.Name) proj.Queue = queue.Name;
                    }
                    UpdateProjects();
                };
                form.ShowDialog();
            };
            return menuItem;
        }
        #endregion
        #endregion
    }
}
