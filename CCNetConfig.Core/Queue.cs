using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Defines the configuration for a queue.
    /// </summary>
    public class Queue
        : ICCNetObject, ICloneable, ICCNetDocumentation, INotifyPropertyChanged
    {
        #region Private fields
        private string name;
        private QueueDuplicateHandlingMode handlingMode;
        private string lockQueues;
        private bool hasConfig = false;
        private List<Project> projects = new List<Project>();
        #endregion

        #region Public properties
        #region Name
        /// <summary>
        /// The name of the queue.
        /// </summary>
        [Description("The name of the queue. This must be unique within the configuration.")]
        [DisplayName("(Name)")]
        [Category("Required")]
        [DefaultValue(null)]
        [Required(MinimumVersion = "1.4.0")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged("Name");
                foreach (Project proj in projects)
                {
                    proj.Queue = name;
                }
            }
        }
        #endregion

        #region HandlingMode
        /// <summary>
        /// The handling mode for duplicates.
        /// </summary>
        [Description("How to handle duplicate force build requests.")]
        [DisplayName("Duplicates Handling Mode")]
        [Category("Optional")]
        [DefaultValue(QueueDuplicateHandlingMode.UseFirst)]
        public QueueDuplicateHandlingMode HandlingMode
        {
            get { return handlingMode; }
            set { handlingMode = value; }
        }
        #endregion

        #region LockQueues
        /// <summary>
        /// The queues that will be locked.
        /// </summary>
        [Description("The names of queues to be locked. Multiple queue names can be entered as long as they are separated by commas.")]
        [DisplayName("Queues to Lock")]
        [Category("Optional")]
        [DefaultValue(null)]
        public string LockQueues
        {
            get { return lockQueues; }
            set { lockQueues = value; }
        }
        #endregion

        #region HasConfig
        /// <summary>
        /// Marks whether this configuration comes from the file or is deduced from the project definitions.
        /// </summary>
        [Description("Defines whether this queue exists as a separate item in the configuration or not.")]
        [DisplayName("Has Configuration")]
        [Category("Optional")]
        [DefaultValue(false)]
        public bool HasConfig
        {
            get { return hasConfig; }
            set
            {
                hasConfig = value;
                FirePropertyChanged("HasConfig");
            }
        }
        #endregion

        #region DocumentationUri
        /// <summary>
        /// Gets the documentation URI.
        /// </summary>
        /// <value>The documentation URI.</value>
        [Browsable(false)]
        public Uri DocumentationUri
        {
            get { return new Uri("http://confluence.public.thoughtworks.org/display/CCNET/Queue+Configuration?decorator=printable"); }
        }
        #endregion

        #region ProjectCount
        /// <summary>
        /// The number of projects that are associated with this queue.
        /// </summary>
        [Description("The number of projects that are associated with this queue.")]
        [DisplayName("Number of Projects")]
        [Category("Information")]
        public int ProjectCount
        {
            get { return projects.Count; }
        }
        #endregion

        #region Projects
        /// <summary>
        /// The projects that are associated with this queue.
        /// </summary>
        [Browsable(false)]
        public List<Project> Projects
        {
            get { return projects; }
        }
        #endregion
        #endregion

        #region Public methods
        #region Serialize()
        /// <summary>
        /// Serialises a queue to an <see cref="XmlElement"/>.
        /// </summary>
        /// <returns>The <see cref="XmlElement"/> containing the queue configuration.</returns>
        public XmlElement Serialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("queue");
            root.SetAttribute("name", Util.CheckRequired(this, "name", name));
            if (handlingMode != QueueDuplicateHandlingMode.UseFirst) root.SetAttribute("duplicates", handlingMode.ToString());

            if (!string.IsNullOrEmpty(lockQueues))
            {
                XmlElement ele = doc.CreateElement("lockqueues");
                ele.InnerText = lockQueues;
                root.AppendChild(ele);
            }

            return root;
        }
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises an <see cref="XmlElement"/> containing queue configuration.
        /// </summary>
        /// <param name="element">The <see cref="XmlElement"/> to deserialise.</param>
        public void Deserialize(XmlElement element)
        {
            name = Util.GetElementOrAttributeValue("name", element);
            string duplicates = Util.GetElementOrAttributeValue("duplicates", element);
            lockQueues = Util.GetElementOrAttributeValue("lockqueues", element);
            hasConfig = true;

            if (!string.IsNullOrEmpty(duplicates))
            {
                handlingMode = (QueueDuplicateHandlingMode)Enum.Parse(typeof(QueueDuplicateHandlingMode), duplicates);
            }
            else
            {
                handlingMode = QueueDuplicateHandlingMode.UseFirst;
            }
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this queue configuration.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Queue newValue = new Queue();
            newValue.name = this.name;
            newValue.lockQueues = this.lockQueues;
            newValue.handlingMode = this.handlingMode;
            return newValue;
        }
        #endregion

        #region AddProject()
        /// <summary>
        /// Adds a new project and fires the PropertyChanged event.
        /// </summary>
        /// <param name="value">The project to add.</param>
        public void AddProject(Project value)
        {
            projects.Add(value);
            FirePropertyChanged("Projects");
        }
        #endregion

        #region RemoveProject()
        /// <summary>
        /// Removes an existing project and fires the PropertyChanged event.
        /// </summary>
        /// <param name="value">The project to remove.</param>
        public void RemoveProject(Project value)
        {
            if (projects.Contains(value))
            {
                projects.Remove(value);
                FirePropertyChanged("Projects");
            }
        }
        #endregion
        #endregion

        #region Public events
        #region PropertyChanged
        /// <summary>
        /// Notifies any listeners that a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #endregion

        #region Protected methods
        #region FirePropertyChanged()
        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="property">The name of the property that was changed.</param>
        protected virtual void FirePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
        #endregion
    }
}
