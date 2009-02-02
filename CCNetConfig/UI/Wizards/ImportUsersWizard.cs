using System;
using System.Collections.Generic;
using System.Text;
using Merlin;
using MerlinStepLibrary;
using CCNetConfig.Core;
using CCNetConfig.CCNet.Security;
using System.IO;
using System.Windows.Forms;

namespace CCNetConfig.UI.Wizards
{
    /// <summary>
    /// The wizard for configuring security.
    /// </summary>
    public class ImportUsersWizard
    {
        #region Private fields
        private CruiseControl configuration;
        private WizardController controller;
        private bool changeManager = false;
        private List<string> settings = new List<string>();
        #endregion

        #region Constructors
        /// <summary>
        /// Starts a new instance of <see cref="ConfigureSecurityWizard"/>.
        /// </summary>
        /// <param name="config"></param>
        public ImportUsersWizard(CruiseControl config)
        {
            configuration = config;
        }
        #endregion

        #region Public methods
        #region Run()
        /// <summary>
        /// Runs the wizard.
        /// </summary>
        public void Run()
        {
            if (!(configuration.Security is InternalServerSecurity))
            {
                if (MessageBox.Show("Importing users is only valid for internal security, do you want to change to internal security now?",
                    "Invalid security manager",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    changeManager = true;
                }
            }
            var steps = new List<IStep>();
            controller = new WizardController(steps);

            // Welcome text
            steps.Add(new TextDisplayStep("This wizard will guide you through the steps of importing users from a CSV file for this configuration", "Welcome"));

            // Display users step
            var users = new UserDisplay();
            var userStep = new TemplateStep(users, 0, "Select Users to Import");
            userStep.NextHandler += () =>
            {
                settings.Clear();
                settings.Add(users.GenerateConfirmation());
                return true;
            };

            // File selection step
            var fileStep = new FileSelectionStep("Select Import File", "What CSV file to you want to import:");
            fileStep.NextHandler += () =>
            {
                if (!File.Exists(fileStep.SelectedFullPath))
                {
                    MessageBox.Show("Unable to find the file '" + fileStep.SelectedFullPath + "'",
                        "Invalid import file",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    try
                    {
                        users.LoadCsv(fileStep.SelectedFullPath);
                        return true;
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Unable to load the file '" + fileStep.SelectedFullPath + "'" + Environment.NewLine + error.Message,
                            "Invalid import file",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return false;
                    }
                }
            };
            steps.Add(fileStep);
            steps.Add(userStep);

            // Configuration mode step
            var confirmation = GenerateConfirmation();
            confirmation.NextHandler += () =>
            {
                if (changeManager) configuration.Security = new InternalServerSecurity();
                users.ApplyConfiguration(configuration);
                configuration.Security = configuration.Security;    // Force a refresh
                return true;
            };
            steps.Add(confirmation);
            steps.Add(new TextDisplayStep("Users have been imported", "Finished"));

            var result = controller.StartWizard("Security Configuration Wizard");
        }
        #endregion
        #endregion

        #region Private methods
        #region GenerateConfirmation()
        /// <summary>
        /// Generates the confirmation step.
        /// </summary>
        /// <returns></returns>
        private TextDisplayStep GenerateConfirmation()
        {
            var sep = Environment.NewLine + "> ";
            var step = new TextDisplayStep(string.Empty, "Confirm settings");
            step.StepReachedHandler += () =>
            {
                var details = string.Join(sep, settings.ToArray());
                step.Text = "The following settings will be applied:" + sep + details;
            };
            return step;
        }
        #endregion
        #endregion
    }
}
