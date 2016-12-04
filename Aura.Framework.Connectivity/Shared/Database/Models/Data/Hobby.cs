using Aura.Framework.Connectivity.Shared.Database.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Aura.Framework.Connectivity.Shared.Database.Models.Data
{
    /// <summary>
    /// Represents a hobby.
    /// </summary>
    public class Hobby
    {
        #region Fields

        /// <summary>
        /// Represents the unique identifier of the current instance of the <see cref="Hobby"/> class.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Represents the name of the hobby.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Specifies how often the hobby is done.
        /// </summary>
        public Frequently Frequently { get; private set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Hobby"/> class.
        /// </summary>
        public Hobby(string name, Frequently frequently)
        {
            Name = name;
            Frequently = frequently;
        }

        #endregion Constructors
    }
}