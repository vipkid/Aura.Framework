using System;

namespace Aura.Framework.Analystics.Events
{
    /// <summary>
    /// Provides data when the <see cref="Logger"/> class writes a line or block.
    /// </summary>
    public class LoggerWritenEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// Represents the line in a <see cref="string"/> value that has been logged.
        /// </summary>
        public string Line { get; private set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerWritenEventArgs"/> class.
        /// </summary>
        public LoggerWritenEventArgs(string line)
        {
            Line = line;
        }

        #endregion Constructors
    }
}