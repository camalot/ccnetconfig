using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core;
using System.Windows.Forms;
using CCNetConfig.UI;
using CCNetConfig.Core.Components;
using System.ComponentModel;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// A generic tree node that has been added via attributes.
    /// </summary>
    public class ReflectionItemTreeNode
        : ValidatingTreeNode, IInteractiveTreeNode, IImageUpdateNode
    {
        #region Private fields
        private readonly ListTreeNodeAttribute source;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise a new <see cref="ReflectionItemTreeNode"/>.
        /// </summary>
        /// <param name="value">The associated data item.</param>
        /// <param name="source">The source of the data.</param>
        public ReflectionItemTreeNode(ICCNetObject value, ListTreeNodeAttribute source)
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
        }
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
            menu.Items.Add(GenerateRemoveMenu());

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
                    (Parent as ReflectionListTreeNode).RemoveItem(this);
                }
            };
            return menuItem;
        }
        #endregion
        #endregion
    }
}
