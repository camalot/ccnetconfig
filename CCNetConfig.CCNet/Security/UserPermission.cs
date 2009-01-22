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
    /// A permission for a single user.
    /// </summary>
    [MinimumVersion("1.5")]
    [ReflectorName("userPermission")]
    [DisplayName("User Permission")]
    public class UserPermission
        : SecurityPermission
    {
        #region Private fields
        private string userName;
        #endregion

        #region Public properties
        #region UserName
        /// <summary>
        /// The login name of the user.
        /// </summary>
        [Description("The identifier of the user this assertion is for.")]
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
            XmlElement root = doc.CreateElement("userPermission");
            root.SetAttribute("name", UserName);
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
            UserName = Util.GetElementOrAttributeValue("name", element);
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
            UserPermission newValue = new UserPermission();
            CopyTo(newValue);
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
            return string.Format("{0} (user permission)", 
                string.IsNullOrEmpty(UserName) ? "<new>" : UserName);
        }
        #endregion
        #endregion

        #region Protected methods
        #region CopyTo()
        /// <summary>
        /// Copies the settings to another instance of this class.
        /// </summary>
        /// <param name="value">The target instance.</param>
        protected virtual void CopyTo(UserPermission value)
        {
            base.CopyTo(value);
            value.UserName = UserName;
        }
        #endregion
        #endregion
    }
}
