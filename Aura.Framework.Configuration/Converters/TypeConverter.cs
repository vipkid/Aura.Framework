using Aura.Framework.Configurations.Interfaces;
using System;

namespace Aura.Framework.Configurations.Converters
{
    /// <summary>
    /// Represents a type-to-string and string-to-type converter that is used for the conversion of setting values.
    /// </summary>
    public abstract class TypeConverter<T> : IConverter
    {
        #region Fields

        /// <summary>
        /// Converts an object to its string representation.
        /// </summary>
        public abstract string ConvertToString(object value);

        /// <summary>
        /// Converts a string value to an object of this converter's type.
        /// </summary>
        public abstract object ConvertFromString(string value, Type hint);

        /// <summary>
        /// The type that this converter is able to convert to and from a string.
        /// </summary>
        public Type ConvertibleType
        {
            get { return typeof(T); }
        }

        #endregion Fields
    }
}