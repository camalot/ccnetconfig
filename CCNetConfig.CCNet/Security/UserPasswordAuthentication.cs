using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using CCNetConfig.Core;
using System.Xml;

namespace CCNetConfig.CCNet.Security
{
    /// <summary>
    /// Stores the details on a user name authentication.
    /// </summary>
    [MinimumVersion("1.5")]
    [ReflectorName("passwordUser")]
    [DisplayName("User/Password")]
    public class UserPasswordAuthentication
        : SecurityUser
    {
        #region Public properties
        #region Password
        /// <summary>
        /// The password for the user.
        /// </summary>
        [Description("The password for the user.")]
        [Category("Required")]
        [DefaultValue(null)]
        [PasswordPropertyText(true)]
        public virtual string Password { get; set; }
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
            XmlElement root = doc.CreateElement("passwordUser");
            root.SetAttribute("name", UserName);
            root.SetAttribute("password", Password);
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
            Password = Util.GetElementOrAttributeValue("password", element);
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
            UserPasswordAuthentication newValue = new UserPasswordAuthentication();
            newValue.DisplayName = DisplayName;
            newValue.UserName = UserName;
            newValue.Password = Password;
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
            return string.Format("{0} (password)",
                string.IsNullOrEmpty(UserName) ? "<new>" : UserName);
        }
        #endregion
        #endregion
    }
}
