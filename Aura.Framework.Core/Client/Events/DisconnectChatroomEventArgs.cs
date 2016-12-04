using Aura.Framework.Core.Client.Chatrooms;
using System;

namespace Aura.Framework.Core.Client.Events
{
    /// <summary>
    /// Provides data for the <see cref="DisconnectChatroomEventArgs"/> event arguments.
    /// </summary>
    public class DisconnectChatroomEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Represents the chatroom the user is disconnecting from.
        /// </summary>
        public Chatroom Chatroom { get; private set; }

        #endregion Properties

        #region Constructors & Decontructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectChatroomEventArgs"/> class.
        /// </summary>
        public DisconnectChatroomEventArgs(Chatroom room)
        {
            Chatroom = room;
        }

        #endregion Constructors & Decontructors
    }
}