namespace Aura.Framework.Configurations.Models
{
    /// <summary>
    /// Enumerates the elements of a Setting that represents an array.
    /// </summary>
    internal struct SettingArrayEnumerator
    {
        #region Fields

        private string _RawValue;
        private string _CurrentElement;
        private int _Index;
        private int _ArrayEndIdx;
        private int _LastElementId;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingArrayEnumerator"/> class.
        /// </summary>
        public SettingArrayEnumerator(string rawValue)
        {
            _RawValue = rawValue;
            _CurrentElement = null;
            _Index = -1;

            _LastElementId = rawValue.IndexOf('{') + 1;
            _ArrayEndIdx = rawValue.LastIndexOf('}');
        }

        #endregion Constructors

        #region Methods

        public bool Next()
        {
            if (_LastElementId > _ArrayEndIdx)
            {
                return false;
            }

            int subStrBegin;
            int subStrEnd;

            int nextElementIdx = _RawValue.IndexOf(Configuration.ArrayElementSeparator, _LastElementId + 1);
            if (nextElementIdx < 0)
            {
                // Last element.
                subStrBegin = _LastElementId;
                subStrEnd = _ArrayEndIdx;
            }
            else
            {
                // An element has been found.
                subStrBegin = _LastElementId;
                subStrEnd = nextElementIdx;
            }

            _CurrentElement = _RawValue.Substring(subStrBegin, subStrEnd - subStrBegin);
            _CurrentElement = _CurrentElement.Trim();
            _LastElementId = subStrEnd + 1;
            ++_Index;

            return true;
        }

        public string Current
        {
            get { return _CurrentElement; }
        }

        public int Index
        {
            get { return _Index; }
        }

        #endregion Methods
    }
}