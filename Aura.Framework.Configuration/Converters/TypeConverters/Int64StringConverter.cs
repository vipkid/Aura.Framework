﻿using System;

namespace Aura.Framework.Configurations.Converters.TypeConverters
{
    /// <summary>
    /// A class that converts <see cref="string"/> values.
    /// </summary>
    internal sealed class Int64StringConverter : TypeConverter<long>
    {
        #region Methods

        /// <summary>
        /// Converts an object into a <see cref="string"/> value.
        /// </summary>
        public override string ConvertToString(object value)
        {
            return ((long)value).ToString(Configuration.NumberFormat);
        }

        /// <summary>
        /// Converts a <see cref="string"/> into a type.
        /// </summary>
        public override object ConvertFromString(string value, Type hint)
        {
            return long.Parse(value, Configuration.NumberFormat);
        }

        #endregion Methods
    }
}