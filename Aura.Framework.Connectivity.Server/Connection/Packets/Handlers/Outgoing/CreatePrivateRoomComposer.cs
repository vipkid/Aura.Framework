using Aura.Framework.Connectivity.Server.Chatrooms;
using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the create private chatroom packet.
    /// </summary>
    public class CreatePrivateRoomComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public CreatePrivateRoomComposer(Chatroom room, string roomName = "") : base(ServerPackets.CreatePrivateRoom)
        {
            WriteString(string.IsNullOrWhiteSpace(roomName) ? room.Name : roomName);
            room.CreatePacket(this);
        }
    }
}