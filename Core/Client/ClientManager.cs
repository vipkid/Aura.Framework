using Aura.Framework.Core.Client.Chatrooms;
using Aura.Framework.Core.Client.Connection;
using Aura.Framework.Core.Client.Connection.Packets;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aura.Framework.Core.Client
{
    /// <summary>
    /// Represents a client manager to manage all the client related functionality.
    /// </summary>
    public class ClientManager
    {
        #region Properties

        /// <summary>
        /// Returns a <see cref="bool"/> value if the <see cref="ClientManager"/> is running.
        /// </summary>
        public bool Running { get; set; } = false;

        /// <summary>
        /// Represents the packet manager used in the current instance of the <see cref="ClientManager"/> class.
        /// </summary>
        public PacketManager PacketManager { get; private set; }

        /// <summary>
        /// Represents the connection core class used in the current instance of the <see cref="ClientManager"/> class.
        /// </summary>
        public ConnectionCore ConnectionCore { get; private set; }

        /// <summary>
        /// Represents the connection status class (keep-alive packet) used in the current instance of the <see cref="ClientManager"/> class.
        /// </summary>
        public ConnectionStatus ConnectionStatus { get; private set; }

        /// <summary>
        /// Represents the connection information/data used in the current instance of the <see cref="ClientManager"/> class.
        /// </summary>
        public ConnectionData ConnectionData { get; private set; }

        /// <summary>
        /// Contains the port that the current instance of the <see cref="ClientManager"/> class is connecting too or connected with.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Contains the ip address that the current instance of the <see cref="ClientManager"/> class is connecting too or connected with.
        /// </summary>
        public string IPAddress { get; private set; }

        /// <summary>
        /// Represents the chatroom mamanger used in the current instance of the <see cref="ClientManager"/> class.
        /// </summary>
        public ChatroomManager ChatroomManager { get; private set; }

        #endregion Properties

        #region Events

        /// <summary>
        /// An event that occurs when the client has succesfully connected to the server.
        /// </summary>
        public event EventHandler ConnectionSucceed;

        /// <summary>
        /// An event that occurs when the client has lost the connection to the server.
        /// </summary>
        public event EventHandler ConnectionFailed;

        /// <summary>
        /// An event that occurs when the client is reconnecting.
        /// </summary>
        public event EventHandler Reconnecting;

        #endregion Events

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientManager"/> class.
        /// </summary>
        public ClientManager(string ip, int port)
        {
            // Check if the framework is compatible:
            if (!OSRequirements.Framework())
                throw new PlatformNotSupportedException("The required version of .NET Framework is not installed on this machine.");

            // Set the connection status instance:
            ConnectionStatus = new ConnectionStatus(this);

            // Set the chatroom manager instance:
            ChatroomManager = new ChatroomManager(this);

            // Set the packet manager instance:
            PacketManager = new PacketManager();

            // Set the ip address and port for the client manager:
            IPAddress = ip;
            Port = port;

            // Subscribe to the 'connection succeed' event
            ConnectionSucceed += ClientManager_ConnectionSucceed;

            // Subscribe to the 'connection failed' event
            ConnectionFailed += ClientManager_ConnectionFailed;
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Tries to connect to the server asynchronously based on the given ip address and port.
        /// </summary>
        public async Task ConnectAsync(ConnectionData user)
        {
            await Task.Run(() => Connect(user)).ConfigureAwait(false);
        }

        /// <summary>
        /// Tries to connect to the server based on the given ip address and port.
        /// </summary>
        public void Connect(ConnectionData user)
        {
            try
            {
                ConnectionData = user;
                ConnectionCore = new ConnectionCore(IPAddress, Port, ConnectionData, this);
                ConnectionCore.Connect();
                Console.WriteLine("Starting the Client...");
                OnConnected(null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

                Console.WriteLine("Failed to connect to the server...");

                OnConnectionFailed(null);
                Stop();
            }
        }

        /// <summary>
        /// Disposes the <see cref="ConnectionCore"/> and stops the keep-alive packet from sending.
        /// </summary>
        public void Stop()
        {
            ConnectionCore.Dispose();
            ConnectionStatus.Stop();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Handles and fires the <see cref="OnConnected(EventArgs)"/> event.
        /// </summary>
        protected virtual void OnConnected(EventArgs e)
        {
            ConnectionSucceed?.Invoke(this, e);
            Running = true;
            Console.WriteLine("Succesfully connected.");
        }

        /// <summary>
        /// Handles and fires the <see cref="OnConnectionFailed(EventArgs)"/> event.
        /// </summary>
        internal void OnConnectionFailed(EventArgs e)
        {
            ConnectionFailed?.Invoke(this, e);
            Running = false;
            Console.WriteLine("Connection lost.");
            Console.WriteLine("Reconneting...");
            OnReconnecting(null);
            Connect(ConnectionData);
        }

        /// <summary>
        /// Handles and fires the <see cref="OnReconnecting(EventArgs)"/> event.
        /// </summary>
        protected virtual void OnReconnecting(EventArgs e)
        {
            Reconnecting?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the <see cref="ConnectionFailed"/> event.
        /// </summary>
        private void ClientManager_ConnectionFailed(object sender, EventArgs e)
        {
            if (ConnectionStatus.IsRunning)
                ConnectionStatus.Stop();
        }

        /// <summary>
        /// Handles the <see cref="ConnectionSucceed"/> event.
        /// </summary>
        private void ClientManager_ConnectionSucceed(object sender, EventArgs e)
        {
            // Initializes a new ConnectionStatus (keep-alive packet):
            ConnectionStatus = new ConnectionStatus(this);

            // Start the keep-alive packet:
            ConnectionStatus.Start();
        }

        #endregion Private Methods
    }
}