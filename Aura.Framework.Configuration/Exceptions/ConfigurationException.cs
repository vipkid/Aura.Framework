using System;

namespace Aura.Framework.Configurations.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when a string value could not be converted to a specific instance.
    /// </summary>
    public sealed class ConfigurationException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        private ConfigurationException(string message, Exception innerException) : base(message, innerException)
        { }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        internal static ConfigurationException Create(string stringValue, Type dstType, Exception innerException)
        {
            string msg = string.Format("Failed to convert value '{0}' to type {1}.", stringValue, dstType.FullName);
            return new ConfigurationException(msg, innerException);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationException"/> class when the converter is invalid/null/missing.
        /// </summary>
        internal static ConfigurationException CreateOnConverterMissing(string stringValue, Type dstType)
        {
            string msg = string.Format(
                "Failed to convert value '{0}' to type {1}; no converter for this type is registered.",
                stringValue, dstType.FullName);

            var innerException = new NotImplementedException("no converter for this type is registered.");

            return new ConfigurationException(msg, innerException);
        }

        #endregion Methods
    }
}