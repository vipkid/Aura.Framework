using Aura.Framework.Core.Client.Chatrooms;

namespace Aura.Framework.Core.Client.Events
{
    /// <summary>
    /// Provides data for the <see cref="MessageReceivedEventArgs"/> event arguments.
    /// </summary>
    public class MessageReceivedEventArgs
    {
        #region Properties

        /// <summary>
        /// Represents the chat message that has been send & received.
        /// </summary>
        public ChatMessage Message { get; private set; }

        #endregion Properties

        #region Contructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceivedEventArgs"/> class.
        /// </summary>
        public MessageReceivedEventArgs(ChatMessage message)
        {
            Message = message;
        }

        #endregion Contructors & Deconstructors
    }
}