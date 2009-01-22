using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core.Components
{
    /// <summary>
    /// An attribute that will generate a node in the tree view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class ListTreeNodeAttribute
        : Attribute
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ListTreeNodeAttribute"/>.
        /// </summary>
        /// <param name="itemType">The type of item the node will work with.</param>
        /// <param name="nodeName">The name of the node.</param>
        /// <param name="property">The property this node is based on.</param>
        public ListTreeNodeAttribute(Type itemType, string property, string nodeName)
        {
            ItemType = itemType;
            NodeName = nodeName;
            Property = property;
        }

        /// <summary>
        /// Instantiates a new <see cref="ListTreeNodeAttribute"/>.
        /// </summary>
        /// <param name="itemType">The type of item the node will work with.</param>
        /// <param name="property">The property this node is based on.</param>
        public ListTreeNodeAttribute(Type itemType, string property)
            : this(itemType, property, property)
        {
        }
        #endregion

        #region Public properties
        #region ItemType
        /// <summary>
        /// The type of item that node will work with.
        /// </summary>
        public Type ItemType { get; private set; }
        #endregion

        #region NodeName 
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string NodeName { get; private set; }
        #endregion

        #region ImageKey
        /// <summary>
        /// An optional key to point to an image to use for the node. If the image cannot be found, 
        /// or this is not set then the parent image will be used.
        /// </summary>
        public string ImageKey { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// The property the node will be based on.
        /// </summary>
        public string Property { get; private set; }
        #endregion

        #region AddMenuText
        /// <summary>
        /// The text to use in the add menu.
        /// </summary>
        public string AddMenuText { get; set; }
        #endregion

        #region RemoveMenuText
        /// <summary>
        /// The text to use in the remove menu.
        /// </summary>
        public string RemoveMenuText { get; set; }
        #endregion
        #endregion
    }
}
