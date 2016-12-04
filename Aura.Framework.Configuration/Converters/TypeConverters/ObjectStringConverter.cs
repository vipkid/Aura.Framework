using Aura.Framework.Configurations.Interfaces;
using System;

namespace Aura.Framework.Configurations.Converters.TypeConverters
{
    /// <summary>
    /// Represents a converter.
    /// </summary>
    internal sealed class FallbackStringConverter : IConverter
    {
        #region Methods

        /// <summary>
        /// Converts a value into a <see cref="string"/> value.
        /// </summary>
        public string ConvertToString(object value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Converts a value from a <see cref="string"/> value.
        /// </summary>
        public object ConvertFromString(string value, Type hint)
        {
            throw new NotImplementedException();
        }

        #endregion Methods

        #region Fields

        /// <summary>
        /// Represents whether the type is a convertable type.
        /// </summary>
        public Type ConvertibleType
        {
            get { return null; }
        }

        #endregion Fields
    }
}