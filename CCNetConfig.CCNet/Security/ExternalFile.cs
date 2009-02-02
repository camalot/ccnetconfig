using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.CCNet.Security
{
    /// <summary>
    /// Stores a reference to an external file.
    /// </summary>
    [MinimumVersion("1.5")]
    [DisplayName("External File")]
    public class ExternalFile
        : ExternalFileBase
    {
        #region Public properties
        #region Location
        /// <summary>
        /// The location of the file.
        /// </summary>
        [Description("The location of the file.")]
        [Category("Optional")]
        [DefaultValue(null)]
        [Editor(typeof(OpenFileDialogUIEditor), typeof(UITypeEditor))]
        [FileTypeFilter("All files (*.*)|*.*" )]
	    [OpenFileDialogTitle("External file location")]
        public string Location { get; set; }
        #endregion
        #endregion

        #region Public methods
        #region Serialize()
        /// <summary>
        /// Serialises the security setting to an <see cref="XmlElement"/>.
        /// </summary>
        /// <returns>The <see cref="XmlElement"/> containing the security setting configuration.</returns>
        public virtual XmlElement Serialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("file");
            root.InnerText = Location;
            return root;
        }
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises an <see cref="XmlElement"/> containing security setting configuration.
        /// </summary>
        /// <param name="element">The <see cref="XmlElement"/> to deserialise.</param>
        public virtual void Deserialize(XmlElement element)
        {
            Location = element.InnerText;
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this security setting configuration.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            var newValue = new ExternalFile();
            CopyTo(newValue);
            return newValue;
        }
        #endregion

        #region ToString()
        /// <summary>
        /// Displays a friendly message for the item.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}",
                string.IsNullOrEmpty(Location) ? "<new>" : Location).Trim();
        }
        #endregion
        #endregion

        #region Protected methods
        #region CopyTo()
        /// <summary>
        /// Copies the settings to another instance of this class.
        /// </summary>
        /// <param name="value">The target instance.</param>
        protected virtual void CopyTo(ExternalFile value)
        {
            value.Location = Location;
        }
        #endregion
        #endregion
    }
}
