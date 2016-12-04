using NAudio.Wave;
using System;

namespace Aura.Framework.Audio.Interfaces
{
    /// <summary>
    /// Represents the network encoder and decoder interface.
    /// </summary>
    public interface INetworkAdapter : IDisposable
    {
        #region Fields

        /// <summary>
        /// Represents a friendly name for this codec.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Represents a <see cref="bool"/> value whether the codec is available on this system.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Represents the bitrate.
        /// </summary>
        int BitsPerSecond { get; }

        /// <summary>
        /// Represents the recording format, which is a "wave-format".
        /// </summary>
        WaveFormat RecordFormat { get; }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Encodes a block of audio.
        /// </summary>
        byte[] Encode(byte[] data, int offset, int length);

        /// <summary>
        /// Decodes a block of audio.
        /// </summary>
        byte[] Decode(byte[] data, int offset, int length);

        #endregion Methods
    }
}