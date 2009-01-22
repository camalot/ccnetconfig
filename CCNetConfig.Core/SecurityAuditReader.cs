using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Xml;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Exposes the audit reader settings.
    /// </summary>
    [MinimumVersion("1.5")]
    public class SecurityAuditReader
        : ICloneable, INotifyPropertyChanged, ICCNetObject
    {
        #region Private fields
        private string type = "(None)";
        #endregion

        #region Public properties
        #region Type
        /// <summary>
        /// The type of audit reader.
        /// </summary>
        [Description("The type of audit reader.")]
        [DisplayName("(Type)")]
        [Category("Information")]
        public string Type
        {
            get { return type; }
            protected set { type = value; }
        }
        #endregion
        #endregion

        #region Public methods
        #region Serialize()
        /// <summary>
        /// Serialises the audit reader settings to an <see cref="XmlElement"/>.
        /// </summary>
        /// <returns>The <see cref="XmlElement"/> containing the audit reader settings configuration.</returns>
        public virtual XmlElement Serialize()
        {
            return null;
        }
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises an <see cref="XmlElement"/> containing audit reader settings configuration.
        /// </summary>
        /// <param name="element">The <see cref="XmlElement"/> to deserialise.</param>
        public virtual void Deserialize(XmlElement element)
        {
            type = "(None)";
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this audit reader settings configuration.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            SecurityAuditReader newValue = new SecurityAuditReader();
            CopyTo(newValue);
            return newValue;
        }
        #endregion
        #endregion

        #region Public events
        #region PropertyChanged
        /// <summary>
        /// Notifies any listeners that a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #endregion

        #region Protected methods
        #region CopyTo()
        /// <summary>
        /// Copies the values of this instance to another.
        /// </summary>
        /// <param name="value">The target instance.</param>
        protected void CopyTo(SecurityAuditReader value)
        {
            value.type = Type;
        }
        #endregion

        #region FirePropertyChanged()
        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="property">The name of the property that was changed.</param>
        protected virtual void FirePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
        #endregion
    }
}
