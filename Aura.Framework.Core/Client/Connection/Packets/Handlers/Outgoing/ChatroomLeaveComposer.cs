using Aura.Framework.Core.Shared.Messages;
using Aura.Framework.Shared.Packets;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the chatroom leave packet.
    /// </summary>
    public class ChatroomLeaveComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server.
        /// </summary>
        public ChatroomLeaveComposer(int userId, int chatroomId) : base(ClientPackets.LeaveChatroom)
        {
            WriteInt(userId);
            WriteInt(chatroomId);
        }
    }
}