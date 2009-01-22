using System;
using System.Xml;
using System.Collections.Generic;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core.Serialization
{
    /// <summary>
    /// Serialises/deserialises security settings.
    /// </summary>
    /// <remarks>
    /// This is mainly a pass-through class to ensure that security settings fit in with the rest of the
    /// item serialisers. All the actual work is done in a <see cref="ServerSecurity"/> instance or one of
    /// it's sub-classes.
    /// </remarks>
    [SerialiserType("nullSecurity")]
    [SerialiserType("internalSecurity")]
    public class SecuritySerialiser
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
            if (!(source is ServerSecurity)) throw new ArgumentException("source must be of type ServerSecurity", "source");
            XmlElement output = (source as ServerSecurity).Serialize();
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
            ServerSecurity output = InitialiseActualType(source.Name);
            output.Deserialize(source);
            return output;
        }
        #endregion
        #endregion

        #region Private methods
        #region InitialiseActualType()
        /// <summary>
        /// Initialises an instance of the actual security type.
        /// </summary>
        /// <param name="name">The name of the type to find.</param>
        /// <returns>The security type, or an instance of <see cref="ServerSecurity"/> if the security type
        /// is unknown.</returns>
        private ServerSecurity InitialiseActualType(string name)
        {
            ServerSecurity value = new ServerSecurity();

            // Retrieve all the types that inherit from ServerSecurity and attempt to find one that has
            // a matching reflector name.
            List<Type> serverSecurityModes = Util.GetAllServerSecurityModes();
            foreach (Type securityMode in serverSecurityModes)
            {
                ReflectorNameAttribute attribute = Util.GetCustomAttribute<ReflectorNameAttribute>(securityMode);
                if ((attribute != null) && (attribute.Name == name))
                {
                    // Once the matching name has been found, instantiate an instance and exit
                    value = Activator.CreateInstance(securityMode) as ServerSecurity;
                    break;
                }
            }

            return value;
        }
        #endregion
        #endregion
    }
}
