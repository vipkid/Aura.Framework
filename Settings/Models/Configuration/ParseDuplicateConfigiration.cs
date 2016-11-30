namespace Aura.Framework.Settings.Models.Configuration
{
    /// <summary>
    /// Represents a duplicate parse configuration.
    /// </summary>
    public class ParseDuplicateConfigiration : ParseConfiguration
    {
        #region Fields

        /// <summary>
        /// Represents a <see cref="bool"/> value if it is allowed to have duplicated keys.
        /// </summary>
        public new bool AllowDuplicateKeys { get { return true; } }

        /// <summary>
        /// Gets or sets the string used to concatenate duplicated keys.
        /// </summary>
        public string ConcatenateSeparator { get; set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseDuplicateConfigiration"/> class.
        /// </summary>
        public ParseDuplicateConfigiration() : base()
        {
            ConcatenateSeparator = ";";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseDuplicateConfigiration"/> class.
        /// </summary>
        public ParseDuplicateConfigiration(ParseDuplicateConfigiration ori) : base(ori)
        {
            ConcatenateSeparator = ori.ConcatenateSeparator;
        }

        #endregion Constructors
    }
}