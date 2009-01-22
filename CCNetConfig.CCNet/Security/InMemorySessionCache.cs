using System;
using System.Collections.Generic;
using System.Text;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.ComponentModel;
using System.Xml;
using System.Drawing.Design;

namespace CCNetConfig.CCNet.Security
{
    /// <summary>
    /// An in-memory session cache.
    /// </summary>
    [MinimumVersion("1.5")]
    [ReflectorName("inMemoryCache")]
    [DisplayName("In Memory")]
    public class InMemorySessionCache
        : SecurityCache
    {
        #region Constructors
        /// <summary>
        /// Start a new instance of a <see cref="InMemorySessionCache"/>.
        /// </summary>
        public InMemorySessionCache()
        {
            Type = "In-Memory";
        }
        #endregion

        #region Public properties
        #region Duration
        /// <summary>
        /// The duration to cache sessions for.
        /// </summary>
        [Description("The duration to cache sessions for (in minutes).")]
        [DisplayName("Cache Duration")]
        [Category("Optional")]
        [DefaultValue(null)]
        public int? Duration { get; set; }
        #endregion

        #region CachingMode
        /// <summary>
        /// The type of caching to use.
        /// </summary>
        [Description("The type of caching to use.")]
        [DisplayName("Cache Mode")]
        [Category("Optional")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableEnumTypeConverter))]
        [Editor(typeof(DefaultableEnumUIEditor), typeof(UITypeEditor))]
        public SessionExpiryMode? CachingMode { get; set; }
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
            XmlElement root = doc.CreateElement("cache");
            root.SetAttribute("type", "inMemoryCache");
            if (Duration.HasValue) root.SetAttribute("duration", Duration.Value.ToString());
            if (CachingMode.HasValue) root.SetAttribute("mode", CachingMode.Value.ToString());
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
            Type = "In-Memory";
            Duration = Util.GetIntFromElementOrAttribute("duration", element);
            CachingMode = Util.GetEnumFromElementOrAttribute<SessionExpiryMode>("mode", element);
        }
        #endregion

        #region Clone()
        /// <summary>
        /// Generates a clone of this security setting configuration.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            InMemorySessionCache newValue = new InMemorySessionCache();
            CopyTo(newValue);
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
            return string.Format("{0} in-memory cache",
                CachingMode.HasValue ? CachingMode.Value.ToString() : "Default").Trim();
        }
        #endregion
        #endregion

        #region Protected methods
        #region CopyTo()
        /// <summary>
        /// Copies the settings to another instance of this class.
        /// </summary>
        /// <param name="value">The target instance.</param>
        protected virtual void CopyTo(InMemorySessionCache value)
        {
            base.CopyTo(value);
            value.CachingMode = CachingMode;
            value.Duration = Duration;
        }
        #endregion
        #endregion
    }
}
