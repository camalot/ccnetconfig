using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using CCNetConfig.Core;
using CCNetConfig.Core.Components;
using System.Collections.Generic;

namespace CCNetConfig.CCNet.Security
{
    /// <summary>
    /// Exposes the security settings for session-based security.
    /// </summary>
    [MinimumVersion("1.5")]
    [ReflectorName("externalFileSecurity")]
    [DisplayName("External File-based")]
    [ListTreeNode(typeof(SecurityLogger), "AuditLoggers", "Audit Loggers", AddMenuText = "Add a new logger", ImageKey = "auditlogger_16x16")]
    [ListTreeNode(typeof(ExternalFileBase), "Files", "External Files", AddMenuText = "Add a new external file", ImageKey = "auditlogger_16x16")]
    [InstanceTreeNode(typeof(SecurityAuditReader), "AuditReader", "Audit Reader", ImageKey = "auditreader_16x16")]
    [InstanceTreeNode(typeof(SecurityCache), "Cache", ImageKey = "securitycache_16x16")]
    public class ExternalFileServerSecurity
        : ServerSecurity
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of <see cref="ExternalFileServerSecurity"/>.
        /// </summary>
        public ExternalFileServerSecurity()
        {
            Type = "External File-based";
            Files = new CloneableList<ExternalFileBase>();
            AuditLoggers = new List<SecurityLogger>();
        }
        #endregion

        #region Public properties
        #region Cache
        /// <summary>
        /// The associated cache.
        /// </summary>
        [Browsable(false)]
        public SecurityCache Cache { get; set; }
        #endregion

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

        #region Files
        /// <summary>
        /// The files.
        /// </summary>
        [Browsable(false)]
        public CloneableList<ExternalFileBase> Files { get; private set; }
        #endregion

        #region AuditLoggers
        /// <summary>
        /// The loggers for audit events.
        /// </summary>
        [Browsable(false)]
        public List<SecurityLogger> AuditLoggers { get; private set; }
        #endregion

        #region AuditReader
        /// <summary>
        /// The associated audit reader.
        /// </summary>
        [Browsable(false)]
        public SecurityAuditReader AuditReader { get; set; }
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
            XmlElement root = doc.CreateElement("externalFileSecurity");

            if (DefaultRight.HasValue) root.SetAttribute("defaultRight", DefaultRight.Value.ToString());

            XmlElement usersNode = Util.CreateElement(root, "files");
            foreach (ExternalFile file in Files)
            {
                root.AppendChild(doc.ImportNode(file.Serialize(), true));
            }
            if (AuditLoggers.Count > 0)
            {
                XmlElement auditNode = Util.CreateElement(root, "audit");
                foreach (SecurityLogger logger in AuditLoggers)
                {
                    auditNode.AppendChild(doc.ImportNode(logger.Serialize(), true));
                }
            }

            if (AuditReader != null)
            {
                XmlElement readerEl = AuditReader.Serialize();
                if (readerEl != null) root.AppendChild(doc.ImportNode(readerEl, true));
            }

            if (Cache != null)
            {
                XmlElement readerEl = Cache.Serialize();
                if (readerEl != null) root.AppendChild(doc.ImportNode(readerEl, true));
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
            Type = "External File-based";
            DefaultRight = Util.GetEnumFromElementOrAttribute<SecurityRight>("defaultRight", element);

            foreach (XmlElement ele in element.SelectNodes("files/*"))
            {
                var externalFile = new ExternalFile();
                externalFile.Deserialize(ele);
                Files.Add(externalFile);
            }

            foreach (XmlElement ele in element.SelectNodes("audit/*"))
            {
                SecurityLogger logger = Util.CreateInstanceFromXmlName<SecurityLogger>(ele.Name);
                if (logger != null)
                {
                    logger.Deserialize(ele);
                    AuditLoggers.Add(logger);
                }
            }

            XmlElement readerEl = element.SelectSingleNode("auditReader") as XmlElement;
            if (readerEl != null)
            {
                string xmlName = readerEl.GetAttribute("type");
                SecurityAuditReader reader = Util.CreateInstanceFromXmlName<SecurityAuditReader>(xmlName);
                if (reader != null)
                {
                    reader.Deserialize(readerEl);
                    AuditReader = reader;
                }
            }

            XmlElement cacheEl = element.SelectSingleNode("cache") as XmlElement;
            if (cacheEl != null)
            {
                string xmlName = cacheEl.GetAttribute("type");
                SecurityCache cache = Util.CreateInstanceFromXmlName<SecurityCache>(xmlName);
                if (cache != null)
                {
                    cache.Deserialize(cacheEl);
                    Cache = cache;
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
            ExternalFileServerSecurity newValue = new ExternalFileServerSecurity();
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
        protected void CopyTo(ExternalFileServerSecurity value)
        {
            base.CopyTo(value);
        }
        #endregion
        #endregion
    }
}
