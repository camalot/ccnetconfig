using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;

namespace CCNetConfig.UI
{
    public partial class QueueProjectLinkForm : Form
    {
        private Dictionary<string, Project> projectsLinks = new Dictionary<string,Project>();

        public QueueProjectLinkForm()
        {
            InitializeComponent();
        }

        public void LoadProjects(Queue queue, CruiseControl configuration)
        {
            projectsList.Items.Clear();
            foreach (Project project in configuration.Projects)
            {
                string display = project.Name;
                if (!string.IsNullOrEmpty(project.Queue)) display += " (" + project.Queue + ")";
                projectsList.Items.Add(display, queue.Name == project.Queue);
                projectsLinks.Add(display, project);
            }
            queueLabel.Text = queue.Name;
        }

        public Project[] SelectedProjects
        {
            get
            {
                List<Project> projects = new List<Project>();
                foreach (string projectName in projectsList.CheckedItems)
                {
                    projects.Add(projectsLinks[projectName]);
                }
                return projects.ToArray();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (Saved != null) Saved(this, EventArgs.Empty);
            Close();
        }

        public event EventHandler Saved;
    }
}
