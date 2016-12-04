namespace Aura.Framework.Connectivity.Server.DatabaseEngine.Enumerators
{
    /// <summary>
    /// Represents an enumerator to specify the login or register state of a user.
    /// </summary>
    public enum States
    {
        /// <summary>
        /// Represents an unknown status.
        /// </summary>
        Unknown,

        /// <summary>
        /// Represents the correct login status.
        /// </summary>
        LoginCorrect,

        /// <summary>
        /// Represents the wrong login status.
        /// </summary>
        LoginWrong,

        /// <summary>
        /// Represents the error status when something happened at our end.
        /// </summary>
        LoginError,

        /// <summary>
        /// Represents the nonexistent login status.
        /// </summary>
        LoginNonexistent,

        /// <summary>
        /// Represents the correct register status.
        /// </summary>
        RegisterCorrect,

        /// <summary>
        /// Represents the failed register status.
        /// </summary>
        RegisterFailed,

        /// <summary>
        /// Represents the already-taken register status.
        /// </summary>
        RegisterAlreadyTaken,
    }
}