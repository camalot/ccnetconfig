using System.Windows.Forms;
using CCNetConfig.UI;

namespace CCNetConfig.Components.Nodes
{
    /// <summary>
    /// Allows interactive functionality for the node to be encapsulated.
    /// </summary>
    public interface IInteractiveTreeNode
    {
        #region HandleMouseClick()
        /// <summary>
        /// Handles a mouse click.
        /// </summary>
        /// <param name="form">The form that is processing the mouse clic.</param>
        /// <param name="args">The mouse click arguments.</param>
        void HandleMouseClick(MainForm form, TreeNodeMouseClickEventArgs args);
        #endregion
    }
}
