using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;
using CCNetConfig.Core.Wizard;
using CCNetConfig.Components.Nodes;
using CCNetConfig.Components.Wizards;

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

        private void configureButton_Click(object sender, EventArgs e)
        {
            SecurityModeWizardPage mode = new SecurityModeWizardPage();
            Form owner = this.Owner;
            WizardForm.Show(mode, "Configure Security", configuration, owner, Properties.Resources.logo48);
        }

        private void importUsersButton_Click(object sender, EventArgs e)
        {
            // TODO: Import users wizard
            MessageBox.Show("Import users wizard", "TODO");
        }

        private void permissionsButton_Click(object sender, EventArgs e)
        {
            // TODO: Assign permissions wizard
            MessageBox.Show("Assign permissions wizard", "TODO");
        }
    }
}
