using Aura.Framework.Connectivity.Client.Chatrooms.Voice;

namespace Aura.Framework.Connectivity.Client.Events
{
    /// <summary>
    /// Provides data for the <see cref="VoiceReceivedEventArgs"/> event arguments.
    /// </summary>
    public class VoiceReceivedEventArgs
    {
        #region Properties

        /// <summary>
        /// Represents the voice message that has been send & received.
        /// </summary>
        public VoiceMessage Message { get; private set; }

        #endregion Properties

        #region Contructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceReceivedEventArgs"/> class.
        /// </summary>
        public VoiceReceivedEventArgs(VoiceMessage message)
        {
            Message = message;
        }

        #endregion Contructors & Deconstructors
    }
}