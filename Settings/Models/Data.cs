using Aura.Framework.Settings.Models.Configuration;
using Aura.Framework.Settings.Models.Formatting;
using System;

namespace Aura.Framework.Settings.Models
{
    /// <summary>
    /// Represents all data from a config file.
    /// </summary>
    public class Data : ICloneable
    {
        #region Non-Public Members

        /// <summary>
        /// Represents all sections from a config file.
        /// </summary>
        private SectionDataCollection _Sections;

        /// <summary>
        /// See property <see cref="Configuration"/> for more information.
        /// </summary>
        private ParseConfiguration _Configuration;

        #endregion Non-Public Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        public Data() : this(new SectionDataCollection()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class using a previous <see cref="SectionDataCollection"/>.
        /// </summary>
        public Data(SectionDataCollection sdc)
        {
            _Sections = (SectionDataCollection)sdc.Clone();
            Global = new KeyDataCollection();
            SectionKeySeparator = '.';
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        public Data(Data ori) : this((SectionDataCollection)ori.Sections)
        {
            Global = (KeyDataCollection)ori.Global.Clone();
            Configuration = ori.Configuration.Clone();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Configuration used to write a config file with the proper delimiter characters and data.
        /// </summary>
        public ParseConfiguration Configuration
        {
            get
            {
                // Lazy initialization
                if (_Configuration == null)
                    _Configuration = new ParseConfiguration();

                return _Configuration;
            }

            set { _Configuration = value.Clone(); }
        }

        /// <summary>
        /// Global sections. Contains key/value pairs which are not enclosed in any section (i.e. they are defined at the beginning of the file, before any section.
        /// </summary>
        public KeyDataCollection Global { get; protected set; }

        /// <summary>
        /// Gets the <see cref="KeyDataCollection"/> instance with the specified section name.
        /// </summary>
        public KeyDataCollection this[string sectionName]
        {
            get
            {
                if (_Sections.ContainsSection(sectionName)) return _Sections[sectionName];

                return null;
            }
        }

        /// <summary>
        /// Gets or sets all the <see cref="SectionData"/> for this IniData instance.
        /// </summary>
        public SectionDataCollection Sections
        {
            get { return _Sections; }
            set { _Sections = value; }
        }

        /// <summary>
        /// Used to mark the separation between the section name and the key name when using <see cref="TryGetKey"/> method.
        /// </summary>
        public char SectionKeySeparator { get; set; }

        #endregion Properties

        #region Override Methods

        public override string ToString()
        {
            return ToString(new DataDefaultFormatter(Configuration));
        }

        public virtual string ToString(DataFormatter formatter)
        {
            return formatter.IniDataToString(this);
        }

        #endregion Override Methods

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance. Return a new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new Data(this);
        }

        #endregion ICloneable Members

        #region Public Methods

        /// <summary>
        /// Deletes all comments in all sections and key values.
        /// </summary>
        public void ClearAllComments()
        {
            Global.ClearComments();

            foreach (var section in Sections)
            {
                section.ClearComments();
            }
        }

        /// <summary>
        /// Merges the other config into this one by overwriting existing values. The comments get appended.
        /// </summary>
        public void Merge(Data toMergeData)
        {
            if (toMergeData == null) return;

            Global.Merge(toMergeData.Global);

            Sections.Merge(toMergeData.Sections);
        }

        /// <summary>
        /// Attempts to retrieve a key, using a single string combining section and key name.
        /// </summary>
        public bool TryGetKey(string key, out string value)
        {
            value = string.Empty;
            if (string.IsNullOrEmpty(key))
                return false;

            var splitKey = key.Split(SectionKeySeparator);
            var separatorCount = splitKey.Length - 1;
            if (separatorCount > 1)
                throw new ArgumentException("key contains multiple separators", "key");

            if (separatorCount == 0)
            {
                if (!Global.ContainsKey(key))
                    return false;

                value = Global[key];
                return true;
            }

            var section = splitKey[0];
            key = splitKey[1];

            if (!_Sections.ContainsSection(section))
                return false;
            var sectionData = _Sections[section];
            if (!sectionData.ContainsKey(key))
                return false;

            value = sectionData[key];
            return true;
        }

        /// <summary>
        /// Retrieves a key using a single input string combining section and key name.
        /// </summary>
        public string GetKey(string key)
        {
            string result;
            return TryGetKey(key, out result) ? result : null;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Merge the sections into this by overwriting this sections.
        /// </summary>
        private void MergeSection(SectionData otherSection)
        {
            // no overlap -> create no section
            if (!Sections.ContainsSection(otherSection.SectionName))
            {
                Sections.AddSection(otherSection.SectionName);
            }

            // merge section into the new one
            Sections.GetSectionData(otherSection.SectionName).Merge(otherSection);
        }

        /// <summary>
        /// Merges the given global values into this globals by overwriting existing values.
        /// </summary>
        private void MergeGlobal(KeyDataCollection globals)
        {
            foreach (var globalValue in globals)
            {
                Global[globalValue.KeyName] = globalValue.Value;
            }
        }

        #endregion Private Methods
    }
}