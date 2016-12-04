using Aura.Framework.Core.Shared.Messages;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the initialization connection packet.
    /// </summary>
    public class InitializeConnection : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            // Read the connectionId
            int connectionId = message.ReadInt32();

            // Set the connectionId
            connection.ConnectionId = connectionId;
        }
    }
}