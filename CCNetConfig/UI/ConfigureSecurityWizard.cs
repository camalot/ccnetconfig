using System;
using System.Collections.Generic;
using System.Text;
using Merlin;
using MerlinStepLibrary;
using CCNetConfig.Core;
using CCNetConfig.CCNet.Security;

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
        private ServerSecurity security;
        private Dictionary<Type, SecurityLogger> loggers = new Dictionary<Type, SecurityLogger>();
        private TemplateStep.VoidDelegate configureProjects = null;
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
                        configureProjects = () =>
                        {
                            foreach (var project in configuration.Projects)
                            {
                                project.Security = new ProjectSecurity();
                            }
                        };
                        security = new ServerSecurity();
                        break;
                    case "Internal":
                        settings.AddRange(new string[] 
                            {
                                "Internal security",
                                "No cache",
                                "No XML logging",
                                "No default server permission",
                                "No default project permission"
                            });
                        controller.AddAfterCurrent(GenerateInternalSteps());
                        security = new InternalServerSecurity();
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
            steps.Add(new TextDisplayStep("Security settings have been updated", "Finished"));
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
            var cacheConfig = new CacheConfiguration();
            var cacheStep = new TemplateStep(cacheConfig, 0, "Session Caching");
            cacheStep.NextHandler += () =>
            {
                (security as InternalServerSecurity).Cache = cacheConfig.GenerateConfig();
                settings[1] = cacheConfig.GenerateDescription();
                return true;
            };
            steps.Add(cacheStep);

            // XML logging
            var xmlConfig = new XmlAuditingConfiguration();
            var xmlStep = new TemplateStep(xmlConfig, 0, "XML Auditing");
            xmlStep.StepReachedHandler += () =>
            {
                if (loggers.ContainsKey(typeof(FileXmlLogger)))
                {
                    loggers.Remove(typeof(FileXmlLogger));
                }
            };
            xmlStep.NextHandler += () =>
            {
                var newLogger = xmlConfig.GenerateConfig();
                if (newLogger != null) loggers.Add(typeof(FileXmlLogger), newLogger);
                settings[2] = xmlConfig.GenerateDescription();
                return true;
            };
            steps.Add(xmlStep);

            // Default server permission
            var defaultServerPermission = new SelectionStep("Default Server Permission",
                "What do you want as the default permission at the server level:",
                "None",
                "Allow",
                "Deny");
            defaultServerPermission.NextHandler += () =>
            {
                switch ((string)defaultServerPermission.Selected)
                {
                    case "None":
                        (security as InternalServerSecurity).DefaultRight = null;
                        settings[3] = "No default server permission";
                        break;
                    case "Allow":
                        (security as InternalServerSecurity).DefaultRight = SecurityRight.Allow;
                        settings[3] = "Default server permission is allow";
                        break;
                    case "Deny":
                        (security as InternalServerSecurity).DefaultRight = SecurityRight.Deny;
                        settings[3] = "Default server permission is deny";
                        break;
                }
                return true;
            };
            steps.Add(defaultServerPermission);


            // Default server permission
            var defaultProjectPermission = new SelectionStep("Default Project Permission",
                "What do you want as the default permission at the project level:",
                "None",
                "Allow",
                "Deny");
            defaultProjectPermission.NextHandler += () =>
            {
                SecurityRight? right = null;
                switch ((string)defaultProjectPermission.Selected)
                {
                    case "None":
                        settings[4] = "No default project permission";
                        break;
                    case "Allow":
                        right = SecurityRight.Allow;
                        settings[4] = "Default project permission is allow";
                        break;
                    case "Deny":
                        right = SecurityRight.Deny;
                        settings[4] = "Default project permission is deny";
                        break;
                }
                configureProjects = () =>
                {
                    foreach (var project in configuration.Projects)
                    {
                        project.Security = new DefaultProjectSecurity
                        {
                            DefaultRight = right
                        };
                    }
                };
                return true;
            };
            steps.Add(defaultProjectPermission);

            // Confirmation page
            steps.Add(GenerateConfirmation());
            steps.Add(new TextDisplayStep("Security settings have been updated", "Finished"));
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
            var step = new TextDisplayStep(string.Empty, "Confirm settings");
            step.StepReachedHandler += () =>
            {
                var details = string.Join(sep, settings.ToArray());
                step.Text = "The following settings will be applied:" + sep + details;
            };
            step.NextHandler += () =>
            {
                if (security is InternalServerSecurity)
                {
                    (security as InternalServerSecurity).AuditLoggers.AddRange(loggers.Values);
                }
                configuration.Security = security;
                configureProjects.Invoke();
                return true;
            };
            return step;
        }
        #endregion
        #endregion
    }
}
