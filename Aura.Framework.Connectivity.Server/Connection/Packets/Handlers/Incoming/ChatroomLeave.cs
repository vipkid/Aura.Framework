using Aura.Framework.Connectivity.Server.Chatrooms;
using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents a packet for a chatroom 'leave' event.
    /// </summary>
    public class ChatroomLeave : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the client.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int userId = message.ReadInt32();
            int roomId = message.ReadInt32();

            Chatroom room = connection.ServerManager.ChatroomManager.GetChatroom(roomId);

            if (room != null)
            {
                room.Leave(connection);
            }
        }
    }
}