using Aura.Framework.Core.Shared.Messages;
using Aura.Framework.Shared.Packets;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Outgoing
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