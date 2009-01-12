using System;
using System.Xml;

namespace CCNetConfig.Core.Serialization
{
    /// <summary>
    /// Serialises/deserialises queues.
    /// </summary>
    /// <remarks>
    /// This is mainly a pass-through class to ensure that queues fit in with the rest of the item 
    /// serialisers. All the actual work is done in the <see cref="Queue"/> instance.
    /// </remarks>
    [SerialiserType("queue")]
    public class QueueSerialiser
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
            if (!(source is Queue)) throw new ArgumentException("source must be of type Queue", "source");
            XmlElement output = (source as Queue).Serialize();
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
            Queue output = new Queue();
            output.Deserialize(source);
            return output;
        }
        #endregion
        #endregion
    }
}
