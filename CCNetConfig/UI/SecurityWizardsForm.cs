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
    /// The menu form for all the security wizards.
    /// </summary>
    public partial class SecurityWizardsForm : Form
    {
        #region Private fields
        private CruiseControl configuration;
        #endregion

        #region Constructors
        /// <summary/>
        public SecurityWizardsForm(CruiseControl config)
        {
            InitializeComponent();
            configuration = config;
        }
        #endregion

        #region Private methods
        #region configureButton_Click()
        private void configureButton_Click(object sender, EventArgs e)
        {
            var wizard = new ConfigureSecurityWizard(configuration);
            wizard.Run();
        }
        #endregion

        #region importUsersButton_Click()
        private void importUsersButton_Click(object sender, EventArgs e)
        {
            // TODO: Import users wizard
            MessageBox.Show("Import users wizard", "TODO");
        }
        #endregion

        #region permissionsButton_Click()
        private void permissionsButton_Click(object sender, EventArgs e)
        {
            // TODO: Assign permissions wizard
            MessageBox.Show("Assign permissions wizard", "TODO");
        }
        #endregion
        #endregion
    }
}
