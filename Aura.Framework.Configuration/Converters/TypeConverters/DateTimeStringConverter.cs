using System;

namespace Aura.Framework.Configurations.Converters.TypeConverters
{
    /// <summary>
    /// A class that converts <see cref="string"/> values.
    /// </summary>
    internal sealed class DateTimeStringConverter : TypeConverter<DateTime>
    {
        #region Methods

        /// <summary>
        /// Converts an object into a <see cref="string"/> value.
        /// </summary>
        public override string ConvertToString(object value)
        {
            return ((DateTime)value).ToString(Configuration.DateTimeFormat);
        }

        /// <summary>
        /// Converts a <see cref="string"/> into a type.
        /// </summary>
        public override object ConvertFromString(string value, Type hint)
        {
            return DateTime.Parse(value, Configuration.DateTimeFormat);
        }

        #endregion Methods
    }
}