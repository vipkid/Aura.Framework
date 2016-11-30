using Aura.Framework.Core.Client.Chatrooms;
using Aura.Framework.Shared.Messages;
using System.Collections.Generic;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the initialize chatrooms packet.
    /// </summary>
    public class InitializeChatrooms : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            Dictionary<int, Chatroom> chatRooms = new Dictionary<int, Chatroom>();
            int chatRoomUsers = message.ReadInt32();

            for (int i = 0; i < chatRoomUsers; i++)
            {
                Chatroom chatRoom = Chatroom.CreateFromPacket(message);
                chatRooms.Add(chatRoom.ID, chatRoom);
            }

            connection.ClientManager.ChatroomManager.InitializeChatrooms(chatRooms);
        }
    }
}