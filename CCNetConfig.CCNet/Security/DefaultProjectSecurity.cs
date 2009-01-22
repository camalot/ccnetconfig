using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.Collections.Generic;

namespace CCNetConfig.CCNet.Security
{
    /// <summary>
    /// Exposes the security settings for the default project security.
    /// </summary>
    [MinimumVersion("1.5")]
    [ReflectorName("defaultProjectSecurity")]
    [DisplayName("Default")]
    [ListTreeNode(typeof(SecurityPermission), "Permissions", AddMenuText = "Add a new permission", ImageKey = "assertion_16x16")]
    public class DefaultProjectSecurity
        : ProjectSecurity
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of <see cref="DefaultProjectSecurity"/>.
        /// </summary>
        public DefaultProjectSecurity()
        {
            Type = "Default";
            Permissions = new List<SecurityPermission>();
        }
        #endregion

        #region Public properties
        #region DefaultRight
        /// <summary>
        /// The default right to apply if no other permission has been set.
        /// </summary>
        [Description("The default right to apply if no other permissions have been set.")]
        [DisplayName("Default Right")]
        [Category("Optional")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableEnumTypeConverter))]
        [Editor(typeof(DefaultableEnumUIEditor), typeof(UITypeEditor))]
        public SecurityRight? DefaultRight { get; set; }
        #endregion

        #region Permissions
        /// <summary>
        /// The server-level permissions.
        /// </summary>
        [Browsable(false)]
        public List<SecurityPermission> Permissions { get; private set; }
        #endregion
        #endregion

        #region Public methods
        #region Serialize()
        /// <summary>
        /// Serialises the security settings to an <see cref="XmlElement"/>.
        /// </summary>
        /// <returns>The <see cref="XmlElement"/> containing the security settings configuration.</returns>
        public override XmlElement Serialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("security");
            root.SetAttribute("type", "defaultProjectSecurity");

            if (DefaultRight.HasValue) root.SetAttribute("defaultRight", DefaultRight.Value.ToString());

            if (Permissions.Count > 0)
            {
                XmlElement permissionsNode = Util.CreateElement(root, "permissions");
                foreach (SecurityPermission permission in Permissions)
                {
                    permissionsNode.AppendChild(doc.ImportNode(permission.Serialize(), true));
                }
            }

            return root;
        }
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises an <see cref="XmlElement"/> containing security settings configuration.
        /// </summary>
        /// <param name="element">The <see cref="XmlElement"/> to deserialise.</param>
        public override void Deserialize(XmlElement element)
        {
            Type = "Default";
            DefaultRight = Util.GetEnumFromElementOrAttribute<SecurityRight>("defaultRight", element);

            foreach (XmlElement ele in element.SelectNodes("permissions/*"))
            {
                SecurityPermission permission = Util.CreateInstanceFromXmlName<SecurityPermission>(ele.Name);
                if (permission != null)
                {
                    permission.Deserialize(ele);
                    Permissions.Add(permission);
                }
            }
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this security settings configuration.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DefaultProjectSecurity newValue = new DefaultProjectSecurity();
            CopyTo(newValue);
            return newValue;
        }
        #endregion
        #endregion

        #region Protected methods
        #region CopyTo()
        /// <summary>
        /// Copies the values of this instance to another.
        /// </summary>
        /// <param name="value">The target instance.</param>
        protected void CopyTo(DefaultProjectSecurity value)
        {
            base.CopyTo(value);
        }
        #endregion
        #endregion
    }
}
