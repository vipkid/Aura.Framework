using System;

namespace Aura.Framework.Configurations.Converters.TypeConverters
{
    /// <summary>
    /// A class that converts <see cref="string"/> values.
    /// </summary>
    internal sealed class EnumStringConverter : TypeConverter<Enum>
    {
        #region Methods

        /// <summary>
        /// Converts an object into a <see cref="string"/> value.
        /// </summary>
        public override string ConvertToString(object value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Converts a <see cref="string"/> into a type.
        /// </summary>
        public override object ConvertFromString(string value, Type hint)
        {
            int indexOfLastDot = value.LastIndexOf('.');

            if (indexOfLastDot >= 0)
                value = value.Substring(indexOfLastDot + 1, value.Length - indexOfLastDot - 1).Trim();

            return Enum.Parse(hint, value);
        }

        #endregion Methods
    }
}