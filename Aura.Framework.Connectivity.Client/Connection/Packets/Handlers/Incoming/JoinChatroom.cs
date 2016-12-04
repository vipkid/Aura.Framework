using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the join state.
    /// </summary>
    public enum JoinState
    {
        /// <summary>
        /// Returns if something happened at our end.
        /// </summary>
        JoinChatRoomError = 0,

        /// <summary>
        /// Returns if the join succeeded.
        /// </summary>
        JoinChatRoomOk = 1,

        /// <summary>
        /// Returns if the given password is incorrect.
        /// </summary>
        JoinChatRoomWrongPassword = 2,

        /// <summary>
        /// Returns if the user is already in the chatroom.
        /// </summary>
        JoinstateAlreadyInChatRoom = 3
    }

    /// <summary>
    /// Represents the join chatroom packet.
    /// </summary>
    public class JoinChatroom : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int chatRoomId = message.ReadInt32();
            JoinState state = (JoinState)message.ReadInt32();

            connection.ClientManager.ChatroomManager.JoinChatroom(chatRoomId, state);
        }
    }
}