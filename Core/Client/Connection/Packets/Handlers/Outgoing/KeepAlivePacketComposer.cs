using Aura.Framework.Shared.Messages;
using Aura.Framework.Shared.Packets;
using System;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the keep-alive packet (which is only for calculating latency).
    /// </summary>
    public class KeepAlivePacketComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server. to the server.
        /// </summary>
        public KeepAlivePacketComposer() : base(ClientPackets.KeepAlivePacket)
        {
            // Changed 1970 -> 2016 to reduce int length:
            int clientTime = (int)(DateTime.UtcNow.Subtract(new DateTime(2016, 1, 1))).TotalMilliseconds;

            // Write the clientTime:
            WriteInt(clientTime);
        }
    }
}