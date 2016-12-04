using System;

namespace Aura.Framework.Configurations.Interfaces
{
    /// <summary>
    /// Defines a type-to-string and string-to-type converter that is used for the conversion of setting values.
    /// </summary>
    public interface IConverter
    {
        #region Fields

        /// <summary>
        /// Converts an object to its string representation.
        /// </summary>
        string ConvertToString(object value);

        /// <summary>
        /// Converts a string value to an object of this converter's type.
        /// </summary>
        object ConvertFromString(string value, Type hint);

        #endregion Fields

        #region Methods

        /// <summary>
        /// The type that this converter is able to convert to and from a string.
        /// </summary>
        Type ConvertibleType { get; }

        #endregion Methods
    }
}