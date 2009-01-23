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
    /// Custom configuration settings for the session cache.
    /// </summary>
    public partial class CacheConfiguration : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initialise a new instance of <see cref="CacheConfiguration"/>.
        /// </summary>
        public CacheConfiguration()
        {
            InitializeComponent();
            cacheMode.SelectedIndex = 0;
            expiryMode.SelectedIndex = 0;
        }
        #endregion
    }
}
