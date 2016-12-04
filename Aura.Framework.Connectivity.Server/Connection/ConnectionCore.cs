using Aura.Framework.Connectivity.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Aura.Framework.Connectivity.Server.Connection
{
    /// <summary>
    /// Represents a connection.
    /// </summary>
    public class ConnectionCore : IDisposable
    {
        #region Properties

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the current instance of the <see cref="ConnectionCore"/> class is disposed.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Represents the buffer size of the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        private byte[] _Buffer;

        /// <summary>
        /// /// <summary>
        /// Represents a unique identifier of the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Represents the connection controller for the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public ConnectionController Controller { get; private set; }

        /// <summary>
        /// Represents basic connection information of the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public ConnectionData ConnectionData { get; private set; }

        /// <summary>
        /// Represents the server manager for the current instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public ServerManager ServerManager => Controller.ServerManager;

        /// <summary>
        /// Gets or sets the socket of the client.
        /// </summary>
        private Socket _Socket;

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionCore"/> class.
        /// </summary>
        public ConnectionCore(int id, Socket handler, ConnectionController controller)
        {
            // Set the ID
            ID = id;

            // Set the ConnectionController
            Controller = controller;

            // Set the Socket
            _Socket = handler;

            // Start receiving information
            BeginReceive();

            // Set the IP in the ConnectionInformation
            ConnectionData = new ConnectionData(_Socket.RemoteEndPoint.ToString());

            // Log everything
            ServerLogger.Connection($"Accepted connection: {id} > {ConnectionData.ClientIP.ToString().Replace("::ffff:", "")}");
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Handles the given data array.
        /// </summary>
        public void HandleData(byte[] data)
        {
            int pos = 0;
            int trycount = 0;
            while (pos < data.Length && trycount++ <= 10)
            {
                int length = BitConverter.ToInt32(data, pos);

                if (length > _Buffer.Length) { ServerLogger.Warning("Packet is to large!"); return; }

                byte[] clientpacket = new byte[length];
                Array.Copy(data, pos + 4, clientpacket, 0, clientpacket.Length);
                ClientMessage clientMessage = new ClientMessage(length, clientpacket);

                // Handle the packet
                ServerManager.PacketManager.HandlePacket(clientMessage, this);
                pos += 4 + length;
            }
        }

        /// <summary>
        /// Sends a packet to the specific client.
        /// </summary>
        public void SendMessage(ServerMessage message)
        {
            // Send the packet
            SendData(message.ToBytes());

            // Log the outgoing packet
            ServerLogger.Outgoing(string.Format("{0}, {1}", message.ID, message.ToString()));
        }

        /// <summary>
        /// Sends data to the stream using the socket.
        /// </summary>
        public void SendData(byte[] data)
        {
            try
            {
                if (Disposed)
                    return;
                _Socket.Send(data);
            }
            catch (Exception) { Dispose(); }
        }

        /// <summary>
        /// Disposes the socket and handles the disconnected user.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;
            Disposed = true;
            ServerLogger.Connection($"{ConnectionData.Username} disconnected from the server. Connection [{ID}] closed.");
            if (_Socket != null)
                _Socket.Dispose();
            HandleDisconnect();
        }

        /// <summary>
        /// Handles a disconnect such as removing the user from every chatroom, logging out, etc.
        /// </summary>
        public void HandleDisconnect()
        {
            ConnectionCore p;
            Controller.Connections.TryRemove(ID, out p);

            ServerManager.PrivateRoomManager.RemoveFromQueue(p);

            List<Chatrooms.Chatroom> ToLeave = new List<Chatrooms.Chatroom>();

            foreach (var room in ServerManager.ChatroomManager.Chatrooms)
            {
                Chatrooms.Chatroom chatroom = room.Value;

                if (room.Value.ChatroomUsers.Contains(p))
                    ToLeave.Add(chatroom);
            }

            foreach (var user in ToLeave)
            {
                user.Leave(p);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Receives data asynchronously.
        /// </summary>
        private void ReceivedData(IAsyncResult ar)
        {
            try
            {
                int length = _Socket.EndReceive(ar);
                if (length <= 0) { Dispose(); return; }

                byte[] packet = new byte[length];
                Array.Copy(_Buffer, packet, length);
                HandleData(packet);
                ServerLogger.Incoming(string.Format("Received data from {0} with data: {1}", ID, Encoding.UTF8.GetString(packet)));
            }
            catch (SocketException) { }
            catch (Exception e) { ServerLogger.Error(e.Message); }
            finally { BeginReceive(); }
        }

        /// <summary>
        /// Starts to receive data again.
        /// </summary>
        private void BeginReceive()
        {
            try
            {
                _Buffer = new byte[4096];
                _Socket.BeginReceive(_Buffer, 0, _Buffer.Length, SocketFlags.None, new AsyncCallback(ReceivedData), null);
            }
            catch (Exception) { Dispose(); }
        }

        #endregion Private Methods
    }
}