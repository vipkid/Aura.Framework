using Aura.Framework.Connectivity.Shared.Messages;

namespace Aura.Framework.Connectivity.Client.Models
{
    /// <summary>
    /// Represents a data package that provides information linked with a certain key.
    /// </summary>
    public class DataPackage
    {
        #region Fields

        /// <summary>
        /// Represents the key value of the current instance of the <see cref="DataPackage"/> class.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Represents the value of the current instance of the <see cref="DataPackage"/> class.
        /// </summary>
        public string Value { get; private set; }

        #endregion Fields

        #region Constuctors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPackage"/> class.
        /// </summary>
        public DataPackage(string key, string value)
        {
            Key = key;
            Value = value;
        }

        #endregion Constuctors

        #region Methods

        /// <summary>
        /// Writes the datapackage to the stream.
        /// </summary>
        public void Write(ServerMessage message)
        {
            message.WriteString(Key);
            message.WriteString(Value);
        }

        #endregion Methods
    }
}