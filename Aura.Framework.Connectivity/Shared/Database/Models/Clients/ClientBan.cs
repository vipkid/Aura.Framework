using System;

namespace Aura.Framework.Connectivity.Shared.Database.Models.Clients
{
    /// <summary>
    /// Represents a user ban.
    /// </summary>
    public class ClientBan
    {
        #region Fields

        /// <summary>
        /// Represents the date that the user has been banned.
        /// </summary>
        public DateTime? DateBanned { get; set; }

        /// <summary>
        /// Represents the date that the user gets unbanned.
        /// </summary>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// Represents the user/client (GUID) that has been banned.
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Represents the reason why the user has been banned.
        /// </summary>
        public string Reason { get; set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBan"/> class.
        /// </summary>
        public ClientBan(Client client, DateTime? unbanDate = null, string reason = null)
        {
            if (Client == null)
                return;

            Client = client.GUID;

            Expiration = (unbanDate != null) ? unbanDate : new DateTime(9999, 1, 1);
            Reason = (reason != null) ? reason : string.Empty;

            // If the unban date is not set, it will be at the 1st of January in 9999.
        }

        #endregion Constructors
    }
}