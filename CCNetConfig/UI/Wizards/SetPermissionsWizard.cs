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
    public class SetPermissionsWizard
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
        public SetPermissionsWizard(CruiseControl config)
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
                if (MessageBox.Show("Setting permissions is only valid for internal security, do you want to change to internal security now?",
                    "Invalid security manager",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.No)
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
            steps.Add(new TextDisplayStep("This wizard will guide you through the steps of setting permissions for a project in this configuration.", "Welcome"));
            settings.AddRange(new string[] {
                "No project selected",
                "No default permission"
            });

            // Select the projec
            var projectSelection = new SelectProjectStep(configuration);
            var projectSelectionStep = new TemplateStep(projectSelection, 0, "Select Project");
            projectSelectionStep.NextHandler += () =>
            {
                settings[0] = string.Format("Configuring project '{0}'", projectSelection.SelectedProject);
                return true;
            };
            steps.Add(projectSelectionStep);

            // Default project permission
            var defaultProjectPermission = new SelectionStep("Default Project Permission",
                "What do you want as the default project permission:",
                "None",
                "Allow",
                "Deny",
                "Inherit");
            defaultProjectPermission.NextHandler += () =>
            {
                switch ((string)defaultProjectPermission.Selected)
                {
                    case "None":
                        settings[1] = "No default permission";
                        break;
                    case "Allow":
                        settings[1] = "Default project permission is allow";
                        break;
                    case "Deny":
                        settings[1] = "Default project permission is deny";
                        break;
                    case "Inherit":
                        settings[1] = "Default project permission is inherit";
                        break;
                }
                return true;
            };
            steps.Add(defaultProjectPermission);

            // Set the force/abort build permissions
            var defaultPermission = new SelectUsersStep(configuration);
            defaultPermission.Caption = "What are the default permissions:";
            var defaultPermissionStep = new TemplateStep(defaultPermission, 0, "Set force/abort build permissions");
            defaultPermissionStep.NextHandler += () =>
            {
                return true;
            };
            steps.Add(defaultPermissionStep);

            // Set the force/abort build permissions
            var forceBuildPermission = new SelectUsersStep(configuration);
            forceBuildPermission.Caption = "What are the allowed permissions for force/abort build:";
            var forceBuildPermissionStep = new TemplateStep(forceBuildPermission, 0, "Set force/abort build permissions");
            forceBuildPermissionStep.NextHandler += () =>
            {
                return true;
            };
            steps.Add(forceBuildPermissionStep);

            // Set the start/stop project permissions
            var startProjectPermission = new SelectUsersStep(configuration);
            startProjectPermission.Caption = "Which users are allowed to start/stop builds:";
            var startProjectPermissionStep = new TemplateStep(startProjectPermission, 0, "Set start/stop project permissions");
            startProjectPermissionStep.NextHandler += () =>
            {
                return true;
            };
            steps.Add(startProjectPermissionStep);

            // Set the send message permissions
            var sendMessagePermission = new SelectUsersStep(configuration);
            sendMessagePermission.Caption = "What are the allowed permissions for sending messages:";
            var sendMessagePermissionStep = new TemplateStep(sendMessagePermission, 0, "Set send message permissions");
            sendMessagePermissionStep.NextHandler += () =>
            {
                return true;
            };
            steps.Add(sendMessagePermissionStep);

            // Configuration mode step
            var confirmation = GenerateConfirmation();
            confirmation.NextHandler += () =>
            {
                if (changeManager) configuration.Security = new InternalServerSecurity();
                configuration.Projects[projectSelection.SelectedProject].Security = GeneratePermissions(defaultPermission,
                    forceBuildPermission,
                    startProjectPermission,
                    sendMessagePermission);
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
        #region GeneratePermissions()
        /// <summary>
        /// Generates the permissions for the security.
        /// </summary>
        /// <returns></returns>
        private DefaultProjectSecurity GeneratePermissions(SelectUsersStep defaultPermission,
            SelectUsersStep forceBuildPermission,
            SelectUsersStep startProjectPermission,
            SelectUsersStep sendMessagePermission)
        {
            var newSecurity = new DefaultProjectSecurity();
            var users = new Dictionary<string, UserPermission>();

            CheckUsersAndSetPermissions(users,
                defaultPermission,
                (u) => { u.DefaultRight = SecurityRight.Allow; },
                (u) => { u.DefaultRight = SecurityRight.Deny; });
            CheckUsersAndSetPermissions(users,
                forceBuildPermission,
                (u) => { u.ForceBuildRight = SecurityRight.Allow; },
                (u) => { u.ForceBuildRight = SecurityRight.Deny; });
            CheckUsersAndSetPermissions(users,
                startProjectPermission,
                (u) => { u.StartProjectRight = SecurityRight.Allow; },
                (u) => { u.StartProjectRight = SecurityRight.Deny; });
            CheckUsersAndSetPermissions(users,
                sendMessagePermission,
                (u) => { u.SendMessageRight = SecurityRight.Allow; },
                (u) => { u.SendMessageRight = SecurityRight.Deny; });

            foreach (var permission in users.Values)
            {
                newSecurity.Permissions.Add(permission);
            }

            return newSecurity;
        }
        #endregion

        #region CheckUsersAndSetPermissions()
        private void CheckUsersAndSetPermissions(Dictionary<string, UserPermission> users,
            SelectUsersStep step,
            Action<UserPermission> setAllowPermission,
            Action<UserPermission> setDenyPermission)
        {
            foreach (string user in step.AllowedUsers)
            {
                if (!users.ContainsKey(user))
                {
                    var newUser = new UserPermission();
                    newUser.UserName = user;
                    users.Add(user, newUser);
                }
                setAllowPermission(users[user]);
            }
            foreach (string user in step.DeniedUsers)
            {
                if (!users.ContainsKey(user))
                {
                    var newUser = new UserPermission();
                    newUser.UserName = user;
                    users.Add(user, newUser);
                }
                setDenyPermission(users[user]);
            }
        }
        #endregion

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
