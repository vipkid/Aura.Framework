using Aura.Framework.Connectivity.Server.Chatrooms;
using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the initialize chatroom packet.
    /// </summary>
    public class InitializeChatroomsComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the client.
        /// </summary>
        public InitializeChatroomsComposer(Dictionary<int, Chatroom> rooms) : base(ServerPackets.InitializeChatrooms)
        {
            Dictionary<int, Chatroom> chatRooms = new Dictionary<int, Chatroom>(rooms);
            var @private = chatRooms.Where(o => !o.Value.Private);

            WriteInt(@private.Count());

            foreach (var chatroom in @private)
            {
                chatroom.Value.CreatePacket(this);
            }
        }
    }
}