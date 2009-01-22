using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CCNetConfig.Core.Components;
using System.Drawing.Design;
using System.Xml;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Base class for security loggers to inherit from.
    /// </summary>
    public abstract class SecurityLogger
        : ICCNetObject, ICloneable, INotifyPropertyChanged
    {
        #region Public properties
        #region LogSuccessfulEvents
        /// <summary>
        /// Whether to log successful events or not.
        /// </summary>
        [Description("Whether to log successful events or not.")]
        [DisplayName("Log Successful Events")]
        [Category("Optional")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableBooleanTypeConverter))]
        [Editor(typeof(DefaultableBooleanUIEditor), typeof(UITypeEditor))]
        public virtual bool? LogSuccessfulEvents { get; set; }
        #endregion

        #region LogFailureEvents
        /// <summary>
        /// Whether to log failed events or not.
        /// </summary>
        [Description("Whether to log failed events or not.")]
        [DisplayName("Log Failed Events")]
        [Category("Optional")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableBooleanTypeConverter))]
        [Editor(typeof(DefaultableBooleanUIEditor), typeof(UITypeEditor))]
        public virtual bool? LogFailureEvents { get; set; }
        #endregion
        #endregion

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

        #region CopyTo()
        /// <summary>
        /// Copies the settings to another instance of this class.
        /// </summary>
        /// <param name="value">The target instance.</param>
        protected virtual void CopyTo(SecurityLogger value)
        {
            value.LogSuccessfulEvents = LogSuccessfulEvents;
            value.LogFailureEvents = LogFailureEvents;
        }
        #endregion

        #region SerialiseTo()
        /// <summary>
        /// Serialises the properties to an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="target">The target element.</param>
        protected virtual void SerialiseTo(XmlElement target)
        {
            if (LogSuccessfulEvents.HasValue) target.SetAttribute("success", LogSuccessfulEvents.Value.ToString());
            if (LogFailureEvents.HasValue) target.SetAttribute("failure", LogFailureEvents.Value.ToString());
        }
        #endregion

        #region DeserialiseFrom()
        /// <summary>
        /// Deserialises the properties from an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="source">The source element.</param>
        protected virtual void DeserialiseFrom(XmlElement source)
        {
            LogSuccessfulEvents = Util.GetBoolFromElementOrAttribute("success", source);
            LogFailureEvents = Util.GetBoolFromElementOrAttribute("failure", source);
        }
        #endregion
        #endregion
    }
}
