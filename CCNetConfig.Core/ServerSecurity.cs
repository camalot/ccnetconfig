using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Exposes the security settings.
    /// </summary>
    [MinimumVersion("1.5")]
    [ReflectorName("nullSecurity")]
    public class ServerSecurity
        : ICloneable, INotifyPropertyChanged, ICCNetObject
    {
        #region Private fields
        private string type = "(None)";
        #endregion

        #region Public properties
        #region Type
        /// <summary>
        /// The type of security.
        /// </summary>
        [Description("The type of security manager.")]
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
        /// Serialises the security settings to an <see cref="XmlElement"/>.
        /// </summary>
        /// <returns>The <see cref="XmlElement"/> containing the security settings configuration.</returns>
        public virtual XmlElement Serialize()
        {
            return null;
        }
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises an <see cref="XmlElement"/> containing security settings configuration.
        /// </summary>
        /// <param name="element">The <see cref="XmlElement"/> to deserialise.</param>
        public virtual void Deserialize(XmlElement element)
        {
            type = "(None)";
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this security settings configuration.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            ServerSecurity newValue = new ServerSecurity();
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
        protected void CopyTo(ServerSecurity value)
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
