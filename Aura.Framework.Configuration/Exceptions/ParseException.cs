using System;

namespace Aura.Framework.Configurations.Exceptions
{
    /// <summary>
    /// Represents an error that occurred during the configuration parsing stage.
    /// </summary>
    public sealed class ParseException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        internal ParseException(string message, int line) : base(string.Format("Line {0}: {1}", line, message))
        { }

        #endregion Constructors
    }
}