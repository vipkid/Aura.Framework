using Aura.Framework.Connectivity.Server.Connection;
using Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing;
using System.Collections.Generic;

namespace Aura.Framework.Connectivity.Server.Chatrooms
{
    /// <summary>
    /// Represents the manager for private chatrooms.
    /// </summary>
    public class PrivateRoomManager
    {
        #region Fields

        /// <summary>
        /// Provides a list of private chatrooms and their IDs.
        /// </summary>
        public Dictionary<int, Chatroom> PrivateChatrooms { get; private set; }

        /// <summary>
        /// Represents the random-person queue list.
        /// </summary>
        public SpecialQueue<ConnectionCore> ConnectionQueue { get; private set; }

        /// <summary>
        /// Represents the server manager.
        /// </summary>
        public ServerManager ServerManager { get; private set; }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Adds a specific connection to the connection queue.
        /// </summary>
        public void AddToQueue(ConnectionCore connection)
        {
            if (ConnectionQueue.Contains(connection))
                return;

            ServerLogger.PrivateRoom($"Added {connection.ConnectionData.Username} ({connection.ConnectionData.ClientIP.ToString()}) to the private chatroom queue");
            ConnectionQueue.Enqueue(connection);

            if (ConnectionQueue.Count >= 2)
            {
                CreatePrivateRoom(2);
            }
        }

        /// <summary>
        /// Removes a specific connection to the connection queue.
        /// </summary>
        public void RemoveFromQueue(ConnectionCore connection)
        {
            if (ConnectionQueue.Contains(connection))
            {
                ConnectionQueue.Remove(connection);
                ServerLogger.PrivateRoom($"Removed {connection.ConnectionData.Username} ({connection.ConnectionData.ClientIP.ToString()}) from the privatechatroom queue");
            }
        }

        /// <summary>
        /// Creates and returns a private chatroom.
        /// </summary>
        public Chatroom CreatePrivateRoom(int joincount)
        {
            if (ConnectionQueue.Count < joincount)
                return null;

            Chatroom privateRoom = new Chatroom(ServerManager.ChatroomManager.LastRoomId++);
            PrivateChatrooms.Add(privateRoom.ID, privateRoom);
            ServerManager.ChatroomManager.AddChatroom(privateRoom);

            for (int i = 0; i < joincount; i++)
            {
                ConnectionCore core = ConnectionQueue.Dequeue();

                privateRoom.Join(core);
                core.SendMessage(new CreatePrivateRoomComposer(privateRoom));
                core.SendMessage(new JoinChatroomComposer(privateRoom.ID, JoinState.JoinChatRoomOk));
            }

            return privateRoom;
        }

        /// <summary>
        /// Removes a private chatroom.
        /// </summary>
        public void RemoveChatroom(int chatRoomId)
        {
            PrivateChatrooms.Remove(chatRoomId);
        }

        /// <summary>
        /// Sends a chatmessage to the chatroom.
        /// </summary>
        public void SendChatMessage(int privateRoomId, string privateMessage, ConnectionCore connection)
        {
            if (PrivateChatrooms.ContainsKey(privateRoomId))
            {
                Chatroom room = PrivateChatrooms[privateRoomId];

                room.SendMessage(connection, privateMessage);
            }
        }

        #endregion Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateRoomManager"/> class.
        /// </summary>
        /// <param name="manager"></param>
        public PrivateRoomManager(ServerManager manager)
        {
            PrivateChatrooms = new Dictionary<int, Chatroom>();
            ConnectionQueue = new SpecialQueue<ConnectionCore>();
            ServerManager = manager;
        }

        #endregion Constructors
    }
}