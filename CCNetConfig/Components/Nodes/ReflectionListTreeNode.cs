using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;
using CCNetConfig.UI;
using CCNetConfig.Core.Components;
using System.Reflection;
using System.Collections;
using CoreUtil = CCNetConfig.Core.Util;
using System.ComponentModel;
using System.Drawing;
using System.Resources;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// A generic list tree node that has been added via attributes.
    /// </summary>
    public class ReflectionListTreeNode
        : TreeNode, IInteractiveTreeNode, IImageUpdateNode
    {
        #region Private fields
        private ICCNetObject dataItem;
        private readonly ListTreeNodeAttribute source;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise a new <see cref="ReflectionListTreeNode"/>.
        /// </summary>
        /// <param name="value">The associated data item.</param>
        /// <param name="source">The source of the data.</param>
        public ReflectionListTreeNode(ICCNetObject value, ListTreeNodeAttribute source)
        {
            dataItem = value;
            this.source = source;
        }
        #endregion

        #region Public properties
        #region DataItem
        /// <summary>
        /// The underlying data item.
        /// </summary>
        public virtual ICCNetObject DataItem
        {
            get { return dataItem; }
            protected set { dataItem = value; }
        }
        #endregion

        #region MainForm
        /// <summary>
        /// The main form that this node belongs to.
        /// </summary>
        public MainForm MainForm
        {
            get { return TreeView.FindForm() as MainForm; }
        }
        #endregion
        #endregion

        #region Public methods
        #region UpdateImage()
        /// <summary>
        /// Updates the image.
        /// </summary>
        public virtual void UpdateImage()
        {
            int imageIndex = Parent.ImageIndex;
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

        #region Refresh()
        /// <summary>
        /// Refreshes all the child nodes.
        /// </summary>
        public void Refresh()
        {
            Nodes.Clear();
            if (dataItem != null)
            {
                IEnumerable data = RetrieveDataList();
                if (data != null)
                {
                    foreach (object listItem in data)
                    {
                        AddItemNode(listItem);
                    }
                }
            }
        }
        #endregion

        #region RemoveItem()
        /// <summary>
        /// Removes an item.
        /// </summary>
        /// <param name="node">The node containing the item.</param>
        public void RemoveItem(ReflectionItemTreeNode node)
        {
            IList data = RetrieveDataList();
            data.Remove(node.DataItem);
            Nodes.Remove(node);
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

        #region GenerateAddMenu()
        /// <summary>
        /// Generates the add menu item.
        /// </summary>
        /// <returns></returns>
        public ToolStripMenuItem GenerateAddMenu()
        {
            string menuText = string.Format("Add new {0}", source.ItemType.Name);
            if (!string.IsNullOrEmpty(source.AddMenuText)) menuText = source.AddMenuText;
            ToolStripMenuItem menuItem = new ToolStripMenuItem(menuText);

            // Set the menu image
            Bitmap image = Properties.Resources.add_16x16;
            if (!string.IsNullOrEmpty(source.ImageKey))
            {
                try
                {
                    image = Properties.Resources.ResourceManager.GetObject(source.ImageKey) as Bitmap;
                }
                catch (MissingManifestResourceException) { }
            }
            menuItem.Image = image;

            List<Type> itemTypes = CoreUtil.GetAllItemsOfType(source.ItemType);
            foreach (Type itemType in itemTypes)
            {
                if (!itemType.IsAbstract)
                {
                    // Retrieve the display name of the item
                    string displayName = itemType.Name;
                    DisplayNameAttribute attribute = CoreUtil.GetCustomAttribute<DisplayNameAttribute>(itemType);
                    if (attribute != null) displayName = attribute.DisplayName;

                    // Add the actual menu item
                    ToolStripMenuItem itemMenuItem = new ToolStripMenuItem(displayName);
                    itemMenuItem.Image = CCNetConfig.Properties.Resources.applications_16x16;
                    itemMenuItem.Tag = itemType;
                    menuItem.DropDownItems.Add(itemMenuItem);
                    itemMenuItem.Click += delegate(object sender, EventArgs e)
                    {
                        IList data = RetrieveDataList();
                        if (data != null)
                        {
                            object newItem = Activator.CreateInstance(
                                (sender as ToolStripMenuItem).Tag as Type);
                            data.Add(newItem);
                            TreeView.SelectedNode = AddItemNode(newItem);
                            TreeView.SelectedNode.EnsureVisible();
                        }
                        else
                        {
                            MessageBox.Show("Unable to add to parent, list has not be initialised",
                                "Error!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    };
                }
            }

            return menuItem;
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
            menu.Items.Add(GenerateAddMenu());

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

        #region RetrieveDataList()
        /// <summary>
        /// Retrieves the underlying data list.
        /// </summary>
        /// <returns></returns>
        private IList RetrieveDataList()
        {
            IList data = null;
            Type dataType = dataItem.GetType();
            PropertyInfo property = dataType.GetProperty(source.Property);
            if (property != null) data = property.GetValue(dataItem, new object[0]) as IList;
            return data;
        }
        #endregion

        #region AddItemNode()
        /// <summary>
        /// Adds a new item to this item.
        /// </summary>
        /// <param name="listItem"></param>
        private ReflectionItemTreeNode AddItemNode(object listItem)
        {
            ReflectionItemTreeNode node = new ReflectionItemTreeNode(listItem as ICCNetObject, source);
            Nodes.Add(node);
            node.UpdateImage();
            return node;
        }
        #endregion
        #endregion
    }
}
