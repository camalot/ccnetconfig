using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;
using CCNetConfig.UI;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// A tree node that also handles validation.
    /// </summary>
    public abstract class ValidatingTreeNode
        : TreeNode
    {
        #region Private fields
        private ICCNetObject dataItem;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialises a new <see cref="ValidatingTreeNode"/>.
        /// </summary>
        /// <param name="dataItem"></param>
        public ValidatingTreeNode(ICCNetObject dataItem)
            : this(dataItem, string.Empty, 0, 0) { }

        /// <summary>
        /// Initialises a new <see cref="ValidatingTreeNode"/>.
        /// </summary>
        /// <param name="dataItem"></param>
        /// <param name="name"></param>
        public ValidatingTreeNode(ICCNetObject dataItem, string name)
            : this(dataItem, name, 0, 0) { }

        /// <summary>
        /// Initialises a new <see cref="ValidatingTreeNode"/>
        /// </summary>
        /// <param name="dataItem"></param>
        /// <param name="name"></param>
        /// <param name="indexImage"></param>
        /// <param name="selectedImageIndex"></param>
        public ValidatingTreeNode(ICCNetObject dataItem, string name, int indexImage, int selectedImageIndex)
            : base(name, indexImage, selectedImageIndex)
        {
            this.dataItem = dataItem;
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
        #region Validate()
        /// <summary>
        /// Validates the data item.
        /// </summary>
        /// <returns>A list of exceptions - this list will be empty if there are no validation errors.</returns>
        public virtual ValidationException[] Validate()
        {
            ValidationException[] errors = Validator.Validate(dataItem).ToArray();
            return errors;
        }
        #endregion
        #endregion
    }
}
