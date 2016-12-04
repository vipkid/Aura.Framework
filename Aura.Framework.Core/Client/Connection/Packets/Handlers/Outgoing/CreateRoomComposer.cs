using Aura.Framework.Core.Shared.Messages;
using Aura.Framework.Shared.Packets;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the create room packet.
    /// </summary>
    public class CreateRoomComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server.
        /// </summary>
        public CreateRoomComposer(string name, string description, string password) : base(ClientPackets.CreateRoom)
        {
            WriteString(name);
            WriteString(description);
            WriteString(password);
        }
    }
}