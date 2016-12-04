using Aura.Framework.Connectivity.Server.Chatrooms;
using Aura.Framework.Connectivity.Server.Connection;
using Aura.Framework.Connectivity.Server.Connection.Packets;
using Aura.Framework.Connectivity.Server.DatabaseEngine.Managers;

namespace Aura.Framework.Connectivity.Server
{
    /// <summary>
    /// Represents the server manager.
    /// </summary>
    public class ServerManager
    {
        #region Properties

        /// <summary>
        /// Gets or sets a <see cref="bool"/> whether the server is running.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Gets or sets the packet manager for the current instance of the <see cref="ServerManager"/> class.
        /// </summary>
        public PacketManager PacketManager { get; private set; }

        /// <summary>
        /// Gets or sets the connection controller for the current instance of the <see cref="ServerManager"/> class.
        /// </summary>
        public ConnectionController ConnectionController { get; private set; }

        /// <summary>
        /// Gets or sets the chatroom manager for the current instance of the <see cref="ServerManager"/> class.
        /// </summary>
        public ChatroomManager ChatroomManager { get; private set; }

        /// <summary>
        /// Gets or sets the private chatroom manager for the current instance of the <see cref="ServerManager"/> class.
        /// </summary>
        public PrivateRoomManager PrivateRoomManager { get; private set; }

        /// <summary>
        /// Gets or sets the database manager for the current instance of the <see cref="ServerManager"/> class.
        /// </summary>
        public DatabaseManager DatabaseManager { get; private set; }

        #endregion Properties

        #region Constructors & Decontructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerManager"/> class.
        /// </summary>
        public ServerManager(int port)
        {
            PacketManager = new PacketManager();
            ConnectionController = new ConnectionController(port, this);
            ChatroomManager = new ChatroomManager(this);
            PrivateRoomManager = new PrivateRoomManager(this);
            DatabaseManager = new DatabaseManager(this, false);
        }

        #endregion Constructors & Decontructors

        #region Public Methods

        /// <summary>
        /// Starts the server.
        /// </summary>
        public async void Start()
        {
            ServerLogger.Info("Preparing and initializing the server...");
            ServerLogger.Info($"The server is hosted on { await OSRequirements.GetIPAddress()}.");

            ConnectionController.Start();
            IsRunning = true;
        }

        #endregion Public Methods
    }
}