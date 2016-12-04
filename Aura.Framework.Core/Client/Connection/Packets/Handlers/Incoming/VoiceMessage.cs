using Aura.Framework.Core.Client.Chatrooms;
using Aura.Framework.Core.Shared.Messages;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the voice message packet.
    /// </summary>
    public class VoiceMessage : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int chatroomId = message.ReadInt32();
            int messageId = message.ReadInt32();
            int UserId = message.ReadInt32();
            int length = message.ReadInt32();
            string username = message.ReadString();
            byte[] chatMessage = message.ReadBytes(length);

            Chatrooms.Voice.VoiceMessage voiceMessage = new Chatrooms.Voice.VoiceMessage(chatroomId, messageId, chatMessage, new ChatroomUser(UserId, username));
            connection.ClientManager.ChatroomManager.HandleMessage(voiceMessage);
        }
    }
}