using System;
using System.Xml;

namespace CCNetConfig.Core.Serialization
{
    /// <summary>
    /// Serialises/deserialises projects.
    /// </summary>
    /// <remarks>
    /// This is mainly a pass-through class to ensure that projects fit in with the rest of the item 
    /// serialisers. All the actual work is done in the <see cref="Project"/> instance.
    /// </remarks>
    [SerialiserType("project")]
    public class ProjectSerialiser
        : IItemSerialiser
    {
        #region Public methods
        #region Serialize()
        /// <summary>
        /// Converts the source object into XML.
        /// </summary>
        /// <param name="source">The object to be converted.</param>
        /// <returns>The root <see cref="XmlElement"/> containing the serialised object.</returns>
        public virtual XmlElement Serialize(object source)
        {
            if (!(source is Project)) throw new ArgumentException("source must be of type Project", "source");
            XmlElement output = (source as Project).Serialize();
            return output;
        }
        #endregion

        #region Deserialize()
        /// <summary>
        /// Deserialises XML into a valid object.
        /// </summary>
        /// <param name="source">An <see cref="XmlElement"/> containing the definition of the object.</param>
        /// <returns>The deserialised object.</returns>
        public virtual object Deserialize(XmlElement source)
        {
            Project output = new Project();
            output.Deserialize(source);
            return output;
        }
        #endregion
        #endregion
    }
}
