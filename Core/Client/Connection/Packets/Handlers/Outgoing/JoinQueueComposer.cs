﻿using Aura.Framework.Shared.Messages;
using Aura.Framework.Shared.Packets;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the join queue packet.
    /// </summary>
    public class JoinQueueComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server.
        /// </summary>
        public JoinQueueComposer(bool add) : base(ClientPackets.JoinQueue)
        {
            if (add)
                WriteInt(0);

            if (!add)
                WriteInt(1);

            // 0 = Enqueue
            // 1 = Dequeue
        }
    }
}