using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the initialize connection packet.
    /// </summary>
    public class InitializeConnectionComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server.
        /// </summary>
        public InitializeConnectionComposer(string username) : base(ClientPackets.InitializeConnection)
        {
            // Write the username
            WriteString(username);
        }
    }
}