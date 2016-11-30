using Aura.Framework.Shared.Messages;

namespace Aura.Framework.Core.Client.Connection.Packets
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