﻿using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the chat message packet.
    /// </summary>
    public class ChatMessageComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server.
        /// </summary>
        public ChatMessageComposer(string message, int chatroomId) : base(ClientPackets.ChatMessage)
        {
            WriteString(message);
            WriteInt(chatroomId);
        }
    }
}