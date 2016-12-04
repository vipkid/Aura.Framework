namespace Aura.Framework.Shared.Packets
{
    /// <summary>
    /// Provides packet-ids associated with the server.
    /// </summary>
    public static class ServerPackets
    {
        #region Base-Packets

        /// <summary>
        /// This ID is part of the base-packets series (0-99).
        /// </summary>
        public const int InitializeConnection = 1;

        /// <summary>
        /// This ID is part of the base-packets series (0-99).
        /// </summary>
        public const int KeepAlivePacket = 3;

        #endregion Base-Packets

        #region Chat-Packets

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int ChatMessage = 100;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int InitializeChatrooms = 102;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int JoinChatroom = 103;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int VoiceMessage = 105;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int BroadcastChatMessage = 107;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int CreatePrivateRoom = 108;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int RemovePrivateRoom = 110;

        #endregion Chat-Packets

        #region Account-Packets

        /// <summary>
        /// This ID is part of the chat-packets series (200-299).
        /// </summary>
        public const int RegisterAccount = 200;

        /// <summary>
        /// This ID is part of the chat-packets series (200-299).
        /// </summary>
        public const int LoginAccount = 201;

        #endregion Account-Packets
    }

    /// <summary>
    /// Provides packet-ids associated with the client.
    /// </summary>
    public static class ClientPackets
    {
        #region Base-Packets

        /// <summary>
        /// This ID is part of the base-packets series (0-99).
        /// </summary>
        public const int InitializeConnection = 2;

        /// <summary>
        /// This ID is part of the base-packets series (0-99).
        /// </summary>
        public const int KeepAlivePacket = 4;

        /// <summary>
        /// This ID is part of the base-packets series (0-99).
        /// </summary>
        public const int ConnectionData = 5;

        #endregion Base-Packets

        #region Chat-Packets

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int ChatMessage = 101;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int JoinChatroom = 104;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int VoiceMessage = 106;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int CreateRoom = 109;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int LeaveChatroom = 111;

        /// <summary>
        /// This ID is part of the chat-packets series (100-199).
        /// </summary>
        public const int JoinQueue = 112;

        #endregion Chat-Packets

        #region Account-Packets

        /// <summary>
        /// This ID is part of the chat-packets series (200-299).
        /// </summary>
        public const int RegisterAccount = 200;

        /// <summary>
        /// This ID is part of the chat-packets series (200-299).
        /// </summary>
        public const int LoginAccount = 201;

        #endregion Account-Packets
    }
}