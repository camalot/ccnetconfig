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
    [ReflectorName("ldapUser")]
    [DisplayName("Active Directory")]
    public class ActiveDirectoryAuthentication
        : SecurityUser
    {
        #region Public properties
        #region DomainName
        /// <summary>
        /// The login name of the user.
        /// </summary>
        [Description("The domain name of the user.")]
        [DisplayName("Domain Name")]
        [Category("Optional")]
        [DefaultValue(null)]
        public virtual string DomainName { get; set; }
        #endregion

        #region DisplayName
        /// <summary>
        /// The login name of the user.
        /// </summary>
        [Browsable(false)]
        public override string DisplayName { get; set; }
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
            XmlElement root = doc.CreateElement("ldapUser");
            root.SetAttribute("name", UserName);
            if (!string.IsNullOrEmpty(DomainName)) root.SetAttribute("domain", DomainName);

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
            DomainName = Util.GetElementOrAttributeValue("domain", element);
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this security setting configuration.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            ActiveDirectoryAuthentication newValue = new ActiveDirectoryAuthentication();
            newValue.DomainName = DomainName;
            newValue.UserName = UserName;
            return newValue;
        }
        #endregion

        #region ToString()
        /// <summary>
        /// Domains a friendly message for the item.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} (LDAP)",
                string.IsNullOrEmpty(UserName) ? "<new>" : UserName);
        }
        #endregion
        #endregion
    }
}
