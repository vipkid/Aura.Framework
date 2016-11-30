using Aura.Framework.Shared.Messages;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the remove private room packet.
    /// </summary>
    public class RemovePrivateRoom : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the server.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int roomId = message.ReadInt32();
            connection.ClientManager.ChatroomManager.RemoveChatroom(roomId);
        }
    }
}