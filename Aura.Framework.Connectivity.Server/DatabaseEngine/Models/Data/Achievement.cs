using System.ComponentModel.DataAnnotations;

namespace Aura.Framework.Connectivity.Server.DatabaseEngine.Models.Data
{
    /// <summary>
    /// Represents an achievement.
    /// </summary>
    public class Achievement
    {
        #region Fields

        /// <summary>
        /// Represents the unique identifier of the current instance of the <see cref="Achievement"/> class.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Represents a bool value whether the current instance of the <see cref="Achievement"/> class is unlocked.
        /// </summary>
        public bool IsUnlocked { get; private set; }

        /// <summary>
        /// Represents the name of the current instance of the <see cref="Achievement"/> class.
        /// </summary>
        public string Name { get; private set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Achievement"/> class.
        /// </summary>
        public Achievement(string name, bool isUnlocked)
        {
            Name = name;
            IsUnlocked = isUnlocked;
        }

        #endregion Constructors
    }
}