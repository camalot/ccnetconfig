using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;
using CCNetConfig.CCNet.Security;

namespace CCNetConfig.UI.Wizards
{
    /// <summary>
    /// Custom configuration settings for XML auditing.
    /// </summary>
    public partial class XmlAuditingConfiguration : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initialise a new instance of <see cref="XmlAuditingConfiguration"/>.
        /// </summary>
        public XmlAuditingConfiguration()
        {
            InitializeComponent();
        }
        #endregion

        #region Public methods
        #region GenerateConfig()
        /// <summary>
        /// Generates the configuration.
        /// </summary>
        /// <returns>The new configuration.</returns>
        public virtual FileXmlLogger GenerateConfig()
        {
            FileXmlLogger logger = null;
            if (useXmlAuditing.Checked && (auditFailed.Checked || auditSuccessful.Checked))
            {
                logger = new FileXmlLogger
                {
                    LogFailureEvents = auditFailed.Checked,
                    LogSuccessfulEvents = auditSuccessful.Checked
                };
            }
            return logger;
        }
        #endregion

        #region GenerateDescription()
        /// <summary>
        /// Generates the configuration.
        /// </summary>
        /// <returns>The new configuration.</returns>
        public virtual string GenerateDescription()
        {
            var description = "No XML auditing";
            if (useXmlAuditing.Checked && (auditFailed.Checked || auditSuccessful.Checked))
            {
                if (auditFailed.Checked)
                {
                    if (auditSuccessful.Checked)
                    {
                        description = "XML logging for all events";
                    }
                    else
                    {
                        description = "XML logging for failed events only";
                    }
                }
                else
                {
                    description = "XML logging for successful events only";
                }
            }
            return description;
        }
        #endregion
        #endregion

        #region Private methods
        #region useXmlAuditing_CheckedChanged()
        /// <summary>
        /// Change the enabled status of the settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useXmlAuditing_CheckedChanged(object sender, EventArgs e)
        {
            xmlSettings.Enabled = useXmlAuditing.Checked;
        }
        #endregion
        #endregion
    }
}
