using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.Xml;

namespace CCNetConfig.CCNet.Security
{
    /// <summary>
    /// Stores the details on a user name authentication.
    /// </summary>
    [MinimumVersion("1.5")]
    [ReflectorName("simpleUser")]
    [DisplayName("Username")]
    public class UsernameAuthentication
        : SecurityUser
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
        [Required(MinimumVersion="1.5")]
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

        #region Public methods
        #region Serialize()
        /// <summary>
        /// Serialises the security setting to an <see cref="XmlElement"/>.
        /// </summary>
        /// <returns>The <see cref="XmlElement"/> containing the security setting configuration.</returns>
        public override XmlElement Serialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("simpleUser");
            root.SetAttribute("name", UserName);
            if (!string.IsNullOrEmpty(DisplayName)) root.SetAttribute("display", DisplayName);

            return root;
        }
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises an <see cref="XmlElement"/> containing security setting configuration.
        /// </summary>
        /// <param name="element">The <see cref="XmlElement"/> to deserialise.</param>
        public override void Deserialize(XmlElement element)
        {
            UserName = Util.GetElementOrAttributeValue("name", element);
            DisplayName = Util.GetElementOrAttributeValue("display", element);
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this security setting configuration.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            UsernameAuthentication newValue = new UsernameAuthentication();
            newValue.DisplayName = DisplayName;
            newValue.UserName = UserName;
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
            return string.Format("{0} (name only)",
                string.IsNullOrEmpty(UserName) ? "<new>" : UserName);
        }
        #endregion
        #endregion
    }
}
