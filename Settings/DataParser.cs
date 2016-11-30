using Aura.Framework.Settings.Exceptions;
using Aura.Framework.Settings.Models;
using Aura.Framework.Settings.Parser;
using System;
using System.IO;
using System.Text;

namespace Aura.Framework.Settings
{
    /// <summary>
    /// Represents a config data parser for files.
    /// </summary>
    public class DataParser : DataParserStream, IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParser"/> class.
        /// </summary>
        public DataParser() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParser"/> class.
        /// </summary>
        public DataParser(SettingsDataParser parser) : base(parser)
        {
            Parser = parser;
        }

        #endregion Constructors

        #region Deprecated methods

        [Obsolete("Please use ReadFile method instead of this one as is more semantically accurate")]
        public Data LoadFile(string filePath)
        {
            return ReadFile(filePath);
        }

        [Obsolete("Please use ReadFile method instead of this one as is more semantically accurate")]
        public Data LoadFile(string filePath, Encoding fileEncoding)
        {
            return ReadFile(filePath, fileEncoding);
        }

        #endregion Deprecated methods

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Implements reading config data from a file.
        /// </summary>
        public Data ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.ASCII);
        }

        /// <summary>
        /// Implements reading config data from a file.
        /// </summary>
        public Data ReadFile(string filePath, Encoding fileEncoding)
        {
            if (filePath == string.Empty)
                throw new ArgumentException("Bad filename.");

            try
            {
                // (FileAccess.Read) we want to open the ini only for reading
                // (FileShare.ReadWrite) any other process should still have access to the ini file
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs, fileEncoding))
                    {
                        return ReadData(sr);
                    }
                }
            }
            catch (IOException ex)
            {
                throw new ParsingException(String.Format("Could not parse file {0}", filePath), ex);
            }
        }

        /// <summary>
        /// Saves config data to a file.
        /// </summary>
        [Obsolete("Please use WriteFile method instead of this one as is more semantically accurate")]
        public void SaveFile(string filePath, Data parsedData)
        {
            WriteFile(filePath, parsedData, Encoding.ASCII);
        }

        /// <summary>
        /// Writes config data to a text file.
        /// </summary>
        public void WriteFile(string filePath, Data parsedData, Encoding fileEncoding = null)
        {
            // The default value can't be assigned as a default parameter value because it is not
            // a constant expression.
            if (fileEncoding == null)
                fileEncoding = Encoding.ASCII;

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Bad filename.");

            if (parsedData == null)
                throw new ArgumentNullException("parsedData");

            using (FileStream stream = File.Open(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream, fileEncoding))
                {
                    WriteData(writer, parsedData);
                }
            }
        }

        #endregion Methods
    }
}