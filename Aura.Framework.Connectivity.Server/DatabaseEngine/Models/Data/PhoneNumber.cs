using Aura.Framework.Connectivity.Server.DatabaseEngine.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Aura.Framework.Connectivity.Server.DatabaseEngine.Models.Data
{
    /// <summary>
    /// Represents a phone number.
    /// </summary>
    public class PhoneNumber
    {
        #region Fields

        /// <summary>
        /// Represents the unique identifier of the current instance of the <see cref="PhoneNumber"/> class.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Represents a phone number.
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// Represents which phone-number type is specified.
        /// </summary>
        public Phone PhoneType { get; private set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumber"/> class.
        /// </summary>
        public PhoneNumber(string number, Phone phone)
        {
            Number = number;
            PhoneType = phone;
        }

        #endregion Constructors
    }
}