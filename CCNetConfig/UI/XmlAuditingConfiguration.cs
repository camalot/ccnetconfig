using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CCNetConfig.UI
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
