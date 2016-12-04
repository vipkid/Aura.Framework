using Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing;
using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the join-chatroom packet.
    /// </summary>
    public class JoinChatroom : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the client.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int chatroomId = message.ReadInt32();
            string password = message.ReadString();

            JoinState state = connection.ServerManager.ChatroomManager.JoinChatroom(chatroomId, password, connection);

            connection.SendMessage(new JoinChatroomComposer(chatroomId, state));
        }
    }
}