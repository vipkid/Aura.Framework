using System;

namespace Aura.Framework.Configurations.Converters.TypeConverters
{
    /// <summary>
    /// A class that converts <see cref="string"/> values.
    /// </summary>
    internal sealed class BoolStringConverter : TypeConverter<bool>
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
            switch (value.ToLowerInvariant())
            {
                case "false":
                case "off":
                case "no":
                case "0":
                    return false;

                case "true":
                case "on":
                case "yes":
                case "1":
                    return true;
            }

            throw new ArgumentException(string.Format("The value cannot be converted to type '{0}'.", hint.FullName), "value");
        }

        #endregion Methods
    }
}