using Aura.Framework.Settings.Models;
using Aura.Framework.Settings.Models.Formatting;
using Aura.Framework.Settings.Parser;
using System;
using System.IO;

namespace Aura.Framework.Settings
{
    /// <summary>
    /// Represents a config data parser for streams.
    /// </summary>
    public class DataParserStream
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParserStream"/> class.
        /// </summary>
        public DataParserStream() : this(new SettingsDataParser()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParserStream"/> class.
        /// </summary>
        public DataParserStream(SettingsDataParser parser)
        {
            Parser = parser;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        /// This instance will handle config data parsing and writing
        /// </summary>
        public SettingsDataParser Parser { get; protected set; }

        #endregion Fields

        #region Public Methods

        /// <summary>
        /// Reads data in config data format from a stream.
        /// </summary>
        public Data ReadData(StreamReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            return Parser.Parse(reader.ReadToEnd());
        }

        /// <summary>
        /// Writes the config data to a stream.
        /// </summary>
        public void WriteData(StreamWriter writer, Data configData)
        {
            if (configData == null)
                throw new ArgumentNullException("configData");
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(configData.ToString());
        }

        /// <summary>
        /// Writes the config data to a stream.
        /// </summary>
        public void WriteData(StreamWriter writer, Data configData, DataFormatter formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException("formatter");
            if (configData == null)
                throw new ArgumentNullException("configData");
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(configData.ToString(formatter));
        }

        #endregion Public Methods
    }
}