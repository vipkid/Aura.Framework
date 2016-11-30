using System;

namespace Aura.Framework.Settings.Models
{
    /// <summary>
    /// Represents all data from a config file exactly as the <see cref="Data"/> class, but searching for sections and keys names is done with a case insensitive search.
    /// </summary>
    public class DataCaseInsensitive : Data
    {
        #region Constructors

        /// <summary>
        /// Initializes an empty instance of the <see cref="DataCaseInsensitive"/> class.
        /// </summary>
        public DataCaseInsensitive() : base(new SectionDataCollection(StringComparer.OrdinalIgnoreCase))
        { }

        /// <summary>
        /// Initializes a new IniData instance using a previous <see cref="SectionDataCollection"/>.
        /// </summary>
        public DataCaseInsensitive(SectionDataCollection sdc) : base(new SectionDataCollection(sdc, StringComparer.OrdinalIgnoreCase))
        { }

        /// <summary>
        /// Copies an instance of the <see cref="Aura.Framework.Settings.Models.DataCaseInsensitive"/> class.
        /// </summary>
        public DataCaseInsensitive(Data ori) : this(new SectionDataCollection(ori.Sections, StringComparer.OrdinalIgnoreCase))
        {
            Global = (KeyDataCollection)ori.Global.Clone();
            Configuration = ori.Configuration.Clone();
        }

        #endregion Constructors
    }
}