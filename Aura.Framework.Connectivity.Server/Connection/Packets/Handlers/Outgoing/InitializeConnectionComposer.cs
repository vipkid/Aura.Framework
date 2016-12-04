using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the initialize connection packet.
    /// </summary>
    public class InitializeConnectionComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public InitializeConnectionComposer(int connectionId) : base(ServerPackets.InitializeConnection)
        {
            WriteInt(connectionId);
        }
    }
}