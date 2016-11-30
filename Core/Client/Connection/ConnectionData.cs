namespace Aura.Framework.Core.Client.Connection
{
    /// <summary>
    /// Provides basic information of the user.
    /// </summary>
    public class ConnectionData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }

        #endregion Properties

        #region Constructors & Decontructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionData"/> class.
        /// </summary>
        public ConnectionData(string name, string password)
        {
            Username = name;
            Password = password;
        }

        #endregion Constructors & Decontructors
    }
}