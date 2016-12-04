using Aura.Framework.Connectivity.Client.Chatrooms;
using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the chatmessage packet.
    /// </summary>
    public class ChatMessage : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int chatroomId = message.ReadInt32();
            int messageId = message.ReadInt32();
            int UserId = message.ReadInt32();
            string username = message.ReadString();
            string chatMessage = message.ReadString();

            Chatrooms.ChatMessage ChatMessage = new Chatrooms.ChatMessage(chatroomId, messageId, chatMessage, new ChatroomUser(UserId, username));
            connection.ClientManager.ChatroomManager.HandleMessage(ChatMessage);
        }
    }
}