namespace Aura.Framework.Core.Client.Chatrooms
{
    /// <summary>
    /// Represents a chatroom user.
    /// </summary>
    public class ChatroomUser
    {
        #region Properties

        /// <summary>
        /// Gets or sets the connection id of the <see cref="ChatroomUser"/>.
        /// </summary>
        public int ConnectionID { get; set; }

        /// <summary>
        /// Gets or sets the username of the <see cref="ChatroomUser"/>.
        /// </summary>
        public string Username { get; set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatroomUser"/> class.
        /// </summary>
        public ChatroomUser(int connectionId, string username)
        {
            ConnectionID = connectionId;
            Username = username;
        }

        #endregion Constructors & Deconstructors
    }
}