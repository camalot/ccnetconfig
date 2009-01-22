using System;

namespace CCNetConfig.Core.Serialization
{
    /// <summary>
    /// Defines the tag name that a serialiser will handle.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SerialiserTypeAttribute
        : Attribute
    {
        #region Private fields
        private readonly string tagName;
        #endregion

        #region Constructors
        /// <summary>
        /// Starts a new serialiser type attribute.
        /// </summary>
        /// <param name="tagName">The type that the serialiser handles.</param>
        public SerialiserTypeAttribute(string tagName)
        {
            if (string.IsNullOrEmpty(tagName)) throw new ArgumentException("The tagName cannot be empty or null", "tagName");
            this.tagName = tagName;
        }
        #endregion

        #region Public properties
        #region TagName
        /// <summary>
        /// The name of the tag that the serialiser will handle.
        /// </summary>
        public string TagName
        {
            get { return tagName; }
        }
        #endregion
        #endregion
    }
}