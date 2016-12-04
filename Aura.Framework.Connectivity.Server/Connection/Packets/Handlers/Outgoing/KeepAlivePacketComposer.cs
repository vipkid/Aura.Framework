using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the keep-alive packet.
    /// </summary>
    public class KeepAlivePacketComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public KeepAlivePacketComposer(int clientTime, int serverTime) : base(ServerPackets.KeepAlivePacket)
        {
            WriteInt(clientTime);
            WriteInt(serverTime);
        }
    }
}