using System.IO;

namespace Aura.Framework.Configurations.Overrides
{
    /// <summary>
    /// Represents a <see cref="BinaryWriter"/> with an overrided empty dispose method.
    /// </summary>
    public class NonClosingBinaryWriter : BinaryWriter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NonClosingBinaryWriter"/> class.
        /// </summary>
        public NonClosingBinaryWriter(Stream stream) : base(stream)
        { }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        { }

        #endregion Methods
    }
}