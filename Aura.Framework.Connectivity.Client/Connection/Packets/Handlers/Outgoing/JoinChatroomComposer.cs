using Aura.Framework.Connectivity.Shared.Messages;
using Aura.Framework.Connectivity.Shared.Packets;

namespace Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the join chatroom packet.
    /// </summary>
    public class JoinChatroomComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server.
        /// </summary>
        public JoinChatroomComposer(int chatRoomId, string password) : base(ClientPackets.JoinChatroom)
        {
            WriteInt(chatRoomId);
            WriteString(password);
        }
    }
}