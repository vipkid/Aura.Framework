﻿using System;

namespace Aura.Framework.Configurations.Converters.TypeConverters
{
    /// <summary>
    /// A class that converts <see cref="string"/> values.
    /// </summary>
    internal sealed class SingleStringConverter : TypeConverter<float>
    {
        #region Methods

        /// <summary>
        /// Converts an object into a <see cref="string"/> value.
        /// </summary>
        public override string ConvertToString(object value)
        {
            return ((float)value).ToString(Configuration.NumberFormat);
        }

        /// <summary>
        /// Converts a <see cref="string"/> into a type.
        /// </summary>
        public override object ConvertFromString(string value, Type hint)
        {
            return float.Parse(value, Configuration.NumberFormat);
        }

        #endregion Methods
    }
}