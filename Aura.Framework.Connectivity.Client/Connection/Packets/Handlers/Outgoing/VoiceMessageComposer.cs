using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the voice message packet.
    /// </summary>
    public class VoiceMessageComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server.
        /// </summary>
        public VoiceMessageComposer(byte[] message, int chatroomId) : base(ClientPackets.VoiceMessage)
        {
            WriteInt(message.Length);
            WriteBytes(message);
            WriteInt(chatroomId);
        }
    }
}