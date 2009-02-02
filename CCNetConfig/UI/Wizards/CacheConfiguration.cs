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

        #region Public methods
        #region GenerateConfig()
        /// <summary>
        /// Generates the configuration.
        /// </summary>
        /// <returns>The new configuration.</returns>
        public virtual SecurityCache GenerateConfig()
        {
            var cache = new SecurityCache();
            switch ((string)cacheMode.SelectedItem)
            {
                case "File based":
                    {
                        var newCache = new FileBasedSessionCache();
                        newCache.CachingMode = GetExpiryMode();
                        newCache.Duration = Convert.ToInt32(sessionLength.Value);
                        cache = newCache;
                    }
                    break;
                case "In memory":
                    {
                        var newCache = new InMemorySessionCache();
                        newCache.CachingMode = GetExpiryMode();
                        newCache.Duration = Convert.ToInt32(sessionLength.Value);
                        cache = newCache;
                    }
                    break;
            }
            return cache;
        }
        #endregion

        #region GenerateDescription()
        /// <summary>
        /// Generates the configuration.
        /// </summary>
        /// <returns>The new configuration.</returns>
        public virtual string GenerateDescription()
        {
            var description = "Default cache";
            switch ((string)cacheMode.SelectedItem)
            {
                case "File based":
                    description = GetDescription("File based cache");
                    break;
                case "In memory":
                    description = GetDescription("In memory cache");
                    break;
            }
            return description;
        }
        #endregion
        #endregion

        #region Private methods
        #region GetExpiryMode()
        /// <summary>
        /// Retrieves the selected expirt mode.
        /// </summary>
        /// <returns></returns>
        private SessionExpiryMode? GetExpiryMode()
        {
            switch ((string)expiryMode.SelectedItem)
            {
                case "Fixed":
                    return SessionExpiryMode.Fixed;
                case "Sliding":
                    return SessionExpiryMode.Sliding;
            }
            return null;
        }
        #endregion

        #region GetDescription()
        /// <summary>
        /// Generates the description.
        /// </summary>
        /// <returns></returns>
        private string GetDescription(string mode)
        {
            var expiry = GetExpiryMode();
            var description = string.Format("{0} ({1}{2} minutes)",
                mode,
                expiry.HasValue ? expiry.ToString() + " - " : string.Empty,
                sessionLength.Value);
            return description;
        }
        #endregion
        #endregion
    }
}
