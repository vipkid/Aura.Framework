using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the enqueue or dequeue packet.
    /// </summary>
    public class JoinQueue : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the client.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            int addOrRemoveObject = message.ReadInt32();

            if (addOrRemoveObject == 0)
                connection.ServerManager.PrivateRoomManager.AddToQueue(connection);

            if (addOrRemoveObject == 1)
                connection.ServerManager.PrivateRoomManager.RemoveFromQueue(connection);

            // 0 = Queue
            // 1 = Dequeue
        }
    }
}