namespace Aura.Framework.Enumerators
{
    /// <summary>
    /// Represents an enumerator to specify the role of a user.
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Represents an unregistered user.
        /// </summary>
        Guest,

        /// <summary>
        /// Represents a registered user.
        /// </summary>
        Member,

        /// <summary>
        /// Represents a moderator.
        /// </summary>
        Moderator,

        /// <summary>
        /// Represents an administrator.
        /// </summary>
        Admin,

        /// <summary>
        /// Represents a developer.
        /// </summary>
        Developer
    }
}