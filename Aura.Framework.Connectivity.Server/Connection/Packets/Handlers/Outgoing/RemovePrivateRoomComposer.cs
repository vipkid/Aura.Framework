using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the remove private chatroom packet.
    /// </summary>
    public class RemovePrivateRoomComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public RemovePrivateRoomComposer(int roomId) : base(ServerPackets.RemovePrivateRoom)
        {
            WriteInt(roomId);
        }
    }
}