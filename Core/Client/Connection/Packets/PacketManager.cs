using Aura.Framework.Core.Client.Connection.Packets.Handlers.Incoming;
using Aura.Framework.Shared.Messages;
using Aura.Framework.Shared.Packets;
using System.Collections.Generic;

namespace Aura.Framework.Core.Client.Connection.Packets
{
    /// <summary>
    /// Provides functionality for handling of incoming packets.
    /// </summary>
    public class PacketManager
    {
        #region Properties

        /// <summary>
        /// Contains al the packets that can be send to the client.
        /// </summary>
        public Dictionary<int, IPacketHandler> PacketHandlers { get; private set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketManager"/> class.
        /// </summary>
        public PacketManager()
        {
            // Initializes a new Dictionary with PacketHandlers:
            PacketHandlers = new Dictionary<int, IPacketHandler>();

            // Add the packets to the PacketHandlers:
            AddPackets();
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Adds a single custom packet to the <see cref="PacketHandlers"/>.
        /// </summary>
        public bool AddPacket(int packetId, IPacketHandler packet)
        {
            if (!PacketHandlers.ContainsKey(packetId))
            {
                PacketHandlers.Add(packetId, packet);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Handles the packet that has been received.
        /// </summary>
        public void HandlePacket(ClientMessage message, ConnectionCore connection)
        {
            if (PacketHandlers.ContainsKey(message.PacketId))
            {
                PacketHandlers[message.PacketId].Parse(message, connection);

                // Log the Handled packet
                // Console.WriteLine($"Handled {message.PacketId} with {message.Packet.GetType().ToString()}");
            }
            else
            {
                // Log the Handled packet
                // Console.WriteLine($"Unhandled {message.PacketId} with {message.Packet.GetType().ToString()}");
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Adds all the default packets.
        /// </summary>
        private void AddPackets()
        {
            // Clear the list to prevent multiple packet handlers:
            PacketHandlers.Clear();

            // Add all the packets:
            PacketHandlers.Add(ServerPackets.InitializeConnection, new InitializeConnection());
            PacketHandlers.Add(ServerPackets.KeepAlivePacket, new KeepAlivePacket());
            PacketHandlers.Add(ServerPackets.InitializeChatrooms, new InitializeChatrooms());
            PacketHandlers.Add(ServerPackets.JoinChatroom, new JoinChatroom());
            PacketHandlers.Add(ServerPackets.ChatMessage, new ChatMessage());
            PacketHandlers.Add(ServerPackets.BroadcastChatMessage, new BroadcastChatMessage());
            PacketHandlers.Add(ServerPackets.CreatePrivateRoom, new CreatePrivateRoom());
            PacketHandlers.Add(ServerPackets.RemovePrivateRoom, new RemovePrivateRoom());
            PacketHandlers.Add(ServerPackets.VoiceMessage, new VoiceMessage());
        }

        #endregion Private Methods
    }
}