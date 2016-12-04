using Aura.Framework.Connectivity.Server.Connection;
using Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing;
using System.Collections.Generic;
using System.Linq;

namespace Aura.Framework.Connectivity.Server.Chatrooms
{
    /// <summary>
    /// Represents a chatroom manager
    /// </summary>
    public class ChatroomManager
    {
        #region Fields

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}"/> with the chatrooms.
        /// </summary>
        public Dictionary<int, Chatroom> Chatrooms { get; private set; }

        /// <summary>
        /// Returns the server manager.
        /// </summary>
        public ServerManager ServerManager { get; private set; }

        /// <summary>
        /// Gets or sets the last chatroom ID.
        /// </summary>
        public int LastRoomId { get; set; }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Connects a user to a chatroom and returns a <see cref="JoinState"/>.
        /// </summary>
        public JoinState JoinChatroom(int chatRoomId, string password, ConnectionCore connection)
        {
            if (Chatrooms.ContainsKey(chatRoomId))
            {
                Chatroom room = Chatrooms[chatRoomId];

                if (string.IsNullOrEmpty(room.Password) || password == room.Password || room.Owner == connection.ID)
                {
                    room.Join(connection);
                    return JoinState.JoinChatRoomOk;
                }
                else
                {
                    return JoinState.JoinChatRoomWrongPassword;
                }
            }
            else
            {
                return JoinState.JoinChatRoomError;
            }
        }

        /// <summary>
        /// Returns a <see cref="Chatroom"/> based on its ID.
        /// </summary>
        public Chatroom GetChatroom(int chatroomId)
        {
            try
            {
                if (Chatrooms.ContainsKey(chatroomId))
                {
                    return Chatrooms[chatroomId];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes all the chatrooms for a connection.
        /// </summary>
        public void InitializeChatrooms(ConnectionCore connection)
        {
            connection.SendMessage(new InitializeChatroomsComposer(Chatrooms));
        }

        /// <summary>
        /// Createsand returns a <see cref="Chatroom"/> based on the name, description and password.
        /// </summary>
        public Chatroom CreateChatRoom(string name, string desc, string pass, ConnectionCore connection)
        {
            Chatroom chatRoom = new Chatroom(LastRoomId++, name, desc, pass);
            chatRoom.Join(connection);
            Chatrooms.Add(chatRoom.ID, chatRoom);

            ServerManager.ConnectionController.SendMessage(new InitializeChatroomsComposer(Chatrooms));
            return chatRoom;
        }

        /// <summary>
        /// Adds a chatroom to the <see cref="Chatrooms"/> <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        public void AddChatroom(Chatroom chatroom)
        {
            Chatrooms.Add(chatroom.ID, chatroom);
        }

        /// <summary>
        /// Disconnects a <see cref="ConnectionCore"/> from each chatroom in <see cref="Chatrooms"/>.
        /// </summary>
        public void LeaveAllChatrooms(ConnectionCore connection)
        {
            foreach (var room in Chatrooms.ToList())
            {
                if (room.Value.ChatroomUsers.Contains(connection))
                    room.Value.Leave(connection);
            }
        }

        /// <summary>
        /// Removes a <see cref="Chatroom"/> based on its ID.
        /// </summary>
        public void RemoveChatroom(int chatroomId)
        {
            Chatrooms.Remove(chatroomId);
            ServerManager.ConnectionController.SendMessage(new InitializeChatroomsComposer(Chatrooms));
        }

        /// <summary>
        /// Sends a message to a certain <see cref="Chatroom"/>.
        /// </summary>
        public void SendMessage(int chatroomId, string chatmessage, ConnectionCore connection)
        {
            if (Chatrooms.ContainsKey(chatroomId))
            {
                Chatroom room = Chatrooms[chatroomId];

                room.SendMessage(connection, chatmessage);
            }
        }

        /// <summary>
        /// Sends a voice message to a certain <see cref="Chatroom"/>.
        /// </summary>
        public void SendMessage(int chatroomId, byte[] voice, ConnectionCore connection)
        {
            if (Chatrooms.ContainsKey(chatroomId))
            {
                Chatroom room = Chatrooms[chatroomId];

                room.SendMessage(connection, voice);
            }
        }

        #endregion Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatroomManager"/> class.
        /// </summary>
        public ChatroomManager(ServerManager manager)
        {
            Chatrooms = new Dictionary<int, Chatroom>();
            ServerManager = manager;
            LastRoomId = 0;
        }

        #endregion Constructors

    }
}