using System;
using System.Windows.Forms;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using CoreUtil = CCNetConfig.Core.Util;
using System.Collections.Generic;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// Helper class for generating dynamic nodes based on attributes.
    /// </summary>
    public static class ReflectionHelper
    {
        #region Public methods
        #region GenerateChildNodes()
        /// <summary>
        /// Generates the child nodes based on the attributes.
        /// </summary>
        /// <param name="parent">The node to add the children to.</param>
        /// <param name="source">The object to retrieve the settings from.</param>
        public static void GenerateChildNodes(TreeNode parent, ICCNetObject source)
        {
            if (source != null)
            {
                Type sourceType = source.GetType();
                GenerateListNodes(parent, source, sourceType);
                GenerateInstanceNodes(parent, source, sourceType);
            }
            UpdateImages(parent);
        }
        #endregion

        #region RemoveDynamicNodes()
        /// <summary>
        /// Removes any nodes that were added dynamically (i.e. by GenerateChildNodes()).
        /// </summary>
        /// <param name="parent">The node to remove the dynamic nodes from.</param>
        public static void RemoveDynamicNodes(TreeNode parent)
        {
            // Find any dynamic nodes ...
            var dynamicNodes = new List<TreeNode>();
            foreach (TreeNode node in parent.Nodes)
            {
                if ((node is ReflectionInstanceTreeNode) ||
                    (node is ReflectionListTreeNode))
                {
                    dynamicNodes.Add(node);
                }
            }

            // ... and remove them
            foreach (var node in dynamicNodes)
            {
                parent.Nodes.Remove(node);
            }
        }
        #endregion
        #endregion

        #region Private methods
        #region GenerateInstanceNodes()
        /// <summary>
        /// Generates all the instance nodes.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="source"></param>
        /// <param name="sourceType"></param>
        private static void GenerateInstanceNodes(TreeNode parent, ICCNetObject source, Type sourceType)
        {
            InstanceTreeNodeAttribute[] attributes = CoreUtil.GetCustomAttributes<InstanceTreeNodeAttribute>(sourceType);
            foreach (InstanceTreeNodeAttribute attribute in attributes)
            {
                ReflectionInstanceTreeNode node = new ReflectionInstanceTreeNode(source, attribute);
                node.Text = attribute.NodeName;
                parent.Nodes.Add(node);
            }
        }
        #endregion

        #region GenerateListNodes()
        /// <summary>
        /// Generates all the list nodes.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="source"></param>
        /// <param name="sourceType"></param>
        private static void GenerateListNodes(TreeNode parent, ICCNetObject source, Type sourceType)
        {
            ListTreeNodeAttribute[] attributes = CoreUtil.GetCustomAttributes<ListTreeNodeAttribute>(sourceType);
            foreach (ListTreeNodeAttribute attribute in attributes)
            {
                ReflectionListTreeNode node = new ReflectionListTreeNode(source, attribute);
                node.Text = attribute.NodeName;
                parent.Nodes.Add(node);
                node.Refresh();
            }
        }
        #endregion

        #region UpdateImages()
        /// <summary>
        /// Updates all the images on the nodes.
        /// </summary>
        /// <param name="node"></param>
        private static void UpdateImages(TreeNode node)
        {
            if (node is IImageUpdateNode) (node as IImageUpdateNode).UpdateImage();
            foreach (TreeNode childNode in node.Nodes)
            {
                UpdateImages(childNode);
            }
        }
        #endregion
        #endregion
    }
}
