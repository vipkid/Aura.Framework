using Aura.Framework.Connectivity.Server.Connection;
using Aura.Framework.Connectivity.Server.Connection.Packets.Handlers.Outgoing;
using Aura.Framework.Connectivity.Shared.Messages;
using System.Collections.Generic;

namespace Aura.Framework.Connectivity.Server.Chatrooms
{
    /// <summary>
    /// Represents a chatroom.
    /// </summary>
    public class Chatroom
    {
        #region Fields

        /// <summary>
        /// Returns the ID of the chatroom.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Returns the description of the chatroom.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Returns the name of the chatroom.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the password of the chatroom.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Returns the users of the chatroom.
        /// </summary>
        public List<ConnectionCore> ChatroomUsers { get; private set; }

        /// <summary>
        /// Returns the owners' ID of the chatroom.
        /// </summary>
        public int Owner { get; internal set; }

        /// <summary>
        /// Returns whether the chatroom is private.
        /// </summary>
        public bool Private { get; private set; }

        #endregion Fields

        #region Private Fields

        /// <summary>
        /// Gets or sets the last message ID.
        /// </summary>
        private int _LastChatMessageId = 0;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Chatroom"/> class.
        /// </summary>
        public Chatroom(int roomId, string roomName, string roomDesc, string password = "", bool isPrivate = false)
        {
            ChatroomUsers = new List<ConnectionCore>();
            ID = roomId;
            Description = roomDesc;
            Name = roomName;
            Password = password;
            Private = isPrivate;
        }

        /// <summary>
        /// Initializes a new instance of the private <see cref="Chatroom"/> class.
        /// </summary>
        public Chatroom(int roomId, bool isPrivate = true)
        {
            ChatroomUsers = new List<ConnectionCore>();
            ID = roomId;
            Description = string.Empty;
            Name = string.Format("Private Room ({0})", ID);
            Password = string.Empty;
            Private = isPrivate;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates and writes the <see cref="Chatroom"/> to the stream.
        /// </summary>
        public void CreatePacket(ServerMessage message)
        {
            message.WriteInt(ID);
            message.WriteString(Name);
            message.WriteString(Description);
            message.AppendBoolean(!string.IsNullOrEmpty(Password));
            message.WriteInt(ChatroomUsers.Count);

            foreach (ConnectionCore connection in ChatroomUsers)
            {
                message.WriteInt(connection.ID);
                message.WriteString(connection.ConnectionData.Username);
            }
        }

        /// <summary>
        /// Sends a message to the <see cref="Chatroom"/> instance.
        /// </summary>
        public void SendMessage(ConnectionCore connection, string message)
        {
            if (!ChatroomUsers.Contains(connection))
                return;

            ChatMessageComposer composer = new ChatMessageComposer(ID, _LastChatMessageId++, connection.ID, connection.ConnectionData.Username, message);
            if (ChatroomUsers.Contains(connection))
            {
                foreach (ConnectionCore user in ChatroomUsers.ToArray())
                {
                    user.SendMessage(composer);
                }
            }
        }

        /// <summary>
        /// Sends a voice message to the <see cref="Chatroom"/> instance.
        /// </summary>
        public void SendMessage(ConnectionCore connection, byte[] message)
        {
            if (!ChatroomUsers.Contains(connection))
                return;

            VoiceMessageComposer composer = new VoiceMessageComposer(ID, _LastChatMessageId++, connection.ID, connection.ConnectionData.Username, message);
            if (ChatroomUsers.Contains(connection))
            {
                foreach (ConnectionCore user in ChatroomUsers.ToArray())
                {
                    user.SendMessage(composer);
                }
            }
        }

        /// <summary>
        /// Broadcasts a message to the <see cref="Chatroom"/>.
        /// </summary>
        public void BroadcastChatMessage(ConnectionCore connection, BroadcastType broadcastType)
        {
            BroadcastMessageComposer comp = new BroadcastMessageComposer(ID, _LastChatMessageId++, connection.ID, connection.ConnectionData.Username, broadcastType);

            foreach (ConnectionCore user in ChatroomUsers.ToArray())
            {
                user.SendMessage(comp);
            }
        }

        /// <summary>
        /// Adds a <see cref="ConnectionCore"/> to the <see cref="Chatroom"/>.
        /// </summary>
        public void Join(ConnectionCore connection)
        {
            if (!Private)
                BroadcastChatMessage(connection, BroadcastType.UserJoinedChatRoom);

            if (!ChatroomUsers.Contains(connection))
            {
                ChatroomUsers.Add(connection);
                ServerLogger.Chatroom(string.Format("{1}({0}) has connected to a chatroom.", connection.ID, connection.ConnectionData.Username));
            }
        }

        /// <summary>
        /// Removes a <see cref="ConnectionCore"/> from the <see cref="Chatroom"/>.
        /// </summary>
        public void Leave(ConnectionCore connection)
        {
            BroadcastChatMessage(connection, BroadcastType.UserLeftChatRoom);

            if (ChatroomUsers.Contains(connection))
            {
                ChatroomUsers.Remove(connection);

                ServerLogger.Chatroom(string.Format("{1}({0}) has disconnected from a chatroom.", connection.ID, connection.ConnectionData.Username));
            }
            else
            {
                ServerLogger.Error(string.Format("{1}({0}) cannot disconnect from a chatroom.", connection.ID, connection.ConnectionData.Username));
            }

            if (Private)
                connection.SendMessage(new RemovePrivateRoomComposer(ID));

            if (ChatroomUsers.Count == 0)
            {
                ServerLogger.Warning(string.Format("Chatroom {0} is empty.", ID.ToString()));
                connection.ServerManager.ChatroomManager.RemoveChatroom(ID);
                ServerLogger.Chatroom(string.Format("Chatroom {0} has been deleted.", ID.ToString()));
            }
        }

        #endregion Methods
    }
}