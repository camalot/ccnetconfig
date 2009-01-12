using System;
using System.Collections.Generic;
using System.Reflection;
using CCNetConfig.Core.Components;
using System.ComponentModel;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Performs validation on classes using the reflection attributes.
    /// </summary>
    public static class Validator
    {
        #region Public methods
        #region Validate()
        /// <summary>
        /// Validates an item.
        /// </summary>
        /// <typeparam name="TItem">An <see cref="ICCNetObject"/>.</typeparam>
        /// <param name="value">The item to validate.</param>
        /// <returns>A list of exceptions that will be empty if the item is valid</returns>
        public static List<ValidationException> Validate<TItem>(TItem value)
            where TItem : ICCNetObject
        {
            List<ValidationException> errors = new List<ValidationException>();

            Type actualType = value.GetType();
            PropertyInfo[] props = actualType.GetProperties();
            Version versionInfo = Util.GetTypeDescriptionProviderVersion(actualType);

            foreach (PropertyInfo pi in props)
            {
                string displayName = RetrieveDisplayName(pi);

                RequiredAttribute[] required = Util.GetCustomAttributes<RequiredAttribute>(pi);
                foreach (RequiredAttribute attribute in required)
                {
                    if (CheckVersionRange(attribute.MinimumVersion, attribute.MaximumVersion, versionInfo))
                    {
                        object propertyValue = pi.GetValue(value, new object[0]);
                        if ((propertyValue == null) || (propertyValue.ToString().Length == 0))
                        {
                            errors.Add(
                                new ValidationException(
                                    actualType.Name,
                                    string.Format("{0} is a required property", displayName)));
                        }
                    }
                }
            }

            return errors;
        }
        #endregion
        #endregion

        #region Private methods
        #region RetrieveDisplayName()
        /// <summary>
        /// Retrieves the display name for a property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static string RetrieveDisplayName(PropertyInfo property)
        {
            DisplayNameAttribute dislayName = Util.GetCustomAttribute<DisplayNameAttribute>(property);
            if (dislayName != null) return dislayName.DisplayName;
            return property.Name;
        }
        #endregion

        #region CheckVersionRange()
        /// <summary>
        /// Checks
        /// </summary>
        /// <param name="minVersion"></param>
        /// <param name="maxVersion"></param>
        /// <returns></returns>
        private static bool CheckVersionRange(string minVersion, string maxVersion, Version actual)
        {
            bool isInRange = Util.IsInVersionRange(new Version(minVersion), new Version(maxVersion), actual);
            return isInRange;
        }
        #endregion
        #endregion
    }
}
