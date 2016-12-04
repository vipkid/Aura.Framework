using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the chat-message packet.
    /// </summary>
    public class ChatMessageComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public ChatMessageComposer(int chatroomId, int messageId, int userId, string username, string message) : base(ServerPackets.ChatMessage)
        {
            WriteInt(chatroomId);
            WriteInt(messageId);
            WriteInt(userId);
            WriteString(username);
            WriteString(message);
        }
    }
}