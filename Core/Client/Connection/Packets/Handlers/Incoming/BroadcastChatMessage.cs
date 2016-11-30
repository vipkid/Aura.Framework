using Aura.Framework.Shared.Messages;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the broadcast chatmessage packet.
    /// </summary>
    public class BroadcastChatMessage : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int chatroomId = message.ReadInt32();
            int messageId = message.ReadInt32();
            int userId = message.ReadInt32();

            string broadcastMessage = message.ReadString();

            Chatrooms.ChatMessage broadcastChatMessage = new Chatrooms.ChatMessage(chatroomId, messageId, broadcastMessage);
            connection.ClientManager.ChatroomManager.HandleMessage(broadcastChatMessage);
        }
    }
}