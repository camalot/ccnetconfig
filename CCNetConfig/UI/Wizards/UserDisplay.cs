using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using CCNetConfig.Core;
using CCNetConfig.CCNet.Security;

namespace CCNetConfig.UI.Wizards
{
    /// <summary>
    /// The wizard step for importing users.
    /// </summary>
    public partial class UserDisplay : UserControl
    {
        private string inputSourceFile;

        /// <summary>
        /// Initialise a new instance of <see cref="UserDisplay"/>.
        /// </summary>
        public UserDisplay()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load a CSV file.
        /// </summary>
        /// <param name="sourceFile"></param>
        public virtual void LoadCsv(string sourceFile)
        {
            inputSourceFile = sourceFile;
            var data = CsvParser.Parse(sourceFile);
            userList.Items.Clear();
            foreach (var line in data)
            {
                var user = new ListViewItem(
                    new string[] {
                        line[0],
                        line.Length > 1 ? line[1] : string.Empty,
                        line.Length > 2 ? "Yes" : "No"
                    });
                user.Checked = true;
                user.Tag = line;
                userList.Items.Add(user);
            }
        }

        /// <summary>
        /// Generate the confirmation details.
        /// </summary>
        /// <returns></returns>
        public string GenerateConfirmation()
        {
            return string.Format("Import {0} users from '{1}'", 
                userList.CheckedIndices.Count,
                inputSourceFile);
        }

        /// <summary>
        /// Applies the configuration settings.
        /// </summary>
        /// <param name="configuration"></param>
        public void ApplyConfiguration(CruiseControl configuration)
        {
            var security = (configuration.Security) as InternalServerSecurity;
            foreach (ListViewItem user in userList.CheckedItems)
            {
                var data = user.Tag as string[];
                if (data.Length > 2)
                {
                    var userValue = new UserPasswordAuthentication();
                    userValue.UserName = data[0];
                    userValue.DisplayName = data[1];
                    userValue.Password = data[2];
                    security.Users.Add(userValue);
                }
                else
                {
                    var userValue = new UsernameAuthentication();
                    userValue.UserName = data[0];
                    if (data.Length > 1) userValue.DisplayName = data[1];
                    security.Users.Add(userValue);
                }
            }
        }
    }
}
