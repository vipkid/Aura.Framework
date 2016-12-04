namespace Aura.Framework.Enumerators
{
    /// <summary>
    /// Represents an enumerator to specify a logged line type.
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// Represents an error log-line.
        /// </summary>
        Error,

        /// <summary>
        /// Represents a normal log-line.
        /// </summary>
        Regular,

        /// <summary>
        /// Represents a warning log-line.
        /// </summary>
        Warning,

        /// <summary>
        /// Represents a chat log-line.
        /// </summary>
        Chat,

        /// <summary>
        /// Represents a call log-line.
        /// </summary>
        Call,

        /// <summary>
        /// Represents an account log-line.
        /// </summary>
        Account,

        /// <summary>
        /// Represents an alternavive exception log-line.
        /// </summary>
        Exception,

        /// <summary>
        /// Represents a command log-line.
        /// </summary>
        Command,

        /// <summary>
        /// Represents a compiled log-line.
        /// </summary>
        Compiled,

        /// <summary>
        /// Represents a build log-line.
        /// </summary>
        Build,

        /// <summary>
        /// Represents an internal log-line.
        /// </summary>
        Internal,

        /// <summary>
        /// Represents an analystic-related log-line.
        /// </summary>
        Analystic,

        /// <summary>
        /// Represents an initialized log-line.
        /// </summary>
        Initialized,

        /// <summary>
        /// Represents an unkown log-line.
        /// </summary>
        Unknown,
    }
}