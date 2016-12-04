using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the chat-message packet.
    /// </summary>
    public class VoiceMessageComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public VoiceMessageComposer(int chatroomId, int messageId, int userId, string username, byte[] voice) : base(ServerPackets.VoiceMessage)
        {
            WriteInt(chatroomId);
            WriteInt(messageId);
            WriteInt(userId);
            WriteInt(voice.Length);
            WriteString(username);
            WriteBytes(voice);
        }
    }
}