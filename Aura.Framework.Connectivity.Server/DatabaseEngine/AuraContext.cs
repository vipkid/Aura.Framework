using Aura.Framework.Connectivity.Server.DatabaseEngine.Models.Clients;
using System.Data.Entity;

namespace Aura.Framework.Connectivity.Server.DatabaseEngine
{
    /// <summary>
    /// Represents the database context.
    /// </summary>
    public class AuraContext : DbContext
    {
        #region Database Context

        /// <summary>
        /// Represents a dataset of clients.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        #endregion Database Context

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuraContext"/> class.
        /// </summary>
        public AuraContext() : base("AuraContext")
        {
        }

        #endregion Constructors
    }
}