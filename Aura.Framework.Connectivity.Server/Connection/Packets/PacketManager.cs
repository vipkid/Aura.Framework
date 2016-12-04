using Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Incoming;
using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;
using System.Collections.Generic;

namespace Aura.Framework.Connectivity.Server.Connection.Packets
{
    /// <summary>
    /// Provides functionality for handling of incoming packets.
    /// </summary>
    public class PacketManager
    {
        #region Properties

        /// <summary>
        /// Contains a set of packets that are currently being handled.
        /// </summary>
        public Dictionary<int, IPacketHandler> PacketHandlers { get; private set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initialize a new instance of the <see cref="PacketManager"/> class.
        /// </summary>
        public PacketManager()
        {
            PacketHandlers = new Dictionary<int, IPacketHandler>();

            AddPackets();
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Adds a single custom packet to the packet handlers list.
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
        /// Handles a packet that has been sent from a client.
        /// </summary>
        public void HandlePacket(ClientMessage message, ConnectionCore connection)
        {
            // Handle the packet if it exists
            if (PacketHandlers.ContainsKey(message.PacketId))
            { ServerLogger.Handled($"[{message.PacketId}] {message.ToString()}"); PacketHandlers[message.PacketId].Parse(message, connection); }
            else
            { ServerLogger.Unhandled($"[{message.PacketId}] {message.ToString()}"); }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Adds all the base packets to the packet handlers.
        /// </summary>
        private void AddPackets()
        {
            PacketHandlers.Clear();

            PacketHandlers.Add(ClientPackets.InitializeConnection, new InitializeConnection());
            PacketHandlers.Add(ClientPackets.KeepAlivePacket, new KeepAlivePacket());
            PacketHandlers.Add(ClientPackets.CreateRoom, new CreateRoom());
            PacketHandlers.Add(ClientPackets.JoinChatroom, new JoinChatroom());
            PacketHandlers.Add(ClientPackets.ChatMessage, new ChatMessage());
            PacketHandlers.Add(ClientPackets.LeaveChatroom, new ChatroomLeave());
            PacketHandlers.Add(ClientPackets.JoinQueue, new JoinQueue());
            PacketHandlers.Add(ClientPackets.ConnectionData, new ConnectionDataPacket());
            PacketHandlers.Add(ClientPackets.VoiceMessage, new VoiceMessage());
        }

        #endregion Private Methods
    }
}