using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;
using CCNetConfig.Components.Nodes;

namespace CCNetConfig.UI
{
    /// <summary>
    /// Displays the validation results.
    /// </summary>
    public partial class ValidationForm : Form
    {
        #region Private fields
        private readonly MainForm mainForm;
        private List<TreeNode> nodesToValidate = new List<TreeNode>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise a new <see cref="ValidationForm"/>.
        /// </summary>
        /// <param name="main">The <see cref="MainForm"/> that owns this form.</param>
        public ValidationForm(MainForm main)
        {
            InitializeComponent();
            Owner = main;
            mainForm = main;
        }
        #endregion

        #region Public methods
        #region Validate()
        /// <summary>
        /// Validates the entire configuration tree.
        /// </summary>
        /// <param name="value"></param>
        public virtual void Validate(TreeView value)
        {
            // Flatten out the tree first, so the progress bar will return some valid results
            nodesToValidate.Clear();
            FlattenTree(value.Nodes);
            StartValidation();
        }
        #endregion

        #region StartValidation()
        /// <summary>
        /// Starts validating all the queued nodes.
        /// </summary>
        public virtual void StartValidation()
        {
            validationResults.Items.Clear();
            validationLoader.RunWorkerAsync(nodesToValidate);
        }
        #endregion

        #region QueueNode()
        /// <summary>
        /// Queues a node for validation.
        /// </summary>
        /// <param name="value"></param>
        public virtual void QueueNode(TreeNode value)
        {
            nodesToValidate.Add(value);
        }
        #endregion
        #endregion

        #region Private methods
        #region FlattenTree()
        /// <summary>
        /// Flattens all the nodes in a collection.
        /// </summary>
        /// <param name="nodes">The nodes to flatten.</param>
        private void FlattenTree(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                QueueNode(node);
                FlattenTree(node.Nodes);
            }
        }
        #endregion

        #region ValidationForm_FormClosing()
        /// <summary>
        /// Prevents the user from closing down the window - instead they can only hide it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
            }
        }
        #endregion

        #region validationLoader_ProgressChanged()
        private void validationLoader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.Value = e.ProgressPercentage;
            if (e.UserState != null) statusMessage.Text = e.UserState.ToString();
        }
        #endregion

        #region validationLoader_DoWork()
        private void validationLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            EventHandler errorLogger = new EventHandler(delegate(object source, EventArgs args)
                        {
                            ValidationException error = source as ValidationException;
                            ListViewItem errorRow = new ListViewItem(
                                new string[] {
                                    error.ItemName,
                                    error.ItemType,
                                    error.Message
                                });
                            errorRow.Tag = error.Tag;
                            validationResults.Items.Add(errorRow);
                        });
            validationLoader.ReportProgress(0, "Validating configuration");
            int errorCount = 0;
            int position = 0;
            List<TreeNode> allNodes = e.Argument as List<TreeNode>;
            foreach (TreeNode node in allNodes)
            {
                if (validationLoader.CancellationPending) break;

                if (node is ValidatingTreeNode)
                {
                    ValidationException[] errors = (node as ValidatingTreeNode).Validate();
                    foreach (ValidationException error in errors)
                    {
                        error.Tag = node;
                        Invoke(errorLogger, error, EventArgs.Empty);
                        errorCount++;
                    }
                }

                validationLoader.ReportProgress(position++ * 100 / allNodes.Count);
            }

            // Check the outcome
            if (validationLoader.CancellationPending)
            {
                validationLoader.ReportProgress(100, "Validation cancelled");
            }
            else
            {
                if (errorCount == 0)
                {
                    validationLoader.ReportProgress(100, "No errors found");
                }
                else
                {
                    if (errorCount == 1)
                    {
                        validationLoader.ReportProgress(100, "1 error was found");
                    }
                    else
                    {
                        validationLoader.ReportProgress(100, string.Format("{0} errors were found", errorCount));
                    }
                }
            }
        }
        #endregion

        #region validationLoader_RunWorkerCompleted()
        private void validationLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentProgress.Value = 100;
            nodesToValidate.Clear();
        }
        #endregion

        #region validationResults_DoubleClick()
        private void validationResults_DoubleClick(object sender, EventArgs e)
        {
            if (validationResults.SelectedItems.Count > 0)
            {
                ListViewItem item = validationResults.SelectedItems[0];
                TreeNode node = item.Tag as TreeNode;
                node.EnsureVisible();
                node.TreeView.SelectedNode = node;
                mainForm.Focus();
            }
        }
        #endregion
        #endregion
    }
}
