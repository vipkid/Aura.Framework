using System;
using System.Text;

namespace Aura.Framework.Core.Shared.Messages
{
    /// <summary>
    /// Represents a message that is received from the client.
    /// </summary>
    public class ClientMessage
    {
        #region Properties

        /// <summary>
        /// Represents the length of the packet.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Represents the current position of the packet.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Represents the packet in a <see cref="byte"/> array format.
        /// </summary>
        public byte[] Packet { get; private set; }

        /// <summary>
        /// Represents the current packet ID.
        /// </summary>
        public int PacketId { get; private set; }

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMessage"/> class.
        /// </summary>
        public ClientMessage(int length, byte[] packet)
        {
            // Set the length:
            Length = length;

            // Set the packet:
            Packet = packet;

            // Set the current (default) position:
            Position = 0;

            // Check if the Length meets the requirements:
            if (length < 2)
                throw new Exception("Packet is invalid.");

            // Read the packet id:
            PacketId = ReadInt16();
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            try
            {
                if (RemainingLength() < 4)
                    return 0;

                int position = BitConverter.ToInt32(Packet, Position);
                Position += 4;
                return position;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            try
            {
                if (RemainingLength() < 2)
                    return 0;

                short position = BitConverter.ToInt16(Packet, Position);
                Position += 2;
                return position;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Reads a System.Boolean value from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            try
            {
                byte[] istrue = ReadBytes(1);
                if (istrue.Length == 1) { return istrue[0] == 1; }
                else { return false; }
            }
            catch { return false; }
        }

        /// <summary>
        /// Reads a string from the current stream. The string is prefixed with the length, encoded as an integer seven bits at a time.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            int length = ReadInt32();
            byte[] stringBytes = ReadBytes(length);
            return Encoding.UTF8.GetString(stringBytes);
        }

        /// <summary>
        /// Returns the remaining length of the packet that is currently read
        /// </summary>
        public int RemainingLength()
        {
            return Length - Position;
        }

        /// <summary>
        /// Reads the specified number of bytes from the current stream into a byte array and advances the current position by that number of bytes.
        /// </summary>
        public byte[] ReadBytes(int length)
        {
            byte[] temp;

            int remaining = RemainingLength();

            if (RemainingLength() < length)
            {
                temp = new byte[remaining];
                Array.Copy(Packet, Position, temp, 0, remaining);
                Position += remaining;
                return temp;
            }
            else
            {
                temp = new byte[length];
                Array.Copy(Packet, Position, temp, 0, length);
                Position += length;

                return temp;
            }
        }

        /// <summary>
        /// Overrides the original ToString() method to make it (somewhat) readable.
        /// </summary>
        public override string ToString()
        {
            var stringValue = string.Empty;

            stringValue += Encoding.Default.GetString(Packet);

            for (var i = 0; i < 13; i++)
                stringValue = stringValue.Replace(char.ToString((char)(i)), $"[{i}]");

            return stringValue;
        }

        #endregion Public Methods
    }
}