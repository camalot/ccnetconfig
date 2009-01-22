using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using CCNetConfig.Core;
using System.Windows.Forms;
using CCNetConfig.UI;
using System.Reflection;
using CoreUtil = CCNetConfig.Core.Util;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// A generic tree node that has been added via attributes.
    /// </summary>
    public class ReflectionInstanceTreeNode
        : ValidatingTreeNode, IInteractiveTreeNode, IImageUpdateNode
    {
        #region Private fields
        private ICCNetObject parent;
        private readonly InstanceTreeNodeAttribute source;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise a new <see cref="ReflectionInstanceTreeNode"/>.
        /// </summary>
        /// <param name="value">The associated data item.</param>
        /// <param name="source">The source of the data.</param>
        public ReflectionInstanceTreeNode(ICCNetObject value, InstanceTreeNodeAttribute source)
            : base(value)
        {
            this.source = source;
            if (value != null)
            {
                Text = value.ToString();
                if (value is INotifyPropertyChanged)
                {
                    (value as INotifyPropertyChanged).PropertyChanged += OnPropertyChanged;
                }
            }
            this.parent = value;
            DataItem = RetrieveDataValue();

            ReflectionHelper.GenerateChildNodes(this, DataItem);
        }
        #endregion

        #region Public methods
        #region UpdateImage()
        /// <summary>
        /// Updates the image.
        /// </summary>
        public virtual void UpdateImage()
        {
            int imageIndex = (Parent == null) ? 0 : Parent.ImageIndex;
            if (!string.IsNullOrEmpty(source.ImageKey) &&
                (TreeView != null) && (TreeView.ImageList != null))   // Just to be safe
            {
                if (TreeView.ImageList.Images.ContainsKey(source.ImageKey))
                {
                    imageIndex = TreeView.ImageList.Images.IndexOfKey(source.ImageKey);
                }
            }
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
                form.DisplayItem(DataItem);
            }
            else if (args.Button == MouseButtons.Right)
            {
                form.ChangeContextMenu(GenerateContextMenu());
            }
        }
        #endregion
        #endregion

        #region Private methods
        #region OnPropertyChanged()
        /// <summary>
        /// Updates the node with any relevant changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Text = DataItem.ToString();
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
            if ((DataItem != null) && (DataItem.Serialize() != null))
            {
                menu.Items.Add(GenerateRemoveMenu());
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
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Validate");
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
            string text = "Remove item";
            if (!string.IsNullOrEmpty(source.RemoveMenuText)) text = source.RemoveMenuText;
            ToolStripMenuItem menuItem = new ToolStripMenuItem(text);
            menuItem.Image = CCNetConfig.Properties.Resources.delete_16x16;
            menuItem.Click += delegate(object sender, EventArgs e)
            {
                if (MessageBox.Show(MainForm,
                    "Are you sure you want to delete this item?",
                    "Confirm delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    DataItem = Activator.CreateInstance(source.ItemType) as ICCNetObject;
                    ChangeDataValue(null);
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
            List<Type> allowedItems = CoreUtil.GetAllItemsOfType(source.ItemType);
            foreach (Type allowedItem in allowedItems)
            {
                // Retrieve the display name of the security mode
                string displayName = allowedItem.Name;
                DisplayNameAttribute attribute = CoreUtil.GetCustomAttribute<DisplayNameAttribute>(allowedItem);
                if (attribute != null) displayName = attribute.DisplayName;

                // Add the actual menu item
                ToolStripMenuItem securityMenuItem = new ToolStripMenuItem(displayName);
                securityMenuItem.Image = CCNetConfig.Properties.Resources.applications_16x16;
                securityMenuItem.Tag = allowedItem;
                menuItem.DropDownItems.Add(securityMenuItem);
                securityMenuItem.Click += delegate(object sender, EventArgs e)
                {
                    // Generate the new instance and update the display
                    ICCNetObject value = Activator.CreateInstance((sender as ToolStripMenuItem).Tag as Type) as ICCNetObject;
                    DataItem = value;
                    ChangeDataValue(value);
                    UpdateDisplay();

                    Nodes.Clear();
                    ReflectionHelper.GenerateChildNodes(this, DataItem);
                };
            }

            return menuItem;
        }
        #endregion

        #region RetrieveDataValue()
        /// <summary>
        /// Retrieves the underlying data value.
        /// </summary>
        /// <returns></returns>
        private ICCNetObject RetrieveDataValue()
        {
            ICCNetObject data = null;
            Type dataType = parent.GetType();
            PropertyInfo property = dataType.GetProperty(source.Property);
            if (property != null) data = property.GetValue(parent, new object[0]) as ICCNetObject;
            if (data == null) data = Activator.CreateInstance(source.ItemType) as ICCNetObject;
            return data;
        }
        #endregion

        #region ChangeDataValue()
        /// <summary>
        /// Changes the underlying data value.
        /// </summary>
        /// <returns></returns>
        private ICCNetObject ChangeDataValue(ICCNetObject value)
        {
            ICCNetObject data = null;
            Type dataType = parent.GetType();
            PropertyInfo property = dataType.GetProperty(source.Property);
            if (property != null) property.SetValue(parent, value, new object[0]);
            return data;
        }
        #endregion

        #region UpdateDisplay()
        /// <summary>
        /// Updates the display if this node is selected.
        /// </summary>
        private void UpdateDisplay()
        {
            if (TreeView.SelectedNode == this) MainForm.DisplayItem(DataItem);
        }
        #endregion
        #endregion
    }
}
