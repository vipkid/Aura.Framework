using Aura.Framework.Audio.Player.Enumerators;
using System;

namespace Aura.Media.EventArguments
{
    /// <summary>
    /// Represents event arguments for media keys that are pressed.
    /// </summary>
    public class MediaKeyEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets the current pressed media key.
        /// </summary>
        public MediaKeys Key { get; private set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialzes a new instance of the <see cref="MediaKeyEventArgs"/> class.
        /// </summary>
        public MediaKeyEventArgs(MediaKeys key)
        {
            Key = key;
        }

        #endregion Constructors
    }
}