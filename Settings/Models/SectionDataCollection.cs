using System;
using System.Collections;
using System.Collections.Generic;

namespace Aura.Framework.Settings.Models
{
    /// <summary>
    /// Represents a collection of SectionData.
    /// </summary>
    public class SectionDataCollection : ICloneable, IEnumerable<SectionData>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDataCollection"/> class.
        /// </summary>
        public SectionDataCollection() : this(EqualityComparer<string>.Default)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDataCollection"/> class.
        /// </summary>
        public SectionDataCollection(IEqualityComparer<string> searchComparer)
        {
            _SearchComparer = searchComparer;

            _SectionData = new Dictionary<string, SectionData>(_SearchComparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDataCollection"/> class from a previous instance of <see cref="SectionDataCollection"/>.
        /// </summary>
        public SectionDataCollection(SectionDataCollection ori, IEqualityComparer<string> searchComparer)
        {
            _SearchComparer = searchComparer ?? EqualityComparer<string>.Default;

            _SectionData = new Dictionary<string, SectionData>(_SearchComparer);
            foreach (var sectionData in ori)
            {
                _SectionData.Add(sectionData.SectionName, (SectionData)sectionData.Clone());
            };
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Returns the number of SectionData elements in the collection.
        /// </summary>
        public int Count { get { return _SectionData.Count; } }

        /// <summary>
        /// Gets the key data associated to a specified section name.
        /// </summary>
        public KeyDataCollection this[string sectionName]
        {
            get
            {
                if (_SectionData.ContainsKey(sectionName))
                    return _SectionData[sectionName].Keys;

                return null;
            }
        }

        #endregion Properties

        #region Public Members

        /// <summary>
        /// Creates a new section with empty data.
        /// </summary>
        public bool AddSection(string keyName)
        {
            if (!ContainsSection(keyName))
            {
                _SectionData.Add(keyName, new SectionData(keyName, _SearchComparer));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a new SectionData instance to the collection.
        /// </summary>
        public void Add(SectionData data)
        {
            if (ContainsSection(data.SectionName))
            {
                SetSectionData(data.SectionName, new SectionData(data, _SearchComparer));
            }
            else
            {
                _SectionData.Add(data.SectionName, new SectionData(data, _SearchComparer));
            }
        }

        /// <summary>
        /// Removes all entries from this collection.
        /// </summary>
        public void Clear()
        {
            _SectionData.Clear();
        }

        /// <summary>
        /// Gets if a section with a specified name exists in the collection.
        /// </summary>
        public bool ContainsSection(string keyName)
        {
            return _SectionData.ContainsKey(keyName);
        }

        /// <summary>
        /// Returns the section data from a specify section given its name.
        /// </summary>
        public SectionData GetSectionData(string sectionName)
        {
            if (_SectionData.ContainsKey(sectionName))
                return _SectionData[sectionName];

            return null;
        }

        /// <summary>
        /// Merges two instances of the <see cref="SectionDataCollection"/> class.
        /// </summary>
        public void Merge(SectionDataCollection sectionsToMerge)
        {
            foreach (var sectionDataToMerge in sectionsToMerge)
            {
                var sectionDataInThis = GetSectionData(sectionDataToMerge.SectionName);

                if (sectionDataInThis == null)
                {
                    AddSection(sectionDataToMerge.SectionName);
                }

                this[sectionDataToMerge.SectionName].Merge(sectionDataToMerge.Keys);
            }
        }

        /// <summary>
        /// Sets the section data for given a section name.
        /// </summary>
        public void SetSectionData(string sectionName, SectionData data)
        {
            if (data != null)
                _SectionData[sectionName] = data;
        }

        /// <summary>
        /// Removes a section from the collection.
        /// </summary>
        public bool RemoveSection(string keyName)
        {
            return _SectionData.Remove(keyName);
        }

        #endregion Public Members

        #region IEnumerable<SectionData> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<SectionData> GetEnumerator()
        {
            foreach (string sectionName in _SectionData.Keys)
                yield return _SectionData[sectionName];
        }

        #endregion IEnumerable<SectionData> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable Members

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return new SectionDataCollection(this, _SearchComparer);
        }

        #endregion ICloneable Members

        #region Non-public Members

        private IEqualityComparer<string> _SearchComparer;

        /// <summary>
        /// Data associated to this section.
        /// </summary>
        private readonly Dictionary<string, SectionData> _SectionData;

        #endregion Non-public Members
    }
}