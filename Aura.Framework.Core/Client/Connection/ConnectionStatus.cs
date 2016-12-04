using Aura.Framework.Core.Client.Connection.Packets.Handlers.Outgoing;
using System.Timers;

namespace Aura.Framework.Core.Client.Connection
{
    /// <summary>
    /// Represents a class that controls the keep-alive packet system.
    /// </summary>
    public class ConnectionStatus
    {
        #region Properties

        /// <summary>
        /// Represents the timer that sends the keep-alive packet.
        /// </summary>
        public Timer ConnectionChecker { get; private set; }

        /// <summary>
        /// Represents the amount of keep-alive packets that have been sent.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Represents the client manager that is used in the current instance of the <see cref="ConnectionStatus"/> class.
        /// </summary>
        public ClientManager ClientManager { get; private set; }

        /// <summary>
        /// Returns a <see cref="bool"/> value if the <see cref="ConnectionChecker"/> timer is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStatus"/> class.
        /// </summary>
        public ConnectionStatus(ClientManager clientManager)
        {
            // Set the client manager:
            ClientManager = clientManager;

            // Initialize a new instance of the Timer class:
            ConnectionChecker = new Timer();

            // Modify the properties of the ConnectionChecker timer:
            ConnectionChecker.Interval = 10000;
            ConnectionChecker.Enabled = false;

            // Subscribe to the elapsed event:
            ConnectionChecker.Elapsed += SendPacket;
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Starts the the <see cref="ConnectionChecker"/> timer.
        /// </summary>
        public void Start()
        {
            ConnectionChecker.Start(); IsRunning = true;
        }

        /// <summary>
        /// Stops the <see cref="ConnectionChecker"/> timer.
        /// </summary>
        public void Stop()
        {
            ConnectionChecker.Stop(); IsRunning = false;
        }

        /// <summary>
        /// Handles the <see cref="ConnectionChecker.Elapsed"/> event and sends a keep-alive packet.
        /// </summary>
        public void SendPacket(object sender, ElapsedEventArgs e)
        {
            // Create the packet:
            ClientManager.ConnectionCore.SendMessage(new KeepAlivePacketComposer());

            // Count one up:
            Count++;
        }

        #endregion Public Methods
    }
}