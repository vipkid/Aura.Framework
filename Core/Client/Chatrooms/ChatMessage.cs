namespace Aura.Framework.Core.Client.Chatrooms
{
    /// <summary>
    /// Represents the type of the <see cref="ChatMessage"/>.
    /// </summary>
    public enum ChatMessageType
    {
        /// <summary>
        /// Represents the broadcast message-type.
        /// </summary>
        BroadCast,

        /// <summary>
        /// Represents the normal message-type.
        /// </summary>
        Message
    }

    /// <summary>
    /// Represents the message that has been send to a particular chat.
    /// </summary>
    public class ChatMessage
    {
        #region Properties

        /// <summary>
        /// Returns the chatroom id.
        /// </summary>
        public int ChatroomID { get; private set; }

        /// <summary>
        /// Returns the message id.
        /// </summary>
        public int MessageID { get; private set; }

        /// <summary>
        /// Returns the message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Returns the user that has send the message.
        /// </summary>
        public ChatroomUser ChatroomUser { get; internal set; }

        /// <summary>
        /// Returns or sets the message type.
        /// </summary>
        public ChatMessageType MessageType { get; set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class as a normal message.
        /// </summary>
        public ChatMessage(int chatRoomId, int messageId, string message, ChatroomUser user)
        {
            ChatroomID = chatRoomId;
            MessageID = messageId;
            Message = message;
            ChatroomUser = user;
            MessageType = ChatMessageType.Message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class as a broadcast message.
        /// </summary>
        public ChatMessage(int chatRoomId, int messageId, string message)
        {
            ChatroomID = chatRoomId;
            MessageID = messageId;
            Message = message;
            MessageType = ChatMessageType.BroadCast;
        }

        #endregion Constructors & Deconstructors
    }
}