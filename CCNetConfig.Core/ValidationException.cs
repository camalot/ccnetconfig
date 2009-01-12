using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core
{
    /// <summary>
    /// An exception that occurred during validation.
    /// </summary>
    public class ValidationException
        : Exception
    {
        #region Private fields
        private string itemType;
        private string itemName;
        private object tag;
        #endregion

        #region Constructors
        /// <summary>
        /// Starts a new <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="message"></param>
        public ValidationException(string itemType, string message)
            : base(message)
        {
            this.itemType = itemType;
        }
        #endregion

        #region Public properties
        #region ItemType
        /// <summary>
        /// The type of item.
        /// </summary>
        public string ItemType
        {
            get { return itemType; }
        }
        #endregion

        #region ItemName
        /// <summary>
        /// The name of the item.
        /// </summary>
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        #endregion

        #region Tag
        /// <summary>
        /// An associated data item.
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        #endregion
        #endregion
    }
}
