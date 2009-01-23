using System;
using System.Collections.Generic;
using System.Text;
using Merlin;
using MerlinStepLibrary;
using CCNetConfig.Core;

namespace CCNetConfig.UI
{
    /// <summary>
    /// The wizard for configuring security.
    /// </summary>
    public class ConfigureSecurityWizard
    {
        #region Private fields
        private CruiseControl configuration;
        private WizardController controller;
        private List<string> settings = new List<string>();
        #endregion

        #region Constructors
        /// <summary>
        /// Starts a new instance of <see cref="ConfigureSecurityWizard"/>.
        /// </summary>
        /// <param name="config"></param>
        public ConfigureSecurityWizard(CruiseControl config)
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
            var steps = new List<IStep>();
            controller = new WizardController(steps);

            // Welcome text
            steps.Add(new TextDisplayStep("This wizard will guide you through the steps of configuring security for this configuration", "Welcome"));

            // Configuration mode step
            var modeStep = new SelectionStep("Security Mode",
                "Please choose the type of security you want:",
                "None",
                "Internal");
            modeStep.NextHandler = () =>
            {
                settings.Clear();
                controller.DeleteAllAfterCurrent();
                switch ((string)modeStep.Selected)
                {
                    case "None":
                        settings.Add("No security");
                        controller.AddAfterCurrent(GenerateNoneSteps());
                        break;
                    case "Internal":
                        settings.AddRange(new string[] 
                            {
                                "Internal security",
                                "No cache",
                                "No XML logging",
                                "No default permission"
                            });
                        controller.AddAfterCurrent(GenerateInternalSteps());
                        break;
                }
                return true;
            };
            steps.Add(modeStep);
            steps.Add(new TextDisplayStep("Nothing"));

            var result = controller.StartWizard("Security Configuration Wizard");
        }
        #endregion
        #endregion

        #region Private methods
        #region GenerateNoneSteps()
        /// <summary>
        /// Generate the steps required for no security.
        /// </summary>
        /// <returns></returns>
        private List<IStep> GenerateNoneSteps()
        {
            var steps = new List<IStep>();

            // Confirmation page
            steps.Add(GenerateConfirmation());
            return steps;
        }
        #endregion

        #region GenerateInternalSteps()
        /// <summary>
        /// Generate the steps required for internal security.
        /// </summary>
        /// <returns></returns>
        private List<IStep> GenerateInternalSteps()
        {
            var steps = new List<IStep>();

            // Cache
            steps.Add(new TemplateStep(new CacheConfiguration(), 0, "Session Caching"));

            // XML logging
            steps.Add(new TemplateStep(new XmlAuditingConfiguration(), 0, "XML Auditing"));

            // Default permission
            var defaultPermission = new SelectionStep("Default Permission",
                "What do you want as the default permission:",
                "None",
                "Allow",
                "Deny");
            steps.Add(defaultPermission);

            // Confirmation page
            steps.Add(GenerateConfirmation());
            return steps;
        }
        #endregion

        #region GenerateConfirmation()
        /// <summary>
        /// Generates the confirmation step.
        /// </summary>
        /// <returns></returns>
        private IStep GenerateConfirmation()
        {
            var sep = Environment.NewLine + "> ";
            var details = string.Join(sep, settings.ToArray());
            var step = new TextDisplayStep("The following settings will be applied:" + sep +
                details, "Confirm settings");
            return step;
        }
        #endregion
        #endregion
    }
}
