using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the type of broadcast message.
    /// </summary>
    public enum BroadcastType
    {
        /// <summary>
        /// A broadcast message that occurs when a user disconnects from a chatroom.
        /// </summary>
        UserLeftChatRoom,

        /// <summary>
        /// A broadcast message that occurs when a user connects to a chatroom.
        /// </summary>
        UserJoinedChatRoom
    }

    /// <summary>
    /// Represents the broadcast message packet.
    /// </summary>
    public class BroadcastMessageComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public BroadcastMessageComposer(int chatroomId, int messageId, int userId, string username, BroadcastType broadcastType) : base(ServerPackets.BroadcastChatMessage)
        {
            WriteInt(chatroomId);
            WriteInt(messageId);
            WriteInt(userId);

            switch (broadcastType)
            {
                case BroadcastType.UserLeftChatRoom:
                    WriteString($"{username} has left the chatroom.");
                    break;

                case BroadcastType.UserJoinedChatRoom:
                    WriteString($"{username} has joined the chatroom.");
                    break;
            }
        }
    }
}