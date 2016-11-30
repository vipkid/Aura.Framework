using Aura.Framework.Settings.Models;
using Aura.Framework.Settings.Models.Configuration;

namespace Aura.Framework.Settings.Parser
{
    /// <summary>
	/// Responsible for parsing a duplicate string from a config file, and creating an <see cref="Data"/> structure.
	/// </summary>
    public class DuplicateParser : SettingsDataParser
    {
        #region Fields

        public new ParseDuplicateConfigiration Configuration
        {
            get
            {
                return (ParseDuplicateConfigiration)base.Configuration;
            }
            set
            {
                base.Configuration = value;
            }
        }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateParser"/> class.
        /// </summary>
        public DuplicateParser() : this(new ParseDuplicateConfigiration())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateParser"/> class.
        /// </summary>
        public DuplicateParser(ParseDuplicateConfigiration parserConfiguration) : base(parserConfiguration)
        { }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Does what it says. Handles the duplicated key in a <see cref="KeyDataCollection"/>.
        /// </summary>
        protected override void HandleDuplicatedKeyInCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
        {
            keyDataCollection[key] += Configuration.ConcatenateSeparator + value;
        }

        #endregion Methods

    }
}