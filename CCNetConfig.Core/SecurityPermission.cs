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
    /// Defines a security permission.
    /// </summary>
    public abstract class SecurityPermission
        : SecuritySetting
    {
        #region Public properties
        #region RefId
        /// <summary>
        /// A reference to another assertion.
        /// </summary>
        [Description("A reference to another assertion. This is only valid for non-server-level assertions.")]
        [DisplayName("Reference")]
        [Category("Optional")]
        [DefaultValue(null)]
        public virtual string RefId { get; set; }
        #endregion

        #region DefaultRight
        /// <summary>
        /// The default permission.
        /// </summary>
        [Description("The default permission to use if no other permissions are applicable.")]
        [DisplayName("Default")]
        [Category("Permissions")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableEnumTypeConverter))]
        [Editor(typeof(DefaultableEnumUIEditor), typeof(UITypeEditor))]
        public virtual SecurityRight? DefaultRight { get; set; }
        #endregion

        #region SendMessageRight
        /// <summary>
        /// The permission to send messages.
        /// </summary>
        [Description("The permission to send messages.")]
        [DisplayName("Can send messages")]
        [Category("Permissions")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableEnumTypeConverter))]
        [Editor(typeof(DefaultableEnumUIEditor), typeof(UITypeEditor))]
        public virtual SecurityRight? SendMessageRight { get; set; }
        #endregion

        #region ForceBuildRight
        /// <summary>
        /// The permission to force or abort builds.
        /// </summary>
        [Description("The permission to force or abort builds.")]
        [DisplayName("Can force or abort builds")]
        [Category("Permissions")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableEnumTypeConverter))]
        [Editor(typeof(DefaultableEnumUIEditor), typeof(UITypeEditor))]
        public virtual SecurityRight? ForceBuildRight { get; set; }
        #endregion

        #region StartProjectRight
        /// <summary>
        /// The permission to start projects.
        /// </summary>
        [Description("The permission to start projects.")]
        [DisplayName("Can start projects")]
        [Category("Permissions")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableEnumTypeConverter))]
        [Editor(typeof(DefaultableEnumUIEditor), typeof(UITypeEditor))]
        public virtual SecurityRight? StartProjectRight { get; set; }
        #endregion

        #region StopProjectRight
        /// <summary>
        /// The permission to stop projects.
        /// </summary>
        [Description("The permission to stop projects.")]
        [DisplayName("Can stop projects")]
        [Category("Permissions")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableEnumTypeConverter))]
        [Editor(typeof(DefaultableEnumUIEditor), typeof(UITypeEditor))]
        public virtual SecurityRight? StopProjectRight { get; set; }
        #endregion

        #region ViewSecurityRight
        /// <summary>
        /// The permission to view the security settings using the dashboard.
        /// </summary>
        [Description("The permission to view the security settings using the dashboard.")]
        [DisplayName("Can view security")]
        [Category("Permissions")]
        [DefaultValue(null)]
        [TypeConverter(typeof(DefaultableEnumTypeConverter))]
        [Editor(typeof(DefaultableEnumUIEditor), typeof(UITypeEditor))]
        public virtual SecurityRight? ViewSecurityRight { get; set; }
        #endregion
        #endregion

        #region Protected methods
        #region CopyTo()
        /// <summary>
        /// Copies the settings to another instance of this class.
        /// </summary>
        /// <param name="value">The target instance.</param>
        protected virtual void CopyTo(SecurityPermission value)
        {
            value.ViewSecurityRight = ViewSecurityRight;
            value.StopProjectRight = StopProjectRight;
            value.StartProjectRight = StartProjectRight;
            value.SendMessageRight = SendMessageRight;
            value.RefId = RefId;
            value.ForceBuildRight = ForceBuildRight;
            value.DefaultRight = DefaultRight;
        }
        #endregion

        #region SerialiseTo()
        /// <summary>
        /// Serialises the properties to an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="target">The target element.</param>
        protected virtual void SerialiseTo(XmlElement target)
        {
            if (!string.IsNullOrEmpty(RefId)) target.SetAttribute("ref", RefId);
            if (DefaultRight.HasValue) target.SetAttribute("defaultRight", DefaultRight.Value.ToString());
            if (SendMessageRight.HasValue) target.SetAttribute("sendMessage", SendMessageRight.Value.ToString());
            if (ForceBuildRight.HasValue) target.SetAttribute("forceBuild", ForceBuildRight.Value.ToString());
            if (StartProjectRight.HasValue) target.SetAttribute("startProject", StartProjectRight.Value.ToString());
            if (StopProjectRight.HasValue) target.SetAttribute("stopProject", StopProjectRight.Value.ToString());
            if (ViewSecurityRight.HasValue) target.SetAttribute("viewSecurity", ViewSecurityRight.Value.ToString());
        }
        #endregion

        #region DeserialiseFrom()
        /// <summary>
        /// Deserialises the properties from an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="source">The source element.</param>
        protected virtual void DeserialiseFrom(XmlElement source)
        {
            RefId = Util.GetElementOrAttributeValue("ref", source);
            DefaultRight = Util.GetEnumFromElementOrAttribute<SecurityRight>("defaultRight", source);
            SendMessageRight = Util.GetEnumFromElementOrAttribute<SecurityRight>("sendMessage", source);
            ForceBuildRight = Util.GetEnumFromElementOrAttribute<SecurityRight>("forceBuild", source);
            StartProjectRight = Util.GetEnumFromElementOrAttribute<SecurityRight>("startProject", source);
            StopProjectRight = Util.GetEnumFromElementOrAttribute<SecurityRight>("stopProject", source);
            ViewSecurityRight = Util.GetEnumFromElementOrAttribute<SecurityRight>("viewSecurity", source);
        }
        #endregion
        #endregion
    }
}
