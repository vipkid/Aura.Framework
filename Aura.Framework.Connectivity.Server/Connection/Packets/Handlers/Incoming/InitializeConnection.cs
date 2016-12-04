using Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing;
using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the initialize connection packet.
    /// </summary>
    public class InitializeConnection : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the client.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            string name = message.ReadString();

            connection.ConnectionData.Username = name;

            connection.SendMessage(new InitializeConnectionComposer(connection.ID));
            connection.SendMessage(new InitializeChatroomsComposer(connection.ServerManager.ChatroomManager.Chatrooms));
        }
    }
}