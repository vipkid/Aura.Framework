using Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing;
using Aura.Framework.Connectivity.Shared.Messages;
using System;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the keep-alive packet.
    /// </summary>
    public class KeepAlivePacket : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the client.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int clientTime = message.ReadInt32();
            int serverTime = (int)(DateTime.UtcNow.Subtract(new DateTime(2016, 1, 1))).TotalMilliseconds;

            connection.SendMessage(new KeepAlivePacketComposer(clientTime, serverTime));
        }
    }
}