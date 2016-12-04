using Aura.Framework.Core.Client.Chatrooms;
using Aura.Framework.Core.Client.Connection.Packets.Handlers.Incoming;

namespace Aura.Framework.Core.Client.Events
{
    /// <summary>
    /// Provides data for the <see cref="JoinedChatroomEventArgs"/> event arguments.
    /// </summary>
    public class JoinedChatroomEventArgs
    {
        #region Properties

        /// <summary>
        /// Represents the chatroom the user is connecting to.
        /// </summary>
        public Chatroom Chatroom { get; private set; }

        /// <summary>
        /// Represents the joinstate of the user that is connecting to a chatroom.
        /// </summary>
        public JoinState JoinState { get; private set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initialzes a new instance of the <see cref="JoinedChatroomEventArgs"/> class.
        /// </summary>
        public JoinedChatroomEventArgs(Chatroom chatroom, JoinState joinState)
        {
            Chatroom = chatroom;
            JoinState = joinState;
        }

        #endregion Constructors & Deconstructors
    }
}