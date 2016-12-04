using Aura.Framework.Audio.Interfaces;
using NAudio.Wave;
using NSpeex;
using System;
using System.ComponentModel.Composition;

namespace Aura.Framework.Audio.Transcoding
{
    #region Bitrates

    /// <summary>
    /// Represents a bandwidth of 8kHz. (Speex Narrow Band)
    /// </summary>
    [Export(typeof(INetworkAdapter))]
    public class NarrowBandSpeex : Speex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NarrowBandSpeex"/> class.
        /// </summary>
        public NarrowBandSpeex() : base(BandMode.Narrow, 8000, "Speex Narrow Band (8kHz)") { }
    }

    /// <summary>
    /// Represents a bandwidth of 16kHz. (Speex Wide Band)
    /// </summary>
    [Export(typeof(INetworkAdapter))]
    public class WideBandSpeex : Speex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WideBandSpeex"/> class.
        /// </summary>
        public WideBandSpeex() : base(BandMode.Wide, 16000, "Speex Wide Band (16kHz)") { }
    }

    /// <summary>
    /// Represents a bandwidth of 32kHz. (Speex Ultra Wide Band)
    /// </summary>
    [Export(typeof(INetworkAdapter))]
    public class UltraBandSpeex : Speex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltraBandSpeex"/> class.
        /// </summary>
        public UltraBandSpeex() : base(BandMode.UltraWide, 32000, "Speex Ultra Wide Band (32kHz)") { }
    }

    /// <summary>
    /// Represents a bandwidth of 64kHz. (Speex Advanced Wide Band)
    /// </summary>
    [Export(typeof(INetworkAdapter))]
    public class AdvancedBandSpeex : Speex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedBandSpeex"/> class.
        /// </summary>
        public AdvancedBandSpeex() : base(BandMode.UltraWide, 64000, "Speex Advanced Band (64kHz)") { }
    }

    #endregion Bitrates

    /// <summary>
    /// Represents the so called 'Speex' voice chat transcoder.
    /// </summary>
    public class Speex : INetworkAdapter
    {
        #region Fields

        /// <summary>
        /// Represents the recording format, which is a "wave-format".
        /// </summary>
        public WaveFormat RecordingFormat { get; private set; }

        /// <summary>
        /// Represents the speex-decoder.
        /// </summary>
        public SpeexDecoder Decoder { get; private set; }

        /// <summary>
        /// Represents the speex-encoder.
        /// </summary>
        public SpeexEncoder Encoder { get; private set; }

        /// <summary>
        /// Represents the encoder buffer size.
        /// </summary>
        public WaveBuffer EncoderBuffer { get; private set; }

        /// <summary>
        /// Represents the description of the codec.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Represents the recording format, which is a "wave-format".
        /// </summary>
        public WaveFormat RecordFormat
        {
            get { return RecordingFormat; }
        }

        /// <summary>
        /// Represents a friendly name for this codec.
        /// </summary>
        public string Name
        {
            get { return Description; }
        }

        /// <summary>
        /// Represents the bitrate.
        /// </summary>
        public int BitsPerSecond
        {
            get { return -1; }
        }

        /// <summary>
        /// Represents a <see cref="bool"/> value whether the codec is available on this system.
        /// </summary>
        public bool IsAvailable { get { return true; } }

        #endregion Fields

        #region Constructors & Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Speex"/> class.
        /// </summary>
        public Speex(BandMode bandMode, int sampleRate, string description)
        {
            Decoder = new SpeexDecoder(bandMode);
            Encoder = new SpeexEncoder(bandMode);
            RecordingFormat = new WaveFormat(sampleRate, 16, 1);
            Description = description;
            EncoderBuffer = new WaveBuffer(RecordingFormat.AverageBytesPerSecond);
        }

        /// <summary>
        /// Releases the unmanaged resources that the current instance of the class is using.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion Constructors & Deconstructors

        #region Methods

        /// <summary>
        /// Encodes a block of audio.
        /// </summary>
        public byte[] Encode(byte[] data, int offset, int length)
        {
            FeedBuffer(data, offset, length);

            int samplesToEncode = EncoderBuffer.ShortBufferCount;
            if (samplesToEncode % Encoder.FrameSize != 0)
            {
                samplesToEncode -= samplesToEncode % Encoder.FrameSize;
            }

            byte[] outputBufferTemp = new byte[length];
            int bytesWritten = Encoder.Encode(EncoderBuffer.ShortBuffer, 0, samplesToEncode, outputBufferTemp, 0, length);
            byte[] encoded = new byte[bytesWritten];

            Array.Copy(outputBufferTemp, 0, encoded, 0, bytesWritten);

            CheckBuffer(samplesToEncode);

            return encoded;
        }

        /// <summary>
        /// Decodes a block of audio.
        /// </summary>
        public byte[] Decode(byte[] data, int offset, int length)
        {
            byte[] outputBufferTemp = new byte[length * 320];

            WaveBuffer buffer = new WaveBuffer(outputBufferTemp);

            int samplesDecoded = Decoder.Decode(data, offset, length, buffer.ShortBuffer, 0, false);
            int bytesDecoded = samplesDecoded * 2;
            byte[] decoded = new byte[bytesDecoded];

            Array.Copy(outputBufferTemp, 0, decoded, 0, bytesDecoded);

            return decoded;
        }

        #endregion Methods

        #region Private Methods

        private void CheckBuffer(int samplesEncoded)
        {
            int leftoverSamples = EncoderBuffer.ShortBufferCount - samplesEncoded;
            Array.Copy(EncoderBuffer.ByteBuffer, samplesEncoded * 2, EncoderBuffer.ByteBuffer, 0, leftoverSamples * 2);
            EncoderBuffer.ShortBufferCount = leftoverSamples;
        }

        private void FeedBuffer(byte[] data, int offset, int length)
        {
            Array.Copy(data, offset, EncoderBuffer.ByteBuffer, EncoderBuffer.ByteBufferCount, length);
            EncoderBuffer.ByteBufferCount += length;
        }

        #endregion Private Methods
    }
}