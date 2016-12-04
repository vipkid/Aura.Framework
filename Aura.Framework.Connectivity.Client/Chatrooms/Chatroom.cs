using Aura.Framework.Connectivity.Client.Chatrooms.Voice;
using Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Outgoing;
using Aura.Framework.Connectivity.Client.Events;
using Aura.Framework.Connectivity.Shared.Messages;
using System;
using System.Collections.Generic;

namespace Aura.Framework.Connectivity.Client.Chatrooms
{
    /// <summary>
    /// Represents a chatroom.
    /// </summary>
    public class Chatroom
    {
        #region Properties

        /// <summary>
        /// Represents the id of the current instance of the <see cref="Chatroom"/> class.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Represents the description of the current instance of the <see cref="Chatroom"/> class.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Represents the name of the current instance of the <see cref="Chatroom"/> class.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the current instance of the <see cref="Chatroom"/> class contains a password.
        /// </summary>
        public bool IsProtected { get; private set; }

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the message is a broadcast.
        /// </summary>
        public bool IsBroadcast { get; private set; }

        /// <summary>
        /// Occurs when the current instance of the <see cref="Chatroom"/> class received a chatmessage.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Occurs when the current instance of the <see cref="Chatroom"/> class received a voicemessage.
        /// </summary>
        public event EventHandler<VoiceReceivedEventArgs> VoiceReceived;

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the requested user is in the chatroom.
        /// </summary>
        public bool IsInChatroom { get; set; }

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the current instance of the <see cref="Chatroom"/> class is private.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Represents a list of all the messages sent to the current instance of the <see cref="Chatroom"/> class.
        /// </summary>
        public List<ChatMessage> Messages { get; private set; }

        #endregion Properties

        #region Events

        /// <summary>
        /// Handles and fires the <see cref="OnMessageReceived(MessageReceivedEventArgs)"/> event.
        /// </summary>
        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Handles and fires the <see cref="OnVoiceReceived(VoiceReceivedEventArgs)"/> event.
        /// </summary>
        protected virtual void OnVoiceReceived(VoiceReceivedEventArgs e)
        {
            VoiceReceived?.Invoke(this, e);
        }

        #endregion Events

        #region Constructors & Deconstructors

        /// <summary>
        /// Initiliazes a new instance of the <see cref="Chatroom"/> class.
        /// </summary>
        public Chatroom(int id, string name, string description, bool password)
        {
            Messages = new List<ChatMessage>();

            ID = id;
            Description = description;
            Name = name;
            IsProtected = password;
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Constructs a new message.
        /// </summary>
        public void NewMessage(ChatMessage message)
        {
            Messages.Add(message);
            OnMessageReceived(new MessageReceivedEventArgs(message));
        }

        /// <summary>
        /// Constructs a new voice message.
        /// </summary>
        public void NewMessage(VoiceMessage message)
        {
            OnVoiceReceived(new VoiceReceivedEventArgs(message));
        }

        /// <summary>
        /// Sends a message to the current instance of the <see cref="Chatroom"/> class.
        /// </summary>
        public ChatMessageComposer SendMessage(string message)
        {
            return new ChatMessageComposer(message, ID);
        }

        /// <summary>
        /// Sends a voice packet to the current instance of the <see cref="Chatroom"/> class.
        /// </summary>
        public VoiceMessageComposer SendMessage(byte[] voice)
        {
            return new VoiceMessageComposer(voice, ID);
        }

        /// <summary>
        /// Leaves the chatroom based on the chatroom-id.
        /// </summary>
        public ChatroomLeaveComposer Leave(int userId, int roomId)
        {
            return new ChatroomLeaveComposer(userId, roomId);
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Creates a chatroom from a packet(<see cref="ClientMessage"/>).
        /// </summary>
        public static Chatroom CreateFromPacket(ClientMessage message)
        {
            int id = message.ReadInt32();
            string name = message.ReadString();
            string description = message.ReadString();
            bool password = message.ReadBoolean();
            List<ChatroomUser> chatroomUsers = new List<ChatroomUser>();
            int users = message.ReadInt32();

            for (int i = 0; i < users; i++)
            {
                int connectionId = message.ReadInt32();
                string username = message.ReadString();

                // TODO: Return or add this list.
                ChatroomUser user = new ChatroomUser(connectionId, username);
                chatroomUsers.Add(user);
            }

            return new Chatroom(id, name, description, password);
        }

        #endregion Static Methods
    }
}