using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Aura.Framework.Connectivity.Shared.Database.Models.Data
{
    /// <summary>
    /// Represents an email address.
    /// </summary>
    public class EmailAddress
    {
        #region Fields

        /// <summary>
        /// Represents the unique identifier of the current instance of the <see cref="EmailAddress"/> class.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Represents the complete email address.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Represents the name of the host.
        /// </summary>
        public string Host { get { return Address.Split('@').Last(); } }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress"/> class.
        /// </summary>
        public EmailAddress(string address)
        {
            Address = address;
        }

        #endregion Constructors
    }
}