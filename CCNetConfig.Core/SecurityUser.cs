using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Defines a security user.
    /// </summary>
    public abstract class SecurityUser
        : SecuritySetting
    {
        #region Private fields
        private string userName;
        #endregion

        #region Public properties
        #region UserName
        /// <summary>
        /// The login name of the user.
        /// </summary>
        [Description("The user (login) name of the user.")]
        [DisplayName("User Name")]
        [Category("Required")]
        [DefaultValue(null)]
        [Required(MinimumVersion = "1.5")]
        public virtual string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                FirePropertyChanged("UserName");
            }
        }
        #endregion

        #region DisplayName
        /// <summary>
        /// The login name of the user.
        /// </summary>
        [Description("The display name of the user.")]
        [DisplayName("Display Name")]
        [Category("Optional")]
        [DefaultValue(null)]
        public virtual string DisplayName { get; set; }
        #endregion
        #endregion
    }
}
