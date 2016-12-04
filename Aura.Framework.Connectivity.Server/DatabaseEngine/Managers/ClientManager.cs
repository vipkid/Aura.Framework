using Aura.Framework.Connectivity.Shared.Database.Enumerators;
using Aura.Framework.Connectivity.Shared.Database.Models.Clients;
using System;
using System.Data.Entity;
using System.Linq;

namespace Aura.Framework.Connectivity.Server.DatabaseEngine.Managers
{
    /// <summary>
    /// Provides functionality for modifying, updating, creating and checking functionality for client-information-related stuff.
    /// </summary>
    public class ClientManager
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientManager"/> class.
        /// </summary>
        public ClientManager()
        {
        }

        #endregion Constructors

        #region Login Methods

        /// <summary>
        /// Updates the status from a user to the status that the user has set while being online, if the login succeeds based on username and password combinations.
        /// </summary>
        public States Login(string guid, string password, Status status)
        {
            try
            {
                using (AuraContext database = new AuraContext())
                {
                    var user = database.Clients.Where(client => client.GUID == guid).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.Password == password)
                        {
                            user.StatusEnumerator = status;

                            database.SaveChanges();

                            return States.LoginCorrect;
                        }
                        return States.LoginWrong;
                    }
                    return States.LoginNonexistent;
                }
            }
            catch (Exception)
            {
                return States.LoginError;
            }
        }

        /// <summary>
        /// Updates the status from a user to 'offline'.
        /// </summary>
        public void Logout(string guid)
        {
            using (AuraContext database = new AuraContext())
            {
                var user = database.Clients.FirstOrDefault(client => client.GUID == guid);

                if (user != null)
                { user.StatusEnumerator = Status.Offline; database.SaveChanges(); }
            }
        }

        #endregion Login Methods

        #region Register Methods

        /// <summary>
        /// Registers and adds a new client to the database.
        /// </summary>
        public States Register(string username, string password)
        {
            try
            {
                using (AuraContext database = new AuraContext())
                {
                    var user = database.Clients.FirstOrDefault(client => client.FullName == username);

                    if (user != null)
                    {
                        try
                        {
                            database.Clients.Add(new Client()
                            {
                                FullName = username,
                                Password = password
                            });

                            database.SaveChanges();

                            return States.RegisterCorrect;
                        }
                        catch (Exception e)
                        {
                            ServerLogger.Error(e);
                            return States.RegisterFailed;
                        }
                    }
                    else
                    {
                        return States.RegisterAlreadyTaken;
                    }
                }
            }
            catch (Exception e)
            {
                ServerLogger.Error(e);
                return States.RegisterFailed;
            }
        }

        /// <summary>
        /// Removes a client based on the GUID property and .
        /// </summary>
        public void UnRegister(string guid, string password)
        {
            using (AuraContext database = new AuraContext())
            {
                var user = database.Clients.FirstOrDefault(client => client.GUID == guid);

                if (user != null)
                {
                    if (user.Password == password)
                    {
                        ServerLogger.Warning($"Removing a user: {user.FullName} ({user.GUID})");
                        database.Clients.Remove(user);

                        ServerLogger.Important($"Removed {user.FullName} from Aura.");
                        database.SaveChanges();
                    }
                }
            }
        }

        #endregion Register Methods

        #region Methods

        /// <summary>
        /// Adds an instance of the <see cref="Client"/> class to the database.
        /// </summary>
        public async void Add(Client client)
        {
            using (AuraContext database = new AuraContext())
            {
                database.Clients.Add(client);

                await database.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes an instance of the <see cref="Client"/> class to the database.
        /// </summary>
        public async void Remove(Client client)
        {
            using (AuraContext database = new AuraContext())
            {
                database.Clients.Remove(client);

                await database.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes an instance of the <see cref="Client"/> class to the database.
        /// </summary>
        public async void Remove(string guid)
        {
            using (AuraContext database = new AuraContext())
            {
                var user = database.Clients.FirstOrDefault(o => o.GUID == guid);
                if (user != null)
                    database.Clients.Remove(user);

                await database.SaveChangesAsync();
            }
        }

        ///<summary>
        /// Returns a <see cref="bool"/> value whether the client exists in the database.
        ///</summary>
        public bool Exists(Client client)
        {
            using (AuraContext database = new AuraContext())
            {
                return database.Clients.Contains(client);
            }
        }

        ///<summary>
        /// Returns a <see cref="bool"/> value whether the GUID of the client exists in the database.
        ///</summary>
        public bool Exists(string guid)
        {
            using (AuraContext database = new AuraContext())
            {
                return database.Clients.Any(o => o.GUID == guid);
            }
        }

        ///<summary>
        /// Returns a <see cref="bool"/> value whether the instance of the <see cref="Client"/> class is banned.
        ///</summary>
        public bool IsBanned(Client client)
        {
            using (AuraContext database = new AuraContext())
            {
                return database.Clients.FirstOrDefault(o => o == client).IsBanned;
            }
        }

        ///<summary>
        /// Returns a <see cref="bool"/> value whether the instance of the <see cref="Client"/> class is banned based on the GUID property.
        ///</summary>
        public bool IsBanned(string guid)
        {
            using (AuraContext database = new AuraContext())
            {
                return database.Clients.FirstOrDefault(o => o.GUID == guid).IsBanned;
            }
        }

        /// <summary>
        /// Gets a client instance based on the GUID property.
        /// </summary>
        public Client Get(string guid)
        {
            using (AuraContext database = new AuraContext())
            {
                return database.Clients.Where(o => o.GUID == guid).FirstOrDefault();
            }
        }

        /// <summary>
        /// Bans a user based on their GUID.
        /// </summary>
        public void Ban(string guid, ClientBan ban)
        {
            using (AuraContext database = new AuraContext())
            {
                var user = database.Clients.Where(client => client.GUID == guid).FirstOrDefault();

                if (user != null)
                    user.Ban(ban);

                database.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a client with the modified data.
        /// </summary>
        public void Update(Client client)
        {
            using (AuraContext database = new AuraContext())
            {
                database.Entry(client).State = EntityState.Modified;
                database.SaveChanges();
            }
        }

        /// <summary>
        /// Bans a user based on their GUID.
        /// </summary>
        public void Unban(string guid)
        {
            using (AuraContext database = new AuraContext())
            {
                var user = database.Clients.Where(client => client.GUID == guid).FirstOrDefault();

                if (user != null)
                    user.Unban();

                database.SaveChanges();
            }
        }

        #endregion Methods
    }
}