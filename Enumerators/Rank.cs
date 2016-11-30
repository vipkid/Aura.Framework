namespace Aura.Framework.Enumerators
{
    /// <summary>
    /// Represents an enumerator to specify the ranking type of a user.
    /// </summary>
    internal enum Rank
    {
        /// <summary>
        /// Represents an unknown rank.
        /// </summary>
        Unknown,

        /// <summary>
        /// Represents a regular rank.
        /// </summary>
        Regular,

        /// <summary>
        /// Represents a rank to specify a user that has abnormal behaviour and has been reported once or more within the last week.
        /// </summary>
        Reported,

        /// <summary>
        /// Represents a rank to specify a user that uses the chat-functionality more often.
        /// </summary>
        Chatter,

        /// <summary>
        /// Represents a rank to specify a user that uses the voice-chat-functionality more often.
        /// </summary>
        Talkative,

        /// <summary>
        /// Represents a rank to specify a user that has been banned.
        /// </summary>
        Banned
    }
}