using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// Defines a node that can update its own image.
    /// </summary>
    public interface IImageUpdateNode
    {
        #region Methods
        /// <summary>
        /// Updates the node's image.
        /// </summary>
        void UpdateImage();
        #endregion
    }
}
