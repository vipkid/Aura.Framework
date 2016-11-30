using System;
using System.Collections.Generic;

namespace Aura.Framework.Settings.Models
{
    /// <summary>
    /// Information associated to a section in a config file that includes both the value and the comments associated to the key.
    /// </summary>
    public class SectionData : ICloneable
    {
        #region Initialization

        public SectionData(string sectionName)
            : this(sectionName, EqualityComparer<string>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionData"/> class.
        /// </summary>
        public SectionData(string sectionName, IEqualityComparer<string> searchComparer)
        {
            _SearchComparer = searchComparer;

            if (string.IsNullOrEmpty(sectionName))
                throw new ArgumentException("section name can not be empty");

            _LeadingComments = new List<string>();
            _KeyDataCollection = new KeyDataCollection(_SearchComparer);
            SectionName = sectionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionData"/> class
        /// from a previous instance of <see cref="SectionData"/>.
        /// </summary>
        /// <remarks>
        /// Data is deeply copied
        /// </remarks>
        /// <param name="ori">
        /// The instance of the <see cref="SectionData"/> class
        /// used to create the new instance.
        /// </param>
        /// <param name="searchComparer">
        /// Search comparer.
        /// </param>
        public SectionData(SectionData ori, IEqualityComparer<string> searchComparer = null)
        {
            SectionName = ori.SectionName;

            _SearchComparer = searchComparer;
            _LeadingComments = new List<string>(ori._LeadingComments);
            _KeyDataCollection = new KeyDataCollection(ori._KeyDataCollection, searchComparer ?? ori._SearchComparer);
        }

        #endregion Initialization

        #region Operations

        /// <summary>
        /// Deletes all comments in this section and key/value pairs.
        /// </summary>
        public void ClearComments()
        {
            LeadingComments.Clear();
            TrailingComments.Clear();
            Keys.ClearComments();
        }

        /// <summary>
        /// Deletes all the key-value pairs in this section.
        /// </summary>
		public void ClearKeyData()
        {
            Keys.RemoveAllKeys();
        }

        /// <summary>
        /// Merges otherSection into this, adding new keys if they don't exists or overwriting values if the key already exists. Comments get appended.
        /// </summary>
        public void Merge(SectionData toMergeSection)
        {
            foreach (var comment in toMergeSection.LeadingComments)
                LeadingComments.Add(comment);

            Keys.Merge(toMergeSection.Keys);

            foreach (var comment in toMergeSection.TrailingComments)
                TrailingComments.Add(comment);
        }

        #endregion Operations

        #region Properties

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        public string SectionName
        {
            get
            {
                return _SectionName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _SectionName = value;
            }
        }

        /// <summary>
        /// Gets or sets the comment list associated to this section.
        /// </summary>
        public List<string> LeadingComments
        {
            get
            {
                return _LeadingComments;
            }

            internal set
            {
                _LeadingComments = new List<string>(value);
            }
        }

        /// <summary>
        /// Gets or sets the comment list associated to this section.
        /// </summary>
        public List<string> Comments
        {
            get
            {
                var list = new List<string>(_LeadingComments);
                list.AddRange(_TrailingComments);
                return list;
            }
        }

        /// <summary>
        /// Gets or sets the comment list associated to this section.
        /// </summary>
        public List<string> TrailingComments
        {
            get
            {
                return _TrailingComments;
            }

            internal set
            {
                _TrailingComments = new List<string>(value);
            }
        }

        /// <summary>
        /// Gets or sets the keys associated to this section.
        /// </summary>
        public KeyDataCollection Keys
        {
            get
            {
                return _KeyDataCollection;
            }

            set
            {
                _KeyDataCollection = value;
            }
        }

        #endregion Properties

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return new SectionData(this);
        }

        #endregion ICloneable Members

        #region Non-public members

        private List<string> _LeadingComments;
        private List<string> _TrailingComments = new List<string>();
        private KeyDataCollection _KeyDataCollection;
        private IEqualityComparer<string> _SearchComparer;
        private string _SectionName;

        #endregion Non-public members
    }
}