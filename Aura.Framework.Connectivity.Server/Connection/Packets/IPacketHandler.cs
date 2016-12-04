using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Server.Connection.Packets
{
    /// <summary>
    /// Represents an interface of the packet handler.
    /// </summary>
    public interface IPacketHandler
    {
        /// <summary>
        /// Parses the incoming packet.
        /// </summary>
        void Parse(ClientMessage message, ConnectionCore connection);
    }
}