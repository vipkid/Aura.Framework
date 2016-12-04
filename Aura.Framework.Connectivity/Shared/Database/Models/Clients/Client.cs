using Aura.Framework.Connectivity.Shared.Database.Enumerators;
using Aura.Framework.Connectivity.Shared.Database.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aura.Framework.Connectivity.Shared.Database.Models.Clients
{
    /// <summary>
    /// Represents a client implemented with the client interface.
    /// </summary>
    public class Client
    {
        #region Fields

        /// <summary>
        /// Represents the unique identifier of the client.
        /// </summary>
        [Key]
        public string GUID { get; set; }

        /// <summary>
        /// Represents the display name of the client.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Represents the full name of the client.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Represents the mood of the client.
        /// </summary>
        public string Mood { get; set; }

        /// <summary>
        /// Represents the password of the client.
        /// </summary>
        public string Password { get; set; }

        #endregion Fields

        #region Bans

        /// <summary>
        /// Represents a <see cref="bool"/> value whether the user is banned.
        /// </summary>
        [NotMapped]
        public bool IsBanned
        {
            get
            {
                foreach (var ban in Bans)
                {
                    if (ban.Expiration > DateTime.Today)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion Bans

        #region Enumerators

        /// <summary>
        /// Specifies the gender of the client.
        /// </summary>
        [NotMapped]
        public Gender GenderEnumerator { get; set; }

        /// <summary>
        /// Specifies the rank of the client.
        /// </summary>
        [NotMapped]
        public Rank RankEnumerator { get; set; }

        /// <summary>
        /// Specifies the status of the client.
        /// </summary>
        [NotMapped]
        public Status StatusEnumerator { get; set; }

        /// <summary>
        /// Specifies the role of the client.
        /// </summary>
        [NotMapped]
        public Role RoleEnumerator { get; set; }

        /// <summary>
        /// Specifies the gender of the client.
        /// </summary>
        [Required]
        public int Gender { get { return (int)GenderEnumerator; } set { GenderEnumerator = (Gender)value; } }

        /// <summary>
        /// Specifies the rank of the client.
        /// </summary>
        [Required]
        public int Rank { get { return (int)RankEnumerator; } set { RankEnumerator = (Rank)value; } }

        /// <summary>
        /// Specifies the status of the client.
        /// </summary>
        [Required]
        public int Status { get { return (int)StatusEnumerator; } set { StatusEnumerator = (Status)value; } }

        /// <summary>
        /// Specifies the role of the client.
        /// </summary>
        [Required]
        public int Role { get { return (int)RoleEnumerator; } set { RoleEnumerator = (Role)value; } }

        #endregion Enumerators

        #region Collections

        /// <summary>
        /// Represents a collection of contacts of the client.
        /// </summary>
        public ICollection<Client> Contacts { get; set; }

        /// <summary>
        /// Represents a collection of achievements of the client.
        /// </summary>
        public ICollection<Achievement> Achievements { get; set; }

        /// <summary>
        /// Represents a collection of email addresses of the client.
        /// </summary>
        public ICollection<EmailAddress> EmailAddresses { get; set; }

        /// <summary>
        /// Represents a collection of bans of the client.
        /// </summary>
        public ICollection<ClientBan> Bans { get; set; }

        /// <summary>
        /// Represents a collection of phone numbers of the client.
        /// </summary>
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }

        /// <summary>
        /// Represents a collection of hobbies of the client.
        /// </summary>
        public ICollection<Hobby> Hobbies { get; set; }

        #endregion Collections

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Bans the user with a specific ban object.
        /// </summary>
        public void Ban(ClientBan ban)
        {
            Bans.Add(ban);
        }

        /// <summary>
        /// Unbans the user and sets the expiration date to today.
        /// </summary>
        public void Unban()
        {
            if (Bans.Count > 0)
                foreach (var ban in Bans)
                {
                    ban.Expiration = DateTime.Today;
                }
        }

        #endregion Methods
    }
}