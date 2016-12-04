using Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing;
using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the create-room packet.
    /// </summary>
    public class CreateRoom : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the client.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            string roomName = message.ReadString();
            string roomDesc = message.ReadString();
            string roomPass = message.ReadString();
            if (!string.IsNullOrEmpty(roomName))
            {
                var chatroom = connection.ServerManager.ChatroomManager.CreateChatRoom(roomName, roomDesc, roomPass, connection);
                if (chatroom != null)
                    connection.SendMessage(new JoinChatroomComposer(chatroom.ID, JoinState.JoinChatRoomOk));
            }
        }
    }
}