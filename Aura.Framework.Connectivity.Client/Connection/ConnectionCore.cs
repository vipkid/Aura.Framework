using Aura.Framework.Connectivity.Client.Connection.Packets.Handlers.Outgoing;
using Aura.Framework.Connectivity.Shared.Messages;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Aura.Framework.Connectivity.Client.Connection
{
    /// <summary>
    /// Represents a connection core that actually connects itself to the server. It also can be disposed.
    /// </summary>
    public class ConnectionCore : IDisposable
    {
        #region Properties

        /// <summary>
        /// Represents the client manager of the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public ClientManager ClientManager { get; private set; }

        /// <summary>
        /// Represents the connection data of the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public ConnectionData ConnectionData { get; set; }

        /// <summary>
        /// Represents the connection id of the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public int ConnectionId { get; internal set; } = -1;

        /// <summary>
        /// Represents the <see cref="Socket"/> of the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Represents the buffer size of the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        private byte[] _Buffer;

        /// <summary>
        /// Represents the ip address of the current instance of the <see cref="ConnectionCore"/> class to connect with.
        /// </summary>
        public string IPAddress { get; private set; }

        /// <summary>
        /// Represents the port of the current instance of the <see cref="ConnectionCore"/> class to connect with.
        /// </summary>
        public int Port { get; private set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public ConnectionCore(string ip, int port, ConnectionData user, ClientManager clientManager)
        {
            // Set the client manager that is used in the current instance of the ConnectionCore class:
            ClientManager = clientManager;

            // Set the ConnectionData of the user:
            ConnectionData = user;

            // Set the Socket:
            Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            // Set the IP and Port:
            IPAddress = ip;
            Port = port;

            if (string.IsNullOrEmpty(ConnectionData.Username))
                ConnectionData.Username = Guid.NewGuid().ToString();
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Connects to the server based on the given ip and port.
        /// </summary>
        public void Connect()
        {
            // Set the IP & Port
            Socket.Connect(IPAddress, Port);

            // Allow the server to send packets
            BeginReceive();

            // Send a message to initialize the connection and to send system data
            SendMessage(new InitializeConnectionComposer(ConnectionData.Username));
            SendMessage(new ConnectionDataComposer(ConnectionData));
        }

        /// <summary>
        /// Handles any incoming data.
        /// </summary>
        public void HandleData(byte[] data)
        {
            // Setting the default position
            int position = 0;

            // Setting the default trycount
            int trycount = 0;

            while (position < data.Length && trycount++ <= 10)
            {
                int _length = BitConverter.ToInt32(data, position);

                if (_length > _Buffer.Length)
                {
                    // Return if the packet is too small
                    return;
                }

                // Create a byte array with the length of the incoming data
                byte[] clientpacket = new byte[_length];

                // Store the incoming data to the new byte array (clientpacket)
                Array.Copy(data, position + 4, clientpacket, 0, clientpacket.Length);

                // Create a ClientMessage to store the incoming data
                ClientMessage clientMessage = new ClientMessage(_length, clientpacket);

                // Handle the incoming packet
                ClientManager.PacketManager.HandlePacket(clientMessage, this);

                // Go to the next position of the incoming data
                position += 4 + _length;
            }
        }

        /// <summary>
        /// Sends a packet (<see cref="ServerMessage"/>) to the server.
        /// </summary>
        public bool SendMessage(ServerMessage message)
        {
            return SendData(message.ToBytes());
        }

        /// <summary>
        /// Sends data as <see cref="byte"/> format to the stream.
        /// </summary>
        public bool SendData(byte[] data)
        {
            try
            {
                if (Socket != null)
                {
                    Socket.Send(data);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (ex is ObjectDisposedException || ex is SocketException)
                    ClientManager.OnConnectionFailed(null);

                Console.WriteLine($"{ex}");

                return false;
            }
        }

        /// <summary>
        /// Disposes and closes the <see cref="Socket"/>.
        /// </summary>
        public void Dispose()
        {
            if (Socket != null)
            {
                Socket.Dispose();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Starts receiving packets (<see cref="ServerMessage"/>)'s again.
        /// </summary>
        private void BeginReceive()
        {
            try
            {
                // Maximum size of incoming data
                _Buffer = new byte[4096];

                // Begin receiving again
                Socket.BeginReceive(_Buffer, 0, _Buffer.Length, SocketFlags.None, new AsyncCallback(ReceivedData), null);
            }
            catch (Exception e)
            {
                // Close the socket (server cannot be reached)
                Dispose();

                // Write the error to the debug console
                Debug.WriteLine($"Failed to receive data from the server: {e}");

                // OnConnectionLost event
                ClientManager.OnConnectionFailed(null);
            }
        }

        /// <summary>
        /// Handles incming data sent by sockets.
        /// </summary>
        private void ReceivedData(IAsyncResult ar)
        {
            try
            {
                // Set the length to the length of the packet
                int length = Socket.EndReceive(ar);

                // Check if the packet contains data
                if (length <= 0)
                {
                    Dispose();
                    return;
                }

                // Create a new packet to store the incoming data
                byte[] packet = new byte[length];

                // Copy the incoming data to the packet byte array
                Array.Copy(_Buffer, packet, length);

                // Handle the packet byte array
                HandleData(packet);
            }
            catch (SocketException)
            {
                // Ignore, SocketException
            }
            catch (Exception e)
            {
                // If any other exception has been thrown, a messagebox will popup
                Debug.WriteLine(e.Message);
            }
            finally
            {
                // Receive again
                BeginReceive();
            }
        }

        #endregion Private Methods
    }
}