using Aura.Framework.Connectivity.Shared.Messages;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Aura.Framework.Connectivity.Server.Connection
{
    /// <summary>
    /// Represents a class that handles the connections.
    /// </summary>
    public class ConnectionController : IDisposable
    {
        #region Properties

        /// <summary>
        /// Used for managing client ids.
        /// </summary>
        public int LastClientID { get; private set; }

        /// <summary>
        /// Represents the port of the current instance of the connection controller class.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Represents the server socket.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Represents a concurrent dictionary containing the current connections.
        /// </summary>
        public ConcurrentDictionary<int, ConnectionCore> Connections { get; private set; }

        /// <summary>
        /// Determines if the connection controller is running or is connected.
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Represents the server manager of the current instance of the connection controller class.
        /// </summary>
        public ServerManager ServerManager { get; private set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionController"/> class.
        /// </summary>
        public ConnectionController(int port, ServerManager serverManager)
        {
            // Set the ServerManager
            ServerManager = serverManager;

            // Set the default value of LastClientID
            LastClientID = 0;

            // Set the port
            Port = port;

            // Set the connected value to false (default)
            Connected = false;

            // Set the Connections List
            Connections = new ConcurrentDictionary<int, ConnectionCore>();

            // Set the Server Socket
            Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Sends a server message (packet) to the each connected client.
        /// </summary>
        public void SendMessage(ServerMessage packet)
        {
            // Copy the list
            var cons = Connections.ToList();

            foreach (var connection in cons)
            {
                // Send the packet
                connection.Value.SendMessage(packet);
            }
        }

        /// <summary>
        /// Starts the connection controller.
        /// </summary>
        public void Start()
        {
            // Log the server information
            ServerLogger.Info($"Server started on port {Port}");
            IPEndPoint endPort = new IPEndPoint(IPAddress.Any, Port);

            // Bind the ServerSocket
            Socket.Bind(endPort);

            Connected = true;

            // Start receiving packets
            Socket.Listen(0);

            // Start accepting clients
            Socket.BeginAccept(new AsyncCallback(AcceptConnection), Socket);
        }

        /// <summary>
        /// Returns a connection based on the id, otherwise it returns a null object.
        /// </summary>
        public ConnectionCore GetConnectionByID(int id)
        {
            if (Connections.ContainsKey(id))
            {
                ConnectionCore connection = Connections[id];
                return connection;
            }
            return null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Accepts a connection.
        /// </summary>
        private void AcceptConnection(IAsyncResult ar)
        {
            try
            {
                Socket handler = Socket.EndAccept(ar);
                ConnectionCore connection = new ConnectionCore(LastClientID++, handler, this);
                Connections.TryAdd(connection.ID, connection);
            }
            catch (Exception e)
            {
                ServerLogger.Error(e.Message);
            }
            finally
            {
                if (Connected)
                    Socket.BeginAccept(AcceptConnection, null);
            }
        }

        #endregion Private Methods
    }
}