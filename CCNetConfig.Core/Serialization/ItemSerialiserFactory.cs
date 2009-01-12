using System;
using System.Collections.Generic;
using System.Reflection;

namespace CCNetConfig.Core.Serialization
{
    /// <summary>
    /// Generates <see cref="IItemSerialiser"/> for the correct item.
    /// </summary>
    public class ItemSerialiserFactory
    {
        #region Private fields
        private Dictionary<string, Type> serialisers = new Dictionary<string,Type>();
        #endregion

        #region Constructors
        /// <summary>
        /// Starts a new instance of the factory and loads all the available serialisers.
        /// </summary>
        public ItemSerialiserFactory()
        {
            // Iterate through all the types in this assembly and extract all the types that contain a
            // SerialiserTypeAttribute
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            foreach (Type typeToCheck in currentAssembly.GetExportedTypes())
            {
                object[] attributes = typeToCheck.GetCustomAttributes(typeof(SerialiserTypeAttribute), false);
                foreach (SerialiserTypeAttribute attribute in attributes)
                {
                    serialisers.Add(attribute.TagName, typeToCheck);
                }
            }
        }
        #endregion

        #region Public methods
        #region Retrieve()
        /// <summary>
        /// Retreives the correct <see cref="IItemSerialiser"/> for a root-level item.
        /// </summary>
        /// <param name="name">The name of the item to retrieve the <see cref="IItemSerialiser"/> for.</param>
        /// <returns>The correct <see cref="IItemSerialiser"/> if found, null otherwise.</returns>
        public IItemSerialiser Retrieve(string name)
        {
            IItemSerialiser serialiser = null;

            if (serialisers.ContainsKey(name))
            {
                serialiser = Activator.CreateInstance(serialisers[name]) as IItemSerialiser;
            }

            return serialiser;
        }
        #endregion
        #endregion
    }
}
