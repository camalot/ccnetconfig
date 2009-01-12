using System.Windows.Forms;
using CCNetConfig.Core;

namespace CCNetConfig.Components
{
    /// <summary>
    /// Displays an integration queue in the tree.
    /// </summary>
    public class IntegrationQueueTreeNode : TreeNode
    {
        #region Private fields
        private readonly Queue queue;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationQueueTreeNode"/> class.
        /// </summary>
        /// <param name="value">The queue.</param>
        /// <param name="imageIndex">The index of the image to use.</param>
        public IntegrationQueueTreeNode(Queue value, int imageIndex)
            : base(value.Name, imageIndex, imageIndex)
        {
            Tag = value.GetHashCode();
            queue = value;
        }
        #endregion

        #region Public properties
        #region Queue
        /// <summary>
        /// The associated queue.
        /// </summary>
        public Queue Queue
        {
            get { return queue; }
        }
        #endregion
        #endregion
    }
}