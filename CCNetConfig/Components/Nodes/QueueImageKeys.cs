using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// Defines all the image keys for use in queue nodes.
    /// </summary>
    public struct QueueImageKeys
    {
        #region Public fields
        /// <summary>
        /// The basic image for a queue.
        /// </summary>
        /// 
        public int BaseImageIndex;
        /// <summary>
        /// The image for a queue with configuration.
        /// </summary>
        public int ConfigImageIndex;

        /// <summary>
        /// The image for a project.
        /// </summary>
        public int ProjectImageIndex;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise a new instance of a <see cref="QueueImageKeys"/>.
        /// </summary>
        /// <param name="baseImageIndex">The basic image for a queue.</param>
        /// <param name="configImageIndex">The image for a queue with configuration.</param>
        /// <param name="projectImageIndex">The image for a project.</param>
        public QueueImageKeys(int baseImageIndex, int configImageIndex, int projectImageIndex)
        {
            BaseImageIndex = baseImageIndex;
            ConfigImageIndex = configImageIndex;
            ProjectImageIndex = projectImageIndex;
        }
        #endregion
    }
}
