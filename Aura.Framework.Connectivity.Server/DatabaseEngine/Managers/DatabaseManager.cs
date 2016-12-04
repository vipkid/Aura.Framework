using System;

namespace Aura.Framework.Connectivity.Server.DatabaseEngine.Managers
{
    /// <summary>
    /// Provides functionality for using the database.
    /// </summary>
    public class DatabaseManager
    {
        #region Fields

        /// <summary>
        /// Represents the server manager.
        /// </summary>
        public ServerManager ServerManager { get; private set; }

        /// <summary>
        /// Represents database functionality for managing clients.
        /// </summary>
        public ClientManager ClientManager { get; private set; }

        #endregion Fields

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseManager"/> class.
        /// </summary>
        public DatabaseManager(ServerManager serverManager, bool loadDatabase = true)
        {
            if (!OSRequirements.Service("MSSQLSERVER"))
                return;

            ServerManager = serverManager;

            ClientManager = new ClientManager();

            if (loadDatabase) Load();
        }

        #endregion Contructors

        #region Methods

        /// <summary>
        /// Initializes the database or creates a new one if it does not exists already.
        /// </summary>
        internal bool Load()
        {
            try
            {
                using (AuraContext database = new AuraContext())
                {
                    bool cine = database.Database.CreateIfNotExists();

                    if (cine)
                        ServerLogger.Database("Creating database...");
                    else
                        ServerLogger.Database("Preparing database...");

                    return true;
                }
            }
            catch (Exception)
            {
                ServerLogger.Error("Failed to load the database!");
                return false;
            }
        }

        #endregion Methods
    }
}