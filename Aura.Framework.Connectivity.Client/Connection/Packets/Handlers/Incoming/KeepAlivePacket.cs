using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the keep-alive packet.
    /// </summary>
    public class KeepAlivePacket : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            // Reads the clientTime
            int clientTime = message.ReadInt32();

            // Reads the serverTime
            int serverTime = message.ReadInt32();

            // Calculate the latency
            int totalLatency = (serverTime) - (clientTime);
        }
    }
}