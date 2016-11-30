using System;

namespace Aura.Framework.Settings.Exceptions
{
    /// <summary>
    /// Represents an error that occurs while parsing invalid data.
    /// </summary>
    public class ParsingException : Exception
    {
        #region Fields

        /// <summary>
        /// Gets or sets the library version.
        /// </summary>
        public Version LibraryVersion { get; private set; }

        /// <summary>
        /// Represents the line number where the exception has occured.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Represents the line value where the exception has occured.
        /// </summary>
        public string LineValue { get; private set; }

        #endregion Fields

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        public ParsingException(string msg) : this(msg, 0, string.Empty, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        public ParsingException(string msg, Exception innerException) : this(msg, 0, string.Empty, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        public ParsingException(string msg, int lineNumber, string lineValue) : this(msg, lineNumber, lineValue, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        public ParsingException(string msg, int lineNumber, string lineValue, Exception innerException) : base(string.Format("{0} while parsing line number {1} with value \'{2}\' - ConfigParser version: {3}", msg, lineNumber, lineValue, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version), innerException)
        {
            LibraryVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            LineNumber = lineNumber;
            LineValue = lineValue;
        }

        #endregion Contructors
    }
}