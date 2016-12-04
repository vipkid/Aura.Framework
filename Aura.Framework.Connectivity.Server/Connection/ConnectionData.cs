using Aura.Framework.Connectivity.Server.Models;
using Aura.Framework.Connectivity.Shared.Database.Models.Clients;
using System;
using System.Collections.Generic;
using System.Net;

namespace Aura.Framework.Connectivity.Server.Connection
{
    /// <summary>
    /// Provides data and information about the connection.
    /// </summary>
    public class ConnectionData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the IP of the client.
        /// </summary>
        public IPAddress ClientIP { get; set; }

        /// <summary>
        /// Gets or sets the username of the client.
        /// </summary>
        public string Username { get; set; } = new Guid().ToString();

        /// <summary>
        /// Gets or sets a list with system information used for error fixing and statistics.
        /// </summary>
        public List<DataPackage> UserData { get; set; } = new List<DataPackage>();

        /// <summary>
        /// Gets or sets the client data.
        /// </summary>
        public Client ClientData { get; set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionData"/> class.
        /// </summary>
        public ConnectionData(string ip)
        {
            // Set the ClientIP
            ClientIP = IPAddress.Parse(ip);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionData"/> class.
        /// </summary>
        public ConnectionData(string ip, Client clienData)
        {
            // Set the ClientIP
            ClientIP = IPAddress.Parse(ip);

            // Set the client data
            ClientData = clienData;
        }

        #endregion Constructors & Deconstructors
    }
}