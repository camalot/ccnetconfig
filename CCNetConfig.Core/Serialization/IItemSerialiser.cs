using System.Xml;

namespace CCNetConfig.Core.Serialization
{
    /// <summary>
    /// Provides serialisation/deserialisation for the various top-level item types within the
    /// configuration.
    /// </summary>
    public interface IItemSerialiser
    {
        #region Methods
        #region Serialize()
        /// <summary>
        /// Converts the source object into XML.
        /// </summary>
        /// <param name="source">The object to be converted.</param>
        /// <returns>The root <see cref="XmlElement"/> containing the serialised object.</returns>
        XmlElement Serialize(object source);
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises XML into a valid object.
        /// </summary>
        /// <param name="source">An <see cref="XmlElement"/> containing the definition of the object.</param>
        /// <returns>The deserialised object.</returns>
        object Deserialize(XmlElement source);
        #endregion
        #endregion
    }
}
