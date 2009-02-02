using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Xml;

namespace CCNetConfig.CCNet.Security
{
    /// <summary>
    /// A permission for a role (a group of users).
    /// </summary>
    [MinimumVersion("1.5")]
    [ReflectorName("rolePermission")]
    [DisplayName("Role Permission")]
    public class RolePermission
        : SecurityPermission
    {
        #region Private fields
        private string roleName;
        private CloneableList<string> users = new CloneableList<string>();
        #endregion

        #region Public properties
        #region RoleName
        /// <summary>
        /// The name of the role.
        /// </summary>
        [Description("The name of the role.")]
        [DisplayName("Role Name")]
        [Category("Required")]
        [DefaultValue(null)]
        [Required(MinimumVersion = "1.5")]
        public virtual string RoleName
        {
            get { return roleName; }
            set
            {
                roleName = value;
                FirePropertyChanged("RoleName");
            }
        }
        #endregion

        #region Users
        /// <summary>
        /// The users associated with this role.
        /// </summary>
        [Description("The users associated with this role.")]
        [Category("Required")]
        [Required(MinimumVersion = "1.5")]
        public virtual CloneableList<string> Users
        {
            get { return users; }
        }
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
            XmlElement root = doc.CreateElement("rolePermission");
            root.SetAttribute("name", RoleName);
            foreach (var user in users)
            {
                var userEl = doc.CreateElement("userName");
                userEl.SetAttribute("name", user);
                root.AppendChild(userEl);
            }
            SerialiseTo(root);
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
            RoleName = Util.GetElementOrAttributeValue("name", element);

            users.Clear();
            foreach (XmlElement userEl in element.SelectNodes("userName"))
            {
                users.Add(Util.GetElementOrAttributeValue("name", userEl));
            }

            DeserialiseFrom(element);
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this security setting configuration.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            RolePermission newValue = new RolePermission();
            CopyTo(newValue);
            newValue.RoleName = RoleName;
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
            return string.Format("{0} (user permission)",
                string.IsNullOrEmpty(RoleName) ? "<new>" : RoleName);
        }
        #endregion
        #endregion

        #region Protected methods
        #region CopyTo()
        /// <summary>
        /// Copies the settings to another instance of this class.
        /// </summary>
        /// <param name="value">The target instance.</param>
        protected virtual void CopyTo(RolePermission value)
        {
            base.CopyTo(value);
            value.RoleName = RoleName;
        }
        #endregion
        #endregion
    }
}
