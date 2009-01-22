using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// Defines all the image keys for use in security nodes.
    /// </summary>
    public struct SecurityImageKeys
    {
        #region Public fields
        /// <summary>
        /// The basic image for a security.
        /// </summary>
        public int BaseImageIndex;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise a new instance of a <see cref="SecurityImageKeys"/>.
        /// </summary>
        /// <param name="baseImageIndex">The basic image for security.</param>
        public SecurityImageKeys(int baseImageIndex)
        {
            BaseImageIndex = baseImageIndex;
        }
        #endregion
    }
}
