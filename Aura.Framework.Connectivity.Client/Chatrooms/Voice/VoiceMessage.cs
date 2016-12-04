namespace Aura.Framework.Connectivity.Client.Chatrooms.Voice
{
    /// <summary>
    /// Represents the voice message that has been send to a particular voice chat.
    /// </summary>
    public class VoiceMessage
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
        public byte[] Message { get; private set; }

        /// <summary>
        /// Returns the user that has send the message.
        /// </summary>
        public ChatroomUser ChatroomUser { get; internal set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceMessage"/> class.
        /// </summary>
        public VoiceMessage(int chatRoomId, int messageId, byte[] message, ChatroomUser user)
        {
            ChatroomID = chatRoomId;
            MessageID = messageId;
            Message = message;
            ChatroomUser = user;
        }

        #endregion Constructors & Deconstructors
    }
}