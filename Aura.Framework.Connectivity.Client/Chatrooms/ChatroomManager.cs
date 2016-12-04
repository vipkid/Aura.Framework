using Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Incoming;
using Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Outgoing;
using Aura.Framework.Connectivity.Client.Events;
using System;
using System.Collections.Generic;

namespace Aura.Framework.Connectivity.Client.Chatrooms
{
    /// <summary>
    /// Represents a chatroom manager that manages all the chatrooms.
    /// </summary>
    public class ChatroomManager
    {
        #region Properties

        /// <summary>
        /// Represents a <see cref="Dictionary{TKey, TValue}"/> containing all the chatrooms.
        /// </summary>
        public Dictionary<int, Chatroom> Chatrooms { get; private set; }

        /// <summary>
        /// Represents a <see cref="List{T}"/> containing all the chatrooms.
        /// </summary>
        public List<Chatroom> ChatroomsList { get; private set; }

        /// <summary>
        /// Represents the client manager used in the current instance of the <see cref="ChatroomManager"/> class.
        /// </summary>
        public ClientManager ClientManager { get; private set; }

        /// <summary>
        /// Occurs when the chatroom list is changed.
        /// </summary>
        public event EventHandler ChatroomsChanged;

        /// <summary>
        /// Occurs when a user disconnect from a chatroom.
        /// </summary>
        public event EventHandler<DisconnectChatroomEventArgs> ChatroomDisconnect;

        /// <summary>
        /// Occurs when a user connects to a chatroom.
        /// </summary>
        public event EventHandler<JoinedChatroomEventArgs> ChatroomJoined;

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatroomManager"/> class.
        /// </summary>
        public ChatroomManager(ClientManager clientManager)
        {
            Chatrooms = new Dictionary<int, Chatroom>();
            ChatroomsList = new List<Chatroom>();
            ClientManager = clientManager;
        }

        #endregion Constructors & Deconstructors

        #region Events

        protected virtual void OnChatroomsChanged(EventArgs e)
        {
            ChatroomsChanged?.Invoke(this, e);
        }

        protected virtual void OnChatroomJoined(JoinedChatroomEventArgs e)
        {
            ChatroomJoined?.Invoke(this, e);
        }

        protected virtual void OnChatroomDisconnect(DisconnectChatroomEventArgs e)
        {
            ChatroomDisconnect?.Invoke(this, e);
        }

        #endregion Events

        #region Public Methods

        /// <summary>
        /// Initiliazes all the chatrooms based on the <see cref="Dictionary{TKey, TValue}"/> parameter.
        /// </summary>
        public void InitializeChatrooms(Dictionary<int, Chatroom> rooms)
        {
            foreach (var item in rooms)
            {
                Chatroom room = item.Value;

                if (!Chatrooms.ContainsKey(room.ID))
                {
                    Chatrooms.Add(room.ID, room);
                    ChatroomsList.Add(room);
                }
            }

            List<Chatroom> toRemove = new List<Chatroom>();

            foreach (var item in Chatrooms)
            {
                if (!item.Value.IsPrivate && !rooms.ContainsKey(item.Key))
                {
                    toRemove.Add(item.Value);
                }
            }

            foreach (var item in toRemove)
            {
                Chatrooms.Remove(item.ID);
                ChatroomsList.Remove(item);
            }

            OnChatroomsChanged(new EventArgs());
        }

        /// <summary>
        /// Adds a private chatroom to the chatroom mananger.
        /// </summary>
        public void AddPrivateRoom(Chatroom room)
        {
            room.IsPrivate = true;

            Chatrooms.Add(room.ID, room);
            ChatroomsList.Add(room);

            OnChatroomsChanged(new EventArgs());
        }

        /// <summary>
        /// Creates a new chatroom and sends the request to the server.
        /// </summary>
        public void CreateChatroom(string name, string description, string password)
        {
            ClientManager.ConnectionCore.SendMessage(new CreateRoomComposer(name, description, password));
        }

        /// <summary>
        /// Disconnects from a <see cref="Chatroom"/>.
        /// </summary>
        public void DisconnectChatroom(Chatroom room)
        {
            room.IsInChatroom = false;
            OnChatroomDisconnect(new DisconnectChatroomEventArgs(room));
        }

        /// <summary>
        /// Joins a chatroom based on <see cref="Chatroom.ID"/>.
        /// </summary>
        public void JoinChatroom(int chatRoomId, JoinState state)
        {
            if (Chatrooms.ContainsKey(chatRoomId))
            {
                Chatroom room = Chatrooms[chatRoomId];
                OnChatroomJoined(new JoinedChatroomEventArgs(room, state));
                if (ClientManager.ConnectionCore.ConnectionId != -1 && state == JoinState.JoinChatRoomOk)
                {
                    ChatroomUser user = new ChatroomUser(ClientManager.ConnectionCore.ConnectionId, ClientManager.ConnectionCore.ConnectionData.Username);

                    // room.ChatRoomUsers.Add(user.ConnectionId, user);

                    room.IsInChatroom = true;
                }
            }
            else
            {
                OnChatroomJoined(new JoinedChatroomEventArgs(null, JoinState.JoinChatRoomError));
            }
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the user is in the chatroom.
        /// </summary>
        public bool IsInChatroom(int id)
        {
            if (Chatrooms.ContainsKey(id))
            {
                Chatroom room = Chatrooms[id];
                return room.IsInChatroom;
            }

            return false;
        }

        /// <summary>
        /// Sends a <see cref="JoinQueueComposer"/> to the server.
        /// </summary>
        public void JoinQueue()
        {
            ClientManager.ConnectionCore.SendMessage(new JoinQueueComposer(true));
        }

        /// <summary>
        /// Sends a <see cref="JoinQueueComposer"/> to the server.
        /// </summary>
        public void LeaveQueue()
        {
            ClientManager.ConnectionCore.SendMessage(new JoinQueueComposer(false));
        }

        /// <summary>
        /// Removes a chatroom based on the <see cref="Chatroom.ID"/>.
        /// </summary>
        public void RemoveChatroom(int chatroomId)
        {
            if (Chatrooms.ContainsKey(chatroomId))
            {
                Chatroom room = Chatrooms[chatroomId];
                DisconnectChatroom(room);
                ChatroomsList.Remove(room);
                Chatrooms.Remove(chatroomId);
            }
        }

        /// <summary>
        /// Handles a chatroom message.
        /// </summary>
        public void HandleMessage(ChatMessage message)
        {
            if (Chatrooms.ContainsKey(message.ChatroomID))
            {
                Chatroom room = Chatrooms[message.ChatroomID];
                room.NewMessage(message);
            }
        }

        /// <summary>
        /// Handles a chatroom voice message.
        /// </summary>
        public void HandleMessage(Voice.VoiceMessage message)
        {
            if (Chatrooms.ContainsKey(message.ChatroomID))
            {
                Chatroom room = Chatrooms[message.ChatroomID];
                room.NewMessage(message);
            }
        }

        #endregion Public Methods
    }
}