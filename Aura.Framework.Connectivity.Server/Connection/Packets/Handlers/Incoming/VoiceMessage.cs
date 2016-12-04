using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the chat-message packet.
    /// </summary>
    public class VoiceMessage : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the client.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int length = message.ReadInt32();
            byte[] voice = message.ReadBytes(length);
            int chatroomId = message.ReadInt32();

            connection.ServerManager.ChatroomManager.SendMessage(chatroomId, voice, connection);
        }
    }
}