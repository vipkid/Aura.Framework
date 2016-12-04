using Aura.Framework.Connectivity.Client.Chatrooms;
using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the create private room packet.
    /// </summary>
    public class CreatePrivateRoom : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            string roomName = message.ReadString();
            Chatroom chatRoom = Chatroom.CreateFromPacket(message);
            connection.ClientManager.ChatroomManager.AddPrivateRoom(chatRoom);
        }
    }
}