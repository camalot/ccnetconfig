using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core;
using System.Windows.Forms;
using System.ComponentModel;
using CCNetConfig.UI;
using CoreUtil = CCNetConfig.Core.Util;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// Encapsulates all the logic for working with server security nodes.
    /// </summary>
    public class ServerSecurityNode
        : ValidatingTreeNode, IInteractiveTreeNode
    {
        #region Private fields
        private ServerSecurity security;
        private CruiseControl configuration;
        private bool hasSecurity = false;
        private readonly SecurityImageKeys imageKeys;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSecurityNode"/> class.
        /// </summary>
        /// <param name="config">The underlying configuration.</param>
        /// <param name="imageKeys">The keys to the images to use.</param>
        public ServerSecurityNode(CruiseControl config, SecurityImageKeys imageKeys)
            : base(config.Security, "Security")
        {
            // Store the configuration and register for any changes of the security
            configuration = config;
            configuration.PropertyChanged += OnSecurityChanged;

            // Update the image and load the security nodes
            this.imageKeys = imageKeys;
            UpdateImage();
            ChangeSecurity(config.Security);
        }
        #endregion

        #region Public properties
        #region Security
        /// <summary>
        /// The associated security settings.
        /// </summary>
        public ServerSecurity Security
        {
            get { return security; }
        }
        #endregion
        #endregion

        #region Public methods
        #region ChangeSecurity()
        /// <summary>
        /// Changes the associated server security settings.
        /// </summary>
        /// <param name="value"></param>
        public void ChangeSecurity(ServerSecurity value)
        {
            if (security != null) security.PropertyChanged -= OnPropertyChanged;
            hasSecurity = (value != null);
            security = value ?? new ServerSecurity();
            DataItem = security;
            Nodes.Clear();
            ReflectionHelper.GenerateChildNodes(this, security);
            security.PropertyChanged += OnPropertyChanged;
        }
        #endregion

        #region UpdateImage()
        /// <summary>
        /// Changes the image of the node to match the configuration.
        /// </summary>
        public void UpdateImage()
        {
            int imageIndex = imageKeys.BaseImageIndex;
            ImageIndex = imageIndex;
            SelectedImageIndex = imageIndex;
        }
        #endregion

        #region HandleMouseClick()
        /// <summary>
        /// Handles a mouse click.
        /// </summary>
        /// <param name="form">The form that is processing the mouse click.</param>
        /// <param name="args">The mouse click arguments.</param>
        public virtual void HandleMouseClick(MainForm form, TreeNodeMouseClickEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            {
                form.DisplayItem(security);
            }
            else if (args.Button == MouseButtons.Right)
            {
                form.ChangeContextMenu(GenerateContextMenu());
            }
        }
        #endregion
        #endregion

        #region Private methods
        #region OnSecurityChanged()
        /// <summary>
        /// Updates the security.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        /// <remarks>
        /// This method will handle any cross-thread marshalling.
        /// </remarks>
        private void OnSecurityChanged(object source, PropertyChangedEventArgs args)
        {
            if (TreeView.InvokeRequired)
            {
                TreeView.Invoke(new PropertyChangedEventHandler(OnSecurityChanged), source, args);
            }
            else
            {
                if (args.PropertyName == "Security")
                {
                    ChangeSecurity((source as CruiseControl).Security);
                }
            }
        }
        #endregion

        #region OnPropertyChanged()
        /// <summary>
        /// Updates the node with any relevant changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Type":
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

            menu.Items.Add(GenerateValidateMenu());
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(GenerateChangeMenu());
            if (hasSecurity)
            {
                menu.Items.Add(GenerateRemoveMenu());
                menu.Items.Add(new ToolStripSeparator());
                foreach (TreeNode node in Nodes)
                {
                    if (node is ReflectionListTreeNode)
                    {
                        menu.Items.Add((node as ReflectionListTreeNode).GenerateAddMenu());
                    }
                }
            }

            return menu;
        }
        #endregion

        #region GenerateValidateMenu()
        /// <summary>
        /// Generates the validate menu item.
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem GenerateValidateMenu()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Validate security");
            menuItem.Image = CCNetConfig.Properties.Resources.checkmark_16x16;
            menuItem.Click += delegate(object sender, EventArgs e)
            {
                MainForm.QueueValidation(this);
                MainForm.StartValidation();
            };
            return menuItem;
        }
        #endregion

        #region GenerateRemoveMenu()
        /// <summary>
        /// Generates the remove menu item.
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem GenerateRemoveMenu()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Remove security");
            menuItem.Image = CCNetConfig.Properties.Resources.securitydelete_16x16;
            menuItem.Click += delegate(object sender, EventArgs e)
            {
                if (MessageBox.Show(MainForm,
                    "Are you sure you want to delete the security?",
                    "Confirm delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    configuration.Security = null;
                    ChangeSecurity(null);
                    UpdateDisplay();
                }
            };
            return menuItem;
        }
        #endregion

        #region GenerateChangeMenu()
        /// <summary>
        /// Generates the change menu item.
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem GenerateChangeMenu()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Change to");
            menuItem.Image = CCNetConfig.Properties.Resources.securitychange_16x16;
            List<Type> serverSecurityModes = CoreUtil.GetAllServerSecurityModes();
            foreach (Type securityMode in serverSecurityModes)
            {
                // Retrieve the display name of the security mode
                string displayName = securityMode.Name;
                DisplayNameAttribute attribute = CoreUtil.GetCustomAttribute<DisplayNameAttribute>(securityMode);
                if (attribute != null) displayName = attribute.DisplayName;

                // Add the actual menu item
                ToolStripMenuItem securityMenuItem = new ToolStripMenuItem(displayName);
                securityMenuItem.Image = CCNetConfig.Properties.Resources.applications_16x16;
                securityMenuItem.Tag = securityMode;
                menuItem.DropDownItems.Add(securityMenuItem);
                securityMenuItem.Click += delegate(object sender, EventArgs e)
                {
                    // Generate the new instance and update the display
                    ServerSecurity value = Activator.CreateInstance((sender as ToolStripMenuItem).Tag as Type) as ServerSecurity;
                    configuration.Security = value;
                    ChangeSecurity(value);
                    UpdateDisplay();
                };
            }

            return menuItem;
        }
        #endregion

        #region UpdateDisplay()
        /// <summary>
        /// Updates the display if this node is selected.
        /// </summary>
        private void UpdateDisplay()
        {
            if (TreeView.SelectedNode == this) MainForm.DisplayItem(security);
        }
        #endregion
        #endregion
    }
}
