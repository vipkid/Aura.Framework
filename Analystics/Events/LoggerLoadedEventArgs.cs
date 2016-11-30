using System;

namespace Aura.Framework.Analystics.Events
{
    /// <summary>
    /// Provides data when the <see cref="Logger"/> class loads a block of logged lines.
    /// </summary>
    public class LoggerLoadedEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// Represents the lines in a <see cref="string"/> array value that has been loaded.
        /// </summary>
        public string[] Block { get; private set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerWritenEventArgs"/> class.
        /// </summary>
        public LoggerLoadedEventArgs(string[] block)
        {
            Block = block;
        }

        #endregion Constructors
    }
}