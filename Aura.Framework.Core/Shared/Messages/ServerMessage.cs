using System;
using System.Collections.Generic;
using System.Text;

namespace Aura.Framework.Core.Shared.Messages
{
    /// <summary>
    /// Represents a message that is received from the server.
    /// </summary>
    public class ServerMessage
    {
        #region Properties

        /// <summary>
        /// Sets an instance of a byte list.
        /// </summary>
        private List<byte> _Builder;

        /// <summary>
        /// Sets an instance of a byte list.
        /// </summary>
        private List<byte> _Final;

        /// <summary>
        /// Gets the ID of the packet which has a default value of -1.
        /// </summary>
        public int ID { get; private set; } = -1;

        #endregion Properties

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMessage"/> class combined with the packet-id property.
        /// </summary>
        public ServerMessage(int id)
        {
            _Builder = new List<byte>();
            _Final = new List<byte>();
            AppendInt16((short)id);

            ID = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMessage"/> class.
        /// </summary>
        public ServerMessage()
        {
            _Builder = new List<byte>();
            _Final = new List<byte>();
        }

        #endregion Constructors & Deconstructors

        #region Public Methods

        /// <summary>
        /// Appends the string representation of a specified object to this instance.
        /// </summary>
        public void Write(object o)
        {
            _Builder.AddRange(Encoding.UTF8.GetBytes(o.ToString()));
        }

        /// <summary>
        /// Appends the string representation of a specified Char object to this instance.
        /// </summary>
        public void WriteChar(int i)
        {
            Write(Convert.ToChar(i));
        }

        /// <summary>
        /// Appends the string representation of a specified 8-bit unsigned integer to this instance.
        /// </summary>
        public void AppendByte(byte b)
        {
            _Builder.Add(b);
        }

        /// <summary>
        /// Writes a string to the current stream.
        /// </summary>
        public void WriteString(string s)
        {
            WriteInt(Encoding.UTF8.GetByteCount(s));
            Write(s);
        }

        /// <summary>
        /// Writes the byte array to the current stream.
        /// </summary>
        public void WriteBytes(byte[] b)
        {
            _Builder.AddRange(b);
        }

        /// <summary>
        /// Writes an integer to the current stream.
        /// </summary>
        public void WriteInt(int i)
        {
            byte[] b = BitConverter.GetBytes(i);
            _Builder.AddRange(b);
        }

        /// <summary>
        /// Appends the string representation of a specified 16-bit signed integer to this instance.
        /// </summary>
        public void AppendInt16(short i)
        {
            byte[] b = BitConverter.GetBytes(i);
            _Builder.AddRange(b);
        }

        /// <summary>
        /// Appends the string representation of a specified <see cref="bool"/> value to this instance.
        /// </summary>
        public void AppendBoolean(bool b)
        {
            _Builder.Add(b ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// Converts the current containing bytes in the builder list into a byte array.
        /// </summary>
        public List<byte> CurrentToBytes()
        {
            List<byte> sendMessage = new List<byte>();

            sendMessage.AddRange(BitConverter.GetBytes(_Builder.Count));
            sendMessage.AddRange(_Builder);
            return sendMessage;
        }

        /// <summary>
        /// Inititializes a new packet.
        /// </summary>
        public void NewPacket(int id)
        {
            if (_Builder.Count > 0)
                _Final.AddRange(CurrentToBytes());

            _Builder.Clear();
            AppendInt16((short)id);
        }

        /// <summary>
        /// Converts the builder list into a byte array.
        /// </summary>
        public byte[] ToBytes()
        {
            if (_Builder.Count > 0)
                _Final.AddRange(CurrentToBytes());

            _Builder.Clear();
            return _Final.ToArray();
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value if the builder byte list contains more than 2 bytes.
        /// </summary>
        public bool HasContent()
        {
            return _Builder.Count > 2;
        }

        /// <summary>
        /// Overrides the original ToString() method to convert a packet to a (somewhat) readable string.
        /// </summary>
        public override string ToString()
        {
            var stringValue = string.Empty;

            stringValue += Encoding.Default.GetString(_Final.ToArray());

            for (var i = 0; i < 13; i++)
            {
                stringValue = stringValue.Replace(char.ToString((char)(i)), $"[{i}]");
            }

            return stringValue;
        }

        #endregion Public Methods
    }
}