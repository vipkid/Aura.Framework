using Aura.Framework.Connectivity.Server.Models;
using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming
{
    /// <summary>
    /// Represents the conncetion data packet.
    /// </summary>
    public class ConnectionDataPacket : IPacketHandler
    {
        /// <summary>
        /// Handles the incoming packet that has been sent from the client.
        /// </summary>
        public void Parse(ClientMessage message, ConnectionCore connection)
        {
            do
            {
                connection.ConnectionData.UserData.Add(DataPackage.Read(message));
            }
            while (message.RemainingLength() > 0);
        }
    }
}