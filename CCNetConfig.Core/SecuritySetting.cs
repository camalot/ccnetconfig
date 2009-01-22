using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Xml;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Defines a setting for security.
    /// </summary>
    public abstract class SecuritySetting
        : ICCNetObject, ICloneable, INotifyPropertyChanged
    {
        #region Public methods
        #region Serialize()
        /// <summary>
        /// Serialises the security setting to an <see cref="XmlElement"/>.
        /// </summary>
        /// <returns>The <see cref="XmlElement"/> containing the security setting configuration.</returns>
        public abstract XmlElement Serialize();
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises an <see cref="XmlElement"/> containing security setting configuration.
        /// </summary>
        /// <param name="element">The <see cref="XmlElement"/> to deserialise.</param>
        public abstract void Deserialize(XmlElement element);
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this security setting configuration.
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
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
