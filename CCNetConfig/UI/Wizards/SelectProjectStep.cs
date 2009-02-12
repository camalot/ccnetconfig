using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;

namespace CCNetConfig.UI.Wizards
{
    /// <summary>
    /// A wizard step control for displaying all the projects.
    /// </summary>
    public partial class SelectProjectStep : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initialise the list.
        /// </summary>
        public SelectProjectStep(CruiseControl configuration)
        {
            InitializeComponent();

            if (configuration.Projects.Count > 0)
            {
                foreach (var project in configuration.Projects)
                {
                    projectList.Items.Add(project.Name);
                }
                projectList.SelectedIndex = 0;
            }
        }
        #endregion

        #region Public properties
        #region SelectedProject
        /// <summary>
        /// The currently selected project.
        /// </summary>
        public string SelectedProject
        {
            get { return projectList.SelectedItem as string; }
        }
        #endregion
        #endregion
    }
}
