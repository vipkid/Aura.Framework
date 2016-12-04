using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the joinstate of the user that connects to a chatroom.
    /// </summary>
    public enum JoinState
    {
        /// <summary>
        /// A joinstate that is used when the user fails to connect to a chatroom.
        /// </summary>
        JoinChatRoomError = 0,

        /// <summary>
        /// A joinstate that is used when the user succeeds to connect to a chatroom.
        /// </summary>
        JoinChatRoomOk = 1,

        /// <summary>
        /// A joinstate that is used when the user uses the wrong password for a protected chatroom.
        /// </summary>
        JoinChatRoomWrongPassword = 2
    }

    /// <summary>
    /// Represents the join chatroom packet.
    /// </summary>
    public class JoinChatroomComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public JoinChatroomComposer(int chatRoomId, JoinState joinState) : base(ServerPackets.JoinChatroom)
        {
            WriteInt(chatRoomId);
            WriteInt((int)joinState);
        }
    }
}